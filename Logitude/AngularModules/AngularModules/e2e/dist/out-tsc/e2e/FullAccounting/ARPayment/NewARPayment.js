"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var protractor_1 = require("protractor");
var NewARPayment = /** @class */ (function () {
    function NewARPayment() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.gn = new GeneralFunctions_1.GeneralFunctions();
    }
    NewARPayment.prototype.CreateNewARPayment = function (BillToName) {
        protractor_1.browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewARPayment');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_BillToId', BillToName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_BillToAddressId', 'Main Address');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_AmountInPaymentCurrency', '10000');
        this.Helper.WaitByIdAndFill('ARPayment_AccountingPaymentMethodId', 'Cash');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0),
            this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ok-AddARPayment');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_BranchId', 'Main Office');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        //        this.Helper.WaitWindowClosed();
        this.Helper.WaitByIdAndClick('ARPayment.B.Approve');
        this.Helper.WaitBusyIndicator();
        //  browser.sleep(6000);
        // this.Helper.WaitByIdAndClick('EditBackbutton');
        // browser.sleep(6000);
    };
    return NewARPayment;
}());
exports.NewARPayment = NewARPayment;
//# sourceMappingURL=NewARPayment.js.map