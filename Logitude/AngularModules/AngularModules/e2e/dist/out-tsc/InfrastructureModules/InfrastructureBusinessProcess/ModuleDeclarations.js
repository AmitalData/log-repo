"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BusinessRoleNewComponent_1 = require("./Components/BusinessRoleNewComponent");
var QueueNewComponent_1 = require("./Components/BusinessProcessQueue/QueueNewComponent");
var TeamNewComponent_1 = require("./Components/Team/TeamNewComponent");
var TeamGeneralTabComponent_1 = require("./Components/Team/TeamGeneralTabComponent");
var TasksWorkspaceComponent_1 = require("./Components/Workspaces/TasksWorkspaceComponent");
exports.Components = [
    BusinessRoleNewComponent_1.BusinessRoleNewComponent,
    QueueNewComponent_1.QueueNewComponent,
    TeamNewComponent_1.TeamNewComponent,
    TeamGeneralTabComponent_1.TeamGeneralTabComponent,
    TasksWorkspaceComponent_1.TasksWorkspaceComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "BusinessRoleNewComponent": {
                myResult = BusinessRoleNewComponent_1.BusinessRoleNewComponent;
                break;
            }
            case "QueueNewComponent": {
                myResult = QueueNewComponent_1.QueueNewComponent;
                break;
            }
            case "TeamNewComponent": {
                myResult = TeamNewComponent_1.TeamNewComponent;
                break;
            }
            case "TeamGeneralTabComponent": {
                myResult = TeamGeneralTabComponent_1.TeamGeneralTabComponent;
                break;
            }
            case "TasksWorkspaceComponent": {
                myResult = TasksWorkspaceComponent_1.TasksWorkspaceComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map