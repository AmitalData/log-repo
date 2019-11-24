import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
import { APPaymentComponent } from './New/APPayment';
import { ARPaymentComponent } from './New/ARPayment';


export class AccountingComp {
    private Helper: FieldsHelper;
    private AccountingTab: GeneralFunctions;
    private APPayment: APPaymentComponent = new APPaymentComponent();
    private ARPayment: ARPaymentComponent = new ARPaymentComponent();

    constructor() {
        this.Helper = new FieldsHelper();
        this.AccountingTab = new GeneralFunctions();
    }
    DoAccounting(AccountingType: string) {
        if (AccountingType == 'ARP') {
            this.ARPayment.CreateARInvoiceFromShipment();
            this.Helper.WaitByIdAndClick('General.MH.Accounting');

            this.AccountingTab.SelectMenuWorkSpaceTabs('RECEIVABLEAccounting');
            this.ARPayment.CreatePayment();

        }
        else if (AccountingType == 'APP') {
            this.APPayment.CreateAPInvoiceFromShipment();
            this.Helper.WaitByIdAndClick('General.MH.Accounting');
        
            this.AccountingTab.SelectMenuWorkSpaceTabs('PAYABLEAccounting');
            this.APPayment.CreatePayment();
        }
    }
}

