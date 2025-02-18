import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';

@Component({
    template:
    `
    <span style="width: 100%; height:100%; position: relative;">
        <img [attr.src]="ImageSRC" style="top: -5px;" />
    </span>
    `
})

export class IsOperationalClosedHeaderTemplate {

    public entityPM: ShipmentPM;
    public fieldName: any;
    public ImageSRC: string;
    constructor() {
    }

    setVariables(entityPM: ShipmentPM, fieldName: string) {
        this.entityPM = entityPM;
        this.fieldName = fieldName;

        if (entityPM.IsOperationalClosed == true) {
            this.ImageSRC = "./Images/Icons/IsClosed.png";
        }

        else {
            this.ImageSRC = "./Images/Icons/IsOpened.png";
        } 
    }

}