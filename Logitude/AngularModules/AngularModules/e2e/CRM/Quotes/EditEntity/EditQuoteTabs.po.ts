import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { OverviewTabComponent } from './OverviewTab';
import { DetailsTabComponent} from './DetailsTab';
import { PartnerTabComponent} from './PartnerTab';
import { PackagesTabComponent} from './PackagesTab';

export class EditTabsComponent {
  private Helper: FieldsHelper;
  private Operation: GeneralFunctions;
  private OverviewTabScenario: OverviewTabComponent;
  private DetailsTabScenario: DetailsTabComponent;
  private PartnerTabScenario: PartnerTabComponent;
  private PackagesTabScenario: PackagesTabComponent;
 

  constructor() {
    this.Helper = new FieldsHelper();
    this.Operation = new GeneralFunctions();
    this.OverviewTabScenario = new OverviewTabComponent();
    this.DetailsTabScenario = new DetailsTabComponent();
    this.PartnerTabScenario = new PartnerTabComponent();
    this.PackagesTabScenario = new PackagesTabComponent();
   
  }
 

  EditTabs(shipperRef1:string,ShipmentType: string,Direction: string,TransportMode: string,QuoteType: string) {

   this.OverviewTabScenario.OverviewTab(ShipmentType);
   this.DetailsTabScenario .DetailsTab(ShipmentType);
   this.PartnerTabScenario .PartnerTab(ShipmentType);
   this.PackagesTabScenario .PackagesTab(ShipmentType);
  

  }

}

