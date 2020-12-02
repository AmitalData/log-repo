declare namespace Cypress {
    interface Chainable {
        Login(): Chainable<Element>
    }
}

Cypress.Commands.add("Login", () => {

    let mode = Cypress.env("Mode")

    if(mode.toLowerCase() === "development"){
        cy.fixture("Login.json").then((LoginData) => {
            cy.visit(LoginData.url)
            cy.get("#Email").clear().type(LoginData.email)
            cy.get("#Password").clear().type(LoginData.password)
            cy.get("#cmdLogin").click()
            if (LoginData.tenant !== null) {
                cy.get("input[name='cmbTenants_input']").clear().type(LoginData.tenant)
                cy.wait(500)
                cy.get("#cmbTenants_listbox").find("li").click()
            }
        })
    }else{
        let url = Cypress.env("Url")
        let email = Cypress.env("Email")
        let password = Cypress.env("Password")
        let tenant = Cypress.env("Tenant")
        cy.visit(url)
        cy.get("#Email").clear().type(email)
        cy.get("#Password").clear().type(password)
        cy.get("#cmdLogin").click()
    
        if (tenant !== null) {
            cy.get("input[name='cmbTenants_input']").clear().type(tenant)
            cy.wait(500)
            cy.get("#cmbTenants_listbox").find("li").click()
        }
    }

    cy.get("#cmdContinue").click()
    cy.server()
    cy.route("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    cy.window().then(win => { win.sessionStorage.setItem("ControlledByCypress", "true") })
    cy.wait("@LoadDataCompleted")

})