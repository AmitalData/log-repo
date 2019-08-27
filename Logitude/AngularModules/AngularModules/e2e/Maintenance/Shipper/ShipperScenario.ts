import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { LoginComp } from '../../Login/Login.po';
import { NewShipper } from './Shipper';


export class NewShipperScenario {

    private generalFun = new GeneralFunctions();
    private login: LoginComp = new LoginComp();
    private helper = new FieldsHelper();
    private Newshipper: NewShipper = new NewShipper();

    constructor() {
    }


    public Quicksearch() {

        this.Newshipper.QuickSearch();

    }

    public SearchShippertTab() {

        this.Newshipper.SearchShippertTab();

    }
    public CreateNewShipper() {

        this.Newshipper.CreateNewShipper();

    }
    public SearchShipper() {
        this.Newshipper.SearchShipper();

    }
    public EditOnShipper() {
        this.Newshipper.EditOnShipper();

    }

}
