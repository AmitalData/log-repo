Examples of Logger usage:            
            
            
            NetCommonHelper.Logger.DevLog.Instance.SetProcessName("ConsoleTester", true); //Set process name and is webApp
                                  

            NetCommonHelper.Logger.DevLog.Instance.WriteTrace("Hello World!");//Log trace     0
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Hello World!");//Log debug     1
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("Hello World!");//Log info       2
            NetCommonHelper.Logger.DevLog.Instance.WriteWarning("Hello World!");//Log warning 3
            NetCommonHelper.Logger.DevLog.Instance.WriteError("Hello World!"); //Log error    4
            NetCommonHelper.Logger.DevLog.Instance.WriteFatal( new Exception("test crash"),"Hello World!"); //Log exception 5
                                   

            NetCommonHelper.Logger.DevLog.Instance.WriteToEventLog(System.Diagnostics.EventLogEntryType.Error, "test"); //Log to event log
                                   

            NetCommonHelper.Logger.DevLog.Instance.AddLogTarget("ConsoleTester*");// Enable Life log
            NetCommonHelper.Logger.DevLog.Instance.RemoveLogTarget(); // Disable Life log
