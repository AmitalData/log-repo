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
var BankCodePM_1 = require("../../EntityPMs/BankCodePM");
var BankCodePMService_1 = require("../../Services/StandardPMs/BankCodePMService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var NewBankCodeComponent = /** @class */ (function (_super) {
    __extends(NewBankCodeComponent, _super);
    function NewBankCodeComponent(CD, entityListService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.DataContext = _this;
        _this.ObjectTableName = "BankCode";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityPM = new BankCodePM_1.BankCodePM();
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.myService = new BankCodePMService_1.BankCodePMService();
        _this.SetUIProperties();
        _this.SelectDefaultValues();
        return _this;
    }
    Object.defineProperty(NewBankCodeComponent.prototype, "Inactive", {
        // Properties
        get: function () { return this.EntityPM.Inactive == null ? false : this.EntityPM.Inactive; },
        set: function (value) {
            if (this.Inactive != value) {
                this.EntityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankCodeComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankCodeComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankCodeComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewBankCodeComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    };
    NewBankCodeComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewBankCodeComponent.prototype.SubmitChanges = function () {
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
    NewBankCodeComponent.prototype.SetUIProperties = function () {
    };
    NewBankCodeComponent.prototype.SelectDefaultValues = function () {
        this.EntityPM.Inactive = false;
    };
    NewBankCodeComponent = __decorate([
        core_1.Component({
            selector: 'NewBankCodeComponent',
            moduleId: module.id,
            providers: [EntityListService_1.EntityListService],
            templateUrl: './NewBankCodeComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], NewBankCodeComponent);
    return NewBankCodeComponent;
}(BaseComponent_1.BaseComponent));
exports.NewBankCodeComponent = NewBankCodeComponent;
//# sourceMappingURL=NewBankCodeComponent.js.map