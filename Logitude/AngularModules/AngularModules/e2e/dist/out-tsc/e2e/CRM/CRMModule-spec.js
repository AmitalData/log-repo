"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var CRMModule_1 = require("./CRMModule");
describe('CRM Module', function () {
    var CRMPage;
    beforeEach(function () {
        CRMPage = new CRMModule_1.CRMComp();
    });
    afterEach(function () {
    });
    it('Operations Success', function () {
        protractor_1.browser.ignoreSynchronization = true;
        // CRMPage.DoCRM('Overview');
        CRMPage.DoCRM('Customers');
        // CRMPage.DoCRM('Quotes');
        // CRMPage.DoCRM('Activities');
        // CRMPage.DoCRM('Opportunities');
    });
});
//# sourceMappingURL=CRMModule-spec.js.map