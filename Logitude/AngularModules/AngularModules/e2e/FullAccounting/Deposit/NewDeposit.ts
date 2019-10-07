import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element, WebDriver, protractor, $ } from 'protractor';
import { variable } from '@angular/compiler/src/output/output_ast';




export class NewDeposit{
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();

    constructor() {


    }

    CreateNewDeposit() {
        browser.ignoreSynchronization = true;
        //this.Helper.WaitByIdAndClick('FABNKS');
        this.Helper.ItemsVisibility('NEWDEPOSIT');
        this.Helper.ItemsPresent('NEWDEPOSIT');
        this.Helper.WaitByIdAndClick('NEWDEPOSIT');
       
        this.Helper.WaitByIdAndFill('BankDeposit_CashBookId', 'Cashbook cash');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'BankDeposit_CashBookId', 'Cashbook cash');

        this.Helper.WaitByIdAndFill('BankDeposit_DepositBankAccountId', 'Bank');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'BankDeposit_DepositBankAccountId', 'Bank');
        
        this.Helper.WaitByIdAndClick('CREATEDEPOSIT');
        this.Helper.WaitBusyIndicator();
       // this.Helper.WaitWindowClosed();
        
       this.Helper.WaitByIdAndClick('BankDeposit.TH.Details');
       this.Helper.ItemsVisibility('BankDeposit_ForeignAmount');
        this.Helper.ItemsPresent('BankDeposit_ForeignAmount');
        this.Helper.WaitByIdAndFill('BankDeposit_ForeignAmount' , '1');

        this.Helper.ItemsVisibility('BankDeposit.B.Approve');
        this.Helper.ItemsPresent('BankDeposit.B.Approve');
        this.Helper.WaitByIdAndClick('BankDeposit.B.Approve');
        this.Helper.WaitEditComponentBusyIndicator();
        
        this.Helper.ItemsVisibility('EditBackbutton');
        this.Helper.ItemsPresent('EditBackbutton');
        this.Helper.WaitByIdAndClick('EditBackbutton');





    }
}
