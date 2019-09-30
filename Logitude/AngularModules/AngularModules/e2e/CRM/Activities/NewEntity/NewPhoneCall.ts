import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
export class NewPhoneCall {
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }
  public CreateNewPhoneCall(phoneCallNo: string) {
    this.Helper.WaitByIdAndClick('NEWACTIVITY');
    this.Helper.WaitByIdAndClick('NEWPHONECALL');

    this.FillPhoneCallFields(phoneCallNo);


  }
  FillPhoneCallFields(phoneCallNo: string) {
    if (phoneCallNo != 'Created from Opportunity') {
      this.Helper.WaitByIdAndFill('Activity_CustomerId', 'Customer Activity');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    }
    this.Helper.WaitByIdAndFill('Activity_CallWithId', 'Protractor Conatact');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Activity_Subject', phoneCallNo);

    this.Helper.WaitByIdAndFill('Activity_Description', 'Phone Call Description - Protractor ');// test random number randomWholeNum

    this.Helper.WaitByIdAndFill('Activity_OwnerId', 'Protractor user');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndClick('Ok-AddActivity');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
  }
}



