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
    // this.Helper.WaitByIdAndClick('Cancel-DontCreatePotCustomer');
    
    // this.Helper.WaitByIdAndClick('NewCustomer');
    // browser.driver.sleep(5000);
    this.FillPotentialCustomerFields(customerNo);

    this.Helper.WaitByIdAndClick('Ok-AddPotCustomer');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();

  }

  FillPotentialCustomerFields(customerNo: string) {

this.Helper.WaitByIdAndClick('Shipper/ConsigneeisNOTmycustomer_CustomerRadio_LBL');

    this.Helper.WaitByIdAndFill('Customer_EnglishName', customerNo);

    this.Helper.WaitByIdAndFill('Customer_VatNumber', '1111155');

    this.Helper.WaitByIdAndFill('Customer_Address1_Potential', 'Ramallah');
    this.Helper.WaitByIdAndFill('Customer_Address2_Potential', 'Nablus');
    this.Helper.WaitByIdAndFill('Customer_ZipCode_Potential', '00970');

    this.Helper.WaitByIdAndFill('Customer_CountryId_Potential', 'p');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

    // ---------- Contact

this.Helper.WaitByIdAndClick('CheckBox_0_7_LBL');
     browser.driver.sleep(5000);


  // var EC = protractor.ExpectedConditions;
  // browser.wait(EC.elementToBeClickable(element(by.css('.LogitudeCheckBox'))), 100000000).then(a => {
  //   var button = element(by.css('.LogitudeCheckBox')).click();
  // });
  // <label _ngcontent-c23="" for="CheckBox_0_4"></label>
//   element(by.id('ProfitCurrency')).getAttribute('textContent').then(function (recCurrency) {            
//     receivableCurrency = recCurrency.trim();
//     if (receivableCurrency != null) {
//         console.log('Receivable  Profit Currency-ReceivableTab : ' + receivableCurrency);
//     } else {

//        s console.log('No Profit Currency in Receiavble Tab');
//     }
//     // return receivableCurrency;
// });

    // this.Helper.WaitByIdAndFill('Contact_Email', 'contactus@mail.com');
    // this.Helper.WaitByIdAndFill('Customer_EntityNotes', 'Potential Customer');
    // this.Helper.WaitBusyIndicator();

  }
  
}



