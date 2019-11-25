import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { protractor, WebDriver, browser, by, element, $ } from 'protractor';

export class NewARInvoice {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    CreateNewARInvoice(BillToName: string) {
        browser.ignoreSynchronization = true;
        this.Helper.ItemsVisibility('NewInvoice');
        this.Helper.ItemsPresent('NewInvoice');
        this.Helper.WaitByIdAndClick('NewInvoice');

        //  this.Helper.WaitByIdAndClick('NewGeneralARInvoice');
        //this.Helper.WaitByCssButtonClick('.ToggleButtonMenu','New Invoice');
        this.Helper.WaitByIdAndClick('NewGeneralInvoice');
        this.Helper.WaitBusyIndicator();

        this.Helper.WaitByIdAndFill('ARInvoice_BillToId', BillToName);
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoice_BillToId', BillToName);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoice_CustomerRef', 'Refrence');
        browser.sleep(5000);
        this.Helper.WaitByIdAndFill('ARInvoice_InvoiceCurrencyId', 'NIS');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoice_InvoiceCurrencyId', 'NIS');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoice_VatNumber', '0');
        this.Helper.WaitBusyIndicator();
       // this.Helper.WaitBusyIndicator(); // closed month
       // this.Helper.WaitBusyIndicator(); // currency rates
        this.Helper.ItemsVisibility('ok-addArInvoice');
        this.Helper.ItemsPresent('ok-addArInvoice');
        this.Helper.WaitByIdAndClick('ok-addArInvoice');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
        // this.Helper.WaitBusyIndicator();
        //this.Helper.WaitBusyIndicatorToShow();
        //this.Helper.WaitBusyIndicator();
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('Add');
        this.Helper.WaitBusyIndicator();
        //this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndFill('ARInvoiceLine_ChargesTypeId', 'Air Freight');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoiceLine_ChargesTypeId', 'Air Freight');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoiceLine_VatTypeId', 'Zero');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoiceLine_VatTypeId', 'Zero');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoiceLine_Quantity', '5');
        this.Helper.WaitByIdAndFill('ARInvoiceLine_UnitPrice', '10');
        this.Helper.WaitByIdAndClick('ok-addArInvoiceline');
        this.Helper.WaitWindowClosed();
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('ARInvoice.B.SaveAsDraft');
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');


        this.WaitBusyIndicatorToShowandHide();



    }
    WaitBusyIndicatorToShowandHide() {
        this.Helper.WaitShowEditComponentBusyIndicator();
        this.Helper.WaitEditComponentBusyIndicator();
    }
}
