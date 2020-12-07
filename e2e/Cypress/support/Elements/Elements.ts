declare namespace Cypress {
    interface Chainable {
        FillLogTextBox(selector: string, value: string, assertRequired: boolean): Chainable<Element>
        FillLogLov(selector: string, value: string, assertRequired: boolean): Chainable<Element>
        FillRandomString(selector: string, length: number, upperCase: boolean, assertRequired: boolean): Chainable<Element>
        FillRandomNumber(selector: string, minimum: number, maximum: number, assertRequired: boolean): Chainable<Element>
        Click(selector: string, contains: string): Chainable<Element>
        SaveClick(selector: string, contains: string, Url: string, resultFile: string): Chainable<Element>
        ClickCheckBox(selector: string): Chainable<Element>
        ClickRadio(selector: string): Chainable<Element>
        SelectLogLovFirstElement(selector: string): Chainable<Element>
        ValidateElementColor(selector: string, expectedcolor: string): Chainable<Element>
        ValidateValue(selector: string, value: string): Chainable<Element>
        SelectSearchBoxFirstElement(selector: string, value: string): Chainable<Element>
    }
}
 
Cypress.Commands.add("FillLogTextBox", (selector, value, assertRequired) => {

    if (assertRequired) {
        cy.get(selector).clear().type(value).should("have.value", value)
    } else {
        cy.get(selector).clear().type(value)
    }

})

Cypress.Commands.add("FillLogLov", (selector, value, assertRequired) => {

    if (assertRequired) {
        cy.get(selector).clear().type(value).should("have.value", value)
    } else {
        cy.get(selector).clear().type(value)
    }
 
    cy.get(".DropDownListItem").find("div[title='" + value + "']").click()

})

Cypress.Commands.add("ValidateValue", (selector, value) => {

    cy.get(selector).should("have.value", value)

})

Cypress.Commands.add("SelectLogLovFirstElement", (selector) => {

    cy.intercept("**/GetByCompactFilters?**").as("LOVDataLoaded"); 
    cy.get(selector).focus().type('{downarrow}'); 
    cy.wait("@LOVDataLoaded")
    //GetByCompactFilters
    cy.get(".DropDownListItem").children().eq(0).click();

})

Cypress.Commands.add("FillRandomString", (selector, length, upperCase, assertRequired) => {

    let randomString = ""
    let possible = "abcdefghijklmnopqrstuvwxyz"

    for (let i = 0; i < length; i++)
        randomString += possible.charAt(Math.floor(Math.random() * possible.length))

    if (upperCase) {
        randomString = randomString.toUpperCase()
    }

    if (assertRequired) {
        cy.get(selector).clear().type(randomString).should("have.value", randomString)
    } else {
        cy.get(selector).clear().type(randomString)
    }

})

Cypress.Commands.add("FillRandomNumber", (selector, minimum, maximum, assertRequired) => {

    minimum = Math.ceil(minimum);
    maximum = Math.floor(maximum);

    let randomNumber = (Math.floor(Math.random() * (maximum - minimum + 1) + minimum)).toString()

    if (assertRequired) {
        cy.get(selector).clear().type(randomNumber).should("have.value", randomNumber)
    } else {
        cy.get(selector).clear().type(randomNumber)
    }

})

Cypress.Commands.add("Click", (selector, contains) => {

    let element = cy.get(selector)

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
            cy.writeFile("cypress/fixtures/ResponseData/" + resultFile + ".json", interception.response.body)
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

    cy.get(selector).should("have.css", "color").and("equal", expectedcolor);

})

Cypress.Commands.add("SelectSearchBoxFirstElement", (selector, value) => {

    cy.get(selector).parents("searchbox").eq(0).find(".SearchBox")
    .within(() => {
        cy.get(selector).type(value).then(() => {
            cy.get("ul > li").eq(0).click({ force: true })
        })
    })

})