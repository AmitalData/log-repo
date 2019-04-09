import {Component, OnDestroy} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateTool, FontTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {OpportunityList} from '../../../../CRM/EntityLists/OpportunityList';
import {OpportunityListService} from '../../../../CRM/Services/StandardLists/OpportunityListService';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {ActivityList} from '../../../../CRM/EntityLists/ActivityList';
import {ActivityListService} from '../../../../CRM/Services/StandardLists/ActivityListService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {OpportunityPMInitService} from '../../../../CRM/EntityPMInitServices/OpportunityPMInitService';
import {OpportunityArgs, ActivityInputArgs} from '../../../../CRM/Args'; 
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {QuoteList} from '../../../../Quote/EntityLists/QuoteList';
import {QuoteListService} from '../../../../Quote/Services/StandardLists/QuoteListService';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuotePMInitService} from '../../../../Quote/EntityPMInitServices/QuotePMInitService';
import {NewQuoteComponentArgs} from '../../../../Quote/Args';
import {TicketList} from '../../../../CRM/EntityLists/TicketList';
import {TicketListService} from '../../../../CRM/Services/StandardLists/TicketListService';
import {TicketPM} from '../../../../CRM/EntityPMs/TicketPM';
import {NewTicketArgs} from '../../../../CRM/Args';
import {CRMTool} from '../../../../CRM/Tools';
import {GeneralEmailSender} from '../../../../Infrastructure/Helpers/GeneralEmailSender';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerSalesTabComponent.html',
})

export class CustomerSalesTabComponent implements OnDestroy {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public OpportunityObsList: OpportunityData[] = [];
    public ActivityObsList: ActivityData[] = [];
    public QuoteObsList: QuoteData[] = [];
    public TicketsList: TicketData[] = [];
    public IsTicketTabDim: boolean = false;
    RegardingEntity: string = "";
    EntityId: string = "";
    EntityDescription: string = "";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.InitializeServices();
        this.EntityId = this.EntityPM.Id;
        this.EntityDescription = this.EntityPM.EnglishName;
        this.RegardingEntity = "Regarding Customer : " + this.EntityPM.Code + " " + this.EntityPM.EnglishName;
        
        if (FeatureLocator.HasFeaturePermession("General", "TICKET")) {
            this.IsTicketTabDim = true;
        }

