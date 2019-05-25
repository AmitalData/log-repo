import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { DirectAWB } from './DirectAWB';
import { HouseAWB } from './HouseAWB';
import { MasterAWB } from './MasterAWB';
import { DirectShipment } from './DirectShipment';
import { HouseShipment } from './HouseShipment';
import { MasterShipment } from './MasterShipment';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
// import { ShipmentSearch } from '../../ShipmentSearch';
import { EditTabsComponent } from '../EditEntity/EditShipmentTabs.po';
import { ShipmentHelper } from '../ShipmentHelper';

export class ShipmentWorkSpace {
  private Helper: FieldsHelper;
  private DirectAWB: DirectAWB;
  private HouseAWB: HouseAWB;
  private MasterAWB: MasterAWB;

  private DirectShipment: DirectShipment;
  private HouseShipment: HouseShipment;
  private MasterShipment: MasterShipment;
  private GeneralFunction: GeneralFunctions;
  private EditShipmentTabs: EditTabsComponent;
  // private QuickSearch: ShipmentSearch;
  private ShipmentMenuButtons: ShipmentHelper;

  constructor() {
    this.Helper = new FieldsHelper();
    this.DirectAWB = new DirectAWB();
    this.HouseAWB = new HouseAWB();
    this.MasterAWB = new MasterAWB();

    this.DirectShipment = new DirectShipment();
    this.HouseShipment = new HouseShipment();
    this.MasterShipment = new MasterShipment();
    this.GeneralFunction = new GeneralFunctions();
    // this.QuickSearch = new ShipmentSearch();
    this.EditShipmentTabs = new EditTabsComponent();

    this.ShipmentMenuButtons = new ShipmentHelper();
  }

  CreateShipment(LogitudeShipType: string, Direction: string, TransportMode: string, ShipmentType: string) {
    this.DirectShipment.CreateAndCloseNewShipment('NEWDIRECT','ShipmentCancelbtn');

    if (LogitudeShipType == 'D') {
      if (TransportMode == 'A') {
        var shipperRef1 = this.GeneralFunction.RandomNum();
        this.DirectShipment.CreateDirectShipment(shipperRef1, Direction, TransportMode, ShipmentType);
        this.Helper.WaitBusyIndicator();

        
        this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
        this.EditShipmentTabs.EditTabs(shipperRef1, LogitudeShipType, ShipmentType,Direction);

      }
      else if ((TransportMode == 'O' || TransportMode == 'I') && ShipmentType != '') {
        var shipperRef1 = this.GeneralFunction.RandomNum();
        this.DirectShipment.CreateDirectShipment(shipperRef1, Direction, TransportMode, ShipmentType);
        this.Helper.WaitBusyIndicator();

        this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
        this.EditShipmentTabs.EditTabs(shipperRef1, LogitudeShipType, ShipmentType,Direction);
      }
      // this.ShipmentMenuButtons.OperationalCloseShipment();
      // this.ShipmentMenuButtons.AccountingCloseShipment();
      // this.ShipmentMenuButtons.AccountedReopenShipment();
      // this.ShipmentMenuButtons.OperationalReopenShipment();
      // this.ShipmentMenuButtons.CopyShipment();
    }
    else if (LogitudeShipType == 'H') {

      var shipperRef1 = this.GeneralFunction.RandomNum();
      this.HouseShipment.CreateHouseShipment(shipperRef1, LogitudeShipType, Direction, TransportMode, ShipmentType);// Create shipment 
      this.Helper.WaitBusyIndicator();

      this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
      this.EditShipmentTabs.EditTabs(shipperRef1, LogitudeShipType, ShipmentType,Direction);

    }
    else if (LogitudeShipType == 'M') {
    // this.DirectShipment.CreateAndCloseNewShipment('NEWMASTER','MasterCancelbtn');
    // this.Helper.WaitBusyIndicator();

      var shipperRef1 = this.GeneralFunction.RandomNum();
      this.MasterShipment.CreateMasterShipment(shipperRef1, LogitudeShipType, Direction, TransportMode, ShipmentType);// Create shipment 
      this.Helper.WaitBusyIndicator();

      this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
      this.EditShipmentTabs.EditTabs(shipperRef1, LogitudeShipType, ShipmentType,Direction);
    }

  }

  CreateWizard(LogitudeWizardType: string) {
    var shipperRef1 = this.GeneralFunction.RandomNum();

    if (LogitudeWizardType == 'D') {
      this.DirectAWB.CreateDirectAWB(shipperRef1, LogitudeWizardType);
    }
    else if (LogitudeWizardType == 'H') {
      this.HouseAWB.CreateHouseWizard(shipperRef1, LogitudeWizardType);
    }
    else if (LogitudeWizardType == 'M') {
      this.MasterAWB.CreateMasterWizard(shipperRef1, LogitudeWizardType);
    }
  }

}

