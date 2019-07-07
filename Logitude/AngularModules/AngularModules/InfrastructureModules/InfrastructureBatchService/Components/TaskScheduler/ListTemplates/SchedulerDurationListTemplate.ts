import {Component,ChangeDetectorRef} from '@angular/core';

@Component({

    template: `
                <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color:green;">{{dateValue}}</div>
            `, 
})

export class SchedulerDurationListTemplate {

    public rowData: any;
    public fieldName: any;
    public dateValue: any;

    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        var startDate = new Date(rowData["StartDateTime"]);
        var endDate = new Date(rowData["EndDateTime"]);

        var seconds = ((endDate.getTime() - startDate.getTime()) / 1000);
        if (startDate.getFullYear() > 1970 && endDate.getFullYear() > 1970) {
            this.dateValue = seconds;
        }
        
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

}
