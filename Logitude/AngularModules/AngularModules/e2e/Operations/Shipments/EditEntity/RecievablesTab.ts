import { browser, by, element, WebDriver, protractor, ExpectedConditions } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { Console } from '@angular/core/src/console';

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

        //this.Helper.ItemsVisibility('ATDS-Receivable');
        this.AddRecievableLines(ShipmentLevelCode, shipmentType);

        this.CreatARInvoicewithVoid(true, 'ARInvoice');
        this.EditARInvoice(true, 'ARInvoice');
        this.CreatARInvoicewithVoid(false, 'ARInvoice');
        this.EditARInvoice(false, 'ARInvoice');
        this.CreatARInvoicewithVoid(true, 'CreditNote');
        this.EditARInvoice(true, 'CreditNote');
        this.CreatARInvoicewithVoid(false, 'CreditNote');
        this.EditARInvoice(false, 'CreditNote');
    }

    AddRecievableLines(ShipmentLevelCode: string, ShipmentType: string) {
        if (ShipmentLevelCode == 'D' || ShipmentLevelCode == 'H') {

            if (ShipmentType == '') {
                this.amount1 = this.AddReceivables('Air Frei', '10', '10', 'USD', 'D');
                this.amount2 = this.AddReceivables('Order', '10', '-20', 'USD', 'D');
            } else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
                this.amount1 = this.AddReceivables('ocean', '10', '10', 'USD', 'D');
                this.amount2 = this.AddReceivables('Order', '10', '-20', 'USD', 'D');
            } else {
                this.amount1 = this.AddReceivables('Inland', '10', '10', 'USD', 'D');
                this.amount2 = this.AddReceivables('Order', '10', '-20', 'USD', 'D');
            }
        }
        else if (ShipmentLevelCode == 'M') {
            console.log('Inside ShipmentLevelCode if statement');

            if (ShipmentType == '') {
                console.log('Inside ShipmentLevelCode if statement');
                this.amount1 = this.AddReceivables('Air Frei', '10', '10', 'USD', 'M');
                this.amount2 = this.AddReceivables('Order', '10', '-20', 'USD', 'M');

            } else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
                this.amount1 = this.AddReceivables('ocean', '10', '10', 'USD', 'M');
                this.amount2 = this.AddReceivables('Order', '10', '-20', 'USD', 'M');

            } else {
                this.amount1 = this.AddReceivables('Inland', '10', '10', 'USD', 'M');
                this.amount2 = this.AddReceivables('Order', '10', '-20', 'USD', 'M');

            }

        }
        this.Helper.WaitByIdAndClick('Shipment-Save');
        this.WaitBusyIndicatorToShowandHide();
    }
    AddReceivables(ChargeType: string, quantity: any, unitPrice: any, currency: any, ShipmentLevelCode: string) {

        this.Helper.WaitEditComponentBusyIndicator();
        if (ShipmentLevelCode == 'M') {
            this.Helper.WaitByIdAndClick('Add_1');
        }
        else {
            this.Helper.WaitByIdAndClick('Add_5');
        }

        this.Helper.WaitByIdAndFill('ShipmentReceivable_ChargesTypeId', ChargeType);
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentReceivable_ChargesTypeId', ChargeType);

        this.Helper.WaitByIdAndFill('ShipmentReceivable_MeasurementId', 'fixed');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentReceivable_MeasurementId', 'fixed');

        this.Helper.WaitByIdAndFill('ShipmentReceivable_Quantity', quantity);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_UnitPrice', unitPrice);
        this.Helper.WaitByIdAndFill('ShipmentReceivable_Rate', '3');
        this.Helper.WaitByIdAndClick('Ok-AddReceivableBtn');
    }

    CreatARInvoicewithVoid(Voided: boolean, type: string) {
        this.Helper.WaitEditComponentBusyIndicator();
        if (type == "ARInvoice") {
            this.Helper.WaitByIdAndClick('CreateARInvoice');
        }
        else {
            this.Helper.WaitByIdAndClick('CreateCreditNote');
        }
        this.Helper.WaitByIdAndFill('ARInvoice_PaymentTermId', 'cash');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoice_PaymentTermId', 'cash');
        var TodayDate = new Date().getDate();
        console.log(TodayDate);
        this.Helper.WaitByIdAndFill('date_ARInvoice_DueDate', TodayDate.toString());

        this.Helper.WaitByIdAndClick('Ok-CreateARInvoice');

        this.Helper.WaitBusyIndicator();
    }
    //  CreateCreditNote(Voided :boolean){
    //    this.Helper.WaitEditComponentBusyIndicator();
    //    this.Helper.WaitByIdAndClick('CreateCreditNote');
    //}

    EditARInvoice(Voided: boolean, type: string) {
        this.Helper.WaitEditComponentBusyIndicator();
       /* this.Helper.WaitByIdAndFill('ARInvoice_VatTypeId', 'Zero');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoice_VatTypeId', 'Zero');
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('VATApplyToAll');*/
        //this.Helper.WaitEditComponentBusyIndicator();
        //this.Helper.WaitByIdAndFill('textboxdiv_ARInvoice_PrintNotes', 'Filled by Protractor');
        this.Helper.WaitByIdAndFill('ARInvoice_VatNumber', 'TestVatNumber');
        this.Helper.WaitByIdAndClick('ARInvoice.B.SaveAsDraft');
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');
        this.WaitBusyIndicatorToShowandHide();
        if (Voided == true) {
            if (type == 'ARInvoice')
                this.Helper.WaitByIdAndClick('MenuButtons_3');
            else
                this.Helper.WaitByIdAndClick('MenuButtons_5');
            this.Helper.WaitByIdAndClick('ARInvoice.B.Void');
            this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
            this.Helper.WaitEditComponentBusyIndicator();
            if (type == 'ARInvoice')
                this.Helper.WaitByIdAndClick('EditBackbutton_3');
            else
                this.Helper.WaitByIdAndClick('EditBackbutton_5');
        }
        else {
            if (type == 'ARInvoice')
                this.Helper.WaitByIdAndClick('EditBackbutton_4');
            else
                this.Helper.WaitByIdAndClick('EditBackbutton_6');
        }
        this.Helper.WaitEditComponentBusyIndicator();
    }
    WaitBusyIndicatorToShowandHide() {
        this.Helper.WaitShowEditComponentBusyIndicator();
        this.Helper.WaitEditComponentBusyIndicator();
    }
}
