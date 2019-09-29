
import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { NewPhoneCall } from '../../Activities/NewEntity/NewPhoneCall';
import { NewTask } from '../../Activities/NewEntity/NewTask';
import { NewAppointment } from '../../Activities/NewEntity/NewAppointment';

export class EditOpportunityMainTab {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;
    private newTask: NewTask = new NewTask();
    private newPhoneCall: NewPhoneCall = new NewPhoneCall();
    private newAppointment: NewAppointment = new NewAppointment();

    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
    }
    public EditMainTab(opportunityDesc: string) {
        this.Helper.WaitByIdAndClick('Opportunity.TH.Overview');
        this.EditMainTabFeilds(opportunityDesc);


    }
    EditMainTabFeilds(opportunityDesc: string) {
        this.Helper.WaitByIdAndFill('Opportunity_NumberOfShipments', '25');
        this.Helper.WaitByIdAndClick('AddPhoneCall');
        this.newPhoneCall.FillPhoneCallFields('Created from appointment');

        this.Helper.WaitByIdAndClick('AddTask');
        this.newTask.FillTaskFields('Created from appointment ...');

        this.Helper.WaitByIdAndClick('AddAppointment');
        this.newAppointment.FillAppointmentFields('Created from appointment ...');

        this.Helper.WaitByIdAndClick('AddQuote');
        this.newAppointment.FillAppointmentFields('Created from appointment ...');




    }

}



