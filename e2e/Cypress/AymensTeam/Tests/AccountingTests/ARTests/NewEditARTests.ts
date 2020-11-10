import { Resolvers } from "../../../Resolvers/Resolvers";
import { NewEditActivitiesScenarios } from '../../../Scenarios/CRMScenarios/NewEditActivitiesScenarios';
import { LoginComp } from "../../../../Login/Login.po";

describe('Activities', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewEditActivitiesScenarios = new NewEditActivitiesScenarios();
    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHCRM').Select();
        Resolvers.MainMenuResolver.Selector('#CRMACT').Select();
    });
 it('Test New Activities', () => {
        scenarios.RunScenario('T');  
    });
    it('Test New Activities', () => {
        scenarios.RunScenario('P');
    });
    it('Test New Activities', () => {
        scenarios.RunScenario('A');
    });
});