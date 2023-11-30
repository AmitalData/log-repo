import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
   
    template: '<span *ngIf="!hasValue">{{params.value}}</span><span *ngIf="hasValue"> <Hyperlink  *ngFor="let shipment of shipmentList; let last = last" [Text]="shipment" (click)="navigate(shipment)"><span *ngIf="!hasOneShipment && !last">,</span></Hyperlink></span>'
})
export class EditShipmentLinkRendererComponent implements ICellRendererAngularComp {
    params: any;
    shipmentList: any  = [];
    hasOneShipment: boolean = true; 
    hasValue: boolean = true; 
    constructor() {

    }

    agInit(params: any): void {
        this.params = params;
        this.GetShipmentList(); 
        if (this.shipmentList.length > 1) this.hasOneShipment = false; 
    }

    private GetShipmentList() { 
        if (this.params && this.params.value) {
            this.CheckValue();
            this.shipmentList = this.params.value.split(",");
        }
    }

    private CheckValue() {
        if (this.params.value == "Not Specified") {
            this.hasValue = false;
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
