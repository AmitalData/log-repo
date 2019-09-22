import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { EditChartOfAccount } from './EditChartOfAccount';
import { createBreak } from 'typescript';
export class NewChartOfAccount {
    private Helper: FieldsHelper;
    private GeneralFun: GeneralFunctions;
   // let E: EditChartOfAccount = new EditChartOfAccount();
    private Edit: EditChartOfAccount;


    constructor() {
        this.Helper = new FieldsHelper();
        this.GeneralFun = new GeneralFunctions();
        this.Edit = new EditChartOfAccount();
        // this.Edit = new EditChartOfAccount();
    }


    public CreateNewChartOFAccount(ChartOfAccountNo: string, Type: String) {

        this.Helper.WaitByIdAndFill('null_Search', 'chart');
        this.Helper.WaitByIdAndClick('MaintenanceItemMTCA');
        this.Helper.WaitByIdAndClick('NewButton_ChartOfAccount');
        this.Helper.ItemsVisibility('ChartOfAccount_EnglishName');
        this.Helper.ItemsPresent('ChartOfAccount_EnglishName');
        this.Helper.WaitByIdAndFill('ChartOfAccount_EnglishName', Type + ChartOfAccountNo);
        this.Helper.WaitByIdAndFill('ChartOfAccount_LocalName', Type + ChartOfAccountNo);
        if (Type == 'Revenue') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Revenue');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo + 'R');
            this.Helper.WaitBusyIndicator();
            this.Helper.ItemsVisibility('ok-AddChartOfAccount');
            this.Helper.ItemsPresent('ok-AddChartOfAccount');
            this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitWindowClosed();
            // this.Edit.EditChartOfAccount(ChartOfAccountNo + 'R');
            // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Customer') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Customer');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo + 'C');
            this.Helper.WaitBusyIndicator();
            this.Helper.ItemsVisibility('ok-AddChartOfAccount');
            this.Helper.ItemsPresent('ok-AddChartOfAccount');
            this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
           
           
                this.Helper.WaitBusyIndicator();
                this.Helper.WaitWindowClosed();
             

            


             
            // this.Helper.WaitBusyIndicator();

        }
        else if (Type == 'Vendor') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Vendor');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo + 'V');
            this.Helper.WaitBusyIndicator();
            this.Helper.ItemsVisibility('ok-AddChartOfAccount');
            this.Helper.ItemsPresent('ok-AddChartOfAccount');
            this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
            


                this.Helper.WaitBusyIndicator();
                this.Helper.WaitWindowClosed();
               

            
            // this.Edit.EditChartOfAccount(ChartOfAccountNo + 'V');
            //this.Helper.WaitBusyIndicator();
        }

        else if (Type == 'Banks') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Banks');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo + 'B');
            this.Helper.WaitBusyIndicator();
            this.Helper.ItemsVisibility('ok-AddChartOfAccount');
            this.Helper.ItemsPresent('ok-AddChartOfAccount');
            this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitWindowClosed();
            //this.Edit.EditChartOfAccount(ChartOfAccountNo + 'B');
            // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Expenses') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Expenses');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo + 'E');
            this.Helper.WaitBusyIndicator();
            this.Helper.ItemsVisibility('ok-AddChartOfAccount');
            this.Helper.ItemsPresent('ok-AddChartOfAccount');
            this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitWindowClosed();
            //this.Edit.EditChartOfAccount(ChartOfAccountNo + 'E');
            // this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Works') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Works');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo + 'W');
            this.Helper.WaitBusyIndicator();
            this.Helper.ItemsVisibility('ok-AddChartOfAccount');
            this.Helper.ItemsPresent('ok-AddChartOfAccount');
            this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitWindowClosed();
            //  this.Edit.EditChartOfAccount(ChartOfAccountNo + 'W');
            //this.Helper.WaitBusyIndicator();
        }
        else if (Type == 'Debtors And Creditors') {
            this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode', 'Debtors And Creditors');
            this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
            this.Helper.WaitBusyIndicator();
            this.Helper.WaitByIdAndFill('ChartOfAccount_Code', ChartOfAccountNo + 'D');
            this.Helper.WaitBusyIndicator();
            this.Helper.ItemsVisibility('ok-AddChartOfAccount');
            this.Helper.ItemsPresent('ok-AddChartOfAccount');
            this.Helper.WaitByIdAndClick('ok-AddChartOfAccount');
            this.Helper.WaitBusyIndicator();

            this.Helper.WaitWindowClosed();
            // this.Edit.EditChartOfAccount(ChartOfAccountNo + 'D');
            //this.Helper.WaitBusyIndicator();
        }
        //this.Helper.WaitByIdAndFill('ChartOfAccount_TypeCode','r');
        // this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        // this.Helper.WaitByIdAndClick('CheckBox');
        //  this.Helper.WaitByIdAndClick('ok-AddChartOfAccount'); 
        // this.Helper.WaitBusyIndicator();
        //   this.Helper.WaitWindowClosed();
        //  this.Edit.EditChartOfAccount(ChartOfAccountNo + 'C');

    }
}


   
  
  





