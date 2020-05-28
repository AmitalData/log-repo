"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var NewVendor = /** @class */ (function () {
    function NewVendor() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.Generator = new GeneralFunctions_1.GeneralFunctions();
    }
    NewVendor.prototype.CreateNewVendorGLAccount = function (Name) {
        this.Helper.WaitByIdAndClick('General.MH.Maintenance');
        this.Generator.GoToMainMenu('MaintenanceItemMTVD');
        //this.Helper.WaitByIdAndClick('MaintenanceItemMTVD');
        this.Helper.WaitByIdAndClick('NewButton_Vendor');
        this.Helper.WaitByIdAndFill('Address_Name', Name);
        this.Helper.WaitByIdAndFill('Address_CountryId', 'ps');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Address_City', 'Nablus');
        this.Helper.WaitByIdAndClick('Ok-AddVendor');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
        this.Helper.WaitByIdAndFill('SearchFieldsId_0_0', Name);
        this.Helper.ItemsPresent('ListDataLoaded');
        this.Helper.WaitByIdAndClick('row0col1');
    };
    NewVendor.prototype.ActivateVendorGLAccount = function (Name, DisplayNumber) {
        //this.Generator.QuickSearchTextBox('SearchFieldsId_0_0', Name);
        // this.Helper.WaitByIdAndClick('row0col1');
        this.Helper.WaitByIdAndClick('Vendor.TH.Accounting');
        this.Helper.WaitByIdAndClick('Activate');
        this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'ven');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
        // this.Helper.WaitByIdAndFill('GLAccount_LocalName', Name + DisplayNumber);
        this.Helper.WaitByIdAndFill('GLAccount_CurrencyId', 'Nis');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicatorToShow();
        this.Helper.WaitBusyIndicator();
        //browser.sleep(5000);
        //this.Helper.WaitBusyIndicator();
        var boo = this.Helper.ItemsVisibility('3mo');
        if ('boo') {
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndClick('Vendor-SaveClose');
            this.Helper.WaitBusyIndicator();
        }
    };
    return NewVendor;
}());
exports.NewVendor = NewVendor;
//# sourceMappingURL=NewVendorGLaccount.js.map