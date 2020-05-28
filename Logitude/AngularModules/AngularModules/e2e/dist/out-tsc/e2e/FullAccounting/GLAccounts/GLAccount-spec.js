"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var NewGLaccount_1 = require("./New/NewGLaccount");
var EditGLaccount_1 = require("./Edit/EditGLaccount");
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
describe('CRM Module', function () {
    var gn1 = new GeneralFunctions_1.GeneralFunctions();
    var h = new FieldsHelper_1.FieldsHelper();
    protractor_1.browser.driver.manage().window().maximize();
    var GLA = new NewGLaccount_1.NewGLAccount();
    var EditGLA = new EditGLaccount_1.EditGLAccount();
    it(' New GLAccount Was Created And Updated', function () {
        protractor_1.browser.ignoreSynchronization = true;
        gn1.GoToMainMenu('General.MH.FullAccounting');
        h.WaitByIdAndClick('FAGLAccouts');
        var GlaccountNumber = gn1.RandomNum();
        GLA.CreateNewGLAccount('My Auto GLAccount', GlaccountNumber);
        EditGLA.EditGLAccount(GlaccountNumber);
    });
});
//# sourceMappingURL=GLAccount-spec.js.map