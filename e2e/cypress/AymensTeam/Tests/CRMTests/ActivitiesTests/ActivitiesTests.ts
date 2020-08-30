import { Resolvers } from "../../../Resolvers/Resolvers";
import { NewActivitiesScenarios } from '../../../Scenarios/CRMScenarios/NewActivitiesScenarios';
import { LoginComp } from "../../../../Login/Login.po";

describe('Activities', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewActivitiesScenarios = new NewActivitiesScenarios();


    beforeEach(() => {

        Resolvers.MainMenuResolver.Selector('#GeneralMHCRM').Select();
        Resolvers.MainMenuResolver.Selector('#CRMACT').Select();

    });

    it('Test New Activities', () => {
        scenarios.RunScenario('T');
        scenarios.RunScenario('P');
        scenarios.RunScenario('A');
    
    });
});