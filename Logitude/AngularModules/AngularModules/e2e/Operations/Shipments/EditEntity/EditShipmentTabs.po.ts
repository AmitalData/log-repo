import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { GeneralTabComponent } from './GeneralTab';
import { OrderTabComponent } from './OrdersTab';
import { PartnersTabComponent } from './PartnersTab';
import { PackagesTabComponent } from './PackagesTab';
import { RoutingTabComponent } from './RoutingTab';
import { PayablesTabComponent } from './PayablesTab';
import { ReceivablesTabComponent } from './RecievablesTab';
import { DocsOutTabComponent } from './DocsOutTab';
import { ShipmentsTabComponent } from './ShipmentsTab';
import { ShipmentSearch } from '../../ShipmentSearch';

export class EditTabsComponent {
  private Helper: FieldsHelper;
  private Operation: GeneralFunctions;
  private OrderTabScenario: OrderTabComponent;
  private PartnersTabScenario: PartnersTabComponent;

  private GeneralTabScenario: GeneralTabComponent;
  private PackagesTabScenario: PackagesTabComponent;
  private RoutingTabScenario: RoutingTabComponent;
  private PayablesTabScenario: PayablesTabComponent;
  private ReceivablesTabScenario: ReceivablesTabComponent;
  private DocsOutTabScenario: DocsOutTabComponent;
  private ShipmentsTabScenario: ShipmentsTabComponent;
  private QuickSearch: ShipmentSearch;

  constructor() {
    this.Helper = new FieldsHelper();
    this.Operation = new GeneralFunctions();
    this.GeneralTabScenario = new GeneralTabComponent();
    this.OrderTabScenario = new OrderTabComponent();
    this.PartnersTabScenario = new PartnersTabComponent();
    this.PackagesTabScenario = new PackagesTabComponent();
    this.RoutingTabScenario = new RoutingTabComponent();
    this.PayablesTabScenario = new PayablesTabComponent();
    this.ReceivablesTabScenario = new ReceivablesTabComponent();
    this.DocsOutTabScenario = new DocsOutTabComponent();
    this.ShipmentsTabScenario = new ShipmentsTabComponent();

    this.QuickSearch = new ShipmentSearch();
  }
  GoToShipment(){
    this.Operation.GoToMainMenu('General.MH.Operations');
    this.Operation.SelectMenuWorkSpaceTabs('SHIP');
    // this.QuickSearch.UseQuickSearch('SR1545342');
  }

  EditTabs(shipperRef1:string,ShipmentLevelCode:string,ShipmentType: string,Direction:string) {


    this.Helper.WaitByIdAndClick('Shipment.TH.Overview');
   this.GeneralTabScenario.GeneralTab(ShipmentLevelCode);
   this.OrderTabScenario.OrderTab(ShipmentLevelCode,ShipmentType);
  this.PartnersTabScenario.PartnersTab(ShipmentLevelCode);
   this.PackagesTabScenario.PackagesTab(ShipmentLevelCode,ShipmentType);
   this.RoutingTabScenario.RoutingTab(ShipmentLevelCode,ShipmentType,Direction);
   this.PayablesTabScenario.PayablesTab(shipperRef1,ShipmentType);
  this.ReceivablesTabScenario.RecievablesTab(ShipmentLevelCode,ShipmentType);
  //this.DocsOutTabScenario.DocsOutTab();
  
   if(ShipmentLevelCode=='M'){
     this.ShipmentsTabScenario.ShipmentsTab();
    }
      this.Helper.WaitByIdAndClick('Shipment-Save');
    this.Helper.WaitBusyIndicator();
  }

}

