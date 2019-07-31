import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
import { NewARInvoice } from './ARInvoice/New/NewARInvoice';
import { NewARPayment } from "./ARPayment/NewARPayment";
import { NewCustomer } from '../FullAccounting/GLAccounts/NewCustomerGLaccount';
import { NewVendor } from '../FullAccounting/GLAccounts/NewVendorGLaccount';
import { NewAPInvoice } from '../FullAccounting/APInvoice/NewAPInvoice';
import { NewGLAccount } from '../FullAccounting/GLAccounts/New/NewGLaccount';
import { EditGLAccount } from '../FullAccounting/GLAccounts/Edit/EditGLaccount';
import { ChartOFAccountModule } from '../FullAccounting/ChartOfAccount/ChartOFAccountModule';
import { NewChartOfAccount } from '../FullAccounting/ChartOfAccount/NewEntity/NewChartOfAccount';
import { EditChartOfAccount } from '../FullAccounting/ChartOfAccount/EditEntity/EditChartOfAccount';
import { NewAPPayment } from '../FullAccounting/APPayment/NewAPPayment';

//import { NewAPInvoice } from '../APInvoice/NewAPInvoice';

export class FullAccountingScenarios {
    private Helper: FieldsHelper = new FieldsHelper();
    private generalFunction: GeneralFunctions = new GeneralFunctions();
    private customer = new NewCustomer();
    private arInvoice = new NewARInvoice();

    private arPayment = new NewARPayment();
    private vendor = new NewVendor();
    private aPinvoice = new NewAPInvoice();
    private arpayment = new NewARPayment();
    private GLA = new NewGLAccount();
    private EditGLA = new EditGLAccount();
    private newChart = new NewChartOfAccount();
    private editChart = new EditChartOfAccount();
    private aPpayment = new NewAPPayment();
    constructor() {
    
    }

    AccountingScenario(type: string) {
        if (type == 'ChartOfAccounts') {
            var chartOfAccountNo = this.generalFunction.RandomNumAcc();
            this.generalFunction.GoToMainMenu('General.MH.Maintenance');
            this.Helper.WaitByIdAndClick('ACC');
            this.Helper.WaitByIdAndClick('MaintenanceItemMTCA');
            this.newChart.CreateNewChartOFAccount(chartOfAccountNo, 'Customer');
            this.editChart.EditChartOfAccount(chartOfAccountNo);


            
        }
        if (type == 'CustomerGLAccount') {
            var number = this.generalFunction.RandomNum();
            this.generalFunction.GoToMainMenu('General.MH.CRM');
            this.Helper.WaitByIdAndClick('CRMCUS');
            this.customer.CreateNewCustomerGLAccount('CustomerGLAccount' + number);
            this.customer.ActivateCustomerGLAccount('CustomerGLAccount' + number, number);
           this.Helper.WaitByIdAndClick('General.MH.FullAccounting');
            this.Helper.WaitByIdAndClick('FACS');
        //   this.arInvoice.CreateNewARInvoice('CustomerGLAccount' + number);
            // this.arPayment.CreateNewARPayment('CustomerGLAccount' + number);
        }
        else if (type == 'VendorGLAccount') {
            var vendornumber = this.generalFunction.RandomNum();
            this.vendor.CreateNewVendorGLAccount('Vendor GLAccount' + vendornumber);
            this.vendor.ActivateVendorGLAccount('Vendor GLAccount' + vendornumber, vendornumber);
            this.Helper.WaitByIdAndClick('General.MH.FullAccounting');
            this.Helper.WaitByIdAndClick('FAVND');
            //  this.aPinvoice.CreateNewAPInvoice('Vendor GLaccount', vendornumber);
           // this.aPpayment.CreateNewAPPayment('Vendor GLAccount' + vendornumber);
        }
        else if (type == 'ARPayment') {
            this.Helper.WaitByIdAndClick('General.MH.FullAccounting');
            this.Helper.WaitByIdAndClick('FACS');
            this.arpayment.CreateNewARPayment('CustomerGLAccount');

        }
        else if (type == 'RevGLAccount') {
            var GlaccountNumber = this.generalFunction.RandomNum();
            this.Helper.WaitByIdAndClick('General.MH.FullAccounting');
            this.Helper.WaitByIdAndClick('FAGLAccouts');
            this.GLA.CreateNewGLAccount('My Auto GLAccount', GlaccountNumber);
            this.EditGLA.EditGLAccount(GlaccountNumber);

        }
    }
}
