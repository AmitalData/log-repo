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
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var core_1 = require("@angular/core");
var ListHeaderTemplateComponent = /** @class */ (function () {
    function ListHeaderTemplateComponent(_elementRef, _ViewContainerRef) {
        this._elementRef = _elementRef;
        this._ViewContainerRef = _ViewContainerRef;
        this.noComponent = false;
    }
    ListHeaderTemplateComponent.prototype.ngOnInit = function () {
        if (this.colDef.ColumnHeaderTemplateName && this.colDef.ColumnHeaderTemplateName) {
            this.noComponent = false;
            //console.log("htmlHeaderComponent is -----> ", this.htmlHeaderComponent);
            if (this.colDef.ColumnHeaderTemplateName === "TransportModeHeaderTemplate") {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Shipment/Components/ListHeaderTemplates/TransportModeListHeaderTemplate", this._ViewContainerRef);
            }
            if (this.colDef.ColumnHeaderTemplateName === "DirectionHeaderTemplate") {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Shipment/Components/ListHeaderTemplates/DirectionListHeaderTemplate", this._ViewContainerRef);
            }
            if (this.colDef.ColumnHeaderTemplateName === "ARInvoiceIsPrintedHeaderTemplate") {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Invoice/Components/ListHeaderTemplates/ARInvoiceIsPrintedHeaderTemplate", this._ViewContainerRef);
            }
            if (this.colDef.ColumnHeaderTemplateName === "ARInvoiceSentHeaderTemplate") {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Invoice/Components/ListHeaderTemplates/ARInvoiceSentHeaderTemplate", this._ViewContainerRef);
            }
        }
        else {
            this.noComponent = true;
        }
    };
    ListHeaderTemplateComponent = __decorate([
        core_1.Component({
            selector: 'list-header-template',
            template: "<!--<div>-->\n                \n               <span><span style=\"text-overflow: ellipsis\" *ngIf=\"noComponent\">{{col.Display}}</span></span>\n            \n               <!--</div>-->",
            inputs: ['colDef']
        }),
        __metadata("design:paramtypes", [core_1.ElementRef, core_1.ViewContainerRef])
    ], ListHeaderTemplateComponent);
    return ListHeaderTemplateComponent;
}());
exports.ListHeaderTemplateComponent = ListHeaderTemplateComponent;
//# sourceMappingURL=ListHeaderTemplateComponent.js.map