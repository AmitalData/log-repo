import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';

export class ReportGenerator {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }



    /*  CheckBox() {
    
          browser.executeScript('arguments[1].click();', element(by.id('CreateDate')).getWebElement());
  
  
      }*/

    RunReportSuccessfully(expectedId) {
        this.helper.WaitByIdAndClick('CheckBox_0_0_LBL');
        this.helper.WaitByIdAndClick('RunReportButton');
       // this.helper.waitElementByIDPresence(expectedId);
        this.helper.ItemsPresent(expectedId);
        this.helper.WaitBusyIndicator();

       

    }

    RunReportFailed(expectedId) {
        this.helper.WaitByIdAndClick('CheckBox_0_0_LBL');
        this.helper.WaitByIdAndClick('RunReportButton');
        this.helper.WaitByIdAndClick('MessageWindow_Ok_0');
        this.helper.ItemsPresent(expectedId);
       // this.helper.waitElementByIDPresence(expectedId);
        this.helper.WaitBusyIndicator();




    }

    /* Partner() {
 
         this.helper.WaitByIdAndFill('CustomerId', 'Ahmad Comp.');
         this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
 
     }
 
     ChooseTemplate() {
         // this.helper.WaitByIdAndClick('Dropdown_0_3') ;
         // this.helper.WaitByCssAndClick_FromTagInsideList('.TextTrimming', 0);
 
         this.helper.WaitByIdAndClick('ReportsTemplatesComboBox');
         this.helper.WaitByIdAndClick('ComboBoxItem');
         this.helper.WaitBusyIndicator();
         //  THIS.helper.WaitByCssAndClick_FromTagInsideList
     }
 
     CalenderDate() {
 
         this.helper.WaitByIdAndFill('FromDate', '2000');
         this.helper.WaitByIdAndFill('ToDate', '25');
 
 
     }
     //should be after run report 
 
 
 
     runReport() {
 
         this.helper.WaitByIdAndClick('RunUnpaidInvoicesReportbtn');
         this.helper.WaitBusyIndicator();
     }
 
     saveReport() {
 
         // this.helper.WaitByCssAndClick_FromTagInsideList('.ToggleButton', 0)
 
         this.helper.WaitByIdAndClick('SaveExcelToggleButton');
         //save as execl file 
         this.helper.WaitByIdAndClick('SaveExcelFileButton');
 
 
     }
 
 }*/

}