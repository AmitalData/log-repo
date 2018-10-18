import {Component,ChangeDetectorRef} from '@angular/core';

@Component({

    template: `
                <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color:green;">{{dateValue | LogBoxStatusDatePipe}}</div>
            `, 
})

export class DateCellDisplayListTemplate {

    public rowData: any;
    public fieldName: any;
    public dateValue: any;

    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        var pmDate = new Date(rowData[fieldName]);
        this.dateValue = pmDate;
        //console.log("pmDate", pmDate);
        //var nowDate = new Date();
        ////console.log("nowDate", nowDate);
        ////var valueDate = pmDate.setDate(nowDate.getDate());
        ////console.log("valueDate = pmDate.getDate", valueDate);
        //if (pmDate.getDate() == nowDate.getDate() && pmDate.getMonth() == nowDate.getMonth() && pmDate.getFullYear() == nowDate.getFullYear()) {
        //    //return "Today";
        //    this.dateValue = "Today";
        //}
        //else if (pmDate.getDate() == (nowDate.getDate() - 1) && pmDate.getMonth() == nowDate.getMonth() && pmDate.getFullYear() == nowDate.getFullYear()) {
        //    //return "Yesterday";
        //    this.dateValue = "Yesterday";
        //}
        //else if (pmDate.getDate() == (nowDate.getDate() + 1) && pmDate.getMonth() == nowDate.getMonth() && pmDate.getFullYear() == nowDate.getFullYear()) {
        //    //return "Tomorrow";
        //    this.dateValue = "Tomorrow";
        //}
        //else {
        //    this.dateValue = pmDate.getDate() + "/" + pmDate.getMonth() + "/" + pmDate.getFullYear();
        //}
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

}