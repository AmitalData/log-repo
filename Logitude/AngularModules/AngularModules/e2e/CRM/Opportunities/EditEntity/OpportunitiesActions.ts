
import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

export class OpportunityActions {
    private Helper: FieldsHelper;
  

    constructor() {
        this.Helper = new FieldsHelper();
        
    }
    public OpportunityActions() {
      this.CloseAsWon();
    }

    CloseAsWon(){
      this.Helper.WaitByIdAndClick('Opportunity.B.CloseAsWon');
      this.Helper.WaitByIdAndFill('Opportunity_ClosingDescription','Closed As Won');
      this.Helper.WaitByIdAndClick('Ok-CloseAsWon');
      this.Helper.WaitBusyIndicator();
    }

    CloseAsLost(){
        this.Helper.WaitByIdAndClick('Opportunity.B.CloseAsLost');
        this.Helper.WaitByIdAndFill('Opportunity_ClosingReasonId','Lost');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Opportunity_ClosingDescription','Close As Lost');
        this.Helper.WaitByIdAndClick('Ok-CloseAsWon');
        this.Helper.WaitBusyIndicator();
    }
    ReOpen(StageId: string){
      this.Helper.WaitByIdAndClick('MenuButtons');
      this.Helper.WaitByIdAndClick('Opportunity.B.ReOpen');
      this.Helper.WaitByIdAndFill(StageId,'qua');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitByIdAndClick('OK-ReOpen');
      this.Helper.WaitBusyIndicator();
      
    }
    Copy(){
    this.Helper.WaitByIdAndClick('Opportunity.B.Copy');
    this.Helper.WaitByIdAndClick('Ok-AddOpportunity');
    this.Helper.WaitBusyIndicator();
    }
    Cancel(){
     this.Helper.WaitByIdAndClick('Opportunity.B.Cancel');
     this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
     this.Helper.WaitBusyIndicator();
    }
  
}



