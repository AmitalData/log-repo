import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { NewTask } from './NewEntity/NewTask';
import { EditTasks } from './EditEntity/EditTask';

import { NewPhoneCall } from './NewEntity/NewPhoneCall';
import { EditPhoneCalls } from './EditEntity/EditPhoneCall';

import { NewAppointment } from './NewEntity/NewAppointment';
import { EditAppointments } from './EditEntity/EditAppointment';

import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class ActivitiesModule {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;

    private addTask: NewTask;
    private editTask: EditTasks;

    private addPhoneCall: NewPhoneCall;
    private editPhoneCall: EditPhoneCalls;

    private addAppointment: NewAppointment;
    private editAppointment: EditAppointments;

    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
        this.addTask = new NewTask();
        this.editTask = new EditTasks();

        this.addPhoneCall = new NewPhoneCall();
        this.editPhoneCall = new EditPhoneCalls();

        this.addAppointment = new NewAppointment();
        this.editAppointment = new EditAppointments();

    }

    public CreateActivity() {
        var activityNo = this.Generator.RandomNum();

        this.addTask.CreateNewTask('Task # ' + activityNo);
        this.QuickSearchBox('Activity_Search', 'Task # ' + activityNo);
        this.editTask.EditTask('Task # ' + activityNo);

        this.addPhoneCall.CreateNewPhoneCall('Phone Call # ' + activityNo);
        this.QuickSearchBox('Activity_Search', 'Phone Call # ' + activityNo);
        this.editPhoneCall.EditPhoneCall('Phone Call # ' + activityNo);

        this.addAppointment.CreateNewAppointment('Appointment # ' + activityNo);
        this.QuickSearchBox('Activity_Search', 'Appointment # ' + activityNo);
        this.editAppointment.EditAppointment('Appointment # ' + activityNo);
        // browser.driver.sleep(6000);

    }
    QuickSearchBox(searchFeildId: string, searchByRef: string) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.LogitudeQuickSearchItem', 0);
    }


}



