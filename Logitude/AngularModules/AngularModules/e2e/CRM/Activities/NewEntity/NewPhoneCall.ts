import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
export class NewPhoneCall {
  private Helper: FieldsHelper;
  private Generator : GeneralFunctions;


  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator=new GeneralFunctions();
  }


  public CreateNewPhoneCall() {
   
    this.Helper.WaitByIdAndClick('NEWACTIVITY');
    this.Helper.WaitByIdAndClick('NEWPHONECALL');

    this.FillPhoneCallFields();
    this.Helper.WaitByIdAndClick('Ok-AddActivity');
    this.Helper.WaitBusyIndicator();

  }



  FillPhoneCallFields() {
    this.Helper.WaitByIdAndFill('Activity_CustomerId', 'razan co');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

    this.Helper.WaitByIdAndFill('Activity_CallWithId', 'm');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

    this.Helper.WaitByIdAndFill('Activity_Subject', 'Phone Call - Added from Protractor');

    this.Helper.WaitByIdAndFill('Activity_Description', 'Phone Call Description - Protractor ');// test random number randomWholeNum


    this.Helper.WaitByIdAndFill('Activity_OwnerId', 'ra');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);


  }

}



