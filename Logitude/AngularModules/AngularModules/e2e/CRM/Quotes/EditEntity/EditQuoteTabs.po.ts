import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { OverviewTabComponent } from './OverviewTab';


export class EditTabsComponent {
  private Helper: FieldsHelper;
  private Operation: GeneralFunctions;
  private OverviewTabScenario: OverviewTabComponent;
 

  constructor() {
    this.Helper = new FieldsHelper();
    this.Operation = new GeneralFunctions();
    this.OverviewTabScenario = new OverviewTabComponent();
   
  }
  GoToQuote(){
    this.Helper.WaitByIdAndClick('Quote.TH.Overview');
  }

  EditTabs(shipperRef1:string,ShipmentType: string,Direction: string,TransportMode: string,QuoteType: string) {


    this.Helper.WaitByIdAndClick('Quote.TH.Overview');
  

  }

}

