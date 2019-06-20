import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class EditARInvoice {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    EditARInvoice(DisplayNumber: string) {
        this.Helper.WaitByIdAndFill('CardGLAccount_Search', DisplayNumber);



        this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.Helper.WaitByIdAndClick('GLAccount.TH.General');
        this.Helper.WaitByIdAndFill('GLAccount_LocalName', 'Updated Local Name')
        this.Helper.WaitByIdAndClick('GLAccount-Save');
        






        browser.sleep(5000);



    }
}







