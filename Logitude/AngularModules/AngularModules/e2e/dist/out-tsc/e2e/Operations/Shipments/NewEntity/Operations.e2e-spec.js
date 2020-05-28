"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Operations_po_1 = require("./Operations.po");
var protractor_1 = require("protractor");
describe('Operations Module', function () {
    var page = new Operations_po_1.OperationsComp();
    // let login: LoginComp=new LoginComp();
    var count = 0;
    afterEach(function () {
        // browser.switchTo().alert().accept();
    });
    it('Operations Success', function () {
        protractor_1.browser.ignoreSynchronization = true;
        page.DoOperations();
    });
});
//# sourceMappingURL=Operations.e2e-spec.js.map