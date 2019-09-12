import { browser, by, element } from 'protractor';
import { NewAgent } from './Agent';
import { NewAgentScenario } from './AgentScenario';
import { LoginComp } from "../../login/Login.po";




describe('NewAgent', () => {

    //  let gAgent: NewAgent = new NewAgent();
    let AgentScenario: NewAgentScenario = new NewAgentScenario();

    beforeEach(() => {

    });

    browser.ignoreSynchronization = true;

    it('QuickSearch', function () {

        AgentScenario.Quicksearch();
    });


    it('SearchAgentTab', function () {

        AgentScenario.SearchAgentTab();
    });


    it('CreateNewAgent', function () {


        AgentScenario.CreateNewAgent();
    });

    it('SearchAgent', function () {


        AgentScenario.SearchAgent();
    });



});