import { Interception } from "cypress/types/net-stubbing";

export function AssertStatusCode(requestAlias: string, expectedStatusCode: number): Cypress.Chainable<Interception> {
    var Interception = cy.wait("@" + requestAlias);
    Interception.then((interception) => {
        assert.equal(interception.response.statusCode, expectedStatusCode)
    })
    return Interception;
}