"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var EditOpportunityMainTab = /** @class */ (function () {
    function EditOpportunityMainTab() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    EditOpportunityMainTab.prototype.EditMainTab = function (opportunityDesc) {
        this.EditMainTabFeilds(opportunityDesc);
        // this.Helper.WaitByIdAndClick('Activity-SaveClose');
        // this.Helper.WaitBusyIndicator();
        // var EC = protractor.ExpectedConditions;
        // browser.wait(EC.invisibilityOf(element(by.id('Activity.B.MarkAsComplete'))), 100000).then(a => {
        // });
    };
    EditOpportunityMainTab.prototype.EditMainTabFeilds = function (opportunityDesc) {
        // this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for ' + opportunityDesc);
        // this.Helper.WaitByIdAndFill('Activity_Description', 'Edit Description for ' + opportunityDesc);// test random number randomWholeNum
        // this.Helper.WaitByIdAndFill('Activity_OwnerId', 'r');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
        // this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
        // // this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');
        // this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
        // // this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');
    };
    return EditOpportunityMainTab;
}());
exports.EditOpportunityMainTab = EditOpportunityMainTab;
//# sourceMappingURL=EditOpportunityMainTab.js.map