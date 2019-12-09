import {Component, OnInit, ViewChildren, QueryList, AfterViewInit} from '@angular/core';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {TicketPMService} from '../../../../../CRM/Services/StandardPMs/TicketPMService';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CRMDomainService} from '../../../../../CRM/Services/CRMDomainService';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../../../Infrastructure/Utilities/LocationDirective';
import {DetailsTabComponent} from '../MainTab/DetailsTabComponent';
import {ActivitiesTabComponent} from '../MainTab/ActivitiesTabComponent';
import {PostsTabComponent} from '../MainTab/PostsTabComponent';
import {CorrespondencePM} from '../../../../../CRM/EntityPMs/CorrespondencePM';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {DocumentDataPM} from '../../../../../CRM/EntityPMs/DocumentDataPM';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {CRMTool} from '../../../../../CRM/Tools'; 
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {UserList} from '../../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../../Common/Services/StandardLists/UserListService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {TicketClassificationList} from '../../../../../CRM/EntityLists/TicketClassificationList';
import {TicketClassificationListService} from '../../../../../CRM/Services/StandardLists/TicketClassificationListService';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {SendEmailArgs, ActivityInputArgs} from '../../../../../CRM/Args';
import {TicketValidator} from '../../../../../CRM/Validators/TicketValidator';
import {RatingList} from '../../../../../CRM/EntityLists/RatingList';
import {RatingListService} from '../../../../../CRM/Services/StandardLists/RatingListService';
import {ListComponentArgs} from '../../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService'
import {ActivityList} from '../../../../../CRM/EntityLists/ActivityList';
import {DateTimePipe} from '../../../../../Controls/Pipes/DateTimePipe';
import {CommunicationLogPM} from '../../../../../Common/EntityPMs/CommunicationLogPM';
import {CommunicationLogPMService} from '../../../../../Common/Services/StandardPMs/CommunicationLogPMService';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ContactList} from '../../../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../../../Common/Services/StandardLists/ContactListService';
declare var window: any;


@Component({
    selector: 'MainTabComponent',
    moduleId: module.id,
    templateUrl: './TicketMainTabComponent.html',
})

