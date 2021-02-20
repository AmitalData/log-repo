import { Resolvers } from "../../../Resolvers/Resolvers";
import { NewEditOpportunitiesScenarios } from '../../../Scenarios/CRMScenarios/NewEditOpportunitiesScenarios';
import { LoginComp } from "../../../../Login/Login.po";

describe('Opportunities', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewEditOpportunitiesScenarios = new NewEditOpportunitiesScenarios();
    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHCRM').Select();
        Resolvers.MainMenuResolver.Selector('#CRMOPP').Select();
    });
    it('Test New Opportunites', () => {
        scenarios.RunScenario();
    });
});