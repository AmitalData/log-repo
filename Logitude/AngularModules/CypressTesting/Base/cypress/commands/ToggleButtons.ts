declare namespace Cypress {
    interface Chainable {
        SelectToggleByIndex(itemIndex:number): Chainable<Element> 
        SelectToggleByLabel(label:string): Chainable<Element> 

    } 
    
}

Cypress.Commands.add("SelectToggleByIndex", (itemIndex) => {  
    //const itemSelector = this.selector == "ToggleButton" ? "ToggleButtonItem" : "Button";

    cy.get('.LogitudeToggleButton').click();
    cy.get('.LogitudeToggleButtonItem').eq(itemIndex).click();
    
})

Cypress.Commands.add("SelectToggleByLabel", (label) => {  
    cy.get('.LogitudeToggleButton').click();
    cy.get('.LogitudeToggleButtonItem').contains(label,{matchCase: false}).click();
})


