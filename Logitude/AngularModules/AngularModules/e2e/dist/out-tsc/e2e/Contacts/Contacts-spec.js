"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../Helpers/GeneralFunctions");
describe('contacts', function () {
    var x = new FieldsHelper_1.FieldsHelper();
    var z = new GeneralFunctions_1.GeneralFunctions();
    it('', function () {
        z.GoToMainMenu('General.MH.Contacts');
        protractor_1.browser.driver.sleep(5000);
    });
});
//# sourceMappingURL=Contacts-spec.js.map