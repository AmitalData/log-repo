import { browser, by, element, WebDriver, protractor } from 'protractor';

import { FieldsHelper } from '../../Helpers/FieldsHelper';

export class PrintDocOut {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    isPrintingCompleted(expectedId, closePopup) {
        this.helper.waitElementByIDPresence(expectedId);
        if(closePopup) {
            this.helper.WaitByCssStringAndClick('.Button', 'Close');
        }   
    }
}