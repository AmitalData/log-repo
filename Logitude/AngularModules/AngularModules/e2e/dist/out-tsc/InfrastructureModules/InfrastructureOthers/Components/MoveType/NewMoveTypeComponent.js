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
var MoveTypePM_1 = require("../../../../Infrastructure/EntityPMs/MoveTypePM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var MoveTypePMService_1 = require("../../../../Infrastructure/Services/StandardPMs/MoveTypePMService");
var ClassLevelValidator_1 = require("../../../../Infrastructure/Validators/ClassLevelValidator");
var NewMoveTypeComponent = /** @class */ (function (_super) {
    __extends(NewMoveTypeComponent, _super);
    function NewMoveTypeComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "MoveType";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.transportMode = null;
        _this.EntityPM = new MoveTypePM_1.MoveTypePM();
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.MoveTypePM = new MoveTypePM_1.MoveTypePM();
        _this.EntityPM = _this.MoveTypePM;
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.MoveTypePMService = new MoveTypePMService_1.MoveTypePMService();
        _this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        _this.EntityPM.AddedManually = true;
        return _this;
    }
    Object.defineProperty(NewMoveTypeComponent.prototype, "TransportMode", {
        get: function () { return this.transportMode; },
        set: function (value) { if (value != null)
            this.transportMode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMoveTypeComponent.prototype, "Code", {
        get: function () { return this.MoveTypePM.Code; },
        set: function (value) { this.MoveTypePM.Code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMoveTypeComponent.prototype, "MoveTypeEnglishName", {
        get: function () { return this.MoveTypePM.MoveTypeEnglishName; },
        set: function (value) { this.MoveTypePM.MoveTypeEnglishName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMoveTypeComponent.prototype, "MoveTypeLocalName", {
        get: function () { return this.MoveTypePM.MoveTypeLocalName; },
        set: function (value) { this.MoveTypePM.MoveTypeLocalName = value; },
        enumerable: true,
        configurable: true
    });
    NewMoveTypeComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        this.BuildErrors();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this.MoveTypePMService.insert(this.MoveTypePM).subscribe(function (res) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                }
                else {
                    pmResponse.ErrorsArray.forEach(function (item) {
                        _this.ValidationErrorsList.push(item);
                    });
                }
            });
        }
    };
    NewMoveTypeComponent.prototype.BuildErrors = function () {
        var _this = this;
        this.MoveTypePM.TransportModeId = this.TransportMode;
        var errorsArray = this.validator.Validate("MoveType", this.MoveTypePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
    };
    NewMoveTypeComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewMoveTypeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewMoveTypeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewMoveTypeComponent);
    return NewMoveTypeComponent;
}(BaseComponent_1.BaseComponent));
exports.NewMoveTypeComponent = NewMoveTypeComponent;
//# sourceMappingURL=NewMoveTypeComponent.js.map