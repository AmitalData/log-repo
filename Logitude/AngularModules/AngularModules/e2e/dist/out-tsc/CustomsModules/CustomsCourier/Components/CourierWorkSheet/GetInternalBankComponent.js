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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CustomBankListService_1 = require("../../../../Customs/Services/StandardLists/CustomBankListService");
var GetInternalBankComponent = /** @class */ (function (_super) {
    __extends(GetInternalBankComponent, _super);
    function GetInternalBankComponent(_entityResourceService, entityArgs) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingPeriod";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._CustomBankListService = new CustomBankListService_1.CustomBankListService();
        _this.BanksList = [];
        _this.UIProperties.SetEnabled("SelectedBank", _this.ObjectTableName, true);
        return _this;
    }
    GetInternalBankComponent.prototype.ngOnInit = function () {
        this.LoadBanks();
        this.CurrentSession.StopBusyIndicator();
    };
    GetInternalBankComponent.prototype.FillErrors = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedBank)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Bank Field is Required");
        }
        else {
            this.ValidationErrorsList = [];
        }
    };
    Object.defineProperty(GetInternalBankComponent.prototype, "SelectedBank", {
        // Properties
        get: function () { return this._SelectedBank; },
        set: function (newValue) {
            this._SelectedBank = newValue;
            this.ValidationErrorsList = [];
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.UIProperties.SetRequired("SelectedBank", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("SelectedBank", this.ObjectTableName, false);
            }
        },
        enumerable: true,
        configurable: true
    });
    GetInternalBankComponent.prototype.LoadBanks = function () {
        var _this = this;
        this._CustomBankListService.getAllFromCache().subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.BanksList = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                    if (_this.BanksList.length == 1) {
                        _this.SelectedBank = _this.BanksList[0];
                    }
                }
            }
        });
    };
    //public OnSend: (InternalBankId: string) => void;
    GetInternalBankComponent.prototype.OkButtonClicked = function () {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.CurrentWindow.Close(this._SelectedBank.Id);
    };
    GetInternalBankComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    GetInternalBankComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'GetInternalBankComponent',
            templateUrl: './GetInternalBankComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EntityArgs_1.EntityArgs])
    ], GetInternalBankComponent);
    return GetInternalBankComponent;
}(BaseComponent_1.BaseComponent));
exports.GetInternalBankComponent = GetInternalBankComponent;
//# sourceMappingURL=GetInternalBankComponent.js.map