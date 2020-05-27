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
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../Locators/ObjectsLocator");
var ListTemplateComponent = /** @class */ (function () {
    function ListTemplateComponent(_elementRef, _ViewContainerRef, CD) {
        this._elementRef = _elementRef;
        this._ViewContainerRef = _ViewContainerRef;
        this.CD = CD;
        this.PassAdditionalData = false;
        this.RTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
        this.noComponent = false;
    }
    ListTemplateComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.htmlListComponentName && this.htmlListComponentUrl) {
            this.noComponent = false;
            if (this.PassAdditionalData == true) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load(this.htmlListComponentUrl, this._ViewContainerRef)
                    .then(function (res) {
                    //console.log("specific response: ", res);
                    res.instance.setVariables(_this.rowData, _this.fieldName, _this.AdditionalData);
                });
            }
            else {
                SessionLocator_1.SessionLocator.DynamicLoader.Load(this.htmlListComponentUrl, this._ViewContainerRef)
                    .then(function (res) {
                    //console.log("specific response: ", res);
                    res.instance.setVariables(_this.rowData, _this.fieldName);
                });
            }
        }
        else {
            this.noComponent = true;
        }
    };
    ListTemplateComponent = __decorate([
        core_1.Component({
            selector: 'list-template',
            template: "<div style=\"overflow: hidden; text-overflow: ellipsis;\">\n                \n               <span><span style=\"text-overflow: ellipsis\" [style.float]=\"RTL == true ? 'right' : 'left'\" *ngIf=\"noComponent\">{{rowData[fieldName]}}</span></span>\n            \n               </div>\n\n",
            inputs: ['htmlListComponentUrl', 'htmlListComponentName', 'fieldName', 'rowData', 'PassAdditionalData', 'AdditionalData']
        }),
        __metadata("design:paramtypes", [core_1.ElementRef, core_1.ViewContainerRef, core_1.ChangeDetectorRef])
    ], ListTemplateComponent);
    return ListTemplateComponent;
}());
exports.ListTemplateComponent = ListTemplateComponent;
//# sourceMappingURL=ListTemplateComponent.js.map