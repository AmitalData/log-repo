import { Component } from "@angular/core";

@Component({
    template: `
    <div class="TextTrimming" *ngIf="fieldName == 'IsActive' && rowData.IsActive" style="text-align: center;  ">
        <img src="./Images/Icons/GreenV.png" style="height:20px;width:15px;vertical-align:central;padding-bottom:5px" />
    </div>

    <div class="TextTrimming" *ngIf="isDateTimeField" style="text-align: center;direction: ltr;">
        <span>{{rowData[this.fieldName] | DateTimePipe: 'DT'}}</span>
    </div>
    `,
}) export class ShaamTokenTemplate {
    public rowData: any = null;
    public fieldName: string = '';
    public isDateTimeField: boolean = false;

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.isDateTimeField = ['AccessExpireDate', 'RefreshExpierDate', 'CreateDate'].includes(fieldName);
    }
}
