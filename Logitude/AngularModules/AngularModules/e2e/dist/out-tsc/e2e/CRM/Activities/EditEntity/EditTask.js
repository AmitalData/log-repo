"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var EditTasks = /** @class */ (function () {
    function EditTasks() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    EditTasks.prototype.EditTask = function (taskNo) {
        this.EditTaskGeneralTab(taskNo);
        this.Helper.WaitByIdAndClick('Activity-SaveClose');
        this.Helper.WaitBusyIndicator();
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.id('Activity.B.MarkAsComplete'))), 100000).then(function (a) {
        });
    };
    EditTasks.prototype.EditTaskGeneralTab = function (taskDesc) {
        this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for ' + taskDesc);
        this.Helper.WaitByIdAndFill('Activity_Description', 'Edit Description for ' + taskDesc); // test random number randomWholeNum
        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'r');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
        this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
        // this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');
        this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
        // this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');
    };
    return EditTasks;
}());
exports.EditTasks = EditTasks;
//# sourceMappingURL=EditTask.js.map