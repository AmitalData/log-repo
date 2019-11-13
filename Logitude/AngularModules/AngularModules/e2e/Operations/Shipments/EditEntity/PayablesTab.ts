import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class PayablesTabComponent {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }
    public PayablesTab(shipperRef1: string, ShipmentType: string, isFromAccounting: boolean) {
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('Shipment.TH.Payables');
        this.Helper.WaitEditComponentBusyIndicator();

        this.Helper.ItemsVisibility('ATDSPayable-payable');
        if (isFromAccounting == false) {
            this.AddPayableLines(shipperRef1, ShipmentType);
            this.CreatAPInvoicewithVoid(shipperRef1, true);
            this.EditAPInvoice(shipperRef1, true, isFromAccounting);
            this.CreatAPInvoicewithVoid(shipperRef1 + '1L1', false);
            this.EditAPInvoice(shipperRef1 + '1L1', false, isFromAccounting);
        } else {
            this.AddPayableLines(shipperRef1, ShipmentType);
            this.CreatAPInvoicewithVoid(shipperRef1, false);
            this.EditAPInvoice(shipperRef1 + '1L1', false, isFromAccounting);
        }

    }
    AddPayableLines(shipperRef1: string, ShipmentType: string) {
        if (ShipmentType == '') {
            this.AddPayables('Air Frei', '10', '10', 'Shipment');
            //this.AddPayables('Order', '0', '0', 'Shipment');

        } else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
            this.AddPayables('ocean', '10', '10', 'Shipment');
            //this.AddPayables('Order', '0', '0', 'Shipment');
        } else {
            this.AddPayables('Inland', '10', '10', 'Shipment');
            //this.AddPayables('Order', '0', '0', 'Shipment');
        }

        this.Helper.WaitByIdAndClick('Shipment-Save');
        this.WaitBusyIndicatorToShowandHide();
    }
    AddPayables(ChargeType: string, quantity: any, unitPrice: any, Source: String) {
        this.Helper.WaitEditComponentBusyIndicator();
        if (Source == "Shipment") {
            this.Helper.WaitByIdAndClick('Add');

            this.Helper.WaitByIdAndFill('ShipmentPayable_ChargesTypeId', ChargeType);
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPayable_ChargesTypeId', ChargeType);

            this.Helper.WaitByIdAndFill('ShipmentPayable_MeasurementId', 'fixed')
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPayable_MeasurementId', 'fixed');

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
            this.Helper.WaitByIdAndFill('APInvoiceLine_VatTypeId', 'zero');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoiceLine_VatTypeId', 'zero');

            this.Helper.WaitByIdAndFill('APInvoiceLine_InvoiceCurrencyAmount_1', '100');
            this.Helper.WaitByIdAndClick('Ok-AddInvoiceLine');
        }
    }
    CreatAPInvoicewithVoid(shipperRef1: string, Voided: boolean) {
        this.Helper.WaitByIdAndClick('ReceiveInvoice');
        this.Helper.WaitEditComponentBusyIndicator();

        this.Helper.WaitByIdAndFill('APInvoice_VendorId', 'TestAgentExport1');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_VendorId', 'TestAgentExport1');

        this.Helper.WaitByIdAndFill('APInvoice_InvoiceNumber', shipperRef1);
        // this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency', Amount);

        this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency', '100');
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceCurrencyId', 'EUR');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_InvoiceCurrencyId', 'EUR');
        var TodayDate = new Date().getDate();
        console.log(TodayDate);
        this.Helper.WaitByIdAndFill('date_APInvoice_InvoiceDate', TodayDate.toString());

        this.Helper.WaitByIdAndFill('APInvoice_PaymentTermId', 'cash');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_PaymentTermId', 'cash');

        this.Helper.WaitByIdAndFill('APInvoice_VATNumber', 'Vat Number ');
        this.Helper.WaitByIdAndClick('Ok-CreateAPInvoice');
        this.Helper.WaitBusyIndicator();
    }
    EditAPInvoice(shipperRef1: string, Voided: boolean, isFromAccounting: boolean) {
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndFill('APInvoice_VatTypeId', 'Zero');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'APInvoice_VatTypeId', 'Zero');
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('VATApplyToAll');

        this.Helper.WaitByIdAndFill('APInvoiceLine_InvoiceCurrencyAmount', '100');
        if (Voided == true) {
            this.AddPayables('Order', '10', '20', 'Invoice');
            /**/
            this.Helper.WaitByIdAndClick('CheckBox_0_4_LBL');
            this.Helper.WaitEditComponentBusyIndicator();
            this.Helper.WaitByIdAndFill('APInvoice_VATNumber', 'Lana');
        }
        this.Helper.WaitByIdAndClick('APInvoice.B.Save');
        this.WaitBusyIndicatorToShowandHide();

        this.Helper.WaitByIdAndClick('APInvoice.B.Approve');
        this.WaitBusyIndicatorToShowandHide();
        if (isFromAccounting == false) {
            this.WaitBusyIndicatorToShowandHide();
            if (Voided == true) {
                this.Helper.WaitByIdAndClick('MenuButtons_1');
                this.Helper.WaitByIdAndClick('APInvoice.B.CancelApproval');
                this.WaitBusyIndicatorToShowandHide();

                this.Helper.WaitByIdAndClick('MenuButtons_1');
                this.Helper.WaitByIdAndClick('APInvoice.B.Void');
                this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
                this.Helper.WaitEditComponentBusyIndicator();
                this.Helper.WaitByIdAndClick('APInvoice.TH.General');
                this.Helper.WaitByIdAndClick('EditBackbutton_1');
            }
            else {
                //this.Helper.WaitByIdAndClick('APInvoice.TH.APPayments');
                //this.Helper.WaitByIdAndClick('Connect');
                //this.Helper.WaitEditComponentBusyIndicator();

                this.Helper.WaitByIdAndClick('EditBackbutton_2');
                this.WaitBusyIndicatorToShowandHide();
                //this.Helper.WaitByIdAndClick('Delete_52');
                //this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
                //this.Helper.WaitByIdAndClick('Shipment-Save');
            }
            this.Helper.WaitEditComponentBusyIndicator();
        } else {//from accounting actions
            this.Helper.WaitByIdAndClick('EditBackbutton_1');
            this.WaitBusyIndicatorToShowandHide();
            this.Helper.WaitByIdAndClick('BackButton');
        }
    }
    WaitBusyIndicatorToShowandHide() {
        this.Helper.WaitShowEditComponentBusyIndicator();
        this.Helper.WaitEditComponentBusyIndicator();
    }
}
