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
var AmitalGatewayUtil_1 = require("../../../Infrastructure/Utilities/AmitalGatewayUtil");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DeclarationShortTitleComponent = /** @class */ (function () {
    function DeclarationShortTitleComponent(cd, entityArgs) {
        this.cd = cd;
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.EntityNumber = null;
        this._EntityNumber = null;
        this._ShowEntityNumberClick = false;
        this._CourierImporterName = null;
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }
    DeclarationShortTitleComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess && _this.CurrentSession.CurrentEditComponent) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildComponent();
                    _this.cd.detectChanges();
                }
            }));
        }
    };
    Object.defineProperty(DeclarationShortTitleComponent.prototype, "CourierImporterName", {
        get: function () {
            if (this.EntityPM.ImporterCode) {
                this._CourierImporterName = this.EntityPM.CalculatedImporterName;
            }
            else {
                this._CourierImporterName = this.EntityPM.ImporterName;
            }
            return this._CourierImporterName;
        },
        set: function (newValue) { this._CourierImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    DeclarationShortTitleComponent.prototype.BuildComponent = function () {
        if (this.EntityPM.IsCourierDeclaration) {
            if (this.EntityPM.CustomFileNo && (this.EntityPM.CalculatedImporterName || this.EntityPM.ImporterName)) {
                this._EntityNumber = this.EntityPM.CustomFileNo;
            }
            else {
                this.EntityNumber = this.EntityPM.CustomFileNo;
            }
            if (this.EntityPM.ImporterCode) {
                this.CourierImporterName = this.EntityPM.CalculatedImporterName;
            }
            else {
                this.CourierImporterName = this.EntityPM.ImporterName;
            }
        }
        else {
            if (this.EntityPM.CustomFileNo && this.EntityPM.CustomerName) {
                this._EntityNumber = this.EntityPM.CustomFileNo;
            }
            else if (this.EntityPM.CustomFileNo == null && this.EntityPM.CustomerName) {
                this.EntityNumber = this.EntityPM.CustomerName;
            }
            else if (this.EntityPM.CustomFileNo && this.EntityPM.CustomerName == null) {
                this.EntityNumber = this.EntityPM.CustomFileNo;
            }
        }
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse && AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsTabCA23) {
            this._ShowEntityNumberClick = true;
        }
    };
    DeclarationShortTitleComponent.prototype.EntityNumberClick = function () {
        var _this = this;
        if (!this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
            this.ShowCustomFileOPCFromDeclaration();
        }
        else {
            this.CurrentSession.StartBusyIndicatorSaving();
            var sub = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                _this.CurrentSession.StopBusyIndicator();
                sub.unsubscribe();
                if (isSaveSuccess) {
                    _this.ShowCustomFileOPCFromDeclaration();
                }
            });
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    DeclarationShortTitleComponent.prototype.ShowCustomFileOPCFromDeclaration = function () {
        var declarationEditComponentController = this.CurrentSession.CurrentEditComponent.EditComponentController;
        declarationEditComponentController.ForceCheckIfLockWhileReload();
        var unifreightMessageM = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.
            DeclarationMessaging.GetMessage(this.EntityPM.CustomFileNo, this.EntityPM.Id, "DeclarationShortTitleComponent");
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.ShowCustomFileOPCFromDeclaration", "CFIHMAIN.LogitudeTask", "ShowCustomFileOPCFromDeclaration", unifreightMessageM, " פתיחת תיק עמילות מהצהרה");
    };
    DeclarationShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "DeclarationShortTitleComponent.html",
        })
        //C: \LW\Customs\AngularModules\AngularModules\Infrastructure\Utilities\AmitalGatewayUtil.ts
        ,
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityArgs_1.EntityArgs])
    ], DeclarationShortTitleComponent);
    return DeclarationShortTitleComponent;
}());
exports.DeclarationShortTitleComponent = DeclarationShortTitleComponent;
//# sourceMappingURL=DeclarationShortTitleComponent.js.map