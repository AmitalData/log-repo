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
var SessionLocator_1 = require("../Infrastructure/Utilities/SessionLocator");
var ActivitiesFilter = /** @class */ (function () {
    function ActivitiesFilter() {
        this.SelectedValue = "All";
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (this.CurrentSession == null) {
            this.ActivityFilter_CL = "ActivityFilter_CL_-1_-1";
            this.ActivityFilter_TS = "ActivityFilter_TS_-1_-1";
            this.ActivityFilter_AP = "ActivityFilter_AP_-1_-1";
            this.ActivityFilter_EO = "ActivityFilter_EO_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("ActivityFilter");
            this.ActivityFilter_CL = "ActivityFilter_CL_" + idIndex;
            this.ActivityFilter_TS = "ActivityFilter_TS_" + idIndex;
            this.ActivityFilter_AP = "ActivityFilter_AP_" + idIndex;
            this.ActivityFilter_EO = "ActivityFilter_EO_" + idIndex;
        }
    }
    ActivitiesFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
            var img_CL = document.getElementById(this.ActivityFilter_CL);
            var img_TS = document.getElementById(this.ActivityFilter_TS);
            var img_AP = document.getElementById(this.ActivityFilter_AP);
            var img_EO = document.getElementById(this.ActivityFilter_EO);
            img_CL.setAttribute("src", "./Images/Activities/CL_g.png");
            img_TS.setAttribute("src", "./Images/Activities/TS_g.png");
            img_AP.setAttribute("src", "./Images/Activities/AP_g.png");
            img_EO.setAttribute("src", "./Images/Activities/EO_g.png");
            switch (itemValue) {
                case "CL": {
                    img_CL.setAttribute("src", "./Images/Activities/CL_w.png");
                    break;
                }
                case "TS": {
                    img_TS.setAttribute("src", "./Images/Activities/TS_w.png");
                    break;
                }
                case "AP": {
                    img_AP.setAttribute("src", "./Images/Activities/AP_w.png");
                    break;
                }
                case "EO": {
                    img_EO.setAttribute("src", "./Images/Activities/EO_w.png");
                    break;
                }
            }
        }
    };
    ActivitiesFilter.prototype.itemMouseOver = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_CL = document.getElementById(this.ActivityFilter_CL);
            var img_TS = document.getElementById(this.ActivityFilter_TS);
            var img_AP = document.getElementById(this.ActivityFilter_AP);
            var img_EO = document.getElementById(this.ActivityFilter_EO);
            switch (itemValue) {
                case "CL": {
                    img_CL.setAttribute("src", "./Images/Activities/CL.png");
                    break;
                }
                case "VM": {
                    img_TS.setAttribute("src", "./Images/Activities/VM.png");
                    break;
                }
                case "TS": {
                    img_TS.setAttribute("src", "./Images/Activities/TS.png");
                    break;
                }
                case "AP": {
                    img_AP.setAttribute("src", "./Images/Activities/AP.png");
                    break;
                }
                case "EO": {
                    img_EO.setAttribute("src", "./Images/Activities/EO.png");
                    break;
                }
                case "EI": {
                    img_EO.setAttribute("src", "./Images/Activities/EI.png");
                    break;
                }
            }
        }
    };
    ActivitiesFilter.prototype.itemMouseLeave = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_CL = document.getElementById(this.ActivityFilter_CL);
            var img_TS = document.getElementById(this.ActivityFilter_TS);
            var img_AP = document.getElementById(this.ActivityFilter_AP);
            var img_EO = document.getElementById(this.ActivityFilter_EO);
            switch (itemValue) {
                case "CL": {
                    img_CL.setAttribute("src", "./Images/Activities/CL_g.png");
                    break;
                }
                case "TS": {
                    img_TS.setAttribute("src", "./Images/Activities/TS_g.png");
                    break;
                }
                case "AP": {
                    img_AP.setAttribute("src", "./Images/Activities/AP_g.png");
                    break;
                }
                case "EO": {
                    img_EO.setAttribute("src", "./Images/Activities/EO_g.png");
                    break;
                }
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ActivitiesFilter.prototype, "SelectedValueChanged", void 0);
    ActivitiesFilter = __decorate([
        core_1.Component({
            selector: 'ActivitiesFilter',
            inputs: ['SelectedValue'],
            template: "\n    <ul class=\"FiltersMenu\">\n        <li (click)=\"itemClicked('All')\" (mouseover)=\"itemMouseOver('All')\" [class.SelectedFilter]=\"SelectedValue === 'All'\">\n            All\n        </li>\n        <li (click)=\"itemClicked('CL')\" (mouseover)=\"itemMouseOver('CL')\" (mouseleave)=\"itemMouseLeave('CL')\" [class.SelectedFilter]=\"SelectedValue === 'CL'\" title=\"Phone Call\">\n            <img [id]=\"ActivityFilter_CL\" class=\"CenterCenter\" src=\"./Images/Activities/CL_g.png\" style=\"top: 1px;\" />\n        </li>\n        <li (click)=\"itemClicked('TS')\" (mouseover)=\"itemMouseOver('TS')\" (mouseleave)=\"itemMouseLeave('TS')\" [class.SelectedFilter]=\"SelectedValue === 'TS'\" title=\"Task\">\n            <img [id]=\"ActivityFilter_TS\" class=\"CenterCenter\" src=\"./Images/Activities/TS_g.png\" style=\"top: 1px;\" />\n        </li>\n        <li (click)=\"itemClicked('AP')\" (mouseover)=\"itemMouseOver('AP')\" (mouseleave)=\"itemMouseLeave('AP')\" [class.SelectedFilter]=\"SelectedValue === 'AP'\" title=\"Appointment\">\n            <img [id]=\"ActivityFilter_AP\" class=\"CenterCenter\" src=\"./Images/Activities/AP_g.png\" style=\"top: 1px;\" />\n        </li>\n        <li (click)=\"itemClicked('EO')\" (mouseover)=\"itemMouseOver('EO')\" (mouseleave)=\"itemMouseLeave('EO')\" [class.SelectedFilter]=\"SelectedValue === 'EO'\" title=\"Email Out\">\n            <img [id]=\"ActivityFilter_EO\" class=\"CenterCenter\" src=\"./Images/Activities/EO_g.png\" style=\"top: 1px;\" />\n        </li>\n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], ActivitiesFilter);
    return ActivitiesFilter;
}());
exports.ActivitiesFilter = ActivitiesFilter;
//# sourceMappingURL=ActivitiesFilter.js.map