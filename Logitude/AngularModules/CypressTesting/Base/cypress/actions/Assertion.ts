import { Interception } from "cypress/types/net-stubbing";

export function AssertStatusCode(requestAlias, expectedStatusCode): Cypress.Chainable<Interception> {
    var Interception = cy.wait("@" + requestAlias);
    Interception.then((interception) => {
        assert.equal(interception.response.statusCode, expectedStatusCode)
    })
    return Interception;
}
