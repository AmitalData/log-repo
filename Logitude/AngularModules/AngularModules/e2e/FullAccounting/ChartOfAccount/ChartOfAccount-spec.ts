
import { browser, by, element } from 'protractor';
//import {ChartOFAccountModule} from './ChartOFAccountModule'
import{ GeneralFunctions } from'../../Helpers/GeneralFunctions';
import { FieldsHelper } from '../../Helpers/fieldshelper'
import { NewChartOfAccount } from './NewChartOfAccount';
import { EditChartOfAccount } from './EditChartOfAccount';

describe('ChartOfAccount Module',function (){
    //let chartofaccount = new ChartOFAccountModule();
    let z:GeneralFunctions=new GeneralFunctions();
    let y: FieldsHelper=new FieldsHelper();
    let c: NewChartOfAccount = new NewChartOfAccount();
    let E: EditChartOfAccount = new EditChartOfAccount();


  it('Chart Of Account Success', function () {
    browser.ignoreSynchronization = true;


    console.log('Go To maintenance ')
    browser.ignoreSynchronization = true;
    z.GoToMainMenu('General.MH.Maintenance');
    
      var chartOfAccountNo = z.RandomNumACCWithChars(); 
    c.CreateNewChartOFAccount(chartOfAccountNo, 'Customer');
    E.EditChartOfAccount(chartOfAccountNo);
  



    
     //y.WaitByIdAndClick('ACC');
   
   //  y.WaitByIdAndClick('NewButton_ChartOfAccount')
      // chartofaccount.CreateAndEditChartOfAccount();
     
      
     // E.EditChartOfAccount(chartOfAccountNo+'C');
   // browser.sleep(5000);



    // CRMPage.DoCRM('Overview');
  //  CRMPage.DoCRM('Customers');
    // CRMPage.DoCRM('Quotes');
    // CRMPage.DoCRM('Activities');
    // CRMPage.DoCRM('Opportunities');
   
  });

});
