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
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var QuoteHelperComponent = /** @class */ (function () {
    function QuoteHelperComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.IsFollowupsVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsFollowupsVisible = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups");
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.Listen();
            this.BuildComponent();
        }
    }
    QuoteHelperComponent.prototype.Listen = function () {
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
    QuoteHelperComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    QuoteHelperComponent.prototype.BuildComponent = function () {
    };
    Object.defineProperty(QuoteHelperComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Quote", "Notes update");
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteHelperComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuoteHelperComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], QuoteHelperComponent);
    return QuoteHelperComponent;
}());
exports.QuoteHelperComponent = QuoteHelperComponent;
//# sourceMappingURL=QuoteHelperComponent.js.map