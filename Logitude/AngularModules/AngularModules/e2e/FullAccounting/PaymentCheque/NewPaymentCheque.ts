import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element, WebDriver, protractor, $ } from 'protractor';
import { variable } from '@angular/compiler/src/output/output_ast';




export class NewPaymentCheque {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();

    constructor() {


    }

    CreateNewPaymentCheque() {
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('FABNKS');
        this.Helper.ItemsVisibility('NEWPaymentCheque');
        this.Helper.ItemsPresent('NEWPaymentCheque');
        this.Helper.WaitByIdAndClick('NEWPaymentCheque');
        this.Helper.ItemsVisibility('PaymentCheque_PayToGLAccountId');
        this.Helper.ItemsPresent('PaymentCheque_PayToGLAccountId');
        
        this.Helper.WaitByIdAndFill('PaymentCheque_PayToGLAccountId', 'diff');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'PaymentCheque_BankAccountId', 'Bank');

        this.Helper.WaitByIdAndFill('PaymentCheque_BankAccountId', 'Bank');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'PaymentCheque_BankAccountId', 'Bank');

        this.Helper.ItemsVisibility('PaymentCheque_LocalAmount');
        this.Helper.ItemsPresent('PaymentCheque_LocalAmount');
        this.Helper.WaitByIdAndFill('PaymentCheque_LocalAmount', '1200');
      
        this.Helper.WaitByIdAndClick('CREATEPAYMENTCHEQUE');
       this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();
        this.Helper.WaitByIdAndClick('PaymentCheque.TH.General');
        this.Helper.ItemsVisibility('PaymentCheque_PayToGLAccountId');
        this.Helper.ItemsPresent('PaymentCheque_PayToGLAccountId');
        this.Helper.ItemsVisibility('EditBackbutton');
        this.Helper.ItemsPresent('EditBackbutton');
        
        this.Helper.WaitByIdAndClick('EditBackbutton');




    }
}
