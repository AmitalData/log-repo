"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewAppointmentComponent_1 = require("./Components/NewEntity/NewAppointmentComponent");
var NewActivityComponent_1 = require("./Components/NewEntity/NewActivityComponent");
var ActivityInputTemplate_1 = require("./Components/NewEntity/ActivityInputTemplate");
var AddEditInviteesComponent_1 = require("./Components/NewEntity/AddEditInviteesComponent");
var InviteeCheckBoxComponent_1 = require("./Components/NewEntity/InviteeCheckBoxComponent");
var NewCallComponent_1 = require("./Components/NewEntity/NewCallComponent");
var NewTaskComponent_1 = require("./Components/NewEntity/NewTaskComponent");
var ActivityGeneralTabComponent_1 = require("./Components/EditTabs/ActivityGeneralTabComponent");
var AddEditActivityNotesComponent_1 = require("./Components/EditTabs/AddEditActivityNotesComponent");
exports.Components = [
    NewAppointmentComponent_1.NewAppointmentComponent,
    NewActivityComponent_1.NewActivityComponent,
    ActivityInputTemplate_1.ActivityInputTemplate,
    AddEditInviteesComponent_1.AddEditInviteesComponent,
    InviteeCheckBoxComponent_1.InviteeCheckBoxComponent,
    NewCallComponent_1.NewCallComponent,
    NewTaskComponent_1.NewTaskComponent,
    ActivityGeneralTabComponent_1.ActivityGeneralTabComponent,
    AddEditActivityNotesComponent_1.AddEditActivityNotesComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewAppointmentComponent": {
                myResult = NewAppointmentComponent_1.NewAppointmentComponent;
                break;
            }
            case "NewActivityComponent": {
                myResult = NewActivityComponent_1.NewActivityComponent;
                break;
            }
            case "ActivityInputTemplate": {
                myResult = ActivityInputTemplate_1.ActivityInputTemplate;
                break;
            }
            case "AddEditInviteesComponent": {
                myResult = AddEditInviteesComponent_1.AddEditInviteesComponent;
                break;
            }
            case "InviteeCheckBoxComponent": {
                myResult = InviteeCheckBoxComponent_1.InviteeCheckBoxComponent;
                break;
            }
            case "NewCallComponent": {
                myResult = NewCallComponent_1.NewCallComponent;
                break;
            }
            case "NewTaskComponent": {
                myResult = NewTaskComponent_1.NewTaskComponent;
                break;
            }
            case "ActivityGeneralTabComponent": {
                myResult = ActivityGeneralTabComponent_1.ActivityGeneralTabComponent;
                break;
            }
            case "AddEditActivityNotesComponent": {
                myResult = AddEditActivityNotesComponent_1.AddEditActivityNotesComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map