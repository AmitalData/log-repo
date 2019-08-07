"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CommunicationLogMessageBodyComponent_1 = require("./Components/CommunicationLog/CommunicationLogMessageBodyComponent");
var CommunicationLogErrorComponent_1 = require("./Components/CommunicationLog/CommunicationLogErrorComponent");
var CommunicationStepsComponent_1 = require("./Components/Communications/CommunicationStepsComponent");
var CommunicationLogMoreDetailsComponent_1 = require("./Components/Communications/CommunicationLogMoreDetailsComponent");
var CommunicationMoreComponent_1 = require("./Components/Communications/CommunicationMoreComponent");
var LogFieldComponent_1 = require("./Components/Communications/LogFieldComponent");
var APILogsErrorsComponent_1 = require("./Components/APILogs/APILogsErrorsComponent");
var APILogsDiagnosticComponent_1 = require("./Components/APILogs/APILogsDiagnosticComponent");
var APILogsResponceBodyComponent_1 = require("./Components/APILogs/APILogsResponceBodyComponent");
var APILogsRequestBodyComponent_1 = require("./Components/APILogs/APILogsRequestBodyComponent");
var CommunicationsTabComponent_1 = require("./Components/Communications/CommunicationsTabComponent");
var MessageBodyTabComponent_1 = require("./Components/AnalyzeQueue/MessageBodyTabComponent");
var AnalyzeQueueErrorsTabComponent_1 = require("./Components/AnalyzeQueue/AnalyzeQueueErrorsTabComponent");
exports.Components = [
    CommunicationLogMessageBodyComponent_1.CommunicationLogMessageBodyComponent,
    CommunicationLogErrorComponent_1.CommunicationLogErrorComponent,
    CommunicationLogMoreDetailsComponent_1.CommunicationLogMoreDetailsComponent,
    CommunicationStepsComponent_1.CommunicationStepsComponent,
    CommunicationMoreComponent_1.CommunicationMoreComponent,
    LogFieldComponent_1.LogFieldComponent,
    APILogsDiagnosticComponent_1.APILogsDiagnosticComponent,
    APILogsErrorsComponent_1.APILogsErrorsComponent,
    APILogsRequestBodyComponent_1.APILogsRequestBodyComponent,
    APILogsResponceBodyComponent_1.APILogsResponceBodyComponent,
    CommunicationsTabComponent_1.CommunicationsTabComponent,
    MessageBodyTabComponent_1.MessageBodyTabComponent,
    AnalyzeQueueErrorsTabComponent_1.AnalyzeQueueErrorsTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CommunicationLogMessageBodyComponent": {
                myResult = CommunicationLogMessageBodyComponent_1.CommunicationLogMessageBodyComponent;
                break;
            }
            case "CommunicationLogErrorComponent": {
                myResult = CommunicationLogErrorComponent_1.CommunicationLogErrorComponent;
                break;
            }
            case "CommunicationLogMoreDetailsComponent": {
                myResult = CommunicationLogMoreDetailsComponent_1.CommunicationLogMoreDetailsComponent;
                break;
            }
            case "CommunicationStepsComponent": {
                myResult = CommunicationStepsComponent_1.CommunicationStepsComponent;
                break;
            }
            case "CommunicationMoreComponent": {
                myResult = CommunicationMoreComponent_1.CommunicationMoreComponent;
                break;
            }
            case "LogFieldComponent": {
                myResult = LogFieldComponent_1.LogFieldComponent;
                break;
            }
            case "APILogsErrorsComponent": {
                myResult = APILogsErrorsComponent_1.APILogsErrorsComponent;
                break;
            }
            case "APILogsDiagnosticComponent": {
                myResult = APILogsDiagnosticComponent_1.APILogsDiagnosticComponent;
                break;
            }
            case "APILogsRequestBodyComponent": {
                myResult = APILogsRequestBodyComponent_1.APILogsRequestBodyComponent;
                break;
            }
            case "APILogsResponceBodyComponent": {
                myResult = APILogsResponceBodyComponent_1.APILogsResponceBodyComponent;
                break;
            }
            case "CommunicationsTabComponent": {
                myResult = CommunicationsTabComponent_1.CommunicationsTabComponent;
                break;
            }
            case "AnalyzeQueueErrorsTabComponent": {
                myResult = AnalyzeQueueErrorsTabComponent_1.AnalyzeQueueErrorsTabComponent;
                break;
            }
            case "MessageBodyTabComponent": {
                myResult = MessageBodyTabComponent_1.MessageBodyTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map