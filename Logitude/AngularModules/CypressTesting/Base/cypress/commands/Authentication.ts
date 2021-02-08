declare namespace Cypress {
    interface Chainable {
        Login(): Chainable<Element>
        OpenChangePasswordPage(): Chainable<Element>
        RedirectToLogin(): Chainable<Element>
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

Cypress.Commands.add("OpenChangePasswordPage", () => {
    var Env = Cypress.env("Env");
    cy.fixture("Login.json").then((LoginData) => {
        var ResetURL = LoginData.url + "/PasswordChangePage.aspx?email=" + LoginData.email;
        cy.visit(ResetURL);
    }) 
})

Cypress.Commands.add("RedirectToLogin", () => {
    let mode = Cypress.env("Mode")
    
    if(mode.toLowerCase() === "development"){ 
        cy.fixture("Login.json").then((LoginData) => {
            CompleteRedirectToLoginProcess(LoginData.url + '/login.aspx');
        }) 
    }
    else{ 
        CompleteRedirectToLoginProcess(Cypress.env("Url") + '/login.aspx'); 
    }
})


function CompleteLoginProcess(Email:string, Password:string, URL: string, Tenant?:number){
    cy.visit(URL)
    cy.get("#Email").clear().type(Email).should("have.value", Email)
    cy.get("#Password").clear().type(Password).should("have.value", Password)
    cy.get("#cmdLogin").click()

    if (Tenant !== null) {
        cy.get("input[name='cmbTenants_input']").clear().type('(' + Tenant + ')')
        cy.get("#cmbTenants_listbox").children().contains('(' + Tenant + ')').eq(0).click({force:true})
        cy.get("#cmdContinue").click()
    }

    cy.window().then(win => { win.sessionStorage.setItem("ControlledByCypress", "true") })
    cy.intercept("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    cy.wait("@LoadDataCompleted")
}

function CompleteRedirectToLoginProcess(URL: string){
    cy.visit(URL) 
}