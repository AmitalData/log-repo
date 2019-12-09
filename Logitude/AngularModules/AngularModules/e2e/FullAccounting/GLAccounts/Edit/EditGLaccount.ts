import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { browser, by, element } from 'protractor';



export class EditGLAccount {
    private Helper = new FieldsHelper();
    private gn = new GeneralFunctions();
    constructor() {
    }

    EditGLAccount(DisplayNumber: string) {
        this.Helper.WaitByIdAndFill('CardGLAccount_Search', DisplayNumber);



        this.Helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('GLAccount.TH.General');
        this.WaitBusyIndicatorToShowandHide();
        this.Helper.WaitByIdAndFill('GLAccount_LocalName', 'Updated Local Name')
        this.Helper.WaitBusyIndicator();
        this.Helper.ItemsVisibility('GLAccount-Save');
        this.Helper.ItemsPresent('GLAccount-Save');
        this.Helper.WaitByIdAndClick('GLAccount-Save');
        this.WaitBusyIndicatorToShowandHide();
        //this.Helper.WaitBusyIndicator();


        // browser.sleep(5000);



    }
    WaitBusyIndicatorToShowandHide() {
        this.Helper.WaitShowEditComponentBusyIndicator();
        this.Helper.WaitEditComponentBusyIndicator();
    }
}







