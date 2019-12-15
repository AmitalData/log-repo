declare var window: any;
import {Component, Output, EventEmitter, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {InfraSettings} from '../../../../../Infrastructure/Utilities/InfraSettings';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {EntityPMServiceResponse} from '../../../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {CorrespondencePM} from '../../../../../CRM/EntityPMs/CorrespondencePM';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {TenantPM} from '../../../../../Common/EntityPMs/TenantPM';
import {SendEmailArgs, TicketStagesArgs} from '../../../../../CRM/Args';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {UserList} from '../../../../../Common/EntityLists/UserList';
import {TicketStageList} from '../../../../../CRM/EntityLists/TicketStageList';
import {TicketStageListService} from '../../../../../CRM/Services/StandardLists/TicketStageListService';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {ContactList} from '../../../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../../../Common/Services/StandardLists/ContactListService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMDomainService} from '../../../../../CRM/Services/CRMDomainService';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DocumentsFilingPM} from '../../../../../Common/EntityPMs/DocumentsFilingPM';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {DocumentTypeListService} from '../../../../../Common/Services/StandardLists/DocumentTypeListService';
import {DocumentsFilingExtendedPMService} from '../../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {UserListService} from '../../../../../Common/Services/StandardLists/UserListService';
import {CorrespondencePMService} from'../../../../../CRM/Services/StandardPMs/CorrespondencePMService';
import {TicketPMService} from '../../../../../CRM/Services/StandardPMs/TicketPMService';
import {TicketClosureArgs} from '../../../../../CRM/Args';

@Component({
    moduleId: './CRMModules/CRMTickets/Components/EditTabs/MainTab/',
    templateUrl: 'SendEmailComponent.html',
    providers: [DocumentTypeListService, DocumentsFilingExtendedPMService]
})

export class SendEmailComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Correspondence";
    public DataContext: SendEmailComponent = this;
    public EntityPM: CorrespondencePM;
    public TenantPM: TenantPM;
    public ObjectTable: any;
    public TicketObjectTable: any;
    public Ticket: TicketPM;
    public IsInternal: boolean;
    private contactId: string;
    public AttachmentsList: AttachmentsArgs[] = [];
    public UsersList: UserList[] = [];
    private InternalCorrespondenceLinesCount: number;
    public ValidationErrorsList: string[];

    @Output() OnCloseAttachmentDocsInEvent: EventEmitter<any> = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeListService: DocumentTypeListService, public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService) {
        super();
        this.TenantPM = InfraSettings.TenantPM;
        this.TicketObjectTable = window.ObjectTables.filter(x => x.Name === "Ticket")[0];
    }
    ngOnInit() {

    }
    SetWindowArgs(args: SendEmailArgs) {
        if (args != null) {
            this.Ticket = args.Ticket;
            this.ContactEmail = this.Ticket.ContactEmail;
            this.IsInternal = args.IsInternal;
            this.contactId = this.Ticket.ContactId;
            this.InternalCorrespondenceLinesCount = args.InternalCorrespondenceLinesCount;
            this.Initialize();
        }
    }
    Initialize() {
        this.InitializeLists();
        this.InitializeServices();
        this.CreateCorrespondence();
        this.InternalExternalEmailChecking();
        this.GetDocumentType();
        if (this.IsInternal) {
            if (AppTool.IsNullOrEmpty(this.InternalUsers)) {
                this.BuildSaveStages();
            }
            else {
                this.BuildSendStages();
            }
        }
        else {
            this.BuildSendStages();
        }

        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
    }
    private GetAllUsers() {
        var filters = new ApiQueryFilters();
        this.UserListService.getAllFromCache(filters).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this.UsersList = myResponse.Result;
            }
        });
    }
    private InitializeLists() {
        this.AttachmentsList = [];
        this.UsersList = [];
        //this.InternalUsersSelectedList = [];
        //this.InternalUsersList = [];
        this.ToList = [];
        //this.CCsList = [];
        //this.ContactsList = [];
    }
    private ContactListService: ContactListService;
    private CRMDomainservice: CRMDomainService;
    private UserListService: UserListService;
    InitializeServices() {
        this.ContactListService = new ContactListService();
        this.CRMDomainservice = new CRMDomainService();
        this.UserListService = new UserListService();
    }
    GetDocumentType() {
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = this.EntityPM.Tenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res: ServiceResponse) => {
            if (!res.HasError) {
                var myResult = res.Result;
                var item = myResult.filter(d => d.Code == "USA")[0];
                this.DocumentTypeId = item.Id;
            }
        });
    }
    private documentTypeId: string = "";
    get DocumentTypeId() {
        return this.documentTypeId;
    }
    set DocumentTypeId(value: string) {
        this.documentTypeId = value;
    }
    CheckTicketCorrespondenceNumbers() {
        if (this.InternalCorrespondenceLinesCount == 1 && !this.EntityPM.IsInternal) {
            this.Ticket.FirstResponseTime = DateTool.GetCurrentDateTimeAsUtc();
        }
    }
    CreateCorrespondence() {
        var isRightToLeft = false;
        if (SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
            isRightToLeft = true;
        }
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new CorrespondencePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.EntityId = this.Ticket.Id;
        this.EntityPM.IsInternal = this.IsInternal;
        this.EntityPM.NotifyMe = false;
        this.EntityPM.NotifyOwner = true;
        this.EntityPM.Direction = "O";
        this.EntityPM.CreatedByContactId = SessionInfo.LoggedUserId;
        this.ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        this.EntityPM.ObjectTableId = this.ObjectTable.Id;
        this.EntityPM.RightToLeft = isRightToLeft;
    }

    // Props
    private correspondenceLine: string;
    get CorrespondenceLine() { return this.correspondenceLine; }
    set CorrespondenceLine(value: string) {
        if (this.correspondenceLine != value) {
            this.correspondenceLine = value;
        }
    }

    //public CCs: string;
    public Bcc: string;

    get InternalUsers() { return this.EntityPM.InternalUsers; }
    set InternalUsers(value: string) {
        if (this.EntityPM.InternalUsers != value) {
            this.EntityPM.InternalUsers = value;
            if (this.IsInternal) {
                if (AppTool.IsNullOrEmpty(value)) {
                    this.BuildSaveStages();
                }
                else {
                    this.BuildSendStages();
                }
            }
        }
    }

    get CCs() { return this.EntityPM.CCs; }
    set CCs(value: string) {
        if (this.EntityPM.CCs != value) {
            this.EntityPM.CCs = value;
        }
    }

    public ToHeaderEnabled: boolean;

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    public ContactEmail: string;
    public ContactId: string;

    get NotifyMe() { return this.EntityPM.NotifyMe; }
    set NotifyMe(newValue: boolean) {
        if (this.EntityPM.NotifyMe != newValue) {
            this.EntityPM.NotifyMe = newValue;
        }
    }

    get NotifyOwner() { return this.EntityPM.NotifyOwner; }
    set NotifyOwner(newValue: boolean) {
        if (this.EntityPM.NotifyOwner != newValue) {
            this.EntityPM.NotifyOwner = newValue;
        }
    }

    get ToVisibility() {
        var myResult = true;

        if (this.IsInternal) {
            myResult = false;
        }
        return myResult;
    }
    get CcVisibility() {
        var myResult = true;
        if (this.IsInternal) {
            myResult = false;
        }
        return myResult;
    }
    get NotifyOwnerVisibility() {
        var myResult = true;
        var loggedId = SessionLocator.LoggedUserId;// TenantContext.Current.LoggedContactId;
        if (this.Ticket.OwnerId == loggedId) {
            myResult = false;
            this.NotifyOwner = false;
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

    // Align Commands
    public FlowDirection: string = "ltr";
    private GetFlowDirection() {
        var myResult = "ltr";
        if (SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.EntityPM.RightToLeft) {
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
        this.EntityPM.RightToLeft = false;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
    }
    public AlignRightClicked() {
        this.EntityPM.RightToLeft = true;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
    }
    public RefreshTextAlgimentVariables() {
        this.GetBackgroundAlignLeft();
        this.GetBackgroundAlignRight();
    }

    // Stages
    public StagesList: TicketStagesArgs[];
    private stageButtonContent: TicketStagesArgs = new TicketStagesArgs();
    get StageButtonContent() {
        return this.stageButtonContent;
    }
    set StageButtonContent(value: TicketStagesArgs) {
        this.stageButtonContent = value;
    }
    private stageButtonCode: string = "";
    get StageButtonCode() {
        return this.stageButtonCode;
    }
    set StageButtonCode(value: string) {
        this.stageButtonCode = value;
    }
    private isOpened = false;
    get IsOpened() {
        return this.isOpened;
    }
    set IsOpened(value: boolean) {
        this.isOpened = value;
    }
    private BuildSendStages() {
        this.StagesList = [];
        this.GetTicketStageMethod("Send and set as ");
    }
    private BuildSaveStages() {
        this.StagesList = [];
        this.GetTicketStageMethod("Save as ");
    }
    private GetTicketStageMethod(msg: string) {
        var myService: TicketStageListService = new TicketStageListService();
        myService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var stages: TicketStageList[] = resp.Result;
                stages.forEach(item => {
                    var IsEnabled = true;

                    if ((!FeatureLocator.HasFeaturePermession("Ticket", "TicketClosure") && (item.Code == "RE" || item.Code == "CS")) ||
                        (!FeatureLocator.HasFeaturePermession("Ticket", "SaveAsClosed") && item.Code == "CS") ||
                        (!FeatureLocator.HasFeaturePermession("Ticket", "SaveAsResolved") && item.Code == "RE") ||
                        (!FeatureLocator.HasFeaturePermession("Ticket", "SaveAsOpen") && item.Code == "OP")) {
                        IsEnabled = false;
                    }

                    var stage = new TicketStagesArgs();
                    stage.Code = item.Code;
                    stage.Name = msg + item.Name;
                    stage.IsEnabled = IsEnabled;
                    stage.ItemOpacity = IsEnabled ? 1 : 0.5;
                    this.StagesList.push(stage);
                    if (this.Ticket.StageCode == stage.Code) {
                        this.StageButtonContent = stage;
                    }
                });

                //this.StageButtonContent.Name = "Save as " + this.EntityPM.StageName;
                this.StageButtonCode = this.Ticket.StageCode;
            }
        });
    }
    TicketStageSelected(option) {
        if (option != null) {
            this.StageButtonContent = option;
            this.StageButtonCode = option.Code;
            this.IsOpened = false;
            this.SendEmail();
        }
    }
    SendEmail() {
        this.AddCorrespondenceLine(this.IsInternal);
    }
    AddCorrespondenceLine(isInternal: boolean) {
        var errors = [];
        if (this.CCs != null) {
            //this.CCs = "";
            var CcEmails = [];
            this.CCs.split(';').forEach(item => {
                CcEmails.push(item.toLocaleLowerCase());
            });
            CcEmails.forEach(item => {
                if (!this.CheckIsValidEmails(item)) {
                    errors.push("\"" + item + "\" email address is not recognised.");
                }
            });
        }
        if (this.InternalUsers != null) {
            var InternalUsersEmails = [];
            this.InternalUsers.split(';').forEach(item => {
                InternalUsersEmails.push(item.toLocaleLowerCase());
            });
            InternalUsersEmails.forEach(item => {
                if (!this.CheckIsValidEmails(item)) {
                    errors.push("\"" + item + "\" email address is not recognised.");
                }
            });
        }

        this.DuplicateEmailValidation(CcEmails, InternalUsersEmails, errors);

        this.EntityPM.Description = this.CorrespondenceLine;
        this.EntityPM.HTMLFullBody = this.CorrespondenceLine;

        if (!AppTool.IsNullOrEmpty(this.Bcc)) {
            this.EntityPM.Bcc = this.Bcc;
        }

        if (!AppTool.IsNullOrEmpty(this.InternalUsers)) {
            this.EntityPM.InternalUsers = this.InternalUsers;
        }
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.Description)) {
            errors.push("Description field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.EmailsValidation != null && this.EmailsValidation.length > 0) {
            this.EmailsValidation.forEach(item => {
                if (this.ValidationErrorsList == null) {
                    this.ValidationErrorsList = [];
                }
                this.ValidationErrorsList.push(item);
            });
        }
        if (this.ValidationErrorsList.length == 0) {
            //Update Ticket Stage
            var myService: CRMDomainService = new CRMDomainService();
            myService.GetTicketOwnerPermission(this.Ticket.OwnerId, this.Ticket.OwnerName).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myService: TicketStageListService = new TicketStageListService();
                    myService.getAll().subscribe((resp: any) => {
                        if (!resp.HasError) {
                            var list = resp.Result;
                            var myStage = list.filter(d => d.Code == this.StageButtonCode && d.Tenant == SessionLocator.TenantPM.Id)[0];
                            if (myStage != null) {
                                this.Ticket.StageId = myStage.Id;
                                this.Ticket.StageCode = myStage.Code;
                                this.Ticket.StageName = myStage.Name;
                            }
                            this.CheckTicketCorrespondenceNumbers(); // Fix Ticket First Resopnse Time

                            // Ticket Ccs & Internal users
                            var ccs: string = this.AddNewEmails(this.EntityPM.CCs, this.Ticket.CCs);
                            var internals: string = this.AddNewEmails(this.EntityPM.InternalUsers, this.Ticket.InternalUsers);

                            if (!AppTool.IsNullOrEmpty(ccs)) {
                                this.Ticket.CCs = ccs;
                            }
                            if (!AppTool.IsNullOrEmpty(internals)) {
                                this.Ticket.InternalUsers = internals;
                            }
                            this.CreateAttachments();
                            if (errors.length == 0) {
                                if (this.StageButtonCode != "CS") {
                                    this.Ticket.IsClosed = false;
                                }
                                // Resolve Case or closed case
                                if (this.StageButtonCode == "RE" || this.StageButtonCode == "CS") {
                                    var currentDateTime = DateTool.GetCurrentDateTimeAsUtc();
                                    if (this.StageButtonCode == "RE") {
                                        if (this.Ticket.FirstResolveDate == null) {
                                            this.Ticket.FirstResolveDate = currentDateTime;
                                        }

                                        if (this.Ticket.FirstResponseTime == null) {
                                            this.Ticket.FirstResponseTime = currentDateTime;
                                        }

                                        this.Ticket.FullResolvedTime = currentDateTime;
                                    }

                                    if (this.StageButtonCode == "CS") {
                                        if (this.Ticket.FirstResolveDate == null) {
                                            this.Ticket.FirstResolveDate = currentDateTime;
                                        }

                                        if (this.Ticket.FullResolvedTime == null) {
                                            this.Ticket.FullResolvedTime = currentDateTime;
                                        }

                                        if (this.Ticket.FirstResponseTime == null) {
                                            this.Ticket.FirstResponseTime = currentDateTime;
                                        }

                                        if (this.Ticket.FirstCloseDate == null) {
                                            this.Ticket.FirstCloseDate = currentDateTime;
                                        }

                                        this.Ticket.LastCloseDate = currentDateTime;
                                    }

                                    this.ClosuerWindow();
                                }
                                else {
                                    this.SaveChanges();
                                }
                            }
                        }
                    });
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }

    DuplicateEmailValidation(ccEmails, internalUsersEmails, errors) {
        if (ccEmails != null && internalUsersEmails != null) {
            var duplicate_emails = ccEmails.filter(x => internalUsersEmails.includes(x));
            if (duplicate_emails != null && duplicate_emails.length > 0) {
                var duplicateEmailsError = "";
                duplicate_emails.forEach(item => {
                    duplicateEmailsError += item + ", ";
                });

                errors.push(duplicateEmailsError.replace(/, \s*$/, "") + " emails are duplicate.");
            }
        }

        if ((ccEmails != null && ccEmails.indexOf(this.ContactEmail.toLocaleLowerCase()) > -1) || (internalUsersEmails != null && internalUsersEmails.indexOf(this.ContactEmail.toLocaleLowerCase()) > -1)) {
            errors.push(this.ContactEmail + " contact email is duplicate.");
        }
    }

    ClosuerWindow() {
        var windowTitle = "Ticket Closure";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        var args = new TicketClosureArgs();
        args.Ticket = this.Ticket;
        args.StageCode = this.StageButtonCode;
        logWindow.WindowArgs = args;
        logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/Others/TicketClosureComponent');
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (comp.IsOkClosed == true) {
                        this.SaveChanges();
                    }
                }
            });
        });
    }
    private AddNewEmails(correspondencelist: string, ticketlist: string) {
        var emails = "";
        var myList1 = [];
        if (!AppTool.IsNullOrEmpty(correspondencelist)) {
            myList1 = correspondencelist.split(';');
        }
        var myList2 = [];
        if (!AppTool.IsNullOrEmpty(ticketlist)) {
            myList2 = ticketlist.split(';');
        }

        var newList = [];
       // newList = myList1.Except(myList2);
        newList = myList1.filter(item => myList2.indexOf(item) < 0);

       if (newList != null && newList.length > 0) {
            emails = newList.join(";");
        }
        return emails;
    }
    private SaveChanges() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetTicketOwnerPermission(this.Ticket.OwnerId, this.Ticket.OwnerName).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CurrentSession.StartBusyIndicator("Sending");
                var myService: CorrespondencePMService = new CorrespondencePMService();
                myService.insert(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                    if (myRespone != null) {
                        if (!myRespone.HasError) {
                            this.UpdateTicket();
                        }
                        else {
                            this.CurrentSession.StopBusyIndicator();
                            this.ValidationErrorsList = myRespone.ErrorsArray;
                            this.CurrentSession.StopBusyIndicator();
                        }
                    }
                });
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
    UpdateTicket() {
        var myService: TicketPMService = new TicketPMService();
        myService.update(this.Ticket).subscribe((myRespone: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
                else {
                    this.ValidationErrorsList = myRespone.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    // Internal / External Email Process
    InternalExternalEmailChecking() {
        if (this.IsInternal) {
            this.ToHeaderEnabled = true;
            //this.ContactEmail = "";
            this.EntityPM.InternalUsers = "";
            this.EntityPM.CCs = "";
        }

        if (!this.IsInternal) {
            this.ToHeaderEnabled = false;
            this.getContact(this.contactId);

            this.EntityPM.InternalUsers = this.Ticket.InternalUsers;
            this.EntityPM.CCs = this.Ticket.CCs;
        }
    }
    public To: string = "";
    public ToList = [];
    private getContact(contact) {
        this.ContactListService.getSingle(contact).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var contact: ContactList = myResponse.Result;
                this.To = contact.Email;
                this.ToList.push(contact);
            }
        });
    }

    CheckIsValidEmails(email: string) {
        var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var IsOk = true;
        if (email) {
            if (!EMAIL_REGEXP.test(email)) {
                IsOk = false;
                return;
            }
        }
        return IsOk;
    }
    private EmailsValidation = [];
    ValidateEmails(errors: string[]) {
        this.EmailsValidation = errors;
    }

    // Attachements
    get AttachmentsVisibility()
    {
        var myResult = false;
        if (this.AttachmentsList.length > 0) {
            myResult = true;
        }
        return myResult;
    }
    documentInPMs: DocumentsFilingPM[];
    IsCloseAttachmentDocsIn: boolean;
    public AttachInternalFile() {
        this._documentsFilingExtendedPMService.getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(this.Ticket.Id, null, this.TicketObjectTable.Id, "I", SessionLocator.Tenant, true).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentInPMs = myResult;
                    if (this.documentInPMs.length > 0) {
                        this.IsCloseAttachmentDocsIn = false;
                        this.OnCloseAttachmentDocsInEvent.subscribe(($event: any) => {
                            if (!this.IsCloseAttachmentDocsIn && $event) {
                                this.IsCloseAttachmentDocsIn = true;
                                this.BliudInternalAttachmentList($event);
                            }
                        });

                        var windowArgs: any = {};
                        windowArgs.DocumentsFilingList = this.documentInPMs;
                        windowArgs.OnCloseAttachmentDocsInEvent = this.OnCloseAttachmentDocsInEvent;
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 800;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = "Attach Docs In";
                        logitudeWindow.WindowArgs = windowArgs;
                        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentDocsInComponent");
                    }
                    else {
                        this.ShowMessage("No Docs In found");
                    }
                }
            }
        });
    }
    BliudInternalAttachmentList(attachmentsLists: any) {
        if (attachmentsLists && attachmentsLists.length > 0) {
            attachmentsLists.forEach((item) => {
                if (this.AttachmentsList == null) {
                    this.AttachmentsList = [];
                }
                var attach = new DocumentsFilingPM();
                attach.FileExtension = item.FileExtension;
                attach.FileName = item.DocumentTypeCopyNameWithDocumentTypeName;
                attach.Tenant = item.Tenant;
                attach.Id = item.DocumentFilingId;
                attach.EntityId = item.EntityId;
                attach.FileSize = item.FileSize;
                this.AttachmentsList.push(new AttachmentsArgs(attach));
            });
        }
    }
    public ShowMessage(message: string, title: string = "") {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    }
    private CreateAttachments() {
        var myList = [];
        this.AttachmentsList.forEach(item => {
            myList.push(item.DocumentFilingId);
        });
        this.EntityPM.Attachments = myList;
    }
    IsLoadUploader: boolean;
    CurrentDocument: DocumentsFilingPM;
    AttachExternalFile() {
        //if (!this.CurrentDocument) {
        this._documentsFilingExtendedPMService.CreateDocumentsFiling(this.documentTypeId, this.EntityPM.EntityId, "", "", this.TicketObjectTable.Id, "I", SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.CurrentDocument = myResult;
                    this.ShowUplpaderAttachment();
                }
            }
        });
        //}
        //else {
        //    this.ShowUplpaderAttachment();
        //}
    }
    public ShowUplpaderAttachment() {
        this.IsLoadUploader = true;

        var windowArgs: any = {};
        windowArgs.EntityId = this.EntityPM.EntityId;
        var objectTable = window.ObjectTables.filter(x => x.Name === "Ticket")[0];
        windowArgs.ObjectTableId = objectTable.Id;
        windowArgs.RequsetPageName = "SendDocument";
        windowArgs.TiggerViewModel = this;
        windowArgs.CurrentDocument = this.CurrentDocument;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
    }
    public OnUploadComplete(uploader: any) {
        if (uploader.IsUploadDone && uploader.CurrentDocument) {
            if (this.AttachmentsList == null) {
                this.AttachmentsList = [];
            }
            if (uploader && uploader.CurrentDocument) {
                this.CurrentDocument = uploader.CurrentDocument;
                this.CurrentDocument.FileExtension = uploader.FileExtension;
                this.AttachmentsList.push(new AttachmentsArgs(this.CurrentDocument));
            }
        }
    }
}

