import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class ButtonResolver extends AbstractResolver implements IResolver {

    private text: string;
    public Text(value: string) {
        this.text = value;
        return this;
    }

    private thenConfirmButtonText: string;
    public ThenConfirmButtonText(value: string) {
        this.thenConfirmButtonText = value;
        return this;
    }

    Reset() {
        super.Reset();
        this.text = null;
        this.thenConfirmButtonText = null;
    }

    public Click() {

        if (this.text) {
            cy.get(this.GetContainer())
                .find(this.selector)
                .contains(this.text, { matchCase: false })
                .eq(this.index)
                .click({ force: true });
        }

        else {
            cy.get(this.GetContainer())
                .find(this.selector)
                .eq(this.index)
                .click({ force: true });
        }

        if (this.thenConfirmButtonText) {

            let buttonText = this.thenConfirmButtonText;

            cy.get(this.GetContainer())
                .find('.ConfirmWindow')
                .eq(0)
                .within(() => {
                    cy.get('button').contains(buttonText).click({ force: true });
                });
        }

        this.Reset();
    }
}