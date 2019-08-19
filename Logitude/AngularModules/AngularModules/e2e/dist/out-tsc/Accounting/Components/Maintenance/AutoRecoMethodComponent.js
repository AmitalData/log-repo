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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var AutomaticReconcileMethodPM_1 = require("../../EntityPMs/AutomaticReconcileMethodPM");
var AutomaticReconcileMethodPMService_1 = require("../../Services/StandardPMs/AutomaticReconcileMethodPMService");
var AutoRecoMethodComponent = /** @class */ (function (_super) {
    __extends(AutoRecoMethodComponent, _super);
    function AutoRecoMethodComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "AutomaticReconcileMethod";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityPM = new AutomaticReconcileMethodPM_1.AutomaticReconcileMethodPM();
        _this.EntityPM.Inactive = false;
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.myService = new AutomaticReconcileMethodPMService_1.AutomaticReconcileMethodPMService();
        return _this;
    }
    Object.defineProperty(AutoRecoMethodComponent.prototype, "Code", {
        //#region Properties
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutoRecoMethodComponent.prototype, "AutomaticReconcile1", {
        get: function () { return this.EntityPM.AutomaticReconcile1; },
        set: function (value) {
            if (this.EntityPM.AutomaticReconcile1 != value) {
                this.EntityPM.AutomaticReconcile1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutoRecoMethodComponent.prototype, "AutomaticReconcile2", {
        get: function () { return this.EntityPM.AutomaticReconcile2; },
        set: function (value) {
            if (this.EntityPM.AutomaticReconcile2 != value) {
                this.EntityPM.AutomaticReconcile2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutoRecoMethodComponent.prototype, "AutomaticReconcile3", {
        get: function () { return this.EntityPM.AutomaticReconcile3; },
        set: function (value) {
            if (this.EntityPM.AutomaticReconcile3 != value) {
                this.EntityPM.AutomaticReconcile3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    // Commands
    AutoRecoMethodComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    };
    AutoRecoMethodComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AutoRecoMethodComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.myService.insert(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    AutoRecoMethodComponent = __decorate([
        core_1.Component({
            selector: 'AutoRecoMethodComponent',
            moduleId: module.id,
            templateUrl: './AutoRecoMethodComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AutoRecoMethodComponent);
    return AutoRecoMethodComponent;
}(BaseComponent_1.BaseComponent));
exports.AutoRecoMethodComponent = AutoRecoMethodComponent;
//# sourceMappingURL=AutoRecoMethodComponent.js.map