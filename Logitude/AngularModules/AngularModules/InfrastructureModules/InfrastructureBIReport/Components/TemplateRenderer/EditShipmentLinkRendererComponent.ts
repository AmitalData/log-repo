import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    template: '<Hyperlink [Text]="params.value" (click)="navigate(params.value)"></Hyperlink>'
})
export class EditShipmentLinkRendererComponent implements ICellRendererAngularComp {
    params: any;

    constructor() {

    }

    agInit(params: any): void {
        this.params = params;
    }

    refresh(params: any): boolean {
        return false;
    }

    // This was needed to make the link work correctly
    navigate(link) {
        this.params.context.componentParent.methodFromParent(`${this.params.value}`)
       
    }
}
