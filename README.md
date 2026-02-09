# Opening the Project
The following sample project can be opened with or without FacotoryTalk Optix Studio Pro
## With FactoryTalk Optix Studio Pro
1. Select the green **Code** dropdown
2. Copy the repository URL to the clipboard
3. Open FactoryTalk Optix Studio and sign in to obtain the FactoryTalk Optix Studio Pro Entitlement
4. Select **Open** > **Remote**, paste the repository URL in the **Remote URL** field, set the **Location**, and Select **Open**
## Without FactoryTalk Optix Studio Pro
1. Select the green **Code** dropdown
2. Select **Download ZIP**
3. Unzip the downloaded Repository to the desired location
4. Open the **.optix** file

# Introduction
The following sample project demonstrates how add a unique identifier to the lifecycle of every alarm (Active > Acknowledged > Confirmed > Inactive). The unique identifier can be used to group alarm records and provide a single row record of the alarm lifecycle.

# Project Components
## UI / MainWindow
The runtime MainWindow provides DigitalAlarmWithGUID1-3 and DigitalAlarm1 toggle switches to make their associated alarms active. DigitalAlarm1 is provided to see the alarm response without a GUID. There are three DataGrids on the MainWindow to visualize the data:
- AlarmsDataGrid: Alarm Summary (live alarms in the Retained Alarms)
- DataGrid1: Raw AlarmsEventLogger1 Records in EmbeddedDatabase1
- DataGrid2: Example query to use the GUID to group raw Alarm data into single row
## Alarms / DigitalAlarmWithGUID (type)
DigitalAlarmWithGUID is a FactoryTalk Optix Alarm based on the Digial Alarm supertype. A GUID (string) property has been added to hold the GUID at runtime and update everytime an alarm transitions to inactive. 
DigitalAlarmWithGUID has a Custom Behavior to automatically initialize the GUID at the start of the Runtime, and holds an InitGUID() method that is exposed publicly if needed.
## Loggers / AlarmsEventLogger1
AlarmsEventLogger1 is a standard AlarmsEventLogger from the FactoryTalk Optix Library, but it has been modified with an additional column to log the GUID property of the DigitalAlarmWithGUID alarms.
## NetLogic / AlarmEventObserverLogic
AlarmEventObserverLogic registers and Event Observer of type UAManagedCore.OpcUa.ObjectTypes.AlarmConditionType at the start of the runtime. On every AlarmConditionType Event, it checks for:
- ActiveState/Id = False
- AckedState/Id = True
- ConfirmedState/Id = True

... indicating the end of that alarm's lifecycle. When this occurs, if the alarm is of type DigitalAlarmWithGUID, the GUID is updated to a new value.
