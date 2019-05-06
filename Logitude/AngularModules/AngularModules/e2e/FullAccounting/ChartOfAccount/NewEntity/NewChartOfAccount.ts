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
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
    this.Helper.WaitByIdAndClick('Ok-CheckBox_3_129_LBL');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
  }



  
  

}



