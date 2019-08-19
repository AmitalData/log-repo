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
var ArchiveListTemplate = /** @class */ (function () {
    function ArchiveListTemplate(CD) {
        this.CD = CD;
        this.value = false;
        this.stringvalue = "hidden";
    }
    ArchiveListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.Id = rowData["Id"];
        if (rowData[fieldName]) {
            var temp = rowData[fieldName];
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
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        //else {
        //}
    };
    ArchiveListTemplate = __decorate([
        core_1.Component({
            template: '<div [id]="Id" style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color:blue;" [style.visibility]="stringvalue" >Archived</div>'
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ArchiveListTemplate);
    return ArchiveListTemplate;
}());
exports.ArchiveListTemplate = ArchiveListTemplate;
//# sourceMappingURL=ArchiveListTemplate.js.map