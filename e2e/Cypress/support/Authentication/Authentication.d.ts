declare namespace Cypress {
    interface Chainable {
        Login(): Chainable<Element>
    }
}