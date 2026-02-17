import { browser, by, element } from "protractor"
import { NewARInvoice} from "./New/NewARInvoice";

import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';


describe('ARInvoice Module', function () {
  var Helper = new GeneralFunctions();
  var F = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var arinvoice= new NewARInvoice();






  it(' New General ARInvoice Was Created', function () {


    browser.ignoreSynchronization = true;
    
    Helper.GoToMainMenu('General.MH.FullAccounting');
    F.WaitByIdAndClick('FACS');
    arinvoice.CreateNewARInvoice('Test Customer GLAccount');
   // arinvoice.CreateNewARInvoice('Basel - Multi Local');
    

    

   



  });
});