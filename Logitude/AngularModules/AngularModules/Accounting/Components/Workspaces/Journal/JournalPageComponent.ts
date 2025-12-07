declare var window: any;
import { ObjectsLocator } from './../../../../Infrastructure/Locators/ObjectsLocator';
import { Component, Output, EventEmitter, OnInit, AfterViewInit ,ChangeDetectorRef} from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { JournalExtendedListService } from '../../../Services/ExtendedLists/JournalExtendedListService';
import { JournalList } from '../../../EntityLists/JournalList';
import { JournalPM } from '../../../EntityPMs/JournalPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { JournalSummary } from '../../../DataContracts/AccountingSummery';
import { FastSearchService } from 'Infrastructure/Components/ListComponent/FastSearchService';
import { FastSearchResult, FastSearchSettings } from 'Customs/Services/WebServices/AzureSearchWebService';
import { BehaviorSubject } from 'rxjs';


@Component({
    
    templateUrl: './JournalPageComponent.html',

})

export class JournalPageComponent implements AfterViewInit {
    @Output() ReloadUserQueries = new EventEmitter();
    public RecentJournalsCount: number = 0;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private myJournalService: JournalExtendedListService = new JournalExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;

    private _JournalExtendedListService: JournalExtendedListService = new JournalExtendedListService();
    public isScreenLoaded: boolean = false;
    public isRTL: boolean = false;
    public showLocal: boolean = false;

    searchDropdownOptions: FastSearchResult[] = [];    
    fastSearchSettings: FastSearchSettings = null;
    enableFastSearch = false;
    constructor(private CD: ChangeDetectorRef, private fastSearchService: FastSearchService) {

        this.enableFastSearch = FeatureLocator.HasFeaturePermession("General", "FASTSEARCH");
        this.getResources();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;

    }

