import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';

export class NewShipper {

    private helper: FieldsHelper;
    private logitudeTab: GeneralFunctions;
    private shipperName: string;

    constructor() {
        this.helper = new FieldsHelper();
        this.shipperName = 'XYZShipper-' + Math.random();
    }


    QuickSearch() {


        this.helper.WaitBusyIndicator();
      //  this.helper.WaitByIdAndClick('General.MH.Maintenance');
        this.helper.waitByCss('#null_Search');

    }

    SearchShippertTab() {
       
        this.helper.WaitByIdAndFill('null_Search', "Shipper");
        this.helper.WaitByIdAndClick('MaintenanceItemMTCL');

    }

    CreateNewShipper() {
        this.helper.WaitByIdAndClick('NewButton_Customer');
        this.helper.WaitByIdAndFill('Address_Name', this.shipperName);
        this.helper.WaitByIdAndFill('Address_CountryId', "Italy");
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndFill('Address_City', "Florence");
        this.helper.WaitByIdAndClick('CheckBox_0_93_LBL')
        this.helper.WaitByIdAndFill('Address_ContactName', "Test123");
        this.helper.WaitByIdAndFill('Address_ContactEmail', "Test@mail.com");
        this.helper.WaitByIdAndClick('Ok-AddCustomer');

    }

    SearchShipper() {
        this.helper.WaitBusyIndicator();
        this.helper.WaitWindowClosed();
        this.helper.WaitByIdAndFill('SearchFieldsId_0_0', this.shipperName);
        this.helper.ItemsVisibility('LogGrid_0_0row0')
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('LogGrid_0_0row0');
    }

    EditOnShipper() {
        this.helper.WaitByIdAndClick('Customer.TH.General');
        this.helper.WaitByIdAndFill('Customer_LocalName', "Test Company 123")
        this.helper.WaitByIdAndClick('Customer.TH.Addresses');
        this.helper.WaitByIdAndClick('rowid')
        this.helper.WaitByIdAndClick('Edit');
        this.helper.WaitByIdAndFill('Address_Address1', "Palestine");
        this.helper.WaitByIdAndFill('Address_ZipCode', "123")
        this.helper.WaitByIdAndClick('OKButtonID');


    }

}

