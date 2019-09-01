import { browser, by, element } from "protractor"
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import {NewPAymentCheque} from './NewPAymentCheque';
import {EditPAymentCheque} from './EditPAymentCheque';
describe('CRM Module', function () {
  var gn1 = new GeneralFunctions();
  var h = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var NP = new NewPAymentCheque();
  var EP = new EditPAymentCheque(); 




  it(' New PAymentCheque Was Created And Updated', function () {

      //console.log('Khawlaaa check ')
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
