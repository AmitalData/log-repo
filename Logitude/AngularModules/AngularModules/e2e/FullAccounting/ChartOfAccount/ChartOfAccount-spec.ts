
import { browser, by, element } from 'protractor';
import {ChartOFAccountModule} from './ChartOFAccountModule'
import{ GeneralFunctions } from'../../Helpers/GeneralFunctions';
import { FieldsHelper } from '../../Helpers/fieldshelper'
import { NewChartOfAccount } from './NewEntity/NewChartOfAccount';
import { EditChartOfAccount } from './EditEntity/EditChartOfAccount';

describe('CRM Module',function (){
    let chartofaccount = new ChartOFAccountModule();
    let z:GeneralFunctions=new GeneralFunctions();
    let y: FieldsHelper=new FieldsHelper();
    let c: NewChartOfAccount = new NewChartOfAccount();
    let E: EditChartOfAccount = new EditChartOfAccount();


  it('Chart Of Account Success', function () {
    browser.ignoreSynchronization = true;

    z.GoToMainMenu('General.MH.Maintenance');
     y.WaitByIdAndClick('ACC');
     y.WaitByIdAndClick('MaintenanceItemMTCA')
     y.WaitByIdAndClick('NewButton_ChartOfAccount')
      // chartofaccount.CreateAndEditChartOfAccount();
      var chartOfAccountNo = this.GeneralFun.RandomNumAcc();
      c.CreateNewChartOFAccount(chartOfAccountNo, 'Customer');
      E.EditChartOfAccount(chartOfAccountNo+'C');



    // CRMPage.DoCRM('Overview');
  //  CRMPage.DoCRM('Customers');
    // CRMPage.DoCRM('Quotes');
    // CRMPage.DoCRM('Activities');
    // CRMPage.DoCRM('Opportunities');
   
  });

});
