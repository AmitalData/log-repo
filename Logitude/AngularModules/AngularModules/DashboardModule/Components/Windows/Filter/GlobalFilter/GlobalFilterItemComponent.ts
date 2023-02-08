import { Component, Input, OnInit } from "@angular/core";
import { FieldValueResolver } from "Infrastructure/Utilities/FieldValueResolver";
import { GlobalFilterItem } from "./GlobalFilterItem";

@Component({
    selector: 'GlobalFilterItem',
    templateUrl: './GlobalFilterItemComponent.html',
    styleUrls: ['./GlobalFilter.scss']
})

export class GlobalFilterItemComponent implements OnInit {
    @Input() FilterItem: GlobalFilterItem;
    @Input() Position: number;
    @Input() HasKpiChart: boolean;

    public Operators: any;
    public DateGroupCodes = ['Day', 'Week', 'Month', 'Quarter', 'Year'];

    public ObjectTableName: string = 'GlobalFilterItem';
    public DatesVisable: boolean = true;

    ngOnInit(): void {
        this.FillOperators();
    }

    get SelectedOperator(): Operator {
        return !this.FilterItem.Operator ? null : this.Operators.filter(x => x.Code == this.FilterItem.Operator)[0];
    }

    FillOperators() {
        this.Operators = [];
        switch (this.FilterItem.DataTypeCode) {
            case "DateTime":
            case "Date":
                this.Operators.push(new Operator("After", "GreaterThan"));
                this.Operators.push(new Operator("Before", "LessThan"));
                this.Operators.push(new Operator("Previous", "Previous"));
                this.Operators.push(new Operator("Current", "Current"));
                if (this.FilterItem.FieldId != "CreateDate") this.Operators.push(new Operator("Next", "Next"));
                this.Operators.push(new Operator("Between", "Between"));
                break;

            case "Integer":
            case "Decimal":
            case "Double":
                this.Operators.push(new Operator("Equal", "Equal"));
                this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
                this.Operators.push(new Operator("Greater Than", "GreaterThan"));
                this.Operators.push(new Operator("Less Than", "LessThan"));
                this.Operators.push(new Operator("Greater Than Or Equal", "GreaterThanOrEqual"));
                this.Operators.push(new Operator("Less Than Or Equal", "LessThanOrEqual"));
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                break;

            case "Boolean":
                this.Operators.push(new Operator("Equal", "Equal"));
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                break;

            case "LookUp":
                this.Operators.push(new Operator("Equal", "Equal"));
                this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                break;
            default:
                this.Operators.push(new Operator("Equal", "Equal"));
                this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
                this.Operators.push(new Operator("Contains", "Contains"));
                this.Operators.push(new Operator("Does Not Contain", "NotContains"));
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                break;
        }
    }

    public OperatorChanged(operator: Operator) {
        this.FilterItem.Operator = operator ? operator.Code : null;
        this.FilterItem.FieldValue3 = null;
        this.FilterItem.FieldValue = null;
        this.FilterItem.FieldValue2 = null;
        this.FilterItem.CompareWithPrevious = false;
        this.GetDefaultDateGroup();

        this.DatesVisable = false;
        setTimeout(() => {
            this.DatesVisable = true
        }, 10);
    }

    private GetDefaultDateGroup() {
        if (this.FilterItem.Operator == "Previous" || this.FilterItem.Operator == "Current" || this.FilterItem.Operator == "Next") this.FilterItem.DateGroupCode = "Day";
        else this.FilterItem.DateGroupCode = null;
    }

    public get TextFieldValue(): string {
        if (this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') return this.FilterItem.FieldValue3;
        return this.FilterItem.FieldValue;
    }

    public TextBoxValueChange(newValue) {
        if (this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') this.FilterItem.FieldValue3 = newValue;
        else this.FilterItem.FieldValue = newValue;
    }

    public GetTextInputType() {
        if (this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') return "unsinteger";
        if (this.FilterItem.DataTypeCode == "Text") return "nText";
        return this.FilterItem.DataTypeCode;
    }

    public ShowTextBox() {
        return this.IsNotEmptyNotEmtyOperator(this.FilterItem) &&
            (!this.FilterItem.DataTypeCode || this.FilterItem.DataTypeCode == '' ||
                this.FilterItem.DataTypeCode == 'Text' || this.FilterItem.DataTypeCode == 'nText' ||
                this.FilterItem.DataTypeCode == 'Integer' || this.FilterItem.DataTypeCode == 'Double' ||
                this.FilterItem.DataTypeCode == 'SigDouble' || this.FilterItem.DataTypeCode == 'Decimal' ||
                ((this.FilterItem.DataTypeCode == 'Date' || this.FilterItem.DataTypeCode == 'DateTime') && (this.FilterItem.Operator == 'Next' || this.FilterItem.Operator == 'Previous'))
            )
    }

    public DatePickerCondationValueChange(date) {
        this.FilterItem.FieldValue = date ? FieldValueResolver.ConvertUTCDateToString(date, "TreeFilter") : "";
        this.ValidateDate();
    }

    public ShowFirstDatePicker() {
        return this.IsNotEmptyNotEmtyOperator(this.FilterItem) && (this.FilterItem.DataTypeCode == 'DateTime' || this.FilterItem.DataTypeCode == 'Date') &&
            (this.FilterItem.Operator == "GreaterThan" || this.FilterItem.Operator == "LessThan" || this.FilterItem.Operator == "Between");
    }

    public SecondDatePickerCondationValueChange(date) {
        this.FilterItem.FieldValue2 = date ? FieldValueResolver.ConvertUTCDateToString(date, "TreeFilter") : "";
        this.ValidateDate();
    }

    public ShowSecondDatePicker() {
        return this.IsNotEmptyNotEmtyOperator(this.FilterItem) && (this.FilterItem.DataTypeCode == 'DateTime' || this.FilterItem.DataTypeCode == 'Date') && this.FilterItem.Operator == "Between";
    }

    public DateGroupCodeChange(DateGroupCode: string) {
        this.FilterItem.DateGroupCode = DateGroupCode;
    }

    public ShowDateGroups() {
        return this.IsNotEmptyNotEmtyOperator(this.FilterItem) && (this.FilterItem.DataTypeCode == 'DateTime' || this.FilterItem.DataTypeCode == 'Date') &&
            (this.FilterItem.Operator == "Previous" || this.FilterItem.Operator == "Current" || this.FilterItem.Operator == "Next");
    }

    public IsNotEmptyNotEmtyOperator(item: GlobalFilterItem) {
        return item.Operator && item.Operator != 'IsEmpty' && item.Operator != 'IsNotEmpty';
    }

    ValidateDate() {
        if (this.FilterItem.Operator == "Between" && this.FilterItem.FieldValue && this.FilterItem.FieldValue2 && this.FilterItem.FieldValue > this.FilterItem.FieldValue2) {
            this.FilterItem.UIProperties.SetValidity("FieldValue2", this.ObjectTableName, false, "To date must be larger than from date");
            this.FilterItem.UIProperties.SetValidity("FieldValue", this.ObjectTableName, false, "To date must be larger than from date");
        }
        else {
            this.FilterItem.UIProperties.SetValidity("FieldValue2", this.ObjectTableName, true, null);
            this.FilterItem.UIProperties.SetValidity("FieldValue", this.ObjectTableName, true, null);
        }
    }


}

export class Operator {
    Code: string;
    Name: string;

    constructor(name: string, code: string) {
        this.Code = code;
        this.Name = name;
    }
}