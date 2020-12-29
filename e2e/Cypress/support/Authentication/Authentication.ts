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
            CompleteLoginProcess(LoginData.email,LoginData.password,LoginData.url,LoginData.tenant);
        }) 
    }
    else{

        CompleteLoginProcess(Cypress.env("Email"),Cypress.env("Password"),Cypress.env("Url"),Cypress.env("Tenant")); 
    }
})

Cypress.Commands.add("OpenAndFillChangePasswordPage", (newPassword,confirmNewPassword) => {
    var Env = Cypress.env("Env");
    cy.fixture("Data/" + Env + ".json").then((LoginData) => {
        var ResetURL = LoginData.url + "/PasswordChangePage.aspx?email=" + LoginData.email;
        cy.visit(ResetURL);
        cy.get("#CurrentPassword").clear().type(LoginData.password).should("have.value", LoginData.password)
        cy.get("#Password").clear().type(newPassword).should("have.value", newPassword)
        cy.get("#ConfirmPassword").clear().type(confirmNewPassword).should("have.value", confirmNewPassword)
    }) 
})


function CompleteLoginProcess(Email:string, Password:string, URL: string, Tenant?:number){
    cy.visit(URL)
    cy.get("#Email").clear().type(Email).should("have.value", Email)
    cy.get("#Password").clear().type(Password).should("have.value", Password)
    cy.get("#cmdLogin").click()

    if (Tenant !== null) {
        cy.get("input[name='cmbTenants_input']").clear().type('(' + Tenant + ')')
        cy.get("#cmbTenants_listbox").children().contains('(' + Tenant + ')').eq(0).click({force:true})
    }

    cy.get("#cmdContinue").click()
    cy.intercept("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    cy.window().then(win => { win.sessionStorage.setItem("ControlledByCypress", "true") })
    cy.wait("@LoadDataCompleted")
}