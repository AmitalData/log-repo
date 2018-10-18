import {Component, OnInit, OnChanges, Injectable, ComponentRef}  from 'angular2/core';
//import {Hero, HeroService}   from './hero.service';
import {RouteParams, Router} from 'angular2/router';
import {Http, HTTP_PROVIDERS} from 'angular2/http';
import {ObjectField, GeneralService} from './general.service';
import {
NgForm,
CORE_DIRECTIVES,
FORM_DIRECTIVES,
FormBuilder,
ControlGroup,
Validators,
Control} from 'angular2/common';
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {LogitudeLabelComponent} from '../../../infrastructure/logitude-components/label/logitude-label.component';
import {LogitudeTextInputComponent} from '../../../infrastructure/logitude-components/text-input/logitude-text-input.component';
import {LogitudeDateInputComponent} from '../../../infrastructure/logitude-components/date-input/logitude-date-input.component';
import {LogitudeCheckboxInputComponent} from '../../../infrastructure/logitude-components/checkbox-input/logitude-checkbox-input.component';
import {LogitudeLovInputComponent} from '../../../infrastructure/logitude-components/lov-input/logitude-lov-input.component';

import {BaseComponent} from '../../../infrastructure/logitude-components/Base.Component'
import {UIProperty, UIProperties}  from '../../../infrastructure/logitude-components/UIProperties'
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {EntityArgs} from '../../../infrastructure/data-contracts/entity-args';
import {EditControlComponent} from '../../../infrastructure/edit-control/edit-control.component';

@Component({
    templateUrl:'./app/shipment/shipment-tabs/general/general.component.html',
    providers: [HTTP_PROVIDERS, GeneralService],
    viewProviders: [HTTP_PROVIDERS],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, LogitudeLabelComponent, LogitudeTextInputComponent, LogitudeDateInputComponent, LogitudeCheckboxInputComponent, LogitudeLovInputComponent],
    pipes: [TextcodeTranslationPipe]
})
@Injectable()
export class GeneralComponent extends BaseComponent implements OnInit {

    public objectFields: Array<ObjectField>;
    public notesObjectField: ObjectField;
    public Notes: string;
    // public EntityPM: any;
    public ObjectTableName: string;
    //private _selectedId: string;
    public GeneratedObjectFields: Array<ObjectField>;
    public context = this;

   
    get ConsigneeName() {
        //console.log("i'm in getter of consignee  " + this.EntityPM.ConsigneeName);
        return this.EntityPM.ConsigneeName;
    }

    set ConsigneeName(newValue: string) {
        //console.log("i'm in setter of consignee  " + newValue);
        this.EntityPM.ConsigneeName = newValue;
    }

    get ShipperName() {
        
        return this.EntityPM.ShipperName;
    }

    set ShipperName(newValue: string) {
       
        this.EntityPM.ShipperName = newValue;
    }
   public myForm: ControlGroup;

    constructor(
        private _service: GeneralService,
        private _http: Http,
        private _shipmentsService: ShipmentsService,
        public entityArgs: EntityArgs, 
        fb: FormBuilder,
        private _componentRef: ComponentRef
    ) {
        super();
        //console.log("entity args: ", this.entityArgs);
        this.myForm = fb.group({
            //'ShipperName': ['', Validators.required]
           
        });
        //EditControlComponent.ActiveViewTab = this._componentRef;
    }

    ngOnInit() {
        this.notesObjectField = window.ObjectFields.filter(d => d.Id === "1-80")[0];
        //this._shipmentsService.getShipment(this._selectedId).then(res => { this.EntityPM = res; console.log(res); });
        /*this.EntityPM = */
        //this._shipmentsService.getSingleEntityPM(this._selectedId, 1).subscribe((res: ShipmentPM) => { this.EntityPM = res; console.log(res as ShipmentPM); }); //.then(res => { this.EntityPM = res; console.log(res); });
        //this._shipmentsService.getSingleShipmentById(this._selectedId, 1).subscribe(res => { this.EntityPM = res; console.log(res); });
        //console.log(this.EntityPM);

        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.myForm.valueChanges.subscribe(
            (value: string) => {
                //console.log('form changed to: ', value);
                //console.log(this.EntityPM);
                //console.log(this.EntityPM.ShipmentPackages);
            }
        );
        this.GeneratedObjectFields = this.getGeneralObjectFields();
    }

    getGeneralObjectFields() {
        var screenFields: any[] = window.ScreenFields.filter(x => x.ScreenId === "1-1");
        var objectFields = [];
        screenFields.forEach((screenField) => {
            var objectfield = window.ObjectFields.filter(x => x.Id === screenField.ObjectFieldId)[0];
            objectFields.push(objectfield);
        });
        return objectFields;
    }

    printObjectField(objectfield) {
       // console.log(objectfield);
        this.notesObjectField = objectfield;
    }

    logObjectFields(objectfields) {
       // console.log(objectfields.json());
        var jsoned = objectfields.json();
        var filtered = jsoned.filter(d => d.ObjectTableId === "1-4");
        var filtered = jsoned.filter(d => d.Id === "1-80")[0];
        this.notesObjectField = filtered;
       // console.log(filtered);
    }
       
    public onClick() {

        this.UIProperties.SetVisibility("ShipperName", "Shipment", false);
        //console.log("on click");
       // this.UIProperties.SetValidity("ConsigneeName", "Shipment", false, "i'm the best error message ever!!!!");
        //this.ConsigneeName = "hahahah";
        //console.log("on click");
        //let shipperCtrl = this.myForm.controls['ConsigneeName'];
        //for (var controll in this.myForm.controls)
        //{
        //    console.log(controll.errors);
        //}
        
        //shipperCtrl.setErrors({ "erre1": "my custom error" });
        //console.log("error is:   ", shipperCtrl);

       // console.log("error is:   ", shipperCtrl.getError("minlength"));
        //console.log("shipper control: ",shipperCtrl);
    }

    sendTrue() {
       // this.UIProperties.SetVisibility("ShipperName", "Shipment", true);
        this.EntityPM.UIProperties.SetVisibility("ShipperName", "Shipment", true);
        this.UIProperties.SetRequired("ConsigneeName", "Shipment", true);

    }
    sendFalse() {
      //  this.UIProperties.SetVisibility("ShipperName", "Shipment", false);
        this.EntityPM.UIProperties.SetVisibility("ShipperName", "Shipment", false);
        this.UIProperties.SetRequired("ConsigneeName", "Shipment", false);
        
    }
    onSubmit(value: any): void {
        //console.log(value);
        
    }



}
