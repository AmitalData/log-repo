import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
export class NewTask {
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;


  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }


  public CreateNewTask(taskNo: string) {

    this.Helper.WaitByIdAndClick('NEWACTIVITY');
    this.Helper.WaitByIdAndClick('NEWTASK');

    this.FillTaskFields(taskNo);
    this.Helper.WaitByIdAndClick('Ok-AddActivity');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();

  }

  FillTaskFields(taskNo: string) {
    this.Helper.WaitByIdAndFill('Activity_Subject', taskNo);

    this.Helper.WaitByIdAndFill('Activity_Description', 'Task Description - Protractor ');// test random number randomWholeNum


    this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

    this.Helper.WaitByIdAndFill('date_Activity_StartDateTime', '2');
    this.Helper.WaitByIdAndFill('time_Activity_StartDateTime', '15');

    this.Helper.WaitByIdAndFill('date_Activity_DueDate', '3');
    this.Helper.WaitByIdAndFill('time_Activity_DueDate', '17');

  }
  
}



