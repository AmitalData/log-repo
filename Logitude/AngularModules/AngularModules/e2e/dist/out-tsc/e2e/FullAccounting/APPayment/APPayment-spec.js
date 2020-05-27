"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var NewAPPayment_1 = require("./NewAPPayment");
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
describe('APInvoice Module', function () {
    var Helper = new GeneralFunctions_1.GeneralFunctions();
    var F = new FieldsHelper_1.FieldsHelper();
    protractor_1.browser.driver.manage().window().maximize();
    var appayment = new NewAPPayment_1.NewAPPayment();
    it(' New APInvoice Was Created', function () {
        protractor_1.browser.ignoreSynchronization = true;
        Helper.GoToMainMenu('General.MH.FullAccounting');
        F.WaitByIdAndClick('FAVND');
        var NUM = Helper.RandomNum();
        appayment.CreateNewAPPayment('Test Customer GLAccount');
    });
});
//# sourceMappingURL=APPayment-spec.js.map