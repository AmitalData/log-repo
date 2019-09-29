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
        this.OpportunityActions= new OpportunityActions();
    }

    public CreateOpportunity() {
        var OpportunityNo = this.Generator.RandomNum();

        this.addOpportunity.CreateNewOpportunity('Opportunity # ' + OpportunityNo);
        this.Generator.QuickSearchTextBox('Opportunity_Search', 'Opportunity # ' + OpportunityNo);
        //this.editMainTab.EditMainTab('Opportunity # ' + OpportunityNo);
        this.editGeneralTab.EditGeneralTab('Opportunity # ' + OpportunityNo);
        this.OpportunityActions.CloseAsWon();
        this.OpportunityActions.ReOpen('Opportunity_StageId_1');
        this.OpportunityActions.CloseAsLost();
        this.OpportunityActions.ReOpen('Opportunity_StageId_2');
        this.OpportunityActions.Copy();
        this.OpportunityActions.Cancel("MenuButtons_1");
        
    

    }
    QuickSearchBox(searchFeildId: string, searchByRef: string) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.LogitudeQuickSearchItem', 0);
    }


}



