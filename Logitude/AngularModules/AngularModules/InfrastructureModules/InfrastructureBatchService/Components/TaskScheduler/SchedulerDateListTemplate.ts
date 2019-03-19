import {Component,ChangeDetectorRef} from '@angular/core';

@Component({

    template: `
                <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color:green;">{{dateValue | DateTimePipe:'DTL12'}}</div>
            `, 
})

export class SchedulerDateListTemplate {

    public rowData: any;
    public fieldName: any;
    public dateValue: any;

    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        var pmDate = new Date(rowData[fieldName]);
        if (pmDate.getFullYear() > 1970) {
            this.dateValue = pmDate;
        }
        
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

}
