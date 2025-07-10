import {Component} from '@angular/core';

@Component({
    
    template: '<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;" *ngIf="value">{{value}}</div>'
})

export class RoutingCellDisplayListTemplate {

    public rowData: any;
    public fieldName: string;
    public value: string;

    constructor() {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (rowData[fieldName]) {
            var routing: string = rowData[fieldName];
            this.value = routing.replace(",", ">");
        }
        //else {

        //}
    }

}