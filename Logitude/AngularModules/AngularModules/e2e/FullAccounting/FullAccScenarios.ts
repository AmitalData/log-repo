import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
import { NewARInvoice } from './ARInvoice/New/NewARInvoice';
import { NewARPayment } from "./ARPayment/NewARPayment";
import { NewCustomer } from '../FullAccounting/GLAccounts/NewCustomerGLaccount';
import { NewVendor } from '../FullAccounting/GLAccounts/NewVendorGLaccount';
import { NewAPInvoice } from '../FullAccounting/APInvoice/NewAPInvoice';
//import { NewAPInvoice } from '../APInvoice/NewAPInvoice';

export class FullAccountingScenarios {
    private Helper: FieldsHelper = new FieldsHelper();
    private generalFunction: GeneralFunctions = new GeneralFunctions();
    private customer = new NewCustomer();
    private arInvoice = new NewARInvoice();
    private arPayment = new NewARPayment();
    private vendor = new NewVendor();
    private aPinvoice = new NewAPInvoice();


    constructor() {
    
    }

    AccountingScenario(type: string) {
        if (type == 'AR') {
            var number = this.generalFunction.RandomNum();
            this.generalFunction.GoToMainMenu('General.MH.CRM');
            this.Helper.WaitByIdAndClick('CRMCUS');
            this.customer.CreateNewCustomerGLAccount('CustomerGLAccount' + number);
            this.customer.ActivateCustomerGLAccount('CustomerGLAccount' + number, number);
            this.Helper.WaitByIdAndClick('General.MH.FullAccounting');
            this.Helper.WaitByIdAndClick('FACS');
            this.arInvoice.CreateNewARInvoice('CustomerGLAccount' + number);
           // this.arPayment.CreateNewARPayment('CustomerGLAccount' + number);
        }
        else if (type == 'AP') {
            var vendornumber = this.generalFunction.RandomNum();
            this.vendor.CreateNewVendorGLAccount('Vendor GLAccount' + vendornumber);
            this.vendor.ActivateVendorGLAccount('Vendor GLAccount' + vendornumber, vendornumber);
            this.Helper.WaitByIdAndClick('General.MH.FullAccounting');
            this.Helper.WaitByIdAndClick('FAVND');
            this.aPinvoice.CreateNewAPInvoice('Vendor GLaccount', vendornumber);








        }
    }
}
