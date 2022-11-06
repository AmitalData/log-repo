import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';

@Component({
    template: '<span><Hyperlink [Text]="Text" (click)="navigate()"></Hyperlink></span>'
})
export class DashboardListLinkRendererComponent implements ICellRendererAngularComp {
    Id: string;
    Text: string;
    params: any;

    agInit(params: any): void {
        this.params = params;
        this.getData(params);
    }

    getData(params: any) {
        let data = params.data;
        let dsiplayField = params.column.colId;

        this.Id = data["Id"];
        this.Text = data[dsiplayField]
    }

    refresh(params: any): boolean {
        return false;
    }

    navigate() {
        this.params.context.componentParent.OnDashboardListClick(`${this.Id}`)

    }
}
