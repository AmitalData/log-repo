
import { browser, by, element } from 'protractor';
import {ChartOFAccountModule} from './ChartOFAccountModule'
import{ GeneralFunctions } from'../../Helpers/GeneralFunctions';
import{ FieldsHelper} from '../../Helpers/fieldshelper'

describe('CRM Module',function (){
    let chartofaccount = new ChartOFAccountModule();
    let z:GeneralFunctions=new GeneralFunctions();
    let y: FieldsHelper=new FieldsHelper();



  it('Operations Success', function () {
    browser.ignoreSynchronization = true;
    browser.sleep(5000);
     z.GoToMainMenu('General.MH.Maintenance');
     y.WaitByIdAndClick('ACC');
     y.WaitByCssStringAndClick('BoxItem','Chart Of Accounts')
     y.WaitByIdAndClick('NewButton_ChartOfAccount')
     chartofaccount.CreateAndEditChartOfAccount();

    // CRMPage.DoCRM('Overview');
  //  CRMPage.DoCRM('Customers');
    // CRMPage.DoCRM('Quotes');
    // CRMPage.DoCRM('Activities');
    // CRMPage.DoCRM('Opportunities');
   
  });

});
