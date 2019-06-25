import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewVendor {
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }

  public CreateNewVendorGLAccount(Name: string) {

    this.Helper.WaitByIdAndClick('General.MH.Maintenance');
    this.Helper.WaitByIdAndClick('MaintenanceItemMTVD');
    this.Helper.WaitByIdAndClick('NewButton_Vendor');
    this.Helper.WaitByIdAndFill('Address_Name', Name);
    this.Helper.WaitByIdAndFill('Address_CountryId', 'ps');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndFill('Address_City', 'Nablus');
    this.Helper.WaitByIdAndClick('Ok-AddVendor');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
    this.Helper.WaitByIdAndFill('SearchFieldsId_0_0',Name);
    this.Helper.ItemsPresent('ListDataLoaded');
    this.Helper.WaitByIdAndClick('row0col1');
    
  }

  public ActivateVendorGLAccount(Name: string, DisplayNumber: string) {
    //this.Generator.QuickSearchTextBox('SearchFieldsId_0_0', Name);
   // this.Helper.WaitByIdAndClick('row0col1');
  
      this.Helper.WaitByIdAndClick('Vendor.TH.Accounting');

    this.Helper.WaitByIdAndClick('Activate');
    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'ven');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
   // this.Helper.WaitByIdAndFill('GLAccount_LocalName', Name + DisplayNumber);
    this.Helper.WaitByIdAndFill('GLAccount_CurrencyId', 'Nis');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
      this.Helper.WaitBusyIndicator();
      this.Helper.WaitWindowClosed();
      this.Helper.WaitBusyIndicatorToShow();
      this.Helper.WaitBusyIndicator();
    //browser.sleep(5000);
    //this.Helper.WaitBusyIndicator();
     var boo=this.Helper.ItemsVisibility('3mo');
    if ('boo') {
      this.Helper.WaitByIdAndClick('Vendor-SaveClose');
      this.Helper.WaitBusyIndicator();
    }
  }

}