        this.BuildScreenData();
        this.getToolTip();
        this.Listen();
        this.SetUIProperties();


    }

    public EnabledNewQuote: boolean = false;
    public EnableNewOpportunity: boolean = false;
    public EnableNewActivity: boolean = false;

    SetUIProperties() {      
        if (FeatureLocator.HasFeaturePermession("Quote", "NEW") && FeatureLocator.HasFeaturePermession("Quote", "NEWQUOTE")) {
            this.EnabledNewQuote = true;
        }

        if (FeatureLocator.HasFeaturePermession("Opportunity", "NEW")) {
            this.EnableNewOpportunity = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "NEW")) {
            this.EnableNewActivity = true;
        }


    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "LoadActivity") {
                    this.GetActivities();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private OpportunityListService: OpportunityListService;
    private ActivityListService: ActivityListService;
    private QuoteListService: QuoteListService;
    private TicketListService: TicketListService;
    private InitializeServices() {
        this.OpportunityListService = new OpportunityListService();
        this.ActivityListService = new ActivityListService();
        this.QuoteListService = new QuoteListService();
        this.TicketListService = new TicketListService();
    }
    private BuildScreenData() {
        this.GetOpportunities();
        //this.GetActivities();
        //this.GetQuotes();
        //this.GetTickets();
    }

    public IsVisible = false; 

    //Opportunity
    private GetOpportunities() {
        if (this.OpportunityObsList != null) {
            this.OpportunityObsList = [];
        }

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.OpportunityListService.getByFilters(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                if (response.Result == null) {
                    this.OpportunityObsList = [];
                }

                else {
                    var tempList: OpportunityData[] = [];
                    var tempList_Open: OpportunityData[] = [];
                    var tempList_Clos: OpportunityData[] = [];
                    var baselist: OpportunityList[] = response.Result;  

                    var openList: OpportunityList[] = baselist.filter(d => !d.IsClosed);
                    var closList: OpportunityList[] = baselist.filter(d => d.IsClosed);

                    baselist.filter(d => !d.IsClosed).forEach(item => {
                        tempList_Open.push(new OpportunityData(item, this));
                    });

                    baselist.filter(d => d.IsClosed).forEach(item => {
                        tempList_Clos.push(new OpportunityData(item, this));
                    });
                    
                    tempList_Open = tempList_Open.sort((a, b) => { return (DateTool.GetDateFromDate(a.DisplayDate) === DateTool.GetDateFromDate(b.DisplayDate)) ? 0 : (DateTool.GetDateFromDate(a.DisplayDate) > DateTool.GetDateFromDate(b.DisplayDate)) ? -1 : 1 });
                    tempList_Clos = tempList_Clos.sort((a, b) => { return (DateTool.GetDateFromDate(a.DisplayDate) === DateTool.GetDateFromDate(b.DisplayDate)) ? 0 : (DateTool.GetDateFromDate(a.DisplayDate) > DateTool.GetDateFromDate(b.DisplayDate)) ? -1 : 1 });

                    tempList = tempList_Open.concat(tempList_Clos);
                    tempList.forEach(item => {
                        if (this.OpportunityObsList.length < 5) {
                            this.OpportunityObsList.push(item);
                        }
                    });
                    
                    if (this.OpportunityObsList.length > 0) {
                        this.NoOppData = false;
                    }
                }

                this.GetActivities();             
            }
        });
    }
    private noOppData = true;
    get NoOppData() { return this.noOppData; }
    set NoOppData(value: boolean) { this.noOppData = value; }
    get NoOppDataIsEnabled() { return !this.NoOppData; }

    //Activity
    private GetActivities() {
        if (this.ActivityObsList != null) {
            this.ActivityObsList = [];
        }
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.ActivityListService.getByFilters(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                if (response.Result == null) {
                    this.ActivityObsList = [];
                }
                else {
                    var dataResult: ActivityList[] = response.Result;
                    if (dataResult != null && dataResult.length > 0) {
                        var myTempList: ActivityList[] = [];

                        dataResult.filter(d => d.IsOpen == true && d.DueDate != null).sort((a, b) => { return (DateTool.GetDateFromDate(a.DueDate) === DateTool.GetDateFromDate(b.DueDate)) ? 0 : (DateTool.GetDateFromDate(a.DueDate) < DateTool.GetDateFromDate(b.DueDate)) ? 1 : -1 }).forEach(item => {
                            myTempList.push(item);
                        });
                        dataResult.filter(d => d.IsOpen == true && d.DueDate == null).sort((a, b) => { return (DateTool.GetDateFromDate(a.StartDateTime) === DateTool.GetDateFromDate(b.StartDateTime)) ? 0 : (DateTool.GetDateFromDate(a.StartDateTime) < DateTool.GetDateFromDate(b.StartDateTime)) ? 1 : -1 }).forEach(item => {
                            myTempList.push(item);
                        });
                        dataResult.filter(d => d.IsOpen == false).sort((a, b) => { return (DateTool.GetDateFromDate(a.CompleteDate) === DateTool.GetDateFromDate(b.CompleteDate)) ? 0 : (DateTool.GetDateFromDate(a.CompleteDate) < DateTool.GetDateFromDate(b.CompleteDate)) ? 1 :-1 }).forEach(item => {
                            myTempList.push(item);
                        });

                        myTempList.forEach(item => {
                            if (this.ActivityObsList.length < 5) {
                                this.ActivityObsList.push(new ActivityData(item, this));
                            }
                        });

                        if (this.ActivityObsList.length > 0) {
                            this.NoActData = false;
                        }
                    }
                }

                this.GetQuotes();
               
            }
        });
    }
    private noActData = true;
    get NoActData() { return this.noActData; }
    set NoActData(value: boolean) { this.noActData = value; }
    get NoActDataIsEnabled() { return !this.NoActData; }

    // Quote
    private GetQuotes() {
        if (this.QuoteObsList != null) {
            this.QuoteObsList = [];
        }
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "OpenDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.QuoteListService.getByFilters(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                if (response.Result == null) {
                    this.QuoteObsList = [];
                }
                else {
                    var baselist: QuoteList[] = response.Result;
                    baselist.sort((a, b) => { return (DateTool.GetDateFromDate(a.OpenDate) === DateTool.GetDateFromDate(b.OpenDate)) ? 0 : (DateTool.GetDateFromDate(a.OpenDate) > DateTool.GetDateFromDate(b.OpenDate)) ? -1 : 1 }).forEach(item => {
                        if (this.QuoteObsList.length < 5) {
                            this.QuoteObsList.push(new QuoteData(item, this));
                        }
                    });

                    if (this.QuoteObsList.length > 0) {
                        this.NoQutData = false;
                    }
                }

                this.GetTickets();
            }
        });
    }
    private noQutData = true;
    get NoQutData() { return this.noQutData; }
    set NoQutData(value: boolean) { this.noQutData = value; }
    get NoQutDataIsEnabled(){ return !this.NoQutData; }

    // Ticket
    private GetTickets() {
        if (this.TicketsList != null) {
            this.TicketsList = [];
        }
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("CompanyId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.TicketListService.getByFilters(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                if (response.Result == null) {
                    this.TicketsList = [];
                }
                else {
                    var baselist: TicketList[] = response.Result;
                    if (baselist != null && baselist.length > 0) {
                        baselist.sort((a, b) => { return (DateTool.GetDateFromDate(a.CreateDate) === DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (DateTool.GetDateFromDate(a.CreateDate) > DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1 }).forEach(item => {
                            if (this.TicketsList.length < 5) {
                                this.TicketsList.push(new TicketData(item, this));
                            }
                        });

                        if (this.TicketsList.length > 0) {
                            this.NoTicketData = false;
                        }
                    }

                    this.IsVisible = true;
                }
            }
        });
    }
    private  noTicketData = true;
    get NoTicketData() { return this.noTicketData; }
    set NoTicketData(value:boolean)
    { 
        this. noTicketData = value;
    }
    get IsTicketDataEnabled(){ return !this.NoTicketData; }

    // Watch
    public WatchToolTip = "";
    private getToolTip() {
        if (this.ActivityWatch) {
            this.WatchToolTip = "Disable Activity Watch";
        }
        else {
            this.WatchToolTip = "Enable Activity Watch";
        }
    }
    public get ActivityWatch() {
        var d = this.entityArgs.EditComponent.ComponentId;
        return this.EntityPM.ActivityWatch;
    }
    public set ActivityWatch(value: boolean)
    {
        this.EntityPM.ActivityWatch = value;
        this.getToolTip();
    }

    SetActivity(value: boolean) {
        this.ActivityWatch = value;
    }

    // Commands 
    private refresh = "";
    Add(m: string) {
        var title = "";
        var path = "";
        var logWindow = new LogitudeWindow();
        var objectTableName = "";
        var args = null;
        switch (m) {
            case "OPP":
                {
                    path = "./CRMModules/CRMOpportunity/Components/NewEntity/NewOpportunityComponent";
                    title = "New Opportunity";
                    this.refresh = "opp";
                    var opp: OpportunityPM = new OpportunityPM();
                    OpportunityPMInitService.InitValues(opp, true);
                    opp.CustomerId = this.EntityPM.Id;
                    args = new OpportunityArgs();
                    args.Entity = opp;
                    args.IsNew = true;
                    args.IsAddCustomerVisible = false;
                    logWindow.WindowArgs = args;
                    objectTableName = "Opportunity";
                    logWindow.Width = 960;
                    logWindow.Height = 570;
                    break;
                }
            case "QUT":
                {
                    path = "./Quote/Components/NewEntity/NewQuoteComponent";
                    title = "New Quote";
                    this.refresh = "qut";
                    objectTableName = "Quote";
                    //var quotePM: QuotePM = new QuotePM();
                    //QuotePMInitService.InitValues(quotePM, true);
                    //quotePM.CustomerId = this.EntityPM.Id;
                    //quotePM.CustomerName = this.EntityPM.EnglishName;
                    //quotePM.CustomerNote = this.EntityPM.Notes;
                    //quotePM.CustomerRankName = this.EntityPM.RankName;
                    //quotePM.IsCustomerSet = true;
                    //args.Entity = quotePM;
                    args = new NewQuoteComponentArgs();
                    args.DefaultCustomerId = this.EntityPM.Id;
                    logWindow.WindowArgs = args;
                    logWindow.Width = 960;
                    logWindow.Height = 570;
                    break;
                }
            case "CAS":
                {
                    break;
                }
            case "CMT":
                {
                    break;
                }
            case "ACT":
                {
                    break;
                }
            case "TKT":
                {
                    path = "./CRMModules/CRMTickets/Components/NewEntity/NewTicketComponent";
                    title = "New Ticket";
                    this.refresh = "tkt";
                    objectTableName = "Ticket";
                    args = new NewTicketArgs();
                    args.CompanyId = this.EntityPM.Id;
                    logWindow.WindowArgs = args;
                    logWindow.Width = 850;
                    logWindow.Height = 700;
                    break;
                }
        }

        this._entityResourceService.getEntityResourceByTableName(objectTableName, 0).subscribe(response => {
            logWindow.Title = title;
            logWindow.Show(path);
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.Refresh();
                }
            });
        });
    }
    private Refresh() {
        if (this.refresh == "act") {
            this.GetActivities();
        }
        else if (this.refresh == "opp") {
            this.GetOpportunities();
        }
        else if (this.refresh == "qut") {
            this.GetQuotes();
        }
        else if (this.refresh == "tkt") {
            this.GetTickets();
        }
    }
    ViewEntity(tableName: string, entityId: string) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, BackButtonLabel: "Customer" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.BuildScreenData();
                });
            });
    }
    ViewAllData(m: string) {
        var objectTableName = "";
        var queryCode = "";
        var filterName = "CustomerId";
        var filterAgrs = new ApiQueryFilters();
        switch (m) {
            case "OPP":
                {
                    objectTableName = "Opportunity";
                    queryCode = "All Opportunities";
                    break;
                }
            case "ACT":
                {
                    objectTableName = "Activity";
                    queryCode = "All Activities";
                    break;
                }
            case "QUT":
                {
                    objectTableName = "Quote";
                    queryCode = "All Quotes";
                    break;
                }

            case "TKT":
                {
                    objectTableName = "Ticket";
                    queryCode = "All Tickets";
                    filterName = "CompanyId";
                    break;
                }
        }

        filterAgrs.addAdditionalFilter(filterName, this.EntityPM.Id, null, null, "Equals", false, false, false, "String");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = listArgs.QueryCode;
        listArgs.BackButtonTitle = "Back";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                });
        });
    }
    NewEntity(code: string) {
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow();
        switch (code) {
            case "TS":
                {
                    windowTitle = "New Task";
                    windowTitleIcon = "./Images/Activities/TS.png";
                    break;
                }

            case "CL": {
                windowTitle = "New Phone Call";
                windowTitleIcon = "./Images/Activities/CL.png";
                break;
            }

            case "AP": {
                windowTitle = "New Appointment";
                windowTitleIcon = "./Images/Activities/AP.png";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }

            default: { break; }
        }
        this.refresh = "act";
        var windowArgs: ActivityInputArgs = new ActivityInputArgs();
        windowArgs.TypeCode = code;
        windowArgs.IsAddCustomerAllowed = false;
        windowArgs.CustomerId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;

        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.Refresh();
                }
            });
        });
    }


    EmailSender: GeneralEmailSender;
    AddEmailActivity() {
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender("Customer", "CUST", this.EntityPM.Id, "", "", "", "", "", null, "LoadActivity", this.EntityPM, true, "CEMO");
            this.EmailSender.ShowFullSendControll();
        }
    }
}

