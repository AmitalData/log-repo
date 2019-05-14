import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { createBreak } from 'typescript';
export class NewChartOfAccount {
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;


  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }


  public CreateNewChartOFAccount(ChartOfAccountNo: string) {

    this.Helper.WaitByIdAndFill('ChartOfAccount_Code',ChartOfAccountNo);
    this.Helper.WaitByIdAndFill('ChartOfAccount_EnglishName','customer chartofaccount');
    this.Helper.WaitByIdAndFill('ChartOfAccount_LocalName','customer chartofaccount local');

    this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode','r');
    this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
   // this.Helper.WaitByIdAndClick('CheckBox');
    this.Helper.WaitByIdAndClick('ok-AddChartOfAccount'); 
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
  }



  
  

}



