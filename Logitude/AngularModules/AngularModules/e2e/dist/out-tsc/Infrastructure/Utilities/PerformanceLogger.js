"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Guid_1 = require("./Guid");
var SessionInfo_1 = require("./SessionInfo");
var PerformanceLog_1 = require("../Others/PerformanceLog");
var Dictionary_1 = require("../GenericTypes/Dictionary");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var PerformanceLogger = /** @class */ (function () {
    function PerformanceLogger() {
    }
    PerformanceLogger.AddLogTime = function () {
        var key = Guid_1.Guid.newGuid();
        PerformanceLogger.logTimes.Add(key, new Date());
        return key;
    };
    PerformanceLogger.GetLogTime = function (key) {
        return PerformanceLogger.logTimes.Item(key);
    };
    PerformanceLogger.RemoveLogTime = function (key) {
        if (PerformanceLogger.logTimes.ContainsKey(key)) {
            PerformanceLogger.logTimes.Remove(key);
        }
    };
    PerformanceLogger.InsertPerformanceLog = function (callTime, completionTime, serverTime, modelName, methodName, methodParams) {
        try {
            if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
                //if (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
                if (ObjectsLocator_1.ObjectsLocator != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
                    return;
                }
            }
            // var logsFileName: string = "PerformanceLog" + PerformanceLogger.Counter + ".txt";
            var cc = (completionTime.getMilliseconds() - callTime.getMilliseconds());
            // var executionTime: number = <number>((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
            var executionTime = ((completionTime.getTime() - callTime.getTime())); // TimeSpan.TicksPerMillisecond);
            // ticks = ((yourDateObject.getTime() * 10000) + 621355968000000000);
            if (executionTime >= 60000 || executionTime < 10) {
                return;
            }
            var performanceLog = new PerformanceLog_1.PerformanceLog();
            performanceLog.Id = Guid_1.Guid.newGuid();
            performanceLog.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            performanceLog.LogDateTimeLocal = new Date();
            performanceLog.Email = SessionInfo_1.SessionInfo.LoggedUserEmail;
            performanceLog.ModelName = modelName;
            performanceLog.MethodName = methodName;
            performanceLog.MethodParameters = methodParams;
            performanceLog.MonitoringService = false;
            performanceLog.ExecutionTime = executionTime;
            performanceLog.ServerTime = serverTime;
            window.sessionStorage.setItem(["PerformanceLogs", performanceLog.Id], JSON.stringify(performanceLog));
        }
        catch (e) {
            console.error(e);
        }
    };
    PerformanceLogger.logTimes = new Dictionary_1.Dictionary();
    return PerformanceLogger;
}());
exports.PerformanceLogger = PerformanceLogger;
//# sourceMappingURL=PerformanceLogger.js.map