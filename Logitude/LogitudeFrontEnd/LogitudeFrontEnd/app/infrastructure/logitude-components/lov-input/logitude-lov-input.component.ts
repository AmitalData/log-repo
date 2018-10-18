
import {Directive, ElementRef, Renderer, Input, Output, Component, View, OnInit, OnChanges, Injector, provide} from 'angular2/core';
import {BaseComponent} from '../Base.Component';
import {UIProperty, UIProperties} from '../UIProperties';
import {
CORE_DIRECTIVES,
FORM_DIRECTIVES,
FormBuilder,
ControlGroup,
Validators,
AbstractControl, Control} from 'angular2/common';
import {Http, HTTP_PROVIDERS, ConnectionBackend, RequestOptions, RequestOptionsArgs} from 'angular2/http';
import {ServiceArgs} from '../../data-contracts/service-args';
import {EntityListService} from '../../services/entity-list.service';

@Component({
    selector: 'logitude-lov-input',
    template:
    `

       <div *ngIf="uiProperty.IsVisible" >

      <select [(ngModel)]="DataContext[objectfieldname]" [ngFormControl]="ctrl"  [disabled]="!uiProperty.IsEnabled" *ngIf="logitudeForm" (focus)="onFocus()" (blur)="onBlur()">
            
            <option *ngFor="#item of DataList" [value]="item.Id">{{item.Code}}</option>
        </select>
       
       </div>

    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES],
    inputs: ['objectfieldname', 'objecttablename', 'DataContext', 'lookuptablename'],
    providers: [HTTP_PROVIDERS, EntityListService, ServiceArgs]
})



export class LogitudeLovInputComponent implements OnInit {
    public objectfieldname: string;
    public objecttablename: string;
    public lookuptablename: string;
    public DataContext: any;
    public DataList: any[];
    private dataContext: BaseComponent;
    private uiProperty: UIProperty;
    private show: boolean;
    ctrl: Control;
    private _serviceType: any;
    @Input() logitudeForm: ControlGroup;



    constructor(public entityListService: EntityListService) {
        this.show = false;


    }
    ngOnInit() {

        var servicename = this.lookuptablename + "Service";
        var servicelink = './app/shipment/services/' + this.lookuptablename + '.service';
        var table = window.ObjectTables.filter(d => d.Name === this.objecttablename)[0];
        var field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.objectfieldname)[0];
                
              
        //console.log("this is the call to the entitylist service");
        this.entityListService.getAll(this.lookuptablename).then(res=>
        {
            //console.log("i'm the resopnse from lov ya kbeeeeeer", res); 
            res.subscribe(resp=> {
                this.DataList = resp; //console.log(resp);
        }));
        
        //console.log("this is the data context ha42345345345fdsahahahahahahahahahahahahahahahahah",this.DataContext);
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.objectfieldname, this.objecttablename);

        this.ctrl = new Control(this.DataContext[this.objectfieldname]);
        this.logitudeForm.addControl(this.objectfieldname, this.ctrl);
       
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