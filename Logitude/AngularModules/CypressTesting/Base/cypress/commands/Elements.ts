import { BaseURLs } from '../constants/URLs'
import * as gr from '../actions/GenerateRandoms'
import { QuickSearchDetails } from '../models/QuickSearchDetails';
import { RestAPI } from '../constants/RestAPI';
import { RequestAliases } from '../constants/RequestAliases';
import * as BaseAssertion from '../actions/Assertion';
import { BaseSelectors } from '../selectors/BaseSelectors';

declare global {
    namespace Cypress {
        interface Chainable {
            FillDate(selector: string, value: string): Chainable<Element>
            FillLogTextBox(selector: string, value: string,ValidateInputDone? :boolean): Chainable<Element>
            FillLogLov(selector: string, value: string, fromCache: boolean, getByFilters?: boolean): Chainable<Element>
            FillRandomString(selector: string, length: number, upperCase: boolean): Chainable<Element>
            FillRandomNumber(selector: string, minimum: number, maximum: number): Chainable<Element>
            Click(selector: string, contains: string, force?: boolean): Chainable<Element>
            Navigate(selector: string, force?: boolean): Chainable<Element>
            ClickCheckBox(selector: string): Chainable<Element>
            ClickRadio(selector: string): Chainable<Element>
            ValidateElementColor(selector: string, expectedcolor: string): Chainable<Element>
            SelectQuickSearchFirstElement(quickSearchDetails: QuickSearchDetails): Chainable<Element>
            BackButton(contains:string): Chainable<Element>
            getAttached(selector: any): Chainable<Element>

            ClickingAfterHovering(LogLovSelector:string,HiddenElementSelector:string): Chainable<Element>
            SelectCheckBox(Selector:string): Chainable<Element>
        }
    }
}
Cypress.Commands.add("SelectCheckBox", (Selector:string) => {
    cy.get(Selector).check({ force: true })

/**
 * getAttached(selector)
 * getAttached(selectorFn)
 *
 * Waits until the selector finds an attached element, then yields it (wrapped).
 * selectorFn, if provided, is passed $(document). Don't use cy methods inside selectorFn.
 */
Cypress.Commands.add("getAttached", selector => {
    const getElement = typeof selector === "function" ? selector : $d => $d.find(selector);
    let $el = null;
    return cy.document().should($d => {
      $el = getElement(Cypress.$($d));
      expect(Cypress.dom.isDetached($el)).to.be.false;
    }).then(() => cy.wrap($el));
  });

})
Cypress.Commands.add("ClickingAfterHovering", (LogLovSelector:string,HiddenElementSelector:string) => {
    cy.get(LogLovSelector).trigger(BaseSelectors.MouseoverTrigger).find(HiddenElementSelector).click()

})
Cypress.Commands.add("BackButton", (contains) => {
    cy.Click(BaseSelectors.BackBottonBodyClass, contains);
})

Cypress.Commands.add("FillDate", (selector, value) => {
    if (value.toUpperCase() == "TODAY") {
        cy.get(selector).focus().clear().type(".{enter}")
    }
    else if (value.toUpperCase() == "TOMORROW") {
        cy.get(selector).focus().clear().type("+1{enter}")
    }
    else {
        cy.get(selector).focus().clear().type(value)
    }
})

Cypress.Commands.add("FillLogTextBox", (selector, value,ValidateInputDone = false) => {

    if(ValidateInputDone){
        cy.get(selector).clear().type(value)//.should('have.value', value)
    }
    else{
        cy.get(selector).clear().type(value).should('have.value', value)
    }

})


Cypress.Commands.add("FillLogLov", (selector, value, fromCache, getByFilters = false) => {

    if (!fromCache) {
        cy.intercept({
            method: RestAPI.GET,
            url: getByFilters ? BaseURLs.GetByFilters : BaseURLs.GetByCompactFilters
        }).as("LOVDataLoaded")
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
    //let element = //.should('exist')
    cy.wait(1000);
    if (contains) {
        cy.get(selector).contains(contains, { matchCase: false }).click({force:force})
    }
    else{
        cy.get(selector).click({force:force})
    }
})

Cypress.Commands.add("Navigate", (selector, force = false) => {
    let element = cy.get(selector)
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

Cypress.Commands.add("SelectQuickSearchFirstElement", (quickSearchDetails: QuickSearchDetails) => {
    cy.DefineRequestWait(RestAPI.GET, quickSearchDetails.WaitURL, RequestAliases.QuickSearchDataLoaded);
    cy.get(quickSearchDetails.Selector).parents(quickSearchDetails.Parent).eq(0).find(quickSearchDetails.ParentClass)
        .within(() => {
            cy.get(quickSearchDetails.Selector).focus().clear().type(quickSearchDetails.Value).then(() => {
                BaseAssertion.AssertStatusCode(RequestAliases.QuickSearchDataLoaded, 200);
                cy.get(BaseSelectors.FirstElementInList).eq(0).click({ force: true });
            })
        })
})
