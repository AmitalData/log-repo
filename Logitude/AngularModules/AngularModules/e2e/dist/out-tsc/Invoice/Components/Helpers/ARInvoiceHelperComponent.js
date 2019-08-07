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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var ARInvoiceHelperComponent = /** @class */ (function () {
    function ARInvoiceHelperComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.Listen();
            this.BuildComponent();
        }
    }
    ARInvoiceHelperComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    ARInvoiceHelperComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoiceHelperComponent.prototype.BuildComponent = function () {
    };
    Object.defineProperty(ARInvoiceHelperComponent.prototype, "InternalNotes", {
        get: function () { return this.EntityPM.InternalNotes; },
        set: function (value) {
            if (this.EntityPM.InternalNotes != value) {
                this.EntityPM.InternalNotes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceHelperComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./ARInvoiceHelperComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceHelperComponent);
    return ARInvoiceHelperComponent;
}());
exports.ARInvoiceHelperComponent = ARInvoiceHelperComponent;
//# sourceMappingURL=ARInvoiceHelperComponent.js.map