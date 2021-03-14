declare namespace Cypress {
    interface Chainable {
        DefineRequestWait(method: string, url: string, requestAlias: string): Chainable<Element>
        DefineWindowOpen(windowOpenAlias: string): Chainable<Cypress.AUTWindow>
    }
}

Cypress.Commands.add("DefineRequestWait", (method, url, requestAlias) => {
    cy.intercept({
        method: method,
        url: url
    }).as(requestAlias)
})

Cypress.Commands.add("DefineWindowOpen", (windowOpenAlias) => {
    cy.window().then((win) => {
        cy.stub(win, 'open').as(windowOpenAlias)
      })
})