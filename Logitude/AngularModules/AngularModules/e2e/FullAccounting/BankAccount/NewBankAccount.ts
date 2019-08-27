import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';




export class NewBankAccount {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();

    constructor() {


    }

    CreateNewBankAccount(Name: string , Name1: string,Name2: string ,Name3: string  ) {
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndFill('BankAccount_BankId ', 'Leumi');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('BankAccount_BranchNumber', '12544');
        //this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        //this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('BankAccount_AccountNumber', Name);
        this.Helper.WaitByIdAndFill('BankAccount_LocalName', Name);
        this.Helper.WaitByIdAndFill('BankAccount_EnglishName', Name+'English');
        this.Helper.WaitByIdAndFill('BankAccount_CurrencyId ', 'NIS');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownList', 0),
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('BankAccount_GLAccountId', Name1);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownList', 0);
        this.Helper.WaitByIdAndFill('BankAccount_DeferredGLAccountId', Name2);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownList', 0);
        this.Helper.WaitByIdAndFill('BankAccount_TransferGLAcccountId', Name3);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownList', 0);
        this.Helper.WaitByIdAndClick('OKBUTTON ');
        this.Helper.WaitBusyIndicator();

      //  browser.sleep(6000);
       // this.Helper.WaitByIdAndClick('EditBackbutton');


       // browser.sleep(6000);






    }
}
