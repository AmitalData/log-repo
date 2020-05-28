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
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ARInvoiceShortTitleComponent = /** @class */ (function () {
    function ARInvoiceShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.DisplaySATSettings = false;
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsConnectedToConsolidation = false;
        this.EntityNumber = null;
        this.EntityPM = this.entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.BuildComponent();
        this.Listen();
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATSettings = true;
        }
    }
    ARInvoiceShortTitleComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildComponent();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildComponent();
                }
            });
        }
    };
    ARInvoiceShortTitleComponent.prototype.BuildComponent = function () {
        if (this.EntityPM != null) {
            if (this.EntityPM.IsConstituentInvoice && this.EntityPM.ConsolidationInvoiceId != null) {
                this.IsConnectedToConsolidation = true;
            }
            this.GetEntityNumber();
        }
    };
    ARInvoiceShortTitleComponent.prototype.GetEntityNumber = function () {
        if (this.EntityPM.StatusCode == "DR") {
            if (this.EntityPM.DraftNumber) {
                this.EntityNumber = this.EntityPM.DraftNumber + ", ";
            }
        }
        else {
            if (this.EntityPM.InvoiceNumber) {
                this.EntityNumber = this.EntityPM.InvoiceNumber + ", ";
            }
        }
    };
    ARInvoiceShortTitleComponent.prototype.ViewEntityClicked = function (invoiceId) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(invoiceId)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: invoiceId, ObjectTableName: 'ARInvoice', BackButtonLabel: "A/R Invoice: " + _this.EntityPM.InvoiceNumber });
            });
        }
    };
    ARInvoiceShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./ARInvoiceShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceShortTitleComponent);
    return ARInvoiceShortTitleComponent;
}());
exports.ARInvoiceShortTitleComponent = ARInvoiceShortTitleComponent;
//# sourceMappingURL=ARInvoiceShortTitleComponent.js.map