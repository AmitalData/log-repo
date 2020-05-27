"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var protractor_1 = require("protractor");
var NewGLAccount = /** @class */ (function () {
    function NewGLAccount() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.gn = new GeneralFunctions_1.GeneralFunctions();
    }
    NewGLAccount.prototype.CreateNewGLAccount = function (LocalName, DisplayNumber) {
        protractor_1.browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewGLAccount');
        this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsTypeCode', 'Revenues');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'Rev');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
        this.Helper.WaitByIdAndFill('GLAccount_LocalName', LocalName);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
    };
    return NewGLAccount;
}());
exports.NewGLAccount = NewGLAccount;
//# sourceMappingURL=NewGLaccount.js.map