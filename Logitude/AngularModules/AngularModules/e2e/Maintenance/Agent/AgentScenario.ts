import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { LoginComp } from '../../Login/Login.po';
import { NewAgent } from './Agent';


export class NewAgentScenario {

    private generalFun = new GeneralFunctions();
    private login: LoginComp = new LoginComp();
    private helper = new FieldsHelper();
    private NewAgent: NewAgent = new NewAgent();

    constructor() {
    }


    public Quicksearch() {

        this.NewAgent.QuickSearch();

    }

    public SearchAgentTab() {

        this.NewAgent.SearchAgentTab();

    }
    public CreateNewAgent() {

        this.NewAgent.CreateNewAgent();

    }
    public SearchAgent() {
        this.NewAgent.SearchAgent();

    }
    public SaveAgent() {
        this.NewAgent.SaveAgent();
    }

}
