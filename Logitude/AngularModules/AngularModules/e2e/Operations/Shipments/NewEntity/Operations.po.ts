import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { ShipmentWorkSpace } from './ShipmentWorkSpace';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { EditTabsComponent } from '../EditEntity/EditShipmentTabs.po';
import { ShipmentActionsComponent } from '../EditEntity/ShipmentActions';
export class OperationsComp {
    private Helper: FieldsHelper;
    private ShipmentWorkSpace: ShipmentWorkSpace;
    private Operation: GeneralFunctions;
    private EditShipmentTabs: EditTabsComponent;
    private ShipmentActions: ShipmentActionsComponent = new ShipmentActionsComponent();

    constructor() {
        this.Helper = new FieldsHelper();
        this.ShipmentWorkSpace = new ShipmentWorkSpace();
        this.Operation = new GeneralFunctions();
        this.EditShipmentTabs = new EditTabsComponent();
    }
    DoOperations() {
        this.Operation.GoToMainMenu('General.MH.Operations');
        this.Operation.SelectMenuWorkSpaceTabs('SHIP');

        return this.ShipmentWorkSpace.CreateShipment(browser.params.ShipParams.ShipmentLevelCode, browser.params.ShipParams.Direction, browser.params.ShipParams.TransportMode, browser.params.ShipParams.ShipmentType);
    }
    SearchForShipment(shipperRef1: string) {
        this.Operation.UseSearchBox('Shipment_Search', shipperRef1, 'ListBoxItem');
    }
    EditShipment(shipperRef1: string) {
        this.EditShipmentTabs.EditTabs(shipperRef1, browser.params.ShipParams.ShipmentLevelCode, browser.params.ShipParams.ShipmentType, browser.params.ShipParams.Direction);
    }
    SaveShip() {
        this.Helper.WaitByIdAndClick('Shipment-Save');
        this.Helper.WaitEditComponentBusyIndicator();
    }
    DoShipmentAction(actionType: string) {
        if (actionType == 'OC') {
            this.ShipmentActions.OperationalCloseShipment(browser.params.ShipParams.ShipmentLevelCode);
        }
        else if (actionType == 'copy') {
            this.ShipmentActions.CopyShipment();

        }
        else if (actionType == 'copycancel') {
            this.ShipmentActions.CopyShipment();
            this.Helper.WaitByIdAndClick('Shipment.TH.Overview');
            this.ShipmentActions.CancelShipment();
        }
    }
}

