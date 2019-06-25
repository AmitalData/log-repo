import { browser, by, element, WebDriver, protractor } from 'protractor';

import { FieldsHelper } from '../../Helpers/FieldsHelper';

export class SendMailPopup {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    sendEmailToFirstUser(emailSubject: string, searchBoxId: string) {
        this.helper.WaitByIdAndClick('SendMessageTobtn');
        this.helper.waitByCss('#' + searchBoxId);
        this.helper.WaitByCssStringAndClick('.DefaultMenuItem', 'All');
        this.helper.WaitByIdAndFill(searchBoxId, 'raghad@logitudeworld.com');
        // Check the first box
        browser.executeScript('arguments[0].click();', element(by.id('SendMessageToCheckBox')).getWebElement());
        this.helper.WaitByIdAndClick('SaveSendMessageTobtn');
        // Write exception test in subject field 
        this.helper.WaitByIdAndFill('EmailSubject', emailSubject);
        this.helper.WaitByIdAndClick('SendMessagebtn');
    }

    isSendingFailed(expectedId) {
        this.helper.waitElementByIDPresence(expectedId);
        this.helper.WaitByIdAndClick('MessageWindow_Ok_0');
        this.helper.WaitByIdAndClick('Delete');
    }





}


//here we need to make sure that the mail send successfully or not there div insife msg pop up 
//if the sunject exseption the msg will find div and make sure that hte mail has failed 