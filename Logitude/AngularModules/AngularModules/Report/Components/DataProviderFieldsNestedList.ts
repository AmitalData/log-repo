import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DataProviderField } from 'Common/DataContracts/DataProviderField';

@Component({
    selector: 'DataProviderFieldsNestedList',
    templateUrl: './DataProviderFieldsNestedList.html'
})
export class DataProviderFieldsNestedList {
    @Input() field: DataProviderField;
    @Input() margin: any;
    @Input() isDestinationFields: boolean = false;
    @Input() IsFromReport: boolean = false;
    @Output() selectChangeEvent: EventEmitter<DataProviderField> = new EventEmitter<DataProviderField>();

    NestedSelectChange(field: DataProviderField) {
        this.selectChangeEvent.emit(field);
    }

    ShowField(field: DataProviderField) {
        return this.isDestinationFields || !field.IsChecked || (field.Fields && this.field.Fields.find(x => !x.IsChecked));
    }

    public get HasFields() {
        return this.field.Fields && this.field.Fields.length > 0;
    }

    SetSelected(field: DataProviderField) {
       // if (this.field.Fields != null && this.field.Fields.length > 0) return;
        this.selectChangeEvent.emit(this.field);
        if (!this.IsFromReport) field.ClassName = "SelectedListBoxItem";
    }

    OnShowHideColumnsClick(){
        this.field.FieldsOpened = !this.field.FieldsOpened;
    }
}
