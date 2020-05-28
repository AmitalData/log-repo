import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element, WebDriver, protractor, $ } from 'protractor';




export class NewARPayment {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();

    constructor() {


    }

    CreateNewARPayment(BillToName: string) {
        browser.ignoreSynchronization = true;
        this.Helper.WaitByIdAndClick('NewARPayment');
        this.Helper.WaitBusyIndicator();
        this.Helper.ItemsVisibility('ARPayment_BillToId');
        this.Helper.ItemsPresent('ARPayment_BillToId');
        this.Helper.WaitByIdAndFill('ARPayment_BillToId', BillToName);
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARPayment_BillToId', BillToName);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_BillToAddressId', 'Main Address');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARPayment_BillToAddressId', 'Main Address');
        //this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_AmountInPaymentCurrency', '10000');
        this.Helper.WaitByIdAndFill('ARPayment_AccountingPaymentMethodId', 'Cash');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARPayment_AccountingPaymentMethodId', 'Cash'),
            this.Helper.WaitBusyIndicator();


        this.Helper.WaitByIdAndClick('ok-AddARPayment');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndFill('ARPayment_BranchId', 'Main Office');
        //this.Helper.WaitBusyIndicator();
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ARPayment_BranchId', 'Main Office');
        this.Helper.WaitBusyIndicator();
        // this.Helper.ItemsVisibility('row0');
        //        this.Helper.WaitWindowClosed();
        var ec = protractor.ExpectedConditions;
        browser.wait(ec.invisibilityOf(element(by.id("ARPaymentSpinner"))), 100000);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('ARPayment.B.Approve');
        this.Helper.WaitBusyIndicator();
        //  browser.sleep(6000);
        // this.Helper.WaitByIdAndClick('EditBackbutton');


        // browser.sleep(6000);






    }
}
