import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class QuoteActions {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }
    QuoteMenubuttonActions(ActionName: string, Direction: string, TransportMode: string, QuoteType: string) {
        if (ActionName == 'copy') {
            this.CopyQuote(Direction, TransportMode);
        }
        else if (ActionName == 'accept') {
            this.QuoteAccepted();
        }
        else if (ActionName == 'decline') {
            this.QuoteDeclined();
        }
        else if (ActionName == 'build') {
            this.BuildShipmentFromQuote(QuoteType);
        }
        else if (ActionName == 'copybuild') {

            this.CopyQuote(Direction, TransportMode);
            this.QuoteAccepted();
            this.BuildShipmentFromQuote(QuoteType);
        }
    }
    CopyQuote(Direction: string, TransportMode: string) {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Quote.B.CopyQuote');
        this.Helper.WaitEditComponentBusyIndicator();
      /*  if (Direction == 'Domestic' && TransportMode == 'I') {

        }
        else {
            this.Helper.ItemsPresent('IncludePickupDiv');
            this.Helper.ItemsPresent('IncludeDeliveryDiv');

           /* var includePickup = element(by.id('Quote_CopyPickUpIsChecked'));
            browser.executeScript("arguments[0].click();", includePickup.getWebElement());

            var includeDelivery = element(by.id('Quote_CopyDeliveryIsChecked'));
            browser.executeScript("arguments[0].click();", includeDelivery.getWebElement());*/

            // this.Helper.WaitByIdAndFill('Quote_FromAddressCity_1', 'Ramallah');
            // this.Helper.WaitByIdAndFill('Quote_FromAddressCountryId_1', 'State Of Palestine');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            // this.Helper.WaitByIdAndFill('Quote_ToAddressCity_1', 'Ramallah');
            // this.Helper.WaitByIdAndFill('Quote_ToAddressCountryId_1', 'State Of Palestine');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }*/
        this.Helper.WaitByIdAndClick('CreateQuote');
    }

    BuildShipmentFromQuote(QuoteType: string) {
        if (QuoteType != 'RoutingRate') {
            this.Helper.WaitByIdAndClick('Quote.B.BuildShipment_1');

            this.Helper.ItemsVisibility('LogLov_Shipment_ShipperId');
            this.Helper.ItemsPresent('LogLov_Shipment_ShipperId');

            var shipmentTypeBtn = element(by.id('ShipmentLevelRadio_0D'));
            browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());
            this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
            this.Helper.WaitByIdAndClick('Shipment.TH.Overview');
        } else {
            // var EC = protractor.ExpectedConditions;
            // browser.wait(EC.visibilityOf(element(by.id('Quote.B.BuildShipment_1'))), 100000).then(a => {
            //     console.log('no build shipments inside routing rate ...');
            // });
        }
    }
    QuoteAccepted() {
        this.Helper.WaitByIdAndClick('Quote.B.Accept_1');
        this.Helper.WaitByIdAndFill('Quote_EventNote', 'Quote Accepted by protractor ... ');
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id('Quote_EventNote'))), 100000).then(a => {
            console.log('inside accept quote ');
        });
        this.Helper.WaitByIdAndClick('ConfrimApproved');
        this.Helper.WaitEditComponentBusyIndicator();

    }
    QuoteDeclined() {
        this.Helper.WaitByIdAndClick('Quote.B.Decline');
        this.Helper.WaitByIdAndFill('Quote_EventNote', 'Quote Declined by protractor ... ');
        this.Helper.WaitByIdAndClick('ConfrimApproved');
    }
}
