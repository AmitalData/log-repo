import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';




export class NewAPPayment {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();

    constructor() {


    }

    CreateNewAPPayment(VendorName: string) {
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewAPPayment');
        this.Helper.WaitByIdAndFill('APPayment_VendorId', VendorName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('APPayment_AccountingPaymentMethodId', 'Cash');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0),
        this.Helper.WaitByIdAndFill('APPayment_AmountInPaymentCurrency', '10000');
        //this.Helper.WaitByIdAndClick('ok-AddARPayment');
        //this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('APPayment_PaymentCurrencyId', 'NIS');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        this.Helper.WaitByIdAndClick('APPayment.B.Approve');
        this.Helper.WaitBusyIndicator();

        this.Helper.WaitByIdAndClick('EditBackbutton');

        browser.sleep(6000);






    }
}