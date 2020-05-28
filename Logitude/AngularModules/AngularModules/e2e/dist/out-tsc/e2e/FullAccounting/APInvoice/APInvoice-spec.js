"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var NewAPInvoice_1 = require("./NewAPInvoice");
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
describe('APInvoice Module', function () {
    var Helper = new GeneralFunctions_1.GeneralFunctions();
    var F = new FieldsHelper_1.FieldsHelper();
    protractor_1.browser.driver.manage().window().maximize();
    var apinvoice = new NewAPInvoice_1.NewAPInvoice();
    it(' New APInvoice Was Created', function () {
        protractor_1.browser.ignoreSynchronization = true;
        Helper.GoToMainMenu('General.MH.FullAccounting');
        F.WaitByIdAndClick('FAVND');
        var NUM = Helper.RandomNum();
        apinvoice.CreateNewAPInvoice('Vendor GLaccount', NUM);
    });
});
//# sourceMappingURL=APInvoice-spec.js.map