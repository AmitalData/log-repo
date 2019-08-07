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
var SessionLocator_1 = require("./../../../Infrastructure/Utilities/SessionLocator");
var GLAccountPMService_1 = require("./../../../Accounting/Services/StandardPMs/GLAccountPMService");
var core_1 = require("@angular/core");
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CardListService_1 = require("../../Services/StandardLists/CardListService");
var Args_1 = require("../../Args");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var AccountingTab_Full = /** @class */ (function (_super) {
    __extends(AccountingTab_Full, _super);
    function AccountingTab_Full(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.GLAccountId = null;
        _this.FullAccountingLabel = "Accounting Activation";
        _this.CardList = null;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._GLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        _this.ShowMessage = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.isFullAccountingClicked = false;
        _this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) { });
        _this.EntityPM = entityArgs.EntityPM;
        _this.ObjectTableName = entityArgs.ObjectTableName;
        _this.myCardListService = new CardListService_1.CardListService();
        _this.LoadCardList();
        _this.Listen();
        return _this;
    }
    AccountingTab_Full.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadCardList();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadCardList();
                    }
                });
            }
        }
    };
    AccountingTab_Full.prototype.ngOnInit = function () {
    };
    AccountingTab_Full.prototype.LoadOverviewTab = function () {
        var _this = this;
        this.ShowMessage = this.GLAccountId == null;
        if (this.GLAccountId) {
            // 1- Get the GLAccount
            this.CurrentSession.StartBusyIndicatorLoading();
            this._GLAccountPMService.get(this.GLAccountId).subscribe(function (myResult) {
                var response = myResult;
                if (!response.HasError) {
                    var entity = response.Result;
                    var myComponentPath = "./Accounting/Components/EditTabs/GLAccount/GLAccountOverviewComponent";
                    SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, _this.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.AccountPM = entity;
                        cmpRef.instance.LoadAllData();
                    });
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    AccountingTab_Full.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    AccountingTab_Full.prototype.LoadCardList = function () {
        var _this = this;
        this.myCardListService.getSingle(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.CardList = myResponse.Result;
                if (_this.CardList) {
                    _this.GLAccountId = _this.CardList.GLAccountId;
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.GLAccountId)) {
                        _this.FullAccountingLabel = _this.ObjectTableName + " GLAccount";
                    }
                    _this.LoadOverviewTab();
                }
            }
        });
    };
    AccountingTab_Full.prototype.FullAccountingClicked = function () {
        if (!this.GLAccountId)
            this.RunNewGLAccount();
        else
            this.EditGLAccount(); // will not be hit!
    };
    AccountingTab_Full.prototype.RunNewGLAccount = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Account";
        var args = new Args_1.NewGLAccountArgs();
        if (this.CardList.PartnerTypeId == "CS" || this.CardList.PartnerTypeId == "PO") {
            args.AccountType = "2";
            args.ChartOfAccountType = "3";
        }
        else {
            args.AccountType = "3";
            args.ChartOfAccountType = "4";
        }
        args.RevenueExpenseType = "3";
        args.CardId = this.CardList.Id;
        args.DisplayNo = this.CardList.Code;
        args.LocalName = this.CardList.LocalName;
        args.EnglishName = this.CardList.EnglishName;
        logWindow.WindowArgs = args;
        logWindow.Show('./Accounting/Components/NewEntity/NewGLAccountComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
    };
    AccountingTab_Full.prototype.EditGLAccount = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.GLAccountId, ObjectTableName: 'GLAccount' });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });
        });
    };
    __decorate([
        core_1.ViewChild("TabPlaceholder", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AccountingTab_Full.prototype, "viewContainerRef", void 0);
    AccountingTab_Full = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AccountingTab_Full.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AccountingTab_Full);
    return AccountingTab_Full;
}(BaseComponent_1.BaseComponent));
exports.AccountingTab_Full = AccountingTab_Full;
//# sourceMappingURL=AccountingTab_Full.js.map