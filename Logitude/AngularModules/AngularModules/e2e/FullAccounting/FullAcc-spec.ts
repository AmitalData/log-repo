import { browser, by, element } from "protractor"
import { NewGLAccount } from "../FullAccounting/GLAccounts/New/NewGLaccount";
import { EditGLAccount } from "../FullAccounting/GLAccounts/Edit/EditGLaccount";
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
//import { FullAccProcess } from './FullAccProcess'
import { NewCustomer } from '../FullAccounting/GLAccounts/NewCustomerGLaccount';
import { NewARInvoice } from './ARInvoice/New/NewARInvoice';
import { NewARPayment } from "./ARPayment/NewARPayment";



describe('', function () {
    var gn1 = new GeneralFunctions();
    var h = new FieldsHelper();
    browser.driver.manage().window().maximize();
    // var GLA = new NewGLAccount();
    //var EditGLA = new EditGLAccount();
  //  var process = new FullAccProcess();
    var cus = new NewCustomer();
    var arinvoice = new NewARInvoice();
    var arpayment = new NewARPayment();






    it(' Creating Customer GLAccount Was Successfully Done', function () {
        var number = gn1.RandomNum();

        browser.ignoreSynchronization = true;
        gn1.GoToMainMenu('General.MH.CRM');
        h.WaitByIdAndClick('CRMCUS');
      // h.WaitByIdAndClick('NewButton_Customer');
        cus.CreateNewCustomerGLAccount('CustomerGLAccount' + number);
        cus.ActivateCustomerGLAccount('CustomerGLAccount' + number, number);
       h.WaitByIdAndClick('General.MH.FullAccounting');
       h.WaitByIdAndClick('FACS');
       arinvoice.CreateNewARInvoice('CustomerGLAccount' + number);
       arpayment.CreateNewARPayment('CustomerGLAccount' + number);




    });
});