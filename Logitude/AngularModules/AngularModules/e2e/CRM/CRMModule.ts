import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';
import { ActivitiesModule } from './Activities/ActivitiesModule';
import { OpportunityModule } from './Opportunities/OpportunitiesModule';
import { CustomerModule } from './Customers/CustomersModule';

export class CRMComp {
    private Helper: FieldsHelper;
    private CRMTab: GeneralFunctions;
    private Activities: ActivitiesModule;
    private Opportunities: OpportunityModule;
    private Customers: CustomerModule;
    constructor() {
        this.Helper = new FieldsHelper();
        this.CRMTab = new GeneralFunctions();
        this.Activities = new ActivitiesModule();
        this.Opportunities = new OpportunityModule();
        this.Customers = new CustomerModule();
    }
    DoCRM(CRMcomponent: string) {
        this.CRMTab.GoToMainMenu('General.MH.CRM');
        if (CRMcomponent == 'Overview') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMOVE');
        }
        else if (CRMcomponent == 'Customers') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMCUS');
            this.Customers.CreateCustomer();
        }
        else if (CRMcomponent == 'Quotes') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMQUT');
        }
        else if (CRMcomponent == 'Activities') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMACT');
            this.Activities.CreateActivity();
        }
        else if (CRMcomponent == 'Opportunities') {
            this.CRMTab.SelectMenuWorkSpaceTabs('CRMOPP');
            this.Opportunities.DoOpportunity();
        }
    }
}

