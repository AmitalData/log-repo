import {Component} from 'angular2/core';
import {EntityArgs} from '../../data-contracts/entity-args';
import {ShipmentPM} from '../../../shipment/EntityPMs/ShipmentPM';

@Component({
    //selector: '<is-operational-closed></is-operational-closed>',
    template: '<span>Op Closed Tmpl</span>'
})

export class IsOperationalClosedHeaderTemplate {

    public entityPM: any;
    public fieldName: any;

    constructor() {
    }

    setVariables(entityPM: ShipmentPM, fieldName: string) {
        //console.log(entityPM, fieldName);
        this.entityPM = entityPM;
        this.fieldName = fieldName;
    }

}