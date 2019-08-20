"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var PayablesTabComponent = /** @class */ (function () {
    function PayablesTabComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    PayablesTabComponent.prototype.PayablesTab = function (shipperRef1, ShipmentType) {
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('Shipment.TH.Payables');
        this.Helper.WaitBusyIndicator();
        this.Helper.ItemsVisibility('ATDSPayable-payable');
        this.Helper.WaitByIdAndClick('ATDSPayable-payable'); //Auto-display 
        //this.Helper.WaitByCssButtonClick('RedButton','Yes');
        //var InvoiceAmount: any;
        if (ShipmentType == '') {
            this.amount1 = this.AddPayables('Air Frei', '10', '10');
            this.amount2 = this.AddPayables('Order', '10', '20');
        }
        else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
            this.amount1 = this.AddPayables('ocean', '10', '10');
            this.amount2 = this.AddPayables('Order', '10', '20');
        }
        else {
            this.amount1 = this.AddPayables('Inland', '10', '10');
            this.amount2 = this.AddPayables('Order', '10', '20');
        }
        //InvoiceAmount = this.amount1 + this.amount2
        // this.AddAPInvoice(shipperRef1);
    };
    PayablesTabComponent.prototype.AddPayables = function (ChargeType, quantity, unitPrice) {
        var amount = 0;
        this.Helper.WaitByIdAndClick('AddPayable');
        this.Helper.WaitByIdAndFill('ShipmentPayable_ChargesTypeId', ChargeType);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('ShipmentPayable_Quantity', quantity);
        this.Helper.WaitByIdAndFill('ShipmentPayable_UnitPrice', unitPrice);
        amount = parseInt(quantity) * parseInt(unitPrice);
        this.Helper.WaitByIdAndFill('ShipmentPayable_CurrencyId', 'EU');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndClick('Ok-AddPayableBtn');
        return amount;
    };
    PayablesTabComponent.prototype.AddAPInvoice = function (shipperRef1) {
        var _this = this;
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ReceiveInvoice');
        this.Helper.WaitByIdAndFill('APInvoice_VendorId', 'TestVendorId');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceNumber', shipperRef1);
        // this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency', Amount);
        this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency', this.amount1);
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceCurrencyId', 'EUR');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('date_APInvoice_InvoiceDate', '.');
        this.Helper.WaitByIdAndFill('APInvoice_PaymentTermId', 'cash');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('APInvoice_VATNumber', 'Vat Number ');
        // this.Helper.WaitBusyIndicator();
        // browser.driver.sleep(2000);
        this.Helper.WaitByIdAndClick('Ok-CreateAPInvoice');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('APInvoice_VatTypeId', 'zero');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndClick('VATApplyToAll');
        this.Helper.WaitByIdAndFill('APInvoiceLine_ForiegnCurrencyAmount', this.amount1);
        // this.Helper.WaitByIdAndFill('APInvoiceLine_ForiegnCurrencyAmount_1', this.amount2);
        // this.Helper.WaitByIdAndClick('APInvoice.B.Save');
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id('APInvoice.B.Save'))), 20000).then(function (a) {
            protractor_1.element(protractor_1.by.id('APInvoice.B.Save')).click();
            // this.Helper.WaitBusyIndicator();
            protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.css('.busyindicatorcontrol'))), 100000).then(function (a) {
                // browser.wait(EC.elementToBeClickable(element(by.id('APInvoice.B.Approve'))), 100000).then(a => {
                _this.Helper.WaitByIdAndClick('APInvoice.B.Approve');
            });
        });
        this.Helper.WaitBusyIndicator();
        protractor_1.browser.driver.sleep(30000);
        this.Helper.WaitByIdAndClick('EditBackbutton_1');
        this.Helper.WaitBusyIndicator();
    };
    return PayablesTabComponent;
}());
exports.PayablesTabComponent = PayablesTabComponent;
//# sourceMappingURL=PayablesTab.js.map