export class OpportunityData {

    private entityPM: OpportunityList;
    constructor(item: OpportunityList, public father: CustomerSalesTabComponent) {
        this.entityPM = item;
        this.GetListBoxBackground();
        this.ComputeDisplayDate();
    }

    get Id() { return this.entityPM.Id; }
    get RatingCode() { return this.entityPM.RatingCode; }
    get RatingName() { return this.entityPM.RatingName; }
    get Topic() { return this.entityPM.Subject; }
    get StageName() { return this.entityPM.StageName; }
    get NumberOfShipments() { return this.entityPM.NumberOfShipments; }
    get IsClosed() { return this.entityPM.IsClosed; }
    get UpdateDate() { return this.entityPM.UpdateDate; }

    public DisplayDate: Date;
    ComputeDisplayDate() {
        var date = this.entityPM.LastStageDate;

        if (this.entityPM.IsClosed) {
            date = this.entityPM.ActualClosingDate;
        }

        this.DisplayDate = date;
    }

    public ListBoxBackground;
    GetListBoxBackground() {
        var result = "rgb(255, 255, 255)";
        if (this.IsClosed) {
            result  = "rgba(0,0,0,0.1)";
        }
        this.ListBoxBackground = result;
    }
}
export class ActivityData {
    private entityPM: ActivityList;
    constructor(item: ActivityList, public father: CustomerSalesTabComponent) {
        this.entityPM = item;
        this.GetDueDateForeground();
        this.GetListBoxBackground();
        this.GetModifiedOrCompleted();
        this.ImageSrc = CRMTool.GetActivityImageSrc(this.entityPM.ActivityTypePathCode);
    }

