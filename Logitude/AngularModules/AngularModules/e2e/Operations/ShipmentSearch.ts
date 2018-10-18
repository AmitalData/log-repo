import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from './../Helpers/GeneralFunctions';
import { Driver } from 'selenium-webdriver/safari';


export class ShipmentSearch {
  private Helper: FieldsHelper;
  private operationTab: GeneralFunctions;


  constructor() {
    this.Helper = new FieldsHelper();
    this.operationTab = new GeneralFunctions();
  }
  QuickSearch() {

    this.operationTab.GoToMainMenu('General.MH.Operations');
    var shipmentsTab = this.Helper.WaitByCssAndClick_SelectItemFromList('.PagesMenu', 1);

    this.UseQuickSearch('1000');
    // this.LeaveEntity('.BackBottonBody', 'Operations');
    // this.EnterViews();
    // this.EditShipmentFromList();
    // this.LeaveEntity('.BackBottonBody', 'Shipments');
    // this.SearchInViews();

  }
     
  UseQuickSearch(searchByRef:string) {
    
     this.Helper.WaitByIdAndFill('Shipment_Search',searchByRef);
     this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem',0);

    //  this.Helper.WaitByIdAndFill('SearchInputId2',searchByRef);
    //  this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem',0);


  }



  LeaveEntity(className: string, Text: string) {
    this.Helper.ButtonClickByCss(className, Text);
    browser.driver.sleep(5000);
  }
  EnterViews() {
    this.Helper.ButtonClickByCss('.QueryLink', 'Shipments');
    browser.driver.sleep(5000);
  }
  EditShipmentFromList() {
    this.Helper.ButtonClick('LogGrid_0_0row2');
    browser.driver.sleep(1000);
  }
  SearchInViews() {
    this.Helper.SetTextFieldValue('1117', 'SearchFieldsId_0_0');
    // element(by.id('SearchFieldsId_0_5')).sendKeys('1117');
    browser.driver.sleep(3000);

    element(by.cssContainingText('.TextTrimming', '1117')).click();
    browser.driver.sleep(3000);
    var result = element.all(by.css('.ShortTitleDiv'));

    expect(result.get(1).getText()).toBe('1117');

  }



}

