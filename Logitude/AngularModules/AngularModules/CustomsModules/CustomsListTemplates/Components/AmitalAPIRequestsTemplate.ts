import { Component } from "@angular/core";

@Component({
    template: `
    <div class="TextTrimming" *ngIf="fieldName == 'HasError'">        
        <img [src]="'./Images/Icons/' + (rowData[this.fieldName] ? 'RedX.png' : 'GreenV.png')" />
    </div>

    <div class="TextTrimming" *ngIf="fieldName == 'CreateDate'">
        <span>{{rowData[this.fieldName] | DateTimePipe: 'DT'}}</span>
    </div>
    `,
    styles: [`
        .TextTrimming {
            text-align: center;
            direction: ltr;
        }

        .TextTrimming img {
            height: 20px;
            width: 15px;
            vertical-align: central;
            padding-bottom: 5px;
        }
    `],
}) export class AmitalAPIRequestsTemplate {
    public rowData: any = null;
    public fieldName: string = '';

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
    }
}
