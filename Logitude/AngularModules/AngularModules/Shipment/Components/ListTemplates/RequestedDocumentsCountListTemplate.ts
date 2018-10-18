import {Component,ChangeDetectorRef} from '@angular/core';

@Component({
    
    template: '<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;text-align: center" >{{value}}</div>'
})

export class RequestedDocumentsCountListTemplate {

    public rowData: any;
    public fieldName: string;
    public value: number;

    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (rowData[fieldName]) {
            var temp: number = rowData[fieldName];
            this.value = temp;
            var isDestroyed: boolean = this.CD['destroyed'];
            if (!isDestroyed) {
                this.CD.detectChanges();
            }
        }
        //else {

        //}
    }

}