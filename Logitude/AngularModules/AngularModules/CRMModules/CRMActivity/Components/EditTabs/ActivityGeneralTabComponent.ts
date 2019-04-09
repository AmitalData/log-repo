import {Component, ViewChild, ViewContainerRef, ChangeDetectorRef} from '@angular/core';
import {ActivityPM} from '../../../../CRM/EntityPMs/ActivityPM';
import {ActivityNotePM} from '../../../../CRM/EntityPMs/ActivityNotePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ActivityInputTemplate} from '../NewEntity/ActivityInputTemplate';
import {ActivityInputArgs} from '../../../../CRM/Args'
import {AppTool, DateTool} from '../../../../Infrastructure/Tools'; 
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow'; 
import {CommunicationLogPMService} from '../../../../Common/Services/StandardPMs/CommunicationLogPMService'; 
import {CommunicationLogPM} from '../../../../Common/EntityPMs/CommunicationLogPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocsOutDataViewModel} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import {CommunicationLogPMViewModel} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/CommunicationLogPMViewModel';

@Component({
    moduleId: module.id,
    templateUrl: './ActivityGeneralTabComponent.html',
})

export class ActivityGeneralTabComponent extends BaseComponent {
    public EntityPM: ActivityPM = new ActivityPM();
    public ObjectTableName: string = "Activity";
    public ActivityNotesObslist: ActivityNoteItem [];
    public DataContext = this;

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ActivityNotesObslist = [];
        this.RunComponent();
        this.Listen();
    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.ActivityInputTemplate.entityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetFieldsEnabled();
                    this.ActivityInputTemplate.SetFieldsEnabled();
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.ActivityInputTemplate.entityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetFieldsEnabled();
                    this.ActivityInputTemplate.SetFieldsEnabled();
                }
            });
        }
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
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private ActivityInputTemplate: ActivityInputTemplate;
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./CRMModules/CRMActivity/Components/NewEntity/ActivityInputTemplate", this.viewContainerRef)
            .then(cmpRef => {
                this.ActivityInputTemplate = cmpRef.instance;
                var args = new ActivityInputArgs();
                args.Activity = this.EntityPM;
                args.TypeCode = this.EntityPM.ActivityTypeCode;
                args.IsEditMode = true;
                args.IsAddCustomerAllowed = true;
                this.ActivityInputTemplate.InitTemplate(args);
                this.Initialize();
            });
    }

    private Initialize() {
        this.BuildNotes();
        this.CommunicationLogVisibility = !AppTool.IsNullOrEmpty(this.EntityPM.CommunicationLogId) ? true : false;
        this.RefreshScreen();
    }

    //View Communication Log
    communicationLogVisibility =false;
    get CommunicationLogVisibility() { return this.communicationLogVisibility; }
    set CommunicationLogVisibility(value: boolean) { this.communicationLogVisibility = value; }

    ViewCommunicationLogClicked() {
        var service = new CommunicationLogPMService();
        service.get(this.EntityPM.CommunicationLogId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var log: CommunicationLogPM = myResponse.Result;
                if (log != null) {

                    var selectedInternalDocument = new DocsOutDataViewModel(null, this.EntityPM.Id, "", "", "", "", null, null, null, this.EntityPM, this.ObjectTableName);
                    selectedInternalDocument.SelectedCommunicationLogViewMode = new CommunicationLogPMViewModel(log);
                    this.SendHtmlDocument(selectedInternalDocument);
                }
            }
        });   
    }

    IsSendClose: boolean = false;
    SendHtmlDocument(SelectedInternalDocument: DocsOutDataViewModel) {
        this.IsSendClose = false;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 1000) sendWindowWidth = 1000;
        if (sendWindowHeight < 600) sendWindowHeight = 600;

        SelectedInternalDocument.EntityId = this.EntityPM.Id;

        SelectedInternalDocument.ModeSendDocument = "preview";
        SelectedInternalDocument.IsViewGeneralAttachment = true;
        var logWindow = new LogitudeWindow();
        logWindow.Width = SelectedInternalDocument.WindowWidth = sendWindowWidth;
        logWindow.Height = SelectedInternalDocument.WindowHeight = sendWindowHeight;
        logWindow.Title = "Document Editor";
        logWindow.DataContext = SelectedInternalDocument;
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            
        });
    }

    // Screens 
    private RefreshScreen() {
        this.SetUIProperties();
        this.SetFieldsEnabled();
        this.ActivityInputTemplate.SetFieldsEnabled();
        this.BuildNotes();
    }

    get ActivityNotesVisibility()
    {
        var myResult = false;
        if (this.EntityPM != null) {
            if (this.EntityPM.ActivityTypeCode == "AP" || this.EntityPM.ActivityTypeCode == "CL" || this.EntityPM.ActivityTypeCode == "TS") {
                myResult = true;
            }
        }
        return myResult;
    }
    get EmailControlsVisibility()
    {
        var myResult = false;
        if (this.EntityPM != null) {
            if (this.EntityPM.ActivityTypeCode == "EO" || this.EntityPM.ActivityTypeCode == "EI") {
                myResult = true;
            }
        }
        return myResult;
    }
    get OnEditModeVisibility() { return true; }

    public IsDataLoaded = false;
    SetUIProperties() {
        if (this.EntityPM.ActivityTypeCode == "EI" || this.EntityPM.ActivityTypeCode == "EO") {
            this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
                this.ActivityInputTemplate.AddCustomerEnabled = false;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }

        switch (this.EntityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.EntityPM.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName, this.EntityPM.EndDateTime == null);
                    break;
                }

            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.EntityPM.CallWithId));
                    break;
                }
        }

        var isOppertunityVisible = !AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId);
        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, isOppertunityVisible);

        var isQuoteVisible = !AppTool.IsNullOrEmpty(this.EntityPM.QuoteId) && AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, isQuoteVisible);
    }
    SetFieldsEnabled() {
        var fieldIsEnabled = this.EntityPM.IsOpen;
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
        this.ActivityInputTemplate.AddCustomerEnabled = fieldIsEnabled;
        this.ActivityInputTemplate.AddContactEnabled = fieldIsEnabled;

        this.ActivityNotesObslist.forEach(item => {
            item.Refresh();
        });
        this.IsDataLoaded = true;
    }

    get IsControlsEnabled() {
        var result = true;
        if (this.EntityPM != null) {
            if (!this.EntityPM.IsOpen) {
                result = false;
            }
        }
        return result;
    }
    get IsControlReadOnly() {
        var myResult = false;
        if (this.EntityPM != null) {
            if (!this.EntityPM.IsOpen) {
                myResult = true;
            }
        }
        return myResult;
    }
    get ViewOpportunityVisibility()
    {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
            result = true;
        }
        return result;
    }
    get ViewCustomerVisibility()
    {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            result = true;
        }
        return result;
    }
    get ViewQuoteVisibility() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
                result = true;
            }
        }
        return result;
    }

    get CustomerName() {
        return this.EntityPM == null ? null : this.EntityPM.CustomerName;
    }
    get QuoteNumber() {
        return this.EntityPM == null ? null : this.EntityPM.QuoteNumber;
    }
    set QuoteNumber(value: string) {
        if (this.EntityPM.QuoteNumber != value) {
            this.EntityPM.QuoteNumber = value;
        }
    }
    get OpportunitySubject() { return this.EntityPM == null ? null : this.EntityPM.OpportunitySubject; }
    set OpportunitySubject(value:string)
    {
        if (this.EntityPM.OpportunitySubject != value) {
            this.EntityPM.OpportunitySubject = value;
        }
    }
    get CustomerId() { return this.EntityPM == null ? null : this.EntityPM.CustomerId; }
    set CustomerId(value:string)
    {
        if (this.EntityPM.CustomerId != value) {
            this.EntityPM.CustomerId = value;
        }
    }
    get OpportunityId() { return this.EntityPM == null ? null : this.EntityPM.OpportunityId; }
    set OpportunityId(value:string)
    {
        if (this.EntityPM.OpportunityId != value) {
            this.EntityPM.OpportunityId = value;
        }
    }
    get QuoteId() { return this.EntityPM == null ? null : this.EntityPM.QuoteId; }
    set QuoteId(value:string)
    {
        if (this.EntityPM.QuoteId != value) {
            this.EntityPM.QuoteId = value;
        }
    }

    //Notes 
    get NoNotesVisibility()
    {
        var myResult = false;
        if (this.ActivityNotesObslist.length == 0) {
            myResult = true;
        }
        return myResult;
    }

    public SelectedNote: ActivityNoteItem;
    public  BuildNotes() {
        this.ActivityNotesObslist = [];
        this.EntityPM.ActivityNotes.forEach(item => {
            this.ActivityNotesObslist.push(new ActivityNoteItem(this.EntityPM, item, false,this));
        });
    }

    public IsEditToolVisible = false;
   
    EditNoteClicked(item: ActivityNoteItem) {
        if (item != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit Activity Note";
            var datacontext = new ActivityNoteItem(item.activityPM, item.entityPM, false,this);
            logWindow.DataContext = datacontext;
            logWindow.ShowHelpIcon = true;
            logWindow.HelpText = "The maximum number of characters allowed in this field is 500.";
            logWindow.Width = 450;
            logWindow.Height = 320;
            logWindow.Show('./CRMModules/CRMActivity/Components/EditTabs/AddEditActivityNotesComponent');
        }
    }

    DeleteNoteClicked(item: ActivityNoteItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this note?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (item != null) {
                    if (this.EntityPM.ActivityNotes.indexOf(item.entityPM) != -1) {
                        this.EntityPM.RemoveActivityNote(item.entityPM);
                        this.BuildNotes();
                    }
                }
            }
        });
    }

    AddNote() {
        var maxDate: Date = new Date();
        var nowData = DateTool.GetCurrentDateTimeAsUtc();

        if (this.EntityPM.ActivityNotes.length > 0) {
            this.EntityPM.ActivityNotes.forEach(item => {
                if (item.UpdateDate.valueOf > maxDate.valueOf) {
                    maxDate = item.UpdateDate;
                }
            });
        }

        if (maxDate == null) {
            maxDate = nowData;
        }

        else if (maxDate < nowData) {
            maxDate = nowData;
        }

        else {
            maxDate.setUTCHours(maxDate.getUTCHours() + 1);
        }

        var newNote = new ActivityNotePM(null);
        newNote.Tenant = SessionLocator.Tenant;
        newNote.ActivityId = this.EntityPM.Id;
        newNote.CreateDate = maxDate;
        newNote.UpdateDate = maxDate;
        newNote.CreatedByUserId = SessionLocator.LoggedUserId;
        newNote.UpdatedByUserId = SessionLocator.LoggedUserId;
        newNote.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        newNote.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        var datacontext = new ActivityNoteItem(this.EntityPM, newNote, true,this);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Note";
        logWindow.DataContext = datacontext;
        logWindow.ShowHelpIcon = true;
        logWindow.HelpText = "The maximum number of characters allowed in this field is 500.";
        logWindow.Width = 450;
        logWindow.Height = 320;
        logWindow.Show('./CRMModules/CRMActivity/Components/EditTabs/AddEditActivityNotesComponent');
    }

    // Commands 
    ViewEntityClicked(m: string) {
        if (m == "OPP") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
                this.ViewEntity("Opportunity", this.EntityPM.OpportunityId);
            }
        }
        else if (m == "CUS") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                this.ViewEntity("Customer", this.EntityPM.CustomerId);
            }
        }
        else if (m == "QUT") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
                this.ViewEntity("Quote", this.EntityPM.QuoteId);
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
}

