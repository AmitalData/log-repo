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
    public RecievablesTab(ShipmentLevelCode: string, shipmentType: string) {

        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('Shipment.TH.Receivables');
        this.Helper.WaitEditComponentBusyIndicator();

        this.Helper.ItemsVisibility('ATDS-Receivable');
        this.AddRecievableLines(ShipmentLevelCode, shipmentType);
        
        this.CreatARInvoicewithVoid(true);
        

        

       
    }

    AddRecievableLines(ShipmentLevelCode: string, ShipmentType: string){
        if (ShipmentLevelCode == 'D' || ShipmentLevelCode == 'H') {
        
            if (ShipmentType == '') {
                this.amount1 = this.AddReceivables('Air Frei', '10', '10', 'USD');
                this.amount2 = this.AddReceivables('Order', '10', '20', 'USD');
            } else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
                this.amount1 = this.AddReceivables('ocean', '10', '10', 'USD');
                this.amount2 = this.AddReceivables('Order', '10', '20', 'USD');
            } else {
                this.amount1 = this.AddReceivables('Inland', '10', '10', 'USD');
                this.amount2 = this.AddReceivables('Order', '10', '20', 'USD');
            }
           
        }
        else if (ShipmentType == 'M') {
        
            this.AddReceivables('A', '5', '10', 'USD');
          
        }
        this.Helper.WaitByIdAndClick('Shipment-Save');
        this.WaitBusyIndicatorToShowandHide();
        
    }
    AddReceivables(ChargeType: string, quantity: any, unitPrice: any, currency: any) {
     
        this.Helper.WaitEditComponentBusyIndicator();

        this.Helper.WaitByIdAndClick('Add');

        this.Helper.WaitByIdAndFill('ShipmentReceivable_ChargesTypeId', ChargeType);
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentReceivable_ChargesTypeId', ChargeType);

        this.Helper.WaitByIdAndFill('ShipmentReceivable_MeasurementId', 'fixed');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentReceivable_MeasurementId', 'fixed');

        this.Helper.WaitByIdAndFill('ShipmentReceivable_Quantity', quantity);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_UnitPrice', unitPrice);

        this.Helper.WaitByIdAndClick('Ok-AddReceivableBtn');
        
        
       
    }

    CreatARInvoicewithVoid(Voided :boolean) {
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('CreateARInvoice');
        this.Helper.WaitByIdAndFill('ARInvoice_PaymentTermId','cash');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoice_PaymentTermId','cash');
        var TodayDate=new Date().getDate();
        console.log(TodayDate);
        this.Helper.WaitByIdAndFill('date_ARInvoice_DueDate',TodayDate.toString() );
        
       
        this.Helper.WaitByIdAndClick('Ok-CreateARInvoice');

        this.Helper.WaitBusyIndicator();
    }
    EditAPInvoice(Voided :boolean){
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ARInvoice.B.SaveAsDraft');
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');
        this.WaitBusyIndicatorToShowandHide();
        this.WaitBusyIndicatorToShowandHide();
        
       
    }

    WaitBusyIndicatorToShowandHide(){
        this.Helper.WaitShowEditComponentBusyIndicator();
        this.Helper.WaitEditComponentBusyIndicator();
       }
}
