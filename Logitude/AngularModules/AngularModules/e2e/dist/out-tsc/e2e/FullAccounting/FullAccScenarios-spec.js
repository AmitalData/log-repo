"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../Helpers/GeneralFunctions");
//import { FullAccProcess } from './FullAccProcess'
var NewCustomerGLaccount_1 = require("../FullAccounting/GLAccounts/NewCustomerGLaccount");
var NewARInvoice_1 = require("./ARInvoice/New/NewARInvoice");
var NewARPayment_1 = require("./ARPayment/NewARPayment");
var Login_po_1 = require("../login/Login.po");
var FullAccScenarios_1 = require("./FullAccScenarios");
describe('', function () {
    var fullAccountingScenario = new FullAccScenarios_1.FullAccountingScenarios();
    var gn1 = new GeneralFunctions_1.GeneralFunctions();
    var h = new FieldsHelper_1.FieldsHelper();
    protractor_1.browser.driver.manage().window().maximize();
    var cus = new NewCustomerGLaccount_1.NewCustomer();
    var arinvoice = new NewARInvoice_1.NewARInvoice();
    var arpayment = new NewARPayment_1.NewARPayment();
    var logins = new Login_po_1.LoginComp();
    if (protractor_1.browser.params.FullAccount.FullAccountingType == 'ARPayment') {
        it(' ARPayment Was Created Successfully  ', function () {
            protractor_1.browser.ignoreSynchronization = true;
            fullAccountingScenario.AccountingScenario(protractor_1.browser.params.FullAccount.FullAccountingType);
        });
    }
    else if (protractor_1.browser.params.FullAccount.FullAccountingType == 'CustomerGLAccount') {
        it(' Customer Was Created Successfully with Activated GLAccount And ARInvoice ', function () {
            protractor_1.browser.ignoreSynchronization = true;
            fullAccountingScenario.AccountingScenario(protractor_1.browser.params.FullAccount.FullAccountingType);
        });
    }
    else if (protractor_1.browser.params.FullAccount.FullAccountingType == 'VendorGLAccount') {
        it(' Vendor Was Created Successfully with Activated GLAccount And APInvoice ', function () {
            protractor_1.browser.ignoreSynchronization = true;
            fullAccountingScenario.AccountingScenario(protractor_1.browser.params.FullAccount.FullAccountingType);
        });
    }
    else if (protractor_1.browser.params.FullAccount.FullAccountingType == 'ChartOfAccounts') {
        it(' ChartOfAccount Was Created Successfully ', function () {
            protractor_1.browser.ignoreSynchronization = true;
            fullAccountingScenario.AccountingScenario(protractor_1.browser.params.FullAccount.FullAccountingType);
        });
    }
    else if (protractor_1.browser.params.FullAccount.FullAccountingType == 'RevGLAccount') {
        it(' Revenue GLAccount Was successfully Created and Editted ', function () {
            protractor_1.browser.ignoreSynchronization = true;
            fullAccountingScenario.AccountingScenario(protractor_1.browser.params.FullAccount.FullAccountingType);
        });
    }
});
//# sourceMappingURL=FullAccScenarios-spec.js.map