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

  if(Env == 'staging'){
    URL = Cypress.env("ProdStagingURL");
    Email = Cypress.env("ProdStagingEmail");
    Password = Cypress.env("ProdStagingPassword");
  }
  else if(Env == 'cloudStaging'){
    URL = Cypress.env("CloudStagingURL");
    Email = Cypress.env("CloudStagingEmail");
    Password = Cypress.env("CloudStagingPassword");
  }
  else //test_staging
  {
    //URL = Cypress.env("TestURL");
    //Email = Cypress.env("TestEmail");
    //Password = Cypress.env("TestPassword");
  }

  cy.visit(URL)

  cy.get('#Email').type(Email, { delay: 50 });//.should('have.value', 'protractor2@test.com')

  cy.get('#Password').type(Password)
  cy.get('#cmdLogin').click()



 cy.server();
   //cy.route('test/api/ObjectTableLastUpdate/GetLastTableUpdateDate/?tenant=1102').as('LoadDataCompleted');
   cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');

 cy.wait('@LoadDataCompleted');




})



