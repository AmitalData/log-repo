import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { createBreak } from 'typescript';
export class NewChartOfAccount {
  private Helper: FieldsHelper;
  private GeneralFun: GeneralFunctions;


  constructor() {
      this.Helper = new FieldsHelper();
      this.GeneralFun = new GeneralFunctions();
  }


    public CreateNewChartOFAccount(ChartOfAccountNo: string, Type: String) {

        // var chartOfAccountNo = this.GeneralFun.RandomNumAcc();
        this.Helper.WaitByIdAndClick('NewButton_ChartOfAccount');
        this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_EnglishName', Type + ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_LocalName', Type + ChartOfAccountNo);
        if (Type == 'Revenue') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Reven');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
           // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Customer') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Custom');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
           // this.Helper.WaitBusyIndicator();

        }
        else if (Type == 'Vendor') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Vendor');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        //this.Helper.WaitBusyIndicator();
        }

        else if (Type == 'Banks') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Banks');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
           // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Expenses') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Expenses');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
           // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Works') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Works');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            //this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Debtors And Creditors') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Debtors And Creditors');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            //this.Helper.WaitBusyIndicator();
        }
    //this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode','r');
   // this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
   // this.Helper.WaitByIdAndClick('CheckBox');
    this.Helper.WaitByIdAndClick('ok-AddChartOfAccount'); 
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
  }



  
  

}



