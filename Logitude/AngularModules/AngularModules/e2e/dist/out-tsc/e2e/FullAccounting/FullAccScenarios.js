"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../Helpers/GeneralFunctions");
var NewARInvoice_1 = require("./ARInvoice/New/NewARInvoice");
var NewARPayment_1 = require("./ARPayment/NewARPayment");
var NewCustomerGLaccount_1 = require("../FullAccounting/GLAccounts/NewCustomerGLaccount");
var NewVendorGLaccount_1 = require("../FullAccounting/GLAccounts/NewVendorGLaccount");
var NewAPInvoice_1 = require("../FullAccounting/APInvoice/NewAPInvoice");
var NewGLaccount_1 = require("../FullAccounting/GLAccounts/New/NewGLaccount");
var EditGLaccount_1 = require("../FullAccounting/GLAccounts/Edit/EditGLaccount");
var NewChartOfAccount_1 = require("../FullAccounting/ChartOfAccount/NewEntity/NewChartOfAccount");
var EditChartOfAccount_1 = require("../FullAccounting/ChartOfAccount/EditEntity/EditChartOfAccount");
var NewAPPayment_1 = require("../FullAccounting/APPayment/NewAPPayment");
//import { NewAPInvoice } from '../APInvoice/NewAPInvoice';
var FullAccountingScenarios = /** @class */ (function () {
    function FullAccountingScenarios() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.generalFunction = new GeneralFunctions_1.GeneralFunctions();
        this.customer = new NewCustomerGLaccount_1.NewCustomer();
        this.arInvoice = new NewARInvoice_1.NewARInvoice();
        this.arPayment = new NewARPayment_1.NewARPayment();
        this.vendor = new NewVendorGLaccount_1.NewVendor();
        this.aPinvoice = new NewAPInvoice_1.NewAPInvoice();
        this.arpayment = new NewARPayment_1.NewARPayment();
        this.GLA = new NewGLaccount_1.NewGLAccount();
        this.EditGLA = new EditGLaccount_1.EditGLAccount();
        this.newChart = new NewChartOfAccount_1.NewChartOfAccount();
        this.editChart = new EditChartOfAccount_1.EditChartOfAccount();
        this.aPpayment = new NewAPPayment_1.NewAPPayment();
    }
    FullAccountingScenarios.prototype.AccountingScenario = function (type) {
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
    };
    return FullAccountingScenarios;
}());
exports.FullAccountingScenarios = FullAccountingScenarios;
//# sourceMappingURL=FullAccScenarios.js.map