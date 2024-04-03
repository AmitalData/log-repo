import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { AppTool } from 'Infrastructure/Tools';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { FieldValueResolver } from 'Infrastructure/Utilities/FieldValueResolver';

@Component({
    selector: 'GlobalFilterValue',
    templateUrl: './GlobalFilterValueComponent.html'
})

export class GlobalFilterValueComponent implements OnInit {
    ngOnInit(): void {
       
    }
    // @Input() FilterItem: GlobalFilterItem;
    // @Output() FilterItemChange = new EventEmitter<GlobalFilterItem>();

    // public BooleanList: boolean[] = [true, false];
    // public DateGroupCodes = ['Day', 'Week', 'Month', 'Quarter', 'Year'];

    // ShowTextBox() {
    //     return this.IsNotEmptyOperator() &&
    //         (!this.FilterItem.DataTypeCode || this.FilterItem.DataTypeCode == '' ||
    //             this.FilterItem.DataTypeCode == 'Text' || this.FilterItem.DataTypeCode == 'nText' ||
    //             this.FilterItem.DataTypeCode == 'Integer' || this.FilterItem.DataTypeCode == 'Double' ||
    //             this.FilterItem.DataTypeCode == 'SigDouble' || this.FilterItem.DataTypeCode == 'Decimal' ||
    //             ((this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') && (this.FilterItem.FilterOperator == 'Next' || this.FilterItem.FilterOperator == 'Previous'))
    //         )
    // }

    // GetTextInputType() {
    //     if (this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') return "Integer";
    //     if (this.FilterItem.DataTypeCode == "Text") return "nText";
    //     return this.FilterItem.DataTypeCode;
    // }


    // TextBoxValueChange(newValue: any) {
    //     if (this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') this.FilterItem.FieldValue3 = newValue;
    //     else this.FilterItem.FieldValue = newValue;

    //     this.FilterItemChange.emit(this.FilterItem);
    //     // MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + newValue });
    // }

    // IsNotEmptyOperator() {
    //     return this.FilterItem.FilterOperator && this.FilterItem.FilterOperator != 'IsEmpty' && this.FilterItem.FilterOperator != 'IsNotEmpty';
    // }

    // get TextFieldValue(): string {
    //     if (this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') return this.FilterItem.FieldValue3;
    //     return this.FilterItem.FieldValue;
    // }

    // BooleanListValueChanged(newValue: boolean) {
    //     this.FilterItem.FieldValue = newValue ? "true" : "false";
    //     this.FilterItemChange.emit(this.FilterItem);
    //     //MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + newValue });
    // }

    // LogLovCondationValueChange(newValue) {
    //     this.FilterItem.FieldValue = newValue ? !AppTool.IsNullOrEmpty(newValue.Id) ? newValue.Id : newValue.Code : "";
    //     this.FilterItemChange.emit(this.FilterItem);
    //     // MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Operator Change", Message: "Changed To " + this.FieldValue });
    // }

    // ShowFirstDatePicker() {
    //     return this.IsNotEmptyOperator() && (this.FilterItem.DataTypeCode == 'DateTime' || this.FilterItem.DataTypeCode == 'Date') &&
    //         (this.FilterItem.FilterOperator == "GreaterThan" || this.FilterItem.FilterOperator == "LessThan" || this.FilterItem.FilterOperator == "Between");
    // }

    // DatePickerCondationValueChange(date) {
    //     this.FilterItem.FieldValue = date ? FieldValueResolver.ConvertUTCDateToString(date, "TreeFilter") : "";
    //     this.FilterItemChange.emit(this.FilterItem);
    //     //MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + this.FilterItem.FieldValue });
    // }

    // ShowSecondDatePicker() {
    //     return this.IsNotEmptyOperator() && (this.FilterItem.DataTypeCode == 'DateTime' || this.FilterItem.DataTypeCode == 'Date') && this.FilterItem.FilterOperator == "Between";
    // }

    // SecondDatePickerCondationValueChange(date) {
    //     this.FilterItem.FieldValue2 = date ? FieldValueResolver.ConvertUTCDateToString(date, "TreeFilter") : "";
    //     this.FilterItemChange.emit(this.FilterItem);
    //     //MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Second Date Value Change", Message: "Changed To " + this.FieldValue });
    // }

    // ShowDateGroups() {
    //     return this.IsNotEmptyOperator() && (this.FilterItem.DataTypeCode == 'DateTime' || this.FilterItem.DataTypeCode == 'Date') &&
    //         (this.FilterItem.FilterOperator == "Previous" || this.FilterItem.FilterOperator == "Current" || this.FilterItem.FilterOperator == "Next");
    // }

    // DateGroupCodeChange(DateGroupCode: string) {
    //     this.FilterItem.DateGroupCode = DateGroupCode;
    //     this.FilterItemChange.emit(this.FilterItem);
    //     //MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Date Group Change", Message: "Changed To " + DateGroupCode });
    // }

    // ngOnInit(): void {

    // }

}