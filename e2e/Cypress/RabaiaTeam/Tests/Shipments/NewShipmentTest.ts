describe('New Shipment Tests', () => {

    beforeEach(() => {
        cy.Login();
        cy.NavigateToMainMenu('#GeneralMHOperations');
        cy.NavigateToWorkSpaceTab('#SHIP');
    });

    it('Create New D/Ex/I/FTL Shipment', () => {
        cy.SelectToggleByLabel('Direct');//LogitudeToggleButton
        cy.ClickRadio('#DirectionRadio_0E');
        cy.ClickRadio('#TransportModeRadio_0I')
        cy.ClickRadio('#ShipmentTypeRadio_0FTL');
        //levelCode='D'
        //direction = 'E';
        //transportMode = 'I';
        //shipmentType = 'FTL';
        //scenarios.RunScenario(levelCode, transportMode, direction, shipmentType);
        //editScenarios.RunEditTabsScenarios(levelCode, transportMode, direction, shipmentType);
        //quoteActions.RunQuoteActions();
    });  
});