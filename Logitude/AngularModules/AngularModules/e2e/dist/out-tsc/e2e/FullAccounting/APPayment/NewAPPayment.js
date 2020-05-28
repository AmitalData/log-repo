"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var protractor_1 = require("protractor");
var NewAPPayment = /** @class */ (function () {
    function NewAPPayment() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.gn = new GeneralFunctions_1.GeneralFunctions();
    }
    NewAPPayment.prototype.CreateNewAPPayment = function (VendorName) {
        protractor_1.browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewAPPayment');
        this.Helper.WaitByIdAndFill('APPayment_VendorId', VendorName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('APPayment_AccountingPaymentMethodId', 'Cash');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0),
            this.Helper.WaitByIdAndFill('APPayment_AmountInPaymentCurrency', '10000');
        //this.Helper.WaitByIdAndClick('ok-AddARPayment');
        //this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('APPayment_PaymentCurrencyId', 'NIS');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        this.Helper.WaitByIdAndClick('APPayment.B.Approve');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('EditBackbutton');
        protractor_1.browser.sleep(6000);
    };
    return NewAPPayment;
}());
exports.NewAPPayment = NewAPPayment;
//# sourceMappingURL=NewAPPayment.js.map