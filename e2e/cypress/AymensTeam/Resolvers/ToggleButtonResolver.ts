import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class ToggleButtonResolver extends AbstractResolver implements IResolver {


    public SelectByIndex(itemIndex: number) {

        const itemSelector = this.selector == "ToggleButton" ? "ToggleButtonItem" : "Button";

        cy.get(this.GetContainer())
            .find(this.selector)
            .eq(this.index)
            .then((element) => {
                cy.wrap(element).click({ force: true }).then(() => {
                    cy.wrap(element).find(itemSelector).eq(itemIndex).click({ force: true });
                });
            });


        //const elementIndex = this.index;
        //const elementSelector = this.selector;
        //cy.get(this.GetContainer())
        //        .within(() => {
        //            cy.get(elementSelector).eq(elementIndex).then((element) => {
        //                cy.wrap(element).click({ force: true }).then(() => {
        //                    cy.wrap(element).find(itemSelector).eq(itemIndex).click({ force: true });
        //                });
        //            });
        //        });

        this.Reset();
    }


    public SelectByLabels(label: string, itemLabel: string) {
        //cy.get(this.parent)
        //    .eq(0)
        //    .within(() => {
        //        cy.get(this.selector).eq(this.index).then((element) => {
        //            cy.wrap(element).click({ force: true }).then(() => {
        //                cy.wrap(element).find(this.itemSelector).eq(this.itemIndex).click({ force: true });
        //            });
        //        });
        //    });

        ////this.Reset();


        //cy.contains('Add Route').parents('.ToggleButton').eq(0).then(($element) => {

        //    cy.wrap($element).click({ force: true }).then(() => {
        //        cy.wrap($element).within(() => {
        //            cy.contains('Pickup').click({ force: true });
        //        });
        //    });

        //    //cy.contains('Add Route').parents('.ToggleButton').eq(0)
        //});
    }
}