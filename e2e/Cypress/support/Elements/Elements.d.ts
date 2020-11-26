declare namespace Cypress {
    interface Chainable {
        FillLogTextBox(selector: string, value: string, assertRequired?: boolean): Chainable<Element>
        FillLogLov(selector: string, value: string, assertRequired?: boolean): Chainable<Element>
        FillRandomString(selector: string, length: number, upperCase: boolean, assertRequired?: boolean): Chainable<Element>
        FillRandomNumber(selector: string, minimum: number, maximum: number, assertRequired?: boolean): Chainable<Element>
        Click(selector: string, contains?: string): Chainable<Element>
        SaveClick(Url: string, selector: string, contains?: string): Chainable<Element>
        ClickCheckBox(selector: string): Chainable<Element>
        ClickRadio(selector: string): Chainable<Element>
    }
}