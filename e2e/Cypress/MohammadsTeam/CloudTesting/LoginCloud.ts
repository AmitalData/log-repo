
export class LoginCloud {

    constructor() {


    }


    dologin() {

        cy.visit('https://pre.amital.co.il/')
        cy.get('input[id=Email]').clear();
        cy.get('input[id=Email]').type('sumaya@cloud.com');
        cy.get('input[id=Password]').clear();
        cy.get('input[id=Password]').type('Sg0592463934!');

        cy.get('#cmdLogin').click();
        cy.server();
        cy.route('**/GetLastTableUpdateDate/**').as('LoadDataCompleted');

        cy.wait('@LoadDataCompleted');
    }

}