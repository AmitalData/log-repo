import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class DatePickerResolver extends AbstractResolver implements IResolver {

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



    Reset() {
        super.Reset();
        this.objectTable = null;
        this.objectField = null;
    }

    public Type(value: string) {
        if (this.selector) {
            cy.get(this.GetContainer())
                .find(this.selector)
                .eq(this.index)
                .type(value);
        }

        else {
            cy.get(this.GetContainer())
                .find('logdatepicker[ng-reflect--object-field-name=' + this.objectField + '][ng-reflect--object-table-name=' + this.objectTable + ']')
                .eq(this.index)
                .within(() => {
                    cy.get('input').type(value);
                });
        }

        this.Reset();
    }

}