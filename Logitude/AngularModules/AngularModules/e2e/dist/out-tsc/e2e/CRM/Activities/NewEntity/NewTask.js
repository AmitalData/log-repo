"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var NewTask = /** @class */ (function () {
    function NewTask() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    NewTask.prototype.CreateNewTask = function (taskNo) {
        this.Helper.WaitByIdAndClick('NEWACTIVITY');
        this.Helper.WaitByIdAndClick('NEWTASK');
        this.FillTaskFields(taskNo);
        this.Helper.WaitByIdAndClick('Ok-AddActivity');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
    };
    NewTask.prototype.FillTaskFields = function (taskNo) {
        this.Helper.WaitByIdAndFill('Activity_Subject', taskNo);
        this.Helper.WaitByIdAndFill('Activity_Description', 'Task Description - Protractor '); // test random number randomWholeNum
        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
        this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
        this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');
        this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
        this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');
    };
    return NewTask;
}());
exports.NewTask = NewTask;
//# sourceMappingURL=NewTask.js.map