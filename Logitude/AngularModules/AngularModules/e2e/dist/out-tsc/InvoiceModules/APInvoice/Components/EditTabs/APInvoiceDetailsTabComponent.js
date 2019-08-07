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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var APInvoiceDetailsTabComponent = /** @class */ (function () {
    function APInvoiceDetailsTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.EntityPM = null;
        this.ObjectTableName = "APInvoice";
        this.isRTL = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
    }
    APInvoiceDetailsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("APInvoice").subscribe(function (res) {
            _this.entityResourceService.getEntityResourceByTableName("APInvoiceLine").subscribe(function (res) {
                if (_this.EntityPM.IsMultipleEntities) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceMultipleDetailsTabComponent", _this.viewContainerRef)
                        .then(function (cmpRef) {
                        //cmpRef.instance
                    });
                }
                else if (_this.EntityPM.IsGeneralInvoice) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabGeneral", _this.viewContainerRef)
                        .then(function (cmpRef) {
                        //cmpRef.instance
                    });
                }
                else {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabNormal", _this.viewContainerRef)
                        .then(function (cmpRef) {
                        //cmpRef.instance
                    });
                }
            });
        });
    };
    __decorate([
        core_1.ViewChild("Child", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], APInvoiceDetailsTabComponent.prototype, "viewContainerRef", void 0);
    APInvoiceDetailsTabComponent = __decorate([
        core_1.Component({
            template: "\n        <table>\n            <tr>\n                <td>\n                    <div class=\"MediaFill\">\n                        <div #Child></div>\n                    </div>\n                </td>\n            </tr>\n        </table>\n    ",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], APInvoiceDetailsTabComponent);
    return APInvoiceDetailsTabComponent;
}());
exports.APInvoiceDetailsTabComponent = APInvoiceDetailsTabComponent;
//# sourceMappingURL=APInvoiceDetailsTabComponent.js.map