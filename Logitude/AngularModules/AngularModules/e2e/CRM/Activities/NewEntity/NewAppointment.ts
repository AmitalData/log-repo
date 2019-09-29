import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { createBreak } from 'typescript';
export class NewAppointment {
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;
  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }
  public CreateNewAppointment(AppointmentNo: string) {
    this.Helper.WaitByIdAndClick('NEWACTIVITY');
    this.Helper.WaitByIdAndClick('NEWAPPOINTMENT');

    this.FillAppointmentFields(AppointmentNo);

  }
  FillAppointmentFields(AppointmentNo: string) {
    this.Helper.WaitByIdAndFill('Activity_Subject', 'Create  ' + AppointmentNo + ' : Protractor ..');
    this.Helper.WaitByIdAndFill('Activity_Location', 'Ramallah');

    // this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
    // this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');

    // this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
    // this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');

    this.Helper.WaitByIdAndFill('Activity_OwnerId', 'Protractor user');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    if(AppointmentNo!='Created from appointment ...'){
      this.Helper.WaitByIdAndFill('Activity_CustomerId', 'Customer Activity');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);  
    }
    this.Helper.WaitByIdAndFill('Activity_Description', AppointmentNo);
    this.Helper.WaitByIdAndClick('Ok-AddActivity');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
  }
}



