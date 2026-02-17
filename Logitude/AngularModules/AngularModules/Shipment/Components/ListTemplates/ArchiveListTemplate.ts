import {Component,ChangeDetectorRef} from '@angular/core';

@Component({
    
    template: '<div [id]="Id" style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color:blue;" [style.visibility]="stringvalue" >Archived</div>'
})

export class ArchiveListTemplate {

    public rowData: any;
    public fieldName: string;
    public value: boolean = false;
    public stringvalue: string = "hidden";
    public Id: string;
    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.Id = rowData["Id"];
        if (rowData[fieldName]) {
            var temp: boolean = rowData[fieldName];
            this.value = temp;
            if (this.value == true) {
                this.stringvalue = "visible";
            }
            else {
                this.stringvalue = "hidden";
            }
            //if (this.value == true) {
            //    var elem = document.getElementById(this.Id);
            //    if (elem) {
            //        elem.style.visibility = "visible";
            //    }
            //}
            //else {
            //    var elem = document.getElementById(this.Id);
            //    if (elem) {
            //        elem.style.visibility = "hidden";
            //    }
            //}  
        }
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        //else {

        //}
    }

}