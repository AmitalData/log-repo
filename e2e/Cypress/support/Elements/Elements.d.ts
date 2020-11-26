declare namespace Cypress {
    interface Chainable {
        FillLogTextBox(selector: string, value: string, assertRequired: boolean = false): Chainable<Element>
        FillLogLov(selector: string, value: string, assertRequired: boolean = false): Chainable<Element>
        FillRandomString(selector: string, length: number, upperCase: boolean, assertRequired: boolean = false): Chainable<Element>
        FillRandomNumber(selector: string, minimum: number, maximum: number, assertRequired: boolean = false): Chainable<Element>
        Click(selector: string, contains: string = null): Chainable<Element>
        SaveClick(Url:string, selector: string ,contains: string = null): Chainable<Element>
        ToggleCheckBox(selector: string): Chainable<Element>
    }
}