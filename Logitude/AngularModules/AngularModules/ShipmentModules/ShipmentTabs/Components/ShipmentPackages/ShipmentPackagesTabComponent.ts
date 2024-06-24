import {  Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";

@Component({    
    templateUrl: './ShipmentPackagesTabComponent.html',
})

export class ShipmentPackagesTabComponent extends BaseComponent {
    constructor(public entityArgs: EntityArgs) {
        super();
    }
}
