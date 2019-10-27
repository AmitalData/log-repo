import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

export class ShipmentActionsComponent {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }
    public ShipmentActions() {

    }
    OperationalCloseShipment() {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.OperationalClose');
        this.Helper.WaitByIdAndClick('ConfirmAction');

        this.Helper.WaitEditComponentBusyIndicator();
    }
    OperationalReopenShipment() {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.OperationalReopen');
        this.Helper.WaitByIdAndFill('EventNotes', 'Operational ReOpen - Protractor Testing .. ')

        this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
        this.Helper.WaitBusyIndicator();
    }
    AccountingCloseShipment() {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.AccountingClose');

        this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
        this.Helper.WaitBusyIndicator();
    }
    AccountedReopenShipment() {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.AccountedReopen');
        this.Helper.WaitByIdAndFill('EventNotes', 'Accounting ReOpen - Protractor Testing .. ');

        this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
        this.Helper.WaitBusyIndicator();
    }
    CopyShipment() {
        this.Helper.WaitByIdAndClick('MenuButtons');
        this.Helper.WaitByIdAndClick('Shipment.B.CopyShipment');

        if (browser.params.ShipParams.ShipmentLevelCode == 'D' || browser.params.ShipParams.ShipmentLevelCode == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_ShipperReference2', 'Copy shipment ... ')
            this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
        }
        else {
            this.Helper.WaitByIdAndFill('Master_Notes', 'Copy shipment ... ')
            this.Helper.WaitByIdAndClick('MasterCreatebtn');
        }
        this.Helper.WaitBusyIndicator();
    }
}
