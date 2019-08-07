"use strict";
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
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var AccountingWorkspaceComponent = /** @class */ (function () {
    function AccountingWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.isRTL = false;
        this.IsMainTabVisibile = false;
        this.IsCustomersTabVisibile = false;
        this.IsVendorsTabVisibile = false;
        this.IsBanksTabVisibile = false;
        this.IsJournalTabVisibile = false;
        this.IsGLAccountsTabVisibile = false;
        this.IsMiscTabVisibile = false;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_GLAccounts = null;
        this.Page_Main = null;
        this.Page_Journals = null;
        this.Page_Receivable = null;
        this.Page_Payable = null;
        this.Page_Banks = null;
        this.Page_Misc = null;
        this.RunComponent();
        this.GetResources();
        this.CheckFeatures();
    }
    AccountingWorkspaceComponent.prototype.GetResources = function () {
        this._entityResourceService.getEntityResourceByTableName("BankDeposit").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("BankDepositLine").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ARPaymentCheque").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("APPayment").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("CashBook").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("CashBookLine").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("BankAccount").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("AccountingPeriod").subscribe(function (response) { });
    };
    AccountingWorkspaceComponent.prototype.CheckFeatures = function () {
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'General'; })[0];
        var mainTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCMAIN") && f.ObjectTableId == table.Id; })[0];
        if (mainTabFeature) {
            this.IsMainTabVisibile = true;
        }
        var CustomersTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCCustomers") && f.ObjectTableId == table.Id; })[0];
        if (CustomersTabFeature) {
            this.IsCustomersTabVisibile = true;
        }
        var VendorsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCVendors") && f.ObjectTableId == table.Id; })[0];
        if (VendorsTabFeature) {
            this.IsVendorsTabVisibile = true;
        }
        var BanksTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCBanks") && f.ObjectTableId == table.Id; })[0];
        if (BanksTabFeature) {
            this.IsBanksTabVisibile = true;
        }
        var journalTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCJORN") && f.ObjectTableId == table.Id; })[0];
        if (journalTabFeature) {
            this.IsJournalTabVisibile = true;
        }
        var GLAccountsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCGLAccounts") && f.ObjectTableId == table.Id; })[0];
        if (GLAccountsTabFeature) {
            this.IsGLAccountsTabVisibile = true;
        }
        var MiscTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCMisc") && f.ObjectTableId == table.Id; })[0];
        if (MiscTabFeature) {
            this.IsMiscTabVisibile = true;
        }
    };
    AccountingWorkspaceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.InitSelectedTab();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    AccountingWorkspaceComponent.prototype.InitSelectedTab = function () {
        // if (this.IsMainTabVisibile) {
        //     this.SelectedItem = "Main";
        // }
        // else
        if (this.IsCustomersTabVisibile) {
            this.SelectedItem = "CS";
        }
        else if (this.IsVendorsTabVisibile) {
            this.SelectedItem = "VND";
        }
        else if (this.IsBanksTabVisibile) {
            this.SelectedItem = "BNKS";
        }
        else if (this.IsJournalTabVisibile) {
            this.SelectedItem = "JORN";
        }
        else if (this.IsGLAccountsTabVisibile) {
            this.SelectedItem = "GLAccounts";
        }
        else if (this.IsMiscTabVisibile) {
            this.SelectedItem = "MISC";
        }
    };
    AccountingWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(AccountingWorkspaceComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            if (this.selectedItem != newValue) {
                this.selectedItem = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountingWorkspaceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation_1 != null) {
                    switch (this.SelectedItem) {
                        case "Main": {
                            if (this.Page_Main == null) {
                                this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Main/MainPageComponent", myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_Main = cmpRef.instance;
                                        _this.Page_Main.InitComponent();
                                    });
                                });
                            }
                            break;
                        }
                        case "JORN": {
                            if (this.Page_Journals == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Journal/JournalPageComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_Journals = cmpRef.instance;
                                    _this.Page_Journals.InitComponent();
                                });
                            }
                            break;
                        }
                        case "CS": {
                            if (this.Page_Receivable == null) {
                                this._entityResourceService.getEntityResourceByTableName("ARInvoice", 0).subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe(function (response) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Receivable/ReceivablePageComponent", myLocation_1.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.Page_Receivable = cmpRef.instance;
                                            _this.Page_Receivable.InitComponent();
                                        });
                                    });
                                });
                            }
                            break;
                        }
                        case "VND": {
                            if (this.Page_Payable == null) {
                                this._entityResourceService.getEntityResourceByTableName("APInvoice", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Payable/PayablePageComponent", myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_Payable = cmpRef.instance;
                                        _this.Page_Payable.InitComponent();
                                    });
                                });
                            }
                            break;
                        }
                        case "BNKS": {
                            if (this.Page_Banks == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Banks/BanksPageComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_Banks = cmpRef.instance;
                                    _this.Page_Banks.InitComponent();
                                });
                            }
                            break;
                        }
                        case "MISC": {
                            if (this.Page_Misc == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Misc/MiscPageComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_Misc = cmpRef.instance;
                                    _this.Page_Misc.InitComponent();
                                });
                            }
                            break;
                        }
                        case "GLAccounts": {
                            if (this.Page_GLAccounts == null) {
                                this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/GLAccounts/GLAccountsPageComponent", myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_GLAccounts = cmpRef.instance;
                                        _this.Page_GLAccounts.InitComponent();
                                    });
                                });
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AccountingWorkspaceComponent.prototype, "AllLocations", void 0);
    AccountingWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'FullAccountingComponent',
            moduleId: module.id,
            templateUrl: './AccountingWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AccountingWorkspaceComponent);
    return AccountingWorkspaceComponent;
}());
exports.AccountingWorkspaceComponent = AccountingWorkspaceComponent;
//# sourceMappingURL=AccountingWorkspaceComponent.js.map