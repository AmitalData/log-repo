declare var window: any;
import {Component, OnInit, AfterViewInit, Output, EventEmitter} from '@angular/core';
import {TicketPMService} from '../../Services/StandardPMs/TicketPMService';
import {TicketListService} from '../../Services/StandardLists/TicketListService';
import {TicketList} from  '../../EntityLists/TicketList';
import {TicketPM} from '../../EntityPMs/TicketPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {CRMDomainService} from '../../Services/CRMDomainService';
import {AppTool, FormatTool, DateTool} from '../../../Infrastructure/Tools';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EmployeeGroupPM} from '../../EntityPMs/EmployeeGroupPM'; 
import {EmployeeGroupPMService} from '../../Services/StandardPMs/EmployeeGroupPMService';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {UserList} from '../../../Common/EntityLists/UserList';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPMService} from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {ChartingDataClass} from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
declare var makeAmBarChart, BarClick, ResetItem: any;
@Component({
    moduleId: module.id,
    templateUrl: './TicketsComponent.html',
})

export class TicketsComponent extends BaseComponent implements OnInit, AfterViewInit {
    public DataContext: TicketsComponent = this;
    public AllOpenCount: string;
    public SLAFailureCount: string;
    public AllUnassignedTicketCount: string;
    public RecentlyUpdatedCount: string;
    //private ShipmentNumber: string = null;
    private ShipmentId: string = null;
    private CompanyId: string = null;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public IsVisible: boolean = false;
    @Output() ReloadUserQueries = new EventEmitter();
    public QuickSearchItems: TicketList[] = [];
    public OpenTicketsDueTimeExistance: boolean = false;
    public OpenTicketsDueTimeId: string;
    public NewOpenTicketsDueTime: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.InitializeIds();

    }
    InitializeIds() {
        this.OpenTicketsDueTimeId = "OpenTicketsDueTimeId_" + this.CurrentSession.GetNewId("OpenTicketsDueTimeId");
    }
    OnOpenTicketsClick(e) {        
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode: string = "All Open Tickets";
        var myTableName: string = "Ticket";
        var displayTitle: string = "";
        var typeName = "Ticket";
        var labelString: String = String.prototype.toLowerCase.apply(this.NewOpenTicketsDueTime[e.target.columnIndex].LabelProperty[item.index] + "");
        if (labelString == "overdue" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " Overdue (First Response) ";
        }
        if (labelString== "overdue" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " Overdue (Resolve) ";
        }
        if (labelString == "due < 1h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 1h (First Response) ";
        }
        if (labelString == "due < 1h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " due < 1h (Resolve) ";
        }
        if (labelString== "due < 2h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 2h (First Response) ";
        }
        if (labelString== "due < 2h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " due < 2h (Resolve) ";
        }
        if (labelString == "due < 4h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 4h (First Response) ";
        }
        if (labelString == "due < 4h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " due < 4h (Resolve) ";
        }
        if (labelString == "due < 8h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 8h (First Response) ";
        }
        if (labelString== "due < 8h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " (Resolve) due < 8h ";
        }

        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
        filterAgrs.addAdditionalFilter("OpenTicketByDueTimeCustomFilter", (this.NewOpenTicketsDueTime[e.target.columnIndex].LabelProperty[item.index] + "").toLowerCase() + (this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] + "").toLowerCase(), null, null, "Equals", true, false, false, "String");       
        filterAgrs.addAdditionalFilter("OwnerId", this.NewOpenTicketsDueTime[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("EmployeeGroupId", this.NewOpenTicketsDueTime[e.target.columnIndex].EmployeeGroupId[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");        
        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Tickets";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });        
    }
    public OpenTicketsClick() {
        if (BarClick() != null) {
            this.OnOpenTicketsClick(BarClick());
            ResetItem();
        }        
    }
    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe((response: any) => {
            this.IsVisible = true;
            this.LoadAllScreenData();
            this.BuildEmployeeGroupFilterList();
        });
    }
    ngAfterViewInit() {
        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "tickets") {
                    if (SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "new") {
                        this.RunNewTicketWizard();
                    }

                    if (SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "query") {
                        SessionLocator.ExternalParams.Args.forEach(arg => {
                            if (arg.FieldName == 'ShipmentId') {
                                this.ShipmentId = arg.FieldValue;
                                //if (arg.FieldValue) {
                                //    this.GetShipmentById(arg.FieldValue);
                                //}
                                //else {
                                //    this.IsShipmentFinished = true;
                                //}
                            }

                            if (arg.FieldName.toLocaleLowerCase() == "companycode") {
                                if (arg.FieldValue) {
                                    this.GetCardFroCode(arg.FieldValue);
                                }
                                else {
                                    this.IsReady = true;
                                }
                            }

                        });

                        this.RunTicketListByShipment();
                    }

                    //if (SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "view") {
                    //    var entityId :any;
                    //    SessionLocator.ExternalParams.Args.forEach(arg => {
                    //        if (arg.FieldName == 'entityId') {
                    //            entityId = arg.FieldValue;
                    //        }

                    //    });
                    //    if (!AppTool.IsNullOrEmpty(entityId)) {
                    //        this.EditTicket(entityId);
                    //    }
                    //}

                    //SessionLocator.ClearExternalParams();
                }
            }
        }
    }

    private IsReady = false;
    GetCardFroCode(code: string) {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        var myService: CardListService = new CardListService();

        if (!AppTool.IsNullOrEmpty(code)) {
            filters.addAdditionalFilter("Code", code, null, null, "Equals", false, false, false, "string");
        }

        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var cards = myResponse.Result;
                if (cards != null && cards.length > 0) {
                    var card = cards[0];
                    this.CompanyId = card.Id;
                   
                }
                this.IsReady = true;
                this.RunTicketListByShipment();
            }
        });
    }
    
    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }
    LoadAllScreenData() {
        this.LoadFilteredQueries();
    }

    private LoadFilteredQueries() {
        //this.OwnerId = this.selectedUserId;
        //this.EmployeeGroupId = this.selectedEmployeeGroupId;
        this.SetQueriesVisibility();
        this.ReloadUsersQuery();
        this.LoadQueriesCounts();
        this.LoadRecentTickets();
        this.BuildTopTicketsFilters();
        this.LoadTopTickets();
        this.LoadOpenTicketsDueTimeData();
    }

    // Queries Features
    public OpenQueriesVisibility: boolean = false;
    public SolvedQueriesVisibility: boolean = false;
    public OthersQueriesVisibility: boolean = false;
    public UnassignedTicketQueryVisibility: boolean = false;
    public RecentlyUpdatedVisibility: boolean = false;
    public SLAFailureVisibility: boolean = false;
    public AllOpenQueryVisibility: boolean = false;
    public SolvedSLAFailureQueryVisibility: boolean = false;
    public SolvedTicketsQueryVisibility: boolean = false;
    public AllCancelledQueryVisibility: boolean = false;
    public AllTicketsQueryVisibility: boolean = false;
    public MyViewsQueryVisibility: boolean = false;

    private SetQueriesVisibility() {
        this.OpenQueriesVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllOpenTickets") || FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.UnassignedTickets") || FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SLAFailureTickets") ? true : false;
        this.UnassignedTicketQueryVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.UnassignedTickets") ? true : false;
        this.RecentlyUpdatedVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.RecentlyUpdatedTickets") ? true : false;
        this.SLAFailureVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SLAFailureTickets") ? true : false;
        this.AllOpenQueryVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllOpenTickets") ? true : false;

        this.SolvedQueriesVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedSLAFailureTickets") || FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedTickets") ? true : false;
        this.SolvedSLAFailureQueryVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedSLAFailureTickets") ? true : false;
        this.SolvedTicketsQueryVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedTickets") ? true : false;

        this.OthersQueriesVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllCancelledTickets") || FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllTickets") ? true : false;
        this.AllCancelledQueryVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllCancelledTickets") ? true : false;
        this.AllTicketsQueryVisibility = FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllTickets") ? true : false;

        this.MyViewsQueryVisibility = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
    }

    // Queries
    LoadQueriesCounts() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetTicketsCounts(this.OwnerId, this.EmployeeGroupId).subscribe(myResult => {
            if (myResult != null) {
                this.AllOpenCount = myResult.MyOpenDataCount > 1000 ? "1000+" : myResult.MyOpenDataCount.toString();
                this.SLAFailureCount = myResult.SLA_Failures > 1000 ? "1000+" : myResult.SLA_Failures.toString();
                this.AllUnassignedTicketCount = myResult.Unassigned_Tickets > 1000 ? "1000+" : myResult.Unassigned_Tickets.toString();
                this.RecentlyUpdatedCount = myResult.RecentlyUpdated_Tickets > 1000 ? "1000+" : myResult.RecentlyUpdated_Tickets.toString();
            }
        });
    }
    filterAgrs: ApiQueryFilters;
    ViewTicketQuery(queryCode: string) {
        if (queryCode != null) {

            var objectTableName = "Ticket";
            var displayTitle = "";
            var backButtonTitle = "Tickets";
            this.filterAgrs = new ApiQueryFilters();

            if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                this.filterAgrs.addAdditionalFilter("OwnerId", this.OwnerId, null, null, "Equals", false, false, false, "string");
            }

            if (!AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
                this.filterAgrs.addAdditionalFilter("EmployeeGroupId", this.EmployeeGroupId, null, null, "Equals", false, false, false, "string");
            }
            switch (queryCode) {
                case "Open:Unassigned":
                    {
                        displayTitle = "Unassigned Tickets";
                        queryCode = "Unassigned Tickets";
                        break;
                    }

                case "Open:SLAFailure":
                    {
                        displayTitle = "SLA Open Failures";
                        queryCode = "SLA Failures";
                        break;
                    }

                case "Open:All":
                    {
                        displayTitle = "All Open Tickets";
                        queryCode = "All Open Tickets";
                        break;
                    }

                case "Open:ReOpen":
                    {
                        break;
                    }

                case "Open:RecentlyUpdated":
                    {
                        displayTitle = "Recently Updated";
                        queryCode = "Recently Updated Tickets";
                        break;
                    }

                case "Others:AllTickets":
                    {
                        displayTitle = "All Tickets";
                        queryCode = "All Tickets";
                        break;
                    }

                case "Solved:SLAFailure":
                    {
                        displayTitle = "SLA Solved\\Closed Failures";
                        queryCode = "Solved with SLA Failures";
                        break;
                    }

                case "Solved:Solved":
                    {
                        displayTitle = "All Solved\\Closed Tickets";
                        queryCode = "Solved Tickets";
                        break;
                    }

                case "Others:AllCancelled":
                    {
                        displayTitle = "Cancelled Tickets";
                        queryCode = "All Cancelled Tickets";
                        break;
                    }
            }

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = "Ticket";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }
    OnBackFromList() {
        this.LoadRecentTickets();
    }

    // Recent Tickets
    public RecentTicketsCount: number = 0;
    public RecentTicketsList: TicketList[] = [];
    LoadRecentTickets() {
        var crmService: CRMDomainService = new CRMDomainService();
        crmService.GetRecentTickets(null, null).subscribe(myResult => {
            if (myResult == null) {
                this.RecentTicketsList = [];
                this.RecentTicketsCount = 0;
            }
            else {
                this.RecentTicketsList = myResult;
                this.RecentTicketsCount = myResult.length;
            }
        });
    }

    //Edit Ticket 
    EditTicket(entity: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Ticket', BackButtonLabel: 'Tickets' });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.OnBackFromEdit();
                    this.LoadAllScreenData();
                });
            });
    }
    OnBackFromEdit() {
        SessionLocator.ClearExternalParams();
    }

    // Top Tickets 
    public TopTicketsCount: number = 0;
    public TopTicketsList: TopTicketClass[] =[];
    private myTicketListService: TicketListService;
    public SeverityVisibility: boolean = false;
    public RankVisibility: boolean = false;
    public ActivityWatchVisibility: boolean = false;
    public UpdateDateVisibility: boolean = false;
    public CreateDateVisibility: boolean = false;
    LoadTopTickets() {
        this.TopTicketsList = [];
        if (this.myTicketListService == null) {
            this.myTicketListService = new TicketListService();
        }

        var filters = new ApiQueryFilters();
        var sortingCol: string;
        var sortingDir: string;


        var myOwnerId = null;
        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;
        }

        var myEmployeeGroupId = null;
        if (!AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
            myEmployeeGroupId = this.EmployeeGroupId;
        }

        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");

        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
            filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
            filters.addAdditionalFilter("EmployeeGroupId", myEmployeeGroupId, null, null, "Equals", false, false, false, "string");
        }

        this.SeverityVisibility = false;
        this.RankVisibility = false;
        this.ActivityWatchVisibility = false;
        this.UpdateDateVisibility = false;

        switch (this.topTicketsComboListSelectedItem.Code) {
            case "SEV":
                {
                    sortingCol = "SeverityPriority";
                    sortingDir = "Ascending";
                    this.SeverityVisibility = true;
                    break;
                }
            case "CRK":
                {
                    sortingCol = "RankCode";
                    sortingDir = "Descending";
                    this.RankVisibility = true;
                    break;
                }

            case "AWT":
                {
                    sortingCol = "ActivityWatch";
                    sortingDir = "Descending";
                    this.ActivityWatchVisibility = true;
                    break;
                }

            case "UPD":
                {
                    sortingCol = "UpdateDate";
                    sortingDir = "Descending";
                    this.UpdateDateVisibility = true;
                    break;
                }
        }

        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        //filters.GetCount = false;
        this.myTicketListService.getByFilters(filters).subscribe(myResult => {
            this.TopTicketsList = [];
            this.TopTicketsCount = 0;

            var baselist: TicketList[] = myResult.Result;
            this.TopTicketsCount = baselist.length;
            switch (this.topTicketsComboListSelectedItem.Code) {
                case "SEV":
                    {
                        baselist.forEach(item => {
                            this.TopTicketsList.push(new TopTicketClass(item, this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }

                case "CRK":
                    {
                        baselist.sort((a, b) => { return (a.RankCode === b.RankCode) ? 0 : (a.RankCode < b.RankCode) ? -1 : 1 }).forEach(item => {
                            this.TopTicketsList.push(new TopTicketClass(item, this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }

                case "AWT":
                    {
                        baselist.sort((a, b) => { return (a.ActivityWatch === b.ActivityWatch) ? 0 : a.ActivityWatch ? -1 : 1}).forEach(item => {
                            this.TopTicketsList.push(new TopTicketClass(item, this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }

                case "UPD":
                    {
                        var tempList: TicketList[] = baselist.filter(d => d.UpdatedByUserId != SessionLocator.LoggedUserId);
                        tempList.sort((a, b) => { return (DateTool.GetDateFromDate(a.UpdateDate) === DateTool.GetDateFromDate(b.UpdateDate)) ? 0 : (DateTool.GetDateFromDate(a.UpdateDate) > DateTool.GetDateFromDate(b.UpdateDate)) ? -1 : 1 }).forEach(item => {
                            this.TopTicketsList.push(new TopTicketClass(item, this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }
            }
        });
    }
    LoadOpenTicketsDueTimeData() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetOpenTicketsByDueTime(this.OwnerId, this.EmployeeGroupId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                if (myResponse.Result.length == 0) {
                    this.OpenTicketsDueTimeExistance = false;
                    try {
                        var elm = document.getElementById(this.OpenTicketsDueTimeId);
                    }
                    catch (er) { }
                    elm.innerHTML = "";
                }
                else {
                    this.OpenTicketsDueTimeExistance = true;
                    this.FillOpenTicketsDueTimeList(myResponse.Result);
                }                
            }
        });
    }

    FillOpenTicketsDueTimeList(List: Array<ChartingDataClass>) {


        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        var StringArr: Array<string> = new Array<string>();
        var j = 0;
        List = List.filter(element => element.DataTypeCode == "FR" || element.DataTypeCode == "RE");

        List.forEach(element => {
            if (!StringArr.includes(element.LabelProperty)) {
                StringArr.push(element.LabelProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], LabelProperty: [], DateTime: [], EmployeeGroupId: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });

        var Graphs = [];
        var index = 0;
        List.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.LabelProperty == StringArr[i]) {             
                    var k = 0;                  
                    if (element.DataTypeCode == "RE")
                        k = 1;
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(2);

                    NewCustomerYAxis[i].data[k] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].EmployeeGroupId[k] = element.EmployeeGroupId;
                    NewCustomerYAxis[i].DateTime[k] = element.DateTimeProperty;
                    NewCustomerYAxis[i].OwnerIds[k] = element.OwnerId;
                    NewCustomerYAxis[i].LabelProperty[k] = element.LabelProperty;

                    
                    if (!NewCustomerXAxis.includes(element.LabelProperty) && element.LabelProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.LabelProperty);

                    }
                }
            }
        });

        var barChartColors: any[] = [
           

            {
                backgroundColor1: '#DA7B38',
                backgroundColor2: '#ecbd9b',
                borderWidth: 0,
            },
            {
                backgroundColor1: '#487E9F',
                backgroundColor2: '#c8d8e2',
                borderWidth: 0
            },
        ]

        this.NewOpenTicketsDueTime = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        var k = 0;
        NewCustomerYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.NewOpenTicketsDueTime[i] == null) {
                    this.NewOpenTicketsDueTime[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], LabelProperty: [], DateTime: [], EmployeeGroupId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.NewOpenTicketsDueTime[i].data.push(element.data[i]);
                this.NewOpenTicketsDueTime[i].BindingElement.push(element.BindingElement[i]);
                this.NewOpenTicketsDueTime[i].OwnerIds.push(element.OwnerIds[i]);
                this.NewOpenTicketsDueTime[i].EmployeeGroupId.push(element.EmployeeGroupId[i]);
                this.NewOpenTicketsDueTime[i].DateTime.push(element.DateTime[i]);
                this.NewOpenTicketsDueTime[i].label = element.label;  
                this.NewOpenTicketsDueTime[i].LabelProperty.push(element.LabelProperty[i]);

                
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "lineAlpha": 0,
                        "id": "AmGraph-1" + i,
                        "title": element.BindingElement[i] + "",
                        "type": "column",
                        "valueField": "col" + (i + 1),
                        // "bulletBorderColor": "#FFFFFF",
                        "fillColors": [barChartColors[i].backgroundColor1 + "", barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                        //   "plotAreaFillColors": ["#ff0000", "#f1783e", "#00ff00"],
                    };
                }
                       
                objectArray[i] = (element.data[i]);

            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0], "col2": objectArray[1] };
            index++;
            k++;
        });

        var InProgressBookingDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();
        try {
            if (NewCustomerXAxis.length != 0) {

                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;

                }
                makeAmBarChart(this.OpenTicketsDueTimeId, Graphs, DataProvider, maximum, null, null, 0, false);
            }

        }
        catch (e) {

        }





    }
    public TopTicketsComboList: CodeNameClass[];
    BuildTopTicketsFilters() {
        this.TopTicketsComboList = [];
        this.TopTicketsComboList.push(new CodeNameClass("SEV", "Severity"));
        this.TopTicketsComboList.push(new CodeNameClass("CRK", "Customer Rank"));
        this.TopTicketsComboList.push(new CodeNameClass("AWT", "Customer Watch"));
        this.TopTicketsComboList.push(new CodeNameClass("UPD", "Update Date"));
        var defaultFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TopTickets);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "SEV";
        }
        this.topTicketsComboListSelectedItem = this.TopTicketsComboList.filter(d => d.Code == defaultFilterCode)[0];
    };

    private topTicketsComboListSelectedItem: CodeNameClass;
    get TopTicketsComboListSelectedItem(){ return this.topTicketsComboListSelectedItem; }
    set TopTicketsComboListSelectedItem(value: CodeNameClass)
    {
        if (this.topTicketsComboListSelectedItem != value) {
            this.topTicketsComboListSelectedItem = value;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TopTickets, (value == null ? null : value.Code));
            this.LoadTopTickets();
        }
    }

    // New Button 
    RunNewTicketWizard() {
        this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe((response: any) => {
            var windowTitle = "New Ticket";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 700;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewTicketWindowClosed($event));
            logWindow.Show('./CRMModules/CRMTickets/Components/NewEntity/NewTicketComponent');
        });
    };
    OnNewTicketWindowClosed(arg: any) {
        if (SessionLocator.IsExternalParams) {
            SessionLocator.ClearExternalParams();
        }

        if (arg == 'OK') {
            this.LoadAllScreenData();
        }
    }
    RunTicketListByShipment() {
        if (this.IsReady) {
            var filters = new ApiQueryFilters();
            if (!AppTool.IsNullOrEmpty(this.ShipmentId)) {
                filters.addAdditionalFilter("ShipmentId", this.ShipmentId, null, null, "Equals", false, false, false, "string");
            }

            if (!AppTool.IsNullOrEmpty(this.CompanyId)) {
                filters.addAdditionalFilter("CompanyId", this.CompanyId, null, null, "Equals", false, false, false, "string");
            }

            var listArgs = new ListComponentArgs();
            listArgs.Filters = filters;
            listArgs.QueryCode = "All Tickets";
            listArgs.ObjectTableName = "Ticket";
            listArgs.DisplayTitle = "All Tickets By Shipment";
            listArgs.BackButtonTitle = "Tickets";
            this._entityResourceService.getEntityResourceByTableName("General", 0).subscribe(response => {
                this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.QueryBackClicked());
                            this.CurrentSession.AddMenuReference(cmpRef);
                            SessionLocator.ClearExternalParams();
                        });
                });
            });
        }
    }
    QueryBackClicked() {
        SessionLocator.ClearExternalParams();
        this.LoadAllScreenData();
    }

    // Filters 
    public EmployeeGroupFilterList: CodeNameClass[] = [];
    public EmployeeGroupFilterListPM: EmployeeGroupPM[] = [];
    public UsersFilterList: CodeNameClass[] = [];
    private  filterName_Owner = "Owner";
    private  filterControlNameSpace = "Logitude.CRM.Views.CRMPages.TicketsPageControl";
    private filterName_EmployeeGroup = "EmployeeGroup";
    private  filterName_TopTickets = "TopTickets";

    private BuildEmployeeGroupFilterList() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetEmployeeGroupsPMList().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: EmployeeGroupPM[] = myResponse.Result;
                this.EmployeeGroupFilterListPM = list;
                this.EmployeeGroupFilterList = [];
                this.EmployeeGroupFilterList.push(new CodeNameClass("M", "My Records"));

                if (list) {
                    list.filter(d => d.Id != SessionLocator.Tenant.toString()).forEach((item) => {
                        this.EmployeeGroupFilterList.push(new CodeNameClass(item.Id, item.Name));
                    });
                }

                this.EmployeeGroupFilterList.push(new CodeNameClass("A", "All Records"));

                var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_EmployeeGroup);
                if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                    defaultFilterCode = "M";
                }

                this.selectedEmployeeGroupFilter = this.EmployeeGroupFilterList.filter(d => d.Code == defaultFilterCode)[0];
                this.GetSelectedEmployeeGroup();
                this.BuildUsersFilters(false);
            }
        });
    }

    private BuildUsersFilters(isUpdatingFilter: boolean) {
        this.UsersFilterList = [];

        if (this.SelectedEmployeeGroupFilter == null) {
            this.selectedUserFilter = null;
            this.GetSelectedOwnerId();

            if (isUpdatingFilter) {
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            }

            this.LoadFilteredQueries();
        }

        else {
            switch (this.SelectedEmployeeGroupFilter.Code) {
                case "M":
                    {
                        var item = new CodeNameClass(SessionLocator.LoggedUserId, SessionLocator.LoggedUserPM.EnglishName);
                        this.UsersFilterList.push(item);
                        this.selectedUserFilter = item;
                        this.GetSelectedOwnerId();

                        if (isUpdatingFilter) {
                            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }

                        this.LoadFilteredQueries();
                        break;
                    }

                case "A":
                    {
                        var item = new CodeNameClass("A", "All Owners");
                        this.UsersFilterList.push(item);

                        if (isUpdatingFilter) {
                            //this.OwnerId = null;
                            //this.listOfValuesUserId = null;
                            //this.selectedUserFilter = null;
                            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }

                        else {
                            this.selectedUserFilter = item;
                            this.GetSelectedOwnerId();
                        }

                        this.LoadFilteredQueries();
                        break;
                    }

                default:
                    {
                        this.UsersFilterList = [];
                        var item = new CodeNameClass("A", "All " + this.SelectedEmployeeGroupFilter.Name + " Owners");
                        this.UsersFilterList.push(item);

                        var selectedEmployee: EmployeeGroupPM = this.EmployeeGroupFilterListPM.filter(a => a.Id == this.SelectedEmployeeGroupFilter.Code)[0];
                        var employeeGroupLinesdIds: string[] = [];
                        selectedEmployee.EmployeeGroupLines.forEach(item => {
                            employeeGroupLinesdIds.push(item.UserId);
                        });

                        var myIds = this.GetIdsString(employeeGroupLinesdIds);
                        var loadedUsersList: UserList[] = [];

                        var service: CRMDomainService = new CRMDomainService();
                        if (employeeGroupLinesdIds != null && employeeGroupLinesdIds.length > 0) {
                            service.GetUsersByEmployeeGroupIds(myIds).subscribe(myResult => {
                                var myResponse: ServiceResponse = myResult;
                                if (!myResponse.HasError) {
                                    loadedUsersList = myResponse.Result.sort((a, b) => { return (a.EnglishName.toLowerCase() === b.EnglishName.toLowerCase()) ? 0 : (a.EnglishName.toLowerCase() < b.EnglishName.toLowerCase()) ? -1 : 1 }); 
                                    if (loadedUsersList != null) {
                                        loadedUsersList.forEach((item) => {
                                            var record: CodeNameClass = new CodeNameClass();
                                            record.Code = item.Id;
                                            record.Name = item.EnglishName;
                                            this.UsersFilterList.push(record);
                                        });
                                    }
                                }

                                if (isUpdatingFilter) {
                                    this.GetSelectedOwnerId();
                                    this.listOfValuesUserId = null;
                                    this.selectedUserFilter = item;
                                    LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                                }

                                else {
                                    var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                                    if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                                        defaultFilterCode = null;
                                    }

                                    this.OwnerId = defaultFilterCode;
                                    this.listOfValuesUserId = this.OwnerId;

                                    if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                                        this.selectedUserFilter = this.UsersFilterList.filter(d => d.Code == this.OwnerId)[0];
                                    }

                                    if (this.selectedUserFilter == null) {
                                        this.selectedUserFilter = this.UsersFilterList[0];
                                    }
                                }

                                this.LoadFilteredQueries();

                            });
                        }

                        break;
                    }
            }
        }
    }
    GetSelectedEmployeeGroup() {
        var myResult: string = null;

        if (this.SelectedEmployeeGroupFilter) {
            switch (this.SelectedEmployeeGroupFilter.Code) {
                case "M": {
                    myResult = null;
                    break;
                }

                case "A": {
                    myResult = null;
                    break;
                }

                default: {
                    myResult = this.SelectedEmployeeGroupFilter.Code;
                    break;
                }
            }
        }

        this.EmployeeGroupId = myResult;
    }
    GetSelectedOwnerId() {
        var myResult = null;
        this.listOfValuesUserId = null;

        if (this.SelectedUserFilter) {
            switch (this.SelectedUserFilter.Code) {
                case "A": {
                    var defaultFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                    if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                        defaultFilterCode = null;
                    }

                    myResult = defaultFilterCode;
                    this.listOfValuesUserId = myResult;
                    break;
                }

                default: {
                    myResult = this.SelectedUserFilter.Code;
                    this.listOfValuesUserId = myResult;
                    break;
                }
            }
        }

        this.OwnerId = myResult;
    }

    private selectedEmployeeGroupFilter: CodeNameClass;
    get SelectedEmployeeGroupFilter() {
        return this.selectedEmployeeGroupFilter;
    }
    set SelectedEmployeeGroupFilter(newValue: CodeNameClass) {
        if (this.selectedEmployeeGroupFilter != newValue) {
            this.selectedEmployeeGroupFilter = newValue;
            this.GetSelectedEmployeeGroup();
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_EmployeeGroup, (newValue == null ? null : newValue.Code));
            this.BuildUsersFilters(false);
        }
    }

    private OwnerId: string = "";
    private EmployeeGroupId: string ="";
    private selectedUserId: string = "";
    private selectedEmployeeGroupId: string ="";

    private selectedUserFilter: CodeNameClass;
    get SelectedUserFilter() {
        return this.selectedUserFilter;
    }
    set SelectedUserFilter(value: CodeNameClass) {
        if (this.selectedUserFilter != value) {
            this.selectedUserFilter = value;
            this.GetSelectedOwnerId();
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
        }
    }

    private listOfValuesUserId: string;
    get ListOfValuesUserId() {
        return this.listOfValuesUserId;
    }
    set ListOfValuesUserId(value: string) {
        if (this.listOfValuesUserId != value) {
            this.OwnerId = value;
            this.listOfValuesUserId = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.BuildUsersFilters(true);
        }
    }


    get IsUsersFilterEnabled() {
        var myResult = false;

        if (this.SelectedEmployeeGroupFilter != null) {
            if (this.SelectedEmployeeGroupFilter.Code != "M" && this.SelectedEmployeeGroupFilter.Code != "A") {
                myResult = true;
            }
        }

        return myResult;
    }
    get IsListOfValuesVisible() {
        var myResult = false;

        if (this.SelectedEmployeeGroupFilter != null) {
            if (this.SelectedEmployeeGroupFilter.Code == "A") {
                myResult = true;
            }
        }

        return myResult;
    }
    private GetIdsString(ids: string[]) {
        var myResult = "";

        ids.forEach(Id => {
            if (AppTool.IsNullOrEmpty(myResult)) {
                myResult = Id;
            }

            else {
                myResult += ":" + Id;
            }
        });

        return myResult;
    }



    // My Views
    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }
    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }
}

