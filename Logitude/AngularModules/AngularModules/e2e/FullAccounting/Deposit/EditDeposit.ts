import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class EditDeposit {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    EditDeposit() {
       // browser.sleep(5000);
        this.Helper.WaitByIdAndClick('DepositsQuery');
        
        this.Helper.WaitByIdAndClick('row0col0');
        this.Helper.ItemsVisibility('BankDeposit.TH.Details');
        this.Helper.ItemsPresent('BankDeposit.TH.Details');
        this.Helper.WaitByIdAndClick('BankDeposit.TH.Details');
        
       // this.Helper.WaitBusyIndicator();
      //  this.Helper.ItemsVisibility('NewButton_BankAccount');

// browser.driver.sleep(5000)
       // browser.sleep(5000);



    }
}


