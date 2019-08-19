import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from './../Helpers/GeneralFunctions';

export class CompanyAddressSetting{

    private helper: FieldsHelper;
    private logitudeTab: GeneralFunctions;

    constructor() {
        this.helper = new FieldsHelper();
        this.logitudeTab = new GeneralFunctions();

    }

    QuickSearch() {

        this.logitudeTab.GoToMainMenu('General.MH.Maintenancets');
    }


    SearchCompanyAddressSetting() {

        this.helper.WaitByIdAndFill('null_Search', "company address setting");
        this.helper.WaitByIdAndClick('MaintenanceItemCOAD');

    }

    EditCompanyAddressSitting() {

   this.helper.WaitByIdAndFill('textboxdiv_Address_Address2',"Ramallah");
   this.helper.WaitByIdAndFill('Address_ZipCode',"99988");
   this.helper.WaitByIdAndClick('OkButton');


    }








}