class TopTicketClass {
    private filter: CodeNameClass;
    public entityList: TicketList;
    constructor(entityList: TicketList, filter: CodeNameClass) {
        this.entityList = entityList;
        this.filter = filter;
        this.setVisibilityByCode();
        this.GetSeverityBrush();
    }

    get Id() { return this.entityList.Id; }
    get Subject() { return this.entityList.Subject; }
    get CompanyName() { return this.entityList.CompanyName; }
    get ContactName() { return this.entityList.ContactName; }
    get SeverityName() { return this.entityList.SeverityName; }
    get SeverityCode() { return this.entityList.SeverityCode; }
    get FilterLable() { return this.filter.Name + ": "; }
    get CreateDate() { return this.entityList.CreateDate; }
    get UpdateDate() { return this.entityList.UpdateDate; }
    get OwnerName() { return this.entityList.OwnerName; }
    get ActivityWatch() { return this.entityList.ActivityWatch; }
    get RankCode() { return this.entityList.RankCode; }
    get TicketNumber() { return this.entityList.TicketNumber; }
    get UpdatedByUserName() { return this.entityList.UpdatedByUserName; }

    public SeverityBrush = "Black";
    GetSeverityBrush() {
        var result = "Black";
        switch (this.entityList.SeverityCode) {
            case "UI": { result = "Red"; break; }
            case "HI": { result = "Orange"; break; }
            case "MD": { result = "Gray"; break; }
            case "LW": { result = "Blue"; break; }
        }
        this.SeverityBrush = result;
    }

