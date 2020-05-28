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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var INTTRADomainService_1 = require("../../Services/INTTRADomainService");
var INTTRACommunicationSettingsComponent = /** @class */ (function (_super) {
    __extends(INTTRACommunicationSettingsComponent, _super);
    function INTTRACommunicationSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.ObjectTableName = "INTTRACommunicationSettings";
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myService = new INTTRADomainService_1.INTTRADomainService();
        _this.myService.GetINTTRACommunicationSettings().subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.EntityPM = myResponse.Result;
                _this.IsResourcesReady = true;
            }
        });
        return _this;
    }
    Object.defineProperty(INTTRACommunicationSettingsComponent.prototype, "INTTRAProdFTPHost", {
        get: function () { return this.EntityPM.INTTRAProdFTPHost; },
        set: function (value) {
            if (this.EntityPM.INTTRAProdFTPHost != value) {
                this.EntityPM.INTTRAProdFTPHost = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRACommunicationSettingsComponent.prototype, "INTTRATestFTPHost", {
        get: function () { return this.EntityPM.INTTRATestFTPHost; },
        set: function (value) {
            if (this.EntityPM.INTTRATestFTPHost != value) {
                this.EntityPM.INTTRATestFTPHost = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    INTTRACommunicationSettingsComponent.prototype.SetUIProperties = function () {
        var isProdFieldValid = true;
        var isTestFieldValid = true;
        var ProdFieldValidMessage = "";
        var TestFieldValidMessage = "";
        if (this.INTTRAProdFTPHost) {
            if (this.INTTRAProdFTPHost.length > 100) {
                isProdFieldValid = false;
                ProdFieldValidMessage = "INTTRA Prod FTP field must be less than 70 and more than 0";
            }
        }
        if (this.INTTRATestFTPHost) {
            if (this.INTTRATestFTPHost.length > 100) {
                isTestFieldValid = false;
                TestFieldValidMessage = "INTTRA Test FTP field must be less than 70 and more than 0";
            }
        }
        this.UIProperties.SetValidity("INTTRAProdFTPHost", this.ObjectTableName, isProdFieldValid, ProdFieldValidMessage);
        this.UIProperties.SetValidity("INTTRATestFTPHost", this.ObjectTableName, isTestFieldValid, TestFieldValidMessage);
    };
    INTTRACommunicationSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    INTTRACommunicationSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (this.INTTRAProdFTPHost) {
            if (this.INTTRAProdFTPHost.length > 100) {
                errors.push("INTTRA Prod FTP field must be less than 70 and more than 0");
            }
        }
        if (this.INTTRATestFTPHost) {
            if (this.INTTRATestFTPHost.length > 100) {
                errors.push("INTTRA Test FTP field must be less than 70 and more than 0");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myService.UpdateINTTRACommunicationSettings(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    };
    INTTRACommunicationSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './INTTRACommunicationSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], INTTRACommunicationSettingsComponent);
    return INTTRACommunicationSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.INTTRACommunicationSettingsComponent = INTTRACommunicationSettingsComponent;
//# sourceMappingURL=INTTRACommunicationSettingsComponent.js.map