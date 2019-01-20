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
    public EditAppointment(appointmentNo: string) {

        this.EditAppointmentGeneralTab(appointmentNo);
        this.Helper.WaitByIdAndClick('Activity-SaveClose');
this.Helper.WaitBusyIndicator();


var EC = protractor.ExpectedConditions;
browser.wait(EC.invisibilityOf(element(by.id('Activity.B.MarkAsComplete'))), 100000).then(a => {
});

    }


    EditAppointmentGeneralTab(appointmentDesc: string) {
        this.Helper.WaitByIdAndFill('Activity_Subject', 'Edit Subject for '+ appointmentDesc);
        this.Helper.WaitByIdAndFill('Activity_Location', 'Ramallah');
    
        this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);
    
        this.Helper.WaitByIdAndFill('Activity_Description',  'Edit Description for '+appointmentDesc);

    }

}



