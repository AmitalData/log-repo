import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { UIProperty } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { ObjectTableList } from "Infrastructure/EntityLists/ObjectTableList";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanValuesList } from "Workflow/Lists/BooleanValuesList";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";

@Component({
    selector: "FieldValue",
    templateUrl: "./FieldValueComponent.html"
})

export class FieldValueComponent extends BaseComponent implements OnInit, AfterViewInit {

    @Input() Name: string;
    @Input() CurrentValue: string;
    @Input() ObjectField: ObjectFieldList | null = null;
    @Input() IsIntegerNumberInput: boolean = false;
    @Input() DataType: string | null = null;
    @Input() LookupType: string | null = null;
    @Input() IsDisabled: boolean = false;
    @Input() QueryFilterItems: ApiQueryFilters | null = null;

    @Output() ValueChanged = new EventEmitter<string>();

    public Value: any = null;

    public DataContext: any = this;
    public LookupTable: ObjectTableList;
    public PickListTable: ObjectTableList;
    public DateTimeCurrentValue: Date;
    public LookupDataTypeTable: ObjectTableList;

    public BooleanValuesItems: ListItem[] = new BooleanValuesList().Items;

    public FieldTypes = FieldTypes;

    constructor() {
        super();
    }

    ngOnInit() {
        this.initialize();
    }

    ngAfterViewInit() {
        this.initializeFieldUIPropertyChangedEvent();
    }

    initialize() {
        this.initializeDataType();
        this.initializeDateTimeCurrentValue();
        if (!this.IsIntegerNumberInput) {
            this.initializeLookupTable();
            this.initializePickListTable();
        }
    }

    initializeDataType() {
        if (this.DataType) {
            this.DataType = this.DataType.replace("[]", "");
            this.LookupDataTypeTable = this.isLookupDataType() ? ObjectTables.getByName(this.LookupType) : null;
        }
    }

    initializeDateTimeCurrentValue() {
        if (this.isDateTimeObjectField() || this.isDataTypeDateTime()) {
            this.setDateTimeCurrentValue();
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

    initializeFieldUIPropertyChangedEvent() {
        if (this.isDateTimeObjectField() || this.isDataTypeDateTime()) {
            let fieldUIProperty = this.getFieldUIProperty();
            if (fieldUIProperty) {
                fieldUIProperty.UIPropertyChanged.subscribe((event: any) => {
                    if (event && (event instanceof UIProperty) && !event.ValidValue) {
                        this.handleUpdateValue(null);
                    } else {
                        this.handleUpdateValue(this.Value);
                    }
                });
            }
        }
    }

    getFieldUIProperty() {
        if (this.Name) {
            let uiProperties = this.UIProperties.UIPropertyList.filter(p => p.FieldName === this.Name);
            return uiProperties && uiProperties.length > 0 ? (uiProperties[0] || null) : null;
        }
        return null;
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
        this.Value = value;

        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(p => !p.ValidValue);
        if (notValidUIProperties.length === 0) {
            this.handleUpdateValue(value);
        } else {
            this.handleUpdateValue(null);
        }
    }

    handleUpdateValue(value: any) {
        if ((this.isDateTimeObjectField() || this.isDataTypeDateTime()) && !this.IsIntegerNumberInput) {
            value = this.getDateValue(value);
        }
        else if (this.isLookupObjectField() && this.LookupTable) {
            value = this.getLookupValue(value, this.LookupTable.KeyPropertyPath);
        }
        else if (this.isLookupDataType() && this.LookupDataTypeTable) {
            value = this.getLookupValue(value, this.LookupDataTypeTable.KeyPropertyPath);
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

    isLookupDataType() {
        return this.DataType && this.DataType === FieldTypes.LookUp;
    }
}