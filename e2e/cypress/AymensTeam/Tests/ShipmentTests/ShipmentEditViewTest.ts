import { WindowResolver } from "../../Resolvers/WindowResolver";
import { MainMenuResolver } from "../../Resolvers/MainMenuResolver";
//import { ListOfValuesResolver } from "../../Resolvers/ListOfValuesResolver";
import { ToggleButtonResolver } from "../../Resolvers/ToggleButtonResolver";

describe('Open Login', () => {

    it('Test Shipments', () => {
        cy.visit('http://localhost:4200/');

        cy.get('#Email').clear();
        cy.get('#Password').clear();
        cy.get('#Email').type('angular@fnarsoft.com');
        cy.get('#Password').type('1');
        cy.get('#cmdLogin').click();
        cy.server();
        cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');
        cy.wait('@LoadDataCompleted');

        //MainMenuResolver.Select('#GeneralMHOperations');


        cy.get('li.RecentEntityItem').eq(0).click({ force: true });

        cy.get('#ShipmentTHRoutings').click({ force: true });


        cy.contains('Add Route').parents('.ToggleButton').eq(0).then(($element) => {

            cy.wrap($element).click({ force: true }).then(() => {
                cy.wrap($element).within(() => {
                    cy.contains('Pickup').click({ force: true });
                });
            });

            //cy.contains('Add Route').parents('.ToggleButton').eq(0)
        });

        //cy.contains('Add Route').parents('.ToggleButton').eq(0).click({ force: true }).then(() => {
        //    cy.contains('Add Route').parents('.ToggleButton').eq(0)
        //});


            ; // .find('#NOTF1').click();



    });
});
