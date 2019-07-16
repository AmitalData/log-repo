import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';


export class PayablesTabComponent {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }
    private amount1: any;
    private amount2: any;


    public PayablesTab(shipperRef1: string, ShipmentType: string) {
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('Shipment.TH.Payables');
        this.Helper.WaitBusyIndicator();
        this.Helper.ItemsVisibility('ATDSPayable-payable');
        this.Helper.WaitByIdAndClick('ATDSPayable-payable');//Auto-display 

         //this.Helper.WaitByCssButtonClick('RedButton','Yes');
        //var InvoiceAmount: any;
        if (ShipmentType == '') {
            this.amount1 = this.AddPayables('Air Frei', '10', '10');
            this.amount2 = this.AddPayables('Order', '10', '20');
        } else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
            this.amount1 = this.AddPayables('ocean', '10', '10');
            this.amount2 = this.AddPayables('Order', '10', '20');
        } else {
            this.amount1 = this.AddPayables('Inland', '10', '10');
            this.amount2 = this.AddPayables('Order', '10', '20');
        }

        //InvoiceAmount = this.amount1 + this.amount2
        // this.AddAPInvoice(shipperRef1);
    }

    AddPayables(ChargeType: string, quantity: any, unitPrice: any) {

        var amount: any = 0;
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
    }
    AddAPInvoice(shipperRef1: string) {
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

        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id('APInvoice.B.Save'))), 20000).then(a => {
            element(by.id('APInvoice.B.Save')).click();
            // this.Helper.WaitBusyIndicator();

            browser.wait(EC.invisibilityOf(element(by.css('.busyindicatorcontrol'))), 100000).then(a => {
                // browser.wait(EC.elementToBeClickable(element(by.id('APInvoice.B.Approve'))), 100000).then(a => {

                this.Helper.WaitByIdAndClick('APInvoice.B.Approve');
            });
        });
        this.Helper.WaitBusyIndicator();
        browser.driver.sleep(30000);

        this.Helper.WaitByIdAndClick('EditBackbutton_1');
        this.Helper.WaitBusyIndicator();

    }

} 