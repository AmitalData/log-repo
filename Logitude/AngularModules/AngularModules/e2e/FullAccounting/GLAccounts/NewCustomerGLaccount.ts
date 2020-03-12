import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewCustomer {
    private Helper: FieldsHelper;
    private Generator: GeneralFunctions;

    constructor() {
        this.Helper = new FieldsHelper();
        this.Generator = new GeneralFunctions();
    }


    public CreateNewCustomerGLAccount(Name: string) {

        this.Helper.WaitByIdAndClick('NewButton_Customer');
      this.Helper.ItemsVisibility('Address_Name');
      this.Helper.ItemsPresent('Address_Name');
      this.Helper.WaitByIdAndFill('Address_Name', Name);
      this.Helper.WaitByIdAndFill('Address_LocalName', Name);
      this.Helper.WaitByIdAndFill('Address_Address1', 'Ramallah');
      this.Helper.WaitByIdAndFill('Address_Address2', 'Nablus');
      this.Helper.WaitByIdAndFill('Address_ZipCode', '00970');
      this.Helper.WaitByIdAndFill('Address_CountryId', 'ps');
      this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Address_CountryId', 'ps')
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();

      this.Helper.WaitByIdAndFill('Address_City', 'Nablus');
        this.Helper.WaitBusyIndicator();

       // this.Helper.WaitByIdAndFill('Contact_EnglishName', 'My Contact');
       // this.Helper.WaitBusyIndicator();

      this.Helper.WaitByIdAndClick('Ok-AddCustomer');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitWindowClosed();





    }

    public ActivateCustomerGLAccount(Name: string) {
       // this.Generator.QuickSearchTextBox('SearchFieldsId_0_0', Name);
        this.Helper.WaitByIdAndFill('SearchFieldsId_0_0', Name);
        this.Helper.WaitElementToBeDisplayedInTheList('.TextTrimming', Name);
        this.Helper.WaitByIdAndClick('LogGrid_0_0row0');
      //  this.Helper.WaitByIdAndClick('Ok-activate');
       // this.Helper.WaitBusyIndicator();
       // this.Helper.WaitWindowClosed();
        this.Helper.WaitByIdAndClick('Customer.TH.Accounting');
        this.Helper.WaitByIdAndClick('Activate');
        this.Helper.WaitByIdAndFill('GLAccount_ChartOfAccountsId', 'cust');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();

        // this.Helper.WaitByIdAndFill('GLAccount_DisplayNumber', DisplayNumber);
        //this.Helper.WaitByIdAndFill('GLAccount_LocalName', Name+DisplayNumber);
        this.Helper.WaitByIdAndFill('GLAccount_CurrencyId', 'Nis');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitBusyIndicator();


        this.Helper.WaitByIdAndClick('Ok-AddGLAccount');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
        //this.Helper.WaitBusyIndicator();
        // var boo=this.Helper.ItemsVisibility('3mo');
        //if ('boo') {
        //  this.Helper.WaitBusyIndicator();
        // this.Helper.WaitByIdAndClick('Customer-SaveClose');
        // }






    }

}


