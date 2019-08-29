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

    
    this.Helper.WaitByIdAndClick('NewGLAccount');
   

    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsTypeCode', 'Banks');
    this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'בנקים');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitBusyIndicator();
      this.Helper.WaitByIdAndFill('GLAccount_LocalName', LocalName);
      this.Helper.WaitByIdAndFill('GLAccount_CurrencyId', 'NIS');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0); 
    //this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndFill('GLAccount_RevenueExpenseType', 'other');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0); 
    this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();

  }

  CreateNewGLAccountandmove(LocalName: string) {

    browser.ignoreSynchronization = true;
    this.Helper.WaitByIdAndClick('NewGLAccount');
    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsTypeCode', 'Banks');
    this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'בנקים');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitBusyIndicator();
      this.Helper.WaitByIdAndFill('GLAccount_LocalName', LocalName);
      this.Helper.WaitByIdAndFill('GLAccount_CurrencyId', 'NIS');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0); 
    //this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndFill('GLAccount_RevenueExpenseType', 'other');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0); 
    this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
     
     // browser.sleep(5000);
             // browser.driver.sleep(6000);

    
  }



}





