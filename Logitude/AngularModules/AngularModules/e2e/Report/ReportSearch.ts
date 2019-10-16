import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from './../Helpers/GeneralFunctions';

export class ReportSearch {
  private Helper: FieldsHelper;
  private logitudeTab: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.logitudeTab = new GeneralFunctions();
  }

  QuickSearch(reportName) {
    this.logitudeTab.GoToMainMenu('General.MH.Reports');
    this.UseSearchBox('null_Search', reportName);


  }

  UseSearchBox(searchFeildId: string, searchByRef: string) {
    this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
    this.Helper.WaitByIdAndClick("BI");
    this.Helper.WaitByIdAndClick("Report")
    this.Helper.ItemsVisibility('ReportID')
    this.Helper.WaitByIdAndClick('ReportID')

  }
}

