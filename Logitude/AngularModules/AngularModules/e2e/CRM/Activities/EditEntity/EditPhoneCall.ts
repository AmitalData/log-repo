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

    }


    EditPhoneCallGeneralTab(phoneCallDesc: string) {
        this.Helper.WaitByIdAndFill('Activity_CustomerId', 'Edit Ta');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Activity_CallWithId', 'razan');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for ' + phoneCallDesc);

        this.Helper.WaitByIdAndFill('Activity_Description', 'Edit Description for ' + phoneCallDesc);// test random number randomWholeNum


        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);


    }

}



