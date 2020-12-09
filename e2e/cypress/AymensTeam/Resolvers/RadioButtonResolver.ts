import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class RadioButtonResolver extends AbstractResolver implements IResolver {


    public Select() {
        if (this.selector) {
            cy.get(this.GetContainer())
                .find(this.selector)
                .eq(this.index)
                .parents('radiobutton')
                .eq(0)
                .within(() => {
                    cy.get('input').click({ force: true });
                });
        }
    }
}