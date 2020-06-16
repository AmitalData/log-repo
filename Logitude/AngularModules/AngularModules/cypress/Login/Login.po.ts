/// <reference types="cypress" />



export class LoginComp {
}


  it('Login Successfully', () => {


 //cy.visit('http://localhost:4200/')
 //cy.visit('http://13.80.78.95:91/')
  cy.visit('https://test.logitudeworld.com/test')
 //cy.visit('https://system.logitudeworld.com')

  //cy.get('#Email').clear()
//cy.get('#Email').type('ahmadb@test.com', { delay: 50 }).should('have.value', 'ahmad2@test.com')

  //cy.get('#Email').type('protractor2@test.com', { delay: 50 }).should('have.value', 'protractor2@test.com')
  cy.get('#Email').type('protractor2@test.com', { delay: 50 }).should('have.value', 'protractor2@test.com')

  cy.get('#Password').type('!P123p456')
  cy.get('#cmdLogin').click()



 cy.server();
   cy.route('test/api/ObjectTableLastUpdate/GetLastTableUpdateDate/?tenant=1102').as('LoadDataCompleted');

 cy.wait('@LoadDataCompleted');




})



