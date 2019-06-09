import { browser, by, element, WebDriver, protractor } from 'protractor';

import { FieldsHelper } from '../../Helpers/FieldsHelper';

export class SendMailPopup {
    private helper: FieldsHelper;

    constructor() {
        this.helper = new FieldsHelper();
    }

    sendEmailToFirstUser(emailSubject: string, searchBoxId: string) {
        this.helper.WaitBusyIndicator();
        browser.driver.sleep(3000);
        this.helper.WaitByIdAndClick('SendMessageTobtn');
        this.helper.waitByCss('#' + searchBoxId);
        // We are currently executing javascript code on the element because it is intractable

        this.helper.WaitByCssStringAndClick('.DefaultMenuItem', 'All');
        this.helper.WaitByIdAndFill(searchBoxId, 'raghad@logitudeworld.com');
        browser.driver.sleep(2000);
        //to check the first box
        browser.executeScript('arguments[0].click();', element(by.id('SendMessageToCheckBox')).getWebElement());
        this.helper.WaitByIdAndClick('SaveSendMessageTobtn');
        //this is to write exception test in subject field 
      //  browser.driver.sleep(20000);
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