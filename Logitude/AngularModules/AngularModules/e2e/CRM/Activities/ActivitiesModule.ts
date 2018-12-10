import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { NewTask } from './NewEntity/NewTask';
import { NewPhoneCall } from './NewEntity/NewPhoneCall';
import { NewAppointment } from './NewEntity/NewAppointment';

export class ActivitiesModule {
    private Helper: FieldsHelper;

    private addTask: NewTask;
    private addPhoneCall: NewPhoneCall;
    private addAppointment: NewAppointment;


    constructor() {
        this.Helper = new FieldsHelper();
        this.addTask = new NewTask();
        this.addPhoneCall = new NewPhoneCall();
        this.addAppointment = new NewAppointment();
    }

    public CreateActivity() {

        this.addTask.CreateNewTask();
        browser.driver.sleep(200);
        this.addPhoneCall.CreateNewPhoneCall();
        this.addAppointment.CreateNewAppointment();


    }



}



