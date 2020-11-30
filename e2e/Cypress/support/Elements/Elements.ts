export {};
declare global {
    namespace Cypress {
        interface Chainable {
            FillLogTextBox(selector: string, value: string, assertRequired?: boolean): Chainable<Element>
            FillLogLov(selector: string, value: string, assertRequired?: boolean): Chainable<Element>
            FillRandomString(selector: string, length: number, upperCase: boolean, assertRequired?: boolean): Chainable<Element>
            FillRandomNumber(selector: string, minimum: number, maximum: number, assertRequired?: boolean): Chainable<Element>
            Click(selector: string, contains?: string): Chainable<Element>
            SaveClick(Url: string, selector: string, contains?: string): Chainable<Element>
            ToggleCheckBox(selector: string): Chainable<Element>
            SelectLogLovFirstElement(selector: string): Chainable<Element>
        }
    }
}
Cypress.Commands.add("FillLogTextBox", (selector, value, assertRequired = false) => {

    if (assertRequired) {
        cy.get(selector).type(value).should("have.value", value)
    } else {
        cy.get(selector).type(value)
    }

})

Cypress.Commands.add("FillLogLov", (selector, value, assertRequired = false) => {

    if (assertRequired) {
        cy.get(selector).type(value).should("have.value", value)
    } else {
        cy.get(selector).type(value)
    }

    cy.get(".DropDownListItem").find("div[title='" + value + "']").click()

})

Cypress.Commands.add("SelectLogLovFirstElement", (selector) => {

    cy.get(selector).focus().type('{downarrow}');

    cy.get(".DropDownListItem").children().eq(0).click();

})

Cypress.Commands.add("FillRandomString", (selector, length, upperCase, assertRequired = false) => {

    let randomString = ""
    let possible = "abcdefghijklmnopqrstuvwxyz"

    for (let i = 0; i < length; i++)
        randomString += possible.charAt(Math.floor(Math.random() * possible.length))

    if (upperCase) {
        randomString = randomString.toUpperCase()
    }

    if (assertRequired) {
        cy.get(selector).type(randomString).should("have.value", randomString)
    } else {
        cy.get(selector).type(randomString)
    }

})

Cypress.Commands.add("FillRandomNumber", (selector, minimum, maximum, assertRequired = false) => {

    minimum = Math.ceil(minimum);
    maximum = Math.floor(maximum);

    let randomNumber = Math.floor(Math.random() * (maximum - minimum + 1) + minimum);

    if (assertRequired) {
        cy.get(selector).type(randomNumber.toString()).should("have.value", randomNumber)
    } else {
        cy.get(selector).type(randomNumber.toString())
    }

})

Cypress.Commands.add("Click", (selector, contains = null) => {

    let element = cy.get(selector)

    if (contains !== null) {
        element = element.contains(contains)
    }

    element.click()

})

Cypress.Commands.add("SaveClick", (url, selector, contains = null) => {

    cy.server()
    cy.route({
        method: "POST",
        url: url,
        onResponse: (xhr) => {
            assert.equal(xhr.status, 200, "Saved Success")
        }
    }).as("WaitRequest")

    let element = cy.get(selector)

    if (contains !== null) {
        element = element.contains(contains)
    }

    element.click()

    cy.wait("@WaitRequest")

})

Cypress.Commands.add("ToggleCheckBox", (selector) => {

    cy.get(selector).next("label").click()

})