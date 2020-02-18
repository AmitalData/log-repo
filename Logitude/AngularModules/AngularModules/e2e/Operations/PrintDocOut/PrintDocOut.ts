import { browser, by, element, WebDriver, protractor } from 'protractor';

import { FieldsHelper } from '../../Helpers/FieldsHelper';

export class PrintDocOut {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    isPrintingCompleted(expectedId, closePopup) {
        this.helper.WaitBusyIndicator();
       this.helper.WaitEditComponentBusyIndicator();
        
        if (closePopup) {
        //   this.helper.ItemsPresent(expectedId);
          this.helper.WaitByCssStringAndClick('.Button', 'Close'); // Edit Component
          //  this.helper.WaitByIdAndClick('closeButtonId')

           this.helper.WaitBusyIndicator(); // Logiude Window
           this.helper.WaitEditComponentBusyIndicator(); // Edit Component
        } else {
            
             this.helper.WaitEditComponentBusyIndicator(); // Edit Component
             this.helper.WaitBusyIndicator(); // window component
             this.helper.WaitByIdAndClick('MessageWindow_Ok_0'); // Message window
           // console.log('Try to click OK ');
        }
    }
}
