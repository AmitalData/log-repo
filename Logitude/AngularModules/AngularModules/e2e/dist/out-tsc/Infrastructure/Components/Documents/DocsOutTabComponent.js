"use strict";
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
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CommunicationLogExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/CommunicationLogExtendedPMService");
var EventTypeExtendedPMService_1 = require("../../../Infrastructure/Services/ExtendedPMs/EventTypeExtendedPMService");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentOutPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var GeneralDocumentFollowUpHelper_1 = require("../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper");
var DocumentTypeListService_1 = require("../../../Common/Services/StandardLists/DocumentTypeListService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var DocsOutDataViewModel_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel");
var CommunicationLogPMViewModel_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/CommunicationLogPMViewModel");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var CardPMService_1 = require("\"../../../Common/Services/StandardPMs/CardPMService");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var DocsOutTabComponent = /** @class */ (function () {
    function DocsOutTabComponent(entityArgs, _documentOutPMService, _communicationLogExtendedPMService, _eventTypeExtendedPMService, _documentTypeListService, _documentsFilingExtendedPMService, _elementRef) {
        this.entityArgs = entityArgs;
        this._documentOutPMService = _documentOutPMService;
        this._communicationLogExtendedPMService = _communicationLogExtendedPMService;
        this._eventTypeExtendedPMService = _eventTypeExtendedPMService;
        this._documentTypeListService = _documentTypeListService;
        this._documentsFilingExtendedPMService = _documentsFilingExtendedPMService;
        this._elementRef = _elementRef;
        this.ComponentName = "DocsOut";
        this.LoadFirstObjectCompleted = new core_1.EventEmitter();
        this.ObjectTableId = "";
        this.EntityId = "";
        this.TransportModeId = "";
        this.ShipmentlevelCode = "";
        this.ChildrenObjectTableIds = "";
        this.ChildEntityReference = "";
        this.ChildObjectTableId = "";
        this.ChildEntityId = "";
        this.EntityReference = "";
        this.ObjectTableName = "";
        this.DownloadAllVisibile = false;
        this.HasDocuments = false;
        this.CustomFilterOperation = "";
        this.CustomFilterValue = "";
        this.IsCustomFilter = false;
        this.IsShowFollowColum = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.IsShowAddFromLibraryLink = false;
        this.InitializeDocsOutForAnotherObjectTable = new core_1.EventEmitter();
        this.isPrintRequested = false;
        this.isSendRequested = false;
        this.IsLoadDocumentOutListsComplete = false;
        this.IsLoadDocumenTypeLists = false;
        this.IsLoadCommunicationLogsListsComplete = false;
        this.IsLoadFollowUpDocumentTypeListsComplete = false;
        this.IsOpenSendComponent = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TabSelectedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.DocOutChangedEvent = null;
        //Send
        this.IsSendClose = false;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.DocumentTypes = [];
        this.Listen();
    }
    DocsOutTabComponent.prototype.DownloadAllClick = function () {
        var _this = this;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Shipment", "Docs Out Downloaded");
        var service = new CardPMService_1.CardPMService();
        service.get(SessionLocator_1.SessionLocator.LoggedUserPM.Id).subscribe(function (res) {
            if (!res.HasError) {
                var link = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "/WebPages/SharedDownloadPage.aspx?id=" + SessionLocator_1.SessionLocator.Tenant + ":" + null + ":ship:" + _this.EntityId + ":O:" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
                var win = window.open(link, '_blank');
                win.focus();
            }
        });
    };
    DocsOutTabComponent.prototype.CheckHasDocuments = function () {
        var _this = this;
        this.HasDocuments = false;
        if (this.StaticDocumentsList) {
            this.StaticDocumentsList.forEach(function (item) {
                if (item.HasFile) {
                    _this.HasDocuments = true;
                }
            });
        }
    };
    DocsOutTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.TabHeaderTextCode = "DocsOut.O.DocsOut"; // this.ObjectTableName + ".TH.DocsOut";
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        if (table) {
            this.ObjectTableName = table.Name;
        }
        else
            this.ObjectTableName = "Shipment";
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "DOCSOUTDOWNLOADDOCUMENTS") && this.ObjectTableName == "Shipment") {
            this.DownloadAllVisibile = true;
        }
        // Ayman:
        // we need this for Translation
        // Please don't remove it
        this.TabHeaderTextCode = this.ObjectTableName + ".TH.DocsOut";
        if (this.InitializeDocsOutForAnotherObjectTable) {
            this.InitializeDocsOutForAnotherObjectTable.subscribe(function ($event) {
                _this.InitializeDocsOutForAnotherObjectTableMethod($event);
            });
        }
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("DocsOut").subscribe(function (response) {
                _this.IsStardLoadPage = true;
                _this.Load();
            });
        });
    };
    DocsOutTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.DocOutChangedEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    DocsOutTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHDO") {
                    var followUpDocumenttypeIds = [];
                    if (_this.EntityPM.FollowUps) {
                        _this.EntityPM.FollowUps.forEach(function (item) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                                if (followUpDocumenttypeIds.indexOf(item.DocumentTypeId) == -1) {
                                    followUpDocumenttypeIds.push(item.DocumentTypeId);
                                }
                            }
                        });
                        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                        apiQueryFilters.GetAll = true;
                        apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                        _this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
                            var pmResponse = res;
                            if (!pmResponse.HasError) {
                                _this.AllDocumentTypeList = pmResponse.Result;
                                _this.FollowUpDocumentTypesLists = _this.AllDocumentTypeList.filter(function (d) { return followUpDocumenttypeIds.indexOf(d.Id) > -1 && d.IsDocOut; });
                                _this.StaticDocumentsList.forEach(function (doc) {
                                    doc.HasFollowUp = false;
                                });
                                _this.SetFollowUpList();
                            }
                        });
                    }
                }
            });
        }
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        //this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        if (_this.isPrintRequested) {
                            _this.InitializePrinting();
                        }
                        if (_this.isSendRequested) {
                            _this.InitializeSending("NONE");
                        }
                    }
                    _this.isPrintRequested = false;
                    _this.isSendRequested = false;
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
        if (!this.DocOutChangedEvent) {
            this.DocOutChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "RefreshDocumentOutPrint") {
                    _this.InitializeDocsOutControl();
                }
                else if (s == "RefreshDocumentOutSend") {
                    _this.LoadDocumentsFilingsWithDocuments(true);
                }
            });
        }
    };
    DocsOutTabComponent.prototype.InitializeDocsOutControl = function () {
        this.IsLoadCommunicationLogsListsComplete = false;
        this.IsLoadDocumentOutListsComplete = false;
        this.IsLoadDocumenTypeLists = false;
        this.IsLoadFollowUpDocumentTypeListsComplete = false;
        this.DocumentOuts = [];
        this.DocumentInPMs = [];
        this.DocumentTypes = [];
        this.FollowUpDocumentTypesLists = [];
        this.CommunicationLogs = [];
        this.ItemsSource = new Array();
        this.StaticDocumentsList = new Array();
        this.SelectedInternalDocument = null;
        this.LoadDocumentOutLists();
        this.LoadCommunicationLogs();
        this.LoadAllDocumentTypeList();
        this.LoadFollowUpDocumentTypeLists();
    };
    DocsOutTabComponent.prototype.InitializeDocsOutForAnotherObjectTableMethod = function (tableId) {
        var _this = this;
        this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, tableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var documentids = "";
            var documentOuts = [];
            var pmResponse = res;
            if (!pmResponse.HasError) {
                documentOuts = pmResponse.Result;
                if (documentOuts) {
                    documentOuts.forEach(function (item) {
                        var docType = _this.DocumentTypes.filter(function (d) { return d.Id == item.DocumentTypeId; })[0];
                        if (docType) {
                            if (!_this.StaticDocumentsList) {
                                _this.StaticDocumentsList = new Array();
                            }
                            // var exists = this.StaticDocumentsList.filter(d => d.Id == item.DocumentTypeId)[0];
                            // if (!exists) {
                            var exists = new DocsOutDataViewModel_1.DocsOutDataViewModel(null, _this.EntityId, item.ChildEntityId, _this.ObjectTableId, _this.ChildObjectTableId, item.ChildEntityReference, documentOuts, _this.CommunicationLogs, _this, _this.EntityPM, "", docType);
                            _this.StaticDocumentsList.push(exists);
                            var isfollow = false;
                            if (_this.EntityPM.FollowUps) {
                                var followUp = _this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == docType.Id && d.Area == "DocOut" && !d.Done; })[0];
                                if (followUp) {
                                    isfollow = true;
                                    exists.HasFollowUp = true;
                                }
                            }
                            // }
                        }
                        else {
                            documentids += (item.DocumentTypeId + ";");
                        }
                    });
                    //if (!AppTool.IsNullOrEmpty(documentids)) {
                    //    this._documentTypePMService.getDocumentTypesByIds(documentids, SessionInfo.LoggedUserTenant).subscribe(res => {
                    //        var pmResponse: ServiceResponse = res;
                    //        if (!pmResponse.HasError) {
                    //            var myResult = pmResponse.Result;
                    //            if (myResult) {
                    //                myResult.forEach((docType) => {
                    //                    var docOut: any = documentOuts.filter(d => d.DocumentTypeId == docType.Id)[0];
                    //                    if (docOut) {
                    //                        var exists = this.StaticDocumentsList.filter(d => d.Id == docOut.DocumentTypeId)[0];
                    //                        if (!exists) {
                    //                            exists = new DocsOutDataViewModel(docType, this.EntityId, docOut.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, docOut.ChildEntityReference, documentOuts, this.CommunicationLogs, this, this.EntityPM);
                    //                            this.StaticDocumentsList.push(exists);
                    //                            var isfollow: boolean = false;
                    //                            if (this.EntityPM.FollowUps) {
                    //                                var followUp: any = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == docType.Id && d.Area == "DocOut" && !d.Done)[0];
                    //                                if (followUp) {
                    //                                    isfollow = true;
                    //                                    exists.HasFollowUp = true;
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    if (this.DocumentTypes.indexOf(docType) == -1) {
                    //                        this.DocumentTypes.push(docType);
                    //                    }
                    //                });
                    //            }
                    //        }
                    //    });
                    //}
                }
            }
            _this.DocumentInPMs = new Array();
            _this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(_this.EntityId, _this.ChildEntityId, tableId, "I", SessionInfo_1.SessionInfo.LoggedUserTenant, false).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    _this.DocumentInPMs.concat(myResult);
                }
            });
        });
    };
    //Load
    DocsOutTabComponent.prototype.Load = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "FROMLIBRARY")) {
            this.IsShowAddFromLibraryLink = true;
        }
        else {
            this.IsShowAddFromLibraryLink = false;
        }
        if (this.EntityPM && this.EntityPM.FollowUps) {
            switch (this.ObjectTableName.toLowerCase()) {
                case "shipment":
                case "master":
                    {
                        this.IsShowFollowColum = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups");
                        break;
                    }
                case "quote":
                    {
                        this.IsShowFollowColum = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups");
                        break;
                    }
            }
        }
        this.InitializeDocsOutControl();
    };
    DocsOutTabComponent.prototype.LoadDocumentsFilingsWithDocuments = function (isLoadOnlay) {
        var _this = this;
        if (isLoadOnlay === void 0) { isLoadOnlay = false; }
        this.DocumentInPMs = new Array();
        this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(this.EntityId, this.ChildEntityId, this.ObjectTableId, "I", SessionInfo_1.SessionInfo.LoggedUserTenant, true).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.DocumentInPMs = myResult;
                if (!isLoadOnlay) {
                    _this.LoadFirstObjectCompleted.emit("Ready");
                }
                _this.CheckHasDocuments();
            }
        });
    };
    DocsOutTabComponent.prototype.LoadAllDocumentTypeList = function () {
        var _this = this;
        this.IsLoadDocumenTypeLists = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomFilterValue)) {
            this.CustomFilterValue = this.CustomFilterValue.toUpperCase();
        }
        this.AllDocumentTypeList = [];
        this.DocumentTypes = [];
        this.DocumentOuts = [];
        if (this.StaticDocumentsList == null) {
            this.StaticDocumentsList = new Array();
        }
        var objecttableid = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId) && this.ChildObjectTableId)
            objecttableid = this.ChildObjectTableId;
        else
            objecttableid = this.ObjectTableId;
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.AllDocumentTypeList = pmResponse.Result;
            }
            var childrenIds = !Tools_1.AppTool.IsNullOrEmpty(_this.ChildrenObjectTableIds) ? _this.ChildrenObjectTableIds.split(',') : [];
            _this.DocumentTypes = _this.AllDocumentTypeList.filter(function (a) { return a.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant && (a.ObjectTableId == objecttableid || childrenIds.indexOf(a.ObjectTableId) != -1) && a.IsDocOut; });
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.TransportModeId)) {
                switch (_this.TransportModeId) {
                    case "A":
                        {
                            _this.DocumentTypes = _this.DocumentTypes.filter(function (a) { return a.IsAir == true; });
                            break;
                        }
                    case "O":
                        {
                            _this.DocumentTypes = _this.DocumentTypes.filter(function (a) { return a.IsOcean == true; });
                            break;
                        }
                    case "I":
                        {
                            _this.DocumentTypes = _this.DocumentTypes.filter(function (a) { return a.IsInland == true; });
                            break;
                        }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.ShipmentlevelCode)) {
                switch (_this.ShipmentlevelCode) {
                    case "C":
                        {
                            _this.DocumentTypes = _this.DocumentTypes.filter(function (a) { return a.IsMaster == true; });
                            break;
                        }
                    case "M":
                        {
                            _this.DocumentTypes = _this.DocumentTypes.filter(function (a) { return a.IsMaster == true; });
                            break;
                        }
                    case "D":
                        {
                            _this.DocumentTypes = _this.DocumentTypes.filter(function (a) { return a.IsDirect == true; });
                            break;
                        }
                    case "H":
                        {
                            _this.DocumentTypes = _this.DocumentTypes.filter(function (a) { return a.IsHouse == true; });
                            break;
                        }
                }
                _this.CheckHasDocuments();
            }
            _this.IsLoadDocumenTypeLists = true;
            _this.LoadComplete();
        });
    };
    DocsOutTabComponent.prototype.RefreshButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.Load();
    };
    DocsOutTabComponent.prototype.LoadDocumentOutLists = function () {
        var _this = this;
        this.IsLoadDocumentOutListsComplete = false;
        var getDocsOut = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId) && Tools_1.AppTool.IsNullOrEmpty(this.ChildEntityId)) {
            getDocsOut = false;
        }
        if (getDocsOut) {
            this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, this.ObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.DocumentOuts = myResult;
                    }
                }
                _this.IsLoadDocumentOutListsComplete = true;
                _this.LoadComplete();
            });
        }
        else {
            this.IsLoadDocumentOutListsComplete = true;
            this.LoadComplete();
        }
    };
    DocsOutTabComponent.prototype.LoadComplete = function () {
        var _this = this;
        if (this.IsLoadCommunicationLogsListsComplete && this.IsLoadDocumentOutListsComplete && this.IsLoadDocumenTypeLists && this.IsLoadFollowUpDocumentTypeListsComplete) {
            if (this.DocumentTypes) {
                if (this.IsCustomFilter) {
                    if (this.CustomFilterOperation) {
                        if (this.CustomFilterOperation == "Equal") {
                            this.DocumentTypes = this.DocumentTypes.filter(function (d) { return d.Code == _this.CustomFilterValue; });
                        }
                        else if (this.CustomFilterOperation == "NotEqual") {
                            this.DocumentTypes = this.DocumentTypes.filter(function (d) { return d.Code != _this.CustomFilterValue; });
                        }
                    }
                }
            }
            this.BuildItemsSource();
            this.LoadDocumentsFilingsWithDocuments();
        }
    };
    DocsOutTabComponent.prototype.LoadFollowUpDocumentTypeLists = function () {
        var _this = this;
        this.IsLoadFollowUpDocumentTypeListsComplete = false;
        var followUpDocumenttypeIds = [];
        if (this.EntityPM && this.EntityPM.FollowUps) {
            this.EntityPM.FollowUps.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    if (followUpDocumenttypeIds.indexOf(item.DocumentTypeId) == -1) {
                        followUpDocumenttypeIds.push(item.DocumentTypeId);
                    }
                }
            });
            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    _this.AllDocumentTypeList = pmResponse.Result;
                    _this.FollowUpDocumentTypesLists = _this.AllDocumentTypeList.filter(function (d) { return followUpDocumenttypeIds.indexOf(d.Id) > -1 && d.IsDocOut; });
                }
                _this.IsLoadFollowUpDocumentTypeListsComplete = true;
                _this.LoadComplete();
            });
        }
        else {
            this.IsLoadFollowUpDocumentTypeListsComplete = true;
            this.LoadComplete();
        }
    };
    DocsOutTabComponent.prototype.LoadCommunicationLogs = function () {
        var _this = this;
        this.IsLoadCommunicationLogsListsComplete = false;
        this.CommunicationLogs = new Array();
        this._communicationLogExtendedPMService.getCommunicationLogPMsByEntityId(this.EntityId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach(function (item) {
                        _this.CommunicationLogs.push(new CommunicationLogPMViewModel_1.CommunicationLogPMViewModel(item));
                    });
                }
            }
            _this.IsLoadCommunicationLogsListsComplete = true;
            _this.LoadComplete();
        });
    };
    DocsOutTabComponent.prototype.onSearchTextChangeEvent = function (search) {
        if (search) {
            if (search != "Search" && this.StaticDocumentsList) {
                this.ItemsSource = this.StaticDocumentsList.filter(function (d) { return d.Name && d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1; });
                this.SortItemSource();
            }
        }
        else
            this.ItemsSource = this.StaticDocumentsList;
    };
    DocsOutTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var documentidsList = [];
        if (this.DocumentOuts && this.DocumentTypes) {
            this.DocumentOuts.forEach(function (item) {
                var docType = _this.DocumentTypes.filter(function (d) { return d.Id == item.DocumentTypeId; })[0];
                if (docType) {
                    var exists = _this.StaticDocumentsList.filter(function (d) { return d.Id == item.Id; })[0] != null ? true : false;
                    if (!exists) {
                        _this.StaticDocumentsList.push(new DocsOutDataViewModel_1.DocsOutDataViewModel(null, _this.EntityId, item.ChildEntityId, _this.ObjectTableId, _this.ChildObjectTableId, item.ChildEntityReference, _this.DocumentOuts, _this.CommunicationLogs, _this, _this.EntityPM, "", docType));
                    }
                }
                else {
                    documentidsList.push(item.DocumentTypeId);
                }
            });
            this.CheckHasDocuments();
        }
        this.AllDocumentTypeList.filter(function (d) { return documentidsList.indexOf(d.Id) > -1; }).forEach(function (item) {
            if (_this.DocumentTypes.indexOf(item) == -1) {
                _this.DocumentTypes.push(item);
            }
        });
        this.FillDocumentTypes();
        this.SortItemSource();
        //this._documentTypePMService.getDocumentTypesByIds(documentids, SessionInfo.LoggedUserTenant).subscribe(res => {
        //    var pmResponse: ServiceResponse = res;
        //    if (!pmResponse.HasError) {
        //        var myResult = pmResponse.Result;
        //        if (myResult) {
        //            myResult.forEach((item) => {
        //                if (this.DocumentTypes.indexOf(item) == -1) {
        //                    this.DocumentTypes.push(item);
        //                }
        //            });
        //        }
        //    }
        //    this.FillDocumentTypes();
        //});
    };
    DocsOutTabComponent.prototype.SortItemSource = function () {
        this.ItemsSource = this.ItemsSource.sort();
        this.ItemsSource.sort(function (a, b) {
            if (a.Name.toLowerCase() < b.Name.toLowerCase()) {
                return -1;
            }
            else if (a.Name.toLowerCase() > b.Name.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
    };
    DocsOutTabComponent.prototype.FillDocumentTypes = function () {
        var _this = this;
        if (this.StaticDocumentsList == null) {
            this.StaticDocumentsList = new Array();
        }
        if (this.DocumentTypes) {
            this.DocumentTypes.filter(function (d) { return d.ObjectTableId == _this.ObjectTableId || d.ObjectTableId == _this.ChildObjectTableId; }).forEach(function (item) {
                var exists = _this.StaticDocumentsList.filter(function (d) { return d.Id == item.Id; })[0] != null ? true : false;
                if (!exists) {
                    if (item != null) {
                        if (_this.CheckIfHasDocumentOut(item) || item.InActive == false) {
                            var docVeiwModel = new DocsOutDataViewModel_1.DocsOutDataViewModel(null, _this.EntityId, _this.ChildEntityId, _this.ObjectTableId, _this.ChildObjectTableId, _this.ChildEntityReference, _this.DocumentOuts, _this.CommunicationLogs, _this, _this.EntityPM, "", item);
                            if (!docVeiwModel.DocumentTypeList.IsReadOnly || (docVeiwModel.DocumentTypeList.IsReadOnly && docVeiwModel.Exists)) {
                                _this.StaticDocumentsList.push(docVeiwModel);
                            }
                        }
                    }
                }
            });
        }
        this.SetFollowUpList();
        this.CurrentSession.StopBusyIndicator();
    };
    DocsOutTabComponent.prototype.SetFollowUpList = function () {
        var _this = this;
        if (this.EntityPM && this.EntityPM.FollowUps && this.EntityPM.FollowUps.length > 0 && this.FollowUpDocumentTypesLists) {
            this.EntityPM.FollowUps.filter(function (d) { return d.Area == "DocOut" && !d.Done && !Tools_1.AppTool.IsNullOrEmpty(d.DocumentTypeId); }).forEach(function (follow) {
                var docType = _this.FollowUpDocumentTypesLists.filter(function (d) { return d.Id == follow.DocumentTypeId && d.IsDocOut; })[0];
                if (docType) {
                    var exists = _this.StaticDocumentsList.filter(function (d) { return d.Id == docType.Id; })[0];
                    if (!exists) {
                        var docVeiwModel = new DocsOutDataViewModel_1.DocsOutDataViewModel(null, _this.EntityId, _this.ChildEntityId, _this.ObjectTableId, _this.ChildObjectTableId, _this.ChildEntityReference, _this.DocumentOuts, _this.CommunicationLogs, _this, _this.EntityPM, "", docType);
                        docVeiwModel.HasFollowUp = true;
                        _this.StaticDocumentsList.push(docVeiwModel);
                    }
                    else {
                        exists.HasFollowUp = true;
                    }
                }
            });
        }
        if (this.ObjectTableName == "Quote") {
            if (!this.EntityPM.IsQuoteDataExternal || !this.EntityPM.IsQuoteDocumentExternal) {
                this.StaticDocumentsList = this.StaticDocumentsList.filter(function (d) { return d.DocumentTypeCode != "QUOTE"; });
            }
        }
        this.ItemsSource = new Array();
        this.ItemsSource = this.StaticDocumentsList;
        this.ItemsSource.sort(function (a, b) {
            if (a.Name.toLowerCase() < b.Name.toLowerCase()) {
                return -1;
            }
            else if (a.Name.toLowerCase() > b.Name.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
        if (this.ItemsSource && this.ItemsSource.length > 0) {
            if (this.SelectedInternalDocument) {
                this.SelectedInternalDocument = this.ItemsSource.filter(function (d) { return d.DocumentTypeId == _this.SelectedInternalDocument.DocumentTypeId; })[0];
            }
            if (!this.SelectedInternalDocument) {
                this.SelectedInternalDocument = this.ItemsSource[0];
            }
            if (this.SelectedInternalDocument.DocumentTypeList.TemplateFormatCode == "M") {
                this.SelectedInternalDocument.VisibleSendButton = true;
            }
            else {
                this.SelectedInternalDocument.VisiblePrintButton = true;
            }
        }
    };
    DocsOutTabComponent.prototype.CheckIfHasDocumentOut = function (docType) {
        var _this = this;
        var hasdoc = false;
        if (docType != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ChildEntityId)) {
                hasdoc = this.DocumentOuts.filter(function (d) { return d.DocumentTypeId == docType.Id && d.ChildEntityId == _this.ChildEntityId; })[0] ? true : false;
            }
            else {
                hasdoc = this.DocumentOuts.filter(function (d) { return d.DocumentTypeId == docType.Id; })[0] ? true : false;
            }
        }
        return hasdoc;
    };
    DocsOutTabComponent.prototype.OnSelectedDocumentOutList = function (item) {
        this.SelectedInternalDocument = item;
        this.ItemsSource.forEach(function (item) {
            item.VisiblePrintButton = false;
            item.VisibleSendButton = false;
        });
        if (item.DocumentTypeList.TemplateFormatCode == "M") {
            item.VisibleSendButton = true;
        }
        else {
            item.VisiblePrintButton = true;
        }
    };
    DocsOutTabComponent.prototype.CreateInternalDocument = function (m) {
        var _this = this;
        this._documentOutPMService.getCreateDocumentOut(this.SelectedInternalDocument.DocumentTypeId, this.EntityId, this.ChildEntityId, this.SelectedInternalDocument.ChildReference, this.ObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.SelectedInternalDocument.CurrentDocument = myResult;
                    _this.ShowSendControl(m);
                }
            }
        });
    };
    //MouseEvent
    DocsOutTabComponent.prototype.OnmMouseOver = function (item) {
        this.ItemsSource.forEach(function (item) {
            item.VisiblePrintButton = false;
            item.VisibleSendButton = false;
        });
        if (item.DocumentTypeList.TemplateFormatCode == "M") {
            item.VisibleSendButton = true;
        }
        else {
            item.VisiblePrintButton = true;
        }
        if (this.SelectedInternalDocument.DocumentTypeList.TemplateFormatCode == "M") {
            this.SelectedInternalDocument.VisibleSendButton = true;
        }
        else {
            this.SelectedInternalDocument.VisiblePrintButton = true;
        }
    };
    DocsOutTabComponent.prototype.OnmMouseleave = function (item) {
        this.ItemsSource.forEach(function (item) {
            item.VisiblePrintButton = false;
            item.VisibleSendButton = false;
        });
        if (this.SelectedInternalDocument.DocumentTypeList.TemplateFormatCode == "M") {
            this.SelectedInternalDocument.VisibleSendButton = true;
        }
        else {
            this.SelectedInternalDocument.VisiblePrintButton = true;
        }
    };
    //FollowUp
    DocsOutTabComponent.prototype.OnFollowMouseOver = function (item) {
        this.ItemsSource.forEach(function (item) {
            item.ShowFollowUp = false;
        });
        item.ShowFollowUp = true;
    };
    DocsOutTabComponent.prototype.OnFollowMouseleave = function (item) {
        this.ItemsSource.forEach(function (item) {
            item.ShowFollowUp = false;
        });
    };
    DocsOutTabComponent.prototype.AddFollowUp = function (item) {
        var generalFollowUpHelper = new GeneralDocumentFollowUpHelper_1.GeneralDocumentFollowUpHelper(this.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildEntityReference, "DocOut", item, this.EntityPM);
        generalFollowUpHelper.AddFollowUp();
    };
    DocsOutTabComponent.prototype.SendButtonClick = function (item) {
        if (!this.IsOpenSendComponent) {
            this.IsOpenSendComponent = true;
            this.SelectedInternalDocument = item;
            if (this.EntityPM && this.EntityPM.IsDirty) {
                this.isSendRequested = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else {
                this.InitializeSending("NONE");
            }
        }
    };
    DocsOutTabComponent.prototype.InitializeSending = function (m) {
        if (m == "NONE") {
            if (this.SelectedInternalDocument.CurrentDocument != null) {
                if (this.SelectedInternalDocument.CurrentDocument.TemplateType == "P") {
                    if (this.SelectedInternalDocument.CurrentDocument.DocumentOutCopies.length != 0) {
                        m = this.SelectedInternalDocument.CurrentDocument.DocumentOutCopies[0].Id;
                    }
                }
                this.ShowSendControl(m);
            }
            else {
                this.CreateInternalDocument(m);
            }
        }
    };
    DocsOutTabComponent.prototype.ShowSendControl = function (documentOutCopyId, title) {
        var _this = this;
        if (title === void 0) { title = "Send Message"; }
        this.IsOpenSendComponent = false;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.SelectedInternalDocument.DocumentTypeName + " Sending");
        this.IsSendClose = false;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 1000)
            sendWindowWidth = 1000;
        if (sendWindowHeight < 600)
            sendWindowHeight = 600;
        if (this.SelectedInternalDocument != null) {
            if (this.SelectedInternalDocument.CurrentDocument != null)
                this.DocumentOutCopyId = documentOutCopyId;
            else
                this.SelectedInternalDocument.Exists = true;
        }
        this.SelectedInternalDocument.EntityId = this.EntityId;
        this.SelectedInternalDocument.documentOutCopyId = this.DocumentOutCopyId;
        this.SelectedInternalDocument.DocsOutItemsList = this.ItemsSource;
        this.SelectedInternalDocument.IsSend = true;
        this.SelectedInternalDocument.PageRequestSendComponent = "DocOut";
        this.SelectedInternalDocument.ModeSendDocument = title == "Document Editor" ? "preview" : "Edit";
        this.SelectedInternalDocument.EventRefreshName = "CommunicationRefresh";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = this.SelectedInternalDocument.WindowWidth = sendWindowWidth;
        logWindow.Height = this.SelectedInternalDocument.WindowHeight = sendWindowHeight;
        logWindow.Title = title;
        logWindow.DataContext = this.SelectedInternalDocument;
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (_this.SelectedInternalDocument.ModeSendDocument == "Edit") {
                if ($event == "SendEnd") {
                    if (!_this.IsSendClose) {
                        _this.IsSendClose = true;
                        _this.LoadDocumentsFilingsWithDocuments(true);
                    }
                }
            }
        });
    };
    //Print
    DocsOutTabComponent.prototype.PrintButtonClick = function (item) {
        this.SelectedInternalDocument = item;
        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.isPrintRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.InitializePrinting();
        }
    };
    ;
    DocsOutTabComponent.prototype.InitializePrinting = function () {
        var _this = this;
        var buildingDocumentText = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.BuildingDocument");
        if (Tools_1.AppTool.IsNullOrEmpty(buildingDocumentText))
            buildingDocumentText = "Building document...";
        if (this.SelectedInternalDocument != null) {
            if (this.SelectedInternalDocument.CurrentDocument != null) {
                this.ShowPrintControl();
            }
            else {
                this.CurrentSession.StartBusyIndicator(buildingDocumentText);
                this._documentOutPMService.getCreateDocumentOut(this.SelectedInternalDocument.DocumentTypeId, this.EntityId, this.ChildEntityId, this.ChildEntityReference, this.ObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var documentout = pmResponse.Result;
                        if (documentout) {
                            _this.SelectedInternalDocument.CurrentDocument = documentout;
                            _this.SelectedInternalDocument.Exists = true;
                            if (documentout.IssuedByUserName && documentout.IssuedDate && documentout.Issued) {
                                _this.SelectedInternalDocument.HasFile = true;
                                _this.SelectedInternalDocument.IssuedDate = documentout.IssuedDate;
                                _this.SelectedInternalDocument.IssuedByUserName = documentout.IssuedByUserName;
                            }
                            _this.CurrentSession.StopBusyIndicator();
                            _this.ShowPrintControl();
                        }
                    }
                });
            }
        }
    };
    DocsOutTabComponent.prototype.ShowPrintControl = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 760;
        logWindow.Height = 552;
        this.SelectedInternalDocument.EntityId = this.EntityId;
        this.SelectedInternalDocument.DocsOutItemsList = this.ItemsSource;
        this.SelectedInternalDocument.PageRequestSendComponent = "DocOut";
        this.SelectedInternalDocument.EventRefreshName = "CommunicationRefresh";
        logWindow.DataContext = this.SelectedInternalDocument;
        logWindow.Title = "Print " + this.SelectedInternalDocument.DocumentTypeList.Name;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.CheckHasDocuments();
            if (_this.CurrentSession.CurrentEditComponent) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
    };
    DocsOutTabComponent.prototype.ViewCommunicationLog = function (docsOutDataViewModel, communicationLog) {
        this.SelectedInternalDocument = docsOutDataViewModel;
        this.SelectedInternalDocument.SelectedCommunicationLogViewMode = communicationLog;
        this.ShowSendControl("", "Document Editor");
    };
    DocsOutTabComponent.prototype.LinkAddDocumentFromLibraryClcik = function () {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.EntityId = this.EntityId;
        windowArgs.TransportModeId = this.TransportModeId;
        windowArgs.ShipmentlevelCode = this.ShipmentlevelCode;
        windowArgs.ChildEntityId = this.ChildObjectTableId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.PageRequest = "DocOut";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 550;
        logWindow.Title = "New Documents";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeFromLibraryComponent");
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DocsOutTabComponent.prototype, "LoadFirstObjectCompleted", void 0);
    DocsOutTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: "DocsOutControl",
            templateUrl: './DocsOutTabComponent.html',
            inputs: ['EntityPM', 'EntityId', 'ObjectTableId', 'TransportModeId', 'ShipmentlevelCode', 'ChildEntityReference', 'ChildObjectTableId', 'ChildEntityId', 'EntityReference', 'ChildrenObjectTableIds', 'InitializeDocsOutForAnotherObjectTable', 'IsCustomFilter', 'CustomFilterValue', 'CustomFilterOperation'],
            providers: [DocumentOutPMService_1.DocumentOutPMService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService, CommunicationLogExtendedPMService_1.CommunicationLogExtendedPMService, EventTypeExtendedPMService_1.EventTypeExtendedPMService, DocumentTypeListService_1.DocumentTypeListService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs,
            DocumentOutPMService_1.DocumentOutPMService,
            CommunicationLogExtendedPMService_1.CommunicationLogExtendedPMService,
            EventTypeExtendedPMService_1.EventTypeExtendedPMService,
            DocumentTypeListService_1.DocumentTypeListService,
            DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService,
            core_1.ElementRef])
    ], DocsOutTabComponent);
    return DocsOutTabComponent;
}());
exports.DocsOutTabComponent = DocsOutTabComponent;
//# sourceMappingURL=DocsOutTabComponent.js.map