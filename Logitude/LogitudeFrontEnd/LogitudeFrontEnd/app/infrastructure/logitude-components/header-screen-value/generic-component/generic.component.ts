import {Component, OnInit} from 'angular2/core';
import {EntityArgs} from '../../../data-contracts/entity-args';
import {ShipmentPM} from '../../../../shipment/EntityPMs/ShipmentPM';

@Component({
    template: '{{entityPM[fieldName]}}',
})

export class GenericHeaderScreenValueComponent implements OnInit {

    public entityPM: any;
    public fieldName: any;

    constructor() {
    }

    ngOnInit() {
    }

    setVariables(entityPM: ShipmentPM, fieldName: string) {
        //console.log(entityPM, fieldName);
        this.entityPM = entityPM;
        this.fieldName = fieldName;
    }

}