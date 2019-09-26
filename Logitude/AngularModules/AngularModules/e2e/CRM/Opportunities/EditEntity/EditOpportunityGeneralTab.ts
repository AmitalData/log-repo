
import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

export class EditOpportunityGeneralTab {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;

    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
    }
    public EditGeneralTab(opportunityDesc: string) {
        this.Helper.WaitByIdAndClick('Opportunity.TH.General');
        this.EditGeneralTabFeilds(opportunityDesc);
        this.Helper.WaitByIdAndClick('Opportunity-Save');
        this.Helper.WaitBusyIndicator();
        
    }
    EditGeneralTabFeilds(opportunityDesc: string) {
        this.Helper.WaitByIdAndFill('Opportunity_LeadSourceId','Advertisement');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Opportunity_LeadDescription','Lead Details');

        this.Helper.WaitByIdAndFill('Opportunity_LeadPartnerId', 'TestShipper');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        

        

    }

}



