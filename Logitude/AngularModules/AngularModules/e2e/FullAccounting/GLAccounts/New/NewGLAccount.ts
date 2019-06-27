import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class NewGLAccount {
  private Helper = new FieldsHelper();
  private gn = new GeneralFunctions();
  constructor() {


  }

  CreateNewGLAccount(LocalName: string, DisplayNumber: string) {
    browser.ignoreSynchronization = true;

    
    this.Helper.WaitByIdAndClick('NewGLAccount');
   

    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsTypeCode', 'Revenues');
    this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
    this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'Rev');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0); 
    this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
      this.Helper.WaitByIdAndFill('GLAccount_LocalName', LocalName);
      this.Helper.WaitBusyIndicator();
    
    this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
    







  }


}





