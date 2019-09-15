import { browser, by, element } from "protractor"
import { NewGLAccount } from "./New/NewGLaccount";
import { EditGLAccount } from "./Edit/EditGLaccount";
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';


describe('GLAccount Module', function () {
  var gn1 = new GeneralFunctions();
  var h = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var GLA = new NewGLAccount();
  var EditGLA = new EditGLAccount();





  it(' New GLAccount Was Created And Updated', function () {


    browser.ignoreSynchronization = true;
    gn1.GoToMainMenu('General.MH.FullAccounting');
    h.WaitByIdAndClick('FAGLAccouts');
    var GlaccountNumber = gn1.RandomNum();
      var s = 'My Auto GLAccount';

    GLA.CreateNewGLAccount(s+GlaccountNumber);
      EditGLA.EditGLAccount(s +GlaccountNumber);
   



  });
});
