import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class LOVResolver extends AbstractResolver implements IResolver {

    private objectTable: string;
    public ObjectTable(objectTable: string) {
        this.objectTable = objectTable;
        return this;
    }

    private objectField: string;
    public ObjectField(objectField: string) {
        this.objectField = objectField;
        return this;
    }

    Reset() {
        super.Reset();
        this.objectTable = null;
        this.objectField = null;
    }

    public Type(value: string) {

        const itemSelector = value == "{downarrow}" ? "li.DropDownListItem" : "li.DropDownListItem.liItemSelected";
        cy.get(this.selector).clear();
        if (this.selector) {
            cy.get(this.GetContainer())
                .find(this.selector)
                .eq(this.index)
                .parents('loglov')
                .eq(0)
                .within(() => {
                    cy.get('input').clear().type(value).then(() => {
                        cy.get('ul.DropDownList').find(itemSelector).eq(0).click({ force: true });
                    });
                });
        }

        else {
            cy.get(this.GetContainer())
                .find('loglov[ng-reflect--object-field-name=' + this.objectField + '][ng-reflect--object-table-name=' + this.objectTable + ']')
                .eq(this.index)
                .within(() => {
                    cy.get('input').type(value).then(() => {
                        cy.get('ul.DropDownList').find(itemSelector).eq(0).click({ force: true });
                    });
                });
        }

        this.Reset();
    }

    public SelectFirst() {
        this.Type('{downarrow}');
    }
}