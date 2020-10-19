import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class TextBoxResolver extends AbstractResolver implements IResolver {

    private objectTable: string;
    public ObjectTable(value: string) {
        this.objectTable = value;
        return this;
    }

    private objectField: string;
    public ObjectField(value: string) {
        this.objectField = value;
        return this;
    }

    private isTextArea: boolean = false;
    public IsTextArea(value: boolean = true) {
        this.isTextArea = value;
        return this;
    }

    private isEditGrid: boolean = false;
    public IsEditGrid(value: boolean = true) {
        this.isEditGrid = value;
        return this;
    }

    Reset() {
        super.Reset();
        this.objectTable = null;
        this.objectField = null;
        this.isTextArea = false;
        this.isEditGrid = false;
    }

    public Type(value: string) {
        if (this.selector) {

            if (this.isEditGrid) {
                cy.get(this.GetContainer())
                    .find(this.selector)
                    .eq(this.index)
                    .within((element) => {
                        cy.wrap(element).click({ force: true }).then(() => {
                            cy.get('input').type(value);
                        });
                    });
            }

            else {
                cy.get(this.GetContainer())
                    .find(this.selector)
                    .eq(this.index)
                    .type(value);
            }
        }

        else {

            const elementSelector = this.isTextArea ? "textarea" : "input";

            cy.get(this.GetContainer())
                .find('LogTextBox[ng-reflect--object-field-name=' + this.objectField + '][ng-reflect--object-table-name=' + this.objectTable + ']')
                .eq(this.index)
                .within(() => {
                    cy.get(elementSelector).type(value);
                });
        }

        this.Reset();
    }

}