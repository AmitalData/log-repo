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
var TaskCellDisplayListTemplate = /** @class */ (function () {
    function TaskCellDisplayListTemplate(CD) {
        this.CD = CD;
        this.CellValue = null;
        this.showWarning = false;
        this.Title = "";
    }
    TaskCellDisplayListTemplate.prototype.setVariables = function (rowData, fieldName) {
        if (rowData != null) {
            if (rowData["IsRequestedDocuments"] == true) {
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
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    TaskCellDisplayListTemplate = __decorate([
        core_1.Component({
            template: " \n                <div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color: #FE6F0A;\" [title]=\"CellValue\">\n                 {{CellValue}}\n                </div>\n                \n              ",
        })
        /*<img *ngIf="isPotentialCustomer" style="vertical-align:middle;float:left; margin: auto;margin-top:-2px" [attr.src]="'./Images/PotentialCustomer.png'" [title]="'Potential'" />
         <img *ngIf="showWarning == true" style="width: 18px;height:18px;margin-bottom: 5px;" src="./Images/warning_notice.png"  [title]="Title" />
                        {{CellValue}}
        */
        ,
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], TaskCellDisplayListTemplate);
    return TaskCellDisplayListTemplate;
}());
exports.TaskCellDisplayListTemplate = TaskCellDisplayListTemplate;
//# sourceMappingURL=TaskCellDisplayListTemplate.js.map