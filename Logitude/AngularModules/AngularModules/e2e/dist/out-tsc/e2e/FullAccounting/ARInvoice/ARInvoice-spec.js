"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var NewARInvoice_1 = require("./New/NewARInvoice");
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
describe('ARInvoice Module', function () {
    var Helper = new GeneralFunctions_1.GeneralFunctions();
    var F = new FieldsHelper_1.FieldsHelper();
    protractor_1.browser.driver.manage().window().maximize();
    var arinvoice = new NewARInvoice_1.NewARInvoice();
    it(' New General ARInvoice Was Created', function () {
        protractor_1.browser.ignoreSynchronization = true;
        Helper.GoToMainMenu('General.MH.FullAccounting');
        F.WaitByIdAndClick('FACS');
        arinvoice.CreateNewARInvoice('Test Customer GLAccount');
        // arinvoice.CreateNewARInvoice('Basel - Multi Local');
    });
});
//# sourceMappingURL=ARInvoice-spec.js.map