export class TicketMainTabComponent extends BaseComponent implements OnInit, AfterViewInit {
    public EntityPM: TicketPM;
    public ObjectTableName: string;
    public DataContext: TicketMainTabComponent = this;
    public LinkColor = "#1E4AC4";
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public TicketCommunicationLogs: CommunicationLogPM[] = [];
    private ContactListService: ContactListService; 
    EntityId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public  _entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.EntityId = this.EntityPM.Id;
        }
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.TicketCommunicationLogs = [];
        this.ContactListService = new ContactListService();
        this.Listen();
        this.SetUIProperties();
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    //this.ReloadEntityPM();
                    if (this.PageChild_DS != null) {
                        this.PageChild_DS.RefreshTab(this);
                    }
                    if (this.IsRefreshButton) {
                        this.IsRefreshButton = false;
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    if (this.IsReload) {
                        this.IsReload = false;
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    this.ReloadhData();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.ReloadhData();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "TIMN") {
                    if (this.PageChild_DS != null) {
                        this.PageChild_DS.GetEntityLinkNumberVisibility();
                        this.PageChild_DS.SetUIProperties();
                    }
                }

            });
        }
    }

    ReloadEntityPM() {
        var myService: TicketPMService = new TicketPMService();
        myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;
                this.CurrentSession.CurrentEditComponent.EntityPM = this.EntityPM;
                this.entityArgs.EditComponent.ReloadEntityPM();
                if (this.PageChild_DS != null) {
                    this.PageChild_DS.RefreshTab(this);
                }
                if (this.IsRefreshButton) {
                    this.IsRefreshButton = false;
                    //this.entityArgs.EditComponent.ReloadEntityPM();
                }
                if (this.IsReload) {
                    this.IsReload = false;
                    //this.entityArgs.EditComponent.ReloadEntityPM();
                }
                this.ReloadhData();
            }
        });
    }

    ReloadhData() {
        this.LoadCommunicationLogsList();
        this.LoadActivities();
        this.SetUIProperties();
    }
    ngOnInit() {
        this.LoadCommunicationLogsList();
        this.LoadActivities();
        this.SetSelectedTab();
    }
    ngAfterViewInit() {
        this.SelectionChanged();
    }

    // UI Properties
    private SetUIProperties() {
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.MainClassificationId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CompanyId));

        if (!AppTool.IsNullOrEmpty(this.ShipmentNumber)) {
            this.shipmentLinkNumberVisibility = true;
        }

        this.SetUIProperties_EntityClosed();
    }
    private SetUIProperties_EntityClosed() {
        this.IsTicketEditEnabled = CRMTool.IsTicketEditEnabled(this.EntityPM);
        this.LinkColor = this.IsTicketEditEnabled ? "#1E4AC4" : "gray" ;
        this.IsTicketReplyEnabled = FeatureLocator.HasFeaturePermession("Ticket", "TicketReply") && CRMTool.IsTicketEditEnabled(this.EntityPM);
        this.IsTicketActivityEnabled = FeatureLocator.HasFeaturePermession("Ticket", "TicketActivities") && CRMTool.IsTicketEditEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("ShipmentNumber", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("EntityType", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CompanyId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CustomerContactId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("SeverityId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CreateDate", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("MainClassificationId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("EmployeeGroupId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("OwnerId", this.ObjectTableName, this.IsTicketEditEnabled);
        if (this.PageChild_DS != null) {
            this.PageChild_DS.SetUIProperties();
        }
    }
    public SetUIRequiredProperties() {
        this.UIProperties.SetRequired("EmployeeGroupId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.EmployeeGroupId));
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OwnerId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CompanyId));
        this.UIProperties.SetRequired("ShipmentNumber", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CompanyId));
    }

    get IsResolveDue() {
        return this.EntityPM.IsResolveDue;
    }
    get ResolveColor() {
        return this.EntityPM.ResolveColor;
    }
    get IsResolveExamination() {
        return this.EntityPM.IsResolveExamination;
    }
    get IsResponseDue() {
        return this.EntityPM.IsResponseDue;
    }
    get TicketFirstResponseTime() {
        return this.EntityPM.TicketFirstResponseTime;
    }
    get TicketFirstResolveTime() {
        return this.EntityPM.TicketFirstResolveTime;
    }
    get IsResponseExamination() {
        return this.EntityPM.IsResponseExamination;
    }
    get ResponseColor() {
        return this.EntityPM.ResponseColor;
    }

    private isTicketEditEnabled;
    get IsTicketEditEnabled() { return this.isTicketEditEnabled; }
    set IsTicketEditEnabled(value: boolean)
    {
        if (this.isTicketEditEnabled != value) {
            this.isTicketEditEnabled = value;
        }
    }

    private  isTicketReplyEnabled;
    get IsTicketReplyEnabled()
    {
        return this.isTicketReplyEnabled;
    }
    set IsTicketReplyEnabled(value: boolean)
    {
        if (this.isTicketReplyEnabled != value) {
            this.isTicketReplyEnabled = value;
        }
    }

    private isTicketActivityEnabled;
    get IsTicketActivityEnabled()
    {
        return this.isTicketActivityEnabled;
    }
    set IsTicketActivityEnabled(value: boolean)
    {
        if (this.isTicketActivityEnabled != value) {
            this.isTicketActivityEnabled = value;
        }
    }

    // Fill Correspondence Lines From Ticket
    public CorrespondenceList: CorrespondenceViewModelData[] = [];
    public FirstCorrespondence: CorrespondenceViewModelData = null;
    FillTicketCorrespondenceLines() {
        this.CorrespondenceList = [];
        this.EntityPM.TicketCorrespondence.sort((a, b) => { return (DateTool.GetDateParts(a.CreateDate).DateObject.valueOf() === DateTool.GetDateParts(b.CreateDate).DateObject.valueOf()) ? 0 : (DateTool.GetDateParts(a.CreateDate).DateObject.valueOf()  > DateTool.GetDateParts(b.CreateDate).DateObject.valueOf() ) ? -1 : 1 }).forEach(item => {
            this.CorrespondenceList.push(new CorrespondenceViewModelData(item, this));
        });   

        this.FirstCorrespondence = this.CorrespondenceList[0];
    }
    LoadCommunicationLogsList() {
        this.TicketCommunicationLogs = [];

        var myService: CRMDomainService = new CRMDomainService();
        myService.GetCommunicationLogs(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var logs: CommunicationLogPM[] = myResponse.Result;
                logs.forEach(item => {
                    this.TicketCommunicationLogs.push(item);
                });
                this.FillTicketCorrespondenceLines();
            }
        });   
    }

    // Properties
    get Subject(){ return this.EntityPM.Subject; }
    set Subject(value:string)
    {
        if (this.EntityPM.Subject != value) {
            this.EntityPM.Subject = value;
           
        }
    }

    get EmployeeGroupId() { return this.EntityPM.EmployeeGroupId; }
    set EmployeeGroupId(newValue: string) {
        if (this.EntityPM.EmployeeGroupId != newValue) {
            this.EntityPM.EmployeeGroupId = newValue;
            this.CheckOwnerEmployeeGroup();
        }
    }

    CheckOwnerEmployeeGroup() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetOwnerEmployeeGroup(this.OwnerId, this.EmployeeGroupId).subscribe((myResult: any) => {
            this.OwnerId = myResult;
        });
    }

    get OwnerId() { return this.EntityPM.OwnerId; }
    set OwnerId(newValue: string) {
        this.EntityPM.OwnerId = newValue;

        if (newValue == null) {
            this.EntityPM.BusinessUnitId = null;
        }

        else {
            var myService: UserListService = new UserListService();
            myService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: ServiceResponse = resp;
                    var list: UserList = result.Result;
                    if (list != null) {
                        this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                    }
                }
            });
        }
    }

    get TicketTypeId() { return this.EntityPM.TicketTypeId; }
    set TicketTypeId(newValue: string) {
        if (this.EntityPM.TicketTypeId != newValue) {
            this.EntityPM.TicketTypeId = newValue;
        }
    }

    get ContactId() { return this.EntityPM.ContactId; }
    set ContactId(newValue: string) {
        if (this.EntityPM.ContactId != newValue) {
            this.EntityPM.ContactId = newValue;
            this.ContactListService.getSingle(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: ContactList = resp.Result;
                    if (result != null) {
                        this.EntityPM.ContactName = result.EnglishName;
                        this.EntityPM.ContactEmail = result.Email;
                        this.EntityPM.ContactPhone = result.BusinessPhone;
                    }
                }
            });
        }
    }

    get CompanyId() { return this.EntityPM.CompanyId; }
    set CompanyId(newValue: string) {
        if (this.EntityPM.CompanyId != newValue) {
            this.EntityPM.CompanyId = newValue;
            this.UpdateCompanyContact();
            this.SetUIProperties();
        }
    }

    UpdateCompanyContact() {
        var myContactId: string = null;
        if (!AppTool.IsNullOrEmpty(this.CompanyId)) {
            var myService: CardListService = new CardListService();
            myService.getSingle(this.CompanyId).subscribe((myResult: ServiceResponse) => {
                if (!myResult.HasError) {
                    var list = myResult.Result;
                    if (list != null) {
                        myContactId = list.PrimaryContactId;
                    }
                }
            });
        }
        this.ContactId = myContactId;
    }

    get ShipmentId() { return this.EntityPM.ShipmentId; }
    set ShipmentId(newValue: string) {
        if (this.EntityPM.ShipmentId != newValue) {
            this.EntityPM.ShipmentId = newValue;
        }
    }

    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }
    set ShipmentNumber(newValue: string) {
        this.EntityPM.ShipmentNumber = newValue;
    }

    get ContactName() { return this.EntityPM.ContactName; }
    set ContactName(newValue: string) {
        this.EntityPM.ContactName = newValue;
    }

    get TicketDescription() { return this.EntityPM.TicketDescription; }
    set TicketDescription(newValue: string) {
        this.EntityPM.TicketDescription = newValue;
    }
   
    get MainClassificationId() { return this.EntityPM.MainClassificationId; }
    set MainClassificationId(newValue: string) {
        if (this.EntityPM.MainClassificationId != newValue) {
            this.EntityPM.MainClassificationId = newValue;
            this.SecondaryClassificationId = null;
            this.SetUIProperties();
            this.setDeafaultsValues();
        }
    }

    setDeafaultsValues() {
        if (this.SecondaryClassificationId != null) {
            var myService: TicketClassificationListService = new TicketClassificationListService();
            myService.getSingleFromCache(this.SecondaryClassificationId).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result = resp.Result;
                    if (result != null) {
                        this.SeverityId = result.DefaultSeverityId;
                        this.EmployeeGroupId = result.EmployeeGroupId;
                    }
                }
            });
        }

        else {
            if (this.MainClassificationId != null) {

                var myService: TicketClassificationListService = new TicketClassificationListService();
                myService.getSingleFromCache(this.MainClassificationId).subscribe((resp: ServiceResponse) => {
                    if (!resp.HasError) {
                        var result= resp.Result;
                        if (result != null) {
                            this.SeverityId = result.DefaultSeverityId;
                            this.EmployeeGroupId = result.EmployeeGroupId;
                        }
                    }
                });
            }

            else {
                this.SeverityId = null;
                this.EmployeeGroupId = null;
            }
        }
    }

    get SecondaryClassificationId() { return this.EntityPM.SecondaryClassificationId; }
    set SecondaryClassificationId(newValue: string) {
        if (this.EntityPM.SecondaryClassificationId != newValue) {
            this.EntityPM.SecondaryClassificationId = newValue;
            this.setDeafaultsValues();
        }
    }

    get StageId() { return this.EntityPM.StageId; }
    set StageId(newValue: string) {
        this.EntityPM.StageId = newValue;
    }

    get StageCode() { return this.EntityPM.StageCode; }
    set StageCode(newValue: string) {
        this.EntityPM.StageCode = newValue;
    }

    get StageName() { return this.EntityPM.StageName; }
    set StageName(newValue: string) {
        this.EntityPM.StageName = newValue;
    }

    get SeverityId() { return this.EntityPM.SeverityId; }
    set SeverityId(newValue: string) {
        if (this.EntityPM.SeverityId != newValue) {
            this.EntityPM.SeverityId = newValue;
           
        }
    }


    get CreateDate() { return this.EntityPM.CreateDate; }
    set CreateDate(newValue: Date) {
        if (this.EntityPM.CreateDate != newValue) {
            this.EntityPM.CreateDate = newValue;
        }
    }

    public IsOpen: boolean;

    get SubmitButtonEnabled() {
        var myResult = true;

        if (AppTool.IsNullOrEmpty(this.CorrespondenceLine)) {
            myResult = false;
        }
        return myResult;
    }

    private  correspondenceLine;
    get CorrespondenceLine() { return this.correspondenceLine; }
    set CorrespondenceLine(value: string)
    {
        if (this.correspondenceLine != value) {
            this.correspondenceLine = value;
        }
    }

    get AddShipmentEnabled()
    {
        return this.EntityPM.CompanyId == null ? false : true;
    }

    get DeleteShipmentVisibility() {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(this.ShipmentNumber)) {
            myResult = true;
        }

        return myResult;
    }

    // View Shipment 
    private shipmentLinkNumberVisibility = false;

    // Send Email Command 
    private code = ""; 
    SendEmailCommand(code: string) {
        this.code = code;
        this.SendEmail();
    }
    SendEmail() {
        var validator: TicketValidator = new TicketValidator();
        var errors = validator.ValidateCurrenctEntity(this.EntityPM);
        if (errors.length == 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = null;
            if (this.code != null) {
                var isInternal = false;
                var windowTitle;
                if (this.code == "Ex") {
                    windowTitle = "Reply";
                    isInternal = false;
                }
                else {
                    windowTitle = "Add Note";
                    isInternal = true;
                }
                var logWindow = new LogitudeWindow();
                logWindow.Width = 850;
                logWindow.Height = 700;
                logWindow.Title = windowTitle;
                var args = new SendEmailArgs();
                args.Ticket = this.EntityPM;
                args.IsInternal = isInternal;
                args.InternalCorrespondenceLinesCount = this.CorrespondenceList.length;
                logWindow.WindowArgs = args;
                logWindow.WindowClosed.subscribe(($event: any) => this.OnNewLineWindowClosed($event));
                logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/MainTab/SendEmailComponent');
            }
        }
        else {
            if (this.CurrentSession.CurrentEditComponent != null) {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            }
        }
    }

    OnNewLineWindowClosed(arg: any) {
        if (arg == "OK") {
            this.RefreshData();
        }
    }

    //Tabs 
    private PageChild_DS: DetailsTabComponent = null;
    private PageChild_OA: ActivitiesTabComponent = null;
    private PageChild_PT: PostsTabComponent = null;

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    // Selected Tab
    private SetSelectedTab() {
        this.selectedTabCode = "DS";
    }
    SelectionChanged() {
        if (this.SelectedTabCode != null) {

            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {

                switch (this.SelectedTabCode) {

                    case "DS": {
                        if (this.PageChild_DS == null) {
                            SessionLocator.DynamicLoader.Load('./CRMModules/CRMTickets/Components/EditTabs/MainTab/DetailsTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_DS = cmpRef.instance;
                                    this.PageChild_DS.InitTab(this);
                                });
                        }

                        else {
                            this.PageChild_DS.RefreshTab(this);
                        }

                        break;
                    }
                    case "OA": {
                        if (this.PageChild_OA == null) {
                            SessionLocator.DynamicLoader.Load('./CRMModules/CRMTickets/Components/EditTabs/MainTab/ActivitiesTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_OA = cmpRef.instance;
                                    this.PageChild_OA.InitTab(this);
                                });
                        }
                        else {
                            this.PageChild_OA.InitTab(this);
                        }
                        break;
                    }
                    case 'PT': {
                        if (this.PageChild_PT == null) {
                            SessionLocator.DynamicLoader.Load('./CRMModules/CRMTickets/Components/EditTabs/MainTab/PostsTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_PT = cmpRef.instance;
                                   
                                });
                        }
                        break;
                    }
                }
            }
        }
    }

    //  Activities
    AddActivity(code: string) {
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow();
        switch (code.toUpperCase()) {
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
        var windowArgs: ActivityInputArgs = new ActivityInputArgs();
        windowArgs.TypeCode = code;
        if (code.toUpperCase() == "CL") {
            windowArgs.CallWithId = this.EntityPM.ContactId;
            windowArgs.IsOpen = false;
            windowArgs.IsMarkedCompleted = true;
        }
        windowArgs.CustomerId = this.EntityPM.CompanyId;
        windowArgs.TicketId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;

        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.RefreshData();
                }
            });
        });
    }
    private  openActivitiesHeader = "Open Activities (0)";
    get OpenActivitiesHeader()
    {
        return this.openActivitiesHeader;
    }
    set OpenActivitiesHeader(value:string)
    {
        this.openActivitiesHeader = value;
    }
    public ActivitiesContent: string = "";
    public ActivitiesList: ActivityItemClass[] = [];
    GetActivitiesContent() {
        var count = this.ActivitiesList.length;
        this.OpenActivitiesHeader = "Open Activities (" + count + ")";;
    }
    public LoadActivities() {
        this.ActivitiesList = [];
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetActivitiesByTicketId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var dataResult: ActivityList[] = myResponse.Result; 
                if (dataResult.length > 0) {
                    dataResult.filter(d => d.IsOpen).sort((a, b) => {
                        return (DateTool.GetDateParts(a.DueDate).DateObject === DateTool.GetDateParts(b.DueDate).DateObject) ? 0 : (DateTool.GetDateParts(a.DueDate).DateObject > DateTool.GetDateParts(b.DueDate).DateObject ) ? 1 : -1
                    }).forEach(item => {
                        this.ActivitiesList.push(new ActivityItemClass(item, this));
                    });
                }
                this.GetActivitiesContent();
                if (this.PageChild_OA != null) {
                    this.PageChild_OA.InitTab(this);
                }
            }
        });
    }

    private IsRefreshButton = false; 
    private IsReload = false; 
    RefreshData() {
        this.IsReload = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
    
    RefreshButtonClicked() {
        this.IsRefreshButton = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
    ReLoadTicket() {
        this.LoadCommunicationLogsList();
        this.LoadActivities();
        this.SetUIProperties();
    }
}
export class CorrespondenceViewModelData extends BaseComponent {
    public entityPM: CorrespondencePM;
    public trigger: TicketMainTabComponent;
    public DataContext: CorrespondenceViewModelData = this;
    public ImageSrc: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: CorrespondencePM, trigger: TicketMainTabComponent) {
        super();
        this.entityPM = item;
        this.trigger = trigger;
        this.Height = this.myBaseHight;
        this.ImageSrc = CRMTool.GetActivityImageSrc(this.entityPM.ActivityTypeCode);
        this.AttachmentsList = this.FillAttachments();
        //this.OnTextBoxLoaded();
        this.RunComponentTimer();
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
        var idIndex = this.CurrentSession.GetNewId("TextArea");
        this._TextAreaId = "TextArea_" + idIndex;
        this._TextAreaId2 = "TextArea_2" + idIndex;
        this.timerToken = setTimeout(() => this.GetRecipients(), 1);
        this.SourceText = this.GetSourceText();
        //this.GetRecipients();
    }

    private myBaseHight = "90px";
    public LineMaxHeight = "auto";
    public LineHeight = "auto";
    public LineDisplay = "initial";
    public LineTextOverflow = "initial";
    public LineOverflow = "auto";
    public IsLineVisible = false;

    private _TextAreaId: string;
    private _TextAreaId2: string;
    private actualHeight = 0;
    OnTextBoxLoaded() {
        var textarea = document.getElementById(this._TextAreaId);
        var textarea2 = document.getElementById(this._TextAreaId2);
        this.IsLineVisible = true;

        var div = document.createElement("div");
        div.innerText = this.Description;
        div.style.position = "absolute";
        div.style.visibility = "hidden";
        div.style.whiteSpace = "nowrap";
        div.style.width = "auto";
        div.style.height = "auto";
        document.body.appendChild(div);
        this.actualHeight = div.clientHeight;
        var height = 90;
        if (this.actualHeight > height) {
            this.ShowMoreVisibility = true;
            textarea.style.height = this.myBaseHight;
            textarea.style.maxHeight = this.myBaseHight;
        }
        else {
            this.ShowMoreVisibility = false;
            if (textarea != null && textarea.style != null) {
                textarea.style.height = textarea.scrollHeight + "px";
                textarea.style.maxHeight = textarea.scrollHeight + "px";
            }
        }
        //textarea2.style.display = "none";
        document.body.removeChild(div);
    }
    public GetTextHeight(myString: string, fontSize: number = 12) {
        var myResult: number = 0;
        if (!AppTool.IsNullOrEmpty(myString)) {
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext("2d");
            ctx.font = fontSize + "px Lucida Sans Unicode";
            var txtHeight = parseInt(ctx.font);
            myResult = canvas.height;
        }
        return myResult;
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.OnTextBoxLoaded(), 1);
        }
    }

    private height: string;
    get Height() {
        return this.height;
    }
    set Height(value: string) {
        this.height = value;
    }

    private showMoreVisibility = false;
    get ShowMoreVisibility() {
        return this.showMoreVisibility;
    }
    set ShowMoreVisibility(value: boolean) {
        this.showMoreVisibility = value;
    }

    private showLessVisibility = false;
    get ShowLessVisibility() {
        return this.showLessVisibility;
    }
    set ShowLessVisibility(value: boolean) {
        this.showLessVisibility = value;
    }

    get CorrespondenceHTMLBodyLinkVisibility() {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(this.entityPM.HTMLFullBody)) {
            myResult = true;
        }

        return myResult;
    }
    get TextAlignRegionVisibility() {
        var myResult = false;
        if (SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
            myResult = true;
        }
        return myResult;
    }

    public ShowMore() {
        var item = document.getElementById(this._TextAreaId);
        item.style.height = this.actualHeight + "px";
        item.style.maxHeight = this.actualHeight + "px";
        this.ShowMoreVisibility = false;
        this.ShowLessVisibility = true;
    }
    private ShowLess() {
        var item = document.getElementById(this._TextAreaId);
        item.style.height = this.myBaseHight;
        item.style.maxHeight = this.myBaseHight;
        this.ShowMoreVisibility = true;
        this.ShowLessVisibility = false;
    }

    // Properties
    public SourceText = null;
    public CorrespondenceCommunicationToolTip: string;
    GetSourceText() {
        var sourceText = null;
        if (this.entityPM.Direction != 'I' && this.trigger.TicketCommunicationLogs != null) {
            var list = this.trigger.TicketCommunicationLogs.filter(a => a.ChildEntityId == this.entityPM.Id);
            this.CorrespondenceCommunicationToolTip = "";
            if (list.length > 0) {

                if (list.filter(a => a.EmailDeliveryError != null)[0]) {
                    sourceText = "./Images/Orange_ball.png";
                }
                else if (list.filter(a => a.CommunicationStatusTypeCode == "W")[0]) {
                    sourceText = "./Images/Orange_ball.png";
                }
                else if (list.filter(a => a.CommunicationStatusTypeCode == "F")[0]) {
                    sourceText = "./Images/Red_ball.png";
                }
                else if (list.filter(a => a.CommunicationStatusTypeCode == "C")[0]) {
                    sourceText = "./Images/Orange_ball.png";
                }
                else {
                    sourceText = "./Images/Green_ball.png";
                }

                list.forEach(item => {
                    if (!(item.CommunicationStatusTypeCode == "W" || item.CommunicationStatusTypeCode == "F" || item.CommunicationStatusTypeCode == "C")) {
                        var communicationLog = item;
                        var error = communicationLog != null ? communicationLog.EmailDeliveryError : "";
                        if (AppTool.IsNullOrEmpty(error)) {
                            if (AppTool.IsNullOrEmpty(this.CorrespondenceCommunicationToolTip)) {
                                this.CorrespondenceCommunicationToolTip = "Sent Successfully";
                            }
                            //sourceText =  "./Images/Green_ball.png";
                        }
                        else {
                            this.CorrespondenceCommunicationToolTip += error;
                            this.CorrespondenceCommunicationToolTip = this.CorrespondenceCommunicationToolTip.replace("Sent Successfully", "");
                            //sourceText = "./Images/Orange_ball.png";
                        }
                    }
                    else if (list.filter(a => a.CommunicationStatusTypeCode == "F")[0]) {
                        this.CorrespondenceCommunicationToolTip = "Error at Sending";
                        //sourceText = "./Images/Red_ball.png";
                    }
                    else if (item.CommunicationStatusTypeCode == "C") {
                        communicationLog = item;
                        var error = communicationLog != null ? communicationLog.EmailDeliveryError : "";
                        if (AppTool.IsNullOrEmpty(error)) {
                            this.CorrespondenceCommunicationToolTip = "Processing";
                        }
                        else {
                            this.CorrespondenceCommunicationToolTip += error;
                        }
                        //sourceText = "./Images/Orange_ball.png";
                    }
                    else {
                        this.CorrespondenceCommunicationToolTip = "Waiting";
                        //sourceText = "./Images/Orange_ball.png";
                    }
                });
            }

            return sourceText;
        }
    }

    get Description() {
        var myResult = this.entityPM.Description;
        return AppTool.IsNullOrEmpty(myResult) ? "" : myResult.trim();
    }
    set Description(value: string) {
        this.entityPM.Description = value;
    }

    get ActivityTypeCode() {
        return this.entityPM.ActivityTypeCode;
    }
    set ActivityTypeCode(value: string) {
        this.entityPM.ActivityTypeCode = value;
    }

    get ActivityIconVisibility() {
        var result = false;

        if (this.ActivityTypeCode != null) {
            result = true;
        }

        return result;
    }
    get TextAlignVisibility() {
        var result = true;

        if (this.ActivityTypeCode != null) {
            result = false;
        }

        return result;
    }

    get ContactName() {
        return this.entityPM.ContactName;
    }
    set ContactName(value: string) {
        this.entityPM.ContactName = value;
    }

    get Id() {
        return this.entityPM.Id;
    }

    get IsInternal() {
        return this.entityPM.IsInternal;
    }
    set IsInternal(value: boolean) {
        this.entityPM.IsInternal = value;
    }

    get CreateDate() {
        return this.entityPM.CreateDate;
    }

    public Recipients = ""; 
    GetRecipients() {
        var recipients = "";
        if (this.entityPM != null) {
            var list = [];
            this.trigger.CorrespondenceList.forEach(item => {
                list.push(item);
            });
            this.timerToken = setTimeout(() => "", 1);

            var correspondence = list.sort((a, b) => { return (DateTool.GetDateFromDate(a.CreateDate).valueOf() === DateTool.GetDateFromDate(b.CreateDate).valueOf()) ? 0 : (DateTool.GetDateFromDate(a.CreateDate).valueOf() < DateTool.GetDateFromDate(b.CreateDate).valueOf()) ? -1 : 1 })[0];
            if (correspondence) {
                var firstCorrespondenceId = correspondence.Id;
                if (!AppTool.IsNullOrEmpty(this.trigger.EntityPM.ContactEmail) && !AppTool.IsNullOrEmpty(this.trigger.EntityPM.ContactEmail.trim()) && !this.entityPM.IsInternal) {
                    if (this.entityPM.ActivityId == null && this.entityPM.Direction == "O") {
                        if (firstCorrespondenceId != this.Id) {
                            recipients += "To: " + this.trigger.EntityPM.ContactEmail + " ";
                        }
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.entityPM.CCs) && !AppTool.IsNullOrEmpty(this.entityPM.CCs.trim()) && !this.entityPM.IsInternal) {
                    if (firstCorrespondenceId != this.Id) {
                        var myCC = this.entityPM.CCs.replace(";{2;}", ";").trim();
                        while (myCC.charAt(0) == ";")
                            myCC = myCC.substr(1);
                        if (myCC != ";") {
                            recipients += " | " + "CC: " + myCC + " ";
                        }
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.entityPM.InternalUsers) && !AppTool.IsNullOrEmpty(this.entityPM.InternalUsers.trim())) {
                    if (firstCorrespondenceId != this.Id) {
                        var myInternalUsers = this.entityPM.InternalUsers.replace(";{2;}", ";").trim();
                        while (myInternalUsers.charAt(0) == ";")
                            myInternalUsers = myInternalUsers.substr(1);
                        if (myInternalUsers != ";") {
                            recipients += " | " + "Internal Users: " + this.entityPM.InternalUsers + " ";
                        }
                    }
                }
            }
        }

        this.Recipients = recipients;

    }
    get LineBoxBackground() {
        var lineBoxBackground = "rgb(255, 255, 255)";
        if (this.entityPM != null && this.entityPM.ActivityId != null) {
            lineBoxBackground = "rgb(230,230,230)";
        }
        else if (this.entityPM != null && this.entityPM.IsInternal) {
            lineBoxBackground = "rgb(255, 250, 220)";
        }
        else {
            lineBoxBackground = "rgb(255, 255, 255)";
        }
        return lineBoxBackground;
    }
    get LoggedUserName() {

        if (this.entityPM.Direction == "O") {
            return SessionLocator.LoggedUserPM.EnglishName;
        }
        else {
            return this.entityPM.ContactName;
        }
    }

    // Align Commands 
    public FlowDirection: string = "ltr";
    private GetFlowDirection() {
        var myResult = "ltr";
        if (SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.entityPM.RightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.FlowDirection = myResult;
    }

    public BackgroundAlignRight = "transparent";
    private BackgroundAlignLeft = "transparent";
    private GetBackgroundAlignRight() {
        if (!AppTool.IsNullOrEmpty(this.FlowDirection)) {
            this.BackgroundAlignRight = this.FlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    }
    private GetBackgroundAlignLeft() {
        if (!AppTool.IsNullOrEmpty(this.FlowDirection)) {
            this.BackgroundAlignLeft = this.FlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    }

    public AlignLeftClicked() {
        this.entityPM.RightToLeft = false;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
        this.UpdateCorrespondence();
    }
    public AlignRightClicked() {
        this.entityPM.RightToLeft = true;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
        this.UpdateCorrespondence();
    }
    public RefreshTextAlgimentVariables() {
        this.GetBackgroundAlignLeft();
        this.GetBackgroundAlignRight();
    }

    UpdateCorrespondence() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetUpdateCorrespondence(this.entityPM.Id, this.entityPM.RightToLeft).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                //this.trigger.FillTicketCorrespondenceLines();
            }
        });
    }

    // Attachments Mangment 
    public AttachmentsList;
    private FillAttachments() {
        if (this.entityPM == null) {
            return [];
        }
        else {
            var list: TicketDocumentDataArgs[] = [];
            var line = this.trigger.EntityPM.TicketDocumentData.filter(a => a.CorrespondenceId == this.entityPM.Id)[0];
            if (line != null) {

                this.trigger.EntityPM.TicketDocumentData.filter(a => a.CorrespondenceId == this.entityPM.Id).forEach(pm => {
                    var viewmodel: TicketDocumentDataArgs = new TicketDocumentDataArgs(pm);
                    list.push(viewmodel);
                });
            }
            return list;
        }
    }

    // Commands 
    public ViewHTMLBody() {
        var token = ServiceHelper.GetLDocumentDownloadToken();
        var link = "/WebPages/CorrespondenceDisplayPage.aspx?id=" + this.entityPM.Id + "&tempId=" + token;
        window.open(ServiceHelper.GetLogitudeURL() + link);
    }

    EditActivity() {
        this.trigger._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.entityPM.ActivityId, ObjectTableName: "Activity", BackButtonLabel: "Tickets" });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.trigger.LoadActivities();
                    });
                });
        });
    }
}
export class TicketDocumentDataArgs {
    public DocumentDataPM: DocumentDataPM;

