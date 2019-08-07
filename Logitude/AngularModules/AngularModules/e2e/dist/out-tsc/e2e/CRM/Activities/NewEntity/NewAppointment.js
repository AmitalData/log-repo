"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var NewAppointment = /** @class */ (function () {
    function NewAppointment() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    NewAppointment.prototype.CreateNewAppointment = function (AppointmentNo) {
        this.Helper.WaitByIdAndClick('NEWACTIVITY');
        this.Helper.WaitByIdAndClick('NEWAPPOINTMENT');
        this.FillAppointmentFields(AppointmentNo);
        this.Helper.WaitByIdAndClick('Ok-AddActivity');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
    };
    NewAppointment.prototype.FillAppointmentFields = function (AppointmentNo) {
        this.Helper.WaitByIdAndFill('Activity_Subject', AppointmentNo);
        this.Helper.WaitByIdAndFill('Activity_Location', 'Ramallah');
        // this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
        // this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');
        // this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
        // this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');
        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
        this.Helper.WaitByIdAndFill('Activity_Description', AppointmentNo);
    };
    return NewAppointment;
}());
exports.NewAppointment = NewAppointment;
//# sourceMappingURL=NewAppointment.js.map