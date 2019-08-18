"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var NewOpportunity = /** @class */ (function () {
    function NewOpportunity() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    NewOpportunity.prototype.CreateNewOpportunity = function (opportunityNo) {
        this.Helper.WaitByIdAndClick('NEWOPPORTUNITY');
        this.FillOpportunityFields(opportunityNo);
        this.Helper.WaitByIdAndClick('Ok-AddOpportunity');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
    };
    NewOpportunity.prototype.FillOpportunityFields = function (opportunityNo) {
        this.Helper.WaitByIdAndFill('Opportunity_OpportunityTypeId', 'i');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
        this.Helper.WaitByIdAndFill('Opportunity_Subject', opportunityNo);
        this.Helper.WaitByIdAndFill('date_Opportunity_EstimatedClosingDate', '1');
        this.Helper.WaitByIdAndFill('Opportunity_CustomerId', 'razan');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
        this.Helper.WaitByIdAndFill('Opportunity_Notes', 'Opportunity_Notes - Protractor '); // test random number randomWholeNum
        // this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
        // this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');
        // this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
        // this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');
    };
    return NewOpportunity;
}());
exports.NewOpportunity = NewOpportunity;
//# sourceMappingURL=NewOpportunity.js.map