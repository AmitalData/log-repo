"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../../Helpers/GeneralFunctions");
var protractor_1 = require("protractor");
var NewAPInvoice = /** @class */ (function () {
    function NewAPInvoice() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.gn = new GeneralFunctions_1.GeneralFunctions();
    }
    NewAPInvoice.prototype.CreateNewAPInvoice = function (VendorName, InvoiceNumer) {
        //console.log("this is the first one");
        protractor_1.browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewAPInvoice');
        //this.Helper.WaitBusyIndicatorToShow();
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('APInvoice_VendorId', VendorName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        ////browser.sleep(5000);
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceNumber', InvoiceNumer);
        this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency', '1000');
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceCurrencyId', 'NIS');
        // console.log("this is the second one");
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('date_APInvoice_InvoiceDate', '10/06/2019');
        this.Helper.WaitByIdAndFill('date_APInvoice_DueDate', '10/06/2019');
        this.Helper.WaitByIdAndFill('APInvoice_VATNumber', '123456789');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('Ok-AddAPInvoice');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitBusyIndicatorToShow();
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('AddInvoiceLine');
        this.Helper.WaitByIdAndFill('APInvoiceLine_ChargesTypeId', 'Air Freight');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('APInvoiceLine_InvoiceCurrencyAmount', '1000');
        this.Helper.WaitByIdAndClick('Ok-AddAPInvoiceLine');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        //browser.sleep(6000);
        this.Helper.WaitByIdAndClick('APInvoice.B.Approve');
        this.Helper.WaitBusyIndicator();
        // this.Helper.WaitBusyIndicator();
        //  browser.sleep(10000);
        //this.Helper.WaitByIdAndClick('EditBackbutton');
    };
    return NewAPInvoice;
}());
exports.NewAPInvoice = NewAPInvoice;
//# sourceMappingURL=NewAPInvoice.js.map