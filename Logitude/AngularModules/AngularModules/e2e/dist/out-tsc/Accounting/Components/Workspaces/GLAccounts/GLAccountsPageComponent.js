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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var JournalExtendedListService_1 = require("../../../Services/ExtendedLists/JournalExtendedListService");
var JournalPM_1 = require("../../../EntityPMs/JournalPM");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var AccountingSummery_1 = require("../../../DataContracts/AccountingSummery");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var GLAccountsPageComponent = /** @class */ (function () {
    function GLAccountsPageComponent() {
        var _this = this;
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.RecentGLAccountsCount = 0;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._GLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        this._JournalExtendedListService = new JournalExtendedListService_1.JournalExtendedListService();
        this.glAccountSummary = new AccountingSummery_1.GLAccountSummary();
        this.journalSummary = new AccountingSummery_1.JournalSummary();
        // Queries Features
        this.ActiveGLAccountsVisibility = false;
        this.InactiveGLAccountsVisibility = false;
        this.AllGLAccountsVisibility = false;
        this.OpenFilesVisibility = false;
        this.ClosedFilesVisibility = false;
        this.AllFilesVisibility = false;
        this.AllJobsVisibility = false;
        this.Draft_JournalsVisibility = false;
        this.Non_Approved_JournalsVisibility = false;
        this.Approved_JournalsVisibility = false;
        this.All_journalsVisibility = false;
        this.ExternalJournalsVisibility = false;
        this.Auto_Created_JournalsVisibility = true;
        this.isRTL = false;
        this.isScreenLoaded = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsQueryVisible_MyViewsGroup = false;
        this.LoadAllScreenData();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Journal").subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe(function (response) {
                    _this._entityResourceService.getEntityResourceByTableName("Reconciliation").subscribe(function (response) {
                        _this.isScreenLoaded = true;
                        _this.CurrentSession.StopBusyIndicator();
                    });
                });
            });
        });
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    GLAccountsPageComponent.prototype.ngAfterViewInit = function () {
        this.LoadAllScreenData();
    };
    GLAccountsPageComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    };
    GLAccountsPageComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    GLAccountsPageComponent.prototype.LoadAllScreenData = function () {
        this.LoadQueriesCounts();
        this.LoadRecentGLAccounts();
        this.ReloadUsersQuery();
    };
    GLAccountsPageComponent.prototype.SetQueriesVisibility = function () {
        this.ActiveGLAccountsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "ACTIVEGLACCOUNTS") ? true : false;
        this.InactiveGLAccountsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "INACTIVEGLACCOUNTS") ? true : false;
        this.AllGLAccountsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "ALLGLACCOUNTS") ? true : false;
        this.OpenFilesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "OPENFILESGLACCOUNTS") ? true : false;
        this.ClosedFilesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "CLOSEDFILESGLACCOUNTS") ? true : false;
        this.AllFilesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "ALLFILESGLACCOUNTS") ? true : false;
        this.AllJobsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "ALLJOBSGLACCOUNTS") ? true : false;
        this.Draft_JournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "DraftJournal") ? true : false;
        this.Non_Approved_JournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "SavedJournal") ? true : false;
        this.Approved_JournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "ApprovedJournal") ? true : false;
        this.All_journalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "JOURNAL") ? true : false;
        this.ExternalJournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "ExternalJournals") ? true : false;
        //this.Auto_Created_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "Auto_Created_Journals") ? true : false;
    };
    GLAccountsPageComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    GLAccountsPageComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    GLAccountsPageComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this._GLAccountExtendedListService.GetGLAccountsSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.glAccountSummary.ActiveGLAccountCount = myResult.ActiveGLAccountCount > 1000 ? "1000+" : myResult.ActiveGLAccountCount.toString();
                _this.glAccountSummary.InactiveGLAccountCount = myResult.InactiveGLAccountCount > 1000 ? "1000+" : myResult.InactiveGLAccountCount.toString();
                _this.glAccountSummary.AllGLAccountCount = myResult.AllGLAccountCount > 1000 ? "1000+" : myResult.AllGLAccountCount.toString();
                _this.glAccountSummary.OpenFilesCount = myResult.OpenFilesCount > 1000 ? "1000+" : myResult.OpenFilesCount.toString();
                _this.glAccountSummary.ClosedFilesGLAccountCount = myResult.ClosedFilesGLAccountCount > 1000 ? "1000+" : myResult.ClosedFilesGLAccountCount.toString();
                _this.glAccountSummary.AllFilesCount = myResult.AllFilesCount > 1000 ? "1000+" : myResult.AllFilesCount.toString();
                _this.glAccountSummary.AllJobsCount = myResult.AllJobsCount > 1000 ? "1000+" : myResult.AllJobsCount.toString();
            }
        });
        this._JournalExtendedListService.GetJournalsSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.journalSummary.AllJournalsCount = myResult.AllJournalsCount > 1000 ? "1000+" : myResult.AllJournalsCount.toString();
                _this.journalSummary.ApprovedJournalsCount = myResult.ApprovedJournalsCount > 1000 ? "1000+" : myResult.ApprovedJournalsCount.toString();
                _this.journalSummary.DraftJournalsCount = myResult.DraftJournalsCount > 1000 ? "1000+" : myResult.DraftJournalsCount.toString();
                _this.journalSummary.VoidedJournalsCount = myResult.VoidedJournalsCount > 1000 ? "1000+" : myResult.VoidedJournalsCount.toString();
                _this.journalSummary.WaitingJournalsCount = myResult.WaitingJournalsCount > 1000 ? "1000+" : myResult.WaitingJournalsCount.toString();
            }
        });
    };
    GLAccountsPageComponent.prototype.EditGLAccount = function (entity) {
        var _this = this;
        if (entity != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'GLAccount', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Main") });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    GLAccountsPageComponent.prototype.RunNewGLAccountWizard = function () {
        var _this = this;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewAccount"); // "New Account";
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewGLAccountComponent');
    };
    GLAccountsPageComponent.prototype.ViewAccountingQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All GLAccounts";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                case "ActiveGLAccounts":
                    {
                        displayTitle = "Active GLAccounts";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.ActiveGLAccounts");
                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("Inactive", false, null, null, "Equals", false, false, false, "boolean");
                        break;
                    }
                case "InactiveGLAccounts":
                    {
                        displayTitle = "Inactive GLAccounts";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.InActiveGLAccounts");
                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("Inactive", true, null, null, "Equals", false, false, false, "string");
                        break;
                    }
                case "All GLAccounts":
                    {
                        displayTitle = "All GLAccounts";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.AllGLAccounts");
                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
                        break;
                    }
                case "OpenFiles":
                    {
                        displayTitle = "Open Files";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.OpenFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("BalanceInLocalCurrency", "0", null, null, "NotEqual", true, false, false, "decimal");
                        break;
                    }
                case "ClosedFiles":
                    {
                        displayTitle = "Closed Files";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.ClosedFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("BalanceInLocalCurrency", "0", null, null, "Equals", true, false, false, "decimal");
                        break;
                    }
                case "AllFiles":
                    {
                        displayTitle = "All Files";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.AllFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");
                        break;
                    }
                case "AllJobs":
                    {
                        displayTitle = "All Jobs";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.AllJobs");
                        //filters.addAdditionalFilter("AccountTypeCode", "4", null, null, "Equals", false, false, false, "string");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "GLAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Main");
            listArgs.Perspective = "GLAccountMain";
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    // Journals
    GLAccountsPageComponent.prototype.ViewJournalQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All Journals";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                case "Draft_Journals":
                    {
                        displayTitle = "Draft Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Draft Journals";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.Q.DraftJournal");
                        break;
                    }
                case "Non_Approved_Journals":
                    {
                        displayTitle = "Waiting for approval Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Saved Journals";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.Q.SavedJournal");
                        break;
                    }
                case "Approved_Journals":
                    {
                        displayTitle = "Approved Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Approved Journals";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.Q.ApprovedJournal");
                        break;
                    }
                case "All_journals":
                    {
                        displayTitle = "All Journals";
                        queryCode = "All Journals";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.Q.Journal");
                        break;
                    }
                case "Auto_Created_Journals":
                    {
                        displayTitle = "Automatic Created Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "NotEqual", false, false, false, "string");
                        queryCode = "All Journals";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.Q.AutoCreatedJournals");
                        break;
                    }
                case "External Journals":
                    {
                        displayTitle = "Extrnal Journals";
                        queryCode = "External Journals";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.Q.ExternalJournals");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = queryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "Journal";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Main");
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    GLAccountsPageComponent.prototype.RunNewJournalWizard = function () {
        var _this = this;
        var windowTitle = "New Journal";
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewJournal");
        var entityPM = new JournalPM_1.JournalPM();
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Main")
            });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                _this.LoadAllScreenData();
                //this.isWindowOpened = false;
            });
        });
    };
    GLAccountsPageComponent.prototype.LoadRecentGLAccounts = function () {
        var _this = this;
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;
        this._GLAccountExtendedListService.GetRecentGLAccounts("1").subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;
                    _this.RecentGLAccountsList = myResult;
                    _this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], GLAccountsPageComponent.prototype, "ReloadUserQueries", void 0);
    GLAccountsPageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GLAccountsPageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GLAccountsPageComponent);
    return GLAccountsPageComponent;
}());
exports.GLAccountsPageComponent = GLAccountsPageComponent;
//# sourceMappingURL=GLAccountsPageComponent.js.map