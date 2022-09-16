import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { ObjectTablePMService } from "Infrastructure/Services/StandardPMs/ObjectTablePMService";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanValuesList } from "Workflow/Models/BooleanValuesList";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    selector: "FieldValue",
    templateUrl: "./FieldValueComponent.html"
})

export class FieldValueComponent extends BaseComponent implements OnInit {

    @Input() ObjectField: ObjectFieldPM;
    @Input() Name: string;
    @Input() CurrentValue: string;
    @Input() IsIntegerNumberInput: boolean = false;

    @Output() ValueChanged = new EventEmitter<string>();

    public DataContext: any = this;
    public LookupTable: ObjectTablePM;
    public PickListTable: ObjectTablePM;
    public DateTimeCurrentValue: Date;

    public ObjectTablePMService = new ObjectTablePMService();

    public BooleanValuesItems: ListItem[] = new BooleanValuesList().Items;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    constructor() {
        super();
    }

    ngOnInit() {
        this.initialize();
    }

    initialize() {
        if (!this.IsIntegerNumberInput) {
            this.initializeLookupTable();
            this.initializePickListTable();
            this.initializeDateTimeCurrentValue();
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
        if (this.isDateTimeObjectField()) {
            this.setDateTimeCurrentValue();
        }
    }

    setLookupTable() {
        this.LookupTable = (window as any).ObjectTables.filter((o: any) => o.Id === this.ObjectField.LookUpTableId)[0];
        if (!this.LookupTable) {
            this.ObjectTablePMService.get(this.ObjectField.LookUpTableId).subscribe((response: any) => { this.handleGetLookupTableResponse(response); });
        }
    }

    setPickListTable() {
        this.PickListTable = (window as any).ObjectTables.filter((o: any) => o.Id === this.ObjectField.ObjectTableId)[0];
        if (!this.PickListTable) {
            this.ObjectTablePMService.get(this.ObjectField.ObjectTableId).subscribe((response: any) => { this.handleGetPickListTableResponse(response); });
        }
    }

    handleGetLookupTableResponse(response: any) {
        if (!response.HasError) {
            this.LookupTable = response.Result;
        }
    }

    handleGetPickListTableResponse(response: any) {
        if (!response.HasError) {
            this.PickListTable = response.Result;
        }
    }

    setDateTimeCurrentValue() {
        this.DateTimeCurrentValue = this.CurrentValue ? new Date(this.CurrentValue) : null;
    }

    updateValue(value: any) {
        if (this.isDateTimeObjectField() && !this.IsIntegerNumberInput) {
            value = this.getDateValue(value);
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

    isTextObjectField() {
        return this.ObjectField && (this.ObjectField.DataTypeCode === FieldTypes.NText || this.ObjectField.DataTypeCode === FieldTypes.Text);
    }

    isBooleanObjectField() {
        return this.ObjectField && this.ObjectField.DataTypeCode === FieldTypes.Boolean;
    }

    isDateTimeObjectField() {
        return this.ObjectField && (this.ObjectField.DataTypeCode === FieldTypes.DateTime || this.ObjectField.DataTypeCode === FieldTypes.Date);
    }

    isNumberObjectField() {
        return this.ObjectField &&
            (
                this.ObjectField.DataTypeCode === FieldTypes.BigInteger ||
                this.ObjectField.DataTypeCode === FieldTypes.Decimal ||
                this.ObjectField.DataTypeCode === FieldTypes.Double ||
                this.ObjectField.DataTypeCode === FieldTypes.Integer ||
                this.ObjectField.DataTypeCode === FieldTypes.SigDouble ||
                this.ObjectField.DataTypeCode === FieldTypes.UnsDecimal ||
                this.ObjectField.DataTypeCode === FieldTypes.UnsInteger
            );
    }
}