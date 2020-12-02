declare namespace Cypress {
    interface Chainable {
        Login(): Chainable<Element>
        OpenAndFillChangePasswordPage(newPassword:string,confirmNewPassword:string): Chainable<Element>
    }
}
Cypress.Commands.add("Login", () => {
    var Env = Cypress.env("Env");
    cy.fixture("Data/" + Env + ".json").then((LoginData) => {

        cy.visit(LoginData.url)
        cy.get("#Email").type(LoginData.email)
        cy.get("#Password").type(LoginData.password)
        cy.get("#cmdLogin").click()
        if (LoginData.tenant !== null) {
            cy.get("input[name='cmbTenants_input']").type(LoginData.tenant)
            //cy.wait(500)
            cy.get("#cmbTenants_listbox").children().contains('(' + LoginData.tenant + ')').eq(0).click({force:true})
        }
    })

    cy.get("#cmdContinue").click()
    //cy.server()
    //cy.route("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    cy.intercept("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    cy.window().then(win => { win.sessionStorage.setItem("ControlledByCypress", "true") })
    cy.wait("@LoadDataCompleted")

})

Cypress.Commands.add("OpenAndFillChangePasswordPage", (newPassword,confirmNewPassword) => {
    var Env = Cypress.env("Env");
    cy.fixture("Data/" + Env + ".json").then((LoginData) => {
        var ResetURL = LoginData.url + "/PasswordChangePage.aspx?email=" + LoginData.email;
        cy.visit(ResetURL);
        cy.FillLogTextBox("#CurrentPassword",LoginData.password); 
        cy.FillLogTextBox('#Password',newPassword);
        cy.FillLogTextBox('#ConfirmPassword',confirmNewPassword);
    })

    //cy.get("#cmdContinue").click()
    //cy.server()
    //cy.route("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    //cy.intercept("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    //cy.window().then(win => { win.sessionStorage.setItem("ControlledByCypress", "true") })
    //cy.wait("@LoadDataCompleted")

})