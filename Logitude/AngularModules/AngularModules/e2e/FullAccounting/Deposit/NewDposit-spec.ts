import { browser, by, element } from "protractor"
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
//import{NewGLAccount } from './NewGLAccount';
import {NewDeposit} from './NewDeposit';
import {EditDeposit} from './EditDeposit';
describe('PaymentCheque Module', function () {
  var gn1 = new GeneralFunctions();
  var h = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var DP = new NewDeposit();
  var DPE = new EditDeposit();

 ; 




  it(' New Payment cheque  Was Created And Updated', function () {

    //  console.log('Khawlaaa check ')
      browser.ignoreSynchronization = true;
      gn1.GoToMainMenu('General.MH.FullAccounting');
      h.WaitByIdAndClick('FABNKS');
      DP.CreateNewDeposit();
      DPE.EditDeposit();
      //edit.EditBankAccount(bank);
  
  });
});
