declare namespace Cypress {
    interface Chainable {
        NavigateToShipmentsWorkSpace(): Chainable<Element> 
    } 
    
}

Cypress.Commands.add("NavigateToShipmentsWorkSpace", () => {
    cy.Login();   
    cy.Click("#GeneralMHOperations");
    cy.Click("#SHIP"); 
})