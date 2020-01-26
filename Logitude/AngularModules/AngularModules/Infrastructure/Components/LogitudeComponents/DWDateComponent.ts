

declare var window: any;
declare var System: any;
import {Directive, ElementRef, Input, Output, Component, OnInit, EventEmitter} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';

import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool, DateTool} from '../../Tools';

import {ServiceResponse} from '../../DataContracts/ServiceResponse';

import {DateAgeHelper} from '../../../Infrastructure/Utilities/DateAgeHelper';

import {CustomEntityArgs} from './DWLogSearchWindowComponent';

import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';


@Component({
    selector: 'DWDate',
    moduleId: module.id,
    templateUrl: './DWDateComponent.html',
    inputs: ['DataContext', 'Operation', 'ObjectFieldName', 'SelectedValue'],

})


export class DWDateComponent extends BaseComponent {
    DateAgeHelper: DateAgeHelper = new DateAgeHelper(null);
    RangeLists: string[];
    SelectedValue: any;
    DataContext: any;
    ObjectFieldName: string;
    Item: any;
    IsLoad: boolean = false;

    @Output() ValueChanged = new EventEmitter();
    constructor() {
        super();

    }

    BetweenDateValue2: Date;
    BetweenDateValue1: Date;


    DateValue: Date;
    SelectedRange: string = "Day";
    IntervalValue: number = 1;


    ShowRange: boolean = false;
    ShowInterval: boolean = false;
    ShowLogDatePicker: boolean = false;

    ngOnInit() {

        this.FillListRange();
        this.ShowControl();
        this.InitializeComponent();
    }


    IsFirstTime: boolean = true;
    private operation: string;
    public get Operation() {
        return this.operation;
    }
    public set Operation(newValue: string) {
        if (this.operation != newValue) {
            this.operation = newValue;
            if (!this.IsFirstTime) {
                this.DataContext.TextValue = "";
                this.SetDefultValue(this.operation, newValue);
                this.ShowControl();
                this.SetValue();
            }
            this.IsFirstTime = false;
  
        }
    }


    ShowControl() {

        this.ShowLogDatePicker = false;
        this.ShowRange = false;
        this.ShowInterval = false;

        if (this.Operation == "Before" || this.Operation == "After") {
            this.ShowLogDatePicker = true;
        } else if (this.Operation == "Previous" || this.Operation == "Next" || this.Operation == "Current") {
            this.ShowRange = true;
            if (this.Operation != "Current") {
                this.ShowInterval = true;
            }
        }
    }

    FillListRange() {

        this.RangeLists = [];
        this.RangeLists.push("Day");
        this.RangeLists.push("Week");
        this.RangeLists.push("Month");
        this.RangeLists.push("Quarter");
        this.RangeLists.push("Year");

    }

    SelectedRangeChanged(value) {
        this.SelectedRange = value;
        this.SetValue();
    }

    DatePickerValueChange(value: Date) {
        if (this.IsDateValueChange(value, this.DateValue)) {
            this.DateValue = value;
            this.SetValue();
        }
    }



    DatePickerBetweenValue1Change(value: Date) {
        if (this.IsDateValueChange(value, this.BetweenDateValue1)) {
            this.BetweenDateValue1 = value;
            this.SetValue();
        }
    }


    DatePickerBetweenValue2Change(value: Date) {

        if (this.IsDateValueChange(value, this.BetweenDateValue2)) {
            this.BetweenDateValue2 = value;
            this.SetValue();
        }

    }



    IsDateValueChange(value1, value2) {
        var isChange: boolean = false;
        var valueA: string = value1 != null ? value1.toString() : "";
        var valueB: string = value2 != null ? value2.toString() : "";

        if (valueA != valueB) {
            isChange = true;
        }
        return isChange;
    }


    SelectedIntervalChanged(value) {

        if (value != this.IntervalValue) {
            this.IntervalValue = value;
            this.SetValue();
        }
    }


    SetDefultValue(operation, newoperation) {

        if ((operation == "Previous" && newoperation != "Next") || (operation == "Next" && newoperation != "Previous") || (operation == "Current" && newoperation != "Current")) {
            this.IntervalValue = 1;
            this.SelectedRange = "Day";
        }
    }

