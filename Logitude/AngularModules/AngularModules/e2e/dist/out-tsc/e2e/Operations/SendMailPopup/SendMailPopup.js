"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var SendMailPopup = /** @class */ (function () {
    function SendMailPopup() {
        this.helper = new FieldsHelper_1.FieldsHelper();
    }
    SendMailPopup.prototype.sendEmailToFirstUser = function (emailSubject, searchBoxId) {
        this.helper.WaitByIdAndClick('SendMessageTobtn');
        this.helper.waitByCss('#' + searchBoxId);
        this.helper.WaitByCssStringAndClick('.DefaultMenuItem', 'All');
        this.helper.WaitByIdAndFill(searchBoxId, 'raghad@logitudeworld.com');
        // Check the first box
        protractor_1.browser.executeScript('arguments[0].click();', protractor_1.element(protractor_1.by.id('SendMessageToCheckBox')).getWebElement());
        this.helper.WaitByIdAndClick('SaveSendMessageTobtn');
        // Write exception test in subject field 
        this.helper.WaitByIdAndFill('EmailSubject', emailSubject);
        this.helper.WaitByIdAndClick('SendMessagebtn');
    };
    SendMailPopup.prototype.isSendingFailed = function (expectedId) {
        // this.helper.waitElementByIDPresence(expectedId);
        this.helper.ItemsPresent(expectedId);
        this.helper.WaitByIdAndClick('MessageWindow_Ok_0');
        this.helper.WaitByIdAndClick('Delete');
    };
    return SendMailPopup;
}());
exports.SendMailPopup = SendMailPopup;
//here we need to make sure that the mail send successfully or not there div insife msg pop up 
//if the sunject exseption the msg will find div and make sure that hte mail has failed 
//# sourceMappingURL=SendMailPopup.js.map