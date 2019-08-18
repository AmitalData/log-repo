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
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ARInvoiceDetailsTabComponent = /** @class */ (function () {
    function ARInvoiceDetailsTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.EntityPM = null;
        this.ObjectTableName = "ARInvoice";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    ARInvoiceDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "ResetARInvoiceBaseDeailsTab") {
                    _this.InitBaseTabComponent();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    };
    ARInvoiceDetailsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe(function (res1) {
            _this.entityResourceService.getEntityResourceByTableName("ARInvoiceLine").subscribe(function (res2) {
                _this.InitBaseTabComponent();
            });
        });
    };
    ARInvoiceDetailsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoiceDetailsTabComponent.prototype.InitBaseTabComponent = function () {
        this.viewContainerRef.clear();
        if (this.EntityPM.IsConsolidationInvoice) {
            if (this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR") {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabNormal", this.viewContainerRef)
                    .then(function (cmpRef) {
                    //cmpRef.instance
                });
            }
            else {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabConsolidation", this.viewContainerRef)
                    .then(function (cmpRef) {
                    //cmpRef.instance
                });
            }
        }
        else if (this.EntityPM.IsGeneralInvoice) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabGeneral", this.viewContainerRef)
                .then(function (cmpRef) {
                //cmpRef.instance
            });
        }
        else {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabNormal", this.viewContainerRef)
                .then(function (cmpRef) {
                //cmpRef.instance
            });
        }
    };
    __decorate([
        core_1.ViewChild("Child", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ARInvoiceDetailsTabComponent.prototype, "viewContainerRef", void 0);
    ARInvoiceDetailsTabComponent = __decorate([
        core_1.Component({
            template: "\n        <table>\n            <tr>\n                <td>\n                    <div class=\"MediaFill\">\n                        <div #Child></div>\n                    </div>\n                </td>\n            </tr>\n        </table>\n    ",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ARInvoiceDetailsTabComponent);
    return ARInvoiceDetailsTabComponent;
}());
exports.ARInvoiceDetailsTabComponent = ARInvoiceDetailsTabComponent;
//# sourceMappingURL=ARInvoiceDetailsTabComponent.js.map