import {Component, OnInit, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {DateTool,AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPPM} from '../../../../QuoteOPM/EntityPMs/QuoteOPPM';
import {QuoteUtilities} from '../../../../QuoteOPM/Utilities/QuoteUtilities';
import {ActivityList} from '../../../../CRM/EntityLists/ActivityList';
import {DateTimePipe} from '../../../../Controls/Pipes/DateTimePipe';
import {QuoteDomainService} from '../../../../QuoteOPM/Services/QuoteDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {OpportunityArgs, ActivityInputArgs} from '../../../../CRM/Args'; 
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CRMDomainService} from '../../../../CRM/Services/CRMDomainService';
import {GeneralEmailSender} from '../../../../Infrastructure/Helpers/GeneralEmailSender';
import {CRMTool} from '../../../../CRM/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'OverviewTabComponent',
    
    templateUrl: './OverviewTabComponent.html',
})

export class OverviewTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: QuoteOPPM;
    public DataContext: OverviewTabComponent = this;
    public ObjectTableName: string = "QuoteOP";
    public ActivitiesList: ActivityItemClass[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    RegardingEntity: string = "";
    EntityId: string = "";
    EntityDescription: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.EntityDescription = this.EntityPM.Subject;
        this.RegardingEntity = "Regarding Quote : " + this.EntityPM.QuoteNumber;


        this.Listen();
        this.Initialize();
        this.LoadActivities();
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "LoadActivity") {
                    this.LoadActivities();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private myQuoteDomainService: QuoteDomainService;
    Initialize() {
        this.myQuoteDomainService = new QuoteDomainService();
    }

    public IsQuoteEditEnabled: boolean = true;
    public IsClosingReasonVisible: boolean = false;
    SetUIProperties() {
        this.IsQuoteEditEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);

        this.UIProperties.SetEnabled("StageId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("StageDueDate", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("RatingCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("QuoteClosingReasonId", this.ObjectTableName, false);

        var isClosingReasonVisibile = this.EntityPM.IsClosed && !AppTool.IsNullOrEmpty(this.QuoteClosingReasonId);
        this.UIProperties.SetVisibility("QuoteClosingReasonId", this.ObjectTableName, isClosingReasonVisibile);

        this.IsClosingReasonVisible = isClosingReasonVisibile;
    }
    
    //Properties
    get StageId() { return this.EntityPM.StageId; }
    set StageId(newValue: string) {
        if (this.EntityPM.StageId != newValue) {
            this.EntityPM.StageId = newValue;
        }
    }

    get StageDueDate() { return this.EntityPM.StageDueDate; }
    set StageDueDate(newValue: Date) {
        if (this.EntityPM.StageDueDate != newValue) {
            this.EntityPM.StageDueDate = newValue;
        }
    }

    get RatingCode() { return this.EntityPM.RatingCode; }
    set RatingCode(newValue: string) {
        if (this.EntityPM.RatingCode != newValue) {
            this.EntityPM.RatingCode = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get QuoteClosingReasonId() { return this.EntityPM.QuoteClosingReasonId; }

    get ControlIsEnabled()
    {
        var myResult = true;
        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
            myResult = false;
        }
        return myResult;
    }

    //Activities
    get ActivitiesContent() {
        return TextCodeTranslator.Translate("QuoteOP.S.Overview.Activities") +  " (" + this.ActivitiesList.length + ")";
    }
    public IsAddActivityEnabled: boolean = true;
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
            windowArgs.CallWithId = this.EntityPM.CustomerContactId;
            windowArgs.IsOpen = false;
            windowArgs.IsMarkedCompleted = true;
        }
        //windowArgs.IsAddCustomerAllowed = true;
        windowArgs.CustomerId = this.EntityPM.CustomerId;
        windowArgs.QuoteId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;

        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe((response:any) => {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadActivities();
                }
            });
        });
    }

    EmailSender: GeneralEmailSender;
    AddEmail() {
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender("QuoteOP", "QUOT", this.EntityPM.Id, this.EntityPM.QuoteNumber, "", "", "", "", null, "LoadActivity", this.EntityPM, true, "QEMO");
            this.EmailSender.ShowFullSendControll();
        }
    }

    LoadActivities() {
        this.ActivitiesList = [];
        this.myQuoteDomainService.GetActivitiesByQuoteId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var dataResult: ActivityList[] = myResponse.Result;
                if (dataResult.length > 0) {
                   
                    dataResult.filter(d => d.IsOpen == true).sort((a, b) => { return (DateTool.GetDateFromDate(a.SortingDate) === DateTool.GetDateFromDate(b.SortingDate)) ? 0 : (DateTool.GetDateFromDate(a.SortingDate) > DateTool.GetDateFromDate(b.SortingDate)) ? -1 : 1 }).forEach(item => {
                        this.ActivitiesList.push(new ActivityItemClass(item, this));
                    });
                    dataResult.filter(d => d.IsOpen == false).sort((a, b) => { return (DateTool.GetDateFromDate(a.SortingDate) === DateTool.GetDateFromDate(b.SortingDate)) ? 0 : (DateTool.GetDateFromDate(a.SortingDate) > DateTool.GetDateFromDate(b.SortingDate)) ? -1 : 1 }).forEach(item => {
                        this.ActivitiesList.push(new ActivityItemClass(item, this));
                    });
                }

            }
        });
    }
    ViewEntity(entity) {
        if (!AppTool.IsNullOrEmpty(entity.Id)) {
            this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe((response:any) => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({EntityId: entity.Id, ObjectTableName: "Activity", BackButtonLabel: "Quotes" });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.LoadActivities();
                        });
                    });
            });
        }
    }
    Updated(arg: boolean) {
        if (arg) {
            this.LoadActivities();
        }
    }

    //Social 
    public FollowersCount: number = 0;
}

export class ActivityItemClass extends BaseComponent {
    public entity: ActivityList;
    public EntityId: string;

    constructor(item: ActivityList, public father: OverviewTabComponent) {
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

        if (this.DueDate != null && this.DueDate.valueOf() < DateTool.GetCurrentDateTimeAsUtc().valueOf()) {
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
                this.entity= resp.Result;
                this.father.LoadActivities();
                this.ReopenButtonVisibility = true;
                this.CompleteButtonVisibility = false;
                this.CompleteToggleButtonVisibility = false;
                this.Background = "rgba(0,0,0,0.1)";
            }
        });
    }

    ReopenClicked() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetReopenActivity(this.EntityId).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                this.entity = resp.Result;
                this.father.LoadActivities();
                this.ReopenButtonVisibility = false;
                if (this.entity.ActivityTypeCode == "AP") {
                    this.CompleteToggleButtonVisibility = true;
                }
                else {
                    this.CompleteButtonVisibility = true;
                }
                this.Background = "rgb(255,255,255)";
            }
        });
    }
}
