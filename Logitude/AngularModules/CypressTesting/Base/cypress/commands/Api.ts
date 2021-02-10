declare namespace Cypress {
    interface Chainable {
        DefineRequestWait(method: string, url: string, requestAlias: string): Chainable<Element>
    }
}

Cypress.Commands.add("DefineRequestWait", (method, url, requestAlias) => {
    cy.intercept({
        method: method,
        url: url
    }).as(requestAlias)
})