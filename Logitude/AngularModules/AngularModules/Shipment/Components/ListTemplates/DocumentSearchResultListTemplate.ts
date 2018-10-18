import {Component} from '@angular/core';

@Component({

    template: `<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;">
                <img style="vertical-align: middle;" *ngIf="ResultFromDocument == true" src="./Images/SearchResult.png" />
               </div> 
            `
})

export class DocumentSearchResultListTemplate {

    public rowData: any;
    public fieldName: string;
    DocumentsSearchFields: string = "";
    ResultFromDocument: boolean = false; 
    constructor() {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        //list.ResultFromDocument = list.DocumentsSearchFields.ToUpper().Contains(SearchFilter.Trim().ToUpper());
        this.DocumentsSearchFields = rowData["DocumentsSearchFields"];
        if (this.fieldName != "" && this.fieldName != null && this.DocumentsSearchFields) {
            this.ResultFromDocument = this.DocumentsSearchFields.toLowerCase().indexOf(this.fieldName.toLowerCase()) > -1 ? true : false;
        }
        else {
            this.ResultFromDocument = false;
        }
       
         
    }

}