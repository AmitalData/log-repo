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
    @Input() set checkAll(value: boolean) {
        if (!value)
            this.field.IsChecked = value;
    }

    checkChange(): void {
        this.field.IsChecked = !this.field.IsChecked;
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
