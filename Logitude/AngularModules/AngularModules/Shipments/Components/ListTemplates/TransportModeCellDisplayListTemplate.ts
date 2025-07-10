import {Component} from '@angular/core';

@Component({

    template: `<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;">
                <img width="15" height="15" style="vertical-align: middle;margin-left: -7px;;" src="./Images/TransportModes/{{rowData[fieldName]}}.png" />
            </div>
            `
})

export class TransportModeCellDisplayListTemplate {

    public rowData: any;
    public fieldName: any;

    constructor() {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
    }

}