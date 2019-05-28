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
        this.helper.waitByCss('#SearchFieldsId_0_0');
        // We are currently executing javascript code on the element because it is intractable
        
        this.helper.WaitByCssStringAndClick('.DefaultMenuItem', 'All');
        this.helper.WaitByIdAndFill('SearchFieldsId_0_1','raghad@logitudeworld.com');
        browser.driver.sleep(2000);
        //to check the first box
        browser.executeScript('arguments[0].click();', element(by.id('SendMessageToCheckBox')).getWebElement());
        this.helper.WaitByIdAndClick('SaveSendMessageTobtn');
        this.helper.WaitByIdAndClick('SendMessagebtn');

        
    }

   // isSendingCompleted(expectedId) {
      //  this.helper.waitElementByIDPresence(expectedId);
        //if(closePopup) {
          // this.helper.WaitByCssStringAndClick('.Button', 'Close');
      // }   


}

//here we need to make sure that the mail send successfully or not there div insife msg pop up 
//if the sunject exseption the msg will find div and make sure that hte mail has failed 