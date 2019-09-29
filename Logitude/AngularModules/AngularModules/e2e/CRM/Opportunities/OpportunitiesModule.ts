import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { NewOpportunity } from './NewEntity/NewOpportunity';
import {EditOpportunityMainTab  } from './EditEntity/EditOpportunityMainTab';
import {EditOpportunityGeneralTab} from './EditEntity/EditOpportunityGeneralTab';
import {OpportunityActions} from './EditEntity/OpportunitiesActions';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
export class OpportunityModule {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;

    private addOpportunity: NewOpportunity;
    private editMainTab: EditOpportunityMainTab;
    private editGeneralTab: EditOpportunityGeneralTab;
    private OpportunityActions: OpportunityActions;
    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
        this.addOpportunity = new NewOpportunity();
        this.editMainTab = new EditOpportunityMainTab();

        this.editGeneralTab = new EditOpportunityGeneralTab();
        this.OpportunityActions = new OpportunityActions();
    }
    public DoOpportunity() {
        var OpportunityNo = this.Generator.RandomNum();
        this.addOpportunity.CreateNewOpportunity('Opportunity # ' + OpportunityNo);
        this.Generator.QuickSearchTextBox('Opportunity_Search', 'Opportunity # ' + OpportunityNo);

        this.editMainTab.EditMainTab('Opportunity # ' + OpportunityNo);
        this.editGeneralTab.EditGeneralTab('Opportunity # ' + OpportunityNo);

       this.OpportunityActions.OpportunityActions();
    }
}



