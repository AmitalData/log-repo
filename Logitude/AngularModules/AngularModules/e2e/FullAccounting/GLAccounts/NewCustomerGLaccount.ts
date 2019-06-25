import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewCustomer{
  private Helper: FieldsHelper;
  private Generator: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.Generator = new GeneralFunctions();
  }


  public CreateNewCustomerGLAccount(Name: string) {

    this.Helper.WaitByIdAndClick('NewCustomer');
      this.Helper.WaitByIdAndFill('Customer_EnglishName', Name);
      this.Helper.WaitByIdAndFill('Customer_LocalName', Name);
    this.Helper.WaitByIdAndFill('Customer_Address1_Potential', 'Ramallah');
    this.Helper.WaitByIdAndFill('Customer_Address2_Potential', 'Nablus');
    this.Helper.WaitByIdAndFill('Customer_ZipCode_Potential', '00970');
    this.Helper.WaitByIdAndFill('Customer_CountryId_Potential', 'ps');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndFill('Customer_City_Potential','Nablus');
    this.Helper.WaitByIdAndFill('Contact_EnglishName', 'My Contact');
    this.Helper.WaitByIdAndClick('Ok-AddPotCustomer');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
     




  }

  public ActivateCustomerGLAccount(Name: string,DisplayNumber: string){
      this.Generator.QuickSearchTextBox('Card_Search', Name);
      this.Helper.WaitByIdAndClick('Customer.B.Activate');
      this.Helper.WaitByIdAndClick('Ok-activate');
      this.Helper.WaitBusyIndicator();
      this.Helper.WaitWindowClosed();
      this.Helper.WaitByIdAndClick('Customer.TH.Accounting');
      this.Helper.WaitByIdAndClick('Activate');
      this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'cus');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
      //this.Helper.WaitByIdAndFill('GLAccount_LocalName', Name+DisplayNumber);
      this.Helper.WaitByIdAndFill('GLAccount_CurrencyId','Nis');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
      this.Helper.WaitWindowClosed();
     this.Helper.WaitBusyIndicator();
     //this.Helper.WaitBusyIndicator();
     var boo=this.Helper.ItemsVisibility('3mo');
     if ('boo'){
        this.Helper.WaitByIdAndClick('Customer-SaveClose');
     }
   
 




}
  
}



