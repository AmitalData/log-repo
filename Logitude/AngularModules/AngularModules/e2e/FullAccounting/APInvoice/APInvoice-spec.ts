import { browser, by, element } from "protractor"
import { NewAPInvoice} from "./NewAPInvoice";

import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';


describe('APInvoice Module', function () {
  var Helper = new GeneralFunctions();
  var F = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var apinvoice= new NewAPInvoice();






  it(' New APInvoice Was Created', function () {


    browser.ignoreSynchronization = true;

    Helper.GoToMainMenu('General.MH.FullAccounting');
    F.WaitByIdAndClick('FAVND');
    var NUM= Helper.RandomNum();
    apinvoice.CreateNewAPInvoice('Vendor GLaccount',NUM);

    

  });
});