export class ActivityNoteItem extends BaseComponent {
    public activityPM: ActivityPM = new ActivityPM();
    public entityPM: ActivityNotePM = new ActivityNotePM(null);
    public isNew: boolean;
    public savedNotes: string;
    public isDataEdited: boolean;
    public DataContext: ActivityNoteItem = this;

    constructor(activityPM: ActivityPM, entityPM: ActivityNotePM, isNew: boolean, public father: ActivityGeneralTabComponent) {
        super();
        this.activityPM = activityPM;
        this.entityPM = entityPM;
        this.isNew = isNew;
        this.GetIsControlsEnabled();
    }

    IsControlsEnabled = false;
    GetIsControlsEnabled() {
        var result = true;
        if (!this.activityPM.IsOpen) {
            result = false;
        }
        return result;
    }

    get PostToFollowersVisibility() {
        var myResult = false;
        if (this.isNew) {
            myResult = true;
        }
        return myResult;
    }

    get PostToFollowers() { return this.entityPM.PostToFollowers; }
    set PostToFollowers(value: boolean) {
        if (this.entityPM.PostToFollowers != value) {
            this.entityPM.PostToFollowers = value;

        }
    }

    // Properties
    get UpdateDate() { return this.entityPM.UpdateDate; }
    set UpdateDate(value: Date) {
        if (this.entityPM.UpdateDate != value) {
            this.entityPM.UpdateDate = value;

        }
    }

