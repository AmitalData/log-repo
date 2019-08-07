"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
//import {NgStyle} from '@angular/common';
//import {StringToColorPipe} from '../../../Infrastructure/Pipes/StringToColorPipe';
var StatusCellDisplayListTemplate = /** @class */ (function () {
    function StatusCellDisplayListTemplate(CD) {
        this.CD = CD;
        this.CellValue = null;
        this.showWarning = false;
        this.Title = "";
    }
    StatusCellDisplayListTemplate.prototype.setVariables = function (rowData, fieldName) {
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
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    StatusCellDisplayListTemplate = __decorate([
        core_1.Component({
            template: " \n                <div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;\"[ngStyle] = \"{color: CellValue | StringToColorPipe}\" [title]=CellValue >\n                  <table>\n                <tr style=\"min-width:200px\">\n                <td style=\"max-width:0px\" >\n                    <div class=\"TextTrimming\">\n                        {{CellValue}}\n                    </div>\n                </td>\n                 <td style=\"min-width:10px;width:10px;max-width:50px\">\n                     <img *ngIf=\"showWarning == true\" style=\"width: 18px;height:18px;margin-bottom: 5px;float:left;\" src=\"./Images/warning_notice.png\"  [title]=\"Title\" />\n                 </td>\n                 </tr></table>\n                </div>\n                \n              ",
        })
        /*<img *ngIf="isPotentialCustomer" style="vertical-align:middle;float:left; margin: auto;margin-top:-2px" [attr.src]="'./Images/PotentialCustomer.png'" [title]="'Potential'" />
         <img *ngIf="showWarning == true" style="width: 18px;height:18px;margin-bottom: 5px;" src="./Images/warning_notice.png"  [title]="Title" />
                        {{CellValue}}
        */
        ,
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], StatusCellDisplayListTemplate);
    return StatusCellDisplayListTemplate;
}());
exports.StatusCellDisplayListTemplate = StatusCellDisplayListTemplate;
//# sourceMappingURL=StatusCellDisplayListTemplate.js.map