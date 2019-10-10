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

        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('Shipment.TH.Payables');
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.ItemsVisibility('ATDSPayable-payable');

        this.AddPayableLines(shipperRef1,ShipmentType);
        this.CreatAPInvoice(shipperRef1);
        this.EditAPInvoice(shipperRef1);



    }

    AddPayableLines(shipperRef1: string, ShipmentType: string){
        if (ShipmentType == '') {
            this.AddPayables('Air Frei', '10', '10','Shipment');
            this.AddPayables('Order', '10', '20','Shipment');

        } else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
            this.AddPayables('ocean', '10', '10','Shipment');
            this.AddPayables('Order', '10', '20','Shipment');
        } else {
            this.AddPayables('Inland', '10', '10','Shipment');
            this.AddPayables('Order', '10', '20','Shipment');
        }
    }
    AddPayables(ChargeType: string, quantity: any, unitPrice: any, Source :String) {
       // var amount: any = 0;

       if(Source == "Shipment"){
        this.Helper.WaitByIdAndClick('AddPayable');
        
        this.Helper.WaitByIdAndFill('ShipmentPayable_ChargesTypeId', ChargeType);
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPayable_ChargesTypeId', ChargeType);

        this.Helper.WaitByIdAndFill('ShipmentPayable_Quantity', quantity);
        this.Helper.WaitByIdAndFill('ShipmentPayable_UnitPrice', unitPrice);
        //amount = parseInt(quantity) * parseInt(unitPrice);

        this.Helper.WaitByIdAndFill('ShipmentPayable_CurrencyId', 'EU');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPayable_CurrencyId', 'EU');

        this.Helper.WaitByIdAndClick('Ok-AddPayableBtn');
    }
    else {
        this.Helper.WaitByIdAndClick('AddInvoiceLine');
        this.Helper.WaitByIdAndFill('APInvoiceLine_ChargesTypeId', 'order');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoiceLine_ChargesTypeId', 'order');
        this.Helper.WaitByIdAndFill('APInvoiceLine_VatTypeId','zero');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoiceLine_VatTypeId','zero');
        this.Helper.WaitByIdAndFill('APInvoiceLine_InvoiceCurrencyAmount_2','100');
        this.Helper.WaitByIdAndClick('Ok-AddInvoiceLine');
    }
        // return amount;
    }
    CreatAPInvoice(shipperRef1: string) {
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ReceiveInvoice');


        this.Helper.WaitByIdAndFill('APInvoice_VendorId', 'TestAgentExport1');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_VendorId', 'TestAgentExport1');

        this.Helper.WaitByIdAndFill('APInvoice_InvoiceNumber', shipperRef1);
        // this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency', Amount);

        this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency', '100');
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceCurrencyId', 'EUR');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_InvoiceCurrencyId', 'EUR');

        this.Helper.WaitByIdAndFill('date_APInvoice_InvoiceDate', '.');

        this.Helper.WaitByIdAndFill('APInvoice_PaymentTermId', 'cash');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_PaymentTermId', 'cash');

        this.Helper.WaitByIdAndFill('APInvoice_VATNumber', 'Vat Number ');
        this.Helper.WaitByIdAndClick('Ok-CreateAPInvoice');
        this.Helper.WaitBusyIndicator();
    }

        EditAPInvoice(shipperRef1: string){
        this.Helper.WaitByIdAndFill('APInvoice_VatTypeId', 'Zero');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_VatTypeId', 'Zero');
        this.Helper.WaitByIdAndClick('VATApplyToAll');

        this.Helper.WaitByIdAndFill('APInvoiceLine_InvoiceCurrencyAmount', '100');

        this.AddPayables('Order', '10', '20','Invoice');

        /**/
        this.Helper.WaitByIdAndClick('CheckBox_0_5_LBL');
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndFill('APInvoice_VATNumber','Lana');
        this.Helper.WaitByIdAndClick('APInvoice.B.Save');
        this.Helper.WaitShowEditComponentBusyIndicator();

        this.Helper.WaitEditComponentBusyIndicator();
        
        
        this.Helper.WaitByIdAndClick('APInvoice.B.Approve');
        this.WaitBusyIndicatorToShowandHide();
        this.WaitBusyIndicatorToShowandHide();        
        

        this.Helper.WaitByIdAndClick('MenuButtons_1');
        this.Helper.WaitByIdAndClick('APInvoice.B.CancelApproval');
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('MenuButtons_1');
        this.Helper.WaitByIdAndClick('APInvoice.B.Void');
        this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
        this.WaitBusyIndicatorToShowandHide();
        

        this.Helper.WaitByIdAndClick('APInvoice.TH.General');

        this.Helper.WaitByIdAndClick('EditBackbutton_1');
        this.WaitBusyIndicatorToShowandHide();
        // this.Helper.WaitBusyIndicator();
        
    }
    WaitBusyIndicatorToShowandHide(){
    this.Helper.WaitShowEditComponentBusyIndicator();
    this.Helper.WaitEditComponentBusyIndicator();
   }
    
}  
