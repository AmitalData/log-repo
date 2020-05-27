"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var EditGLAccount = /** @class */ (function () {
    function EditGLAccount() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.gn = new GeneralFunctions_1.GeneralFunctions();
    }
    EditGLAccount.prototype.EditGLAccount = function (DisplayNumber) {
        this.Helper.WaitByIdAndFill('CardGLAccount_Search', DisplayNumber);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('GLAccount.TH.General');
        this.Helper.WaitByIdAndFill('GLAccount_LocalName', 'Updated Local Name');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('GLAccount-Save');
        this.Helper.WaitBusyIndicator();
        // browser.sleep(5000);
    };
    return EditGLAccount;
}());
exports.EditGLAccount = EditGLAccount;
//# sourceMappingURL=EditGLaccount.js.map