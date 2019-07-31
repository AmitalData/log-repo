import { browser, by, element } from "protractor"
import { NewAPPayment} from "./NewAPPayment";

import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';


describe('APInvoice Module', function () {
  var Helper = new GeneralFunctions();
  var F = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var appayment= new NewAPPayment();






  it(' New APInvoice Was Created', function () {


    browser.ignoreSynchronization = true;

    Helper.GoToMainMenu('General.MH.FullAccounting');
    F.WaitByIdAndClick('FAVND');
    var NUM= Helper.RandomNum();
    appayment.CreateNewAPPayment('Test Customer GLAccount');

    

   



  });
});