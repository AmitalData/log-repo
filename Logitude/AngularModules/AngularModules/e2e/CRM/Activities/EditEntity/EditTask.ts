
import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
export class EditTasks {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;


    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
    }
    public EditTask(taskNo: string) {

        this.EditTaskGeneralTab(taskNo);
        this.Helper.WaitByIdAndClick('Activity-SaveClose');

    }


    EditTaskGeneralTab(taskDesc: string) {
        this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for ' + taskDesc);

        this.Helper.WaitByIdAndFill('Activity_Description', 'Edit Description for ' + taskDesc);// test random number randomWholeNum


        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'r');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

        this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
        // this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');

        this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
        // this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');

    }

}



