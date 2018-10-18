import {Component, OnInit} from 'angular2/core';
import {EntityArgs} from '../../../data-contracts/entity-args';
import {ShipmentPM} from '../../../../shipment/EntityPMs/ShipmentPM';

@Component({
    //selector: 'header-screen-value',
    template: '<span class="HeaderScreenValue" *ngIf="entityPM && entityArgs">{{entityPM[fieldName]}}</span>',
    inputs: ['htmlHeaderComponent', 'fieldName', 'entityPM', 'entityArgs']
})

export class DefaultHeaderScreenValueComponent implements OnInit {

    public entityPM: any;
    public fieldName: any;
    public htmlHeaderComponent: string;
    public entityArgs: EntityArgs;

    constructor() {

    }

    ngOnInit() {
        console.log("HS CMP: ", this.entityPM, this.fieldName, this.htmlHeaderComponent);
        //this.entityPM = this.entityArgs.EntityPM;
        console.log(this.entityArgs);
        if (this.htmlHeaderComponent) {
            console.log("htmlHeaderComponent is -----> ", this.htmlHeaderComponent);
        }
    }

}