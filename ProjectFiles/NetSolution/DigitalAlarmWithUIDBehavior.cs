#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.HMIProject;
using FTOptix.EventLogger;
using FTOptix.NetLogic;
using FTOptix.NativeUI;
using FTOptix.Alarm;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.SerialPort;
using FTOptix.Core;
using FTOptix.OPCUAServer;
#endregion

[CustomBehavior]
public class DigitalAlarmWithUIDBehavior : BaseNetBehavior
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined behavior is started

        InitGUID();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined behavior is stopped
    }

    [ExportMethod]
    public void InitGUID()
    {
        Node.GUID = Guid.NewGuid().ToString();
    }

#region Auto-generated code, do not edit!
    protected new DigitalAlarmWithUID Node => (DigitalAlarmWithUID)base.Node;
#endregion
}
