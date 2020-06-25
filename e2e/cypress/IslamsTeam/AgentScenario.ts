/// <reference types="cypress" />
import { LoginComp } from '.././Login/Login.po';
import { NewAgent } from './Agent';


export class NewAgentScenario {

    private login: LoginComp = new LoginComp();
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


}
