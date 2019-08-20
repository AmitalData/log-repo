import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class QuoteActions {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }

    CopyQuote() {

        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Quote.B.CopyQuote');
        this.Helper.WaitByIdAndClick('Quote_IncludePickUp');
        this.Helper.WaitByIdAndClick('IncludeDelivery');
// browser.driver.sleep(6000);
        // this.Helper.ItemsVisibility('LogitudeWindow_0_1');
        // var EC = protractor.ExpectedConditions;

        //     browser.wait(EC.visibilityOf(element(by.id("EditBusyInicator"))), 100000).then(a => { });
        // browser.wait(EC.visibilityOf(element(by.id("SessionBusyComponent"))), 100000).then(a => { });
        // browser.driver.sleep(5000)
        // this.Helper.WaitByIdAndFill('textboxdiv_Quote_DescriptionOfGoods', 'Created by protractor .. Copy Quote ');
        
        this.Helper.WaitByIdAndFill('Quote_FromAddressCity','Ramallah');
        this.Helper.WaitByIdAndFill('Quote_FromAddressCountryId', 'State Of Palestine');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Quote_ToAddressCity','Ramallah');
    this.Helper.WaitByIdAndFill('Quote_ToAddressCountryId', 'State Of Palestine');
this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndClick('CreateQuote');
    }

    BuildShipmentFromQuote() {
        this.Helper.WaitByIdAndClick('Quote.B.BuildShipment');
        // this.Helper.WaitByIdAndClick('ShipmentLevelRadio_0D');
        var shipmentTypeBtn = element(by.id('ShipmentLevelRadio_0D'));
        browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
        browser.driver.sleep(5000);
    }
    QuoteAccepted() {
        this.Helper.WaitByIdAndClick('Quote.B.Accept');
        this.Helper.WaitByIdAndFill('Quote_EventNote', 'Quote Accepted by protractor ... ');
        this.Helper.WaitByIdAndClick('ConfrimApproved');
        // this.Helper.WaitWindowClosed();
        this.Helper.WaitEditComponentBusyIndicator();

    }
    QuoteDeclined() {
        this.Helper.WaitByIdAndClick('Quote.B.Decline');
        this.Helper.WaitByIdAndFill('Quote_EventNote', 'Quote Declined by protractor ... ');
        this.Helper.WaitByIdAndClick('ConfrimApproved');
    }
}
