import {Component, DynamicComponentLoader, ElementRef, AfterViewInit} from 'angular2/core';
import {EntityArgs} from '../../data-contracts/entity-args';
import {ShipmentPM} from '../../../shipment/EntityPMs/ShipmentPM';

@Component({
    selector: 'header-screen-value',
    templateUrl: './app/infrastructure/logitude-components/header-screen-value/header-screen-value.component.html',
    inputs: ['htmlHeaderComponentUrl', 'htmlHeaderComponentName', 'fieldName', 'entityPM', 'entityArgs']
})

export class HeaderScreenValueComponent implements AfterViewInit {

    public entityPM: any;
    public fieldName: any;
    public htmlHeaderComponentUrl: string;
    public htmlHeaderComponentName: string;
    public entityArgs: EntityArgs;

    constructor(private _dynamicCmpLoader: DynamicComponentLoader, private _elementRef: ElementRef) {
        
    }

    ngAfterViewInit() {
        // viewChild is updated after the view has been initialized
        //console.log('ngAfterViewInit: ');

        if (this.htmlHeaderComponentName && this.htmlHeaderComponentUrl) {
            //console.log("htmlHeaderComponent is -----> ", this.htmlHeaderComponent);
            System.import(this.htmlHeaderComponentUrl)
                .then(m => {
                    if (m) {
                        //console.log("i'm inside the import for short title");
                        var screenCmp = m[this.htmlHeaderComponentName];
                        //console.log("i'm the current entity args ", this.entityArgs.EntityPM);
                        this._dynamicCmpLoader.loadNextToLocation(screenCmp, this._elementRef)
                            .then((res) => {
                                //console.log("specific response: ", res);
                                res.instance.setVariables(this.entityPM, this.fieldName);
                            });
                    }
                })
        }
        else {
            System.import('./app/infrastructure/logitude-components/header-screen-value/generic-component/generic.component')
                .then(m => {
                    //console.log("i'm inside the import for short title");
                    var screenCmp = m["GenericHeaderScreenValueComponent"];
                    this._dynamicCmpLoader.loadNextToLocation(screenCmp, this._elementRef)
                        .then((res) => {
                            //console.log("generic response: ", res);
                            res.instance.setVariables(this.entityPM, this.fieldName);
                        });
                })
            //this.entity = this.entityPM;
            //this.field = this.fieldName;
        }

    }

    public entity: ShipmentPM;
    public field: string;
    
}