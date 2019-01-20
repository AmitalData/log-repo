import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
export class EditPhoneCalls {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;

    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
    }
    public EditPhoneCall(phoneCallNo: string) {

        this.EditPhoneCallGeneralTab(phoneCallNo);
        this.Helper.WaitByIdAndClick('Activity-SaveClose');
        this.Helper.WaitBusyIndicator();


        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id('Activity.B.MarkAsComplete'))), 100000).then(a => {
        });

    }


    EditPhoneCallGeneralTab(phoneCallDesc: string) {
        this.Helper.WaitByIdAndFill('Activity_CustomerId', 'razan j');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Activity_CallWithId', 'razan');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for ' + phoneCallDesc);

        this.Helper.WaitByIdAndFill('Activity_Description', 'Edit Description for ' + phoneCallDesc);// test random number randomWholeNum


        // this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);


    }

}



