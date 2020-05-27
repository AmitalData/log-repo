"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var LoginComp = /** @class */ (function () {
    function LoginComp() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    LoginComp.prototype.navigateTo = function (url) {
        return protractor_1.browser.get(url + '?Menu=protractor');
    };
    LoginComp.prototype.DoLogin = function (userName, password) {
        protractor_1.browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndFill('Email', userName);
        this.Helper.WaitByIdAndFill('Password', password);
        this.Helper.ButtonClick('cmdLogin');
    };
    return LoginComp;
}());
exports.LoginComp = LoginComp;
//# sourceMappingURL=Login.po.js.map