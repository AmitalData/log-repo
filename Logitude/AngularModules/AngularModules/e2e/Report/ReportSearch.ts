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
   // this.UseSearchBox('null_Search', 'Unpaid Invoices');
    this.UseSearchBox('null_Search',reportName);

  }

  UseSearchBox(searchFeildId: string, searchByRef: string) {
    this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
    this.Helper.WaitByCssAndClick_FromTagInsideList('.HyperlinkQueryButtonControl', 0);
    
  }
}

