import * as sr from '../../RabaiaTeam/General/Selector'
import * as gr from '../../RabaiaTeam/General/Generator'

declare global{
namespace Cypress {
    interface Chainable {
        FillLogTextBox(selector: string, value: string, assertRequired: boolean): Chainable<Element>
        FillLogLov(selector: string, value: string, assertRequired: boolean, fromCache: boolean): Chainable<Element>
        FillRandomString(selector: string, length: number, upperCase: boolean, assertRequired: boolean): Chainable<Element>
        FillRandomNumber(selector: string, minimum: number, maximum: number, assertRequired: boolean): Chainable<Element>
        Click(selector: string, contains: string): Chainable<Element>
        SaveClick(selector: string, contains: string, Url: string, resultFile: string): Chainable<Element>
        ClickCheckBox(selector: string): Chainable<Element>
        ClickRadio(selector: string): Chainable<Element>
        SelectLogLovFirstElement(selector: string, fromCache: boolean): Chainable<Element>
        ValidateElementColor(selector: string, expectedcolor: string): Chainable<Element>
        ValidateInputValue(selector: string, value: string): Chainable<Element>
        SelectQuickSearchFirstElement(selector: string, value: string): Chainable<Element>
    }
}
}

Cypress.Commands.add("FillLogTextBox", (selector, value, assertRequired) => {

    if (assertRequired) {
        sr.SelectElement(selector).focus().clear().type(value).should("have.value", value)
    } else {
        sr.SelectElement(selector).focus().clear().type(value)
    }

})

Cypress.Commands.add("FillLogLov", (selector, value, assertRequired, fromCache) => {

    if(!fromCache){
        cy.intercept("**/GetByCompactFilters?**").as("LOVDataLoaded")
    }
    if (assertRequired) {
        sr.SelectElement(selector).focus().clear().type(value).should("have.value", value)
    } else {
        sr.SelectElement(selector).focus().clear().type(value)
    }
    if(!fromCache){
        cy.wait("@LOVDataLoaded")
    }
    cy.get(".DropDownListItem").children().eq(0).click()

})

Cypress.Commands.add("ValidateInputValue", (selector, value) => {

    sr.SelectElement(selector).should("have.value", value)

})

Cypress.Commands.add("SelectLogLovFirstElement", (selector, fromCache) => {

    if(!fromCache){
        cy.intercept("**/GetByCompactFilters?**").as("LOVDataLoaded")
    }
    sr.SelectElement(selector).focus().clear().type("{downarrow}")
    if(!fromCache){
        cy.wait("@LOVDataLoaded")
    }
    cy.get(".DropDownListItem").children().eq(0).click()

})

Cypress.Commands.add("FillRandomString", (selector, length, upperCase, assertRequired) => {

    let randomString = gr.GenerateRandomString(length, upperCase)

    if (assertRequired) {
        sr.SelectElement(selector).focus().clear().type(randomString).should("have.value", randomString)
    } else {
        sr.SelectElement(selector).focus().clear().type(randomString)
    }

})

Cypress.Commands.add("FillRandomNumber", (selector, minimum, maximum, assertRequired) => {

    let randomNumber = gr.GenerateRandomNumber(minimum, maximum).toString()

    if (assertRequired) {
        sr.SelectElement(selector).focus().clear().type(randomNumber).should("have.value", randomNumber)
    } else {
        sr.SelectElement(selector).focus().clear().type(randomNumber)
    }

})

Cypress.Commands.add("Click", (selector, contains) => {

    let element = sr.SelectElement(selector)

    if (contains !== null) {
        element = element.contains(contains, {matchCase: false})

    }

    element.click()

})

Cypress.Commands.add("SaveClick", (selector, contains, url, resultFile) => {

    cy.intercept({
        method: "POST",
        url: url
    }).as("WaitRequest")

    let element = sr.SelectElement(selector)

    if (contains !== null) {
        element = element.contains(contains, {matchCase: false})
    }

    element.click()

    cy.wait("@WaitRequest").then((interception) => {
        assert.equal(interception.response.statusCode, 200)
        if(resultFile !== null){
            cy.writeFile("cypress/fixtures/ResponseData/" + resultFile + ".json", interception.response.body)
        }
    })

})

Cypress.Commands.add("ClickCheckBox", (selector) => {

    sr.SelectElement(selector).next("label").click()

})

Cypress.Commands.add("ClickRadio", (selector) => {

    sr.SelectElement(selector).next("label").click()

})

Cypress.Commands.add("ValidateElementColor", (selector, expectedcolor) => {

    sr.SelectElement(selector).should("have.css", "color").and("equal", expectedcolor)

})

Cypress.Commands.add("SelectQuickSearchFirstElement", (selector, value) => {

    cy.intercept("**/GetQuickSearch?**").as("QuickSearchDataLoaded")
    sr.SelectElement(selector).parents("searchbox").eq(0).find(".SearchBox")
    .within(() => {
        sr.SelectElement(selector).focus().clear().type(value).then(() => {
            cy.wait("@QuickSearchDataLoaded")
            cy.get("ul > li").eq(0).click({ force: true })
        })
    })

})