import { browser, by, element } from "protractor"


import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { NewVendor } from "./NewVendorGLaccount";
import { NewCustomer } from "./NewCustomerGLaccount";


describe('CustomerGlAccount Module', function () {
  var Helper = new GeneralFunctions();
  var F = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var customer= new NewCustomer();

  it(' New Customer GLAccount Was Created', function () {


    browser.ignoreSynchronization = true;
    
     // Helper.GoToMainMenu('General.MH.CRM');
      //F.WaitByIdAndClick('CRMCUS');
      Helper.GoToMainMenu('General.MH.Customers');
      var n = Helper.RandomNum();
      customer.CreateNewCustomerGLAccount('Test Customer GLAccount'+n);
      customer.ActivateCustomerGLAccount('Test Customer GLAccount'+n);

    

   
   

  });
});
