import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class SearchBoxResolver extends AbstractResolver implements IResolver {

    private workspace: string;
    public Workspace(value: string) {
        this.workspace = value;
        return this;
    }

    Reset() {
        super.Reset();
        this.workspace = null;
    }

    public Type(value: string) {

        if (this.workspace) {

            let selector = "QuickSearchTextBox";

            switch (this.workspace.toLowerCase()) {
                case "operations":
                case "shipments":{
                    selector = "searchbox";
                    break;
                }

                case "quotes": {
                    selector = "quicksearchtextbox";
                    break;
                }
            }

            cy.get(this.GetContainer())
                .find(selector)
                .eq(this.index)
                .within(() => {
                    cy.get('input').type(value).then(() => {
                        cy.get('ul > li').eq(0).click({ force: true });
                    });
                });
        }

        else {
            cy.get(this.GetContainer())
                .find(this.selector)
                .eq(this.index)
                .within(() => {
                    cy.get('input').type(value).then(() => {
                        cy.get('ul > li').eq(0).click({ force: true });
                    });
                });
        }

        this.Reset();
    }

}