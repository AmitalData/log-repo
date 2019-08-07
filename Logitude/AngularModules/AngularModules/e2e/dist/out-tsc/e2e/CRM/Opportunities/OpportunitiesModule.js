"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var NewOpportunity_1 = require("./NewEntity/NewOpportunity");
var EditOpportunityMainTab_1 = require("./EditEntity/EditOpportunityMainTab");
var EditOpportunityGeneralTab_1 = require("./EditEntity/EditOpportunityGeneralTab");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var OpportunityModule = /** @class */ (function () {
    function OpportunityModule() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
        this.addOpportunity = new NewOpportunity_1.NewOpportunity();
        this.editMainTab = new EditOpportunityMainTab_1.EditOpportunityMainTab();
        this.editGeneralTab = new EditOpportunityGeneralTab_1.EditOpportunityGeneralTab();
    }
    OpportunityModule.prototype.CreateOpportunity = function () {
        var activityNo = this.Generator.RandomNum();
        this.addOpportunity.CreateNewOpportunity('Opportunity # ' + activityNo);
        this.QuickSearchBox('Opportunity_Search', 'Opportunity # ' + activityNo);
        this.editMainTab.EditMainTab('Opportunity # ' + activityNo);
        this.editGeneralTab.EditGeneralTab('Opportunity # ' + activityNo);
        // browser.driver.sleep(6000);
    };
    OpportunityModule.prototype.QuickSearchBox = function (searchFeildId, searchByRef) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.LogitudeQuickSearchItem', 0);
    };
    return OpportunityModule;
}());
exports.OpportunityModule = OpportunityModule;
//# sourceMappingURL=OpportunitiesModule.js.map