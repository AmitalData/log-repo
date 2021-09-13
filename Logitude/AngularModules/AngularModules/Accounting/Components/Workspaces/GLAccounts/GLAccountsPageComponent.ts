import {Component, Output, EventEmitter , OnInit , AfterViewInit} from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {JournalExtendedListService} from '../../../Services/ExtendedLists/JournalExtendedListService';
import {GLAccountList} from '../../../EntityLists/GLAccountList';
import {JournalPM} from '../../../EntityPMs/JournalPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {GLAccountSummary, JournalSummary} from '../../../DataContracts/AccountingSummery';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { GLAccountSecurityLevelService } from 'Accounting/Utilities/GLAccountSecurityLevelService';

@Component({

    templateUrl: './GLAccountsPageComponent.html',

})

export class GLAccountsPageComponent implements AfterViewInit {
    @Output() ReloadUserQueries = new EventEmitter();
    public RecentGLAccountsCount: number = 0;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    private _JournalExtendedListService: JournalExtendedListService = new JournalExtendedListService();
    glAccountSummary: GLAccountSummary = new GLAccountSummary();
    journalSummary: JournalSummary = new JournalSummary();

    // Queries Features
    public ActiveGLAccountsVisibility: boolean = false;
    public InactiveGLAccountsVisibility: boolean = false;
    public AllGLAccountsVisibility: boolean = false;
    public OpenFilesVisibility: boolean = false;
    public OpenMastersVisibility: boolean = false;
    public ClosedFilesVisibility: boolean = false;
    public AllFilesVisibility: boolean = false;
    public AllJobsVisibility: boolean = false;
    public Draft_JournalsVisibility: boolean = false;
    public Non_Approved_JournalsVisibility: boolean = false;
    public Approved_JournalsVisibility: boolean = false;
    public All_journalsVisibility: boolean = false;
    public ExternalJournalsVisibility: boolean = false;
    public Auto_Created_JournalsVisibility: boolean = true;
    public isRTL: boolean = false;
    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor()
    {
        this.LoadAllScreenData();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) =>
        {
            this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) =>
            {
                this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) =>
                {
                    this._entityResourceService.getEntityResourceByTableName("BankAccount").subscribe((response: any) =>
                    {
                        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe((response: any) =>
                        {
                            this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe((response: any) =>
                            {
                                this._entityResourceService.getEntityResourceByTableName("Reconciliation").subscribe((response: any) =>
                                {
                                    this._entityResourceService.getEntityResourceByTableName("ExternalReconciliation").subscribe((response: any) =>
                                    {
                                        this.isScreenLoaded = true;
                                        this.CurrentSession.StopBusyIndicator();
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    ngAfterViewInit() {
        this.LoadAllScreenData();
    }
    public IsQueryVisible_MyViewsGroup: boolean = false;

    InitComponent() {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    public LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.LoadRecentGLAccounts();
        this.ReloadUsersQuery();
    }

    SetQueriesVisibility() {
        this.ActiveGLAccountsVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ACTIVEGLACCOUNTS") ? true : false;
        this.InactiveGLAccountsVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "INACTIVEGLACCOUNTS") ? true : false;
        this.AllGLAccountsVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ALLGLACCOUNTS") ? true : false;
        this.OpenFilesVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "OPENFILESGLACCOUNTS") ? true : false;
        this.OpenMastersVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "GLAccount.Q.OpenMasters") ? true : false;

        this.ClosedFilesVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "CLOSEDFILESGLACCOUNTS") ? true : false;
        this.AllFilesVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ALLFILESGLACCOUNTS") ? true : false;
        this.AllJobsVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ALLJOBSGLACCOUNTS") ? true : false;

        this.Draft_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "DraftJournal") ? true : false;
        this.Non_Approved_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "SavedJournal") ? true : false;
        this.Approved_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "ApprovedJournal") ? true : false;
        this.All_journalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "JOURNAL") ? true : false;
        this.ExternalJournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "ExternalJournals") ? true : false;
        //this.Auto_Created_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "Auto_Created_Journals") ? true : false;
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    LoadQueriesCounts() {
        this._GLAccountExtendedListService.GetGLAccountsSummary().subscribe((myResult:GLAccountSummary) => {
            if (myResult != null) {
                this.glAccountSummary.ActiveGLAccountCount = myResult.ActiveGLAccountCount > 1000 ? "1000+" : myResult.ActiveGLAccountCount.toString();
                this.glAccountSummary.InactiveGLAccountCount = myResult.InactiveGLAccountCount > 1000 ? "1000+" : myResult.InactiveGLAccountCount.toString();
                this.glAccountSummary.AllGLAccountCount = myResult.AllGLAccountCount > 1000 ? "1000+" : myResult.AllGLAccountCount.toString();
                this.glAccountSummary.OpenFilesCount = myResult.OpenFilesCount > 1000 ? "1000+" : myResult.OpenFilesCount.toString();
                this.glAccountSummary.OpenMastersCount = myResult.OpenMastersCount > 1000 ? "1000+" : myResult.OpenMastersCount.toString();
                this.glAccountSummary.ClosedFilesGLAccountCount = myResult.ClosedFilesGLAccountCount > 1000 ? "1000+" : myResult.ClosedFilesGLAccountCount.toString();
                this.glAccountSummary.AllFilesCount = myResult.AllFilesCount > 1000 ? "1000+" : myResult.AllFilesCount.toString();
                this.glAccountSummary.AllJobsCount = myResult.AllJobsCount > 1000 ? "1000+" : myResult.AllJobsCount.toString();
            }
        });
        this._JournalExtendedListService.GetJournalsSummary().subscribe((myResult:JournalSummary) => {
            if (myResult != null) {

                this.journalSummary.AllJournalsCount = myResult.AllJournalsCount > 1000 ? "1000+" : myResult.AllJournalsCount.toString();
                this.journalSummary.ApprovedJournalsCount = myResult.ApprovedJournalsCount > 1000 ? "1000+" : myResult.ApprovedJournalsCount.toString();
                this.journalSummary.DraftJournalsCount = myResult.DraftJournalsCount > 1000 ? "1000+" : myResult.DraftJournalsCount.toString();
                this.journalSummary.VoidedJournalsCount = myResult.VoidedJournalsCount > 1000 ? "1000+" : myResult.VoidedJournalsCount.toString();
                this.journalSummary.WaitingJournalsCount = myResult.WaitingJournalsCount > 1000 ? "1000+" : myResult.WaitingJournalsCount.toString();
            }
        });
    }

    EditGLAccount(entity: any) {
        if (entity != null) {
            GLAccountSecurityLevelService.CheckLevel(entity.Id).then(hasAccess =>
                {
                    if (hasAccess)
                        this.OpenGLAccountEditWindow(entity);
                    else
                        GLAccountSecurityLevelService.ShowSecurityBockingMessage();
                });


        }
    }
    OpenGLAccountEditWindow(entity)
    {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef =>
            {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'GLAccount', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Main") });
                cmpRef.instance.BackCompleted.subscribe(($event: any) =>
                {
                    this.RefreshButtonClicked();
                });
            });
    }
    RunNewGLAccountWizard() {
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewAccount"); // "New Account";
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewGLAccountComponent');
    }

    ViewAccountingQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All GLAccounts";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "ActiveGLAccounts":
                    {
                        displayTitle = "Active GLAccounts";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.ActiveGLAccounts");

                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("Inactive", false, null, null, "Equals", false, false, false, "boolean");

                        break;
                    }
                case "InactiveGLAccounts":
                    {
                        displayTitle = "Inactive GLAccounts";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.InActiveGLAccounts");
                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("Inactive", true, null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case "All GLAccounts":
                    {
                        displayTitle = "All GLAccounts";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllGLAccounts");
                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case "OpenFiles":
                    {
                        displayTitle = "Open Files";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.OpenFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("BalanceInLocalCurrency", "0", null, null, "NotEqual", true, false, false, "decimal");

                        break;
                    }
                case "OpenMasters":
                    {
                        displayTitle = "Open Masters";
                        displayTitle = TextCodeTranslator.Translate("GLAccount.Q.OpenMasters");
                        filters.addAdditionalFilter("BalanceInLocalCurrencyNotNull", true, null, null, "Equals", true, false, false, "string", false, false);

                        break;
                    }
                case "ClosedFiles":
                    {
                        displayTitle = "Closed Files";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.ClosedFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("BalanceInLocalCurrency", "0", null, null, "Equals", true, false, false, "decimal");

                        break;
                    }
                case "AllFiles":
                    {
                        displayTitle = "All Files";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case "AllJobs":
                    {
                        displayTitle = "All Jobs";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllJobs");
                        //filters.addAdditionalFilter("AccountTypeCode", "4", null, null, "Equals", false, false, false, "string");

                        break;
                    }


                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "GLAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Main");
            listArgs.Perspective = "GLAccountMain";
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    // Journals
    ViewJournalQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All Journals";

            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "Draft_Journals":
                    {
                        displayTitle = "Draft Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Draft Journals";
                        displayTitle = TextCodeTranslator.Translate("Journal.Q.DraftJournal");

                        break;
                    }
                case "Non_Approved_Journals":
                    {
                        displayTitle = "Waiting for approval Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Saved Journals";
                        displayTitle = TextCodeTranslator.Translate("Journal.Q.SavedJournal");

                        break;
                    }
                case "Approved_Journals":
                    {
                        displayTitle = "Approved Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "Equals", false, false, false, "string");
                        queryCode = "Approved Journals";
                        displayTitle = TextCodeTranslator.Translate("Journal.Q.ApprovedJournal");

                        break;
                    }
                case "All_journals":
                    {
                        displayTitle = "All Journals";
                        queryCode = "All Journals";
                        displayTitle = TextCodeTranslator.Translate("Journal.Q.Journal");

                        break;
                    }
                case "Auto_Created_Journals":
                    {
                        displayTitle = "Automatic Created Journals";
                        filters.addAdditionalFilter("AccountingEntityCode", "1", null, null, "NotEqual", false, false, false, "string");
                        queryCode = "All Journals";
                        displayTitle = TextCodeTranslator.Translate("Journal.Q.AutoCreatedJournals");

                        break;
                    }

                case "External Journals":
                    {
                        displayTitle = "Extrnal Journals";
                        queryCode = "External Journals";
                        displayTitle = TextCodeTranslator.Translate("Journal.Q.ExternalJournals");

                        break;
                    }

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = queryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "Journal";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Main");
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    RunNewJournalWizard() {
        var windowTitle = "New Journal";
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewJournal");


        var entityPM: JournalPM = new JournalPM();
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Main") });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                });
            });
    }


    public RecentGLAccountsList: GLAccountList[];
    LoadRecentGLAccounts() {
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;

        this._GLAccountExtendedListService.GetRecentGLAccounts("1").subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;

                    this.RecentGLAccountsList = myResult;
                    this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    }
}
