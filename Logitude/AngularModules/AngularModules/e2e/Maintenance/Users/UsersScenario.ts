import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { LoginComp } from '../../Login/Login.po';
import { NewUser } from './Users';


export class NewUserScenario {

    private generalFun = new GeneralFunctions();
    private login: LoginComp = new LoginComp();
    private helper = new FieldsHelper();
    private NewUser: NewUser = new NewUser();

    constructor() {
    }


    public Quicksearch() {

        this.NewUser.QuickSearch();

    }

    public SearchUserTab() {

        this.NewUser.SearchUserTab();

    }
    public CreateNewUser() {

        this.NewUser.CreateNewUser();

    }
    public SearchUser() {
        this.NewUser.SearchUser();

    }


}
