import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { NewPotentialCustomer } from './NewEntity/NewPotentialCustomer';

import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class CustomerModule {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;

    private addPotCustomer: NewPotentialCustomer;


    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
        this.addPotCustomer = new NewPotentialCustomer();
    }

    public CreateCustomer() {
        var activityNo = this.Generator.RandomNum();

        this.addPotCustomer.CreateNewPotentialCustomer('Customer # ' + activityNo);
        this.QuickSearchBox('Opportunity_Search', 'Customer # ' + activityNo);
     
        // browser.driver.sleep(6000);

    }
    QuickSearchBox(searchFeildId: string, searchByRef: string) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.LogitudeQuickSearchItem', 0);
    }


}



