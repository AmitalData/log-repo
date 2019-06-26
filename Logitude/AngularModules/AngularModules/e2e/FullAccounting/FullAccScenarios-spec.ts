import { browser, by, element } from "protractor"
import { NewGLAccount } from "../FullAccounting/GLAccounts/New/NewGLaccount";
import { EditGLAccount } from "../FullAccounting/GLAccounts/Edit/EditGLaccount";
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
//import { FullAccProcess } from './FullAccProcess'
import { NewCustomer } from '../FullAccounting/GLAccounts/NewCustomerGLaccount';
import { NewARInvoice } from './ARInvoice/New/NewARInvoice';
import { NewARPayment } from "./ARPayment/NewARPayment";
import { LoginComp } from "../login/Login.po";
import { FullAccountingScenarios } from "./FullAccScenarios";




describe('', function () {
    let fullAccountingScenario: FullAccountingScenarios = new FullAccountingScenarios();


    var gn1 = new GeneralFunctions();
    var h = new FieldsHelper();
    browser.driver.manage().window().maximize();
    var cus = new NewCustomer();
    var arinvoice = new NewARInvoice();
    var arpayment = new NewARPayment();
    var logins = new LoginComp();




    if (browser.params.FullAccount.FullAccountingType == 'ARPayment') {
        it(' ARPayment Was Created Successfully  ', function () {

            browser.ignoreSynchronization = true;

            fullAccountingScenario.AccountingScenario(browser.params.FullAccount.FullAccountingType);

        })
    }
    else if (browser.params.FullAccount.FullAccountingType == 'AR') {
        it(' Customer Was Created Successfully with Activated GLAccount And ARInvoice ', function () {

            browser.ignoreSynchronization = true;

            fullAccountingScenario.AccountingScenario(browser.params.FullAccount.FullAccountingType);

        })
    }
    else if (browser.params.FullAccount.FullAccountingType == 'AP') {
        it(' Vendor Was Created Successfully with Activated GLAccount And APInvoice ', function () {

            browser.ignoreSynchronization = true;

            fullAccountingScenario.AccountingScenario(browser.params.FullAccount.FullAccountingType);

        })
    }

    else if (browser.params.FullAccount.FullAccountingType == 'RevGLAccount') {
        it(' Revenue GLAccount Was successfully Created and Editted ', function () {

            browser.ignoreSynchronization = true;

            fullAccountingScenario.AccountingScenario(browser.params.FullAccount.FullAccountingType);

        })
    }

}); 
