import { Resolvers } from "../../Resolvers/Resolvers";
import { NewQuoteWizardScenarios } from '../../Scenarios/QuoteScenarios/NewQuoteWizardScenarios';
import { LoginComp } from "../../../Login/Login.po";

describe('Operations', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewQuoteWizardScenarios = new NewQuoteWizardScenarios();

    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHCRM').Select();
        Resolvers.MainMenuResolver.Selector('#CRMQUT').Select();
    });

    it('Test New Quote ', () => {
        scenarios.RunScenario('E', 'A');
        
    });
});