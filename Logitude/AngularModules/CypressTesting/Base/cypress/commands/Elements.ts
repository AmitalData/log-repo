import * as gr from '../actions/GenerateRandoms'

declare global{
namespace Cypress {
    interface Chainable {
        FillDate(selector: string, value: string): Chainable<Element>
        FillLogTextBox(selector: string, value: string): Chainable<Element>
        FillLogLov(selector: string, value: string, fromCache: boolean): Chainable<Element>
        FillRandomString(selector: string, length: number, upperCase: boolean): Chainable<Element>
        FillRandomNumber(selector: string, minimum: number, maximum: number): Chainable<Element>
        Click(selector: string, contains: string): Chainable<Element>
        SaveClick(selector: string, contains: string, url: string, resultFile: string): Chainable<Element>
        ClickCheckBox(selector: string): Chainable<Element>
        ClickRadio(selector: string): Chainable<Element>
        SelectLogLovFirstElement(selector: string, fromCache: boolean): Chainable<Element>
        SelectLogLovElement(selector: string, fromCache: boolean, moveType: string): Chainable<Element>
        ValidateElementColor(selector: string, expectedcolor: string): Chainable<Element>
        ValidateInputValue(selector: string, value: string): Chainable<Element>
        SelectQuickSearchFirstElement(selector: string, value: string): Chainable<Element>
    }
}
}
Cypress.Commands.add("FillDate", (selector, value) => {
if(value=="today"){
    cy.get(selector).focus().clear().type(".{enter}")
}
else{
    cy.get(selector).focus().clear().type(value)
}
})
Cypress.Commands.add("FillLogTextBox", (selector, value) => {

    cy.get(selector).focus().clear().type(value)

})

Cypress.Commands.add("FillLogLov", (selector, value, fromCache) => {

    if(!fromCache){
        cy.intercept("**/GetByCompactFilters?**").as("LOVDataLoaded")
    }

    cy.get(selector).focus().clear().type(value)

    if(!fromCache){
        cy.wait("@LOVDataLoaded")
    }
    cy.get(".DropDownListItem").children().eq(0).click()

})

Cypress.Commands.add("ValidateInputValue", (selector, value) => {

    cy.get(selector).should("have.value", value)

})

Cypress.Commands.add("SelectLogLovFirstElement", (selector, fromCache) => {

    if(!fromCache){
        cy.intercept("**/GetByCompactFilters?**").as("LOVDataLoaded")
    }
    cy.get(selector).focus().clear().type("{downarrow}")
    if(!fromCache){
        cy.wait("@LOVDataLoaded")
    }
    cy.get(".DropDownListItem").children().eq(1).click()

})

Cypress.Commands.add("SelectLogLovElement", (selector, fromCache, moveType) => {

    if(!fromCache){
        cy.intercept("**/GetByCompactFilters?**").as("LOVDataLoaded")
    }
    cy.get(selector).focus().clear().type(moveType)
    if(!fromCache){
        cy.wait("@LOVDataLoaded")
    }

    //let randomIndex = gr.GenerateRandomNumber(1, (maxOptions - 1))
    cy.get(".DropDownListItem").children().eq(0).click({ force: true })

})

Cypress.Commands.add("FillRandomString", (selector, length, upperCase) => {

    let randomString = gr.GenerateRandomString(length, upperCase)

    cy.get(selector).focus().clear().type(randomString)

})

Cypress.Commands.add("FillRandomNumber", (selector, minimum, maximum) => {

    let randomNumber = gr.GenerateRandomNumber(minimum, maximum).toString()

    cy.get(selector).focus().clear().type(randomNumber)

})

Cypress.Commands.add("Click", (selector, contains) => {

    let element = cy.get(selector).should('exist')

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

    let element = cy.get(selector)

    if (contains !== null) {
        element = element.contains(contains, {matchCase: false})
    }

    element.click()

    cy.wait("@WaitRequest").then((interception) => {
        assert.equal(interception.response.statusCode, 200)
        if(resultFile !== null){
            cy.writeFile("cypress/fixtures/" + resultFile, interception.response.body)
        }
    })

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

    cy.intercept("**/GetQuickSearch?**").as("QuickSearchDataLoaded")
    cy.get(selector).parents("searchbox").eq(0).find(".SearchBox")
    .within(() => {
        cy.get(selector).focus().clear().type(value).then(() => {
            cy.wait("@QuickSearchDataLoaded")
            cy.get("ul > li").eq(0).click({ force: true })
        })
    })

})