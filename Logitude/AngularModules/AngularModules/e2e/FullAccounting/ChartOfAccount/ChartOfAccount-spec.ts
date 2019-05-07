
import { browser, by, element } from 'protractor';
import {ChartOFAccountModule} from './ChartOFAccountModule'
import{ GeneralFunctions } from'../../Helpers/GeneralFunctions';
import{ FieldsHelper} from '../../Helpers/fieldshelper'

describe('CRM Module',function (){
    let chartofaccount = new ChartOFAccountModule();
    let z:GeneralFunctions=new GeneralFunctions();
    let y: FieldsHelper=new FieldsHelper();



  it('Chart Of Account Success', function () {
    browser.ignoreSynchronization = true;

    z.GoToMainMenu('General.MH.Maintenance');
     y.WaitByIdAndClick('ACC');
     y.WaitByIdAndClick('MaintenanceItemMTCA')
     y.WaitByIdAndClick('NewButton_ChartOfAccount')
     chartofaccount.CreateAndEditChartOfAccount();

    // CRMPage.DoCRM('Overview');
  //  CRMPage.DoCRM('Customers');
    // CRMPage.DoCRM('Quotes');
    // CRMPage.DoCRM('Activities');
    // CRMPage.DoCRM('Opportunities');
   
  });

});
