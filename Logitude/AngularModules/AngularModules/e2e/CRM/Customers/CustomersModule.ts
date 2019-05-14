import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { NewPotentialCustomer } from './NewEntity/NewPotentialCustomer';

import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class CustomerModule {
    private Helper: FieldsHelper;
    private GeneralFun: GeneralFunctions;

    private addPotCustomer: NewPotentialCustomer;


    constructor() {
        this.Helper = new FieldsHelper();
        this.GeneralFun = new GeneralFunctions();
        this.addPotCustomer = new NewPotentialCustomer();
    }

    public CreateCustomer() {
        var customerNo = this.GeneralFun.RandomNum();

        this.addPotCustomer.CreateNewPotentialCustomer('Customer # ' + customerNo);
        this.GeneralFun.QuickSearchTextBox('Card_Search', 'Customer # ' + customerNo);
     
        // browser.driver.sleep(6000);

    }
  

}



