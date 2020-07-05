
export class Login {

    constructor() {


    }


    dologin() {

        cy.visit('https://test.logitudeworld.com/TEST/')
        cy.get('input[id=Email]').clear();
        cy.get('input[id=Email]').type("sg1209@test.com");
        cy.get('input[id=Password]').clear();
        cy.get('input[id=Password]').type('!Sg13579');

        cy.get('#cmdLogin').click();
        cy.server();
        cy.route('**/GetLastTableUpdateDate/**').as('LoadDataCompleted');

        cy.wait('@LoadDataCompleted', {timeout:80000});
    }

}