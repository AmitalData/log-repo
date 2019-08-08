"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var protractor_1 = require("protractor");
var EditARInvoice = /** @class */ (function () {
    function EditARInvoice() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.gn = new GeneralFunctions_1.GeneralFunctions();
    }
    EditARInvoice.prototype.EditARInvoice = function (DisplayNumber) {
        this.Helper.WaitByIdAndFill('CardGLAccount_Search', DisplayNumber);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.Helper.WaitByIdAndClick('GLAccount.TH.General');
        this.Helper.WaitByIdAndFill('GLAccount_LocalName', 'Updated Local Name');
        this.Helper.WaitByIdAndClick('GLAccount-Save');
        protractor_1.browser.sleep(5000);
    };
    return EditARInvoice;
}());
exports.EditARInvoice = EditARInvoice;
//# sourceMappingURL=EditARInvoice.js.map