    private severityVisibility = false;
    get SeverityVisibility() { return this.severityVisibility; }
    set SeverityVisibility(value: boolean) {
        this.severityVisibility = value;
    }

    private rankVisibility = false;
    get RankVisibility() { return this.rankVisibility; }
    set RankVisibility(value: boolean) {
        this.rankVisibility = value;
    }

    private activityWatchVisibility = false;
    get ActivityWatchVisibility() { return this.activityWatchVisibility; }
    set ActivityWatchVisibility(value: boolean) {
        this.activityWatchVisibility = value;
    }

    private updateDateVisibility = false;
    get UpdateDateVisibility() { return this.updateDateVisibility; }
    set UpdateDateVisibility(value: boolean) {
        this.updateDateVisibility = value;
    }

    get CreateDateVisibility() {
        var myResult = true;
        if (this.UpdateDateVisibility == true) {
            myResult = false;
        }
        return myResult;
    }

    private setVisibilityByCode() {
        this.SeverityVisibility = false;
        this.RankVisibility = false;
        this.ActivityWatchVisibility = false;
        this.UpdateDateVisibility = false;

        switch (this.filter.Code) {
            case "SEV":
                {
                    this.SeverityVisibility = true;
                    break;
                }

            case "CRK":
                {
                    this.RankVisibility = true;
                    break;
                }

            case "AWT":
                {
                    if (this.ActivityWatch) {
                        this.ActivityWatchVisibility = true;
                    }
                    break;
                }

            case "UPD":
                {
                    this.UpdateDateVisibility = true;
                    break;
                }
        }
    }

    get RankName() {
        var myResult: string = null;
        if (this.entityList != null) {
            myResult = this.entityList.RankCode;
        }

        return myResult;
    }
    get RankSource1() {
        var myResult: string = null;
        // silver to lower
        if (this.entityList != null) {
            var RankCode = this.entityList.RankCode;
            switch (RankCode) {
                case "1":
                case "2":
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }
                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }
    get RankSource2() {
        var myResult: string = null;

        if (this.entityList != null) {
            var RankCode = this.entityList.RankCode;

            switch (RankCode) {
                case "2":
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }
    get RankSource3() {
        var myResult: string = null;

        if (this.entityList != null) {
            var RankCode = this.entityList.RankCode;

            switch (RankCode) {
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }
}