    constructor(documentDataPM: DocumentDataPM) {
        this.DocumentDataPM = documentDataPM;
    }

    get FileName() {
        return this.DocumentDataPM.FileName;
    }
    get FileExtension() {
        return this.DocumentDataPM.FileExtension;
    }
    get Tenant() {
        return this.DocumentDataPM.Tenant;
    }

    ViewAttachment() {
        var documentSecurity = this.DocumentDataPM.SecurityId;
        var link = "/WebPages/CorrespondenceDownloadpage.aspx?id=" + documentSecurity + "~" + this.Tenant;
        window.open(ServiceHelper.GetLogitudeURL() + link);
    }
}
export class ActivityItemClass extends BaseComponent {
    public entity: ActivityList;
    public EntityId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: ActivityList, public father: TicketMainTabComponent) {
        super();
        this.entity = item;
        this.EntityId = item.Id;
        this.ImageSrc = CRMTool.GetActivityImageSrc(this.entity.ActivityTypePathCode);
        this.GetDueDateForeground();
        this.GetDateValue();
        this.GetDateLable();
        this.GetAction();
        this.GetActionBy();
        this.getActionDate();
    }

    // Properties
    private background: string = "rgb(255,255,255)";
    get Background() {
        if (!this.entity.IsOpen) {
            this.background = "rgba(0,0,0,0.1)";
        }
        return this.background;
    }
    set Background(value: string) {
        this.background = value;
    }

    ControlIsEnabled() { return this.entity.IsOpen; }
    get ActivityTypePathCode() { return this.entity.ActivityTypePathCode; }
    get Id() { return this.entity.Id; }
    get CallWithId() { return this.entity.CallWithId; }
    get ActivityTypeName() { return this.entity.ActivityTypeName; }
    get ActivityTypeCode() { return this.entity.ActivityTypeCode; }
    get Subject() { return this.entity.Subject; }
    get Owner() { return this.entity.OwnerName; }
    get CustomerId() { return this.entity.CustomerId; }
    get SortingBy() { return this.entity.SortingBy; }
    get DueDate() { return this.entity.DueDate; }
    get StartDate() { return this.entity.StartDateTime; }
    get SortingDate() { return this.entity.SortingDate; }
    get MeetingSummary() { return this.entity.MeetingSummary; }
    set MeetingSummary(value: string) {
        if (this.entity.MeetingSummary != value) {
            this.entity.MeetingSummary = value;
        }
    }
    get PostToFollowers() { return this.entity.PostToFollowers; }
    set PostToFollowers(value: boolean) {
        if (this.entity.PostToFollowers != value) {
            this.entity.PostToFollowers = value;

        }
    }

    public DateLable: string = "";
    GetDateLable() {
        var myResult = "Due Date";
        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = "Due Date";
                    }

                    else {
                        myResult = "Start Date";
                    }

                    break;
                }

            case "AP":
                {
                    myResult = "Start Date";
                    break;
                }

            case "EO":
                {
                    myResult = "To";
                    break;
                }

            case "EI":
                {
                    myResult = "From";
                    break;
                }
        }

        this.DateLable = myResult;
    }

    public DateValue: string = "";
    GetDateValue() {
        var DatePipe = new DateTimePipe();
        var myResult = "";

        if (this.DueDate != null) {
            myResult = DatePipe.transform(this.DueDate, "SD");
        }

        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = DatePipe.transform(this.DueDate, "SD");

                    }

                    else if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }

                    break;
                }

            case "AP":
                {
                    if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }

                    break;
                }

            case "EO":
                {
                    myResult = this.entity.RecipientsEmails;
                    break;
                }

            case "EI":
                {
                    break;
                }
        }
        this.DateValue = myResult;
    }

    public DueDateForeground: string = "";
    GetDueDateForeground() {
        var result = "rgb(40,46,48)";

        if (this.DueDate != null && DateTool.GetDateParts(this.DueDate) < DateTool.GetDateParts(DateTool.GetCurrentDateTimeAsUtc())) {
            result = "Red";
        }
        this.DueDateForeground = result;
    }

    public ImageSrc: string;

    // Action Fields
    public Action: string = "";
    GetAction() {
        var myResult = "Modified by";

        if (this.entity.ActivityTypeCode == "EI") {
            myResult = "Recorded by";
        }

        else if (this.entity.ActivityTypeCode == "EO") {
            myResult = "Sent by";
        }

        else {
            switch (this.entity.ActivityStatusCode) {
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

        this.Action = myResult;
    }

    public ActionBy: string = "";
    GetActionBy() {
        var myResult = this.entity.UpdatedByUserName;

        if (!this.entity.IsOpen) {
            if (this.entity.ActivityTypeCode == "EO") {
                myResult = this.entity.CreatedByUserName;
            }
        }

        this.ActionBy = myResult;
    }

    public ActionDate: Date;
    getActionDate() {
        var myResult = this.entity.UpdateDate;
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityStatusCode == "C") {
                myResult = this.entity.CompleteDate;
            }
        }

        this.ActionDate = myResult;
    }

    private completeVisi = false;
    get CompleteButtonVisibility() {
        if (this.entity.IsOpen && this.entity.ActivityTypeCode != "AP") {
            this.completeVisi = true;
        }
        return this.completeVisi;
    }
    set CompleteButtonVisibility(value: boolean) { this.completeVisi = value; }

    private reopenVisi = false;
    get ReopenButtonVisibility() {
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityTypeCode != "EI" && this.entity.ActivityTypeCode != "EO") {
                this.reopenVisi = true;
            }
        }
        return this.reopenVisi;
    }
    set ReopenButtonVisibility(value: boolean) { this.reopenVisi = value; }

    private completetogVisi = false;
    get CompleteToggleButtonVisibility() {
        if (this.entity.IsOpen && this.entity.ActivityTypeCode == "AP") {
            this.completetogVisi = true;
        }
        return this.completetogVisi;
    }
    set CompleteToggleButtonVisibility(value: boolean) { this.completetogVisi = value; }

    // Commands
    CompleteClicked() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetCompleteActivity(this.EntityId, this.PostToFollowers, this.MeetingSummary).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                this.entity = resp.Result;
                this.ReopenButtonVisibility = true;
                this.CompleteButtonVisibility = false;
                this.CompleteToggleButtonVisibility = false;
                this.Background = "rgba(0,0,0,0.1)";
                this.father.LoadActivities();
            }
        });
    }
    ReopenClicked() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetReopenActivity(this.EntityId).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                this.entity = resp.Result;
                this.ReopenButtonVisibility = false;
                if (this.entity.ActivityTypeCode == "AP") {
                    this.CompleteToggleButtonVisibility = true;
                }
                else {
                    this.CompleteButtonVisibility = true;
                }
                this.Background = "rgb(255,255,255)";
                this.father.LoadActivities();
            }
        });
    }
    ViewEntity(entity) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this.father._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: "Activity", BackButtonLabel: "Tickets" });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.father.LoadActivities();
                        });
                    });
            });
        }
    }
}
