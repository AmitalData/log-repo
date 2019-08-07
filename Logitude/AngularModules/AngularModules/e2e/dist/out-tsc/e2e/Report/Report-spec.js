"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var SenarioTest_1 = require("./SenarioTest");
var Login_po_1 = require("../login/Login.po");
describe('Report', function () {
    var Senarios = new SenarioTest_1.SenarioTest();
    var page = new Login_po_1.LoginComp();
    beforeEach(function () {
        protractor_1.browser.driver.manage().window().maximize();
        protractor_1.browser.ignoreSynchronization = true;
    });
    it('Run Report Sucssefuly', function () {
        Senarios.ReportScienarios();
    });
    it('Run Report Faield', function () {
        Senarios.FailedScienarios();
    });
});
//# sourceMappingURL=Report-spec.js.map