    SetValue() {
        var selectedValue = "";

        if (this.Operation == "Before" || this.Operation == "After") {
            if (this.DateValue) {
                var myFormats = DateTool.GetDateFormats(this.DateValue);
                if (myFormats) {
                    selectedValue = this.GetDateFormats(myFormats);
                }
            }

        } else if (this.Operation == "Previous" || this.Operation == "Next") {
            selectedValue = this.Operation;
            selectedValue += "^";
            selectedValue += (!AppTool.IsNullOrEmpty(this.IntervalValue) ? this.IntervalValue : 0);
            selectedValue += "^";
            selectedValue += (!AppTool.IsNullOrEmpty(this.SelectedRange) ? this.SelectedRange : "");

        } else if (this.Operation == "Current") {
            selectedValue = this.Operation;
            selectedValue += "^";
            selectedValue += (!AppTool.IsNullOrEmpty(this.SelectedRange) ? this.SelectedRange : "");

        }
        else if (this.Operation == "Between") {
            if (this.BetweenDateValue1) {
                var myFormats = DateTool.GetDateFormats(this.BetweenDateValue1);
                if (myFormats) {
                    selectedValue = this.GetDateFormats(myFormats);
                }
            }

            if (this.BetweenDateValue2) {
                var myFormats = DateTool.GetDateFormats(this.BetweenDateValue2);
                if (myFormats) {
                    selectedValue += ("^" + this.GetDateFormats(myFormats));
                }
            }


        }
        else if (this.Operation == "Between") {
            if (this.BetweenDateValue1) {
                var myFormats = DateTool.GetDateFormats(this.BetweenDateValue1);
                if (myFormats) {
                    selectedValue = this.GetDateFormats(myFormats);
                }
            }
        }

        if (this.DataContext.TextValue != selectedValue) {
            this.ValueChanged.emit(selectedValue);
        }


    }

        InitializeComponent() {

            if (this.SelectedValue) {
                if (this.Operation == "Before" || this.Operation == "After") {
                    this.DateValue = this.ConvertDateToString(this.SelectedValue);
                }

                else if (this.Operation == "Previous" || this.Operation == "Next") {

                    var values: string[] = this.SelectedValue.toString().split('^');
                    if (values.length > 1) this.IntervalValue = Number(values[1]);
                    if (values.length > 2) this.SelectedRange = values[2];
                }

                else if (this.Operation == "Current") {
                    var values: string[] = this.SelectedValue.toString().split('^');
                    if (values.length > 1) this.SelectedRange = values[1];
                }

                else if (this.Operation == "Between") {
                    var dateBetweenValues = this.SelectedValue.toString().split('^');
                    if (dateBetweenValues[0]) this.BetweenDateValue1 = this.ConvertDateToString(dateBetweenValues[0]);
                    if (dateBetweenValues[1]) this.BetweenDateValue2 = this.ConvertDateToString(dateBetweenValues[1]);
                }
            }
            this.IsLoad = true;
        }

        ConvertDateToString(value: any) {
            var result = new Date();
            if (value) {
                var date = new Date(value);
                result.setUTCFullYear(date.getUTCFullYear());
                result.setUTCMonth(date.getUTCMonth());
                result.setUTCDate(date.getUTCDate());
                result.setUTCHours(date.getUTCHours());
                result.setUTCMinutes(date.getUTCMinutes());
                result.setUTCSeconds(date.getUTCSeconds());
            }

            return result;
        }
        GetDateFormats(myFormats: any) {
            var result = "";
            if (myFormats) {

                var myDateParts = myFormats.DateParts;
                var stringOfYear = AppTool.PadLeft("" + myDateParts.Year, 4, '0');
                var stringOfMonth = AppTool.PadLeft("" + myDateParts.Month, 2, '0');
                var stringOfDay = AppTool.PadLeft("" + myDateParts.Day, 2, '0');
                result = stringOfYear + "-" + stringOfMonth + "-" + stringOfDay;

            }
            return result;
        }










    } 

