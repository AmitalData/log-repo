import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanValuesList } from "Workflow/Models/BooleanValuesList";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Models/ObjectTables";

@Component({
    selector: "FieldValue",
    templateUrl: "./FieldValueComponent.html"
})

export class FieldValueComponent extends BaseComponent implements OnInit {

    @Input() ObjectField: ObjectFieldList;
    @Input() Name: string;
    @Input() CurrentValue: string;
    @Input() IsIntegerNumberInput: boolean = false;
    @Input() DataType: string;
    @Input() IsDisabled: boolean = false;

    @Output() ValueChanged = new EventEmitter<string>();

    public DataContext: any = this;
    public LookupTable: ObjectTablePM;
    public PickListTable: ObjectTablePM;
    public DateTimeCurrentValue: Date;

    public BooleanValuesItems: ListItem[] = new BooleanValuesList().Items;

    public FieldTypes = FieldTypes;

    constructor() {
        super();
    }

    ngOnInit() {
        this.initialize();
    }

    initialize() {
        this.initializeDateTimeCurrentValue();

        if (!this.IsIntegerNumberInput) {
            this.initializeLookupTable();
            this.initializePickListTable();
        }
    }

    initializeLookupTable() {
        if (this.isLookupObjectField()) {
            this.setLookupTable();
        }
    }

    initializePickListTable() {
        if (this.isPickListObjectField()) {
            this.setPickListTable();
        }
    }

    initializeDateTimeCurrentValue() {
        if (this.isDateTimeObjectField() || this.isDataTypeDateTime()) {
            this.setDateTimeCurrentValue();
        }
    }

    setLookupTable() {
        this.LookupTable = ObjectTables.getById(this.ObjectField.LookUpTableId);
    }

    setPickListTable() {
        this.PickListTable = ObjectTables.getById(this.ObjectField.ObjectTableId);
    }

    setDateTimeCurrentValue() {
        this.DateTimeCurrentValue = this.CurrentValue ? new Date(this.CurrentValue) : null;
    }

    updateValue(value: any) {
        if ((this.isDateTimeObjectField() || this.isDataTypeDateTime()) && !this.IsIntegerNumberInput) {
            value = this.getDateValue(value);
        } else if (this.isLookupObjectField() && this.LookupTable) {
            value = this.getLookupValue(value, this.LookupTable.KeyPropertyPath);
        }
        this.ValueChanged.emit(value);
    }

    getDateValue(value: Date) {
        if (value) {
            return [
                value.getFullYear(),
                this.getTwoDigitsNumber(value.getMonth() + 1),
                this.getTwoDigitsNumber(value.getDate())
            ].join('-');
        }
        return null;
    }

    getLookupValue(value: any, lookupKeyPropertyPath: string) {
        if (value) {
            return lookupKeyPropertyPath ? (value[lookupKeyPropertyPath] || value.Id) : value.Id;
        }
        return null;
    }

    getTwoDigitsNumber(number: number) {
        if (number) {
            return (number < 10) ? "0" + number : number;
        }
        return "00";
    }

    isLookupObjectField() {
        return this.ObjectField && this.ObjectField.DataTypeCode === FieldTypes.LookUp;
    }

    isPickListObjectField() {
        return this.ObjectField && this.ObjectField.DataTypeCode === FieldTypes.PickList;
    }

    isDateTimeObjectField() {
        return (this.ObjectField && (this.ObjectField.DataTypeCode === FieldTypes.DateTime || this.ObjectField.DataTypeCode === FieldTypes.Date));
    }

    isDataTypeDateTime() {
        return (this.DataType && (this.DataType === FieldTypes.DateTime || this.DataType === FieldTypes.Date));
    }
}