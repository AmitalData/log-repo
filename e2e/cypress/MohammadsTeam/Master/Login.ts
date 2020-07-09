/// <reference types="cypress"/>

export class Login {
      constructor() {


 
  }
  login(url: string, email: string, password: string) {
 {
 cy.visit(url);
    cy.get('input[id=Email]').clear().type(email)
    cy.get('input[id=Password]').clear().type(password)
    cy.get('input[id=cmdLogin]').click()


cy.server();
   cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');

 

 cy.wait('@LoadDataCompleted');




 }

  }
}