    get Id() { return this.entityPM.Id; }
    get Subject() { return this.entityPM.Subject; }
    get DueDate() { return this.entityPM.DueDate; }
    get ActivityTypePathCode() { return this.entityPM.ActivityTypePathCode; }
    get ActivityTypeName() { return this.entityPM.ActivityTypeName; }
    get IsOpen() { return this.entityPM.IsOpen; }
    get UpdatedByUserName() { return this.entityPM.UpdatedByUserName; }
    get OwnerName() { return this.entityPM.OwnerName; }
    get UpdateDate() { return this.entityPM.UpdateDate; }
    public ImageSrc: string;

    public DueDateForeground;
    GetDueDateForeground()
    {
        var result = "rgb(40, 46, 48)";
        var now = DateTool.GetCurrentDateTimeAsUtc();
        if (this.DueDate != null && (this.DueDate.valueOf() < now.valueOf())) {
            result = "red";
        }
        this.DueDateForeground = result;
    }

    public ListBoxBackground;
    GetListBoxBackground() {
        var result = "rgb(255,255,255)";
        if (!this.IsOpen){
            result = "rgba(0,0,0,0.1)";
        }
        this.ListBoxBackground = result;
    }

    public ModifiedOrCompleted;
    GetModifiedOrCompleted()
    {
        var myResult = "Modified by";
        if (this.entityPM.ActivityTypeCode == "EI") {
            myResult = "Recorded by";
        }

        else if (this.entityPM.ActivityTypeCode == "EO") {
            myResult = "Sent by";
        }

        else {
            switch (this.entityPM.ActivityStatusCode) {
                case "C":
                    {
                        myResult = "Completed by";
                        break;
                    }

                case "X":
                    {
                        myResult = "Closed by";
                        break;
                    }

                default:
                    {
                        myResult = "Modified by";
                        break;
                    }
            }
        }
        this.ModifiedOrCompleted = myResult;
    }
}
export class QuoteData {
    private entityPM: QuoteList;
    constructor(item: QuoteList, public father: CustomerSalesTabComponent) {
        this.entityPM = item;
        this.GetListBoxBackground();
        this.GetIsClosed();
    }

