import {Component, ChangeDetectorRef} from '@angular/core';
//import {NgStyle} from '@angular/common';
//import {StringToColorPipe} from '../../../Infrastructure/Pipes/StringToColorPipe';

@Component({
    template: ` 
                <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color: #FE6F0A;" [title]="CellValue">
                 {{CellValue}}
                </div>
                
              `,
    //directives: [NgStyle],
})

    /*<img *ngIf="isPotentialCustomer" style="vertical-align:middle;float:left; margin: auto;margin-top:-2px" [attr.src]="'./Images/PotentialCustomer.png'" [title]="'Potential'" />
     <img *ngIf="showWarning == true" style="width: 18px;height:18px;margin-bottom: 5px;" src="./Images/warning_notice.png"  [title]="Title" />
                    {{CellValue}}
    */
export class TaskCellDisplayListTemplate {

    public CellValue: string = null;
    public showWarning: boolean = false;
    public Title: string = "";
    constructor(private CD: ChangeDetectorRef) {
      
    }

    setVariables(rowData: any, fieldName: string) {
        this.CellValue = null;
        if (rowData != null) {
            if (rowData["IsRequestedDocuments"] == true || rowData["RequestedDocumentsCount"] > 0) {
                this.CellValue = (this.CellValue ? (this.CellValue + " \\ Requested Document") : "Requested Document");
            }
            if (rowData["IsImporterApprovalRequried"] == true) {
                this.CellValue = (this.CellValue ? (this.CellValue + " \\ Declaration Approval") : "Declaration Approval");
            }
            if (rowData["IsDigitalSignRequired"] == true) {
                this.CellValue = (this.CellValue ? (this.CellValue + " \\ Sign Required") : "Sign Required");
            }
            if (rowData["IsDepositionRequired"] == true) {
                this.CellValue = (this.CellValue ? (this.CellValue + " \\ Deposition Required") : "Deposition Required");
            }
           //IsDigitalSignRequired
           //IsDepositionRequired
           //IsRequestedDocuments
           //IsImporterApprovalRequried
        }
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }
}
