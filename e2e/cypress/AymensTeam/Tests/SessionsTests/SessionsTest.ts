//import { Resolvers } from "../../Resolvers/Resolvers";
//import { NewQuoteWizardScenarios } from '../../Scenarios/QuoteScenarios/NewQuoteWizardScenarios';
//import { NewShipmentWizardScenarios } from '../../Scenarios/ShipmentScenarios/NewShipmentWizardScenarios';

//describe('Sessions', () => {

//    let quoteScenarios = new NewQuoteWizardScenarios();
//    let shipmentScenarios1 = new NewShipmentWizardScenarios();
//    let shipmentScenarios2 = new NewShipmentWizardScenarios();

//    beforeEach(() => {
//        cy.visit('http://localhost:4200/');
//        cy.get('#Email').clear();
//        cy.get('#Password').clear();
//        cy.get('#Email').type('angular@fnarsoft.com');
//        cy.get('#Password').type('1');
//        cy.get('#cmdLogin').click();
//        cy.server();
//        cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');
//        cy.wait('@LoadDataCompleted');
//    });

//    it('Test Sessions', () => {

//        Resolvers.MainMenuResolver.Text('Operations').Select();

//        shipmentScenarios1.RunScenario('D', 'I', 'I', 'LTL');
//        shipmentScenarios1.Save().then((entityNumber: string) => {
//            Resolvers.SearchBoxResolver.Workspace("Operations").Type(entityNumber);
//            Resolvers.EditComponentResolver.ShouldBeOpend();
//            Resolvers.EditComponentResolver.Tab('Overview').ShouldBeSelected();
//        });

//        Resolvers.OpenSession();

//        Resolvers.MainMenuResolver.Text('Quotes').Select();
//        quoteScenarios.RunScenario('A', 'E');
//        quoteScenarios.Save().then((entityNumber: string) => {
//            //Resolvers.SearchBoxResolver.Workspace("Quotes").Type(entityNumber);
//            //Resolvers.EditComponentResolver.ShouldBeOpend();
//            //Resolvers.EditComponentResolver.Tab('Overview').ShouldBeSelected();
//        });


//        Resolvers.SelectSession(0);
//        Resolvers.EditComponentResolver.Tab('Routings').Select();
//    });
//});