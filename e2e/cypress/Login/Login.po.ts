/// <reference types="cypress" />



export class LoginComp {
}


it('Login Successfully', () => {


    //cy.visit('http://localhost:4200/')

    //cy.visit('https://test.logitudeworld.com/test')
    var Email = Cypress.env("TestEmail");
    var Password = Cypress.env("TestPassword");
    var URL = Cypress.env("TestURL");
    var Env = Cypress.env("Env");

    URL = Cypress.env("LocalURL");
    Email = Cypress.env("LocalEmail");
    Password = Cypress.env("LocalPassword");
   
   

    cy.visit(URL)

    cy.get('#Email').clear();
    cy.get('#Password').clear();
    cy.get('#Email').type(Email, { delay: 50 });//.should('have.value', 'protractor2@test.com')

    cy.get('#Password').type(Password)
    cy.get('#cmdLogin').click()



    cy.server();
    //cy.route('test/api/ObjectTableLastUpdate/GetLastTableUpdateDate/?tenant=1102').as('LoadDataCompleted');
    cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');

    cy.window().then(win => { win.sessionStorage.setItem('ControlledByCypress', 'true') });

    cy.wait('@LoadDataCompleted'); 




})



