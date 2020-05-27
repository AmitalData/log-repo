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
var ObjectsLocator_1 = require("./../../../../Infrastructure/Locators/ObjectsLocator");
var core_1 = require("@angular/core");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var JournalExtendedListService_1 = require("../../../Services/ExtendedLists/JournalExtendedListService");
var JournalPM_1 = require("../../../EntityPMs/JournalPM");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AccountingSummery_1 = require("../../../DataContracts/AccountingSummery");
var JournalPageComponent = /** @class */ (function () {
    function JournalPageComponent() {
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.RecentJournalsCount = 0;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.myJournalService = new JournalExtendedListService_1.JournalExtendedListService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._JournalExtendedListService = new JournalExtendedListService_1.JournalExtendedListService();
        this.isScreenLoaded = false;
        this.isRTL = false;
        this.showLocal = false;
        this.IsQueryVisible_MyViewsGroup = false;
        // Queries Features
        this.Draft_JournalsVisibility = false;
        this.Non_Approved_JournalsVisibility = false;
        this.Approved_JournalsVisibility = false;
        this.All_journalsVisibility = false;
        this.ExternalJournalsVisibility = false;
        this.Auto_Created_JournalsVisibility = true;
        this.journalSummary = new AccountingSummery_1.JournalSummary();
        // this.LoadAllScreenData();
        this.getResources();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
    }
    JournalPageComponent.prototype.getResources = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Journal").subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe(function (response) {
                    _this._entityResourceService.getEntityResourceByTableName("Reconciliation").subscribe(function (response) {
                        _this._entityResourceService.getEntityResourceByTableName("Revaluation").subscribe(function (response) {
                            _this.isScreenLoaded = true;
                            _this.CurrentSession.StopBusyIndicator();
                            _this.InitComponent();
                        });
                    });
                });
            });
        });
    };
    JournalPageComponent.prototype.ngAfterViewInit = function () {
        // this.LoadAllScreenData();
    };
    JournalPageComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
        this.IsQueryVisible_MyViewsGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
    };
    JournalPageComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    JournalPageComponent.prototype.SetQueriesVisibility = function () {
        this.Draft_JournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "DraftJournal") ? true : false;
        this.Non_Approved_JournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "SavedJournal") ? true : false;
        this.Approved_JournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "ApprovedJournal") ? true : false;
        this.All_journalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "JOURNAL") ? true : false;
        this.ExternalJournalsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Journal", "ExternalJournals") ? true : false;
        //this.Auto_Created_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "Auto_Created_Journals") ? true : false;
    };
    JournalPageComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
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
    JournalPageComponent.prototype.LoadAllScreenData = function () {
        this.LoadQueriesCounts();
        this.LoadRecentJournals();
        this.ReloadUsersQuery();
    };
    JournalPageComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    JournalPageComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    JournalPageComponent.prototype.EditJournal = function (entity) {
        var _this = this;
        if (entity != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    JournalPageComponent.prototype.ViewAccountingQuery = function (myQueryCode) {
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
                        //filters.addAdditionalFilter("StatusCode", "0", null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Draft Journals";
                        break;
                    }
                case "Non_Approved_Journals":
                    {
                        displayTitle = "Waiting for approval Journals";
                        //filters.addAdditionalFilter("StatusCode", "1", null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Saved Journals";
                        break;
                    }
                case "Approved_Journals":
                    {
                        displayTitle = "Approved Journals";
                        //filters.addAdditionalFilter("StatusCode", "2", null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Approved Journals";
                        break;
                    }
                case "All_journals":
                    {
                        displayTitle = "All Journals";
                        queryCode = "All Journals";
                        break;
                    }
                case "Auto_Created_Journals":
                    {
                        displayTitle = "Automatic Created Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "NotEqual", false, false, false, "string");
                        queryCode = "";
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
            listArgs.BackButtonTitle = "Full Accounting";
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
    JournalPageComponent.prototype.ViewRevaluationQuery = function () {
        var _this = this;
        var displayTitle = "";
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Revaluation.Q.AllRevaluations");
        var queryCode = "AllRevaluations";
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.QueryCode = queryCode;
        listArgs.Filters = filters;
        listArgs.ObjectTableName = "Revaluation";
        listArgs.DisplayTitle = displayTitle;
        // listArgs.BackButtonTitle = "Full Accounting";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    JournalPageComponent.prototype.LoadRecentJournals = function () {
        var _this = this;
        this.RecentJournalsList = [];
        this.RecentJournalsCount = 0;
        this.myJournalService.GetRecentJournals().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;
                    _this.RecentJournalsList = myResult;
                    _this.RecentJournalsCount = myResult.length;
                }
            }
        });
    };
    // Journals
    JournalPageComponent.prototype.ViewJournalQuery = function (myQueryCode) {
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
    JournalPageComponent.prototype.RunNewJournalWizard = function () {
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
    JournalPageComponent.prototype.NewRevaluation = function () {
        var _this = this;
        var useLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        if (useLocal) {
            var GeneralText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            var ChangedText = GeneralText.split('%')[0];
            var NewText = TextCodeTranslator_1.TextCodeTranslator.TranslateTable('Revaluation');
            var FinalText = NewText + " " + ChangedText;
            var newEntityButtonLabel = FinalText;
        }
        else {
            var newEntityButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable('Revaluation'));
        }
        var windowTitle = newEntityButtonLabel;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 550;
        logWindow.Height = 450;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewRevaluationComponent');
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], JournalPageComponent.prototype, "ReloadUserQueries", void 0);
    JournalPageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './JournalPageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], JournalPageComponent);
    return JournalPageComponent;
}());
exports.JournalPageComponent = JournalPageComponent;
//# sourceMappingURL=JournalPageComponent.js.map