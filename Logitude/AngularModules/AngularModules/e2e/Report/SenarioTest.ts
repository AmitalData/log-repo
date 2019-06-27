import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
import { LoginComp } from '../Login/Login.po';
import { ReportSearch } from '../Report/ReportSearch';
import { ReportGenerator } from '../Report/ReportGenerator';
import { OperationsComp } from '../Operations/Shipments/NewEntity/Operations.po';



export class SenarioTest {

  //private page: OperationsComp = new OperationsComp();
  private searchPage: ReportSearch = new ReportSearch();
  private reportGenerator = new ReportGenerator();
    private generalFun = new GeneralFunctions();
    constructor() {
    }
    public ReportScienarios() {

        this.searchPage.QuickSearch('Automation Test Report');
        this.reportGenerator.RunReportSuccessfully('RunReportSucceededDiv');

     
    }

    public FailedScienarios() {
        this.reportGenerator.RunReportFailed('RunReportFailedDiv');
    }   


}

