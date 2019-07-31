import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { protractor, WebDriver, browser, by, element, $ } from 'protractor';

export class NewARInvoice {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    CreateNewARInvoice(BillToName: string) {
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewInvoice');
        //  this.Helper.WaitByIdAndClick('NewGeneralARInvoice');
        //this.Helper.WaitByCssButtonClick('.ToggleButtonMenu','New Invoice');
        this.Helper.WaitByIdAndClick('NewGeneralInvoice');
        this.Helper.WaitByIdAndFill('ARInvoice_BillToId', BillToName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoice_CustomerRef', 'Refrence');
        //browser.sleep(5000);
        this.Helper.WaitByIdAndFill('ARInvoice_InvoiceCurrencyId', 'NIS');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoice_VatNumber', '0');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitBusyIndicator(); // closed month
        this.Helper.WaitBusyIndicator(); // currency rates
        this.Helper.WaitByIdAndClick('ok-addArInvoice');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
       // this.Helper.WaitBusyIndicator();
        this.Helper.WaitBusyIndicatorToShow();
        this.Helper.WaitBusyIndicator();


        // var EC = protractor.ExpectedConditions;
        // Waits for the element with id 'abc' to be clickable.
        this.Helper.WaitByIdAndClick('Add');
        //  browser.wait(EC.elementToBeClickable($('#Add')), 1000000).then(a => {
        //   this.Helper.WaitByIdAndClick('Add');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARInvoiceLine_ChargesTypeId', 'Air Freight');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('ARInvoiceLine_VatTypeId', 'Zero');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('ARInvoiceLine_Quantity', '5');
        this.Helper.WaitByIdAndFill('ARInvoiceLine_UnitPrice', '10');
        this.Helper.WaitByIdAndClick('ok-addArInvoiceline');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');
        
        //this.Helper.WaitBusyIndicatorToShow();
        this.Helper.WaitBusyIndicator();
        //this.Helper.CheckIfChanged('ARInvoiceHeaderStatusName', 'Unpaid');
     //   this.Helper.WaitByIdAndCheckText('ARInvoiceHeaderStatusName', 'Unpaid');
       
       // browser.sleep(6000);

       // browser.sleep(9000);
       // this.Helper.WaitByIdAndClick('EditBackbutton');

       // console.log('Approved00');
       // browser.sleep(6000);
    }
}
        //  });

        //  });
        // this.Helper
        //this.Helper.WaitBusyIndicator();
        // this.Helper.WaitByIdAndClick('ARInvoice.B.Approve');
        //browser.driver.sleep(5000);
        // this.Helper.ItemsVisibility('Add');

        // this.Helper.WaitBusyIndicator();
        // this.Helper.WaitByIdAndCheckText('ARInvoiceHeaderStatusName', 'Draft');
        // console.log('Invoice Is Draft Now');
        //browser.sleep(10000);


        //this.Helper.WaitBusyIndicator()
        //    // this.Helper.
        //    .then(a => {
        //        browser.sleep(5000);

        //        this.Helper.WaitByIdAndCheckText('ARInvoiceHeaderStatusName', 'Unpaid');

                //console.log('Invoice Is Approved and Unpdaid Now');
                //  browser.sleep(9000);

            
        // browser.sleep(9000);
        // this.Helper.WaitByIdAndCheckText('ARInvoiceHeaderStatusName', 'Unpaid');

        // console.log('Invoice Is Approved and Unpdaid Now');


        // this.Helper.WaitByIdAndClick('EditBackbutton');

     //   browser.sleep(5000);
    


