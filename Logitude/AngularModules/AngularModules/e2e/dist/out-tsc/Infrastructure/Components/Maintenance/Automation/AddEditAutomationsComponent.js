"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="../../../datacontracts/automationargs.ts" />
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AutomationExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/AutomationExtendedPMService");
var ObjectFieldPM_1 = require("../../../../Infrastructure/EntityPMs/ObjectFieldPM");
var AutomatedBackup_1 = require("../../../../Infrastructure/DataContracts/AutomatedBackup");
var AutomationSetValue_1 = require("../../../../Infrastructure/DataContracts/AutomationSetValue");
var AutomationFollowUp_1 = require("../../../../Infrastructure/DataContracts/AutomationFollowUp");
var EventTypeListService_1 = require("../../../../Infrastructure/Services/StandardLists/EventTypeListService");
var SLAHeaderListService_1 = require("../../../../CRM/Services/StandardLists/SLAHeaderListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var AutomationCondition_1 = require("../../../../Infrastructure/DataContracts/AutomationCondition");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocumentTypeListService_1 = require("../../../../Common/Services/StandardLists/DocumentTypeListService");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var AutomationHistoryExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/AutomationHistoryExtendedPMService");
var AutomationResultEmailRecipientExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/AutomationResultEmailRecipientExtendedService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var AutomationResultEmailRecipientPM_1 = require("../../../../Common/EntityPMs/AutomationResultEmailRecipientPM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var DocumentTypeTemplateViewModel_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var AutomationConditionViewModel_1 = require("./ViewModel/AutomationConditionViewModel");
var ResultEmailRecipientViewModel_1 = require("./ViewModel/ResultEmailRecipientViewModel");
var AutomationSetValueViewModel_1 = require("./ViewModel/AutomationSetValueViewModel");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var AutomationQueuedTask_1 = require("../../../../Infrastructure/DataContracts/AutomationQueuedTask");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var AddEditAutomationsComponent = /** @class */ (function (_super) {
    __extends(AddEditAutomationsComponent, _super);
    function AddEditAutomationsComponent(_automationResultEmailRecipientExtendedService, _documentTypeTemplatePMExtendedService, _automationExtendedPMService, _automationHistoryExtendedPMService, cd, entityArgs) {
        var _this = _super.call(this) || this;
        _this._automationResultEmailRecipientExtendedService = _automationResultEmailRecipientExtendedService;
        _this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        _this._automationExtendedPMService = _automationExtendedPMService;
        _this._automationHistoryExtendedPMService = _automationHistoryExtendedPMService;
        _this.cd = cd;
        _this.entityArgs = entityArgs;
        _this.AutomationSetSLAValue = new AutomatedBackup_1.AutomationSetSLAValue();
        _this.AutomationFollowUp = new AutomationFollowUp_1.AutomationFollowUp();
        _this.AutomationQueuedTask = new AutomationQueuedTask_1.AutomationQueuedTask();
        _this.ObjectFieldsLists = [];
        _this.AllowedinAutomationConditionsFieldLists = [];
        _this.AutomationEmailRecipientFieldLists = [];
        _this.AutomationSetValuebjectFieldLists = [];
        _this.IsLoadPage = false;
        _this.IsChangeAutomation = false;
        _this.EmailRecipientFieldLists = [];
        _this.FollowDateUniteCode = "Days";
        _this.AutomationResultEmailRecipientPMList = [];
        _this.DataContext = _this;
        _this.AllDocumentTypeLists = [];
        _this.IsEnableAddTemplate = false;
        _this.IsEnableEditTemplate = false;
        _this.AutomationCondationOrList = [];
        _this.AutomationCondationAndList = [];
        _this.AutomationSetValueLists = [];
        _this.PageChild_EVE = null;
        _this.IsNewEntity = false;
        _this.IsSelectedImmediatly = false;
        _this.IsSelectedDelayed = false;
        _this.IsLoadingComplete = false;
        _this.EventDocFollowUpTypeLists = [];
        _this.EventFollowUpTypeLists = [];
        _this.FollowUpOwnerObjectFieldLists = [];
        _this.FollowUpDateObjectFieldLists = [];
        _this.FollowUpOwnerId = "";
        _this.FollowOwnerObjectFieldId = "";
        _this.DateValue = "";
        _this.FollowDateEscalationActionTimeIndicatorCode = "";
        _this.FollowUpNote = "";
        _this.IsShowAutomationCodeField = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsLoadEventFollowUp = false;
        _this.IsLoadSLAHeaders = false;
        _this.IsAutomationResultEmailAllActiveUsers = false;
        _this.IsMasterShipment = false;
        _this.IsFirstTime = false;
        _this.UserIds = "";
        _this.OldUserIds = "";
        _this.IsLoadAutomationResultEmailRecipient = false;
        _this.IsChangeCondition = false;
        _this.IsChangeSetValue = false;
        _this.ParticipantsList = [];
        _this.ExsitCode = "";
        //Queued Task
        _this.IsQueueRequired = false;
        _this.IsSubjectRequired = false;
        if (SessionLocator_1.SessionLocator.TenantPM.Id == 0)
            _this.IsShowAutomationCodeField = true;
        _this._documentTypeListService = new DocumentTypeListService_1.DocumentTypeListService();
        return _this;
    }
    AddEditAutomationsComponent.prototype.ngOnInit = function () {
        this.DataContext.UIProperties.SetEnabled("FollowDateUniteCode", "Automation", false);
    };
    AddEditAutomationsComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.ObjectTableId = args.ObjectTableId;
        this.DataViewModel = args.DataViewModel;
        this.PageType = args.PageType;
        this.CurrentEntityPM = args.AutomationPM;
        this.Mode = args.Mode;
        this.ObjectTableName = args.ObjectTableName;
        this.IsNewEntity = args.IsNewEntity;
        this.DelayedHtmlinputId = Guid_1.Guid.newGuid();
        this.ImmediatlyHtmlinputId = Guid_1.Guid.newGuid();
        this.InactiveKey = Guid_1.Guid.newGuid();
        this.IsMasterShipment = args.IsMasterShipment;
        this.entityArgs.EntityPM = this.CurrentEntityPM;
        this.entityArgs.ObjectTableName = "Automation";
        this.SLAHeaderLists = [];
        this.BuildQueuedTaskFilters();
        var myService = new EventTypeListService_1.EventTypeListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var lists = myResponse.Result;
                // this.EventFollowUpTypeLists = lists.filter(f => f.ObjectTableId == this.ObjectTableId && f.AllowedInAutomation == true);
                _this.EventFollowUpTypeLists = lists.filter(function (f) { return f.ObjectTableId == _this.ObjectTableId && f.ManualActivatedFollowUp == true; });
                _this.EventDocFollowUpTypeLists = lists.filter(function (f) { return f.ObjectTableId == _this.ObjectTableId && (f.Code == "DOCO" || f.Code == "DOCI"); });
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.AutomationFollowUp.EventTypeId)) {
                    _this.FollowUpTypeSelected = _this.EventFollowUpTypeLists.filter(function (d) { return d.Id == _this.AutomationFollowUp.EventTypeId; })[0];
                }
            }
            _this.IsLoadEventFollowUp = true;
        });
        if (this.ObjectTableName == "Ticket") {
            var sLAHeaderListService = new SLAHeaderListService_1.SLAHeaderListService();
            sLAHeaderListService.getAll().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        _this.SLAHeaderLists = myResponse.Result.filter(function (d) { return d.Inactive == false; });
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.AutomationSetSLAValue.SLAId)) {
                            _this.SLAHeaderSelected = _this.SLAHeaderLists.filter(function (d) { return d.Id == _this.AutomationSetSLAValue.SLAId; })[0];
                        }
                    }
                }
                _this.IsLoadSLAHeaders = true;
            });
        }
        if (this.CurrentEntityPM.AutomatedDataBackup) {
            this.AutomatedBackupClass = this.CurrentEntityPM.AutomatedDataBackup;
            this.Start();
        }
        else {
            if (this.IsNewEntity) {
                this.AutomatedBackupClass = new AutomatedBackup_1.AutomatedBackup();
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
    };
    AddEditAutomationsComponent.prototype.LoadAutomationDataBackup = function () {
        var _this = this;
        if (this.CurrentEntityPM && this.CurrentEntityPM.Id) {
            this._automationExtendedPMService.getAutomationBackupDataById(this.CurrentEntityPM.Id, this.CurrentEntityPM.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    _this.CurrentEntityPM.AutomatedDataBackup = myResult;
                    _this.AutomatedBackupClass = myResult;
                    _this.Start();
                }
            });
        }
    };
    AddEditAutomationsComponent.prototype.LoadDocumentType = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.DocumentTypeLists = [];
        this.AllDocumentTypeLists = [];
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        apiQueryFilters.ObjectTableName = this.ObjectTableName;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.AllDocumentTypeLists = pmResponse.Result.filter(function (a) { return a.ObjectTableId == _this.ObjectTableId && !a.InActive; });
                if (_this.ObjectTableName == "Shipment") {
                    if (_this.IsMasterShipment) {
                        _this.AllDocumentTypeLists = _this.AllDocumentTypeLists.filter(function (d) { return d.IsMaster || d.IsDirect; });
                    }
                    else {
                        _this.AllDocumentTypeLists = _this.AllDocumentTypeLists.filter(function (d) { return d.IsHouse || d.IsDirect; });
                    }
                }
                _this.DocumentTypeLists = _this.AllDocumentTypeLists.filter(function (a) { return a.IsDocOut && a.TemplateFormatCode == "M"; });
                if (_this.DocumentTypeLists && _this.DocumentTypeLists.length > 0) {
                    if (_this.CurrentEntityPM.DocumentTypeId) {
                        _this.DocumentTypeSelected = _this.DocumentTypeLists.filter(function (d) { return d.Id == _this.CurrentEntityPM.DocumentTypeId; })[0];
                    }
                    if (!_this.DocumentTypeSelected) {
                        _this.DocumentTypeSelected = _this.DocumentTypeLists[0];
                        if (_this.DocumentTypeSelected) {
                            _this.CurrentEntityPM.DocumentTypeId = _this.DocumentTypeSelected.Id;
                        }
                    }
                    if (_this.DocumentTypeSelected) {
                        _this.LoadDocumentTypeTemplate(_this.DocumentTypeSelected);
                    }
                    else
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    _this.IsEnableAddTemplate = true;
                }
                else {
                    _this.IsEnableAddTemplate = false;
                    _this.IsEnableEditTemplate = false;
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                }
            }
            else {
                _this.IsEnableAddTemplate = false;
                _this.IsEnableEditTemplate = false;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    };
    AddEditAutomationsComponent.prototype.LoadDocumentTypeTemplate = function (documentTypeList) {
        var _this = this;
        this.DocumentTypeTemplateLists = [];
        this._documentTypeTemplatePMExtendedService.getDocumentTypeTemplatesByDocumentTypeIdForAutomations(documentTypeList.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach(function (item) {
                    _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                });
                if (_this.DocumentTypeTemplateLists && _this.DocumentTypeTemplateLists.length > 0) {
                    if (_this.CurrentEntityPM.TemplateId) {
                        _this.DocumentTypetTemplateSelected = _this.DocumentTypeTemplateLists.filter(function (d) { return d.Id == _this.CurrentEntityPM.TemplateId; })[0];
                    }
                    if (!_this.DocumentTypetTemplateSelected) {
                        _this.DocumentTypetTemplateSelected = _this.DocumentTypeTemplateLists.filter(function (d) { return d.Id == documentTypeList.DocumentTypeDefaultHTMLTemplateId; })[0];
                        if (!_this.DocumentTypetTemplateSelected) {
                            _this.DocumentTypetTemplateSelected = _this.DocumentTypeTemplateLists[0];
                        }
                        if (_this.DocumentTypetTemplateSelected) {
                            _this.CurrentEntityPM.TemplateId = _this.DocumentTypetTemplateSelected.Id;
                        }
                    }
                    _this.IsEnableEditTemplate = true;
                }
                else {
                    _this.IsEnableEditTemplate = false;
                    _this.DocumentTypetTemplateSelected = null;
                }
            }
        });
    };
    AddEditAutomationsComponent.prototype.DocumentTypeListsValueChanged = function (item) {
        if (item != null) {
            this.DocumentTypeSelected = item;
            this.CurrentEntityPM.DocumentTypeId = item.Id;
            this.LoadDocumentTypeTemplate(item);
            this.IsChangeAutomation = true;
        }
    };
    AddEditAutomationsComponent.prototype.DocumentTypeTemplateListValueChanged = function (item) {
        if (item != null) {
            this.DocumentTypetTemplateSelected = item;
            this.CurrentEntityPM.TemplateId = item.Id;
            this.IsChangeAutomation = true;
        }
    };
    AddEditAutomationsComponent.prototype.EditDocumentTemplate = function (item) {
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
                var windowArgs = {};
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
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Html Template";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
            }
        }
    };
    AddEditAutomationsComponent.prototype.AddDocumentTypeTemplate = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Maintenance";
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.CurrentEntityPM = this.DocumentTypeSelected;
        windowArgs.DocumentTypeTemplateLists = this.DocumentTypeTemplateLists;
        windowArgs.TypeTab = "RichText";
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = "New Html Template";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/NewReportTemplateComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            if ($event && _this.DocumentTypeTemplateLists && _this.DocumentTypeTemplateLists.length > 0) {
                _this.IsEnableEditTemplate = true;
            }
        });
    };
    AddEditAutomationsComponent.prototype.VeiwAutomationHositoryButtonClicked = function (item) {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.AutomationHistoryPM = item;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 560;
        logWindow.IsShowCloseButton = true;
        logWindow.Title = "View Automation";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/ViewAutomationHistoryComponent");
    };
    AddEditAutomationsComponent.prototype.UpdateCurrentAutomationHository = function (item) {
        this.AutomationHistoryLists.filter(function (d) { return d.AutomationsId == item.AutomationsId && d.Version == item.Version; })[0] = item;
    };
    Object.defineProperty(AddEditAutomationsComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAutomationsComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.SelectedTabCode != null) {
            if (this.SelectedTabCode == "EVE") {
                var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'EVE'; });
                var myLocation = locs.filter(function (f) { return f.Code == "EVE"; })[0];
                if (myLocation != null) {
                    if (this.PageChild_EVE == null) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Common/Components/Events/EventsTabComponent', myLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            _this.PageChild_EVE = cmpRef.instance;
                        });
                    }
                    else {
                        this.PageChild_EVE.LoadData();
                    }
                }
            }
        }
    };
    AddEditAutomationsComponent.prototype.LoadAutomationHistory = function () {
        var _this = this;
        if (this.CurrentEntityPM && !Tools_1.AppTool.IsNullOrEmpty(this.CurrentEntityPM.Id)) {
            this._automationHistoryExtendedPMService.getAutomationHistoryesByAutomationId(this.CurrentEntityPM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    myResult.forEach(function (item) {
                        _this.AutomationHistoryLists.push(item);
                    });
                }
            });
        }
    };
    Object.defineProperty(AddEditAutomationsComponent.prototype, "ResultCodeSelected", {
        get: function () { return this.resultCodeSelected; },
        set: function (newValue) {
            if (this.resultCodeSelected != newValue) {
                this.resultCodeSelected = newValue;
                this.SetQueuedTaskRequired();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAutomationsComponent.prototype.Start = function () {
        var _this = this;
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
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.BusinessProcessQueue")) {
                    this.ResultCodeList.push(new ResultCode("Queued Task", "QUEUE"));
                }
            }
            this.ResultCodeSelected = this.ResultCodeList.filter(function (d) { return d.Code == _this.AutomatedBackupClass.ResultCode; })[0];
            if (!this.ResultCodeSelected) {
                this.ResultCodeSelected = this.ResultCodeList.filter(function (d) { return d.Code == "EMAIL"; })[0];
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
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                        this.FollowUpTypeSelected = this.EventFollowUpTypeLists.filter(function (d) { return d.Id == _this.AutomationFollowUp.EventTypeId; })[0];
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
                this.SelectedDueDateField = this.DueDateFieldList.filter(function (f) { return f.Code == _this.AutomationQueuedTask.DateFieldValue; })[0];
                this.SelectedTaskOffsetType = this.TaskOffsetTypeList.filter(function (f) { return f.Code == _this.AutomationQueuedTask.OffsetTypeValue; })[0];
                this.TaskOffset = this.AutomationQueuedTask.TaskOffset;
                this.SelectedTaskTimeUnit = this.TaskTimeUnitList.filter(function (f) { return f.Code == _this.AutomationQueuedTask.TaskTimeUnitValue; })[0];
                this.TaskCustomerId = this.AutomationQueuedTask.TaskCustomerId;
                this.TaskPriorityId = this.AutomationQueuedTask.TaskPriorityId;
            }
            if (this.ResultCodeSelected.Code == "SETSLA") {
                this.AutomationSetSLAValue.SLAId = this.AutomatedBackupClass.AutomationSetSLAValue ? this.AutomatedBackupClass.AutomationSetSLAValue.SLAId : "";
                if (this.AutomationSetSLAValue) {
                    if (this.SLAHeaderLists && this.IsLoadSLAHeaders) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.AutomationSetSLAValue.SLAId)) {
                            this.SLAHeaderSelected = this.SLAHeaderLists.filter(function (d) { return d.Id == _this.AutomationSetSLAValue.SLAId; })[0];
                        }
                    }
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode)) {
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
            this.Code = this.CurrentEntityPM.Code;
            this.FillObjectField();
            this.LoadAutomationHistory();
            this.LoadDocumentType();
        }
        this.IsLoadingComplete = true;
    };
    AddEditAutomationsComponent.prototype.DelaytimeIndicatorChange = function (value) {
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
    };
    AddEditAutomationsComponent.prototype.SelectDocumentTypes = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        var windowArgs = {};
        windowArgs.AddEditAutomationsComponent = this;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "Document Types";
        logWindow.DataContext = this;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/SelectDocumentTypesComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                if (_this.AutomationFollowUp.DocumentTypeLists && _this.AutomationFollowUp.DocumentTypeLists.length > 0) {
                    _this.CountDocumentSelection = _this.AutomationFollowUp.DocumentTypeLists.length + " documents selected";
                }
                else {
                    _this.CountDocumentSelection = "no documents selected";
                }
            }
        });
    };
    AddEditAutomationsComponent.prototype.FillObjectField = function () {
        var _this = this;
        this.AllowedinAutomationConditionsFieldLists = [];
        this.AutomationEmailRecipientFieldLists = [];
        this.AutomationSetValuebjectFieldLists = [];
        this.FollowUpOwnerObjectFieldLists = [];
        this.ObjectFieldsLists = [];
        this.ObjectFieldsLists = window.ObjectFields.filter(function (f) { return f.ObjectTableId == _this.ObjectTableId; });
        this.ObjectFieldsLists.forEach(function (objectField) {
            if (objectField.AllowedinAutomationConditions || objectField.IsCustom) {
                _this.AllowedinAutomationConditionsFieldLists.push(objectField);
            }
            if (objectField.AutomationEmailRecipient && (objectField.ObjectTable_LookUpTableName == "User" || objectField.ObjectTable_LookUpTableName == "Contact" || objectField.DataTypeCode == "Emails")) {
                _this.AutomationEmailRecipientFieldLists.push(objectField);
            }
            if (objectField.CanAutomateSetValue && !objectField.IsCustom)
                _this.AutomationSetValuebjectFieldLists.push(objectField);
            if (objectField.FieldName == "CreatedByUserId" || objectField.FieldName == "SalesmanUserId" || objectField.FieldName == "UpdatedByUserId" || objectField.FieldName == "AccountManagerUserId") {
                _this.FollowUpOwnerObjectFieldLists.push(objectField);
            }
            if (objectField.FieldName == "MainCarriageETD" || objectField.FieldName == "MainCarriageATD" || objectField.FieldName == "MainCarriageFinalDestinationETA" || objectField.FieldName == "MainCarriageFinalDestinationATA") {
                _this.FollowUpDateObjectFieldLists.push(objectField);
            }
            if (_this.ObjectTableName == "Ticket") {
                if (objectField.FieldName == "SLAId") {
                    _this.AutomationSetSLAValue.ObjectFieldId = objectField.Id;
                }
            }
        });
        var specifiOwnerObjectField = new ObjectFieldPM_1.ObjectFieldPM();
        specifiOwnerObjectField.FullNameTextCodeDefaultText = "Specific";
        specifiOwnerObjectField.Id = "Specific";
        specifiOwnerObjectField.FieldName = "Specific";
        this.FollowUpOwnerObjectFieldLists.push(specifiOwnerObjectField);
        if (this.FollowUpOwnerObjectFieldLists) {
            if (this.AutomationFollowUp.OwnerFieldType == "Specific") {
                this.FollowUpOwnerObjectFieldSelected = this.FollowUpOwnerObjectFieldLists.filter(function (d) { return d.Id == "Specific"; })[0];
            }
            else {
                this.FollowUpOwnerObjectFieldSelected = this.FollowUpOwnerObjectFieldLists.filter(function (d) { return d.Id == _this.FollowOwnerObjectFieldId; })[0];
            }
        }
        this.FollowUpDateObjectFieldSelected = this.FollowUpDateObjectFieldLists.filter(function (d) { return d.Id == _this.DateValue; })[0];
        if (this.AutomatedBackupClass) {
            this.BuildAutomationCondition();
            this.BuildAutomationSetValue();
        }
        this.GenerateControlEntityContactVariable();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentEntityPM.Id)) {
            this.LoadAutomationResultEmailRecipient();
        }
        else {
            this.IsLoadAutomationResultEmailRecipient = true;
        }
    };
    AddEditAutomationsComponent.prototype.BuildAutomationCondition = function () {
        var _this = this;
        this.IsViewCondition = false;
        var automationConditionPMList = this.AutomatedBackupClass.AautomationConditionLists;
        if (automationConditionPMList != null) {
            automationConditionPMList.forEach(function (item) {
                if (item.ConditionType == "And") {
                    _this.AutomationCondationAndList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(item, _this));
                }
                else if (item.ConditionType == "Or") {
                    _this.AutomationCondationOrList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(item, _this));
                }
            });
        }
    };
    AddEditAutomationsComponent.prototype.BuildAutomationSetValue = function () {
        var _this = this;
        this.AutomationSetValueLists = [];
        var automationSetValueLists = this.AutomatedBackupClass.AutomationSetValueLists;
        if (automationSetValueLists) {
            automationSetValueLists.forEach(function (item) {
                _this.AutomationSetValueLists.push(new AutomationSetValueViewModel_1.AutomationSetValueViewModel(item, _this));
            });
        }
    };
    AddEditAutomationsComponent.prototype.LoadAutomationResultEmailRecipient = function () {
        var _this = this;
        this.EntityContactVariable = [];
        this.UserIds = "";
        this._automationResultEmailRecipientExtendedService.getAutomationResultEmailRecipientByAutomationId(this.CurrentEntityPM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach(function (item) {
                    if (item.RecipientType == "Fixed") {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.RecipientValue)) {
                            _this.UserIds += (item.RecipientValue + ";");
                            _this.OldUserIds += (item.RecipientValue + ";");
                        }
                    }
                    else {
                        _this.EntityContactVariable.push(item.RecipientValue);
                    }
                    _this.AutomationResultEmailRecipientPMList.push(item);
                });
            }
            _this.IsLoadAutomationResultEmailRecipient = true;
            _this.GenerateControlEntityContactVariable();
        });
    };
    AddEditAutomationsComponent.prototype.GenerateControlEntityContactVariable = function () {
        var _this = this;
        this.EmailRecipientFieldLists = [];
        this.AutomationEmailRecipientFieldLists.forEach(function (item) {
            _this.EmailRecipientFieldLists.push(new ResultEmailRecipientViewModel_1.ResultEmailRecipientViewModel(item, _this));
        });
    };
    AddEditAutomationsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("Cancel");
    };
    AddEditAutomationsComponent.prototype.AddAutomationConditionMethod = function (conditionType) {
        var automationConditionPM = new AutomationCondition_1.AutomationCondition();
        automationConditionPM.ConditionType = conditionType;
        automationConditionPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        automationConditionPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        automationConditionPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        automationConditionPM.Value = "";
        automationConditionPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.OperatorCode = "Equals";
        automationConditionPM.ObjectFieldId = "";
        if (conditionType == "And") {
            this.AutomationCondationAndList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(automationConditionPM, this));
        }
        else {
            this.AutomationCondationOrList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(automationConditionPM, this));
        }
    };
    AddEditAutomationsComponent.prototype.ResultCodeListValueChanged = function (value) {
        this.ResultCodeSelected = value;
        this.CurrentEntityPM.ResultCode = value.Code;
        this.IsChangeAutomation = true;
        if (value.Code == "DOCOUTFOLLOWUP" || value.Code == "DOCINFOLLOWUP") {
            this.AutomationFollowUp.DocumentTypeLists = [];
            this.CountDocumentSelection = "no documents selected";
            this.FollowUpNote = "";
        }
    };
    Object.defineProperty(AddEditAutomationsComponent.prototype, "TypeWidth", {
        get: function () {
            var result = 200;
            if (this.ResultCodeSelected != null) {
                if (this.ResultCodeSelected.Code == 'FOLLOWUP' || this.ResultCodeSelected.Code == 'DOCOUTFOLLOWUP' || this.ResultCodeSelected.Code == 'DOCINFOLLOWUP') {
                    result = 270;
                }
                else if (this.ResultCodeSelected.Code == "QUEUE") {
                    result = 236;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    AddEditAutomationsComponent.prototype.SelectedDelayedImmediatlRadioClcik = function (type) {
        if (type == "Immeduiatly") {
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
    };
    AddEditAutomationsComponent.prototype.ViewDelayAutomationconditionsButtonClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.IsShowCloseButton = true;
        logWindow.DataContext = this;
        logWindow.IsShowAutomationDelayTitle = true;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/DelayAutomationconditionsComponent");
    };
    AddEditAutomationsComponent.prototype.AddAutomationSetValueButtonClick = function () {
        var automationSetValue = new AutomationSetValue_1.AutomationSetValue();
        automationSetValue.ObjectFieldId = "";
        automationSetValue.OperatorCode = "SV";
        automationSetValue.Value = "";
        automationSetValue.FieldName = "";
        this.AutomationSetValueLists.push(new AutomationSetValueViewModel_1.AutomationSetValueViewModel(automationSetValue, this));
    };
    AddEditAutomationsComponent.prototype.AutomationCodeValueChange = function (value) {
        var _this = this;
        if (this.Code && this.ExsitCode != this.Code) {
            this.ExsitCode = this.Code;
            this._automationExtendedPMService.GetDoesAutomationCodeExist(this.Code).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult == true) {
                        _this.ValidationErrorsList = [];
                        _this.ValidationErrorsList.push("The code " + _this.Code + " already exists");
                    }
                    else
                        _this.ValidationErrorsList = [];
                }
            });
        }
    };
    AddEditAutomationsComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ParticipantsList = [];
        this.ValidationErrorsList = [];
        if (this.OldUserIds != this.UserIds) {
            this.IsChangeAutomation = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.UserIds)) {
            this.UserIds.split(';').forEach(function (id) {
                if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
                    _this.ParticipantsList.push(id);
                }
            });
        }
        if (this.Name != this.CurrentEntityPM.Name || this.Description != this.CurrentEntityPM.Description || this.Inactive != this.IsActiveAutomation)
            this.IsChangeAutomation = true;
        this.CurrentEntityPM.Name = this.Name;
        this.CurrentEntityPM.Code = this.Code;
        this.CurrentEntityPM.Description = this.Description;
        this.CurrentEntityPM.Inactive = this.Inactive;
        var isFollowUp = this.IsFollowUp();
        if (isFollowUp) {
            this.AutomationFollowUp.OwnerValue = this.AutomationFollowUp.OwnerFieldType == "Field" ? this.FollowOwnerObjectFieldId : this.FollowUpOwnerId;
            this.AutomationFollowUp.NoteValue = this.FollowUpNote;
            this.AutomationFollowUp.DateEscalationTime = this.FollowDateEscalationTime;
            this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode = this.FollowDateEscalationActionTimeIndicatorCode;
            this.AutomationFollowUp.DateValue = this.DateValue;
            if (this.CurrentEntityPM.ResultCode == "DOCOUTFOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCINFOLLOWUP") {
                var eventCode = this.CurrentEntityPM.ResultCode == "DOCOUTFOLLOWUP" ? "DOCO" : "DOCI";
                var eventType = this.EventDocFollowUpTypeLists.filter(function (d) { return d.Code == eventCode; })[0];
                if (eventType)
                    this.AutomationFollowUp.EventTypeId = eventType.Id;
                if (!this.AutomationFollowUp.DocumentTypeLists)
                    this.AutomationFollowUp.DocumentTypeLists = [];
                if (this.AutomationFollowUp.DocumentTypeLists.length == 0)
                    this.ValidationErrorsList.push("Please select at least one document");
            }
            else if (this.CurrentEntityPM.ResultCode == "FOLLOWUP") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                    var eventType = this.EventDocFollowUpTypeLists.filter(function (d) { return d.Id == _this.AutomationFollowUp.EventTypeId; })[0];
                    if (eventType)
                        this.AutomationFollowUp.EventTypeId = "";
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                this.ValidationErrorsList.push("F/U Type field is required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.AutomationFollowUp.OwnerValue)) {
                this.ValidationErrorsList.push("Owner field is required");
            }
            this.AutomationFollowUp.LegType = "";
            if (this.EventFollowUpTypeLists && !Tools_1.AppTool.IsNullOrEmpty(this.AutomationFollowUp.EventTypeId)) {
                var eventList = this.EventFollowUpTypeLists.filter(function (d) { return d.Id == _this.AutomationFollowUp.EventTypeId; })[0];
                if (eventList != null) {
                    if (eventList.Code == "DEP")
                        this.AutomationFollowUp.LegType = "MainCarriageDeparture";
                    else if (eventList.Code == "ARR")
                        this.AutomationFollowUp.LegType = "MainCarriageArrival";
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
        if (Tools_1.AppTool.IsNullOrEmpty(this.CurrentEntityPM.Name)) {
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
            this.AutomationSetValueLists.forEach(function (item) {
                if (Tools_1.AppTool.IsNullOrEmpty(item.CurrentEntityPM.Value)) {
                    _this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " field is required");
                }
                else if (item.CurrentEntityPM.Value.length > item.SelectedCustomField.MaxLength || item.CurrentEntityPM.Value.length < item.SelectedCustomField.MinLength) {
                    _this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " must butween " + item.SelectedCustomField.MinLength + " and " + item.SelectedCustomField.MaxLength + " characters");
                }
            });
        }
        if (this.CurrentEntityPM.ResultCode == "SETSLA" && Tools_1.AppTool.IsNullOrEmpty(this.AutomationSetSLAValue.SLAId)) {
            this.ValidationErrorsList.push("Please select sla type");
        }
        if (this.AutomatedBackupClass.Delaytime != this.DelayTime) {
            this.IsChangeAutomation = true;
        }
        if (this.CurrentEntityPM.ResultCode == "QUEUE") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.QueueId)) {
                this.ValidationErrorsList.push("Queue field is required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.TaskSubject)) {
                this.ValidationErrorsList.push("Subject field is required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.TaskOwnerId) && Tools_1.AppTool.IsNullOrEmpty(this.TeamId)) {
                this.ValidationErrorsList.push("Please fill Owner or Team");
            }
        }
        if (this.AutomatedBackupClass.Type == "Delayed") {
            if (!this.DelayTime || this.DelayTime < 1) {
                this.ValidationErrorsList.push("Delay time must be greater than 0");
            }
        }
        if (this.IsShowAutomationCodeField && Tools_1.AppTool.IsNullOrEmpty(this.Code)) {
            this.ValidationErrorsList.push("Code field is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this.SaveAutomationBackup();
            this.SaveAutomationResultEmailRecipientLists();
            if (this.IsNewEntity) {
                this._automationExtendedPMService.insert(this.CurrentEntityPM).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Automation", "New Automation");
                        var myResult = pmResponse.Result;
                        _this.CurrentEntityPM = myResult;
                        _this.DataViewModel.RefreshAutomation(_this.CurrentEntityPM, "Add");
                        _this.CloseButtonClicked();
                    }
                    else {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                });
            }
            else {
                if ((this.IsChangeCondition || this.IsChangeSetValue || this.IsChangeAutomation) || (isFollowUp && this.CheckIfAutomationFollowUpChange()) || (this.CurrentEntityPM.ResultCode == "SETSLA" && this.CheckIfAutomationSetSLAValueChange()) || (this.CurrentEntityPM.ResultCode == "QUEUE" && this.CheckIfAutomationQueuedTaskChange())) {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                    this.CurrentEntityPM.Version += 1;
                    this.CurrentEntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    this._automationExtendedPMService.update(this.CurrentEntityPM).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Automation", "Edit Automation");
                            var myResult = pmResponse.Result;
                            _this.CurrentEntityPM = myResult;
                            _this.DataViewModel.RefreshAutomation(_this.CurrentEntityPM, "Edit");
                            _this.CloseButtonClicked();
                        }
                        else {
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    });
                }
                else
                    this.CloseButtonClicked();
            }
        }
    };
    AddEditAutomationsComponent.prototype.SaveAutomationResultEmailRecipientLists = function () {
        if (this.CurrentEntityPM.ResultCode == "EMAIL")
            this.CurrentEntityPM.AutomationResultEmailRecipientLists = this.BuildAutomationResultEmailRecipient();
    };
    AddEditAutomationsComponent.prototype.IsFollowUp = function () {
        if (this.CurrentEntityPM.ResultCode == "FOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCOUTFOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCINFOLLOWUP")
            return true;
        else
            return false;
    };
    AddEditAutomationsComponent.prototype.SLAHeaderValueChanged = function (event) {
        var value = event ? event.Id : "";
        this.AutomationSetSLAValue.SLAId = value;
    };
    AddEditAutomationsComponent.prototype.BuildAutomationResultEmailRecipient = function () {
        var _this = this;
        var resultEmailRecipientPMLists = [];
        this.ParticipantsList.forEach(function (userid) {
            if (!_this.AutomationResultEmailRecipientPMList.filter(function (d) { return d.RecipientValue == userid && (d.RecipientType == "Fixed"); })[0]) {
                var automationResultEmailRecipientPM = new AutomationResultEmailRecipientPM_1.AutomationResultEmailRecipientPM();
                automationResultEmailRecipientPM.RecipientType = "Fixed";
                automationResultEmailRecipientPM.RecipientValue = userid;
                automationResultEmailRecipientPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                automationResultEmailRecipientPM.AutomationsId = _this.CurrentEntityPM.Id;
                resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
            }
        });
        this.EntityContactVariable.forEach(function (Id) {
            if (!_this.AutomationResultEmailRecipientPMList.filter(function (d) { return d.RecipientValue == Id && (d.RecipientType == "Variable" || d.RecipientType == "Emails"); })[0]) {
                var automationResultEmailRecipientPM = new AutomationResultEmailRecipientPM_1.AutomationResultEmailRecipientPM();
                automationResultEmailRecipientPM.RecipientType = "Variable",
                    automationResultEmailRecipientPM.RecipientValue = Id,
                    automationResultEmailRecipientPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                automationResultEmailRecipientPM.AutomationsId = _this.CurrentEntityPM.Id;
                var objectFieldPM = _this.AutomationEmailRecipientFieldLists.filter(function (d) { return d.Id == Id; })[0];
                if (objectFieldPM != null) {
                    if (objectFieldPM.DataTypeCode == "Emails") {
                        automationResultEmailRecipientPM.RecipientType = "Emails";
                    }
                }
                if (!_this.AutomationResultEmailRecipientPMList.filter(function (d) { return d.RecipientType == automationResultEmailRecipientPM.RecipientType && d.RecipientValue == automationResultEmailRecipientPM.RecipientValue; })[0]) {
                    resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
                }
            }
        });
        this.AutomationResultEmailRecipientPMList.forEach(function (automationResultEmailRecipientPM) {
            if (automationResultEmailRecipientPM.RecipientType == "Fixed") {
                if (!_this.ParticipantsList.filter(function (d) { return d == automationResultEmailRecipientPM.RecipientValue; })[0]) {
                    resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
                }
            }
            else {
                if (_this.EntityContactVariable.indexOf(automationResultEmailRecipientPM.RecipientValue) == -1) {
                    resultEmailRecipientPMLists.push(automationResultEmailRecipientPM);
                }
            }
        });
        return resultEmailRecipientPMLists;
    };
    AddEditAutomationsComponent.prototype.SaveAutomationBackup = function () {
        var _this = this;
        var automationConditionList = [];
        var automationSetValuelist = [];
        this.AutomationCondationAndList.forEach(function (item) {
            item.CurrentEntityPM.UpdateDate = _this.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);
        });
        this.AutomationCondationOrList.forEach(function (item) {
            item.CurrentEntityPM.UpdateDate = _this.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);
        });
        if (this.CurrentEntityPM.ResultCode == "FIELDSET") {
            this.AutomationSetValueLists.forEach(function (item) {
                automationSetValuelist.push(item.CurrentEntityPM);
            });
        }
        var automatedBackup = new AutomatedBackup_1.AutomatedBackup();
        automatedBackup.Name = this.CurrentEntityPM.Name;
        automatedBackup.CreateDate = this.CurrentEntityPM.CreateDate;
        automatedBackup.Description = this.CurrentEntityPM.Description;
        automatedBackup.ResultCode = this.CurrentEntityPM.ResultCode;
        automatedBackup.Version = this.CurrentEntityPM.Version;
        automatedBackup.UpdateDate = this.CurrentEntityPM.UpdateDate;
        automatedBackup.Delaytime = this.DelayTime;
        automatedBackup.DelaytimeIndicator = this.DelaytimeIndicator;
        automatedBackup.Type = this.AutomatedBackupClass.Type;
        automatedBackup.DelayAautomationConditionLists = this.AutomatedBackupClass.DelayAautomationConditionLists;
        automatedBackup.AautomationConditionLists = automationConditionList;
        automatedBackup.IsAutomationResultEmailAllActiveUsers = this.IsAutomationResultEmailAllActiveUsers;
        automatedBackup.AutomationSetValueLists = this.CurrentEntityPM.ResultCode == "FIELDSET" ? automationSetValuelist : null;
        automatedBackup.AutomationSetSLAValue = this.CurrentEntityPM.ResultCode == "SETSLA" ? this.AutomationSetSLAValue : null;
        automatedBackup.AutomationQueuedTask = this.CurrentEntityPM.ResultCode == "QUEUE" ? this.AutomationQueuedTask : null;
        automatedBackup.AutomationFollowUp = this.IsFollowUp() ? this.AutomationFollowUp : null;
        this.CurrentEntityPM.AutomatedDataBackup = automatedBackup;
    };
    AddEditAutomationsComponent.prototype.CheckIfAutomationSetSLAValueChange = function () {
        var isChange = false;
        if (this.CurrentEntityPM.ResultCode == "SETSLA") {
            if (this.AutomatedBackupClass.AutomationSetSLAValue) {
                if (this.AutomatedBackupClass.AutomationSetSLAValue.SLAId != this.AutomationSetSLAValue.SLAId)
                    isChange = true;
            }
            else
                isChange = true;
        }
        return isChange;
    };
    AddEditAutomationsComponent.prototype.CheckIfAutomationFollowUpChange = function () {
        var isChange = false;
        if (this.AutomatedBackupClass.AutomationFollowUp) {
            if (this.AutomatedBackupClass.AutomationFollowUp.EventTypeId != this.AutomationFollowUp.EventTypeId)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.LegType != this.AutomationFollowUp.LegType)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.ObjectTableName != this.AutomationFollowUp.ObjectTableName)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.OwnerValue != this.AutomationFollowUp.OwnerValue)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.OwnerFieldType != this.AutomationFollowUp.OwnerFieldType)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.NoteValue != this.AutomationFollowUp.NoteValue)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DateValue != this.AutomationFollowUp.DateValue)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DocumentTypeLists != this.AutomationFollowUp.DocumentTypeLists)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DateEscalationTime != this.AutomationFollowUp.DateEscalationTime)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationFollowUp.DateEscalationActionTimeIndicatorCode != this.AutomationFollowUp.DateEscalationActionTimeIndicatorCode)
                isChange = true;
        }
        else
            isChange = true;
        return isChange;
    };
    AddEditAutomationsComponent.prototype.CheckIfAutomationQueuedTaskChange = function () {
        var isChange = false;
        if (this.AutomatedBackupClass.AutomationQueuedTask) {
            if (this.AutomatedBackupClass.AutomationQueuedTask.DateFieldValue != this.AutomationQueuedTask.DateFieldValue)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.OffsetTypeValue != this.AutomationQueuedTask.OffsetTypeValue)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskTimeUnitValue != this.AutomationQueuedTask.TaskTimeUnitValue)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.QueueId != this.AutomationQueuedTask.QueueId)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TeamId != this.AutomationQueuedTask.TeamId)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskOffset != this.AutomationQueuedTask.TaskOffset)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskOwnerId != this.AutomationQueuedTask.TaskOwnerId)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskCustomerId != this.AutomationQueuedTask.TaskCustomerId)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskSubject != this.AutomationQueuedTask.TaskSubject)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskPriorityId != this.AutomationQueuedTask.TaskPriorityId)
                isChange = true;
            if (this.AutomatedBackupClass.AutomationQueuedTask.TaskDescription != this.AutomationQueuedTask.TaskDescription)
                isChange = true;
        }
        else
            isChange = true;
        return isChange;
    };
    //Automation Follow Up    
    AddEditAutomationsComponent.prototype.FollowUpTypeComboBoxChanged = function (value) {
        if (value) {
            this.AutomationFollowUp.EventTypeId = value.Id;
        }
    };
    //Owner
    AddEditAutomationsComponent.prototype.FollowUpOwnerObjectFieldComboBoxChanged = function (item) {
        this.FollowUpOwnerId = "";
        if (item) {
            this.FollowOwnerObjectFieldId = item.Id;
            this.AutomationFollowUp.OwnerFieldType = item.FieldName == "Specific" ? "Specific" : "Field";
        }
        this.FollowUpOwnerObjectFieldSelected = item;
    };
    AddEditAutomationsComponent.prototype.FollowUpOwnerValueChange = function (item) {
        if (item) {
            this.FollowUpOwnerId = item.Id;
            this.FollowOwnerObjectFieldId = "";
            this.AutomationFollowUp.OwnerFieldType = "Specific";
        }
        else
            this.FollowUpOwnerId = "";
    };
    //Date
    AddEditAutomationsComponent.prototype.FollowUpDateObjectFieldComboBoxChanged = function (item) {
        if (item) {
            this.DateValue = item.Id;
        }
        else
            this.DateValue = "";
        this.FollowUpDateObjectFieldSelected = item;
    };
    AddEditAutomationsComponent.prototype.FollowDateEscalationActionTimeIndicatorCodeValueChange = function (event) {
        if (event.Code == "IM") {
            this.DataContext.UIProperties.SetEnabled("FollowDateEscalationTime", "Automation", false);
            this.FollowDateEscalationTime = 0;
        }
        else
            this.DataContext.UIProperties.SetEnabled("FollowDateEscalationTime", "Automation", true);
    };
    AddEditAutomationsComponent.prototype.SetQueuedTaskRequired = function () {
        var isQueueRequired = false;
        var isSubjectRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.QueueId)) {
            isQueueRequired = true;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.TaskSubject)) {
            isSubjectRequired = true;
        }
        this.IsQueueRequired = isQueueRequired;
        this.IsSubjectRequired = isSubjectRequired;
    };
    Object.defineProperty(AddEditAutomationsComponent.prototype, "QueueId", {
        get: function () { return this.queueId; },
        set: function (newValue) {
            if (this.queueId != newValue) {
                this.queueId = newValue;
                this.SetQueuedTaskRequired();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAutomationsComponent.prototype, "TaskSubject", {
        get: function () { return this.taskSubject; },
        set: function (newValue) {
            if (this.taskSubject != newValue) {
                this.taskSubject = newValue;
                this.SetQueuedTaskRequired();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAutomationsComponent.prototype.BuildQueuedTaskFilters = function () {
        var _this = this;
        this.DueDateFieldList = [];
        this.TaskOffsetTypeList = [];
        this.TaskTimeUnitList = [];
        var shipmentObjecttableId = window.ObjectTables.filter(function (f) { return f.Id == _this.ObjectTableId; })[0].Id;
        var taskObjectFields = window.ObjectFields.filter(function (f) { return f.ObjectTableId == shipmentObjecttableId && f.DataTypeCode == "DateTime"; });
        taskObjectFields.forEach(function (item) {
            if (item.FieldName == "MainCarriageETD" || item.FieldName == "MainCarriageATD" || item.FieldName == "MainCarriageFinalDestinationETA" || item.FieldName == "MainCarriageFinalDestinationATA"
                || item.FieldName == "OperationalDate" || item.FieldName == "OperationalCloseDate" || item.FieldName == "AccountingCloseDate" || item.FieldName == "FinalArrivalDate") {
                var dateItem = new CodeNameClass_1.CodeNameClass(item.FieldName, item.FullNameTextCodeDefaultText);
                _this.DueDateFieldList.push(dateItem);
            }
        });
        this.TaskOffsetTypeList.push(new CodeNameClass_1.CodeNameClass("B", "Before"));
        this.TaskOffsetTypeList.push(new CodeNameClass_1.CodeNameClass("A", "After"));
        this.TaskTimeUnitList.push(new CodeNameClass_1.CodeNameClass("D", "Days"));
        this.TaskTimeUnitList.push(new CodeNameClass_1.CodeNameClass("H", "Hours"));
        this.selectedDueDateField = new CodeNameClass_1.CodeNameClass();
        this.selectedTaskOffsetType = this.TaskOffsetTypeList.filter(function (d) { return d.Code == "B"; })[0];
        this.selectedTaskTimeUnit = this.TaskTimeUnitList.filter(function (d) { return d.Code == "D"; })[0];
    };
    Object.defineProperty(AddEditAutomationsComponent.prototype, "SelectedDueDateField", {
        get: function () { return this.selectedDueDateField; },
        set: function (value) {
            if (this.selectedDueDateField != value) {
                this.selectedDueDateField = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAutomationsComponent.prototype, "SelectedTaskOffsetType", {
        get: function () { return this.selectedTaskOffsetType; },
        set: function (value) {
            if (this.selectedTaskOffsetType != value) {
                this.selectedTaskOffsetType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAutomationsComponent.prototype, "SelectedTaskTimeUnit", {
        get: function () { return this.selectedTaskTimeUnit; },
        set: function (value) {
            if (this.selectedTaskTimeUnit != value) {
                this.selectedTaskTimeUnit = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AddEditAutomationsComponent.prototype, "AllLocations", void 0);
    AddEditAutomationsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddEditAutomationsComponent',
            templateUrl: './AddEditAutomationsComponent.html',
            providers: [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, AutomationResultEmailRecipientExtendedService_1.AutomationResultEmailRecipientExtendedService, AutomationExtendedPMService_1.AutomationExtendedPMService, AutomationHistoryExtendedPMService_1.AutomationHistoryExtendedPMService, EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [AutomationResultEmailRecipientExtendedService_1.AutomationResultEmailRecipientExtendedService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, AutomationExtendedPMService_1.AutomationExtendedPMService, AutomationHistoryExtendedPMService_1.AutomationHistoryExtendedPMService, core_1.ChangeDetectorRef, EntityArgs_1.EntityArgs])
    ], AddEditAutomationsComponent);
    return AddEditAutomationsComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditAutomationsComponent = AddEditAutomationsComponent;
var ResultCode = /** @class */ (function () {
    function ResultCode(name, code) {
        this.Code = code;
        this.Name = name;
    }
    return ResultCode;
}());
//# sourceMappingURL=AddEditAutomationsComponent.js.map