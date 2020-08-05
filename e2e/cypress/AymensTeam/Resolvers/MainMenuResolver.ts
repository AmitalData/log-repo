import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class MainMenuResolver extends AbstractResolver implements IResolver {

    private text: string;
    public Text(value: string) {
        this.text = value;
        return this;
    }

    Reset() {
        super.Reset();
        this.text = null;
    }

    public Select() {

        if (this.text) {
            cy.get(this.GetContainer()).contains('li.DefaultMenuItem', this.text).click();

            switch (this.text) {
                case "Operations": {
                    cy.get('searchbox[ng-reflect--object-table-name=Shipment]').should('be.exist');
                    break;
                }

                case "Quotes": {
                    cy.get('quicksearchtextbox[ng-reflect--object-table-name=Quote]').should('be.exist');
                    break;
                }
            }
        }

        else {
            cy.get(this.GetContainer()).find(this.selector).click();
        }

        this.Reset();
    }
}

