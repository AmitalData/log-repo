declare var window: any;
declare var System: any;
import {Directive, ElementRef, Input, Output, Component, OnInit, EventEmitter} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';

import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool} from '../../Tools';


import {ServiceResponse} from '../../DataContracts/ServiceResponse';


import {CustomEntityArgs} from './DWLogSearchWindowComponent';


import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';




@Component({
    selector: 'DWDate',
    moduleId: module.id,
    templateUrl: './DWDateComponent.html',
    inputs: ['DataContext', 'Operation', 'ObjectFieldName', 'SelectedValue'],
       
})


export class DWDateComponent extends BaseComponent {

    RangeLists: string[];
    SelectedValue: any;
    DataContext: any;
    ObjectFieldName: string;
    Item: any;

    constructor() {
        super();
       
    }


    SelectedRange: string;
    DateValue: Date;

    ShowRange: boolean = false;
    ShowInterval: boolean = false;
    ShowLogDatePicker: boolean = false;

    ngOnInit() { 


        this.FillListRange();
        this.ShowControl();
        this.GetValue();
    }

    private operation: string;
    public get Operation() {
        return this.operation;
    }
    public set Operation(newValue: string) {
        if (this.operation != newValue) {
            this.operation = newValue;
            this.ShowControl();
            this.SetValue();

        }
    }
    IntervalValue: number;
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
        } else {
            this.ShowLogDatePicker = true;
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

    DatePickerValueChange(value) {
        if (value != this.DateValue) {
            this.DateValue = value;
            this.SetValue();
        }
    }

    SelectedIntervalChanged(value) {

        if (value != this.IntervalValue) {
            this.IntervalValue = value;
            this.SetValue();
        }
    }



    SetValue() {

        if (this.Operation == "Before" || this.Operation == "After") {
            this.SelectedValue = this.DateValue;


        } else if (this.Operation == "Previous" || this.Operation == "Next" ) {
            this.SelectedValue = this.Operation;
            this.SelectedValue += "^";
            this.SelectedValue += (!AppTool.IsNullOrEmpty(this.IntervalValue) ? this.IntervalValue :0);
            this.SelectedValue += "^";
            this.SelectedValue += (!AppTool.IsNullOrEmpty(this.SelectedRange) ? this.SelectedRange : "");

        } else if (this.Operation == "Current") {
            this.SelectedValue = this.Operation;
            this.SelectedValue += "^";
            this.SelectedValue += (!AppTool.IsNullOrEmpty(this.SelectedRange) ? this.SelectedRange:"");

        }

        if (this.DataContext.TextValue != this.SelectedValue) {
            this.DataContext.TextValue = this.SelectedValue;
        }

    }

    GetValue() {

        if (this.Operation == "Before" || this.Operation == "After") {
            this.DateValue = this.SelectedValue;

        } else if (this.Operation == "Previous" || this.Operation == "Next") {

            var values: string[] = this.SelectedValue.toString().split('^');
            if (values.length > 1) this.IntervalValue = Number(values[1]);
            if (values.length > 2) this.SelectedRange = values[2];
            
        }
        else if (this.Operation == "Current") {
            var values: string[] = this.SelectedValue.toString().split('^');
            if (values.length > 1) this.SelectedRange = values[1];
        
        }

    }

}

