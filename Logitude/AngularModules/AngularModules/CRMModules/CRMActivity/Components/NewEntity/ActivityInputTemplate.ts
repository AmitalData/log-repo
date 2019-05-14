import {Component, ViewChild, ViewContainerRef, OnInit} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ActivityPM} from '../../../../CRM/EntityPMs/ActivityPM';
import {ActivityInputArgs, InviteeArgs} from '../../../../CRM/Args'
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {CallTypeList} from '../../../../CRM/EntityLists/CallTypeList';
import {ActivityTypeListService} from '../../../../CRM/Services/StandardLists/ActivityTypeListService';
import {ActivityTypeList} from '../../../../CRM/EntityLists/ActivityTypeList';
import {CRMTool, ActivtyDuration} from '../../../../CRM/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ContactList} from '../../../../Common/EntityLists/ContactList';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator'; 
import {ActivityPMInitService} from '../../../../CRM/EntityPMInitServices/ActivityPMInitService'; 
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ContactInputTemplateArgs} from '../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import {CallTypeListService} from '../../../../CRM/Services/StandardLists/CallTypeListService'; 
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {DocumentDataPM} from '../../../../CRM/EntityPMs/DocumentDataPM';
import {ActivityInviteePM} from '../../../../CRM/EntityPMs/ActivityInviteePM';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {ActivityPriorityListService} from '../../../../CRM/Services/StandardLists/ActivityPriorityListService'; 
import {ActivityPriorityList} from '../../../../CRM/EntityLists/ActivityPriorityList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {GeneralDomainService} from '../../../../Infrastructure/Services/GeneralDomainService';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './ActivityInputTemplate.html',
})

export class ActivityInputTemplate extends BaseComponent implements OnInit {
    public entityPM: ActivityPM;
    public ObjectTableName = "Activity";
    public DataContext: ActivityInputTemplate = this;
    public CallTypesList: CallTypeList[] = [];
    public Durations: ActivtyDuration[];
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor() {
        super();
        this.entityPM = new ActivityPM();
        //ActivityPMInitService.InitValues(this.entityPM, true);
        this.CallTypesList = [];
    }

    public TypeCode = "";
    public CustomerVisibility = false;
    public AddContactEnabled: boolean = true;
    public OnEditModeVisibility = false;
    public InitTemplate(args: ActivityInputArgs) {
        if (args != null) {
            this.entityPM = args.Activity;
            if (this.entityPM.ActivityTypeCode == "CL") {
                this.CallWithId = this.entityPM.CallWithId;
            }
            this.TypeCode = this.entityPM.ActivityTypeCode;
            this.CustomerVisibility = args.IsAddCustomerAllowed;
            this.AddCustomerVisibility = this.CustomerVisibility;
            this.AddContactEnabled = args.IsEnabled;
            this.OnEditModeVisibility = args.IsEditMode;
            this.AddCustomerEnabled = args.IsEnabled;
        }
        this.InitializeData();
    }
    SetWindowArgs(args: ActivityInputArgs) {
        if (args != null) {
            this.entityPM = args.Activity;

            if (this.entityPM.ActivityTypeCode == "CL") {
                this.CallWithId = args.Activity.CallWithId;
            }

            this.TypeCode = this.entityPM.ActivityTypeCode;
            this.CustomerVisibility = args.IsAddCustomerAllowed;
            this.AddCustomerVisibility = this.CustomerVisibility;
            this.AddContactEnabled = args.IsEnabled;
            this.OnEditModeVisibility = args.IsEditMode;
            this.AddCustomerEnabled = args.IsEnabled;
        }
        this.InitializeData();
    }

