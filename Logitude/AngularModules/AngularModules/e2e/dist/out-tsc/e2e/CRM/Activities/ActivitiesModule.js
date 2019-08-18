"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var NewTask_1 = require("./NewEntity/NewTask");
var EditTask_1 = require("./EditEntity/EditTask");
var NewPhoneCall_1 = require("./NewEntity/NewPhoneCall");
var EditPhoneCall_1 = require("./EditEntity/EditPhoneCall");
var NewAppointment_1 = require("./NewEntity/NewAppointment");
var EditAppointment_1 = require("./EditEntity/EditAppointment");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var ActivitiesModule = /** @class */ (function () {
    function ActivitiesModule() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.GeneralFun = new GeneralFunctions_1.GeneralFunctions();
        this.addTask = new NewTask_1.NewTask();
        this.editTask = new EditTask_1.EditTasks();
        this.addPhoneCall = new NewPhoneCall_1.NewPhoneCall();
        this.editPhoneCall = new EditPhoneCall_1.EditPhoneCalls();
        this.addAppointment = new NewAppointment_1.NewAppointment();
        this.editAppointment = new EditAppointment_1.EditAppointments();
    }
    ActivitiesModule.prototype.CreateActivity = function () {
        var activityNo = this.GeneralFun.RandomNum();
        this.editTask.EditTask('Task # ' + activityNo);
        this.addTask.CreateNewTask('Task # ' + activityNo);
        this.GeneralFun.QuickSearchTextBox('Activity_Search', 'Task # ' + activityNo);
        this.addPhoneCall.CreateNewPhoneCall('Phone Call # ' + activityNo);
        this.GeneralFun.QuickSearchTextBox('Activity_Search', 'Phone Call # ' + activityNo);
        this.editPhoneCall.EditPhoneCall('Phone Call # ' + activityNo);
        this.addAppointment.CreateNewAppointment('Appointment # ' + activityNo);
        this.GeneralFun.QuickSearchTextBox('Activity_Search', 'Appointment # ' + activityNo);
        this.editAppointment.EditAppointment('Appointment # ' + activityNo);
        // browser.driver.sleep(6000);
    };
    return ActivitiesModule;
}());
exports.ActivitiesModule = ActivitiesModule;
this.addPhoneCall.CreateNewTask('task#' + activityNo);
this.QuickSearchBox('activity', 'task#' + activityNo);
this.editPhoneCall.EditPhoneCall("");
//# sourceMappingURL=ActivitiesModule.js.map