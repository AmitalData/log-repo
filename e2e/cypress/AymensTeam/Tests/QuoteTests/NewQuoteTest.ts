import { Resolvers } from "../../Resolvers/Resolvers";
import { LoginComp } from "../../../Login/Login.po";
import { NewQuoteWizardScenarios } from '../../Scenarios/QuoteScenarios/NewQuoteWizardScenarios';
import { EditQuoteTabsScenarios } from '../../Scenarios/QuoteScenarios/EditQuoteTabsScenarios';
import { QuoteActions } from '../../Scenarios/QuoteScenarios/QuoteActions';


describe('Quotes Modules', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewQuoteWizardScenarios = new NewQuoteWizardScenarios();
    let editScenarios: EditQuoteTabsScenarios = new EditQuoteTabsScenarios();
    let quoteActions: QuoteActions = new QuoteActions();

    var quoteType;
    var direction;
    var transportMode;
    var shipmentType;

    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHCRM').Select();
        Resolvers.MainMenuResolver.Selector('#CRMQUT').Select();
    });

    it('Test New Quote ', () => {
        quoteType = 'RR'; // RR: Routing Rate , SR : spot Rate 
        direction = 'E';
        transportMode = 'A';
        shipmentType = '';

        scenarios.RunScenario(direction, transportMode, shipmentType, quoteType);
        editScenarios.RunEditTabsScenarios(direction, transportMode, shipmentType, quoteType);
        quoteActions.RunQuoteActions(quoteType);     
    });
   
});