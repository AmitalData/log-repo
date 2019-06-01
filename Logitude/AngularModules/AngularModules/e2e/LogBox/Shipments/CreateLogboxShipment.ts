import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from './../Helpers/FieldsHelper';
import { GeneralFunctions } from './../Helpers/GeneralFunctions';

export class LogboxShipment {
    private helper: FieldsHelper;
    private GeneralFun: GeneralFunctions;

    constructor() {
        this.helper = new FieldsHelper();
        this.GeneralFun = new GeneralFunctions();
    }


     ClickNewShipment() {

        
         this.helper.WaitByIdAndClick('LogBoxNEWSHIP');
        }

     ChooseTransportMode() {
       
        this.helper.WaitByIdAndFill('TransportModeId', 'Air');
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }

     InsertOrderNumber() {

        var orderNumber = this.GeneralFun.RandomNum();
        this.helper.WaitByIdAndFill('CustomerReference1', orderNumber);
        }


     InsertAgent() {
         //this.helper.WaitByIdAndFill('ForwarderPartnerId', 'Test Env 5.6');
         this.helper.WaitByIdAndFill('ForwarderPartnerId', 'Ahmad');
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

         }


     SaveShipment() {

        this.helper.WaitByIdAndClick('OKButton');
         this.helper.WaitBusyIndicator();
         browser.sleep(4000);
        }


   // SearchForCreatedShipment(searchFeildId: string, orderNumber:string) {

   //     this.helper.WaitByIdAndFill('SearchFieldsId_0_0', orderNumber);
   //  //this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem',0);
   //}


    //SearchForCreatedShipment() {
    //    this.helper.WaitByIdAndFill('SearchFieldsId_0_0', '1009');
    //    browser.sleep(4000);
    //    }

    SelectShipment() {
        this.helper.WaitByIdAndClick('LogGrid_0_0row0');
        this.helper.WaitBusyIndicator();
    }

    CreateDocument() {
        this.helper.WaitByIdAndClick('AddLogboxDocument');
        this.helper.WaitByIdAndClick('DocumentTypeCode');
        this.helper.WaitByIdAndFill('Description', 'Test Document');
        this.helper.WaitByIdAndFill('Notes', 'Logbox test scenario');
        this.helper.WaitByIdAndClick('OK');
        this.helper.WaitBusyIndicator();
        browser.sleep(4000);

    }



    //////--------------------------------------------------------------


//    InsertGetway() {

//        // this.helper.WaitByCssAndClick_FromTagInsideList('.ToggleButton', 0)

//        this.helper.WaitByIdAndClick('SaveExcelToggleButton');
//        //save as execl file 
//        this.helper.WaitByIdAndClick('SaveExcelFileButton');


//    }


//    FillSupplierName() {

//        this.helper.WaitByIdAndClick('RunUnpaidInvoicesReportbtn');
//    }



//    FillMyReference() {

//        this.helper.WaitByIdAndClick('RunUnpaidInvoicesReportbtn');
//    }

//    FillDestination() {

//        this.helper.WaitByIdAndClick('RunUnpaidInvoicesReportbtn');
//    }

}

//this.Helper.WaitWindowClosed();
