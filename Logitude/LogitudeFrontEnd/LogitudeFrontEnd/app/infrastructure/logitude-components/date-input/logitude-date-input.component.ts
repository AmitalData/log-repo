import {Directive, ElementRef, Renderer, Input, Output, Component, View, OnInit, OnChanges} from 'angular2/core';
import {BaseComponent} from '../Base.Component';
import {UIProperty, UIProperties} from '../UIProperties';
import {
CORE_DIRECTIVES,
FORM_DIRECTIVES,
FormBuilder,
ControlGroup,
Validators,
AbstractControl, Control} from 'angular2/common';
@Component({
    selector: 'logitude-date-input',
    template:
    `

       <div *ngIf="uiProperty.IsVisible" >

        <input [disabled]="!uiProperty.IsEnabled" *ngIf="logitudeForm" type="date" [ngFormControl]="ctrl" [(ngModel)]="DataContext[objectfieldname]" (focus)="onFocus()" (blur)="onBlur()"/>
       
        <div *ngIf="show" [hidden]="!uiProperty.IsRequired || (DataContext[objectfieldname] !=null && DataContext[objectfieldname] !='')" class="alert alert-danger">
          {{uiProperty.ValidationError}}
        </div>
        </div>

    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES],
    inputs: ['objectfieldname', 'objecttablename', 'DataContext'],

})



export class LogitudeDateInputComponent implements OnInit {
    public objectfieldname: string;
    public objecttablename: string;
    public DataContext: any;

    private dataContext: BaseComponent;
    private uiProperty: UIProperty;
    private show: boolean;
    ctrl: Control;

    @Input() logitudeForm: ControlGroup;



    constructor() {
        this.show = false;


    }
    ngOnInit() {

        var table = window.ObjectTables.filter(d => d.Name === this.objecttablename)[0];
        var field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.objectfieldname)[0];
        
        
        //this.dataContext = this.DataContext as BaseComponent;
        //console.log(this.DataContext);
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.objectfieldname, this.objecttablename);

        this.ctrl = new Control(this.DataContext[this.objectfieldname]);
        this.logitudeForm.addControl(this.objectfieldname, this.ctrl);
        //this.ctrl.valueChanges.subscribe(
        //    (value: string) => {
        //        //this.uiProperty = this.dataContext.UIProperties.RefreshUIProperty(this.objectfieldname, this.objecttablename, this.dataContext.EntityPM);
        //    }
        //);
        this.uiProperty.UIPropertyChanged.subscribe(uiProperty=> this.SetControlPropertiesAndValidations(uiProperty, this.ctrl));
        this.SetControlPropertiesAndValidations(this.uiProperty, this.ctrl);
    }

    SetControlPropertiesAndValidations(uiProperty: UIProperty, ctrl: Control) {
        //console.log(ctrl.errors);
        //console.log("set properties", uiProperty);
        this.uiProperty.IsRequired = uiProperty.IsRequired;
        var table = window.ObjectTables.filter(d => d.Name === uiProperty.ObjectTableName)[0];
        var field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === uiProperty.FieldName)[0];

        var minlength = field.MinLength;
        var maxlenght = field.MaxLength;
        var hasminmax: boolean;
        hasminmax = false;

        if (field.DataTypeCode.toLowerCase() == "text" || field.DataTypeCode.toLowerCase() == "ntext") {
            if (maxlenght != 0) {
                hasminmax = true;
            }
        }

        if (this.uiProperty.IsRequired) {
            //console.log("i'm required");

            if (this.DataContext[this.objectfieldname] == null || this.DataContext[this.objectfieldname] == "") {
                //console.log(this.objectfieldname + " is required");
                this.ctrl.setErrors({ "required": true });
            }

            if (hasminmax) {
                this.ctrl.validator = Validators.compose([Validators.required, Validators.minLength(minlength), Validators.maxLength(maxlenght)]);
            }
            else {
                this.ctrl.validator = Validators.required;
            }
        }
        else {
            this.ctrl.setErrors(null);
            //console.log(this.ctrl.errors);
            //console.log("i'm not required");
            if (hasminmax) {
                //console.log("i have min max");
                this.ctrl.validator = Validators.compose([Validators.minLength(minlength), Validators.maxLength(maxlenght)]);
            }
            else {
                this.ctrl.validator = null;
            }
        }
        //console.log(ctrl);

    }

    onFocus() {
        this.show = true;

    }

    onBlur() {
        this.show = false;
    }

}