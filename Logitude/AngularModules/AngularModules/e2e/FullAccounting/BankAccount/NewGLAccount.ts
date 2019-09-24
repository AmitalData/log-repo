import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class NewGLAccount {
  private Helper = new FieldsHelper();
  private gn = new GeneralFunctions();
  constructor() {


  }

  CreateNewGLAccount(LocalName: string) {
    browser.ignoreSynchronization = true;

    
 
  
    this.Helper.ItemsVisibility('NewGLAccount');
    this.Helper.ItemsPresent('NewGLAccount');
    this.Helper.WaitByIdAndClick('NewGLAccount');
    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsTypeCode', 'Banks');
      this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'GLAccount_ChartOfAccountsTypeCode', 'Banks');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'בנקים');
      this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'GLAccount_ChartOfAccountsId', 'בנקים');
      this.Helper.WaitBusyIndicator();
      this.Helper.WaitByIdAndFill('GLAccount_LocalName', LocalName);
      this.Helper.ItemsVisibility('GLAccount_CurrencyId');  
     
      this.Helper.ItemsPresent('GLAccount_CurrencyId');
      this.Helper.WaitByIdAndFill('GLAccount_CurrencyId', 'NIS');
      this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0 ,'GLAccount_CurrencyId', 'NIS'); 
    //this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
    this.Helper.WaitBusyIndicator();
    this.Helper.ItemsPresent('GLAccount_RevenueExpenseType');
    this.Helper.ItemsVisibility('GLAccount_RevenueExpenseType');
    this.Helper.WaitByIdAndFill('GLAccount_RevenueExpenseType', 'other');
    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'GLAccount_RevenueExpenseType', 'other'); 
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();

  }

  



}





