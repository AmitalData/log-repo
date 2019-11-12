import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
import { APPaymentComponent } from './New/APPayment';

export class AccountingComp {
    private Helper: FieldsHelper;
    private AccountingTab: GeneralFunctions;
    private APPayment: APPaymentComponent = new APPaymentComponent();

    constructor() {
        this.Helper = new FieldsHelper();
        this.AccountingTab = new GeneralFunctions();
    }
    DoAccounting(AccountingType: string) {
        this.APPayment.Shipment();
        this.AccountingTab.SelectMenuWorkSpaceTabs('BOOK');

        this.AccountingTab.GoToMainMenu('General.MH.Accounting');
        if (AccountingType == 'ARP') {
            this.AccountingTab.SelectMenuWorkSpaceTabs('RECEIVABLEAccounting');
        }
        else if (AccountingType == 'APP') {
            this.AccountingTab.SelectMenuWorkSpaceTabs('PAYABLEAccounting');
            this.APPayment.CreatePayment();
        }
    }
}