    InitializeData() {
        this.Durations = [];
        CRMTool.GetDurationsList().forEach(item => {
            this.Durations.push(item);
        });

        if (!this.OnEditModeVisibility) {
            this.SetActivityTypeName();

            if (this.TypeCode == "AP") {
                this.StartDateTime = CRMTool.RoundTimeForwardByMinutes(DateTool.GetCurrentDateTimeAsUtc(), 30);
                this.Duration = 30;
                this.entityPM.ActivityTimeTypeCode = "BS";
                this.OnDurationChanged();
            }

            else if (this.TypeCode == "CL") {
                this.CallTypeCode = "O";
            }

            this.SetUIProperties_New();
            this.InitializeRightToLeft();
        }

        else {
            this.UIProperties.SetEnabled("DueDateOffset", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DueDateDateField", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DueDateDefaultText", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BusinessProcessQueueId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TeamId", this.ObjectTableName, false);
        }

        this.GetDueDateObjectField();
        this.GetDescriptionFlowDirection();
        this.GetMeetingSummaryFlowDirection();
        this.RefreshTextAlgimentVariables();
        this.RefreshDescriptionTextAlgimentVariables();

        if ((this.TypeCode == "EO" || this.TypeCode == "EI") && this.OnEditModeVisibility == true) {
            this.BuildInternalAttachmentList();
        }

        if (this.OnEditModeVisibility) {
            this.BuildInviteesControl();
        }

        this.SetUIProperties_Qwner();
    }

    public DueDateDefaultText: string;
    private GetDueDateObjectField() {
        if (this.entityPM.ActivityTypeCode == "TX") {
            if (!AppTool.IsNullOrEmpty(this.entityPM.DueDateDateField)) {
                var shipmentObjecttableId: string = window.ObjectTables.filter(f => f.Name == "Shipment")[0].Id;

                if (!AppTool.IsNullOrEmpty(shipmentObjecttableId)) {
                    var service: GeneralDomainService = new GeneralDomainService();
                    service.GetSingleObjectFieldByFieldNameAndTableId(this.entityPM.DueDateDateField, shipmentObjecttableId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var myField: ObjectFieldPM = myResponse.Result;
                            if (myField != null) {
                                this.DueDateDefaultText = myField.FullNameTextCodeDefaultText;
                            }
                        }
                    });
                }                
            }
        }
    }

    private InitializeRightToLeft() {
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            if (this.entityPM != null) {
                this.entityPM.DescriptionRightToLeft = true;
                this.entityPM.MeetingSummaryRightToLeft = true;
            }
        }
    }
    private SetActivityTypeName() {
        var service = new ActivityTypeListService();
        service.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var typeList = myResponse.Result;
                if (typeList != null && typeList.length > 0) {
                    var type = typeList.filter(d => d.Code == this.TypeCode)[0];
                    this.entityPM.ActivityTypeName = typeList.Name;
                }
            }
        });
    }

    private IsMettingSummeryVisibile = true;
    private SetUIProperties_New() {
        if (!AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
            this.UIProperties.SetEnabled("CustomerId", "Activity", false);
        }

        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, false);

        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName, this.EndDateTime == null);
                    this.UIProperties.SetVisibility("MeetingSummary", this.ObjectTableName, false);
                    this.IsMettingSummeryVisibile = false;
                    break;
                }

            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CallWithId));
                    break;
                }
        }
    }
    SetUIProperties_Edit() {
        if (this.entityPM.ActivityTypeCode == "EI" || this.entityPM.ActivityTypeCode == "EO") {
            this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Subject", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ArchiveDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CreateDate", this.ObjectTableName, false);

        }
        else {
            if (!AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            }
        }

        if (!AppTool.IsNullOrEmpty(this.entityPM.QuoteId)) {
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }

        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName, this.EndDateTime == null);
                    break;
                }

            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.entityPM.CallWithId));
                    break;
                }
        }

        var isOppertunityVisible = !AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, isOppertunityVisible);

        var isQuoteVisible = !AppTool.IsNullOrEmpty(this.entityPM.QuoteId) && AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, isQuoteVisible);
    }
    private SetUIProperties_Qwner() {
        var isQwnerRequired = false;

        if (this.TypeCode != "TX") {
            if (AppTool.IsNullOrEmpty(this.OwnerId)) {
                isQwnerRequired = true;
            }
        }

        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, isQwnerRequired);
    }

    // Additional Feilds
    ngOnInit() {
        this.RunComponent();
        this.FillCallTypeList();
    }
    FillCallTypeList() {
        var myService: CallTypeListService = new CallTypeListService();
        myService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var list: CallTypeList[] = resp.Result;
                this.CallTypesList = list.sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 });
                
                //if (this.TypeCode == "CL") {
                //    this.SelectedCallType = this.CallTypesList.filter(d => d.Code == "O")[0];
                //}
            }
        });
    }
    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }
    private Retries: number = 0;
    private timerToken: any;
    private GeneratedComponent: any;

    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;
                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });
                if (this.TypeCode == "AP") {
                    cmpRef.instance.LabelWidth = 120;
                } else {
                    cmpRef.instance.LabelWidth = 110;
                }
                cmpRef.instance.Run(this.entityPM, this.ObjectTableName, "Activity.AdditionalFields");
            });
    }   
    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.entityPM.IsOpen);
        }
    }

    //Duration
    get IsControlsEnabled() {
        var result = true;
        if (this.entityPM != null) {
            if (!this.entityPM.IsOpen) {
                result = false;
            }
        }
        return result;
    }

    private selectedDuration: ActivtyDuration;
    get SelectedDuration() {
        var result: ActivtyDuration = null;
        if (this.Duration != null) {
            result = this.Durations.filter(d => d.Minuts == this.Duration)[0];

            if (result == null) {
                result = new ActivtyDuration(this.Duration, "", false);
                if (this.Duration < 60) {
                    var minutes = Math.floor(this.Duration);
                    result.Name = this.Duration + " minutes";
                }

                else if (this.Duration < 1440) {
                    var hours = Math.floor(this.Duration / 60);
                    result.Name = hours + " hours";
                }

                else {
                    var days = Math.floor(this.Duration / 1440);
                    result.Name = days + " days";
                    result.IsDay = true;
                }

                if (this.Durations.indexOf(result) == -1) {
                    this.Durations.push(result);
                }
            }
        }
        return result;
    }
    set SelectedDuration(value: ActivtyDuration) {
        if (this.selectedDuration != value) {
            this.selectedDuration = value;
            if (value == null) {
                this.Duration = null;
            }
            else {
                this.Duration = this.selectedDuration.Minuts;
            }
            this.OnDurationChanged();
        }
    }
    get Duration() { return this.entityPM.Duration; }
    set Duration(value: number) {
        if (this.entityPM.Duration != value) {
            this.entityPM.Duration = value;
        }
    }
    get StartDateTime() { return this.entityPM.StartDateTime; }
    set StartDateTime(value: Date) {
        if (this.entityPM.StartDateTime != value) {
            this.entityPM.StartDateTime = value;
            if (this.OnEditModeVisibility) {
                this.SetUIProperties_Edit();
            }
            else{
                this.SetUIProperties_New();
            } 
           
            this.ComputeEndDate();
        }
    }
    get EndDateTime() { return this.entityPM.EndDateTime; }
    set EndDateTime(value: Date) {
        if (this.entityPM.EndDateTime != value) {
            this.entityPM.EndDateTime = value;
            if (this.OnEditModeVisibility) {
                this.SetUIProperties_Edit();
            }
            else {
                this.SetUIProperties_New();
            }
            this.OnEndDateTimeChanged();
        }
    }
    private OnEndDateTimeChanged() {
        this.Duration = null;
        if (this.EndDateTime != null && this.StartDateTime != null && this.EndDateTime.valueOf() > this.StartDateTime.valueOf()) {
            var totalmins = this.GetMinutesBetweenDates(this.StartDateTime, this.EndDateTime);
            if (totalmins != 0) {
                this.Duration = totalmins;
            }
        }
    }
    public GetMinutesBetweenDates(date1: Date, date2: Date) {
        var myResult: number = 0;

        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());
                var Daysdiff = Math.ceil(timeDiff / (1000 * 60));
                myResult = Daysdiff;
            }
        }
        return myResult;
    }

    get AllDayEvent() { return this.entityPM.AllDayEvent; }
    set AllDayEvent(value: boolean) {
        if (this.entityPM.AllDayEvent != value) {
            this.entityPM.AllDayEvent = value;
            this.OnAllDayEventChanged();
        }
    }

    public ApTimeVisibility: boolean = true; 

    private OnDurationChanged() {
        if ((this.SelectedDuration != null) && !this.SelectedDuration.IsDay) {
            this.entityPM.AllDayEvent = false;
        }
        this.ComputeEndDate();
    }
    private OnAllDayEventChanged() {
        if (this.AllDayEvent) {
            var days = 1;

            if (this.StartDateTime != null) {
                this.StartDateTime = DateTool.TruncateTime(this.StartDateTime);
            }

            if (this.EndDateTime != null) {
                this.EndDateTime = DateTool.TruncateTime(this.EndDateTime);
            }

            if (this.StartDateTime != null && this.EndDateTime != null && this.EndDateTime.valueOf() > this.StartDateTime.valueOf()) {
                days = DateTool.GetDaysBetweenDates(this.StartDateTime, this.EndDateTime);
            }
            this.Duration = (days * 24 * 60);
        }

        this.ComputeEndDate();
        this.ApTimeVisibility = this.IsTimeControlEnabled;
    }

    get IsTimeControlEnabled() { return !this.AllDayEvent; }
    private ComputeEndDate() {
        if (this.Duration == null) {
            this.EndDateTime = this.StartDateTime;
        }
        else {
            if (this.StartDateTime != null) {
                var startDateTime = DateTool.GetDateFormats(this.StartDateTime).DateParts.DateObject;
                var date: Date = new Date();
                date.setUTCFullYear(startDateTime.getUTCFullYear());
                date.setUTCMonth(startDateTime.getUTCMonth());
                date.setUTCDate(startDateTime.getUTCDate());
                date.setUTCHours(startDateTime.getUTCHours());
                date.setUTCMinutes(startDateTime.getUTCMinutes());
                date.setUTCSeconds(startDateTime.getUTCSeconds());
                date.setUTCMilliseconds(startDateTime.getUTCMilliseconds());
                date.setUTCMinutes(date.getUTCMinutes() + this.Duration);
                this.EndDateTime = date;
            }
        }
    }

    // Call Type
    //get SelectedCallType() { return this.CallTypesList.filter(d => d.Code == this.entityPM.CallTypeCode)[0]; }
    //set SelectedCallType(value: CallTypeList) {
    //    if (value == null) {
    //        this.CallTypeCode = null;
    //    }
    //    else {
    //        this.CallTypeCode = value.Code;
    //    }
    //}

    get CallTypeCode() { return this.entityPM.CallTypeCode; }
    set CallTypeCode(value: string) {
        if (this.entityPM.CallTypeCode != value) {
            this.entityPM.CallTypeCode = value;
        }
    }

    get CallWithId() { return this.entityPM.CallWithId; }
    set CallWithId(value: string) {
        if (this.entityPM.CallWithId != value) {
            this.entityPM.CallWithId = value;
            this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, AppTool.IsNullOrEmpty(value));
        }
    }
    get IsLeftVoiceMail() { return this.entityPM.IsLeftVoiceMail; }
    set IsLeftVoiceMail(value: boolean) {
        if (this.entityPM.IsLeftVoiceMail != value) {
            this.entityPM.IsLeftVoiceMail = value;
        }
    }
    private callWithContact: ContactList = null;
    get CallWithContact() { return this.callWithContact; }
    set CallWithContact(value: ContactList) {
        if (this.callWithContact != value) {
            this.callWithContact = value;
            this.OnContactChanged(value);
        }
    }

    public Email = "";
    public BusinessPhone = "";
    public Mobile = "";
    OnContactChanged(contact: ContactList) {
        this.Email ="";
        this.BusinessPhone = "";
        this.Mobile = "";
        if (contact) {
            this.Email = contact.Email;
            this.BusinessPhone = contact.BusinessPhone;
            this.Mobile = contact.Mobile;
        }
    }

    //Properties
    get Subject() { return this.entityPM.Subject; }
    set Subject(value: string) {
        if (this.entityPM.Subject != value) {
            this.entityPM.Subject = value;
        }
    }

    get Location() { return this.entityPM.Location; }
    set Location(value: string) {
        if (this.entityPM.Location != value) {
            this.entityPM.Location = value;
        }
    }

    get Description() { return this.entityPM.Description; }
    set Description(value: string) {
        if (this.entityPM.Description != value) {
            this.entityPM.Description = value;
        }
    }

    get OwnerId() { return this.entityPM.OwnerId; }
    set OwnerId(value: string) {
        if (this.entityPM.OwnerId != value) {
            this.entityPM.OwnerId = value;
            if (value == null) {
                this.entityPM.BusinessUnitId = null;
            }
            else {
                var listService: UserListService = new UserListService();
                listService.getSingleFromCache(value).subscribe(result => {
                    var list: UserList = result.Result;
                    if (list != null)
                        this.entityPM.BusinessUnitId = list.BusinessUnitId;
                });
            }

            this.SetUIProperties_Qwner();
        }
    }

    private owner: UserList = null;
    get Owner() { return this.owner; }
    set Owner(value: UserList) {
        if (this.owner != value) {
            this.owner = value; 
            //if (value == null) {
            //    this.entityPM.BusinessUnitId = null;
            //}
            //else {
            //    this.entityPM.BusinessUnitId = (this.owner == null ? null : this.owner.BusinessUnitId);
            //}      
        }
    }

    get DueDate() { return this.entityPM.DueDate; }
    set DueDate(value: Date) {
        this.entityPM.DueDate = value;
    }

    get PriorityCode() { return this.entityPM.PriorityCode; }
    set PriorityCode(value: string) {
        if (this.entityPM.PriorityCode != value) {
            this.entityPM.PriorityCode = value;
            var name = "";
            if (value != null) {
                var service = new ActivityPriorityListService();
                service.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list != null && list.length > 0) {
                            var priority = list.filter(d => d.Code == value)[0];
                            this.entityPM.PriorityName = priority.Name;
                        }
                    }
                });
            }
        }
    }

    get ActivityTimeTypeCode() { return this.entityPM.ActivityTimeTypeCode; }
    set ActivityTimeTypeCode(value: string) {
        if (this.entityPM.ActivityTimeTypeCode != value) {
            this.entityPM.ActivityTimeTypeCode = value;
        }
    }

    get PhoneNumber() { return this.entityPM.PhoneNumber; }
    set PhoneNumber(value: string) {
        if (this.entityPM.PhoneNumber != value) {
            this.entityPM.PhoneNumber = value;
        }
    }

    get CustomerId() { return this.entityPM.CustomerId; }
    set CustomerId(value: string) {
        if (this.entityPM.CustomerId != value) {
            this.entityPM.CustomerId = value;
            this.CallWithId = null;
            this.UpdateCustomerName(value);
        }
    }
    private UpdateCustomerName(value: string) {
        var myContactId: string = null;
        if (!AppTool.IsNullOrEmpty(value)) {
            var cardService: CardListService = new CardListService();
            cardService.getSingle(value).subscribe(result => {
                var card: CardList = result.Result;
                if (card != null) {
                    this.CustomerName = card.EnglishName;
                }
            });
        }
        else {
            this.CustomerName = null;
        }
    }

    get OpportunityId() { return this.entityPM.OpportunityId; }
    set OpportunityId(value: string) { this.entityPM.OpportunityId = value; }

    get QuoteId() { return this.entityPM.QuoteId; }
    set QuoteId(value: string) { this.entityPM.QuoteId = value; }

    get DueDateOffset() { return this.entityPM.DueDateOffset; }
    set DueDateOffset(value: number) {
        if (this.entityPM.DueDateOffset != value) {
            this.entityPM.DueDateOffset = value;
        }
    }

    //get DueDateDateFieldDefaultText() {
    //    if (!AppTool.IsNullOrEmpty(this.entityPM.DueDateDateField)) {
    //        var myField = window.ObjectFields.filter(f => f.FieldName == this.entityPM.DueDateDateField)[0];

    //        if (myField != null) {
    //            return myField.FullNameTextCodeDefaultText;
    //        }
    //    }  
    //}

    get DueDateDateField() { return this.entityPM.DueDateDateField; }
    set DueDateDateField(value: string) {
        if (this.entityPM.DueDateDateField != value) {
            this.entityPM.DueDateDateField = value;                      
        }
    }

    get BusinessProcessQueueId() { return this.entityPM.BusinessProcessQueueId; }
    set BusinessProcessQueueId(value: string) {
        if (this.entityPM.BusinessProcessQueueId != value) {
            this.entityPM.BusinessProcessQueueId = value;
        }
    }

    get TeamId() { return this.entityPM.TeamId; }
    set TeamId(value: string) {
        if (this.entityPM.TeamId != value) {
            this.entityPM.TeamId = value;
        }
    }

    private addCustomerVisibility = false;
    get AddCustomerVisibility() { return this.addCustomerVisibility; }
    set AddCustomerVisibility(value: boolean) { this.addCustomerVisibility = value; }

    public AddCustomerEnabled: boolean;

    get ViewCustomerEnabled()
    {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
            result = true;
        }
        return result;
    }

    SetFieldsEnabled() {
        var fieldIsEnabled = this.entityPM.IsOpen;
        this.UIProperties.SetEnabled("CallWithId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("EndDateTime", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("StartDateTime", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Location", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("EntityId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("OwnerId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("OrganizerId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("PriorityCode", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("ActivityTimeTypeCode", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("PhoneNumber", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Duration", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("AllDayEvent", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("MeetingSummary", this.ObjectTableName, fieldIsEnabled);
        if (this.entityPM.ActivityTypeCode == "TS" || this.entityPM.ActivityTypeCode == "CL") {
            this.UIProperties.SetEnabled("Description", this.ObjectTableName, true);
        }
        if (this.entityPM.ActivityTypeCode == "AP") {
            this.UIProperties.SetEnabled("MeetingSummary", this.ObjectTableName, true);

        }


        
      
        this.AddCustomerEnabled = fieldIsEnabled;
        this.AddContactEnabled = fieldIsEnabled;
        this.SetUIPropertiesInEditMode();
        this.SetUIProperties_GeneratedComponent();
    }
    private SetUIPropertiesInEditMode() {
        if (this.entityPM.ActivityTypeCode == "EI" || this.entityPM.ActivityTypeCode == "EO") {
            this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Subject", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ArchiveDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CreateDate", this.ObjectTableName, false);
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
                this.AddCustomerEnabled = false;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.entityPM.QuoteId)) {
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }

        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName,this.EndDateTime == null);
                    break;
                }

            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CallWithId));
                    break;
                }
        }

        var isOppertunityVisible = !AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, isOppertunityVisible);

        var isQuoteVisible = !AppTool.IsNullOrEmpty(this.entityPM.QuoteId) && AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, isQuoteVisible);
    }

    get ViewOpportunityVisibility() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
            result = true;
        }
        return result;
    }
    get ViewCustomerVisibility() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.entityPM.CustomerId)) {
            result = true;
        }
        return result;
    }
    get ViewQuoteVisibility() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.entityPM.QuoteId)) {
            if (AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
                result = true;
            }
        }
        return result;
    }
    get QuoteNumber() {
        return this.entityPM == null ? null : this.entityPM.QuoteNumber;
    }
    set QuoteNumber(value: string) {
        if (this.entityPM.QuoteNumber != value) {
            this.entityPM.QuoteNumber = value;
        }
    }
    get OpportunitySubject() { return this.entityPM == null ? null : this.entityPM.OpportunitySubject; }
    set OpportunitySubject(value: string) {
        if (this.entityPM.OpportunitySubject != value) {
            this.entityPM.OpportunitySubject = value;
        }
    }

    //RightToLeft
    get MeetingSummary() { return this.entityPM.MeetingSummary; }
    set MeetingSummary(value: string) {
        if (this.entityPM.MeetingSummary != value) {
            this.entityPM.MeetingSummary = value;
        }
    }

    get IsDescriptionRightToLeftEnabled() {
        var myResult = false;
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = true;
        }
        return myResult;
    }

    get IsMeetingSummaryRightToLeftEnabled() {
        var myResult = false;
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true && this.IsMettingSummeryVisibile) {
            myResult = true;
        }
        return myResult;
    }

    get MeetingSummaryRightToLeft() { return this.entityPM.MeetingSummaryRightToLeft; }
    set MeetingSummaryRightToLeft(value: boolean) {
        if (this.entityPM.MeetingSummaryRightToLeft != value) {
            this.entityPM.MeetingSummaryRightToLeft = value;
        }
    }

    public MeetingSummaryFlowDirection: string = "ltr";
    private GetMeetingSummaryFlowDirection() {
        var myResult = "ltr";

        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = "ltr";
            if (this.entityPM.MeetingSummaryRightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }

        this.MeetingSummaryFlowDirection = myResult;
    }

    public MeetingSummaryBackgroundAlignLeft = "transparent";
    public MeetingSummaryBackgroundAlignRight = "transparent";
    private GetMeetingSummaryBackgroundAlignLeft() {
        if (!AppTool.IsNullOrEmpty(this.MeetingSummaryFlowDirection)) {
            this.MeetingSummaryBackgroundAlignLeft = this.MeetingSummaryFlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    }
    private GetMeetingSummaryBackgroundAlignRight() {
        if (!AppTool.IsNullOrEmpty(this.MeetingSummaryFlowDirection)) {
            this.MeetingSummaryBackgroundAlignRight = this.MeetingSummaryFlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    }

    public AlignMeetingSummaryLeftClicked() {
        this.entityPM.MeetingSummaryRightToLeft = false;
        this.GetMeetingSummaryFlowDirection();
        this.RefreshTextAlgimentVariables();
    }

    public AlignMeetingSummaryRightClicked() {
        this.entityPM.MeetingSummaryRightToLeft = true;
        this.GetMeetingSummaryFlowDirection();
        this.RefreshTextAlgimentVariables();
    }

    private RefreshTextAlgimentVariables() {
        this.GetMeetingSummaryBackgroundAlignLeft();
        this.GetMeetingSummaryBackgroundAlignRight();
    }

    get DescriptionRightToLeft() { return this.entityPM.DescriptionRightToLeft; }
    set DescriptionRightToLeft(value: boolean) {
        if (this.entityPM.DescriptionRightToLeft != value) {
            this.entityPM.DescriptionRightToLeft = value;
        }
    }

    public DescriptionFlowDirection: string = "ltr";
    private GetDescriptionFlowDirection() {
        var myResult = "ltr";
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.entityPM.DescriptionRightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.DescriptionFlowDirection = myResult;
    }

    public DescriptionBackgroundAlignRight = "transparent";
    private DescriptionBackgroundAlignLeft = "transparent";
    private GetDescriptionBackgroundAlignRight() {
        if (!AppTool.IsNullOrEmpty(this.DescriptionFlowDirection)) {
            this.DescriptionBackgroundAlignRight = this.DescriptionFlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    }
    private GetDescriptionBackgroundAlignLeft() {
        if (!AppTool.IsNullOrEmpty(this.DescriptionFlowDirection)) {
            this.DescriptionBackgroundAlignLeft = this.DescriptionFlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    }

    public AlignDescriptionLeftClicked() {
        this.entityPM.DescriptionRightToLeft = false;
        this.GetDescriptionFlowDirection();
        this.RefreshDescriptionTextAlgimentVariables();
    }
    public AlignDescriptionRightClicked() {
        this.entityPM.DescriptionRightToLeft = true;
        this.GetDescriptionFlowDirection();
        this.RefreshDescriptionTextAlgimentVariables();
    }
    public RefreshDescriptionTextAlgimentVariables() {
        this.GetDescriptionBackgroundAlignLeft();
        this.GetDescriptionBackgroundAlignRight();
    }

    // Invitees
    private requiredBoxText;
    get RequiredBoxText() { return this.requiredBoxText; }
    set RequiredBoxText(value: string) {
        this.requiredBoxText = value;
    }
    private optionalBoxText;
    get OptionalBoxText() { return this.optionalBoxText; }
    set OptionalBoxText(value: string) {
        this.optionalBoxText = value;
    }
    public RequiredBoxTextColor;
    public OptionalBoxTextColor;
    public Invitees_Optional: OptionalData[] = [];
    public Invitees_Required: RequiredData[] = [];
    private BuildInviteesControl() {
        if (this.entityPM != null) {
            this.BuildRequiredEmailList();
            this.BuildOptionalEmailList();
        }
    }
    BuildRequiredEmailList() {
        this.Invitees_Required = [];
        this.RequiredBoxText = "";
        var list = this.entityPM.ActivityInvitees.filter(d => d.IsRequired == true);
        list.forEach(item => {
            this.Invitees_Required.push(new RequiredData(item));
        });
    }
    BuildOptionalEmailList() {
        this.Invitees_Optional = [];
        this.OptionalBoxText = "";
        var list = this.entityPM.ActivityInvitees.filter(d => d.IsRequired == false);
        list.forEach(item => {
            this.Invitees_Optional.push(new OptionalData(item));
        });
    }
    public AddInviteeClicked() {
        var windowArgs: InviteeArgs = new InviteeArgs();
        windowArgs.Entity = this.entityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Contacts List";
        logWindow.Width = window.innerWidth - 100;
        logWindow.Height = window.innerHeight - 100;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./CRMModules/CRMActivity/Components/NewEntity/AddEditInviteesComponent");
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d == "OK") {
                    this.errors = [];
                    this.CheckIsValidEmails(s.RequiredEmailBoxText);
                    this.CheckIsValidEmails(s.OptionalEmailBoxText);
                    if (this.errors.length == 0) {
                        var myRequiredEmailsList = [];
                        if (!AppTool.IsNullOrEmpty(s.RequiredEmailBoxText)) {
                            var requiredEmails = s.RequiredEmailBoxText.split(';');
                            if (requiredEmails != null) {
                                myRequiredEmailsList = requiredEmails.filter(d => !AppTool.IsNullOrEmpty(d));
                            }
                        }

                        var myOptionalEmailsList = [];
                        if (!AppTool.IsNullOrEmpty(s.OptionalEmailBoxText)) {
                            var optionalEmails = s.OptionalEmailBoxText.split(';');
                            if (optionalEmails != null) {
                                myOptionalEmailsList = optionalEmails.filter(d => !AppTool.IsNullOrEmpty(d));
                            }
                        }

                        var removedList: ActivityInviteePM[] = [];

                        s.entityPM.ActivityInvitees.forEach(item => {
                            if (item.IsRequired) {
                                if (!AppTool.IsNullOrEmpty(item.ContactId)) {
                                    var list = s.RequiredList.filter(d => d.Email == item.Email && d.ContactId == item.ContactId)[0];
                                    if (list == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }
                                else {
                                    var check = myRequiredEmailsList.filter(d => d == item.Email)[0];
                                    if (check == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }
                            }

                            else {
                                if (!AppTool.IsNullOrEmpty(item.ContactId)) {
                                    var list = s.OptionalList.filter(d => d.Email == item.Email && d.ContactId == item.ContactId)[0];
                                    if (list == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }

                                else {
                                    var check = myOptionalEmailsList.filter(d => d == item.Email)[0];
                                    if (check == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }
                            }
                        });

                        removedList.forEach(item => {
                            this.entityPM.RemoveActivityInvitee(item);
                        });

                        s.RequiredList.filter(d => !AppTool.IsNullOrEmpty(d.ContactId)).forEach(item => {
                            var check = this.entityPM.ActivityInvitees.filter(d => d.Email == item.Email && d.ContactId == item.ContactId && d.IsRequired == true)[0];
                            if (check == null) {
                                var entity = new ActivityInviteePM(null);
                                entity.ActivityId = this.entityPM.Id;
                                entity.Tenant = SessionLocator.Tenant;
                                entity.Email = item.Email;
                                entity.ContactId = item.ContactId;
                                entity.ContactName = item.ContactName;
                                entity.IsRequired = true;
                                this.entityPM.AddActivityInvitee(entity);
                            }
                        });

                        s.OptionalList.filter(d => !AppTool.IsNullOrEmpty(d.ContactId)).forEach(item => {
                            var check = this.entityPM.ActivityInvitees.filter(d => d.Email == item.Email && d.ContactId == item.ContactId && d.IsRequired == false)[0];
                            if (check == null) {
                                var entity = new ActivityInviteePM(null);
                                entity.ActivityId = this.entityPM.Id;
                                entity.Tenant = SessionLocator.Tenant;
                                entity.Email = item.Email;
                                entity.ContactId = item.ContactId;
                                entity.ContactName = item.ContactName;
                                entity.IsRequired = false;
                                this.entityPM.AddActivityInvitee(entity);
                            }
                        });

                        myRequiredEmailsList.forEach(email => {
                            if (!AppTool.IsNullOrEmpty(email)) {
                                var check = this.entityPM.ActivityInvitees.filter(d => d.Email == email && AppTool.IsNullOrEmpty(d.ContactId) && d.IsRequired == true)[0];
                                if (check == null) {
                                    var entity = new ActivityInviteePM(null);
                                    entity.ActivityId = this.entityPM.Id;
                                    entity.Tenant = SessionLocator.Tenant;
                                    entity.Email = email;
                                    entity.ContactId = null;
                                    entity.ContactName = null;
                                    entity.IsRequired = true;
                                    this.entityPM.AddActivityInvitee(entity);
                                }
                            }
                        });

                        myOptionalEmailsList.forEach(email => {
                            if (!AppTool.IsNullOrEmpty(email)) {
                                var check = this.entityPM.ActivityInvitees.filter(d => d.Email == email && AppTool.IsNullOrEmpty(d.ContactId) && d.IsRequired == false)[0];
                                if (check == null) {
                                    var entity = new ActivityInviteePM(null);
                                    entity.ActivityId = this.entityPM.Id;
                                    entity.Tenant = SessionLocator.Tenant;
                                    entity.Email = email;
                                    entity.ContactId = null;
                                    entity.ContactName = null;
                                    entity.IsRequired = false;
                                    this.entityPM.AddActivityInvitee(entity);
                                }
                            }
                        });

                        this.BuildInviteesControl();
                    }
                }
            });
        });
    }
    private errors = [];
    CheckIsValidEmails(mailsList: string) {
        var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var IsOk = true;

        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach((item) => {
                if (item) {
                    if (!EMAIL_REGEXP.test(item)) {
                        IsOk = false;
                        this.errors.push(item + " has invalid format");
                        return;
                    }
                }
            });
        }
        return IsOk;
    }
    private CurrentSession = SessionLocator.SelectedSession;
    // Commands 
    AddContactClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Contact";
        var args = new ContactInputTemplateArgs();
        args.CustomerId = this.CustomerId;
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var contact = s.EntityPM;
                if (contact != null) {
                    this.CallWithId = contact.Id;
                }
            });
        });
    }
    AddCustomerClicked() {
        this._entityResourceService.getEntityResourceByTableName("Customer").subscribe(response => {
            var str = TextCodeTranslator.Translate("General.O.NewEntity");
            str = "New Potential Customer";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = str;
            var args = new NewEntityArgs();
            logWindow.WindowArgs = args;
            logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    if (d) {
                        var customer = s.EntityPM;
                        if (customer != null) {
                            this.CustomerId = customer.Id;
                        }
                    }
                });
            });
        });
    }
    Validate() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.entityPM.BusinessUnitId)) {
            this.entityPM.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
        }

        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);

        if (this.entityPM.ActivityTypeCode != "TX") {
            if (AppTool.IsNullOrEmpty(this.OwnerId)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.OwnerId")));
            }
        }

        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    if (this.StartDateTime == null) {
                        errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.StartDateTime")));
                    }

                    if (this.EndDateTime == null) {
                        errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.EndDateTime")));
                    }

                    if (this.StartDateTime != null && this.EndDateTime != null) {
                        if (this.EndDateTime.valueOf() <= this.StartDateTime.valueOf()) {
                            var text = TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator.Translate("Activity.F.EndDateTime");
                            errors.push(text);
                        }
                    }

                    break;
                }

            case "TS":
                {
                    if (this.StartDateTime != null && this.entityPM.DueDate != null) {
                        var date1 = DateTool.GetDateFormats(this.entityPM.DueDate).DateParts.DateObject;
                        var date2 = DateTool.GetDateFormats(this.StartDateTime).DateParts.DateObject;

                        if (date1 != null && date2 != null && (date1.valueOf() <= date2.valueOf())) {
                            var text: string = TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator.Translate("Activity.F.DueDate");
                            errors.push(text);
                        }
                    }

                    break;
                }

            case "CL":
                {
                    if (AppTool.IsNullOrEmpty(this.CallWithId)) {
                        errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.CallWithId")));
                    }

                    break;
                }
        }
        return errors;
    }
    ViewEntityClicked(m: string) {
        if (m == "OPP") {
            if (!AppTool.IsNullOrEmpty(this.OpportunityId)) {
                this.ViewEntity("Opportunity", this.OpportunityId);
            }
        }
        else if (m == "CUS") {
            if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
                this.ViewEntity("Customer", this.CustomerId);
            }
        }
        else if (m == "QUT") {
            if (!AppTool.IsNullOrEmpty(this.QuoteId)) {
                this.ViewEntity("Quote", this.QuoteId);
            }
        }
    }
    ViewEntity(tableName: string, entityId: string) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, });
            });
    }

    //Email Out | In
    get ArchiveDate() {
        return this.entityPM.ArchiveDate;
    }
    get CreateDate() {
        return this.entityPM.CreateDate;
    }

    get SortingDate() {
        return this.entityPM.SortingDate;

    }
    get SenderEmail() { return this.entityPM == null ? null : this.entityPM.SenderEmail; }
    set SenderEmail(value: string) {
        if (this.entityPM.SenderEmail != value) {
            this.entityPM.SenderEmail = value;
        }
    }
    get SenderContactName() {
        return this.entityPM == null ? null : this.entityPM.SenderContactName;
    }
    get EmailFrom() {
        var myResult = "";
        if (!AppTool.IsNullOrEmpty(this.SenderContactName)) {
            myResult = this.SenderContactName;
        }

        else {
            myResult = this.SenderEmail;
        }
        return myResult;
    }
    get SenderEmailForeground() {
        var myResult = "#282E30";
        if (this.entityPM != null) {
            if (AppTool.IsNullOrEmpty(this.entityPM.SenderContactId)) {
                myResult = "#E53030";
            }
        }

        return myResult;
    }
    get EmailTO()
    {
        var myResult = "";
        this.entityPM.ActivityEmailRecipients.filter(d => d.RecipientTypeCode.toUpperCase() == "TO").forEach(item => {
            if (AppTool.IsNullOrEmpty(myResult)) {
                myResult += item.Email;
            }

            else {
                myResult += ";" + item.Email;
            }
        });
        return myResult;
    }
    get EmailCC() {
        var myResult = "";
        this.entityPM.ActivityEmailRecipients.filter(d => d.RecipientTypeCode.toUpperCase() == "CC").forEach(item => {
            if (AppTool.IsNullOrEmpty(myResult)) {
                myResult += item.Email;
            }
            else {
                myResult += ";" + item.Email;
            }
        });
        return myResult;
    }
    get EmailBCC() {
        var myResult = "";
        this.entityPM.ActivityEmailRecipients.filter(d => d.RecipientTypeCode.toUpperCase() == "BCC").forEach(item => {
            if (AppTool.IsNullOrEmpty(myResult)) {
                myResult += item.Email;
            }
            else {
                myResult += ";" + item.Email;
            }
        });
        return myResult;
    }
    get OwnerName() {
        return this.entityPM == null ? null : this.entityPM.OwnerName;
    }
    get CustomerName() {
        return this.entityPM == null ? null : this.entityPM.CustomerName;
    }
    set CustomerName(value: string) {
        if (this.entityPM.CustomerName != value) {
            this.entityPM.CustomerName = value
        }
    }
    get CCVisibility() {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(this.EmailCC)) {
            myResult = true;
        }
        return myResult;
    }
    get BCCVisibility() {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(this.EmailBCC)) {
            myResult = true;
        }
        return myResult;
    }
    get AttachmentsVisibility() {
        return this.entityPM.ActivityDocumentDatas.length > 0 ? true : false;
    } 
    public DocumentDatasList: AttachmentsArgs[] = [];
    BuildInternalAttachmentList() {
        if (this.entityPM != null) {
            var list: AttachmentsArgs[] = [];
            this.entityPM.ActivityDocumentDatas.forEach(item => {
                this.DocumentDatasList.push(new AttachmentsArgs(item));
            });
        }
    }
}

