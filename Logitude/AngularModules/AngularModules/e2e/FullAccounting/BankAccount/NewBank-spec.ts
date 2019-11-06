import { browser, by, element } from "protractor"
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import{NewGLAccount } from './NewGLAccount';
import {NewBankAccount} from './NewBankAccount';
import {EditBankAccount} from './EditBankAccount';
describe('BankAccount Module', function () {
  var gn1 = new GeneralFunctions();
  var h = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var GLA = new NewGLAccount();
  var BNk = new NewBankAccount();
 var edit = new EditBankAccount(); 




    it(' New BankAccount Was Created And Updated', function () {

      console.log('Khawlaaa check ')
    browser.ignoreSynchronization = true;
    gn1.GoToMainMenu('General.MH.FullAccounting');
    h.WaitByIdAndClick('FAGLAccouts');
    var GlaccountNumber = gn1.RandomNum();
      var Gl = 'Bank'+GlaccountNumber;
      var diff ='Diff'+GlaccountNumber;
      var trans='trans'+GlaccountNumber;
      var bank = 'Bank'+GlaccountNumber;
    GLA.CreateNewGLAccount(Gl);
    GLA.CreateNewGLAccount(diff);
    GLA.CreateNewGLAccount(trans);
    //GLA.CreateNewGLAccountandmove();  
    BNk.CreateNewBankAccount(bank,Gl,diff,trans);
    edit.EditBankAccount(bank);
  
  });
});
