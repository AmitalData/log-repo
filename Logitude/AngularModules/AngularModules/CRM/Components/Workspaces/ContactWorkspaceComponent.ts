import {Component, Output, EventEmitter} from '@angular/core';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {ContactList} from '../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../Common/Services/StandardLists/ContactListService';
import {CommonDomainService, ContactSummary} from '../../../Common/Services/CommonDomainService';
import {CRMWorkspaceComponent} from './CRMWorkspaceComponent';

@Component({
    moduleId: module.id,
    templateUrl: './ContactWorkspaceComponent.html',
})

export class ContactWorkspaceComponent {
    @Output() ReloadUserQueries = new EventEmitter();
    private myCommonDomainService: CommonDomainService;
    private ContactListService: ContactListService;
    public QuickSearchItems: ContactList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myCommonDomainService = new CommonDomainService;
        this.ContactListService = new ContactListService();
    }

    public FatherComp: CRMWorkspaceComponent;
    InitComponent(father: CRMWorkspaceComponent) {
        this.FatherComp = father;
        this.BuildFilters();
        this.LoadAllScreenData();
    }
    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    //View by Filter
    private filterName_ViewUpcomingBirthdays: string = "ViewUpcomingBirthdays";
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.ContactsPageControl";

    public ViewByFilterList: CodeNameClass[] = [];
    private BuildFilters() {
        this.ViewByFilterList = [];

        this.ViewByFilterList.push(new CodeNameClass( "W05", "Within 5 Days"));
        this.ViewByFilterList.push(new CodeNameClass("W10", "Within 10 Days"));
        this.ViewByFilterList.push(new CodeNameClass("W30", "Within a Month"));
        this.ViewByFilterList.push(new CodeNameClass ("TOD","Today"));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_ViewUpcomingBirthdays);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "W05";
        }

        this.SelectedViewByItem = this.ViewByFilterList.filter(d => d.Code == defaultFilterCode)[0];
    }

    private selectedViewByItem: CodeNameClass;
    get SelectedViewByItem() { return this.selectedViewByItem; }
    set SelectedViewByItem(newValue: CodeNameClass) {
        if (this.selectedViewByItem != newValue) {
            this.selectedViewByItem = newValue;

            this.RunUpcomingBirthdaysFilter();
        }
    }

    private RunUpcomingBirthdaysFilter() {
        if (this.SelectedViewByItem == null) {
            this.LoadUpcomingBirthdays(0, 5);
        }

        else {
            switch (this.SelectedViewByItem.Code) {
                case "W05":
                    {
                        this.LoadUpcomingBirthdays(0, 5);
                        break;
                    }
                case "W10":
                    {
                        this.LoadUpcomingBirthdays(0, 10);
                        break;
                    }
                case "W30":
                    {
                        this.LoadUpcomingBirthdays(0, 30);
                        break;
                    }
                case "TOD":
                    {
                        this.LoadUpcomingBirthdays(0, 0);
                        break;
                    }
            }
        }
    }
    public LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.RunUpcomingBirthdaysFilter();
        this.ReloadUsersQuery();
    }
    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }
    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    //Load Data Counts
    public WithoutRemindersCount: number;
    private LoadQueriesCounts() {
        this.myCommonDomainService.GetContactsCounts().subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                var myData: ContactSummary = myResponse.Result;
                if (myData != null) {
                    this.WithoutRemindersCount = myData.WithoutRemindersCount;
                }
            }
        });
    }

    // Load  Data
    public UpcomingBirthdaysListCount: number = 0;
    public UpcomingBirthdaysList: ContactList[];
    private LoadUpcomingBirthdays(start: number, end: number) {
        if (this.ContactListService == null) {
            this.ContactListService = new ContactListService();
        }
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("UpcomingBirthdaysFilter", start, end, null, "Equals", true, true, false, "number");
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortDirection = "Descending";
        filters.SortBy = "EnglishName";
        filters.GetCount = true;
        filters.GetAll = true;
        this.ContactListService.getByFilters(filters).subscribe(myResult => {
            if (myResult == null) {
                this.UpcomingBirthdaysList = [];
                this.UpcomingBirthdaysListCount = 0;
                this.FatherComp.UpcomingCountVisibility = false;
                this.FatherComp.UpcomingCount = 0;
            }
            else {
                this.UpcomingBirthdaysList = myResult.Result;
                this.UpcomingBirthdaysListCount = myResult.Count;

                this.FatherComp.UpcomingCount = myResult.Count;
                if (myResult.Count != 0) {
                    this.FatherComp.UpcomingCountVisibility = true;
                }
                else {
                    this.FatherComp.UpcomingCountVisibility = false;
                }
            }
        });
    }

    ViewContactQuery(code: string) {
        if (code != null) {
            var objectTableName = "Contact";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = "CRM";

            switch (code) {
                case "all":
                    {
                        queryCode = "Contacts";
                        displayTitle = "All Contacts";
                        break;
                    }

                case "reminders":
                    {
                        queryCode = "No Reminders";
                        displayTitle = "Contacts Without Reminders";
                        break;
                    }

                //case "upcoming":
                //    {
                //        queryCode = "Upcoming Birthdays";
                //        displayTitle = "Upcoming Birthdays";
                //        break;
                //    }              

                default: { break; }
            }

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();

            var listArgs = new ListComponentArgs();
            //listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    this.CurrentSession.AddMenuReference(cmpRef);
                });;
        }
    }
    EditContact(entity: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Contact', BackButtonLabel: "Contacts" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadAllScreenData();
                });
            });
    }


}
