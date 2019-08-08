"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BatchServicesComponent_1 = require("./Components/BatchService/BatchServicesComponent");
var EditBatchServiceComponent_1 = require("./Components/BatchService/EditBatchServiceComponent");
var TaskSchedulerComponent_1 = require("./Components/TaskScheduler/TaskSchedulerComponent");
var AddEditTaskSchedulerComponent_1 = require("./Components/TaskScheduler/AddEditTaskSchedulerComponent");
var MainSchedulerComponent_1 = require("./Components/TaskScheduler/MainSchedulerComponent");
var SchedulerDateListTemplate_1 = require("./Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate");
var SchedulerDurationListTemplate_1 = require("./Components/TaskScheduler/ListTemplates/SchedulerDurationListTemplate");
var FTBSchedulerTemplateComponent_1 = require("./Components/TaskScheduler/SchedulerTemplates/FTBSchedulerTemplateComponent");
var TaskSchedulerTemplateComponent_1 = require("./Components/TaskScheduler/SchedulerTemplates/TaskSchedulerTemplateComponent");
exports.Components = [
    BatchServicesComponent_1.BatchServicesComponent,
    TaskSchedulerComponent_1.TaskSchedulerComponent,
    AddEditTaskSchedulerComponent_1.AddEditTaskSchedulerComponent,
    EditBatchServiceComponent_1.EditBatchServiceComponent,
    MainSchedulerComponent_1.MainSchedulerComponent,
    SchedulerDateListTemplate_1.SchedulerDateListTemplate,
    SchedulerDurationListTemplate_1.SchedulerDurationListTemplate,
    FTBSchedulerTemplateComponent_1.FTBSchedulerTemplateComponent,
    TaskSchedulerTemplateComponent_1.TaskSchedulerTemplateComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "BatchServicesComponent": {
                myResult = BatchServicesComponent_1.BatchServicesComponent;
                break;
            }
            case "TaskSchedulerComponent": {
                myResult = TaskSchedulerComponent_1.TaskSchedulerComponent;
                break;
            }
            case "AddEditTaskSchedulerComponent": {
                myResult = AddEditTaskSchedulerComponent_1.AddEditTaskSchedulerComponent;
                break;
            }
            case "EditBatchServiceComponent": {
                myResult = EditBatchServiceComponent_1.EditBatchServiceComponent;
                break;
            }
            case "MainSchedulerComponent": {
                myResult = MainSchedulerComponent_1.MainSchedulerComponent;
                break;
            }
            case "SchedulerDateListTemplate": {
                myResult = SchedulerDateListTemplate_1.SchedulerDateListTemplate;
                break;
            }
            case "SchedulerDurationListTemplate": {
                myResult = SchedulerDurationListTemplate_1.SchedulerDurationListTemplate;
                break;
            }
            case "FTBSchedulerTemplateComponent": {
                myResult = FTBSchedulerTemplateComponent_1.FTBSchedulerTemplateComponent;
                break;
            }
            case "TaskSchedulerTemplateComponent": {
                myResult = TaskSchedulerTemplateComponent_1.TaskSchedulerTemplateComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map