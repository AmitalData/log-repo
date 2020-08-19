import { Resolvers } from "../../Resolvers/Resolvers";
import { LoginComp } from "../../../Login/Login.po";
import { NewQuoteWizardScenarios } from '../../Scenarios/QuoteScenarios/NewQuoteWizardScenarios';
import { EditQuoteTabsScenarios } from '../../Scenarios/QuoteScenarios/EditQuoteTabsScenarios';

describe('Quotes Modules', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewQuoteWizardScenarios = new NewQuoteWizardScenarios();
    let editScenarios: EditQuoteTabsScenarios = new EditQuoteTabsScenarios();


    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHCRM').Select();
        Resolvers.MainMenuResolver.Selector('#CRMQUT').Select();
    });

    it('Test New Quote ', () => {
        scenarios.RunScenario('E', 'A');
        editScenarios.RunEditTabsScenarios();
    });
});