    get Id() { return this.entityPM.Id; }
    get RatingCode() { return this.entityPM.RatingCode; }
    get RatingName() { return this.entityPM.RatingName; }
    get Subject() { return this.entityPM.Subject; }
    get FromCountryCode() { return this.entityPM.FromCountryCode; }
    get FromPortCountry() { return this.entityPM.FromPortCountry; }
    get ToCountryCode() { return this.entityPM.ToCountryCode; }
    get ToPortCountry() { return this.entityPM.ToPortCountry; }
    get OpenDate() { return this.entityPM.OpenDate; }
    get DirectionId() { return this.entityPM.DirectionId; }
    get DirectionName() { return this.entityPM.DirectionName; }
    get TransportModeId() { return this.entityPM.TransportModeId; }
    get TransportModeName() { return this.entityPM.TransportModeName; }
    get StageName() { return this.entityPM.StageName; }

    public IsClosed = false;
    GetIsClosed() {
        var isClosed = false;
        if (this.entityPM.StageName == "Declined") {
            isClosed = true;
        }
        else if (this.entityPM != null && this.entityPM.ExpirationDate != null && (this.entityPM.ExpirationDate.valueOf() < DateTool.GetCurrentDateAsUtc().valueOf())) {
            if (this.entityPM.StageName != "Accepted") {
                isClosed = true;
            }
        }
        this.IsClosed = isClosed;
    }

    public ListBoxBackground;
    GetListBoxBackground() {
        var result = "rgb(255,255,255)";
        if (this.IsClosed) {
            result = "rgba(0,0,0,0.1)";
        }
        this.ListBoxBackground = result;
    }
}
export class TicketData {
    private entityPM: TicketList;
    constructor(item: TicketList, public father: CustomerSalesTabComponent) {
        this.entityPM = item;
    }
    get Id() { return this.entityPM.Id; }
    get IsClosed() { return this.entityPM.IsClosed; }
    get SeverityName() { return this.entityPM.SeverityName; }
    get SeverityCode() { return this.entityPM.SeverityCode; }
    get Subject() { return this.entityPM.Subject; }
    get TypeName() { return this.entityPM.TypeName; }
    get CompanyName() { return this.entityPM.CompanyName; }
    get StageName() { return this.entityPM.StageName; }
    get ContactName() { return this.entityPM.ContactName; }
    get OwnerName() { return this.entityPM.OwnerName; }
    get TicketNumber() { return this.entityPM.TicketNumber; }
}