export class AttachmentsArgs {
    public DocumentFilingPM: DocumentsFilingPM;
    constructor(documentFilingPM: DocumentsFilingPM) {
        this.DocumentFilingPM = documentFilingPM;
    }

    get FileName() { return this.DocumentFilingPM.FileName;}
    get FileExtension() {  return this.DocumentFilingPM.FileExtension;   }
    get Tenant() {return this.DocumentFilingPM.Tenant;}
    get DocumentFilingId(){ return this.DocumentFilingPM.Id; }
    get EntityId() { return this.DocumentFilingPM.EntityId; }
    get ObjectTableId() { return this.DocumentFilingPM.ObjectTableId; }
    get CreatedByUserId() { return this.DocumentFilingPM.CreatedByUserId; }
    get CreateDate() { return this.DocumentFilingPM.CreateDate; }
    get OwnerId() { return this.DocumentFilingPM.OwnerId; }
    get UpdatedByUserId() { return this.DocumentFilingPM.UpdatedByUserId; }
    get UpdateDate() { return this.DocumentFilingPM.UpdateDate; }
    get FileSize() { return this.DocumentFilingPM.FileSize; }

    ViewAttachment() {
        var documentSecurity = this.DocumentFilingPM.SecurityId;
        var link = "/WebPages/CorrespondenceDownloadpage.aspx?id=" + documentSecurity + "~" + this.Tenant;
        window.open(ServiceHelper.GetLogitudeURL() + link);
    }
}
