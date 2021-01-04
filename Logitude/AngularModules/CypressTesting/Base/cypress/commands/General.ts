declare namespace Cypress {
    interface Chainable {
        NavigateToMainMenu(menuSelector:string): Chainable<Element>
        NavigateToWorkSpaceTab(tabSelector:string): Chainable<Element>
        DefineRequestWait(method: string, url: string, requestAlias: string): Chainable<Element>
        AssertResponseStatusCode(requestAlias: string, expectedStatusCode: number, resultFile: string): Chainable<Element>
    }
}

Cypress.Commands.add("NavigateToMainMenu", (menuSelector) => {  
    cy.Click(menuSelector, null); 
})

Cypress.Commands.add("NavigateToWorkSpaceTab", (tabSelector) => {  
    cy.Click(tabSelector, null); 
})

Cypress.Commands.add("DefineRequestWait", (method, url, requestAlias) => {

    cy.intercept({
        method: method,
        url: url
    }).as(requestAlias)

})

Cypress.Commands.add("AssertResponseStatusCode", (requestAlias, expectedStatusCode, resultFile) => {

    cy.wait("@" + requestAlias).then((interception) => {
        assert.equal(interception.response.statusCode, expectedStatusCode)
        if(resultFile !== null){
            cy.writeFile("cypress/fixtures/" + resultFile, interception.response.body)
        }
    })

})