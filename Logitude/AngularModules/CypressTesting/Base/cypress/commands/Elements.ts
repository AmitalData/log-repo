import { URLs } from '../constants/URLs'
import * as gr from '../actions/GenerateRandoms'

declare global {
    namespace Cypress {
        interface Chainable {
            FillDate(selector: string, value: string): Chainable<Element>
            FillLogTextBox(selector: string, value: string): Chainable<Element>
            FillLogLov(selector: string, value: string, fromCache: boolean): Chainable<Element>
            FillRandomString(selector: string, length: number, upperCase: boolean): Chainable<Element>
            FillRandomNumber(selector: string, minimum: number, maximum: number): Chainable<Element>
            Click(selector: string, contains: string, force?: boolean): Chainable<Element>
            ClickCheckBox(selector: string): Chainable<Element>
            ClickRadio(selector: string): Chainable<Element>
            ValidateElementColor(selector: string, expectedcolor: string): Chainable<Element>
            SelectQuickSearchFirstElement(selector: string, value: string): Chainable<Element>
            SelectQuickSearchFirstElement2(selector: string, value: string): Chainable<Element>
        }
    }
}

Cypress.Commands.add("BackButton", (contains) => {
    cy.Click(".BackBottonBody", contains);
})

Cypress.Commands.add("FillDate", (selector, value) => {
    if (value.toUpperCase() == "TODAY") {
        cy.get(selector).focus().clear().type(".{enter}")
    }
    else {
        cy.get(selector).focus().clear().type(value)
    }
})

Cypress.Commands.add("FillLogTextBox", (selector, value) => {
    cy.get(selector).clear().type(value)//.should('have.value', value)

})


Cypress.Commands.add("FillLogLov", (selector, value, fromCache) => {

    if (!fromCache) {
        cy.intercept(URLs.GetByCompactFilters).as("LOVDataLoaded")
    }

    //cy.get(selector).clear().type(value)
    cy.get(selector).type("{selectall}" + value,{delay:5})

    if (!fromCache) {
        cy.wait("@LOVDataLoaded")
    }
    cy.get(".DropDownListItem").children().eq(0).click()

})

Cypress.Commands.add("FillRandomString", (selector, length, upperCase) => {
    let randomString = gr.GenerateRandomString(length, upperCase)
    cy.get(selector).focus().clear().type(randomString)
})

Cypress.Commands.add("FillRandomNumber", (selector, minimum, maximum) => {
    let randomNumber = gr.GenerateRandomNumber(minimum, maximum).toString()
    cy.get(selector).focus().clear().type(randomNumber)
})

Cypress.Commands.add("Click", (selector, contains, force = false) => {
    let element = cy.get(selector)//.should('exist')
    if (contains) {
        element = element.contains(contains, { matchCase: false })
    }
    element.click({force:force})
})

Cypress.Commands.add("ClickCheckBox", (selector) => {

    cy.get(selector).next("label").click()

})

Cypress.Commands.add("ClickRadio", (selector) => {
    cy.get(selector).next("label").click()
})

Cypress.Commands.add("ValidateElementColor", (selector, expectedcolor) => {

    cy.get(selector).should("have.css", "color").and("equal", expectedcolor)

})

Cypress.Commands.add("SelectQuickSearchFirstElement", (selector, value) => {
    cy.intercept(URLs.GetQuickSearch).as("QuickSearchDataLoaded")
    cy.get(selector).parents("searchbox").eq(0).find(".SearchBox")
        .within(() => {
            cy.get(selector).focus().clear().type(value).then(() => {
                cy.wait("@QuickSearchDataLoaded")
                cy.get("ul > li").eq(0).click({ force: true })
            })
        })
})

Cypress.Commands.add("SelectQuickSearchFirstElement2", (selector, value) => {
    cy.intercept(URLs.GetQuickSearch).as("QuickSearchDataLoaded")
    cy.get(selector).parents("quicksearchtextbox").eq(0).find(".LogitudeQuickSearchTextBox")
        .within(() => {
            cy.get(selector).focus().clear().type(value).then(() => {
                cy.get("ul > li").eq(0).click({ force: true })
            })
        })
})
