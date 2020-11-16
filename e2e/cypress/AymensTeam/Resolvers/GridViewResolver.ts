import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';
import { Resolvers } from './Resolvers';

export class GridViewResolver extends AbstractResolver implements IResolver {



    public DeleteSimpleGridRow(indexOfRow: number, withConfirm: boolean = true) {
        this.Delete('.SimpleGridViewRow', indexOfRow, withConfirm);
    }

    private Delete(rowSelector: string, indexOfRow: number, withConfirm: boolean) {

        cy.get(this.GetContainer())
            .find(rowSelector)
            .eq(indexOfRow)
            .then((element) => {
                const selector = 'iconbutton[ng-reflect--name=\"' + 'Delete' + '\"]';
                cy.wrap(element).find(selector).click({ force: true });

                if (withConfirm) {
                    cy.get('ConfirmWindow')
                        .eq(0)
                        .within(() => {
                            cy.get('button').contains('Yes').click();
                        });
                }
            });
    }
}