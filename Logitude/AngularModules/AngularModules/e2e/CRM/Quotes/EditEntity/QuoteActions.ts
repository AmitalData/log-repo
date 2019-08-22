import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class QuoteActions {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }
    QuoteMenubuttonActions(ActionName: string) {
        if (ActionName == 'copy') {
            this.CopyQuote();
        }
        else if (ActionName == 'accept') {
            this.QuoteAccepted();
        }
        else if (ActionName == 'decline') {
            this.QuoteDeclined();
        }
        else if (ActionName == 'build') {
            this.BuildShipmentFromQuote();
        }
        else if (ActionName == 'copybuild') {
            this.CopyQuote();
            this.QuoteAccepted();
            this.BuildShipmentFromQuote();
        }
    }
    CopyQuote() {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Quote.B.CopyQuote');

        this.Helper.WaitEditComponentBusyIndicator();
        var includePickup = element(by.id('CheckBox_0_13'));
        browser.executeScript("arguments[0].click();", includePickup.getWebElement());

        var includeDelivery = element(by.id('CheckBox_0_14'));
        browser.executeScript("arguments[0].click();", includeDelivery.getWebElement());

        // this.Helper.WaitByIdAndFill('Quote_FromAddressCity_1', 'Ramallah');
        // this.Helper.WaitByIdAndFill('Quote_FromAddressCountryId_1', 'State Of Palestine');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        // this.Helper.WaitByIdAndFill('Quote_ToAddressCity_1', 'Ramallah');
        // this.Helper.WaitByIdAndFill('Quote_ToAddressCountryId_1', 'State Of Palestine');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndClick('CreateQuote');
    }

    BuildShipmentFromQuote() {
        this.Helper.WaitByIdAndClick('Quote.B.BuildShipment_1');

        this.Helper.ItemsVisibility('LogLov_Shipment_ShipperId');
        this.Helper.ItemsPresent('LogLov_Shipment_ShipperId');

        var shipmentTypeBtn = element(by.id('ShipmentLevelRadio_0D'));
        browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
    }
    QuoteAccepted() {
        this.Helper.WaitByIdAndClick('Quote.B.Accept_1');
        this.Helper.WaitByIdAndFill('Quote_EventNote', 'Quote Accepted by protractor ... ');
        this.Helper.WaitByIdAndClick('ConfrimApproved');
        this.Helper.WaitEditComponentBusyIndicator();
    }
    QuoteDeclined() {
        this.Helper.WaitByIdAndClick('Quote.B.Decline');
        this.Helper.WaitByIdAndFill('Quote_EventNote', 'Quote Declined by protractor ... ');
        this.Helper.WaitByIdAndClick('ConfrimApproved');
    }
}
