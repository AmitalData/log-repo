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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var BankAccountLitePM_1 = require("../../EntityPMs/BankAccountLitePM");
var BankAccountLitePMService_1 = require("../../Services/StandardPMs/BankAccountLitePMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var NewBankAccountLiteComponent = /** @class */ (function (_super) {
    __extends(NewBankAccountLiteComponent, _super);
    function NewBankAccountLiteComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "BankAccountLite";
        _this.EntityPM = new BankAccountLitePM_1.BankAccountLitePM();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new BankAccountLitePM_1.BankAccountLitePM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        return _this;
    }
    NewBankAccountLiteComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(NewBankAccountLiteComponent.prototype, "AccountNumber", {
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (newValue) {
            if (this.EntityPM.AccountNumber != newValue) {
                this.EntityPM.AccountNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountLiteComponent.prototype, "BankCode", {
        get: function () { return this.EntityPM.BankCode; },
        set: function (newValue) {
            if (this.EntityPM.BankCode != newValue) {
                this.EntityPM.BankCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountLiteComponent.prototype, "BranchNumber", {
        get: function () { return this.EntityPM.BranchNumber; },
        set: function (newValue) {
            if (this.EntityPM.BranchNumber != newValue) {
                this.EntityPM.BranchNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountLiteComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.CurrencyId != newValue) {
                this.EntityPM.CurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountLiteComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountLiteComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewBankAccountLiteComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewBankAccountLiteComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var myService = new BankAccountLitePMService_1.BankAccountLitePMService();
            myService.insert(this.EntityPM).subscribe(function (response) {
                if (response != null) {
                    if (!response.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = response.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    };
    NewBankAccountLiteComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewBankAccountLiteComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewBankAccountLiteComponent);
    return NewBankAccountLiteComponent;
}(BaseComponent_1.BaseComponent));
exports.NewBankAccountLiteComponent = NewBankAccountLiteComponent;
//# sourceMappingURL=NewBankAccountLiteComponent.js.map