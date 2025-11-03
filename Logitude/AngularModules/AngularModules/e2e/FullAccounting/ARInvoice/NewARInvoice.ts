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
        this.Helper.WaitByIdAndClick('NewGeneralInvoice');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoice_InvoiceCurrencyId', 'NIS');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoice_InvoiceCurrencyId', 'NIS');
        this.Helper.WaitBusyIndicator();

        this.Helper.WaitByIdAndFill('ARInvoice_BillToId', BillToName);
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoice_BillToId', BillToName);
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
       
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('Add');
        this.Helper.WaitBusyIndicator();
       
        this.Helper.WaitByIdAndFill('ARInvoiceLine_ChargesTypeId', 'Air Freight');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoiceLine_ChargesTypeId', 'Air Freight');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoiceLine_VatTypeId', 'Zero');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoiceLine_VatTypeId', 'Zero');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoiceLine_ForiegnCurrencyId', 'NIS');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARInvoiceLine_ForiegnCurrencyId', 'NIS');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoiceLine_Quantity', '5');
        this.Helper.WaitByIdAndFill('ARInvoiceLine_UnitPrice', '10');
        this.Helper.WaitByIdAndClick('ok-addArInvoiceline');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
       
        this.Helper.WaitByIdAndClick('ARInvoice.B.SaveAsDraft');
        
        this.WaitBusyIndicatorToShowandHide();
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');
        this.Helper.WaitBusyIndicator();
        
        // Wait 2 minutes for status to update
        browser.sleep(120000);
        
        // Verify status is "Unpaid"
        var EC = protractor.ExpectedConditions;
        this.Helper.ItemsPresent('ARInvoiceHeaderStatusName');
        this.Helper.ItemsVisibility('ARInvoiceHeaderStatusName');
        browser.wait(EC.textToBePresentInElement(element(by.id('ARInvoiceHeaderStatusName')), 'Unpaid'), 60000);
       



    }
    WaitBusyIndicatorToShowandHide() {
        this.Helper.WaitShowEditComponentBusyIndicator();
        this.Helper.WaitEditComponentBusyIndicator();
    }
}
