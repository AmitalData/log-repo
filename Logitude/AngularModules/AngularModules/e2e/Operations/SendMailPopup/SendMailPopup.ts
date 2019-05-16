import { browser, by, element, WebDriver, protractor } from 'protractor';

import { FieldsHelper } from '../../Helpers/FieldsHelper';

export class SendMailPopup {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    sendEmailToFirstUser() {
        this.helper.WaitBusyIndicator();
        browser.driver.sleep(3000);
        this.helper.WaitByIdAndClick('SendMessageTobtn');
        this.helper.waitByCss('tocomponent');
        // We are currently executing javascript code on the element because it is intractable
        browser.executeScript('arguments[0].click();', element(by.id('SendMessageToCheckBox')).getWebElement());
        this.helper.WaitByIdAndClick('SaveSendMessageTobtn');
        this.helper.WaitByIdAndClick('SendMessagebtn');
    }
}