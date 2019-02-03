import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

export class NewOpportunity {
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }


  public CreateNewOpportunity(opportunityNo: string) {

    this.Helper.WaitByIdAndClick('NEWOPPORTUNITY');

    this.FillOpportunityFields(opportunityNo);
    this.Helper.WaitByIdAndClick('Ok-AddOpportunity');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();

  }

  FillOpportunityFields(opportunityNo: string) {
    
    this.Helper.WaitByIdAndFill('Opportunity_OpportunityTypeId', 'i');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

    this.Helper.WaitByIdAndFill('Opportunity_Subject', opportunityNo);

    this.Helper.WaitByIdAndFill('date_Opportunity_EstimatedClosingDate', '1');

    this.Helper.WaitByIdAndFill('Opportunity_CustomerId', 'razan');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

    this.Helper.WaitByIdAndFill('Opportunity_Notes', 'Opportunity_Notes - Protractor ');// test random number randomWholeNum
    
    // this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
    // this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');

    // this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
    // this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');

  }
  
}



