import { Interception } from "cypress/types/net-stubbing";

export function AssertStatusCode(requestAlias: string, expectedStatusCode: number): Cypress.Chainable<Interception> {
    var Interception = cy.wait("@" + requestAlias);
    Interception.then((interception) => {
        assert.equal(interception.response.statusCode, expectedStatusCode)
    })
    return Interception;
}

export function AssertElementExist(selector: string){
    cy.get(selector).should("exist");
}

export function AssertElementHaveClass(selector: string, classValue: string){
    cy.get(selector).should("have.class", classValue);
}

export function AssertElementNotExist(selector: string){
    cy.get(selector).should("not.exist");
}

export function AssertElementContain(selector: string, Value: string){
    cy.get(selector).should('contain', Value)
}