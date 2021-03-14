declare namespace Cypress {
    interface Chainable {
        Login(customerCareUser?: boolean): Chainable<Element>
        GetLoggedInUser(customerCareUser?: boolean): Chainable<string>
        LogoutThenLogin(customerCareUser?: boolean): Chainable<Element>
        OpenChangePasswordPage(): Chainable<Element>
        RedirectToLogin(): Chainable<Element>
    }
}

Cypress.Commands.add("Login", (customerCareUser = false) => {
    let mode = Cypress.env("Mode");
    if (mode.toLowerCase() === "development") {
        cy.fixture("Login.json").then(loginData => {
            let email = customerCareUser ? loginData.customerCareEmail : loginData.email;
            let password = customerCareUser ? loginData.customerCarePassword : loginData.password;
            let url = loginData.url;
            let tenant = customerCareUser ? loginData.tenant : null;
            CompleteLoginProcess(email, password, url, tenant);
        });
    }
    else {
        let email = customerCareUser ? Cypress.env("CustomerCareEmail") : Cypress.env("Email");
        let password = customerCareUser ? Cypress.env("CustomerCarePassword") : Cypress.env("Password");
        let url = Cypress.env("Url");
        let tenant = customerCareUser ? Cypress.env("Tenant") : null;
        CompleteLoginProcess(email, password, url, tenant);
    }
})

Cypress.Commands.add("GetLoggedInUser", (customerCareUser = false) => {
    let mode = Cypress.env("Mode");
    if (mode.toLowerCase() === "development") {
        cy.fixture("Login.json").then(loginData => {
            return customerCareUser ? loginData.customerCareEmail : loginData.email;
        });
    }
    else {
        return customerCareUser ? Cypress.env("CustomerCareEmail") : Cypress.env("Email");
    }
})

Cypress.Commands.add("LogoutThenLogin", (customerCareUser = false) => {
    cy.intercept("**/Login.aspx").as("LoginPage");
    cy.get("iconbutton[title='Sign Out'] img").click();
    cy.wait("@LoginPage");
    cy.Login(customerCareUser);
})

Cypress.Commands.add("OpenChangePasswordPage", () => {
    cy.fixture("Login.json").then((LoginData) => {
        var resetURL = LoginData.url + "/PasswordChangePage.aspx?email=" + LoginData.email;
        cy.visit(resetURL);
    })
})

Cypress.Commands.add("RedirectToLogin", () => {
    let mode = Cypress.env("Mode")
    if (mode.toLowerCase() === "development") {
        cy.fixture("Login.json").then((LoginData) => {
            CompleteRedirectToLoginProcess(LoginData.url + '/login.aspx');
        })
    }
    else {
        CompleteRedirectToLoginProcess(Cypress.env("Url") + '/login.aspx');
    }
})

function CompleteLoginProcess(Email: string, Password: string, URL: string, Tenant?: number) {
    cy.visit(URL)
    cy.intercept("**/ObjectTableLastUpdate/**").as("LoadDataCompleted")
    cy.window().then(win => { win.sessionStorage.setItem("ControlledByCypress", "true") })
    cy.get("#Email").clear().type(Email).should("have.value", Email)
    cy.get("#Password").clear().type(Password).should("have.value", Password)
    cy.get("#cmdLogin").click()

    if (Tenant !== null) {
        cy.get("input[name='cmbTenants_input']").clear().type('(' + Tenant + ')')
        cy.get("#cmbTenants_listbox").children().contains('(' + Tenant + ')').eq(0).click({ force: true })
        cy.get("#cmdContinue").click()
    }
    cy.wait("@LoadDataCompleted")
}

function CompleteRedirectToLoginProcess(URL: string) {
    cy.visit(URL)
}