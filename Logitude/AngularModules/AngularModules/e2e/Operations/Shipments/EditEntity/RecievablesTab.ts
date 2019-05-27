import { browser, by, element, WebDriver, protractor, ExpectedConditions } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class ReceivablesTabComponent {
    private Helper: FieldsHelper;
    private amount1: any;
    private amount2: any;
    private receivableCurrency = '';
    constructor() {
        this.Helper = new FieldsHelper();
    }


    public RecievablesTab(shipmentType: string) {
        this.Helper.WaitByIdAndClick('Shipment.TH.Receivables');
        this.Helper.WaitBusyIndicator();
        // var EC = protractor.ExpectedConditions;
        // this.Helper.WaitByIdAndClick('Shipment_EstimateProfitInSelectedCurrency');

        if (shipmentType == 'D' || shipmentType == 'H') {
            // this.Helper.WaitByIdAndClick('ATDS-Receivable');
            // var InvoiceAmount: any;
            this.amount1 = this.AddReceivables('Air Frei', '10', '10', 'USD');
            this.amount2 = this.AddReceivables('Order', '10', '20', 'USD');

            expect(element(by.id('ReceivableAmount')).getAttribute('textContent')).toBe(this.amount1 + this.amount2);

            // this.AddReceivables('Air Frei', '10', '10', 'USD');
            // this.AddReceivables('Order', '10', '20','EUR');
            // InvoiceAmount = this.amount1 + this.amount2

            this.Helper.WaitByIdAndClick('Shipment-Save');
            this.Helper.WaitBusyIndicator();
            // this.AddARInvoice();
        }
        else if (shipmentType == 'M') {
            // this.Helper.WaitByIdAndFill('Shipment_EstimateProfitInSelectedCurrency', '44');

            this.AddReceivables('A', '5', '10', 'USD');
            // this.AddReceivables('Order', '10', '20');
            // InvoiceAmount = this.amount1 + this.amount2
            this.AddARInvoice();
        }
    }

    AddReceivables(ChargeType: string, quantity: any, unitPrice: any, currency: any) {
        var amount: any = 0;
        var receivableCurrency = '';
        // var testcurr = element(by.id('ProfitCurrency')).getAttribute('textContent').then(function (recCurrency) {
         element(by.id('ProfitCurrency')).getAttribute('textContent').then(function (recCurrency) {            
            receivableCurrency = recCurrency.trim();
            if (receivableCurrency != null) {
                console.log('Receivable  Profit Currency-ReceivableTab : ' + receivableCurrency);
            } else {

                console.log('No Profit Currency in Receiavble Tab');
            }
            // return receivableCurrency;
        });

        this.Helper.WaitByIdAndClick('AddReceivableId');

        this.Helper.WaitByIdAndFill('ShipmentReceivable_ChargesTypeId', ChargeType);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('ShipmentReceivable_MeasurementId', 'Gross');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('ShipmentReceivable_Quantity', quantity);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_UnitPrice', unitPrice);

        this.Helper.WaitByIdAndFill('ShipmentReceivable_CurrencyId', currency);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var EC = protractor.ExpectedConditions;
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


        element(by.id('ShipmentReceivable_Rate')).getAttribute('value').then(function (divText) {
            if (divText != '') {
                console.log('Exchange Rate : ' + divText);
            } else {
                element(by.id('ShipmentReceivable_Rate')).sendKeys('4');
                // this.Helper.WaitByIdAndFill('ShipmentReceivable_Rate', '4');
                console.log('Exchange Rate is Changed : 4 ');
            }
        });
        var lineAmount ;
        lineAmount =  quantity * unitPrice;
        expect(element(by.id('ShipmentReceivable_TotalAmount')).getAttribute('value')).toBe(lineAmount+'.00');

        element(by.id('ShipmentReceivable_TotalAmount')).getAttribute('value').then(function (recAmount) {
            receivableAmount = recAmount;
            console.log('ReceivableAmount : ' + receivableAmount);
        });
        element(by.id('ShipmentReceivable_TotalAmountLocal')).getAttribute('value').then(function (locAmount) {
            localAmount = locAmount;
            console.log('LocalAmount : ' + localAmount);

            if (receivableCurrency != currency) {
                element(by.id('ShipmentReceivable_AmountInProfitCurrency')).getAttribute('value').then(function (profAmount) {
                    profitAmount = profAmount;
                    console.log('ProfitAmount : ' + profitAmount);
                });
            } else {
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

    }

    AddARInvoice() {
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
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.css('.BusyIndicatorControl'))), 20000).then(a => {
            browser.wait(EC.invisibilityOf(element(by.css('.BusyIndicatorControl')))).then(a => {
                element(by.id('ARInvoice.B.Approve')).click();
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
        browser.driver.sleep(5000);

    }

}