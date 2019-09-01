import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class EditBankAccount {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    EditBankAccount(DisplayNumber: string) {
       // browser.sleep(5000);
        this.Helper.WaitByIdAndClick('BANKSQUIERY');
        this.Helper.WaitByIdAndFill('SearchFieldsId_0_0', DisplayNumber);
        this.Helper.ItemsPresent('ListDataLoaded');
        this.Helper.WaitByIdAndClick('row0col0');
        this.Helper.WaitByIdAndClick('BankAccount.TH.General');
        this.Helper.WaitByIdAndFill('BankAccount_LocalName', 'Updated Local Name');
        this.Helper.WaitByIdAndClick('BankAccount-Save');
       // browser.sleep(5000);



    }
}


