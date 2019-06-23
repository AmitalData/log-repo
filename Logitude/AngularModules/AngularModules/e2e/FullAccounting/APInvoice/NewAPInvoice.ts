import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';




export class NewAPInvoice {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();

    constructor() {
    }
   
    CreateNewAPInvoice(VendorName : string,InvoiceNumer : string){
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewAPInvoice');
        this.Helper.WaitByIdAndFill('APInvoice_VendorId',VendorName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem',0);
        this.Helper.WaitBusyIndicator();
        //browser.sleep(5000);
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceNumber',InvoiceNumer);
        this.Helper.WaitByIdAndFill('APInvoice_AmountInInvoiceCurrency','1000');
        this.Helper.WaitByIdAndFill('APInvoice_InvoiceCurrencyId','NIS');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem',0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('date_APInvoice_InvoiceDate','10/06/2019');
        this.Helper.WaitByIdAndFill('date_APInvoice_DueDate','10/06/2019');
        this.Helper.WaitByIdAndFill('APInvoice_VATNumber','123456789');
        this.Helper.WaitByIdAndClick('Ok-AddAPInvoice');
        this.Helper.WaitWindowClosed();

        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('AddInvoiceLine');
        this.Helper.WaitByIdAndFill('APInvoiceLine_ChargesTypeId','Air Freight');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('APInvoiceLine_InvoiceCurrencyAmount','1000');
        this.Helper.WaitByIdAndClick('Ok-AddAPInvoiceLine');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        //browser.sleep(6000);

       this.Helper.WaitByIdAndClick('APInvoice.B.Approve');
       this.Helper.WaitBusyIndicator();
       browser.sleep(10000);
       

       //this.Helper.WaitByIdAndClick('EditBackbutton');
       



    }

}