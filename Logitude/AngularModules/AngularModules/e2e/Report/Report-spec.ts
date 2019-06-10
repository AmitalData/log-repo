import { LoginComp } from '../Login/Login.po';
import { browser, by, element } from 'protractor';
import { ReportSearch } from '../Report/ReportSearch';
import { ReportGenerator } from '../Report/ReportGenerator';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
describe('Report', () => {


  let page: LoginComp = new LoginComp();
  let searchPage: ReportSearch = new ReportSearch();
  let reportGenerator = new ReportGenerator();
  let generalFun = new GeneralFunctions();

  beforeEach(() => {
    browser.driver.manage().window().maximize();
    browser.ignoreSynchronization = true;
  });


  it('Run Report Sucssefuly', function () {
    // generalFun.GoToMainMenu('General.MH.Reports');
    // generalFun.UseSearchBox('null_Search','unpaid');
    searchPage.QuickSearch('Automation Test Report');
    reportGenerator.RunReportSuccessfully('RunReportSucceededDiv');
    // reportGenerator.CheckBox();
    // reportGenerator.Partner();
    //reportGenerator.ChooseTemplate();
    //reportGenerator.CalenderDate();
    // reportGenerator.runReport();
    //browser.driver.sleep(2000);
    //reportGenerator.saveReport();
    //browser.driver.sleep(5000);
  });


  it('Run Report Faield', function () {
    reportGenerator.RunReportFailed('RunReportFailedDiv');
   

  });
});
