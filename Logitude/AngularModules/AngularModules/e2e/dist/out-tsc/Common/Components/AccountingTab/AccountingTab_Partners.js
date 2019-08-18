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
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var AccountingTab_Partners = /** @class */ (function (_super) {
    __extends(AccountingTab_Partners, _super);
    function AccountingTab_Partners(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        //get ExternalId2() { return this.EntityPM.ExternalId2; }
        //set ExternalId2(value: string) {
        //    if (this.EntityPM.ExternalId2 != value) {
        //        this.EntityPM.ExternalId2 = value;
        //    }
        //}
        //get ExternalAccountingBusinessArea() { return this.EntityPM.ExternalAccountingBusinessArea; }
        //set ExternalAccountingBusinessArea(value: string) {
        //    if (this.EntityPM.ExternalAccountingBusinessArea != value) {
        //        this.EntityPM.ExternalAccountingBusinessArea = value;
        //    }
        //}
        _this.IsExternalByProductsVisible = false;
        _this.isExternalByProductsRequestd = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ObjectTableName = entityArgs.ObjectTableName;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            if (SessionLocator_1.SessionLocator.AccountingSystemPM.Code == "GI" || SessionLocator_1.SessionLocator.AccountingSystemPM.Code == "AI") {
                _this.IsExternalByProductsVisible = true;
            }
        }
        if (_this.ObjectTableName == "Customer") {
            _this.UIProperties.SetVisibility("PayablesAccountingCard", _this.ObjectTableName, false);
        }
        _this.Listen();
        return _this;
    }
    AccountingTab_Partners.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        if (_this.isExternalByProductsRequestd) {
                            _this.ApplyExternalByProducts();
                        }
                    }
                    _this.isExternalByProductsRequestd = false;
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    AccountingTab_Partners.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    Object.defineProperty(AccountingTab_Partners.prototype, "ReceivablesAccountingCard", {
        get: function () { return this.EntityPM.ReceivablesAccountingCard; },
        set: function (value) {
            if (this.EntityPM.ReceivablesAccountingCard != value) {
                this.EntityPM.ReceivablesAccountingCard = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTab_Partners.prototype, "PayablesAccountingCard", {
        get: function () { return this.EntityPM.PayablesAccountingCard; },
        set: function (value) {
            if (this.EntityPM.PayablesAccountingCard != value) {
                this.EntityPM.PayablesAccountingCard = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountingTab_Partners.prototype.ExternalByProductsClicked = function () {
        if (!this.isExternalByProductsRequestd) {
            this.isExternalByProductsRequestd = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    AccountingTab_Partners.prototype.ApplyExternalByProducts = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Accounting advanced";
        logWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName };
        logWindow.Show('./Common/Components/Partners/AddEdit/ExternalAccountsByProductsComponent');
    };
    AccountingTab_Partners = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AccountingTab_Partners.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AccountingTab_Partners);
    return AccountingTab_Partners;
}(BaseComponent_1.BaseComponent));
exports.AccountingTab_Partners = AccountingTab_Partners;
//# sourceMappingURL=AccountingTab_Partners.js.map