    get UpdatedByUserId() { return this.entityPM.UpdatedByUserId; }
    set UpdatedByUserId(value: string) {
        if (this.entityPM.UpdatedByUserId != value) {
            this.entityPM.UpdatedByUserId = value;
        }
    }

    get UpdatedByUserName() { return this.entityPM.UpdatedByUserName; }
    set UpdatedByUserName(value: string) {
        if (this.entityPM.UpdatedByUserName != value) {
            this.entityPM.UpdatedByUserName = value;
        }
    }

    get CreateDate() { return this.entityPM.CreateDate; }
    set CreateDate(value: Date) {
        if (this.entityPM.CreateDate != value) {
            this.entityPM.CreateDate = value;
        }
    }

    get CreatedByUserName() { return this.entityPM.CreatedByUserName; }
    set CreatedByUserName(value: string) {
        if (this.entityPM.CreatedByUserName != value) {
            this.entityPM.CreatedByUserName = value;
        }
    }

    get Notes() { return this.entityPM.Notes; }
    set Notes(value: string) {
        if (this.entityPM.Notes != value) {
            this.entityPM.Notes = value;
            this.isDataEdited = true;
        }
    }

    get DateLabel() {
        var myResult = "Created by ";

        if (this.entityPM.CreateDate != this.entityPM.UpdateDate) {
            myResult = "Modified by ";
        }
        return myResult;
    }

    public Refresh() {
        this.GetIsControlsEnabled();
    }
}
