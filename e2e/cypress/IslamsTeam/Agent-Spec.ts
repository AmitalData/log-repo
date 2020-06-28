/// <reference types="cypress" />

import { NewAgent } from './Agent';
import { NewAgentScenario } from './AgentScenario';
import { LoginComp } from ".././login/Login.po";





describe('NewAgent', () => {


    let AgentScenario: NewAgentScenario = new NewAgentScenario();

    beforeEach(() => {

    });



    it('QuickSearch', function () {

        AgentScenario.Quicksearch();
    });


    it('SearchAgentTab', function () {

        AgentScenario.SearchAgentTab();
    });


    it('CreateNewAgent', function () {


        AgentScenario.CreateNewAgent();
    });




});
