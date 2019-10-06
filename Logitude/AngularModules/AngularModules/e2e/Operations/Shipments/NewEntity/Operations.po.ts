import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { ShipmentWorkSpace } from './ShipmentWorkSpace';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { EditTabsComponent } from '../EditEntity/EditShipmentTabs.po';

export class OperationsComp {
    private Helper: FieldsHelper;
    private ShipmentWorkSpace: ShipmentWorkSpace;
    private Operation: GeneralFunctions;
    private EditShipmentTabs: EditTabsComponent;


    constructor() {
        this.Helper = new FieldsHelper();
        this.ShipmentWorkSpace = new ShipmentWorkSpace();
        this.Operation = new GeneralFunctions();
        this.EditShipmentTabs = new EditTabsComponent();
    }
    DoOperations() {
        this.Operation.GoToMainMenu('General.MH.Operations');
        this.Operation.SelectMenuWorkSpaceTabs('SHIP');
        //  this.ShipmentWorkSpace.CreateWizard('M');

        return this.ShipmentWorkSpace.CreateShipment(browser.params.ShipParams.ShipmentLevelCode, browser.params.ShipParams.Direction, browser.params.ShipParams.TransportMode, browser.params.ShipParams.ShipmentType);
        // this.ShipmentWorkSpace.CreateShipment('D', 'Export','A', '');

    }
    SearchForShipment(shipperRef1: string) {
        this.Operation.UseSearchBox('Shipment_Search', shipperRef1, 'ListBoxItem');
    }

    EditShipment(shipperRef1: string) {

        this.EditShipmentTabs.EditTabs(shipperRef1, browser.params.ShipParams.ShipmentLevelCode, browser.params.ShipParams.ShipmentType, browser.params.ShipParams.Direction);
        //this.EditShipment(shipperRef1,browser.params.ShipParams.ShipmentLevelCode, browser.params.ShipParams.Direction, browser.params.ShipParams.TransportMode, browser.params.ShipParams.ShipmentType);
    }
    saveShip() {
        this.Helper.WaitByIdAndClick('Shipment-Save');
        this.Helper.WaitEditComponentBusyIndicator();
    }
}

