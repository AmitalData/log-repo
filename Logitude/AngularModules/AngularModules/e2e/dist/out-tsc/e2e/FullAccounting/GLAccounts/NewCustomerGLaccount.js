"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var NewCustomer = /** @class */ (function () {
    function NewCustomer() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    NewCustomer.prototype.CreateNewCustomerGLAccount = function (Name) {
        this.Helper.WaitByIdAndClick('NewCustomer');
        this.Helper.WaitByIdAndFill('Customer_EnglishName', Name);
        this.Helper.WaitByIdAndFill('Customer_LocalName', Name);
        this.Helper.WaitByIdAndFill('Customer_Address1_Potential', 'Ramallah');
        this.Helper.WaitByIdAndFill('Customer_Address2_Potential', 'Nablus');
        this.Helper.WaitByIdAndFill('Customer_ZipCode_Potential', '00970');
        this.Helper.WaitByIdAndFill('Customer_CountryId_Potential', 'ps');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Customer_City_Potential', 'Nablus');
        this.Helper.WaitByIdAndFill('Contact_EnglishName', 'My Contact');
        this.Helper.WaitByIdAndClick('Ok-AddPotCustomer');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
    };
    NewCustomer.prototype.ActivateCustomerGLAccount = function (Name, DisplayNumber) {
        this.Generator.QuickSearchTextBox('Card_Search', Name);
        this.Helper.WaitByIdAndClick('Customer.B.Activate');
        this.Helper.WaitByIdAndClick('Ok-activate');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
        this.Helper.WaitByIdAndClick('Customer.TH.Accounting');
        this.Helper.WaitByIdAndClick('Activate');
        this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'cust');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
        //this.Helper.WaitByIdAndFill('GLAccount_LocalName', Name+DisplayNumber);
        this.Helper.WaitByIdAndFill('GLAccount_CurrencyId', 'Nis');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        //this.Helper.WaitBusyIndicator();
        var boo = this.Helper.ItemsVisibility('3mo');
        if ('boo') {
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndClick('Customer-SaveClose');
        }
    };
    return NewCustomer;
}());
exports.NewCustomer = NewCustomer;
//# sourceMappingURL=NewCustomerGLaccount.js.map