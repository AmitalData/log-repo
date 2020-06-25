/// <reference types="cypress" />



export class LoginComp {
}


  it('Login Successfully', () => {


 //cy.visit('http://localhost:4200/')
 
  //cy.visit('https://test.logitudeworld.com/test')
  cy.visit(Cypress.env("URL"))

  cy.get('#Email').type(Cypress.env("Email"), { delay: 50 }).should('have.value', 'protractor2@test.com')

  cy.get('#Password').type(Cypress.env("Password"))
  cy.get('#cmdLogin').click()



 cy.server();
   //cy.route('test/api/ObjectTableLastUpdate/GetLastTableUpdateDate/?tenant=1102').as('LoadDataCompleted');
   cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');

 cy.wait('@LoadDataCompleted');




})



