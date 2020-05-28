"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var NewARPayment_1 = require("./NewARPayment");
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
describe('ARInvoice Module', function () {
    var Helper = new GeneralFunctions_1.GeneralFunctions();
    var F = new FieldsHelper_1.FieldsHelper();
    protractor_1.browser.driver.manage().window().maximize();
    var arpay = new NewARPayment_1.NewARPayment();
    it(' New ARPayment Was Created', function () {
        protractor_1.browser.ignoreSynchronization = true;
        Helper.GoToMainMenu('General.MH.FullAccounting');
        F.WaitByIdAndClick('FACS');
        arpay.CreateNewARPayment('Test Customer GLAccount');
    });
});
//# sourceMappingURL=ARPayment-spec.js.map