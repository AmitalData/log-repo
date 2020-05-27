"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DeclarationCourierStatusPMService_1 = require("../../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService");
var CourierPendingReasonGeneralComponent = /** @class */ (function (_super) {
    __extends(CourierPendingReasonGeneralComponent, _super);
    function CourierPendingReasonGeneralComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationCourierStatus";
        _this.DeclarationsList = [];
        _this.ValidationErrorsList = [];
        _this._DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService_1.DeclarationCourierStatusPMService();
        _this._IsNewPending = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    CourierPendingReasonGeneralComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.CourierHawb = args.CourierHawb;
            if (args.Mode == "FromDeclaration") {
                this.CurrentSession.StartBusyIndicatorLoading();
                this._DeclarationCourierStatusPMService.get(args.DeclarationId).subscribe(function (response) {
                    _this.CurrentSession.StopBusyIndicator();
                    var declarationCourierStatusPM = response.Result;
                    if (declarationCourierStatusPM != null) {
                        _this.DeclarationsList.push(declarationCourierStatusPM);
                        if (!Tools_1.AppTool.IsNullOrEmpty(declarationCourierStatusPM.CourierPendingReasonCode) || !Tools_1.AppTool.IsNullOrEmpty(declarationCourierStatusPM.PendingRemarks)) {
                            _this._IsNewPending = false;
                            _this.CourierPendingReasonCode = declarationCourierStatusPM.CourierPendingReasonCode;
                            _this.PendingRemarks = declarationCourierStatusPM.PendingRemarks;
                        }
                    }
                });
            }
            else {
                if (args.Mode == "Update") {
                    this._IsNewPending = false;
                    this.CourierPendingReasonCode = args.CourierPendingReasonCode;
                    this.PendingRemarks = args.PendingRemarks;
                }
                if (args.DeclarationIdList != null) {
                    args.DeclarationIdList.forEach(function (declarationCourierStatusPM) {
                        _this.DeclarationsList.push(declarationCourierStatusPM);
                    });
                }
            }
        }
    };
    Object.defineProperty(CourierPendingReasonGeneralComponent.prototype, "CourierHawb", {
        get: function () { return this._CourierHawb; },
        set: function (newValue) {
            this._CourierHawb = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierPendingReasonGeneralComponent.prototype, "CourierPendingReasonCode", {
        get: function () { return this._CourierPendingReasonCode; },
        set: function (newValue) {
            this._CourierPendingReasonCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierPendingReasonGeneralComponent.prototype, "CourierPendingReasonName", {
        get: function () { return this._CourierPendingReasonName; },
        set: function (newValue) {
            this._CourierPendingReasonName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierPendingReasonGeneralComponent.prototype, "PendingRemarks", {
        get: function () { return this._PendingRemarks; },
        set: function (newValue) {
            this._PendingRemarks = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion\
    CourierPendingReasonGeneralComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.DeclarationsList.forEach(function (declarationCourierStatusPM) {
            declarationCourierStatusPM.CourierPendingReasonCode = _this.CourierPendingReasonCode;
            declarationCourierStatusPM.PendingRemarks = _this.PendingRemarks;
            _this._DeclarationCourierStatusPMService.update(declarationCourierStatusPM).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindow();
            });
        });
    };
    CourierPendingReasonGeneralComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CourierPendingReasonGeneralComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CourierPendingReasonGeneralComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CourierPendingReasonGeneralComponent);
    return CourierPendingReasonGeneralComponent;
}(BaseComponent_1.BaseComponent));
exports.CourierPendingReasonGeneralComponent = CourierPendingReasonGeneralComponent;
//# sourceMappingURL=CourierPendingReasonGeneralComponent.js.map