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
var FeatureLocator_1 = require("../Infrastructure/Utilities/FeatureLocator");
var DirectionsFilter = /** @class */ (function () {
    function DirectionsFilter() {
        this.DirectionWidth = 140;
        this.itmImportShipments = false;
        this.itmImportDomistic = false;
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedValue = "All";
        this.hideCustomsImport = false;
        this.hideImportDomistic = false;
        if (this.CurrentSession == null) {
            this.FilterId_E = "DirectionsFilter_E_-1_-1";
            this.FilterId_I = "DirectionsFilter_I_-1_-1";
            this.FilterId_R = "DirectionsFilter_R_-1_-1";
            this.FilterId_D = "DirectionsFilter_D_-1_-1";
            this.FilterId_C = "DirectionsFilter_C_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("DirectionsFilter");
            this.FilterId_E = "DirectionsFilter_E_" + idIndex;
            this.FilterId_I = "DirectionsFilter_I_" + idIndex;
            this.FilterId_R = "DirectionsFilter_R_" + idIndex;
            this.FilterId_D = "DirectionsFilter_D_" + idIndex;
            this.FilterId_C = "DirectionsFilter_C_" + idIndex;
        }
        this.SetVisibilityImportShipments();
        this.SetVisibilityImportDomistic();
    }
    Object.defineProperty(DirectionsFilter.prototype, "SelectedValue", {
        get: function () { return this.selectedValue; },
        set: function (value) {
            if (this.selectedValue != value) {
                this.selectedValue = value;
                this.ApplySelectedStyle();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DirectionsFilter.prototype, "HideCustomsImport", {
        get: function () { return this.hideCustomsImport; },
        set: function (value) {
            if (this.hideCustomsImport != value) {
                this.hideCustomsImport = value;
                this.SetVisibilityImportShipments();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DirectionsFilter.prototype, "HideImportDomistic", {
        get: function () { return this.hideImportDomistic; },
        set: function (value) {
            if (this.hideImportDomistic != value) {
                this.hideImportDomistic = value;
                this.SetVisibilityImportDomistic();
            }
        },
        enumerable: true,
        configurable: true
    });
    DirectionsFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    };
    DirectionsFilter.prototype.itemMouseOver = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_E = document.getElementById(this.FilterId_E);
            if (!this.itmImportDomistic)
                var img_I = document.getElementById(this.FilterId_I);
            var img_R = document.getElementById(this.FilterId_R);
            if (!this.itmImportDomistic)
                var img_D = document.getElementById(this.FilterId_D);
            if (this.itmImportShipments)
                var img_C = document.getElementById(this.FilterId_C);
            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E.png");
                    break;
                }
                case "I": {
                    if (!this.itmImportDomistic)
                        img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I.png");
                    break;
                }
                case "R": {
                    img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R.png");
                    break;
                }
                case "D": {
                    if (!this.itmImportDomistic)
                        img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D.png");
                    break;
                }
                case "C": {
                    if (this.itmImportShipments)
                        img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C.png");
                    break;
                }
            }
        }
    };
    DirectionsFilter.prototype.itemMouseLeave = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_E = document.getElementById(this.FilterId_E);
            if (!this.itmImportDomistic)
                var img_I = document.getElementById(this.FilterId_I);
            var img_R = document.getElementById(this.FilterId_R);
            if (!this.itmImportDomistic)
                var img_D = document.getElementById(this.FilterId_D);
            if (this.itmImportShipments)
                var img_C = document.getElementById(this.FilterId_C);
            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E_g.png");
                    break;
                }
                case "I": {
                    if (!this.itmImportDomistic)
                        img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I_g.png");
                    break;
                }
                case "R": {
                    img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R_g.png");
                    break;
                }
                case "D": {
                    if (!this.itmImportDomistic)
                        img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D_g.png");
                    break;
                }
                case "C": {
                    if (this.itmImportShipments)
                        img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C_g.png");
                    break;
                }
            }
        }
    };
    DirectionsFilter.prototype.ApplySelectedStyle = function () {
        var img_E = document.getElementById(this.FilterId_E);
        if (!this.itmImportDomistic)
            var img_I = document.getElementById(this.FilterId_I);
        var img_R = document.getElementById(this.FilterId_R);
        if (!this.itmImportDomistic)
            var img_D = document.getElementById(this.FilterId_D);
        if (this.itmImportShipments)
            var img_C = document.getElementById(this.FilterId_C);
        if (img_E) {
            img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E_g.png");
            if (!this.itmImportDomistic)
                img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I_g.png");
            img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R_G.png");
            if (!this.itmImportDomistic)
                img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D_G.png");
            if (this.itmImportShipments)
                img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C_G.png");
            switch (this.SelectedValue) {
                case "E": {
                    img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E_w.png");
                    break;
                }
                case "I": {
                    if (!this.itmImportDomistic)
                        img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I_w.png");
                    break;
                }
                case "R": {
                    img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R_w.png");
                    break;
                }
                case "D": {
                    if (!this.itmImportDomistic)
                        img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D_w.png");
                    break;
                }
                case "C": {
                    if (this.itmImportShipments)
                        img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C_w.png");
                    break;
                }
            }
        }
    };
    DirectionsFilter.prototype.SetVisibilityImportShipments = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "IMPORTSHIPMETNS") && !this.HideCustomsImport) {
            this.itmImportShipments = true;
            this.DirectionWidth = 168;
        }
        else {
            this.itmImportShipments = false;
            this.DirectionWidth = 140;
        }
    };
    DirectionsFilter.prototype.SetVisibilityImportDomistic = function () {
        if (this.HideImportDomistic) {
            this.itmImportDomistic = true;
            this.DirectionWidth = 112;
        }
        //else {
        //}
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DirectionsFilter.prototype, "SelectedValueChanged", void 0);
    DirectionsFilter = __decorate([
        core_1.Component({
            selector: 'DirectionsFilter',
            inputs: ['SelectedValue', 'HideCustomsImport', 'HideImportDomistic'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <ul class=\"FiltersMenu\" [style.width.px]=\"DirectionWidth\" style=\"display:block\">\n        <li (click)=\"itemClicked('All')\" (mouseover)=\"itemMouseOver('All')\" [class.SelectedFilter]=\"SelectedValue === 'All'\">\n            All\n        </li>\n        <li (click)=\"itemClicked('E')\" (mouseover)=\"itemMouseOver('E')\" (mouseleave)=\"itemMouseLeave('E')\" [class.SelectedFilter]=\"SelectedValue === 'E'\" title=\"Export\">\n            <img [attr.id]=\"FilterId_E\" class=\"CenterCenter\" [attr.src]=\"SelectedValue === 'E' ? './_Resources/Images/Icons/Directions/E_w.png' : './_Resources/Images/Icons/Directions/E_g.png'\"   />\n        </li>\n        <li *ngIf=\"!itmImportDomistic\" (click)=\"itemClicked('I')\" (mouseover)=\"itemMouseOver('I')\" (mouseleave)=\"itemMouseLeave('I')\" [class.SelectedFilter]=\"SelectedValue === 'I'\" title=\"Import\">\n            <img [attr.id]=\"FilterId_I\" class=\"CenterCenter\"  [attr.src]=\"SelectedValue === 'I' ? './_Resources/Images/Icons/Directions/I_w.png' : './_Resources/Images/Icons/Directions/I_g.png'\"  />\n        </li>\n        <li (click)=\"itemClicked('R')\" (mouseover)=\"itemMouseOver('R')\" (mouseleave)=\"itemMouseLeave('R')\" [class.SelectedFilter]=\"SelectedValue === 'R'\" title=\"Drop\">\n            <img [attr.id]=\"FilterId_R\" class=\"CenterCenter\" [attr.src]=\"SelectedValue === 'R' ? './_Resources/Images/Icons/Directions/R_w.png' : './_Resources/Images/Icons/Directions/R_g.png'\"  />\n        </li>\n        <li *ngIf=\"!itmImportDomistic\" (click)=\"itemClicked('D')\" (mouseover)=\"itemMouseOver('D')\" (mouseleave)=\"itemMouseLeave('D')\" [class.SelectedFilter]=\"SelectedValue === 'D'\" title=\"Domestic\">\n            <img [attr.id]=\"FilterId_D\" class=\"CenterCenter\"  [attr.src]=\"SelectedValue === 'D' ? './_Resources/Images/Icons/Directions/D_w.png' : './_Resources/Images/Icons/Directions/D_g.png'\" />\n        </li>\n        <li *ngIf=\"itmImportShipments || itmImportDomistic\" (click)=\"itemClicked('C')\" (mouseover)=\"itemMouseOver('C')\" (mouseleave)=\"itemMouseLeave('C')\" [class.SelectedFilter]=\"SelectedValue === 'C'\" title=\"Customs Import\">\n            <img [attr.id]=\"FilterId_C\" class=\"CenterCenter\" [attr.src]=\"SelectedValue === 'C' ? './_Resources/Images/Icons/Directions/C_w.png' : './_Resources/Images/Icons/Directions/C_g.png'\" />\n        </li>\n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], DirectionsFilter);
    return DirectionsFilter;
}());
exports.DirectionsFilter = DirectionsFilter;
//# sourceMappingURL=DirectionsFilter.js.map