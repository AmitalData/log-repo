import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { LoginComp } from '../../Login/Login.po';
import { CompanyAddressSetting } from './CompanyAddressSetting';



export class CompanyAddressSettingScenario {

    private generalFun = new GeneralFunctions();
    private login: LoginComp = new LoginComp();
    private helper = new FieldsHelper();
    private CompanyAddress: CompanyAddressSetting = new CompanyAddressSetting();

    constructor() {
    }


    public Quicksearch() {

        this.CompanyAddress.QuickSearch();


    }

    public SearchCompanyAddressSetting() {

        this.CompanyAddress.SearchCompanyAddressSetting();


    }

    public EditCompanyAddressSitting() {

        this.CompanyAddress.EditCompanyAddressSitting();


    }






}
