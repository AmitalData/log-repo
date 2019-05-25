import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';




export class ShipmentSearch {
  private helper: FieldsHelper;
    //private operationTab: GeneralFunctions;
    private LogboxTab: GeneralFunctions;


  constructor() {
    this.helper = new FieldsHelper();
    this.LogboxTab = new GeneralFunctions();
  }
   QuickSearch() {

       this.LogboxTab.GoToMainMenu('General.MH.Importers');
      // this.helper.WaitByCssStringAndClick('Counter',' Agent Shipments'); 
     //var shipmentsTab = this.Helper.WaitByCssAndClick_SelectItemFromList('.PagesMenu', 1);
      
    }
     
  
}

