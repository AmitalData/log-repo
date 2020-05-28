"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../Helpers/GeneralFunctions");
var ActivitiesModule_1 = require("./Activities/ActivitiesModule");
var OpportunitiesModule_1 = require("./Opportunities/OpportunitiesModule");
var CustomersModule_1 = require("./Customers/CustomersModule");
var CRMComp = /** @class */ (function () {
    function CRMComp() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.CRMTab = new GeneralFunctions_1.GeneralFunctions();
        this.Activities = new ActivitiesModule_1.ActivitiesModule();
        this.Opportunities = new OpportunitiesModule_1.OpportunityModule();
        this.Customers = new CustomersModule_1.CustomerModule();
    }
    CRMComp.prototype.DoCRM = function (CRMcomponent) {
        this.CRMTab.GoToMainMenu('General.MH.CRM');
        if (CRMcomponent == 'Overview') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMOVE');
        }
        else if (CRMcomponent == 'Customers') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMCUS');
            this.Customers.CreateCustomer();
        }
        else if (CRMcomponent == 'Quotes') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMQUT');
        }
        else if (CRMcomponent == 'Activities') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMACT');
            this.Activities.CreateActivity();
        }
        else if (CRMcomponent == 'Opportunities') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMOPP');
            this.Opportunities.CreateOpportunity();
        }
    };
    return CRMComp;
}());
exports.CRMComp = CRMComp;
//# sourceMappingURL=CRMModule.js.map