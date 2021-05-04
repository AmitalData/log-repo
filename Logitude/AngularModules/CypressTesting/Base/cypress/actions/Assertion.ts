import { RequestAliases } from "../../cypress/constants/RequestAliases";
import { BaseSelectors } from "../../cypress/selectors/BaseSelectors";
import { Interception } from "cypress/types/net-stubbing";

export function AssertStatusCode(requestAlias: string, expectedStatusCode: number): Cypress.Chainable<Interception> {
    var Interception = cy.wait("@" + requestAlias);
    Interception.then((interception) => {
        assert.equal(interception.response.statusCode, expectedStatusCode)
    })
    return Interception;
}

export function AssertWindowOpen(windowOpenAlias: string){
  cy.get('@' + windowOpenAlias).should('be.called')
}

export function AssertElementExist(selector: string) {
    cy.get(selector).should("exist");
}

export function AssertElementHaveClass(selector: string, classValue: string) {
    cy.get(selector).should("have.class", classValue);
}

export function AssertElementNotHaveClass(selector: string, classValue: string) {
    cy.get(selector).should("not.have.class", classValue);
}

export function AssertElementHaveValue(selector: string, Value: string) {
    cy.get(selector).should("have.value", Value);
}

export function AssertElementNotExist(selector: string) {
    cy.get(selector).should("not.exist");
}

//be.disable | not.be.disable
export function AssertElementDisabled(selector: string, condition: string) {
    cy.get(selector).should(condition);
}

export function AssertElementHaveClasss(selector: string, condition: string , classValue:string) {
    cy.get(selector).should(condition,classValue);
}

export function AssertElementContain(selector: string, Value: string){
    cy.get(selector).should('contain', Value)
}

export function AssertElementTextEqual(elementSelector :string ,expectedValue:string ,find? :string){
    if(find){
        cy.get(elementSelector).find(find).invoke('text').then((text) => {
            assert.equal(text.trim(), expectedValue)
        })
    }else{
        cy.get(elementSelector).invoke('text').then((text) => {
            assert.equal(text.trim(), expectedValue)
        })
    }
    
}
export function AssertInformationMessage(message:string){
    AssertMessageWindow(message)
}

export function AssertMessageWindow(message:string){
    AssertElementExist(BaseSelectors.MessageWindow)
    AssertElementContain(BaseSelectors.MessageWindow,message)
    CloseWindow(BaseSelectors.MessageWindow,BaseSelectors.ContainsOK)
}

export function CloseWindow(windowSelector:string,contain:string){
    cy.get(windowSelector).last().contains(contain).click({force:true});
}