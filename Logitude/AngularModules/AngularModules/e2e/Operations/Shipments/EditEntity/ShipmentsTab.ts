import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { HouseShipment } from '../NewEntity/HouseShipment';


export class ShipmentsTabComponent {
    private Helper: FieldsHelper;
    private CreateHouseShipment: HouseShipment;

    constructor() {
        this.Helper = new FieldsHelper();
        this.CreateHouseShipment = new HouseShipment();
    }

    public ShipmentsTab() {
        this.Helper.WaitByIdAndClick('Shipment.TH.Consolidation');
        this.Helper.WaitBusyIndicator();

        this.Helper.WaitByIdAndClick('NewHouseBtn')
        this.Helper.WaitBusyIndicator();

        this.CreateHouseShipment.FillHouseShipmentFields('CreatedFromMaster','');
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
        this.Helper.WaitBusyIndicator();
    }
}  