export class AttachmentsArgs {
    public DocumentDataPM: DocumentDataPM;
    constructor(documentDataPM: DocumentDataPM) {
        this.DocumentDataPM = documentDataPM;
    }

    get DocumentTypeName() { return this.DocumentDataPM.FileName; }
   
    ViewAttachment() {
      
        DownloadManager.DownloadPage(this.DocumentDataPM.DocumentId);
    }
}

export class OptionalData {
    private entity: ActivityInviteePM;
    constructor(entity: ActivityInviteePM) {
        this.entity = entity;
        this.check();
    }

    private check() {
        if (AppTool.IsNullOrEmpty(this.entity.ContactId)) {
            this.OptionalBoxText += this.entity.Email + ";";
            this.Tooltip = "";
            this.OptionalBoxTextColor = "rgb(229,48,48)";
        }
        else {
            this.OptionalBoxText += this.entity.ContactName + ";";
            this.Tooltip = this.entity.Email;
            this.OptionalBoxTextColor = "rgb(40,46,48)";
        }
    }
    public OptionalBoxText = "";
    public OptionalBoxTextColor = "";
    public Tooltip = "";
}

export class RequiredData {
    private entity: ActivityInviteePM;
    constructor(entity: ActivityInviteePM) {
        this.entity = entity;
        this.check();
    }
    private check() {
        if (AppTool.IsNullOrEmpty(this.entity.ContactId)) {
            this.RequiredBoxText += this.entity.Email + ";";
            this.Tooltip = "";
            this.RequiredBoxTextColor = "rgb(229,48,48)";
        }
        else {
            this.RequiredBoxText += this.entity.ContactName + ";";
            this.Tooltip = this.entity.Email;
            this.RequiredBoxTextColor = "rgb(40,46,48)";
        }
    }
    public RequiredBoxText = "";
    public RequiredBoxTextColor = "";
    public Tooltip = "";
}
