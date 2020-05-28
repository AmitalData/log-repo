import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { DirectShipment } from './DirectShipment';
import { HouseShipment } from './HouseShipment';
import { MasterShipment } from './MasterShipment';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { EditTabsComponent } from '../EditEntity/EditShipmentTabs.po';
import { ShipmentHelper } from '../ShipmentHelper';

export class ShipmentWorkSpace {
    private Helper: FieldsHelper;

    private DirectShipment: DirectShipment;
    private HouseShipment: HouseShipment;
    private MasterShipment: MasterShipment;
    private GeneralFunction: GeneralFunctions;
    private EditShipmentTabs: EditTabsComponent;
    private shipHelper: ShipmentHelper;

    constructor() {
        this.Helper = new FieldsHelper();
        this.DirectShipment = new DirectShipment();
        this.HouseShipment = new HouseShipment();
        this.MasterShipment = new MasterShipment();
        this.GeneralFunction = new GeneralFunctions();
        this.EditShipmentTabs = new EditTabsComponent();
        this.shipHelper = new ShipmentHelper();
    }
    CreateShipment(ShipmentLevelCode: string, Direction: string, TransportMode: string, ShipmentType: string) {
        if (ShipmentLevelCode == 'D') {
            this.shipHelper.CreateAndCloseNewShipment('NEWDIRECT', 'ShipmentCancelbtn', Direction, TransportMode, ShipmentType);
            if (TransportMode == 'A') {
                var shipperRef1 = this.GeneralFunction.RandomNum();
                this.DirectShipment.CreateDirectShipment(shipperRef1, Direction, TransportMode, ShipmentType);
                this.Helper.WaitBusyIndicator();
                this.Helper.WaitWindowClosed();
                return shipperRef1;
            }
            else if ((TransportMode == 'O' || TransportMode == 'I') && ShipmentType != '') {
                var shipperRef1 = this.GeneralFunction.RandomNum();
                this.DirectShipment.CreateDirectShipment(shipperRef1, Direction, TransportMode, ShipmentType);
                this.Helper.WaitBusyIndicator();
                return shipperRef1;
            }
        }
        else if (ShipmentLevelCode == 'H') {
            this.shipHelper.CreateAndCloseNewShipment('NEWDIRECT', 'ShipmentCancelbtn', Direction, TransportMode, ShipmentType);

            var shipperRef1 = this.GeneralFunction.RandomNum();
            this.HouseShipment.CreateHouseShipment(shipperRef1, ShipmentLevelCode, Direction, TransportMode, ShipmentType);// Create shipment 
            this.Helper.WaitBusyIndicator();
            return shipperRef1;
        }
        else if (ShipmentLevelCode == 'M') {
            this.shipHelper.CreateAndCloseNewShipment('NEWMASTER', 'MasterCancelbtn', Direction, TransportMode, ShipmentType);
            this.Helper.WaitBusyIndicator();

            var shipperRef1 = this.GeneralFunction.RandomNum();
            this.MasterShipment.CreateMasterShipment(shipperRef1, ShipmentLevelCode, Direction, TransportMode, ShipmentType);// Create shipment 
            this.Helper.WaitBusyIndicator();
            return shipperRef1;
        }
    }
}
