import {Component} from '@angular/core';

@Component({

    template: `<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;">
                <img width="15" height="15" style="vertical-align: middle;margin-left: -7px;" src="Images/Directions/{{rowData[fieldName]}}.png" />
               </div>
                <!--<img *ngIf="rowData[fieldName] == 'D'" width="20" height="20" src="./Images/D.png" />
                <img *ngIf="rowData[fieldName] == 'E'" width="20" height="20" src="./Images/E.png" />
                <img *ngIf="rowData[fieldName] == 'I'" width="20" height="20" src="./Images/I.png" />
                <img *ngIf="rowData[fieldName] == 'R'" width="20" height="20" src="./Images/R.png" />-->
            `
})

export class DirectionCellDisplayListTemplate {

    public rowData: any;
    public fieldName: any;

    constructor() {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
    }

}