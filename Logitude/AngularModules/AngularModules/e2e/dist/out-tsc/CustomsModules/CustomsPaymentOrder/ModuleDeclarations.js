"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewPaymentOrderComponent_1 = require("./Components/NewEntity/NewPaymentOrderComponent");
var PaymentOrdersGeneralTabComponent_1 = require("./Components/EditTabs/General/PaymentOrdersGeneralTabComponent");
var AccountingCustomFilesComponent_1 = require("./Components/EditTabs/General/AccountingCustomFilesComponent");
var SendPaymentOrderComponent_1 = require("./Components/SendPaymentOrder/SendPaymentOrderComponent");
var PaymentOrderDepositDataComponent_1 = require("./Components/EditTabs/Tapag/Deposit/PaymentOrderDepositDataComponent");
var PaymentOrderDeficitComponent_1 = require("./Components/EditTabs/Tapag/Deficit/PaymentOrderDeficitComponent");
var BankAccountToRefundComponent_1 = require("./Components/EditTabs/Tapag/Deposit/BankAccountToRefundComponent");
exports.Components = [
    NewPaymentOrderComponent_1.NewPaymentOrderComponent,
    PaymentOrdersGeneralTabComponent_1.PaymentOrdersGeneralTabComponent,
    AccountingCustomFilesComponent_1.AccountingCustomFilesComponent,
    SendPaymentOrderComponent_1.SendPaymentOrderComponent,
    PaymentOrderDepositDataComponent_1.PaymentOrderDepositDataComponent,
    PaymentOrderDeficitComponent_1.PaymentOrderDeficitComponent,
    BankAccountToRefundComponent_1.BankAccountToRefundComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewPaymentOrderComponent": {
                myResult = NewPaymentOrderComponent_1.NewPaymentOrderComponent;
                break;
            }
            case "PaymentOrdersGeneralTabComponent": {
                myResult = PaymentOrdersGeneralTabComponent_1.PaymentOrdersGeneralTabComponent;
                break;
            }
            case "AccountingCustomFilesComponent": {
                myResult = AccountingCustomFilesComponent_1.AccountingCustomFilesComponent;
                break;
            }
            case "SendPaymentOrderComponent": {
                myResult = SendPaymentOrderComponent_1.SendPaymentOrderComponent;
                break;
            }
            case "PaymentOrderDepositDataComponent": {
                myResult = PaymentOrderDepositDataComponent_1.PaymentOrderDepositDataComponent;
                break;
            }
            case "PaymentOrderDeficitComponent": {
                myResult = PaymentOrderDeficitComponent_1.PaymentOrderDeficitComponent;
                break;
            }
            case "BankAccountToRefundComponent": {
                myResult = BankAccountToRefundComponent_1.BankAccountToRefundComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map