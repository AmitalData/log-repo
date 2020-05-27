import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class EditPaymentCheque {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    EditPaymentCheque() {
       // browser.sleep(5000);
        this.Helper.WaitByIdAndClick('PaymentChequesQuery');
        this.Helper.WaitByIdAndClick('row0col0');
        this.Helper.WaitByIdAndClick('PaymentCheque.TH.General');
        this.Helper.WaitByIdAndFill('PaymentCheque_PayToName', 'updatednameby protractor');

        this.Helper.ItemsVisibility('PaymentCheque.B.PaymentChequeSaveAsDraft');
        this.Helper.ItemsPresent('PaymentCheque.B.PaymentChequeSaveAsDraft');    
        this.Helper.WaitByIdAndClick('PaymentCheque.B.PaymentChequeSaveAsDraft');
        this.Helper.WaitBusyIndicator();
      //  this.Helper.ItemsVisibility('NewButton_BankAccount');

// browser.driver.sleep(5000)
       // browser.sleep(5000);



    }
}


