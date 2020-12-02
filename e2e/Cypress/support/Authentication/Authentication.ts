declare namespace Cypress {
    interface Chainable {
        Login(): Chainable<Element>
        OpenAndFillChangePasswordPage(newPassword:string,confirmNewPassword:string): Chainable<Element>
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

        if (LoginData.tenant !== null) {
            cy.get("input[name='cmbTenants_input']").type(LoginData.tenant)
            //cy.wait(500)
            cy.get("#cmbTenants_listbox").children().contains('(' + LoginData.tenant + ')').eq(0).click({force:true})

        }

    }



    cy.server()
    cy.route("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
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
