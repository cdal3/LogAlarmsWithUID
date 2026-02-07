#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.Alarm;
using FTOptix.SerialPort;
using FTOptix.EventLogger;
using FTOptix.Store;
using FTOptix.SQLiteStore;
using FTOptix.OPCUAServer;
#endregion

public class EventLogic : BaseNetLogic, IUAEventObserver
{
    public override void Start()
    {
        var serverObject = LogicObject.Context.GetObject(OpcUa.Objects.Server);
        // Register the observer to the server node to listen for events of type AlarmConditionType
        eventRegistration = serverObject.RegisterUAEventObserver(this, UAManagedCore.OpcUa.ObjectTypes.AlarmConditionType);
    }

    public override void Stop()
    {
        // Insert code to be run when the user-defined logic is stopped
    }

    public void OnEvent(IUAObject eventNotifier, IUAObjectType eventType, IReadOnlyList<object> eventData, ulong senderId)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"Event of type {eventType.BrowseName} triggered");

        var eventArguments = eventType.EventArguments;
        // Listen for AlarmConditionType events and check when they are deactivated (ActiveState = False, AckedState = True and ConfirmedState = True) to udpate the alarm GUID
        if (eventArguments.GetFieldValue(eventData, "ActiveState/Id")?.ToString() == "False" &&
            eventArguments.GetFieldValue(eventData, "AckedState/Id")?.ToString() == "True" &&
            eventArguments.GetFieldValue(eventData, "ConfirmedState/Id")?.ToString() == "True")
        {
            Log.Info($"EventLogic {eventArguments.GetFieldValue(eventData, "ConditionId")?.ToString()} Alarm Removed from Summary");
            var alarm = InformationModel.Get<DigitalAlarmWithUID>((NodeId) eventArguments.GetFieldValue(eventData, "ConditionId"));
            var alarmGUID = alarm.GetVariable("GUID");
            alarmGUID.Value = Guid.NewGuid().ToString();
            Log.Info($"{alarm.BrowseName} new GUID: {alarmGUID.Value}");
        }
    }

    private IEventRegistration eventRegistration;
}
