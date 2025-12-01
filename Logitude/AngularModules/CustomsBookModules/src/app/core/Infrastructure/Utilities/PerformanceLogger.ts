import { Guid } from './Guid';
import { SessionInfo } from './SessionInfo';
import { SessionLocator } from './SessionLocator';
import { PerformanceLog } from '../Others/PerformanceLog';
import { Dictionary } from '../GenericTypes/Dictionary';
import { DateTool } from '../Tools';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';

declare var window: any;
export class PerformanceLogger {

    public static LoginStartTimeKey: string;

    static logTimes: Dictionary<Date> = new Dictionary<Date>();
    public static AddLogTime(): string {
        var key: string = Guid.newGuid();
        PerformanceLogger.logTimes.Add(key, new Date());
        return key;
    }
    public static GetLogTime(key: string): Date {

        return PerformanceLogger.logTimes.Item(key);
    }
    public static RemoveLogTime(key: string): void {
        if (PerformanceLogger.logTimes.ContainsKey(key)) {
            PerformanceLogger.logTimes.Remove(key);
        }
    }

    public static InsertPerformanceLog(callTime: Date, completionTime: Date, serverTime: number, modelName: string, methodName: string, methodParams: string): void {
        try {
            if (ObjectsLocator.GlobalSetting) {
                //if (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
                if (ObjectsLocator != null && ObjectsLocator.GlobalSetting != null && ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
                    return;
                }
            }
            // var logsFileName: string = "PerformanceLog" + PerformanceLogger.Counter + ".txt";
            var cc: number = <number>(completionTime.getMilliseconds() - callTime.getMilliseconds());
            // var executionTime: number = <number>((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);

            var executionTime: number = <number>((completionTime.getTime() - callTime.getTime())); // TimeSpan.TicksPerMillisecond);
            // ticks = ((yourDateObject.getTime() * 10000) + 621355968000000000);
            if (executionTime >= 60000 || executionTime < 10) {
                return
            }


            var performanceLog: PerformanceLog = new PerformanceLog();
            performanceLog.Id = Guid.newGuid();
            performanceLog.Tenant = SessionInfo.LoggedUserTenant;
            performanceLog.LogDateTimeLocal = new Date();
            performanceLog.Email = SessionInfo.LoggedUserEmail;
            performanceLog.ModelName = modelName;
            performanceLog.MethodName = methodName;
            performanceLog.MethodParameters = methodParams;
            performanceLog.MonitoringService = false;
            performanceLog.ExecutionTime = executionTime;
            performanceLog.ServerTime = serverTime;

            window.sessionStorage.setItem(["PerformanceLogs", performanceLog.Id], JSON.stringify(performanceLog));


        } catch (e) { console.error(e); }

    }
}