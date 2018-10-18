import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {JournalExtendedListService} from '../../../Services/ExtendedLists/JournalExtendedListService';
import {JournalList} from '../../../EntityLists/JournalList';
import {JournalPM} from '../../../EntityPMs/JournalPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    moduleId: module.id,
    templateUrl: './JournalPageComponent.html',

})

export class JournalPageComponent implements AfterViewInit {
    @Output() ReloadUserQueries = new EventEmitter();
    public RecentJournalsCount: number = 0;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private myJournalService: JournalExtendedListService = new JournalExtendedListService();

    constructor() {
        this.LoadAllScreenData();

    }

    ngAfterViewInit() {
        this.LoadAllScreenData();
    }

    public IsQueryVisible_MyViewsGroup: boolean = false;

    InitComponent() {
        this.LoadAllScreenData();
        this.IsQueryVisible_MyViewsGroup = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
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

    LoadQueriesCounts() {
    }

    EditJournal(entity: any) {
        if (entity != null) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.RefreshButtonClicked();
                    });
                });
        }
    }

    RunNewJournalWizard() {
        var windowTitle = "New Journal";
        var entityPM: JournalPM = new JournalPM();
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.FullAccounting") });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                });
            });
    }

    ViewAccountingQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All Journals";

            var filters = new ApiQueryFilters();
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
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        SessionLocator.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
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
}
