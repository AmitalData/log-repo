declare namespace Cypress {
    interface Chainable {
        NavigateToMainMenu(menuSelector:string): Chainable<Element> 
        NavigateToWorkSpaceTab(tabSelector:string): Chainable<Element> 

    } 
    
}

Cypress.Commands.add("NavigateToMainMenu", (menuSelector) => {  
    cy.Click(menuSelector); 
})

Cypress.Commands.add("NavigateToWorkSpaceTab", (tabSelector) => {  
    cy.Click(tabSelector); 
})