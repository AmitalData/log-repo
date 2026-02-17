import { browser, by, element } from "protractor"
import { NewARPayment} from "./NewARPayment";

import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';


describe('ARPayment Module', function () {
  var Helper = new GeneralFunctions();
  var F = new FieldsHelper();
  browser.driver.manage().window().maximize();
  var arpay= new NewARPayment();






  it(' New ARPayment Was Created', function () {


    browser.ignoreSynchronization = true;
    Helper.GoToMainMenu('General.MH.FullAccounting');
    F.WaitByIdAndClick('FACS');
    arpay.CreateNewARPayment('Test Customer GLAccount');

    

   



  });
});
