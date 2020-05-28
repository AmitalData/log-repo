"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var ReceivablesTabComponent = /** @class */ (function () {
    function ReceivablesTabComponent() {
        this.receivableCurrency = '';
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    ReceivablesTabComponent.prototype.RecievablesTab = function (ShipmentLevelCode, shipmentType) {
        this.Helper.WaitByIdAndClick('Shipment.TH.Receivables');
        this.Helper.WaitBusyIndicator();
        // var EC = protractor.ExpectedConditions;
        // this.Helper.WaitByIdAndClick('Shipment_EstimateProfitInSelectedCurrency');
        if (ShipmentLevelCode == 'D' || ShipmentLevelCode == 'H') {
            // var InvoiceAmount: any;
            this.Helper.WaitByIdAndClick('ATDS-Receivable'); //Generate Receivables 
            if (shipmentType == '') {
                this.amount1 = this.AddReceivables('Air Frei', '10', '10', 'USD');
                this.amount2 = this.AddReceivables('Order', '10', '20', 'USD');
            }
            else if (shipmentType == 'FCL' || shipmentType == 'LCL') {
                this.amount1 = this.AddReceivables('ocean', '10', '10', 'USD');
                this.amount2 = this.AddReceivables('Order', '10', '20', 'USD');
            }
            else {
                this.amount1 = this.AddReceivables('Inland', '10', '10', 'USD');
                this.amount2 = this.AddReceivables('Order', '10', '20', 'USD');
            }
            // expect(element(by.id('ReceivableAmount')).getAttribute('textContent')).toBe(this.amount1 + this.amount2);
            // this.AddARInvoice();
        }
        else if (shipmentType == 'M') {
            // this.Helper.WaitByIdAndFill('Shipment_EstimateProfitInSelectedCurrency', '44');
            this.AddReceivables('A', '5', '10', 'USD');
            //this.AddReceivables('Order', '10', '20');
            // InvoiceAmount = this.amount1 + this.amount2
            // this.AddARInvoice();
        }
    };
    ReceivablesTabComponent.prototype.AddReceivables = function (ChargeType, quantity, unitPrice, currency) {
        var amount = 0;
        var receivableCurrency = '';
        protractor_1.element(protractor_1.by.id('ProfitCurrency')).getAttribute('textContent').then(function (recCurrency) {
            receivableCurrency = recCurrency.trim();
            if (receivableCurrency != null) {
                console.log('Receivable  Profit Currency-ReceivableTab : ' + receivableCurrency);
            }
            else {
                console.log('No Profit Currency in Receiavble Tab');
            }
            // return receivableCurrency;
        });
        this.Helper.WaitByIdAndClick('AddReceivable');
        this.Helper.WaitByIdAndFill('ShipmentReceivable_ChargesTypeId', ChargeType);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_MeasurementId', 'Gross');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_Quantity', quantity);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_UnitPrice', unitPrice);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_CurrencyId', currency);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        var EC = protractor_1.protractor.ExpectedConditions;
        // browser.wait(EC.elementToBeClickable(element(by.id('ShipmentReceivable_Rate'))), 20000).then(a => {
        //     // console.log('inside the Exchange Rate Feild ');
        // });
        // browser.driver.sleep(5000);
        // this.Helper.WaitByIdAndFill('ShipmentReceivable_Notes', 'Note :' + receivableCurrency);
        // browser.driver.sleep(6000);
        // this.Helper.WaitByIdAndFill('ShipmentReceivable_Notes', 'Add Receivable');
        var receivableAmount = '';
        var localAmount = '';
        var profitAmount = '';
        protractor_1.element(protractor_1.by.id('ShipmentReceivable_Rate')).getAttribute('value').then(function (divText) {
            if (divText != '') {
                console.log('Exchange Rate : ' + divText);
            }
            else {
                protractor_1.element(protractor_1.by.id('ShipmentReceivable_Rate')).sendKeys('4');
                // this.Helper.WaitByIdAndFill('ShipmentReceivable_Rate', '4');
                console.log('Exchange Rate is Changed : 4 ');
            }
        });
        var lineAmount;
        lineAmount = quantity * unitPrice;
        expect(protractor_1.element(protractor_1.by.id('ShipmentReceivable_TotalAmount')).getAttribute('value')).toBe(lineAmount + '.00');
        protractor_1.element(protractor_1.by.id('ShipmentReceivable_TotalAmount')).getAttribute('value').then(function (recAmount) {
            receivableAmount = recAmount;
            console.log('ReceivableAmount : ' + receivableAmount);
        });
        protractor_1.element(protractor_1.by.id('ShipmentReceivable_TotalAmountLocal')).getAttribute('value').then(function (locAmount) {
            localAmount = locAmount;
            console.log('LocalAmount : ' + localAmount);
            if (receivableCurrency != currency) {
                protractor_1.element(protractor_1.by.id('ShipmentReceivable_AmountInProfitCurrency')).getAttribute('value').then(function (profAmount) {
                    profitAmount = profAmount;
                    console.log('ProfitAmount : ' + profitAmount);
                });
            }
            else {
                console.log('There is no profit Amount');
            }
        });
        // element(by.id('ShipmentReceivable_AmountInProfitCurrency')).getAttribute('value').then(function (profAmount) {
        //     profitAmount = profAmount;
        //     console.log('ProfitAmount : ' + profitAmount);
        // });
        // expect(element(by.id('ShipmentReceivable_Notes')).getAttribute('value')).toBe('razan');
        this.Helper.WaitByIdAndClick('Ok-AddReceivableBtn');
        return lineAmount;
    };
    ReceivablesTabComponent.prototype.AddARInvoice = function () {
        this.Helper.WaitByIdAndClick('CreateARInvoice');
        this.Helper.WaitByIdAndFill('ARInvoice_InvoiceCurrencyId', 'EUR');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('date_ARInvoice_InvoiceDate', '.');
        this.Helper.WaitByIdAndFill('ARInvoice_PaymentTermId', 'cash');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('ARInvoice_VatNumber', 'Vat Number ');
        this.Helper.WaitByIdAndClick('Ok-CreateARInvoice');
        this.Helper.WaitByIdAndFill('ARInvoice_VatTypeId', 'zero');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByCssButtonClick('.Button', 'Apply to all');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ARInvoice.TH.General');
        // this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoice_MasterNumber', 'Master #');
        this.Helper.WaitByIdAndClick('ARInvoice.TH.ARPayments');
        // this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ARInvoice.B.SaveAsDraft');
        // this.Helper.WaitBusyIndicator();
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.visibilityOf(protractor_1.element(protractor_1.by.css('.BusyIndicatorControl'))), 20000).then(function (a) {
            protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.css('.BusyIndicatorControl')))).then(function (a) {
                protractor_1.element(protractor_1.by.id('ARInvoice.B.Approve')).click();
            });
            // element(by.id('ARInvoice.B.Approve')).click();
        });
        // this.Helper.WaitByIdAndClick('ARInvoice.TH.General');
        // this.Helper.WaitByIdAndClick('ARInvoice_PrintNotes');
        // browser.driver.sleep(4000);
        // this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');
        // this.Helper.WaitBusyIndicator();
        // browser.driver.sleep(4000);
        //  this.Helper.WaitByCssButtonClick('.Button','New Payment');
        //  this.Helper.WaitByIdAndFill('ARPayment_AmountInPaymentCurrency','3000');
        // this.Helper.WaitByIdAndClick('EditBackbutton_1');
        protractor_1.browser.driver.sleep(5000);
    };
    return ReceivablesTabComponent;
}());
exports.ReceivablesTabComponent = ReceivablesTabComponent;
//# sourceMappingURL=RecievablesTab.js.map