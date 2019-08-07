"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var NewChartOfAccount = /** @class */ (function () {
    function NewChartOfAccount() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.GeneralFun = new GeneralFunctions_1.GeneralFunctions();
    }
    NewChartOfAccount.prototype.CreateNewChartOFAccount = function (ChartOfAccountNo, Type) {
        // var chartOfAccountNo = this.GeneralFun.RandomNumAcc();
        this.Helper.WaitByIdAndClick('NewButton_ChartOfAccount');
        this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_EnglishName', Type + ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_LocalName', Type + ChartOfAccountNo);
        if (Type == 'Revenue') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Reven');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Customer') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Custom');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Vendor') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Vendor');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            //this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Banks') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Banks');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Expenses') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Expenses');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Works') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Works');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            //this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Debtors And Creditors') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Debtors And Creditors');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            //this.Helper.WaitBusyIndicator();
        }
        //this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode','r');
        // this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        // this.Helper.WaitByIdAndClick('CheckBox');
        this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
    };
    return NewChartOfAccount;
}());
exports.NewChartOfAccount = NewChartOfAccount;
//# sourceMappingURL=NewChartOfAccount.js.map