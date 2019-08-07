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
var BaseComponent_1 = require("../../Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../Tools");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var RatioBoxComponent = /** @class */ (function (_super) {
    __extends(RatioBoxComponent, _super);
    function RatioBoxComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.DataContext = null;
        _this.ObjectFieldName = null;
        _this.ObjectTableName = null;
        _this.IDataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.isEnabled = true;
        _this.iRatio = null;
        _this.Listen();
        return _this;
    }
    RatioBoxComponent.prototype.Listen = function () {
        var _this = this;
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "RatioBoxValueChanged") {
                _this.iRatio = _this.DataContext[_this.ObjectFieldName];
                //this.Validate();
            }
        });
    };
    RatioBoxComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
    };
    RatioBoxComponent.prototype.ngOnInit = function () {
        this.iRatio = this.DataContext[this.ObjectFieldName];
        this.Validate();
    };
    Object.defineProperty(RatioBoxComponent.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) {
            if (this.isEnabled != value) {
                this.isEnabled = value;
                this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatioBoxComponent.prototype, "Ratio", {
        get: function () { return this.iRatio; },
        set: function (value) {
            if (this.iRatio != value) {
                this.iRatio = value;
                this.DataContext[this.ObjectFieldName] = value;
                this.Validate();
                this.CurrentSession.FireEvent("RatioBoxValueChanged");
            }
        },
        enumerable: true,
        configurable: true
    });
    RatioBoxComponent.prototype.Validate = function () {
        if (!this.iRatio) {
            this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio field is required");
            this.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio field is required");
        }
        else {
            if (this.iRatio <= 10 && this.iRatio >= 1) {
                this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, true, null);
                this.UIProperties.SetValidity("Ratio", this.ObjectTableName, true, null);
            }
            else {
                this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio must be between 1-10");
                this.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio must be between 1-10");
            }
        }
    };
    RatioBoxComponent.prototype.OnLostFocus = function () {
        var _this = this;
        this.iRatio = null;
        setTimeout(function () {
            _this.iRatio = _this.DataContext[_this.ObjectFieldName];
            _this.Validate();
        }, 1);
        //if (AppTool.IsNullOrEmpty(this.Ratio)) {
        //    if (this.EntityPM['IsDirty']) {
        //        this.iRatio = 0;
        //        setTimeout(() => {
        //            this.NumericButtonClicked(true);
        //        }, 1)
        //    }
        //}
    };
    RatioBoxComponent.prototype.NumericButtonClicked = function (isIncreas) {
        if (this.IsEnabled) {
            if (isIncreas) {
                if (this.Ratio < 10) {
                    this.Ratio += 1;
                }
            }
            else {
                if (this.Ratio > 1) {
                    this.Ratio -= 1;
                }
            }
        }
    };
    RatioBoxComponent = __decorate([
        core_1.Component({
            selector: 'RatioBox',
            moduleId: module.id,
            templateUrl: './RatioBoxComponent.html',
            inputs: ['EntityPM', 'ObjectFieldName', 'ObjectTableName', 'DataContext', 'IsEnabled'],
        }),
        __metadata("design:paramtypes", [])
    ], RatioBoxComponent);
    return RatioBoxComponent;
}(BaseComponent_1.BaseComponent));
exports.RatioBoxComponent = RatioBoxComponent;
//# sourceMappingURL=RatioBoxComponent.js.map