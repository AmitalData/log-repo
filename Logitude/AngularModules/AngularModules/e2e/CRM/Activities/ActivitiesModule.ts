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
    private GeneralFun: GeneralFunctions;

    private addTask: NewTask;
    private editTask: EditTasks;

    private addPhoneCall: NewPhoneCall;
    private editPhoneCall: EditPhoneCalls;

    private addAppointment: NewAppointment;
    private editAppointment: EditAppointments;

    constructor() {
        this.Helper = new FieldsHelper();
        this.GeneralFun = new GeneralFunctions();
        this.addTask = new NewTask();
        this.editTask = new EditTasks();

        this.addPhoneCall = new NewPhoneCall();
        this.editPhoneCall = new EditPhoneCalls();

        this.addAppointment = new NewAppointment();
        this.editAppointment = new EditAppointments();

    }

    public CreateActivity() {
        var activityNo = this.GeneralFun.RandomNum();

        this.editTask.EditTask('Task # ' + activityNo);
        this.addTask.CreateNewTask('Task # ' + activityNo);
        this.GeneralFun.QuickSearchTextBox('Activity_Search', 'Task # ' + activityNo);

        this.addPhoneCall.CreateNewPhoneCall('Phone Call # ' + activityNo);
        this.GeneralFun.QuickSearchTextBox('Activity_Search', 'Phone Call # ' + activityNo);
        this.editPhoneCall.EditPhoneCall('Phone Call # ' + activityNo);

        this.addAppointment.CreateNewAppointment('Appointment # ' + activityNo);
        this.GeneralFun.QuickSearchTextBox('Activity_Search', 'Appointment # ' + activityNo);
        this.editAppointment.EditAppointment('Appointment # ' + activityNo);
        // browser.driver.sleep(6000);

    }
    


}



