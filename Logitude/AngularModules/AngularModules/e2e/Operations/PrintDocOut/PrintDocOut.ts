import { browser, by, element, WebDriver, protractor } from 'protractor';

import { FieldsHelper } from '../../Helpers/FieldsHelper';

export class PrintDocOut {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    isPrintingCompleted(expectedId, closePopup) {
        this.helper.WaitBusyIndicator();
        this.helper.ItemsPresent(expectedId);
        if (closePopup) {
            this.helper.WaitByCssStringAndClick('.Button', 'Close');
            this.helper.WaitBusyIndicator();
            this.helper.WaitWindowClosed();
        } else {
            // browser.driver.sleep(5000)
            this.helper.WaitByIdAndClick('MessageWindow_Ok_0');
        }
    }
}