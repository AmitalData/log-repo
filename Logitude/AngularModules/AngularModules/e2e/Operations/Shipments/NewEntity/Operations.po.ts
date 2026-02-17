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
    DoOperations(ShipmentLevelCode: string, Direction: string, TransportMode: string, ShipmentType: string) {
        this.Operation.GoToMainMenu('General.MH.Operations');
        this.Operation.SelectMenuWorkSpaceTabs('SHIP');

        return this.ShipmentWorkSpace.CreateShipment(ShipmentLevelCode, Direction, TransportMode, ShipmentType);
    }
    SearchForShipment(shipperRef1: string) {
        this.Operation.UseSearchBox('Shipment_Search', shipperRef1, 'ListBoxItem');
    }
    EditShipment(shipperRef1: string, ShipmentLevelCode: string, ShipmentType: string, Direction: string) {
        this.EditShipmentTabs.EditTabs(shipperRef1, ShipmentLevelCode, ShipmentType, Direction);
    }
    SaveShip() {
        this.Helper.WaitByIdAndClick('Shipment-Save');
        this.Helper.WaitEditComponentBusyIndicator();
    }
    DoShipmentAction(actionType: string) {
        if (actionType == 'OC') {
            this.ShipmentActions.OperationalCloseShipment(browser.params.ShipParams.ShipmentLevelCode);
        }
        else if (actionType == 'OR') {
            this.ShipmentActions.OperationalReopenShipment(browser.params.ShipParams.ShipmentLevelCode);
        }
        else if (actionType == 'AC') {
            this.ShipmentActions.AccountingCloseShipment(browser.params.ShipParams.ShipmentLevelCode);
        }
        else if (actionType == 'AR') {
            this.ShipmentActions.AccountedReopenShipment(browser.params.ShipParams.ShipmentLevelCode);
        }
        else if (actionType == 'copy') {
            this.ShipmentActions.CopyShipment(browser.params.ShipParams.ShipmentLevelCode);
        }
        else if (actionType == 'cancel') {
            this.ShipmentActions.CopyShipment(browser.params.ShipParams.ShipmentLevelCode);
        }
        else if (actionType == 'multiActions') {
            this.ShipmentActions.OperationalCloseShipment(browser.params.ShipParams.ShipmentLevelCode);
            this.ShipmentActions.AccountingCloseShipment(browser.params.ShipParams.ShipmentLevelCode);
            this.ShipmentActions.AccountedReopenShipment(browser.params.ShipParams.ShipmentLevelCode);
            this.ShipmentActions.OperationalReopenShipment(browser.params.ShipParams.ShipmentLevelCode);
            this.ShipmentActions.CopyShipment(browser.params.ShipParams.ShipmentLevelCode);
            //this.Helper.WaitByIdAndClick('Shipment.TH.Overview_1');
            //this.ShipmentActions.CancelShipment();
        }
    }
}

