"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var NewVendorGLaccount_1 = require("./NewVendorGLaccount");
describe('ARInvoice Module', function () {
    var Helper = new GeneralFunctions_1.GeneralFunctions();
    var F = new FieldsHelper_1.FieldsHelper();
    protractor_1.browser.driver.manage().window().maximize();
    var v = new NewVendorGLaccount_1.NewVendor();
    it(' New Vendor GLAccount Was Created', function () {
        protractor_1.browser.ignoreSynchronization = true;
        // Helper.GoToMainMenu('General.MH.FullAccounting');
        // F.WaitByIdAndClick('FACS');
        var n = Helper.RandomNum();
        v.CreateNewVendorGLAccount('Test Vendor GLAccount');
        v.ActivateVendorGLAccount('Test Vendor GLAccount' + n, n);
    });
});
//# sourceMappingURL=VendorGLAccount-spec.js.map