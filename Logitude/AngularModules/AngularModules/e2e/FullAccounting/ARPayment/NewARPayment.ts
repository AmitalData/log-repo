import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';




export class NewARPayment {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();

    constructor() {


    }

    CreateNewARPayment(BillToName: string) {
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewARPayment');
        this.Helper.WaitByIdAndFill('ARPayment_BillToId', BillToName);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitWindowClosed();

        this.Helper.WaitByIdAndFill('ARPayment_AccountingPaymentMethodId', 'Cash');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0),
        this.Helper.WaitWindowClosed();

        this.Helper.WaitByIdAndFill('ARPayment_AmountInPaymentCurrency', '10000');
        this.Helper.WaitByIdAndClick('ok-AddARPayment');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_BranchId', 'Main Office');
        this.Helper.WaitByCssAndClick_SelectItemFromList('.DropDownList', 0);
        this.Helper.WaitWindowClosed();

        this.Helper.WaitByIdAndClick('ARPayment.B.Approve');
        this.Helper.WaitBusyIndicator();

        this.Helper.WaitByIdAndClick('EditBackbutton');

        browser.sleep(6000);






    }
}