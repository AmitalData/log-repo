import { browser, by, element, WebDriver, protractor } from 'protractor';

import { FieldsHelper } from '../../Helpers/FieldsHelper';

export class PrintDocOut {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    isPrintingCompleted(expectedId, closePopup) {
       // this.helper.WaitBusyIndicator();
       this.helper.WaitEditComponentBusyIndicator();
        this.helper.ItemsPresent(expectedId);
        if (closePopup) {
            this.helper.WaitByCssStringAndClick('.Button', 'Close');
           // this.helper.WaitBusyIndicator();
           this.helper.WaitEditComponentBusyIndicator();
            this.helper.WaitWindowClosed();
        } else {
           
            this.helper.WaitByIdAndClick('MessageWindow_Ok_0');
        }
    }
}
