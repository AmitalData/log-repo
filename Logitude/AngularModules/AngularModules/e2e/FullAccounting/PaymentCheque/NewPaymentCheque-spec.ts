import { browser, by, element } from "protractor"
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
//import{NewGLAccount } from './NewGLAccount';
import {NewPaymentCheque} from './NewPaymentCheque';
import {EditPaymentCheque} from './EditPaymentCheque';
describe('PaymentCheque Module', function () {
  var gn1 = new GeneralFunctions();
  var h = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var PYC = new NewPaymentCheque();
  var PYCE = new EditPaymentCheque();

 ; 




  it(' New Payment cheque  Was Created And Updated', function () {

      console.log('Khawlaaa check ')
    browser.ignoreSynchronization = true;
    gn1.GoToMainMenu('General.MH.FullAccounting');
    h.WaitByIdAndClick('FABNKS');
    PYC.CreateNewPaymentCheque();
    PYCE.EditPaymentCheque();
    //edit.EditBankAccount(bank);
  
  });
});
