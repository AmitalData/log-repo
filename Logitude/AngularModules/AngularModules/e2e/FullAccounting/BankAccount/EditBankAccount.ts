import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class EditBankAccount {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    EditBankAccount(DisplayNumber: string) {
        this.Helper.WaitByIdAndClick('BANKSQUIERY');
        this.Helper.WaitByIdAndFill('SearchFieldsId', DisplayNumber);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.Helper.WaitByIdAndClick('BankAccount.TH.General');
        this.Helper.WaitByIdAndFill('BankAccount_LocalName', 'Updated Local Name')
        this.Helper.WaitByIdAndClick('BankAccount-SaveClose');
        browser.sleep(5000);



    }
}


