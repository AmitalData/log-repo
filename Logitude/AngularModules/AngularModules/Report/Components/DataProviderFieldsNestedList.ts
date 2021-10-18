import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DataProviderField } from 'Common/DataContracts/DataProviderField';

@Component({
    selector: 'DataProviderFieldsNestedList',
    templateUrl: './DataProviderFieldsNestedList.html'
})
export class DataProviderFieldsNestedList {
    @Input() field: DataProviderField;
    @Input() isDisabled: boolean;
    @Input() margin: any;

    @Output() checkChangeEvent: EventEmitter<string> = new EventEmitter<string>();

    @Input() set checkAll(value: boolean) {
        if (!value)
            this.field.IsChecked = value;
    }

    NestedCheckChange(expression: string) {
        this.checkChangeEvent.emit(expression)
    }

    CheckChange(): void {
        this.field.IsChecked = !this.field.IsChecked;
        this.checkChangeEvent.emit(this.field.Expression)
        if (this.field.IsChecked === false) {
            if (this.field.Fields != null) {
                this.checkAll = this.field.IsChecked;
                this.field.Fields.forEach(item => {
                    item.IsChecked = false;
                });
            }
        }
    }
}
