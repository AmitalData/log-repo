import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from './../Helpers/FieldsHelper';

export class LogboxShipment {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }



    ClickNewShipment() {

        
        this.helper.WaitByIdAndClick('LogBoxNEWSHIP');
        
    }

    ChooseTransportMode() {
        // this.helper.WaitByIdAndClick('Dropdown_0_3') ;
        // this.helper.WaitByCssAndClick_FromTagInsideList('.TextTrimming', 0);
        this.helper.WaitByIdAndFill('TransportModeId', 'Air');
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

       // this.helper.WaitByIdAndClick('ComboBoxItem');
        //this.helper.WaitBusyIndicator();
        //  THIS.helper.WaitByCssAndClick_FromTagInsideList
    }

    InsertOrderNumber() {

        this.helper.WaitByIdAndFill('CustomerReference1', '123456');
        //this.helper.WaitByIdAndFill('ToDate', '25');


    }
    //should be after run report 



    InsertAgent() {
        this.helper.WaitByIdAndFill('ForwarderPartnerId', 'Simplog LTD AH Baker');
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        //this.helper.WaitByIdAndClick('RunUnpaidInvoicesReportbtn');
    }


    SaveShipment() {

        this.helper.WaitByIdAndClick('OKButton');

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
