Cypress.Commands.add("Login", () => {

    cy.fixture("Data/Login.json").then((LoginData) => {

        cy.visit(LoginData.url)
        cy.get("#Email").type(LoginData.email)
        cy.get("#Password").type(LoginData.password)
        cy.get("#cmdLogin").click()
        if (LoginData.tenant !== null) {
            cy.get("input[name='cmbTenants_input']").type(LoginData.tenant)
            cy.wait(500)
            cy.get("#cmbTenants_listbox").find("li").click()
        }
    })

    cy.get("#cmdContinue").click()
    cy.server()
    cy.route("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    cy.window().then(win => { win.sessionStorage.setItem("ControlledByCypress", "true") })
    cy.wait("@LoadDataCompleted")

})