import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

export class NewPotentialCustomer{
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }


  public CreateNewPotentialCustomer(customerNo: string) {

    this.Helper.WaitByIdAndClick('NewCustomer');

    this.FillPotentialCustomerFields(customerNo);
    this.Helper.WaitByIdAndClick('Ok-AddPotCustomer');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();

  }

  FillPotentialCustomerFields(customerNo: string) {
    this.Helper.WaitByIdAndFill('Customer_EnglishName', customerNo);

    this.Helper.WaitByIdAndFill('Customer_VatNumber', '1111155');

    this.Helper.WaitByIdAndFill('Customer_Address1_Potential', 'Ramallah');
    this.Helper.WaitByIdAndFill('Customer_Address2_Potential', 'Nablus');
    this.Helper.WaitByIdAndFill('Customer_ZipCode_Potential', '00970');

    this.Helper.WaitByIdAndFill('Customer_CountryId_Potential', 'p');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

    // ---------- Contact

    this.Helper.WaitByIdAndFill('Contact_Email', 'contactus@mail.com');

    this.Helper.WaitByIdAndFill('Customer_EntityNotes', 'Potential Customer');


  }
  
}



