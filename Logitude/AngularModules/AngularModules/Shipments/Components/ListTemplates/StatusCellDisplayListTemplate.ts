import {Component, ChangeDetectorRef} from '@angular/core';
//import {NgStyle} from '@angular/common';
//import {StringToColorPipe} from '../../../Infrastructure/Pipes/StringToColorPipe';

@Component({
    template: ` 
                <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;"[ngStyle] = "{color: CellValue | StringToColorPipe}" [title]=CellValue >
                  <table>
                <tr style="min-width:200px">
                <td style="max-width:0px" >
                    <div class="TextTrimming">
                        {{CellValue}}
                    </div>
                </td>
                 <td style="min-width:10px;width:10px;max-width:50px">
                     <img *ngIf="showWarning == true" style="width: 18px;height:18px;margin-bottom: 5px;float:left;" src="./Images/warning_notice.png"  [title]="Title" />
                 </td>
                 </tr></table>
                </div>
                
              `,
    //directives: [NgStyle],
})

    /*<img *ngIf="isPotentialCustomer" style="vertical-align:middle;float:left; margin: auto;margin-top:-2px" [attr.src]="'./Images/PotentialCustomer.png'" [title]="'Potential'" />
     <img *ngIf="showWarning == true" style="width: 18px;height:18px;margin-bottom: 5px;" src="./Images/warning_notice.png"  [title]="Title" />
                    {{CellValue}}
    */
export class StatusCellDisplayListTemplate {

    public CellValue: string = null;
    public showWarning: boolean = false;
    public Title: string = "";
    constructor(private CD: ChangeDetectorRef) {
      
    }

    setVariables(rowData: any, fieldName: string) {
        if (rowData != null) {
            this.CellValue = rowData[fieldName];
            var today = new Date(rowData["ExceptionDate"]);
            var d = today.getDate();
            var m = today.getMonth() + 1; //January is 0!
            var H = today.getHours();
            var mu = today.getMinutes(); 
            var dd = "";
            var mm = "";
            var HH = "";
            var muu = "";
            var yyyy = today.getFullYear().toString();
            if (d < 10) {
                dd = '0' + d;
            }
            else {
                dd = d.toString();
            }
            if (m < 10) {
                mm = '0' + m;
            }
            else {
                mm = m.toString();
            }
            if (H < 10) {
                HH = '0' + H;
            }
            else {
                HH = H.toString();
            }
            if (mu < 10) {
                muu = '0' + mu;
            }
            else {
                muu = d.toString();
            }
            var to = dd + '/' + mm + '/' + yyyy + " " + HH + ":" + muu;
            this.Title = to + "\n" + rowData["ExceptionDescription"];
            this.showWarning = rowData["HasException"];
        }
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }
}