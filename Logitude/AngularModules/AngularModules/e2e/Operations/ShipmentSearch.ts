import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from './../Helpers/GeneralFunctions';
// import { Driver } from 'selenium-webdriver/safari';


export class ShipmentSearch {
  private Helper: FieldsHelper;
  private operationTab: GeneralFunctions;


  constructor() {
    this.Helper = new FieldsHelper();
    this.operationTab = new GeneralFunctions();
  }
  // QuickSearch() {

  //   this.operationTab.GoToMainMenu('General.MH.Operations');
  //   var shipmentsTab = this.Helper.WaitByCssAndClick_SelectItemFromList('.PagesMenu', 1);

  //   this.UseSearchBox('searchFeildId','1000');
  //   // this.LeaveEntity('.BackBottonBody', 'Operations');
  //   // this.EnterViews();
  //   // this.EditShipmentFromList();
  //   // this.LeaveEntity('.BackBottonBody', 'Shipments');
  //   // this.SearchInViews();

  // }
     
  // UseSearchBox(searchFeildId:string,searchByRef:string) {

  //    this.Helper.WaitByIdAndFill(searchFeildId,searchByRef);
  //    this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem',0);
  // }
}