    private getResources() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) => {
                    this._entityResourceService.getEntityResourceByTableName("Reconciliation").subscribe((response: any) => {
                        this._entityResourceService.getEntityResourceByTableName("Revaluation").subscribe((response: any) => {
                            this.isScreenLoaded = true;
                            this.CurrentSession.StopBusyIndicator();
                            this.InitComponent();
                        });
                    });
                });
            });
        });
    }

    ngAfterViewInit() {
        // this.LoadAllScreenData();
    }

    public IsQueryVisible_MyViewsGroup: boolean = false;

    async InitComponent() {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();

        this.IsQueryVisible_MyViewsGroup = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
        var objectTable= window.ObjectTables.filter(d=> d.Name == "Journal")[0];
        await this.fastSearchService.initFastSearch(objectTable, "Journal", "Journal", true ,"journalline");
        this.fastSearchSettings = this.fastSearchService.Settings;
        if (this.fastSearchSettings) {
            this.fastSearchSettings.left = this.isRTL ? -1 : 0;
        }

    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    // Queries Features
    public Draft_JournalsVisibility: boolean = false;
    public Non_Approved_JournalsVisibility: boolean = false;
    public Approved_JournalsVisibility: boolean = false;
    public All_journalsVisibility: boolean = false;
    public ExternalJournalsVisibility: boolean = false;
    public Auto_Created_JournalsVisibility: boolean = true;
    public LoadCVS_JournalsVisibility: boolean = true;
    
    SetQueriesVisibility() {
        this.Draft_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "DraftJournal") ? true : false;
        this.Non_Approved_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "SavedJournal") ? true : false;
        this.Approved_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "ApprovedJournal") ? true : false;
        this.All_journalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "JOURNAL") ? true : false;
        this.ExternalJournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "ExternalJournals") ? true : false;
        this.LoadCVS_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "LOADJOURNALCSV") ? true : false;
        //this.Auto_Created_JournalsVisibility = FeatureLocator.HasFeaturePermession("Journal", "Auto_Created_Journals") ? true : false;
    }

    journalSummary: JournalSummary = new JournalSummary();
    LoadQueriesCounts() {

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



    public LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.LoadRecentJournals();
        this.ReloadUsersQuery();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    EditJournal(entity: any) {
        if (entity != null) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.RefreshButtonClicked();
                    });
                });
        }
    }

    ViewAccountingQuery(myQueryCode: string , filter : ApiQueryFilters = null) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All Journals";

            var filters = filter ?? new ApiQueryFilters();
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

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = queryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "Journal";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = "Full Accounting";
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

    ViewLedgerTransactionsQuery(){

        var displayTitle = TextCodeTranslator.Translate("Accounting.General.O.LedgerTransactions");
        var filters = new ApiQueryFilters();
        var queryCode = "LedgerTransactions";

        var listArgs = new ListComponentArgs();
        listArgs.QueryCode = queryCode;
        listArgs.Filters = filters;
        listArgs.ObjectTableName = "LedgerTransaction";
        listArgs.DisplayTitle = displayTitle;
        // listArgs.BackButtonTitle = "Full Accounting";
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

    ViewRevaluationQuery(){

        var displayTitle = "";
        var filters = new ApiQueryFilters();
        displayTitle = TextCodeTranslator.Translate("Revaluation.Q.AllRevaluations");
        var queryCode = "AllRevaluations";

        var listArgs = new ListComponentArgs();
        listArgs.QueryCode = queryCode;
        listArgs.Filters = filters;
        listArgs.ObjectTableName = "Revaluation";
        listArgs.DisplayTitle = displayTitle;
        // listArgs.BackButtonTitle = "Full Accounting";
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

    public RecentJournalsList: JournalList[];
    LoadRecentJournals() {
        this.RecentJournalsList = [];
        this.RecentJournalsCount = 0;

        this.myJournalService.GetRecentJournals().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;

                    this.RecentJournalsList = myResult;
                    this.RecentJournalsCount = myResult.length;
                }
            }
        });
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
        entityPM.IsNew=true;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Main")
                });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                });
            });
    }

    NewRevaluation() {

        var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        if (useLocal) {
            var GeneralText = TextCodeTranslator.TranslateTable("Accounting.General.O.New");
            var ChangedText = GeneralText.split('%')[0];
            var NewText = TextCodeTranslator.TranslateTable('Revaluation');
            var FinalText = NewText + " " + ChangedText;
            var newEntityButtonLabel = FinalText;
        }
        else {
            var newEntityButtonLabel = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable('Revaluation'));
        }


        var windowTitle = newEntityButtonLabel;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 550;
        logWindow.Height = 450;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewRevaluationComponent');

    }

    LoadJournalFromFile() {
        var logWindow = new LogitudeWindow();
        logWindow.IsShowCloseButton = true;
        logWindow.Width = 900;
        logWindow.Height = 400;
        logWindow.Title = TextCodeTranslator.Translate("Journal.Features.LOADJOURNALCSV");
        logWindow.WindowArgs = {};
        logWindow.WindowClosed.subscribe(($event: any) => {

        });
        logWindow.Show('./Accounting/Components/NewEntity/JournalCSVLoadComponent');

    }
    async showRecentSearches() {
        if ( this.searchFields?.length >= this.fastSearchService.Settings.minimumSearchQueryLength) return;

        this.searchDropdownOptions = await this.fastSearchService.getRecentSearches();
        this.CD.detectChanges();    
    }
    public searchFields: string;
    private timerToken: any;

    onSearchTextChangeEvent(searchtext) {
        const timer: number = this.fastSearchService.Settings.idleSearchTimeMs ?? 400;

        console.log("Search");
        if ((this.searchFields != searchtext) && !(searchtext == null && this.searchFields == "")) {
            this.searchFields = searchtext;
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            this.timerToken = setTimeout(() => this.searchMethod(), timer);
        }

        this.showRecentSearches() 
    }
    CurrentQueryFilters: ApiQueryFilters;    
    @Output() onQueryChangeEvent = new EventEmitter();


    async searchMethod() {    
       
        try {
            if (this.searchFields?.length < this.fastSearchService.Settings.minimumSearchQueryLength) return;

            this.CD.detectChanges();
            this.searchDropdownOptions = await this.fastSearchService.search(this.CurrentQueryFilters, this.searchFields)
            if (this.searchDropdownOptions) {
                this.CD.detectChanges();
                return;
            }    
        } catch (error) {
            
               
        } finally {
        } 
       
        this.CurrentQueryFilters = new ApiQueryFilters();

        this.CD.detectChanges();            
    }
    searchDropdownSelected(optionSelected: FastSearchResult | string) {
        this.onQueryChangeEvent.subscribe(a =>{
            this.ViewAccountingQuery("All Journals",a?.Filters);
        })
        this.fastSearchService.searchDropdownSelected(optionSelected, this.CurrentQueryFilters, null, this.searchDropdownOptions, null, this.onRowSelected.bind(this), this.onQueryChangeEvent);
    }
    onRowSelected($event) {
        this.EditJournal($event?.rowData);
    }

}
