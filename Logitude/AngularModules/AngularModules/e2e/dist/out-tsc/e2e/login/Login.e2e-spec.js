"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Login_po_1 = require("./Login.po");
var protractor_1 = require("protractor");
describe('Login Module', function () {
    var page = new Login_po_1.LoginComp();
    beforeEach(function () {
        protractor_1.browser.driver.manage().window().maximize();
    });
    it('Login Success', function () {
        protractor_1.browser.ignoreSynchronization = true;
        page.navigateTo(protractor_1.browser.params.Link);
        page.DoLogin(protractor_1.browser.params.Login.Email, protractor_1.browser.params.Login.Password);
        //page.navigateTo('http://localhost:4200/');
        //page.DoLogin('sgautomation@pro.com', 'Sg0592463934!');
    });
});
//# sourceMappingURL=Login.e2e-spec.js.map