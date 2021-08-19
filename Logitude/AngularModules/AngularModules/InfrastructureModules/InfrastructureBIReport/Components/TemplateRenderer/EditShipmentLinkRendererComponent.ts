import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
   
    template: '<Hyperlink  *ngFor="let shipment of shipmentList; let last = last" [Text]="shipment" (click)="navigate(shipment)"><span *ngIf="!hasOneShipment && !last">,</span></Hyperlink>'
})
export class EditShipmentLinkRendererComponent implements ICellRendererAngularComp {
    params: any;
    shipmentList: any  = [];
    hasOneShipment: boolean = true; 

    constructor() {

    }

    agInit(params: any): void {
        this.params = params;
        this.GetShipmentList(); 
        if (this.shipmentList.length > 1) this.hasOneShipment = false; 
    }

    private GetShipmentList() {
        if (this.params && this.params.value) {
            this.shipmentList = this.params.value.split(",");
        }
    }

    refresh(params: any): boolean {
        return false;
    }

    // This was needed to make the link work correctly
    navigate(shipmentId) { 
        shipmentId = shipmentId.trim();
        this.params.context.componentParent.methodFromParent(`${shipmentId}`)  
    }
}
