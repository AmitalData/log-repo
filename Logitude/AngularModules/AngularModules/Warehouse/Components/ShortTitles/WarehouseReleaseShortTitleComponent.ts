import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {WarehouseReleasePM} from '../../EntityPMs/WarehouseReleasePM';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./WarehouseReleaseShortTitleComponent.html",
})

export class WarehouseReleaseShortTitleComponent {
    public EntityPM: WarehouseReleasePM;
    public DirectionImageSRC: string;
    public Background: string;
    public TransportModeImageSRC: string;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            this.DirectionImageSRC = "./Images/Directions/" + this.EntityPM.DirectionId + ".png";
            this.TransportModeImageSRC = "./Images/Icons/" + this.EntityPM.TransportModeId + ".png";

            if (this.EntityPM.ShipmentLevelCode == "C") {
                this.Background = "rgba(35, 172, 214, 0.15)";
            }

            else {
                this.Background = "rgba(235, 235, 235, 1)";
            }


        }
    }


}