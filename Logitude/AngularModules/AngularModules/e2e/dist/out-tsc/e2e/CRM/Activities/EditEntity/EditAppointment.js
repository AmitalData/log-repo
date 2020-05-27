"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var EditAppointments = /** @class */ (function () {
    function EditAppointments() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    EditAppointments.prototype.EditAppointment = function (appointmentNo) {
        this.EditAppointmentGeneralTab(appointmentNo);
        this.Helper.WaitByIdAndClick('Activity-SaveClose');
        this.Helper.WaitBusyIndicator();
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.id('Activity.B.MarkAsComplete'))), 100000).then(function (a) {
        });
    };
    EditAppointments.prototype.EditAppointmentGeneralTab = function (appointmentDesc) {
        this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for ' + appointmentDesc);
        this.Helper.WaitByIdAndFill('Activity_Location', 'Ramallah');
        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
        this.Helper.WaitByIdAndFill('Activity_Description', 'Edit Description for ' + appointmentDesc);
    };
    return EditAppointments;
}());
exports.EditAppointments = EditAppointments;
//# sourceMappingURL=EditAppointment.js.map