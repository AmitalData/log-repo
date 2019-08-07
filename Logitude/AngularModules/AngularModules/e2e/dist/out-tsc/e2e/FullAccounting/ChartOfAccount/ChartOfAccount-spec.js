"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var ChartOFAccountModule_1 = require("./ChartOFAccountModule");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var fieldshelper_1 = require("../../Helpers/fieldshelper");
describe('CRM Module', function () {
    var chartofaccount = new ChartOFAccountModule_1.ChartOFAccountModule();
    var z = new GeneralFunctions_1.GeneralFunctions();
    var y = new fieldshelper_1.FieldsHelper();
    it('Chart Of Account Success', function () {
        protractor_1.browser.ignoreSynchronization = true;
        z.GoToMainMenu('General.MH.Maintenance');
        y.WaitByIdAndClick('ACC');
        y.WaitByIdAndClick('MaintenanceItemMTCA');
        y.WaitByIdAndClick('NewButton_ChartOfAccount');
        chartofaccount.CreateAndEditChartOfAccount();
        // CRMPage.DoCRM('Overview');
        //  CRMPage.DoCRM('Customers');
        // CRMPage.DoCRM('Quotes');
        // CRMPage.DoCRM('Activities');
        // CRMPage.DoCRM('Opportunities');
    });
});
//# sourceMappingURL=ChartOfAccount-spec.js.map