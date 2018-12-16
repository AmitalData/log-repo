import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
export class EditAppointments {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;

    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
    }
    public EditAppointment(appointmentDesc: string) {

        this.EditAppointmentGeneralTab(appointmentDesc);
        this.Helper.WaitByIdAndClick('Activity-SaveClose');

    }


    EditAppointmentGeneralTab(appointmentDesc: string) {
        this.Helper.WaitByIdAndFill('Activity_Subject', appointmentDesc);
        this.Helper.WaitByIdAndFill('Activity_Location', 'Ramallah');
    
        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
    
        this.Helper.WaitByIdAndFill('Activity_Description', appointmentDesc);

    }

}



