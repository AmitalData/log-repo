import { browser, by, element } from "protractor"


import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { NewVendor } from "./NewVendorGLaccount";


describe('ARInvoice Module', function () {
  var Helper = new GeneralFunctions();
  var F = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var v= new NewVendor();

  it(' New Vendor GLAccount Was Created', function () {


    browser.ignoreSynchronization = true;
    
   // Helper.GoToMainMenu('General.MH.FullAccounting');
   // F.WaitByIdAndClick('FACS');
   var n= Helper.RandomNum()
    v.CreateNewVendorGLAccount('Test Vendor GLAccount');
    v.ActivateVendorGLAccount('Test Vendor GLAccount'+n,n);

    

   



  });
});
