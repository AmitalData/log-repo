/// <reference path="../../../datacontracts/automationargs.ts" />
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren}  from '@angular/core';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool, DateTool, FileLoader} from '../../../../Infrastructure/Tools';
declare var System: any;
declare var window: any;
import {AutomationItemViewModel} from './ViewModel/AutomationItemViewModel';
import {AutomationExtendedPMService} from '../../../../Common/Services/ExtendedPMs/AutomationExtendedPMService';
import {AutomationPMService} from '../../../../Common/Services/StandardPMs/AutomationPMService';
import {AutomationPM} from '../../../../Common/EntityPMs/AutomationPMExtended';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {AutomatedBackup, AutomationSetSLAValue} from '../../../../Infrastructure/DataContracts/AutomatedBackup';
import {AutomationSetValue} from '../../../../Infrastructure/DataContracts/AutomationSetValue';
import {EventTypeArgs} from '../../../../Infrastructure/DataContracts/EventTypeArgs';
import {AutomationFollowUp} from '../../../../Infrastructure/DataContracts/AutomationFollowUp';
import {EventTypeList} from '../../../../Infrastructure/EntityLists/EventTypeList';
import {EventTypeListService} from '../../../../Infrastructure/Services/StandardLists/EventTypeListService';
import {SLAHeaderListService} from '../../../../CRM/Services/StandardLists/SLAHeaderListService';
import {SLAHeaderList} from '../../../../CRM/EntityLists/SLAHeaderList';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AutomationCondition} from '../../../../Infrastructure/DataContracts/AutomationCondition';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {DocumentTypeListService} from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import {DocumentTypeTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {AutomationHistoryExtendedPMService} from '../../../../Common/Services/ExtendedPMs/AutomationHistoryExtendedPMService';
import {AutomationResultEmailRecipientExtendedService} from '../../../../Common/Services/ExtendedPMs/AutomationResultEmailRecipientExtendedService';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {AutomationHistoryPM} from '../../../../Common/EntityPMs/AutomationHistoryPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AutomationResultEmailRecipientPM} from '../../../../Common/EntityPMs/AutomationResultEmailRecipientPM';
import {AutomationsSettingsComponent} from '../../../../Infrastructure/Components/Maintenance/Automation/AutomationsSettingsComponent';
import {DocumentTypeList} from '../../../../Common/EntityLists/DocumentTypeList';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {DocumentTypeTemplateViewModel} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {AutomationConditionViewModel} from './ViewModel/AutomationConditionViewModel';
import {ResultEmailRecipientViewModel} from './ViewModel/ResultEmailRecipientViewModel';
import {AutomationSetValueViewModel} from './ViewModel/AutomationSetValueViewModel';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {TraceEventExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {AutomationQueuedTask} from '../../../../Infrastructure/DataContracts/AutomationQueuedTask';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AutomationArgs} from '../../../../Infrastructure/DataContracts/AutomationArgs';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
@Component({
    moduleId: module.id,
    selector: 'AddEditAutomationsComponent',
    templateUrl: './AddEditAutomationsComponent.html',
    providers: [DocumentTypeTemplatePMExtendedService, AutomationResultEmailRecipientExtendedService, AutomationExtendedPMService, AutomationHistoryExtendedPMService, EntityArgs, TraceEventExtendedPMService],
})

export class AddEditAutomationsComponent extends BaseComponent implements OnInit {
    _documentTypeListService: DocumentTypeListService;
    AutomationSetSLAValue: AutomationSetSLAValue = new AutomationSetSLAValue();
    AutomationFollowUp: AutomationFollowUp = new AutomationFollowUp();
    AutomationQueuedTask: AutomationQueuedTask = new AutomationQueuedTask();
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    ObjectFieldsLists: ObjectFieldPM[] = [];
    AllowedinAutomationConditionsFieldLists: ObjectFieldPM[] = [];
    AutomationEmailRecipientFieldLists: ObjectFieldPM[] = [];
    AutomationSetValuebjectFieldLists: ObjectFieldPM[] = [];
    AutomationHistoryLists: AutomationHistoryPM[];
    IsLoadPage: boolean = false;
    IsChangeAutomation: boolean = false;

    EmailRecipientFieldLists: ResultEmailRecipientViewModel[] = [];
    FollowDateUniteCode: string = "Days";
    AutomationResultEmailRecipientPMList: AutomationResultEmailRecipientPM[] = [];

    public DataContext: AddEditAutomationsComponent = this;
    SelectedAutomationHistory: AutomationHistoryPM;

    AutomatedBackupClass: AutomatedBackup;
    public ValidationErrorsList: string[];
    ResultCodeList: ResultCode[];
    DocumentTypeLists: DocumentTypeList[];
    DocumentTypeSelected: DocumentTypeList;
    AllDocumentTypeLists: DocumentTypeList[] = [];
    DocumentTypeTemplateLists: DocumentTypeTemplateViewModel[];
    DocumentTypetTemplateSelected: DocumentTypeTemplateViewModel;

    DelayTime: number;
    IsEnableAddTemplate: boolean = false;
    IsEnableEditTemplate: boolean = false;    
    DelaytimeIndicator: string;
    CountDocumentSelection: string;

    AutomationCondationOrList: AutomationConditionViewModel[] = [];
    AutomationCondationAndList: AutomationConditionViewModel[] = [];
    AutomationSetValueLists: AutomationSetValueViewModel[] = [];
    SelectedAutomationSetValueLists: AutomationSetValueViewModel;

    SelectedAutomationCondationAndList: AutomationConditionViewModel;
    SelectedAutomationCondationOrList: AutomationConditionViewModel;

    PageType: string;
    CurrentEntityPM: AutomationPM;
    DataViewModel: AutomationsSettingsComponent;
    Mode: string;
    ObjectTableId: string;
    ObjectTableName: string;
    IsActiveAutomation: boolean;

    Description: string;
    Name: string;
    Inactive: boolean;

    InactiveKey: string;
    private PageChild_EVE: any = null;
    DelayedHtmlinputId: string;
    ImmediatlyHtmlinputId: string;
    IsNewEntity: boolean = false;
    IsSelectedImmediatly: boolean = false;
    IsSelectedDelayed: boolean = false;
    IsLoadingComplete: boolean = false;
    
    EventDocFollowUpTypeLists: EventTypeList[] = [];
    EventFollowUpTypeLists: EventTypeList[] = [];
    FollowUpTypeSelected: EventTypeList;

    FollowUpOwnerObjectFieldLists: ObjectFieldPM[] = [];
    FollowUpDateObjectFieldLists: ObjectFieldPM[] = [];

    FollowUpOwnerObjectFieldSelected: ObjectFieldPM;
    FollowUpDateObjectFieldSelected: ObjectFieldPM;

    FollowUpOwnerId: string = "";
    FollowOwnerObjectFieldId: string = "";

    DateValue: string = "";
  
    public FollowDateEscalationTime: number;
    public FollowDateEscalationActionTimeIndicatorCode: string = "";

    FollowUpNote: string = "";
    EventTypeCode: string;
   
    constructor(public _automationResultEmailRecipientExtendedService: AutomationResultEmailRecipientExtendedService,   public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService, public _automationExtendedPMService: AutomationExtendedPMService, public _automationHistoryExtendedPMService: AutomationHistoryExtendedPMService, private cd: ChangeDetectorRef, public entityArgs: EntityArgs, public _traceEventExtendedPMService: TraceEventExtendedPMService) {
        super();

        this._documentTypeListService = new DocumentTypeListService();
    }
    
    ngOnInit() {
        this.DataContext.UIProperties.SetEnabled("FollowDateUniteCode", "Automation", false);
    }

    SLAHeaderSelected: SLAHeaderList;
    SLAHeaderLists: SLAHeaderList[];
    IsLoadEventFollowUp: boolean = false;
    IsLoadSLAHeaders: boolean = false;
    IsAutomationResultEmailAllActiveUsers: boolean = false;

    IsMasterShipment: boolean = false;
    SetWindowArgs(args: any) {
        this.ObjectTableId = args.ObjectTableId;
        this.DataViewModel = args.DataViewModel;
        this.PageType = args.PageType;
        this.CurrentEntityPM = args.AutomationPM;
        this.Mode = args.Mode;
        this.ObjectTableName = args.ObjectTableName;
        this.IsNewEntity = args.IsNewEntity;
        this.DelayedHtmlinputId = Guid.newGuid();
        this.ImmediatlyHtmlinputId = Guid.newGuid();
        this.InactiveKey = Guid.newGuid();
        this.IsMasterShipment = args.IsMasterShipment;
        this.entityArgs.EntityPM = this.CurrentEntityPM;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        this.SLAHeaderLists = [];

        this.BuildQueuedTaskFilters();

        var myService = new EventTypeListService();
        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var lists: EventTypeList[] = myResponse.Result;
                // this.EventFollowUpTypeLists = lists.filter(f => f.ObjectTableId == this.ObjectTableId && f.AllowedInAutomation == true);
                this.EventFollowUpTypeLists = lists.filter(f => f.ObjectTableId == this.ObjectTableId && f.ManualActivatedFollowUp == true);
                this.EventDocFollowUpTypeLists = lists.filter(f => f.ObjectTableId == this.ObjectTableId && (f.Code == "DOCO" || f.Code == "DOCI" ));
       
                if (!AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                    this.FollowUpTypeSelected = this.EventFollowUpTypeLists.filter(d => d.Id == this.AutomationFollowUp.EventTypeId)[0];
                }            
            }

            this.IsLoadEventFollowUp = true;
        });
        
        if (this.ObjectTableName == "Ticket") {
            var sLAHeaderListService = new SLAHeaderListService();
            sLAHeaderListService.getAll().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        this.SLAHeaderLists = myResponse.Result.filter(d => d.Inactive == false);
                        if (!AppTool.IsNullOrEmpty(this.AutomationSetSLAValue.SLAId)) {
                            this.SLAHeaderSelected = this.SLAHeaderLists.filter(d => d.Id == this.AutomationSetSLAValue.SLAId)[0];
                        }
                    }
                }

                this.IsLoadSLAHeaders = true;
            });
        }

        if (this.CurrentEntityPM.AutomatedDataBackup) {

            this.AutomatedBackupClass = this.CurrentEntityPM.AutomatedDataBackup;

            this.Start();
        }
        else {

            if (this.IsNewEntity) {
                this.AutomatedBackupClass = new AutomatedBackup();
                this.AutomatedBackupClass.Type = "Immeduiatly";
                this.AutomatedBackupClass.DelaytimeIndicator = "OO";
             
                this.AutomatedBackupClass.Delaytime = 0;
                this.AutomatedBackupClass.ResultCode = "EMAIL";
                this.Start();
            }
            else {
                this.LoadAutomationDataBackup();
            }
        }
    }

    LoadAutomationDataBackup() {
        if (this.CurrentEntityPM && this.CurrentEntityPM.Id) {
            this._automationExtendedPMService.getAutomationBackupDataById(this.CurrentEntityPM.Id, this.CurrentEntityPM.Tenant).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    this.CurrentEntityPM.AutomatedDataBackup = myResult;
                    this.AutomatedBackupClass = myResult;
                    this.Start();
                }
            });
        }
    }
   
    LoadDocumentType() {
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.DocumentTypeLists = [];
        this.AllDocumentTypeLists = [];

        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator.Tenant;
        apiQueryFilters.ObjectTableName = this.ObjectTableName;

        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.AllDocumentTypeLists = pmResponse.Result.filter(a => a.ObjectTableId == this.ObjectTableId && !a.InActive);
                if (this.ObjectTableName == "Shipment") {
                    if (this.IsMasterShipment) {
                        this.AllDocumentTypeLists = this.AllDocumentTypeLists.filter(d => d.IsMaster || d.IsDirect);
                    }
                    else {
                        this.AllDocumentTypeLists = this.AllDocumentTypeLists.filter(d => d.IsHouse || d.IsDirect);
                    }
                }

                this.DocumentTypeLists = this.AllDocumentTypeLists.filter(a => a.IsDocOut && a.TemplateFormatCode == "M");

                if (this.DocumentTypeLists && this.DocumentTypeLists.length > 0) {
                    if (this.CurrentEntityPM.DocumentTypeId) {
                        this.DocumentTypeSelected = this.DocumentTypeLists.filter(d => d.Id == this.CurrentEntityPM.DocumentTypeId)[0];
                    }

                    if (!this.DocumentTypeSelected) {
                        this.DocumentTypeSelected = this.DocumentTypeLists[0];
                        if (this.DocumentTypeSelected) {
                            this.CurrentEntityPM.DocumentTypeId = this.DocumentTypeSelected.Id;
                        }
                    }

                    if (this.DocumentTypeSelected) {
                        this.LoadDocumentTypeTemplate(this.DocumentTypeSelected);
                    }

                    else SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                    this.IsEnableAddTemplate = true;
                }
                else {
                    this.IsEnableAddTemplate = false;
                    this.IsEnableEditTemplate = false;
                    SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                }
            }

            else {
                this.IsEnableAddTemplate = false;
                this.IsEnableEditTemplate = false;
                SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    }

    LoadDocumentTypeTemplate(documentTypeList: DocumentTypeList) {
        this.DocumentTypeTemplateLists = [];

        this._documentTypeTemplatePMExtendedService.getDocumentTypeTemplatesByDocumentTypeIdForAutomations(documentTypeList.Id, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;

                myResult.forEach((item) => {
                    this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                });

                if (this.DocumentTypeTemplateLists && this.DocumentTypeTemplateLists.length > 0) {
                    if (this.CurrentEntityPM.TemplateId) {
                        this.DocumentTypetTemplateSelected = this.DocumentTypeTemplateLists.filter(d => d.Id == this.CurrentEntityPM.TemplateId)[0];
                    }

                    if (!this.DocumentTypetTemplateSelected) {
                        this.DocumentTypetTemplateSelected = this.DocumentTypeTemplateLists.filter(d => d.Id == documentTypeList.DocumentTypeDefaultHTMLTemplateId)[0];

                        if (!this.DocumentTypetTemplateSelected) {
                            this.DocumentTypetTemplateSelected = this.DocumentTypeTemplateLists[0];
                        }

                        if (this.DocumentTypetTemplateSelected) {
                            this.CurrentEntityPM.TemplateId = this.DocumentTypetTemplateSelected.Id;
                        }
                    }

                    this.IsEnableEditTemplate = true;
                }
                else {
                    this.IsEnableEditTemplate = false;
                    this.DocumentTypetTemplateSelected = null;
                }
            }
        });
    }

    DocumentTypeListsValueChanged(item: DocumentTypeList) {
        if (item != null) {
            this.DocumentTypeSelected = item;
            this.CurrentEntityPM.DocumentTypeId = item.Id;
            this.LoadDocumentTypeTemplate(item);
            this.IsChangeAutomation = true;
        }
    }

    DocumentTypeTemplateListValueChanged(item: DocumentTypeTemplateViewModel) {
        if (item != null) {
            this.DocumentTypetTemplateSelected = item;
            this.CurrentEntityPM.TemplateId = item.Id;
            this.IsChangeAutomation = true;
        }       
    }
    
    EditDocumentTemplate(item: DocumentTypeTemplateViewModel) {
        if (item) {
            if (this.DocumentTypetTemplateSelected) {
                if (item.Id != this.DocumentTypetTemplateSelected.Id) {
                    this.DocumentTypetTemplateSelected = item;
                    this.CurrentEntityPM.TemplateId = item.Id;
                }
            }
            else {
                this.DocumentTypetTemplateSelected = item;
                this.CurrentEntityPM.TemplateId = item.Id;
            }
        }

        if (this.DocumentTypetTemplateSelected) {

            if (this.DocumentTypetTemplateSelected && this.DocumentTypetTemplateSelected.EditorTool == "R") {
                var windowArgs: any = {};
                windowArgs.DataViewModel = this;
                windowArgs.PageType = "Maintenance";
                windowArgs.TemplateId = this.DocumentTypetTemplateSelected.Id;
                windowArgs.Tenant = this.DocumentTypetTemplateSelected.Tenant;
                windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
                windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplateLists;
                windowArgs.ObjectTableId = this.ObjectTableId;
                windowArgs.EntityId = this.CurrentEntityPM.Id;
                windowArgs.ChildObjectTableId = "";

                var widthwindow = window.innerWidth;
                var heighthwindow = window.innerHeight;

                var logWindow = new LogitudeWindow();
                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Html Template";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
            }
        }
    }
    
    AddDocumentTypeTemplate() {
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Maintenance";
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.CurrentEntityPM = this.DocumentTypeSelected;
        windowArgs.DocumentTypeTemplateLists = this.DocumentTypeTemplateLists;
        windowArgs.TypeTab = "RichText";

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = "New Html Template";

        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/NewReportTemplateComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if ($event && this.DocumentTypeTemplateLists && this.DocumentTypeTemplateLists.length > 0) {
                this.IsEnableEditTemplate = true;
            }
        });
    }

    VeiwAutomationHositoryButtonClicked(item: AutomationHistoryPM) {
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.AutomationHistoryPM = item;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 560;
        logWindow.IsShowCloseButton = true;
        logWindow.Title = "View Automation";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/ViewAutomationHistoryComponent");

    }
    
    UpdateCurrentAutomationHository(item: AutomationHistoryPM) {
        this.AutomationHistoryLists.filter(d => d.AutomationsId == item.AutomationsId && d.Version == item.Version)[0] = item;
    }
    
    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    SelectionChanged() {
        if (this.SelectedTabCode != null) {

            if (this.SelectedTabCode == "EVE") {
                let locs = this.AllLocations.toArray().filter(f => f.Code == 'EVE');
                let myLocation: LocationDirective = locs.filter(f => f.Code == "EVE")[0];
                if (myLocation != null) {

                    if (this.PageChild_EVE == null) {
                        SessionLocator.DynamicLoader.Load('./Common/Components/Events/EventsTabComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_EVE = cmpRef.instance;

                            });
                    }

                    else {
                        this.PageChild_EVE.LoadData();
                    }
                }
            }
        }
    }

    LoadAutomationHistory() {
        if (this.CurrentEntityPM && !AppTool.IsNullOrEmpty(this.CurrentEntityPM.Id)) {

            this._automationHistoryExtendedPMService.getAutomationHistoryesByAutomationId(this.CurrentEntityPM.Id, SessionLocator.Tenant).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;

                    myResult.forEach((item) => {
                        this.AutomationHistoryLists.push(item);
                    });
                }
            });
        }
    }
    
    private resultCodeSelected: ResultCode;
    get ResultCodeSelected() { return this.resultCodeSelected; }
    set ResultCodeSelected(newValue: ResultCode) {
        if (this.resultCodeSelected != newValue) {
            this.resultCodeSelected = newValue;

            this.SetQueuedTaskRequired();
        }
    }

    IsFirstTime: boolean = false;
    Start() {
        if (this.AutomatedBackupClass) {
            this.SelectedTabCode = "DET";
            this.DelaytimeIndicator = this.AutomatedBackupClass.DelaytimeIndicator;
            this.ResultCodeList = [];
            this.AutomationSetValuebjectFieldLists = [];
            this.AutomationEmailRecipientFieldLists = [];
            this.AllowedinAutomationConditionsFieldLists = [];
            this.AutomationCondationAndList = [];
            this.AutomationCondationOrList = [];
            this.EntityContactVariable = [];
            this.ParticipantsList = [];
            this.AutomationHistoryLists = [];
            
            this.ResultCodeList.push(new ResultCode("E-mail", "EMAIL"));

            if (this.ObjectTableName == "Ticket") {
                this.ResultCodeList.push(new ResultCode("Set Fields Value", "FIELDSET"));
                this.ResultCodeList.push(new ResultCode("Set SLA", "SETSLA"));
            }

            //Masters and Houses
            if (this.ObjectTableName == "Shipment") {
                this.ResultCodeList.push(new ResultCode("F/U Creation", "FOLLOWUP"));
                this.ResultCodeList.push(new ResultCode("Docs Out F/U Creation", "DOCOUTFOLLOWUP"));
                this.ResultCodeList.push(new ResultCode("Docs In F/U Creation", "DOCINFOLLOWUP"));
                this.ResultCodeList.push(new ResultCode("Queued Task", "QUEUE"));
            }

            this.ResultCodeSelected = this.ResultCodeList.filter(d => d.Code == this.AutomatedBackupClass.ResultCode)[0];

            if (!this.ResultCodeSelected) {

                this.ResultCodeSelected = this.ResultCodeList.filter(d => d.Code == "EMAIL")[0];
            }

            this.AutomationFollowUp.ObjectTableName = this.ObjectTableName;
            this.AutomationFollowUp.OwnerFieldType = "Field";
            
            if ((this.ResultCodeSelected.Code == "FOLLOWUP" || this.ResultCodeSelected.Code == "DOCOUTFOLLOWUP" || this.ResultCodeSelected.Code == "DOCINFOLLOWUP") && this.AutomatedBackupClass.AutomationFollowUp) {
                this.AutomationFollowUp.EventTypeId = this.AutomatedBackupClass.AutomationFollowUp.EventTypeId;
                this.AutomationFollowUp.OwnerFieldType = this.AutomatedBackupClass.AutomationFollowUp.OwnerFieldType;
                this.AutomationFollowUp.OwnerValue = this.AutomatedBackupClass.AutomationFollowUp.OwnerValue;
                this.AutomationFollowUp.DateValue = this.AutomatedBackupClass.AutomationFollowUp.DateValue;
                this.AutomationFollowUp.NoteValue = this.AutomatedBackupClass.AutomationFollowUp.NoteValue;
                this.AutomationFollowUp.LegType = this.AutomatedBackupClass.AutomationFollowUp.LegType;
                this.AutomationFollowUp.ObjectTableName = this.AutomatedBackupClass.AutomationFollowUp.ObjectTableName;
                this.AutomationFollowUp.DocumentTypeLists = this.AutomatedBackupClass.AutomationFollowUp.DocumentTypeLists;
                this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode = this.FollowDateEscalationActionTimeIndicatorCode = this.AutomatedBackupClass.AutomationFollowUp.DateEscalationActionTimeIndicatorCode;

                this.AutomationFollowUp.DateEscalationTime = this.FollowDateEscalationTime = this.AutomatedBackupClass.AutomationFollowUp.DateEscalationTime;

                if (this.AutomationFollowUp.DocumentTypeLists && this.AutomationFollowUp.DocumentTypeLists.length > 0) {
                    this.CountDocumentSelection = this.AutomationFollowUp.DocumentTypeLists.length + " documents selected";
                }
                else {
                    this.CountDocumentSelection = "no documents selected";
                }

                if (this.EventFollowUpTypeLists && this.IsLoadEventFollowUp && this.ResultCodeSelected.Code == "FOLLOWUP") {
                    if (!AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                        this.FollowUpTypeSelected = this.EventFollowUpTypeLists.filter(d => d.Id == this.AutomationFollowUp.EventTypeId)[0];
                    }
                }

                this.FollowOwnerObjectFieldId = this.AutomationFollowUp.OwnerFieldType == "Field" ? this.AutomationFollowUp.OwnerValue : "";
                this.FollowUpOwnerId = this.AutomationFollowUp.OwnerFieldType == "Specific" ? this.AutomationFollowUp.OwnerValue : "";
                this.DateValue = this.AutomationFollowUp.DateValue;
                this.FollowUpNote = this.AutomationFollowUp.NoteValue;
            }

            if (this.ResultCodeSelected.Code == "QUEUE" && this.AutomatedBackupClass.AutomationQueuedTask) {
                this.AutomationQueuedTask.QueueId = this.AutomatedBackupClass.AutomationQueuedTask.QueueId;
                this.AutomationQueuedTask.TeamId = this.AutomatedBackupClass.AutomationQueuedTask.TeamId;
                this.AutomationQueuedTask.TaskOffset = this.AutomatedBackupClass.AutomationQueuedTask.TaskOffset;
                this.AutomationQueuedTask.TaskOwnerId = this.AutomatedBackupClass.AutomationQueuedTask.TaskOwnerId;
                this.AutomationQueuedTask.TaskCustomerId = this.AutomatedBackupClass.AutomationQueuedTask.TaskCustomerId;
                this.AutomationQueuedTask.TaskSubject = this.AutomatedBackupClass.AutomationQueuedTask.TaskSubject;
                this.AutomationQueuedTask.TaskPriorityId = this.AutomatedBackupClass.AutomationQueuedTask.TaskPriorityId;
                this.AutomationQueuedTask.TaskDescription = this.AutomatedBackupClass.AutomationQueuedTask.TaskDescription;
                this.AutomationQueuedTask.DateFieldValue = this.AutomatedBackupClass.AutomationQueuedTask.DateFieldValue;
                this.AutomationQueuedTask.OffsetTypeValue = this.AutomatedBackupClass.AutomationQueuedTask.OffsetTypeValue;
                this.AutomationQueuedTask.TaskTimeUnitValue = this.AutomatedBackupClass.AutomationQueuedTask.TaskTimeUnitValue;

                this.QueueId = this.AutomationQueuedTask.QueueId;
                this.TeamId = this.AutomationQueuedTask.TeamId;                
                this.TaskOwnerId = this.AutomationQueuedTask.TaskOwnerId;
                this.TaskSubject = this.AutomationQueuedTask.TaskSubject;                
                this.TaskDescription = this.AutomationQueuedTask.TaskDescription;
                this.SelectedDueDateField = this.DueDateFieldList.filter(f => f.Code == this.AutomationQueuedTask.DateFieldValue)[0];
                this.SelectedTaskOffsetType = this.TaskOffsetTypeList.filter(f => f.Code == this.AutomationQueuedTask.OffsetTypeValue)[0];
                this.TaskOffset = this.AutomationQueuedTask.TaskOffset;
                this.SelectedTaskTimeUnit = this.TaskTimeUnitList.filter(f => f.Code == this.AutomationQueuedTask.TaskTimeUnitValue)[0];
                this.TaskCustomerId = this.AutomationQueuedTask.TaskCustomerId;
                this.TaskPriorityId = this.AutomationQueuedTask.TaskPriorityId;
            }

            if (this.ResultCodeSelected.Code == "SETSLA") {
                this.AutomationSetSLAValue.SLAId = this.AutomatedBackupClass.AutomationSetSLAValue ? this.AutomatedBackupClass.AutomationSetSLAValue.SLAId : "" ;
                if (this.AutomationSetSLAValue) {
                    if (this.SLAHeaderLists && this.IsLoadSLAHeaders ) {
                        if (!AppTool.IsNullOrEmpty(this.AutomationSetSLAValue.SLAId)) {
                            this.SLAHeaderSelected = this.SLAHeaderLists.filter(d => d.Id == this.AutomationSetSLAValue.SLAId)[0];
                        }
                    }
                }
            }

            if (AppTool.IsNullOrEmpty(this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode)) {
                this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode = this.FollowDateEscalationActionTimeIndicatorCode = "AF";
            }

            if (!this.AutomationFollowUp.DateEscalationTime) {
                this.AutomationFollowUp.DateEscalationTime = this.FollowDateEscalationTime = 0;
            }       

            this.IsSelectedImmediatly = this.AutomatedBackupClass.Type == "Immeduiatly" ? true : false;
            this.IsSelectedDelayed = this.AutomatedBackupClass.Type == "Delayed" ? true : false;

            if (this.IsSelectedDelayed) {
                this.IsFirstTime = true;
            }

            this.Description = this.CurrentEntityPM.Description;
            this.Name = this.CurrentEntityPM.Name;
            this.Inactive = this.CurrentEntityPM.Inactive;
            this.IsActiveAutomation = this.CurrentEntityPM.Inactive;
            this.DelayTime = this.AutomatedBackupClass.Delaytime;
            this.IsAutomationResultEmailAllActiveUsers = this.AutomatedBackupClass.IsAutomationResultEmailAllActiveUsers;

            this.FillObjectField();
            this.LoadAutomationHistory();
            this.LoadDocumentType();
        }

        this.IsLoadingComplete = true;
    }
    
    DelaytimeIndicatorChange(value) {

        if (value != null && value.Code != this.DelaytimeIndicator) {
            var delayTime = this.DelayTime;
            var delaytimeIndicator = this.DelaytimeIndicator;

            var numOfMinutes = 1 * 60;
            if (this.DelaytimeIndicator != "OO" && value.Code == "OO" && this.DelayTime >= 60) {
                delayTime = this.DelayTime / numOfMinutes;
          
            }
            else if (this.DelaytimeIndicator != "II" && value.Code == "II") {
                delayTime = this.DelayTime * numOfMinutes;
            }


            this.DelaytimeIndicator = value.Code;
            this.DelayTime = delayTime;
            this.IsChangeAutomation = true;

        }
    }
    
    SelectDocumentTypes() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        var windowArgs: any = {};
        windowArgs.AddEditAutomationsComponent = this;
        logWindow.WindowArgs = windowArgs;

        logWindow.Title = "Document Types"
        logWindow.DataContext = this;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/SelectDocumentTypesComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
                if (this.AutomationFollowUp.DocumentTypeLists && this.AutomationFollowUp.DocumentTypeLists.length > 0) {
                    this.CountDocumentSelection = this.AutomationFollowUp.DocumentTypeLists.length + " documents selected";
                } else {
                    this.CountDocumentSelection = "no documents selected";
                }
            }
        });
    }

    FillObjectField() {
        this.AllowedinAutomationConditionsFieldLists = [];
        this.AutomationEmailRecipientFieldLists = [];
        this.AutomationSetValuebjectFieldLists = [];
        this.FollowUpOwnerObjectFieldLists = [];
        this.ObjectFieldsLists = [];

        this.ObjectFieldsLists = window.ObjectFields.filter(f => f.ObjectTableId == this.ObjectTableId);
        this.ObjectFieldsLists.forEach((objectField) => {

            if (objectField.AllowedinAutomationConditions || objectField.IsCustom) {
                this.AllowedinAutomationConditionsFieldLists.push(objectField);
            }

            if (objectField.AutomationEmailRecipient && (objectField.ObjectTable_LookUpTableName == "User" || objectField.ObjectTable_LookUpTableName == "Contact" || objectField.DataTypeCode == "Emails")) {
                this.AutomationEmailRecipientFieldLists.push(objectField);
            }

            if (objectField.CanAutomateSetValue && !objectField.IsCustom) this.AutomationSetValuebjectFieldLists.push(objectField);

            if (objectField.FieldName == "CreatedByUserId" || objectField.FieldName == "SalesmanUserId" || objectField.FieldName == "UpdatedByUserId" || objectField.FieldName == "AccountManagerUserId") {
                this.FollowUpOwnerObjectFieldLists.push(objectField);
            }

            if (objectField.FieldName == "MainCarriageETD" || objectField.FieldName == "MainCarriageATD" || objectField.FieldName == "MainCarriageFinalDestinationETA" || objectField.FieldName == "MainCarriageFinalDestinationATA") {
                this.FollowUpDateObjectFieldLists.push(objectField);
            }

            if (this.ObjectTableName == "Ticket") {
                if (objectField.FieldName == "SLAId") {
                    this.AutomationSetSLAValue.ObjectFieldId = objectField.Id;
                }
            }
        });

        var specifiOwnerObjectField: ObjectFieldPM = new ObjectFieldPM();
        specifiOwnerObjectField.FullNameTextCodeDefaultText = "Specific";
        specifiOwnerObjectField.Id = "Specific";
        specifiOwnerObjectField.FieldName = "Specific";
        this.FollowUpOwnerObjectFieldLists.push(specifiOwnerObjectField);

        if (this.FollowUpOwnerObjectFieldLists) {

            if (this.AutomationFollowUp.OwnerFieldType == "Specific") {
                this.FollowUpOwnerObjectFieldSelected = this.FollowUpOwnerObjectFieldLists.filter(d => d.Id == "Specific")[0];
            }

            else {
                this.FollowUpOwnerObjectFieldSelected = this.FollowUpOwnerObjectFieldLists.filter(d => d.Id == this.FollowOwnerObjectFieldId)[0];
            }
        }

        this.FollowUpDateObjectFieldSelected = this.FollowUpDateObjectFieldLists.filter(d => d.Id == this.DateValue)[0];
        
        if (this.AutomatedBackupClass) {
            this.BuildAutomationCondition();
            this.BuildAutomationSetValue();
        }

        this.GenerateControlEntityContactVariable();

        if (!AppTool.IsNullOrEmpty(this.CurrentEntityPM.Id)) {
            this.LoadAutomationResultEmailRecipient();
        }
        else {
            this.IsLoadAutomationResultEmailRecipient = true;
        }
    }

    IsViewCondition: boolean;
    BuildAutomationCondition() {
        this.IsViewCondition = false;
        var automationConditionPMList = this.AutomatedBackupClass.AautomationConditionLists;

        if (automationConditionPMList != null) {
            automationConditionPMList.forEach((item) => {
                if (item.ConditionType == "And") {
                    this.AutomationCondationAndList.push(new AutomationConditionViewModel(item, this));
                }
                else if (item.ConditionType == "Or") {
                    this.AutomationCondationOrList.push(new AutomationConditionViewModel(item, this));
                }         
            });
        }
    }
    
    BuildAutomationSetValue() {
        this.AutomationSetValueLists = [];
        var automationSetValueLists = this.AutomatedBackupClass.AutomationSetValueLists;
        if (automationSetValueLists) {
            automationSetValueLists.forEach((item) => {
                this.AutomationSetValueLists.push(new AutomationSetValueViewModel(item, this));
            });
        }
    }
    
    EntityContactVariable: string[];
    UserIds: string = "";
    OldUserIds: string = "";

    IsLoadAutomationResultEmailRecipient: boolean = false;
    LoadAutomationResultEmailRecipient() {
        this.EntityContactVariable = [];
        this.UserIds = "";
        
        this._automationResultEmailRecipientExtendedService.getAutomationResultEmailRecipientByAutomationId(this.CurrentEntityPM.Id, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach((item) => {
                    if (item.RecipientType == "Fixed") {

                        if (!AppTool.IsNullOrEmpty(item.RecipientValue)) {
                            this.UserIds += (item.RecipientValue + ";");
                            this.OldUserIds += (item.RecipientValue + ";");
                        }                      
                    }
                    else {
                        this.EntityContactVariable.push(item.RecipientValue);
                    }

                    this.AutomationResultEmailRecipientPMList.push(item);
                });
            }

            this.IsLoadAutomationResultEmailRecipient = true;
            this.GenerateControlEntityContactVariable();
        });
    }
    
    GenerateControlEntityContactVariable() {
        this.EmailRecipientFieldLists = [];
        this.AutomationEmailRecipientFieldLists.forEach((item) => {
            this.EmailRecipientFieldLists.push(new ResultEmailRecipientViewModel(item, this));
          });
    }

    CloseButtonClicked() {
        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
        SessionLocator.CurrentSession.CurrentWindow.Close("Cancel");
    }
    
    AddAutomationConditionMethod(conditionType: string) {
        var automationConditionPM: AutomationCondition = new AutomationCondition();

        automationConditionPM.ConditionType = conditionType;
        automationConditionPM.Tenant = SessionLocator.TenantPM.Id;
        automationConditionPM.CreatedByUserId = SessionLocator.LoggedUserId;
        automationConditionPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        automationConditionPM.Value = "";
        automationConditionPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.OperatorCode = "Equals";
        automationConditionPM.ObjectFieldId = "";
        automationConditionPM.AutomationsId = this.CurrentEntityPM.Id;
        
        if (conditionType == "And") {
            this.AutomationCondationAndList.push(new AutomationConditionViewModel(automationConditionPM, this));
        }

        else {
            this.AutomationCondationOrList.push(new AutomationConditionViewModel(automationConditionPM, this));
        }
    }

    ResultCodeListValueChanged(value) {
        this.ResultCodeSelected = value;
        this.CurrentEntityPM.ResultCode = value.Code;
        this.IsChangeAutomation = true;
       
        if (value.Code == "DOCOUTFOLLOWUP" || value.Code == "DOCINFOLLOWUP") {
            this.AutomationFollowUp.DocumentTypeLists = [];
            this.CountDocumentSelection = "no documents selected";
            this.FollowUpNote = "";
        }
    }
    
    get TypeWidth() {
        var result: number = 200;

        if (this.ResultCodeSelected != null) {
            if (this.ResultCodeSelected.Code == 'FOLLOWUP' || this.ResultCodeSelected.Code == 'DOCOUTFOLLOWUP' || this.ResultCodeSelected.Code == 'DOCINFOLLOWUP') {
                result = 270;
            }

            else if (this.ResultCodeSelected.Code == "QUEUE") {
                result = 236;
            }
        }

        return result;
    }
    
    SelectedDelayedImmediatlRadioClcik(type:string) {
        if ( type == "Immeduiatly") {
            this.AutomatedBackupClass.Type = "Immeduiatly";
            this.IsSelectedImmediatly = true;
            this.IsSelectedDelayed = false;
        }

        else {
            this.AutomatedBackupClass.Type = "Delayed";
            this.IsSelectedImmediatly = false;
            this.IsSelectedDelayed = true;
        }

        this.IsChangeAutomation = true;
    }

    ViewDelayAutomationconditionsButtonClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.IsShowCloseButton = true;
        logWindow.DataContext = this;
        logWindow.IsShowAutomationDelayTitle = true;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/DelayAutomationconditionsComponent");
    }
    
    AddAutomationSetValueButtonClick() {
        var automationSetValue: AutomationSetValue = new AutomationSetValue();
        automationSetValue.ObjectFieldId = "";
        automationSetValue.OperatorCode = "SV";
        automationSetValue.Value = "";
        automationSetValue.FieldName = "";
        this.AutomationSetValueLists.push(new AutomationSetValueViewModel(automationSetValue, this));
    }

    IsChangeCondition: boolean = false;
    IsChangeSetValue: boolean = false;
    ParticipantsList: any[] = []; 
    EventTypeCodeList: string[] = [];

    SaveButtonClicked() {
        this.ParticipantsList = [];
        this.ValidationErrorsList = [];

        if (this.OldUserIds != this.UserIds) {
            this.IsChangeAutomation = true;
        }

        if (!AppTool.IsNullOrEmpty(this.UserIds)) {
            this.UserIds.split(';').forEach((id) => {
                if (!AppTool.IsNullOrEmpty(id)) {
                    this.ParticipantsList.push(id);
                }
            });
        }

        if (this.Name != this.CurrentEntityPM.Name || this.Description != this.CurrentEntityPM.Description || this.Inactive != this.IsActiveAutomation) this.IsChangeAutomation = true;

        this.CurrentEntityPM.Name = this.Name;
        this.CurrentEntityPM.Description = this.Description;
        this.CurrentEntityPM.Inactive = this.Inactive;

        var isFollowUp: boolean = this.IsFollowUp();
  
        if (isFollowUp) {
            this.AutomationFollowUp.OwnerValue = this.AutomationFollowUp.OwnerFieldType == "Field" ? this.FollowOwnerObjectFieldId : this.FollowUpOwnerId;
            this.AutomationFollowUp.NoteValue = this.FollowUpNote;
            this.AutomationFollowUp.DateEscalationTime = this.FollowDateEscalationTime;
            this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode = this.FollowDateEscalationActionTimeIndicatorCode;
            this.AutomationFollowUp.DateValue = this.DateValue;

            if (this.CurrentEntityPM.ResultCode == "DOCOUTFOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCINFOLLOWUP") {
                var eventCode: string = this.CurrentEntityPM.ResultCode == "DOCOUTFOLLOWUP" ? "DOCO" : "DOCI";
                var eventType: EventTypeList = this.EventDocFollowUpTypeLists.filter(d => d.Code == eventCode)[0];
                if (eventType) this.AutomationFollowUp.EventTypeId = eventType.Id;

                if (!this.AutomationFollowUp.DocumentTypeLists) this.AutomationFollowUp.DocumentTypeLists = [];
                if (this.AutomationFollowUp.DocumentTypeLists.length == 0) this.ValidationErrorsList.push("Please select at least one document");
            }

            else if (this.CurrentEntityPM.ResultCode == "FOLLOWUP") {
                if (!AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                    var eventType: EventTypeList = this.EventDocFollowUpTypeLists.filter(d => d.Id == this.AutomationFollowUp.EventTypeId)[0];
                    if (eventType) this.AutomationFollowUp.EventTypeId = "";
                }
            }

            if (AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                this.ValidationErrorsList.push("F/U Type field is required");
            }

            if (AppTool.IsNullOrEmpty(this.AutomationFollowUp.OwnerValue)) {
                this.ValidationErrorsList.push("Owner field is required");
            }
            this.AutomationFollowUp.LegType = "";
            if (this.EventFollowUpTypeLists && !AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                var eventList: EventTypeList = this.EventFollowUpTypeLists.filter(d => d.Id == this.AutomationFollowUp.EventTypeId)[0]
                if (eventList != null) {
                    if (eventList.Code == "DEP") this.AutomationFollowUp.LegType = "MainCarriageDeparture";
                    else if (eventList.Code == "ARR") this.AutomationFollowUp.LegType = "MainCarriageArrival";
                }
            }
        }
        
        if (this.CurrentEntityPM.ResultCode == "QUEUE") {
            this.AutomationQueuedTask.QueueId = this.QueueId;
            this.AutomationQueuedTask.TeamId = this.TeamId;
            this.AutomationQueuedTask.TaskOwnerId = this.TaskOwnerId;
            this.AutomationQueuedTask.TaskSubject = this.TaskSubject;
            this.AutomationQueuedTask.TaskDescription = this.TaskDescription;
            this.AutomationQueuedTask.DateFieldValue = this.SelectedDueDateField == null ? null : this.SelectedDueDateField.Code;
            this.AutomationQueuedTask.OffsetTypeValue = this.SelectedTaskOffsetType == null ? null : this.SelectedTaskOffsetType.Code;
            this.AutomationQueuedTask.TaskOffset = this.TaskOffset;
            this.AutomationQueuedTask.TaskTimeUnitValue = this.SelectedTaskTimeUnit == null ? null : this.SelectedTaskTimeUnit.Code;
            this.AutomationQueuedTask.TaskCustomerId = this.TaskCustomerId;
            this.AutomationQueuedTask.TaskPriorityId = this.TaskPriorityId;
        }

        if (AppTool.IsNullOrEmpty(this.CurrentEntityPM.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }

        if (this.AutomationCondationAndList.length == 0 && this.AutomationCondationOrList.length == 0 && this.CurrentEntityPM.Type == "OnUpdate") {
            this.ValidationErrorsList.push("Please add at least one condation");
        }

        if (this.CurrentEntityPM.ResultCode == "FIELDSET" && (!this.AutomationSetValueLists || (this.AutomationSetValueLists && this.AutomationSetValueLists.length == 0))) {
            this.ValidationErrorsList.push("Please add at least one set Value");
        }

        if (this.ParticipantsList.length == 0 && this.EntityContactVariable.length == 0 && this.CurrentEntityPM.ResultCode == "EMAIL") {
            if (!this.IsAutomationResultEmailAllActiveUsers) {
                this.ValidationErrorsList.push("Please add at least one recipient");
            }
        }

        if (this.CurrentEntityPM.ResultCode == "FIELDSET" && this.AutomationSetValueLists && this.AutomationSetValueLists.length > 0) {
            this.AutomationSetValueLists.forEach((item) => {
                if (AppTool.IsNullOrEmpty(item.CurrentEntityPM.Value)) {
                    this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " field is required");
                }

                else if (item.CurrentEntityPM.Value.length > item.SelectedCustomField.MaxLength || item.CurrentEntityPM.Value.length < item.SelectedCustomField.MinLength) {
                    this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " must butween " + item.SelectedCustomField.MinLength + " and " + item.SelectedCustomField.MaxLength + " characters");
                }
            });
        }

        if (this.CurrentEntityPM.ResultCode == "SETSLA" && AppTool.IsNullOrEmpty(this.AutomationSetSLAValue.SLAId)) {
            this.ValidationErrorsList.push("Please select sla type");
        }

        if (this.AutomatedBackupClass.Delaytime != this.DelayTime) {
            this.IsChangeAutomation = true;
        }

        if (this.CurrentEntityPM.ResultCode == "QUEUE") {
            if (AppTool.IsNullOrEmpty(this.QueueId)) {
                this.ValidationErrorsList.push("Queue field is required");
            }

            if (AppTool.IsNullOrEmpty(this.TaskSubject)) {
                this.ValidationErrorsList.push("Subject field is required");
            }

            if (AppTool.IsNullOrEmpty(this.TaskOwnerId) && AppTool.IsNullOrEmpty(this.TeamId)) {
                this.ValidationErrorsList.push("Please fill Owner or Team");
            }
        }

        if (this.AutomatedBackupClass.Type == "Delayed") {
            if (!this.DelayTime ||  this.DelayTime<1) {
                this.ValidationErrorsList.push("Delay time must be greater than 0");
            }
        }



        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNewEntity) {
                SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                this.EventTypeCode = "AUCR";
                this.CurrentEntityPM.AutomatedDataBackup = null;
                this.CurrentEntityPM.IsChangeAutomationXaml = false;
                this._automationExtendedPMService.insert(this.CurrentEntityPM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        ServiceLocator.SendTotangoUserActivity("Automation", "New Automation");


                        var myResult = pmResponse.Result;
                        this.CurrentEntityPM = myResult;

                        if (this.IsActiveAutomation != this.CurrentEntityPM.Inactive) {

                            if (this.CurrentEntityPM.Inactive) this.EventTypeCodeList.push("AUSI");
                            else this.EventTypeCodeList.push("AURE");
                        }

                        this.SaveAutomationResultEmailRecipient();
                    }

                    else {
                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                });
            }

            else {
                if ((this.IsChangeCondition || this.IsChangeSetValue || this.IsChangeAutomation) || (isFollowUp && this.CheckIfAutomationFollowUpChange()) || this.CheckIfAutomationSetSLAValueChange() || this.CheckIfAutomationQueuedTaskChange()) {
                    SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

                    if (this.IsActiveAutomation != this.CurrentEntityPM.Inactive) {
                        if (this.CurrentEntityPM.Inactive) this.EventTypeCodeList.push("AUSI");
                        else this.EventTypeCodeList.push("AURE");
                    }


                    this.SaveAutomationResultEmailRecipient();
                }

                else this.CloseButtonClicked();
            }
        }
    }

    IsFollowUp() {

        if (this.CurrentEntityPM.ResultCode == "FOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCOUTFOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCINFOLLOWUP") return true;
        else return false;

    }

    SLAHeaderValueChanged(event) {
        var value: string = event ? event.Id : "";
        this.AutomationSetSLAValue.SLAId = value;
    }
    
    SaveAutomationResultEmailRecipient() {
        var resultEmailRecipientPMLists: AutomationResultEmailRecipientPM[] = [];

        if (this.CurrentEntityPM.ResultCode == "EMAIL") {
            this.ParticipantsList.forEach((userid) => {
                if (!this.AutomationResultEmailRecipientPMList.filter(d => d.RecipientValue == userid && (d.RecipientType == "Fixed"))[0]) {
                    var automationResultEmailRecipientPM: AutomationResultEmailRecipientPM = new AutomationResultEmailRecipientPM();
                    automationResultEmailRecipientPM.RecipientType = "Fixed";
                    automationResultEmailRecipientPM.RecipientValue = userid;
                    automationResultEmailRecipientPM.Tenant = SessionLocator.Tenant;
                    automationResultEmailRecipientPM.AutomationsId = this.CurrentEntityPM.Id;
                    resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
                }
            });

            this.EntityContactVariable.forEach((Id) => {
                if (!this.AutomationResultEmailRecipientPMList.filter(d => d.RecipientValue == Id && (d.RecipientType == "Variable" || d.RecipientType == "Emails"))[0]) {
                    var automationResultEmailRecipientPM: AutomationResultEmailRecipientPM = new AutomationResultEmailRecipientPM()
                    automationResultEmailRecipientPM.RecipientType = "Variable",
                        automationResultEmailRecipientPM.RecipientValue = Id,
                        automationResultEmailRecipientPM.Tenant = SessionLocator.Tenant;
                    automationResultEmailRecipientPM.AutomationsId = this.CurrentEntityPM.Id

                    var objectFieldPM: ObjectFieldPM = this.AutomationEmailRecipientFieldLists.filter(d => d.Id == Id)[0];
                    if (objectFieldPM != null) {
                        if (objectFieldPM.DataTypeCode == "Emails") {
                            automationResultEmailRecipientPM.RecipientType = "Emails";
                        }
                    }

                    if (!this.AutomationResultEmailRecipientPMList.filter(d => d.RecipientType == automationResultEmailRecipientPM.RecipientType && d.RecipientValue == automationResultEmailRecipientPM.RecipientValue)[0]) {
                        resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
                    }
                }
            });

            this.AutomationResultEmailRecipientPMList.forEach((automationResultEmailRecipientPM) => {

                if (automationResultEmailRecipientPM.RecipientType == "Fixed") {
                    if (!this.ParticipantsList.filter(d => d == automationResultEmailRecipientPM.RecipientValue)[0]) {
                        resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
                    }
                }

                else {
                    if (this.EntityContactVariable.indexOf(automationResultEmailRecipientPM.RecipientValue) == -1) {
                        resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
                    }
                }
            });
        }

        if (resultEmailRecipientPMLists.length > 0) {

            var automationArgsList: AutomationArgs[] = [];
            resultEmailRecipientPMLists.forEach((item) => {

                var automationArgs: AutomationArgs = new AutomationArgs();
                    automationArgs.RecipientType = item.RecipientType,
                    automationArgs.RecipientValue = item.RecipientValue,
                    automationArgs.Tenant = SessionLocator.Tenant;
                    automationArgs.AutomationsId = item.AutomationsId;
                    automationArgs.Id = item.Id;
                    automationArgsList.push(automationArgs);
            });

            this._automationResultEmailRecipientExtendedService.update(automationArgsList).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    this.SaveAutomation();
                }

                else {
                    SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                }
            });
        }

        else this.SaveAutomation();
    }

    SaveAutomation() {
        var automationConditionList: AutomationCondition[] = [];
        var automationSetValuelist: AutomationSetValue[] = [];

        this.AutomationCondationAndList.forEach((item) => {
            item.CurrentEntityPM.AutomationsId = this.CurrentEntityPM.Id;
            item.CurrentEntityPM.UpdateDate = this.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);

        });

        this.AutomationCondationOrList.forEach((item) => {
            item.CurrentEntityPM.AutomationsId = this.CurrentEntityPM.Id;
            item.CurrentEntityPM.UpdateDate = this.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);
        });


        if (this.CurrentEntityPM.ResultCode == "FIELDSET") {
            this.AutomationSetValueLists.forEach((item) => {
                automationSetValuelist.push(item.CurrentEntityPM);
            });
        }


        var automatedBackup: AutomatedBackup = new AutomatedBackup();

        automatedBackup.Name = this.CurrentEntityPM.Name;
        automatedBackup.CreateDate = this.CurrentEntityPM.CreateDate;
        automatedBackup.Description = this.CurrentEntityPM.Description;
        automatedBackup.ResultCode = this.CurrentEntityPM.ResultCode;
        automatedBackup.Id = this.CurrentEntityPM.Id;
        automatedBackup.Version = this.CurrentEntityPM.Version;
        automatedBackup.UpdateDate = this.CurrentEntityPM.UpdateDate;
        automatedBackup.Delaytime = this.DelayTime;
        automatedBackup.DelaytimeIndicator = this.DelaytimeIndicator;
        automatedBackup.Type = this.AutomatedBackupClass.Type;
        automatedBackup.DelayAautomationConditionLists = this.AutomatedBackupClass.DelayAautomationConditionLists;
        automatedBackup.AautomationConditionLists = automationConditionList;
        automatedBackup.IsAutomationResultEmailAllActiveUsers = this.IsAutomationResultEmailAllActiveUsers;

        if (this.CurrentEntityPM.ResultCode == "FIELDSET") automatedBackup.AutomationSetValueLists = automationSetValuelist;
        else if (this.CurrentEntityPM.ResultCode == "SETSLA") automatedBackup.AutomationSetSLAValue = this.AutomationSetSLAValue;
        else if (this.IsFollowUp()) automatedBackup.AutomationFollowUp = this.AutomationFollowUp;
        else if (this.CurrentEntityPM.ResultCode == "QUEUE") automatedBackup.AutomationQueuedTask = this.AutomationQueuedTask;

        this.CurrentEntityPM.AutomatedDataBackup = automatedBackup;
        this.CurrentEntityPM.IsChangeAutomationXaml = true;
        this.CurrentEntityPM.Version += 1;
        this.CurrentEntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();

        this._automationExtendedPMService.update(this.CurrentEntityPM).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                if (!this.IsNewEntity) {
                    ServiceLocator.SendTotangoUserActivity("Automation", "Edit Automation");
                }
                var myResult = pmResponse.Result;
                this.CurrentEntityPM = myResult;

                if (this.CurrentEntityPM && this.IsNewEntity) {
                    this.DataViewModel.RefreshAutomation(this.CurrentEntityPM, "Add");
                }
                else {
                    this.DataViewModel.RefreshAutomation(this.CurrentEntityPM, "Edit");
                }

                if (AppTool.IsNullOrEmpty(this.EventTypeCode)) {
                    this.EventTypeCodeList.push("AUUP");
                }
                else {
                    this.EventTypeCodeList.push("AUCR");
                }

                this.EventTypeCode = "";
                this.SaveTraceEvent();
            }
            else {
                SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    }

    SaveTraceEvent() {   
        if (this.EventTypeCodeList  && this.EventTypeCodeList.length != 0) {
            var traceEventArgs: EventTypeArgs = new EventTypeArgs();
            traceEventArgs.EventTypeCodeList = this.EventTypeCodeList;
            traceEventArgs.Tenant = SessionLocator.Tenant;
            traceEventArgs.ObjectTableId = this.ObjectTableId;
            traceEventArgs.EntityId = this.CurrentEntityPM.Id;
            traceEventArgs.LoggedContactId = SessionLocator.LoggedUserId;
            
            this._traceEventExtendedPMService.PutTraceEventGroup(traceEventArgs).subscribe(res => {    
                this.CloseButtonClicked();
            });
        }

        else {
            this.CloseButtonClicked();
        }
    }
    
    CheckIfAutomationSetSLAValueChange() {
        var isChange: boolean = false;
        if (this.CurrentEntityPM.ResultCode == "SETSLA") {

            if (this.AutomatedBackupClass.AutomationSetSLAValue) {
                if (this.AutomatedBackupClass.AutomationSetSLAValue.SLAId != this.AutomationSetSLAValue.SLAId) isChange = true;
            } else isChange = true;
        }
        return isChange;
    }
    
    CheckIfAutomationFollowUpChange() {
        var isChange: boolean = false;
        if (this.AutomatedBackupClass.AutomationFollowUp) {
            if (this.AutomatedBackupClass.AutomationFollowUp.EventTypeId != this.AutomationFollowUp.EventTypeId) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.LegType != this.AutomationFollowUp.LegType) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.ObjectTableName != this.AutomationFollowUp.ObjectTableName) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.OwnerValue != this.AutomationFollowUp.OwnerValue) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.OwnerFieldType != this.AutomationFollowUp.OwnerFieldType) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.NoteValue != this.AutomationFollowUp.NoteValue) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DateValue != this.AutomationFollowUp.DateValue) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DocumentTypeLists != this.AutomationFollowUp.DocumentTypeLists) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DateEscalationTime != this.AutomationFollowUp.DateEscalationTime) isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DateEscalationActionTimeIndicatorCode != this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode) isChange = true;
        }

        else isChange = true;

        return isChange;
    }

    CheckIfAutomationQueuedTaskChange() {
        var isChange: boolean = false;
        if (this.AutomatedBackupClass.AutomationQueuedTask) {
            if (this.AutomatedBackupClass.AutomationQueuedTask.DateFieldValue != this.AutomationQueuedTask.DateFieldValue) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.OffsetTypeValue != this.AutomationQueuedTask.OffsetTypeValue) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskTimeUnitValue != this.AutomationQueuedTask.TaskTimeUnitValue) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.QueueId != this.AutomationQueuedTask.QueueId) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TeamId != this.AutomationQueuedTask.TeamId) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskOffset != this.AutomationQueuedTask.TaskOffset) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskOwnerId != this.AutomationQueuedTask.TaskOwnerId) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskCustomerId != this.AutomationQueuedTask.TaskCustomerId) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskSubject != this.AutomationQueuedTask.TaskSubject) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskPriorityId != this.AutomationQueuedTask.TaskPriorityId) isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskDescription != this.AutomationQueuedTask.TaskDescription) isChange = true;
        }

        else isChange = true;

        return isChange;
    }
    
    //Automation Follow Up    
    FollowUpTypeComboBoxChanged(value: any) {
        if (value) {
            this.AutomationFollowUp.EventTypeId = value.Id;
        }
    }
    
    //Owner
    FollowUpOwnerObjectFieldComboBoxChanged(item: any) {
        this.FollowUpOwnerId = "";
        if (item) {
            this.FollowOwnerObjectFieldId = item.Id;
            this.AutomationFollowUp.OwnerFieldType = item.FieldName == "Specific" ? "Specific" : "Field";
        }

        this.FollowUpOwnerObjectFieldSelected = item;
    }
    
    FollowUpOwnerValueChange(item: any) {
        if (item) {
            this.FollowUpOwnerId = item.Id;
            this.FollowOwnerObjectFieldId = "";
            this.AutomationFollowUp.OwnerFieldType = "Specific"; 
        }
               
        else this.FollowUpOwnerId = "";
    }
    
    //Date
    FollowUpDateObjectFieldComboBoxChanged(item: any) {
        if (item) {
            this.DateValue = item.Id;
        } else this.DateValue = "";

        this.FollowUpDateObjectFieldSelected = item;
    }

    FollowDateEscalationActionTimeIndicatorCodeValueChange(event) {
        if (event.Code == "IM") {
            this.DataContext.UIProperties.SetEnabled("FollowDateEscalationTime", "Automation", false);
            this.FollowDateEscalationTime = 0;
        }

        else this.DataContext.UIProperties.SetEnabled("FollowDateEscalationTime", "Automation", true);
    }

    //Queued Task
    public IsQueueRequired: boolean = false;
    public IsSubjectRequired: boolean = false;

    private SetQueuedTaskRequired() {
        var isQueueRequired: boolean = false
        var isSubjectRequired: boolean = false

        if (AppTool.IsNullOrEmpty(this.QueueId)) {
            isQueueRequired = true;
        }

        if (AppTool.IsNullOrEmpty(this.TaskSubject)) {
            isSubjectRequired = true;
        }

        this.IsQueueRequired = isQueueRequired;
        this.IsSubjectRequired = isSubjectRequired;
    }

    private queueId: string;
    get QueueId() { return this.queueId; }
    set QueueId(newValue: string) {
        if (this.queueId != newValue) {
            this.queueId = newValue;
            
            this.SetQueuedTaskRequired();
        }
    }

    private taskSubject: string;
    get TaskSubject() { return this.taskSubject; }
    set TaskSubject(newValue: string) {
        if (this.taskSubject != newValue) {
            this.taskSubject = newValue;

            this.SetQueuedTaskRequired();
        }
    }
    
    public TeamId: string;
    public TaskOffset: number;
    public TaskOwnerId: string;
    public TaskCustomerId: string;
    public TaskPriorityId: string;
    public TaskDescription: string;

    public DueDateFieldList: CodeNameClass[];
    public TaskOffsetTypeList: CodeNameClass[];
    public TaskTimeUnitList: CodeNameClass[];

    private BuildQueuedTaskFilters() {
        this.DueDateFieldList = [];
        this.TaskOffsetTypeList = [];
        this.TaskTimeUnitList = [];

        var shipmentObjecttableId: string = window.ObjectTables.filter(f => f.Id == this.ObjectTableId)[0].Id;
        var taskObjectFields: ObjectFieldPM[] = window.ObjectFields.filter(f => f.ObjectTableId == shipmentObjecttableId && f.DataTypeCode == "DateTime");

        taskObjectFields.forEach((item) => {
            if (item.FieldName == "MainCarriageETD" || item.FieldName == "MainCarriageATD" || item.FieldName == "MainCarriageFinalDestinationETA" || item.FieldName == "MainCarriageFinalDestinationATA"
                || item.FieldName == "OperationalDate" || item.FieldName == "OperationalCloseDate" || item.FieldName == "AccountingCloseDate" || item.FieldName == "FinalArrivalDate") {
                var dateItem: CodeNameClass = new CodeNameClass(item.FieldName, item.FullNameTextCodeDefaultText);
                this.DueDateFieldList.push(dateItem);
            }            
        });
        
        this.TaskOffsetTypeList.push(new CodeNameClass("B", "Before"));
        this.TaskOffsetTypeList.push(new CodeNameClass("A", "After"));

        this.TaskTimeUnitList.push(new CodeNameClass("D", "Days"));
        this.TaskTimeUnitList.push(new CodeNameClass("H", "Hours"));

        this.selectedDueDateField = new CodeNameClass();
        this.selectedTaskOffsetType = this.TaskOffsetTypeList.filter(d => d.Code == "B")[0];
        this.selectedTaskTimeUnit = this.TaskTimeUnitList.filter(d => d.Code == "D")[0];
    }

    private selectedDueDateField: CodeNameClass;
    get SelectedDueDateField() { return this.selectedDueDateField; }
    set SelectedDueDateField(value: CodeNameClass) {
        if (this.selectedDueDateField != value) {
            this.selectedDueDateField = value;
        }
    }

    private selectedTaskOffsetType: CodeNameClass;
    get SelectedTaskOffsetType() { return this.selectedTaskOffsetType; }
    set SelectedTaskOffsetType(value: CodeNameClass) {
        if (this.selectedTaskOffsetType != value) {
            this.selectedTaskOffsetType = value;
        }
    }

    private selectedTaskTimeUnit: CodeNameClass;
    get SelectedTaskTimeUnit() { return this.selectedTaskTimeUnit; }
    set SelectedTaskTimeUnit(value: CodeNameClass) {
        if (this.selectedTaskTimeUnit != value) {
            this.selectedTaskTimeUnit = value;
        }
    }
}

class ResultCode {
    Code: string;
    Name: string;
    
    constructor(name: string, code : string) {
        this.Code = code;
        this.Name = name;

    }
}
