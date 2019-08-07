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
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CustomsDocumentPM_1 = require("../../../Customs/EntityPMs/CustomsDocumentPM");
var CustomsDocumentsTicketPM_1 = require("../../../Customs/EntityPMs/CustomsDocumentsTicketPM");
var CustomsDocumentTicketViewModel_1 = require("./CustomsDocumentTicketViewModel");
var RelatedDocumentViewModel_1 = require("./RelatedDocumentViewModel");
var CustDocsTicketWebService_1 = require("../../../Customs/Services/WebServices/CustDocsTicketWebService");
var CustDocMetaDataValuesWebService_1 = require("../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService");
var CustomsDocumentsDataProvider_1 = require("./CustomsDocumentsDataProvider");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ImageLibraryService_1 = require("../../../Common/Services/Others/ImageLibraryService");
var CustDocRelatedDocsWebService_1 = require("../../../Customs/Services/WebServices/CustDocRelatedDocsWebService");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var CustomsDocumentPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsDocumentPMService");
var CommunicationLogStepListService_1 = require("../../../Common/Services/ExtendedLists/CommunicationLogStepListService");
var CustomsRequestMenuService_1 = require("../../../Customs/Services/Others/CustomsRequestMenuService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
///import { setTimeout } from 'timers';
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var CustomsDocumentsComponent = /** @class */ (function (_super) {
    __extends(CustomsDocumentsComponent, _super);
    function CustomsDocumentsComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        //****************Inputs****************//
        _this.ChildEntityId1 = null;
        _this.ChildEntityId2 = null;
        _this.ChildEntityId3 = null;
        _this.ParentEntityCode = null;
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.BuildHeader = false;
        _this.DisplayOnlyMessage = "";
        _this.IsRelatedDocsVisible = true;
        _this.IsWindowMode = false;
        _this.DontLoadTickets = false;
        _this.PreventEdit = false;
        _this.SelectedDocumentId = null;
        //*************************************//
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DocumentFilterSelectedValue = "customs";
        _this.IsRefreshButtonDisabled = false;
        _this.RefreshDocsScreen = false;
        _this.ListOfStatusCode2Show = ["1", "2"];
        if (entityArgs.EntityPM && !entityArgs.SkipCtor) {
            _this.Start(entityArgs.EntityPM, entityArgs.ObjectTableName);
        }
        return _this;
    }
    Object.defineProperty(CustomsDocumentsComponent.prototype, "CustomDocumentTypeCode", {
        get: function () { return this.customDocumentTypeCode; },
        set: function (value) {
            if (this.customDocumentTypeCode != value) {
                this.customDocumentTypeCode = value;
                this.FilterCustomsDocumentsTickets();
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsDocumentsComponent.prototype.ngOnDestroy = function () {
        console.log("CustomsDocumentsComponent:ngOnDestroy");
        this.entityArgs = null;
        if (this.CustomsDocumentsTicketViewModels == null)
            return;
        this.CustomsDocumentsTicketViewModels.forEach(function (item) { item.DataContext = null; });
        this.CustomsDocumentsTicketViewModels = null;
    };
    CustomsDocumentsComponent.prototype.Start = function (entityPM, objectTableName) {
        var _this = this;
        this.EntityPM = entityPM;
        this.ObjectTableName = objectTableName;
        this.ParentEntityCode = this.ObjectTableName.split('.')[1];
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsDocument").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsDocumentsTicket").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsDocumentPointer").subscribe(function (response) {
                        _this.InsureCustomsDocumentsController();
                        //if (AppTool.IsNullOrEmpty(this.customsDocumentsDataProvider)) {
                        //    this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(this.ObjectTableName, this.EntityPM);
                        //}
                        //if (AppTool.IsNullOrEmpty(this.iCustomsDocumentsController)) {
                        //    this.iCustomsDocumentsController = this.customsDocumentsDataProvider.GetCustomsDocumentsController();
                        //}
                        _this.IsRelatedDocsVisible = _this.iCustomsDocumentsController.IsRelatedDocumentsVisible();
                        _this.DisplayOnlyCheck();
                        _this.InitiateComponent();
                        _this.Listen();
                        _this.BuildHeader = true;
                        _this.FilterSelectedValue = 'alltickets';
                        _this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
                        _this.custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
                    });
                });
            });
        });
    };
    CustomsDocumentsComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.DisplayOnlyCheck();
                    _this.InsureCustomsDocumentsController();
                    //this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(this.ObjectTableName, this.EntityPM);
                    //this.iCustomsDocumentsController = this.customsDocumentsDataProvider.GetCustomsDocumentsController();
                    if (_this.RefreshDocsScreen) {
                        _this.RefreshButtonClicked(_this.SelectedDocumentId);
                        _this.RefreshDocsScreen = false;
                    }
                    //if (!this.DontLoadTickets) {
                    //    this.InitiateComponent();
                    //    this.FilterSelectedValue = 'alltickets';
                    //}
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCCD") {
                        _this.DisplayOnlyCheck();
                        _this.RefreshButtonClicked(_this.SelectedDocumentId);
                    }
                }
            }));
        }
    };
    CustomsDocumentsComponent.prototype.InsureCustomsDocumentsController = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsDataProvider)) {
            this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider_1.CustomsDocumentsDataProvider(this.ObjectTableName, this.EntityPM);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.iCustomsDocumentsController)) {
            this.iCustomsDocumentsController = this.customsDocumentsDataProvider.GetCustomsDocumentsController();
        }
    };
    CustomsDocumentsComponent.prototype.InitiateComponent = function (selectedDocId) {
        var _this = this;
        if (selectedDocId === void 0) { selectedDocId = null; }
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CustomsDocumentsTickets = [];
        this.MetadataValues = [];
        this.CustomsDocumentsTicketViewModels = [];
        this.StaticCustomsDocumentsTicketViewModels = [];
        var custDocsTicketWebService = new CustDocsTicketWebService_1.CustDocsTicketWebService();
        var custDocsMetadataWebService = new CustDocMetaDataValuesWebService_1.CustDocMetaDataValuesWebService();
        //******Getting customs documents ticket for the entity*****//
        // this.DisplayOnlyCheck();
        custDocsTicketWebService.GetCustomsDocumentsTicketsByEntityIdAndChilds(this.EntityPM.Id, null, null, null, this.ParentEntityCode).subscribe(function (response) {
            _this.CustomsDocumentsTickets = response.Result;
            var customsDocTickets = "";
            var tickets = _this.CustomsDocumentsTickets.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.DocumentTypeCode); });
            var codeValues = "";
            _this.DocTypesFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
            for (var j = 0; j < tickets.length; j++) {
                codeValues = codeValues + tickets[j].DocumentTypeCode + ',';
            }
            if (codeValues != null) {
                codeValues = codeValues.substr(0, codeValues.length - 1);
            }
            _this.DocTypesFilterItems.addAdditionalFilter("Code", codeValues, null, null, "InListExact", false, false, false, "string", false, true);
            for (var i = 0; i < _this.CustomsDocumentsTickets.length; i++) {
                customsDocTickets = customsDocTickets + "," + _this.CustomsDocumentsTickets[i].DocumentsFilingId;
            }
            customsDocTickets = customsDocTickets.substr(1, customsDocTickets.length - 1);
            custDocsMetadataWebService.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocTickets).subscribe(function (response2) {
                _this.MetadataValues = response2.Result;
                _this.GetRelatedDocuments();
                _this.GetAutoGeneratedTickets(selectedDocId);
                _this.CurrentSession.StopBusyIndicator();
            });
        });
    };
    CustomsDocumentsComponent.prototype.FillCustomsDocumentsTickets = function (tickets, selectedDocId) {
        if (selectedDocId === void 0) { selectedDocId = null; }
        if (this.CustomsDocumentsTicketViewModels == null) {
            this.CustomsDocumentsTicketViewModels = [];
        }
        for (var i = 0; i < tickets.length; i++) {
            var customsDocumentsTicketViewModel = new CustomsDocumentTicketViewModel_1.CustomsDocumentTicketViewModel(tickets[i], this.MetadataValues, false, this.IsDisplayOnly, this.EntityPM, this.ObjectTableName, this.iCustomsDocumentsController);
            customsDocumentsTicketViewModel.DataContext = this;
            this.CustomsDocumentsTicketViewModels.push(customsDocumentsTicketViewModel);
            this.StaticCustomsDocumentsTicketViewModels.push(customsDocumentsTicketViewModel);
        }
        console.log(this.CustomsDocumentsTicketViewModels);
        this.SetFilterCounts();
        if (selectedDocId) {
            var ticket = this.CustomsDocumentsTicketViewModels.filter(function (d) { return d.Id == selectedDocId; })[0];
            this.EditCustomsDocumentsTicket(ticket);
            this.SelectedDocumentId = null;
        }
    };
    CustomsDocumentsComponent.prototype.GetRelatedDocuments = function () {
        var _this = this;
        if (this.iCustomsDocumentsController.IsRelatedDocumentsVisible()) {
            this.RelatedDocuments = [];
            this.customsDocumentsDataProvider.GetCustomsDocumentsRelatedDocuments(this.DocumentFilterSelectedValue).subscribe(function (response) {
                var relatedDocs;
                relatedDocs = response.Result;
                for (var i = 0; i < relatedDocs.length; i++) {
                    var ticket = _this.CustomsDocumentsTickets.filter(function (d) { return d.DocumentsFilingId == relatedDocs[i].Id; })[0];
                    var values = _this.MetadataValues.filter(function (d) { return d.CustomsDocumentId == relatedDocs[i].Id; });
                    if (!ticket) {
                        var relatedDocViewModel = new RelatedDocumentViewModel_1.RelatedDocumentViewModel(relatedDocs[i], values, _this.IsDisplayOnly);
                        _this.RelatedDocuments.push(relatedDocViewModel);
                    }
                }
            });
        }
    };
    CustomsDocumentsComponent.prototype.FilterItemClicked = function (key) {
        this.FilterSelectedValue = key;
        this.FilterCustomsDocumentsTickets();
    };
    CustomsDocumentsComponent.prototype.DocumentFilterItemClicked = function (value) {
        this.DocumentFilterSelectedValue = value;
        this.GetRelatedDocuments();
    };
    CustomsDocumentsComponent.prototype.RefreshButtonClicked = function (selectedDocId) {
        var _this = this;
        if (selectedDocId === void 0) { selectedDocId = null; }
        this.IsRefreshButtonDisabled = true;
        var t = setTimeout(function () { _this.IsRefreshButtonDisabled = false; clearTimeout(t); }, 1000);
        this.InitiateComponent(selectedDocId);
    };
    CustomsDocumentsComponent.prototype.FilterCustomsDocumentsTickets = function () {
        var _this = this;
        this.CustomsDocumentsTicketViewModels = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomDocumentTypeCode)) {
            this.CustomsDocumentsTicketViewModels = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return d.DocumentTypeCode == _this.CustomDocumentTypeCode; });
        }
        else {
            this.CustomsDocumentsTicketViewModels = this.StaticCustomsDocumentsTicketViewModels;
        }
        this.SetFilterCounts();
        switch (this.FilterSelectedValue) {
            case 'alltickets': {
                break;
            }
            case 'notuploaded': {
                this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.filter(function (d) { return Tools_1.AppTool.IsNullOrEmpty(d.CustomsDocId); });
                break;
            }
            case 'uploaded': {
                this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.CustomsDocId); });
                break;
            }
            case 'requireddocs': {
                this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.RequestedDocumentId); });
                break;
            }
        }
        this.SortCustomsDocumentTickets();
    };
    CustomsDocumentsComponent.prototype.SetFilterCounts = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomDocumentTypeCode)) {
            this.AllTicketsCount = this.StaticCustomsDocumentsTicketViewModels.length + "";
            this.NotUploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return Tools_1.AppTool.IsNullOrEmpty(d.CustomsDocId); }).length + "";
            this.UploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.CustomsDocId); }).length + "";
            this.RequiredDocsCount = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.RequestedDocumentId); }).length + "";
        }
        else {
            this.AllTicketsCount = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return d.DocumentTypeCode == _this.CustomDocumentTypeCode; }).length + "";
            this.NotUploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return d.DocumentTypeCode == _this.CustomDocumentTypeCode && Tools_1.AppTool.IsNullOrEmpty(d.CustomsDocId); }).length + "";
            this.UploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return d.DocumentTypeCode == _this.CustomDocumentTypeCode && !Tools_1.AppTool.IsNullOrEmpty(d.CustomsDocId); }).length + "";
            this.RequiredDocsCount = this.StaticCustomsDocumentsTicketViewModels.filter(function (d) { return d.DocumentTypeCode == _this.CustomDocumentTypeCode && !Tools_1.AppTool.IsNullOrEmpty(d.RequestedDocumentId); }).length + "";
        }
    };
    CustomsDocumentsComponent.prototype.GetAutoGeneratedTickets = function (selectedDocId) {
        var _this = this;
        this.iCustomsDocumentsController.GetAutoGeneratedTickets(this.CustomsDocumentsTicketViewModels)
            .subscribe(function (resp) {
            var sub = _this.iCustomsDocumentsController
                .GetCustomsInterfaceSettingsDocumentTypesCompleted
                .subscribe(function (response) {
                var tickets = response.Result; // this is the full result of tickets.
                sub.unsubscribe();
                if (!Tools_1.AppTool.IsNullOrEmpty(tickets)) {
                    tickets.forEach(function (autoGeneratedTicketVM) {
                        var pm = _this.CustomsDocumentsTickets.filter(function (r) { return r.DocumentTypeCode == autoGeneratedTicketVM.DocumentTypeCode; })[0];
                        //  let task39629SuppressUnique: boolean = true;///CALL#309883 לא נפתח טיקט לכל ח-ן ספק +CALL#309868;310765, 311721
                        //  if (task39629SuppressUnique  || AppTool.IsNullOrEmpty(vm)) {
                        //      ticket.DataContext = this;
                        //      ticket.isDisplayOnly = this.IsDisplayOnly;
                        //      this.CustomsDocumentsTicketViewModels.push(ticket);
                        //      this.StaticCustomsDocumentsTicketViewModels.push(ticket);
                        //}
                        var toAdd = true;
                        ;
                        if ( /*autoGeneratedTicketVM.DocumentTypeCode == "380" &&*/autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers.length == 1) {
                            _this.CustomsDocumentsTickets
                                .filter(function (r) { return r.DocumentTypeCode == autoGeneratedTicketVM.DocumentTypeCode; })
                                .forEach(function (ticket1) {
                                var pointer = ticket1.CustomsDocumentPointers
                                    .filter(function (currPointer) {
                                    return currPointer.ParentEntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].ParentEntityId &&
                                        currPointer.ParentEntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].ParentEntityCode &&
                                        currPointer.Child1EntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child1EntityCode &&
                                        currPointer.Child2EntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child2EntityCode &&
                                        currPointer.Child3EntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child3EntityCode &&
                                        currPointer.Child1EntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child1EntityId &&
                                        currPointer.Child2EntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child2EntityId &&
                                        currPointer.Child3EntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child3EntityId;
                                })[0];
                                if (!Tools_1.AppTool.IsNullOrEmpty(pointer)) {
                                    toAdd = false;
                                }
                            });
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(pm) && autoGeneratedTicketVM.FromCompanyDocumentType2Add) { // already Exist DocumentTypeCode and from   GetCustomsInterfaceSettingsDocumentTypesCompleted
                            console.log("Task 42286: CALL#315874 טיקטים כפולים לסוגי מסמך");
                            toAdd = false;
                        }
                        if (toAdd) {
                            autoGeneratedTicketVM.DataContext = _this;
                            autoGeneratedTicketVM.isDisplayOnly = _this.IsDisplayOnly;
                            _this.CustomsDocumentsTicketViewModels.push(autoGeneratedTicketVM);
                            _this.StaticCustomsDocumentsTicketViewModels.push(autoGeneratedTicketVM);
                            //this.CustomsDocumentsTickets.push(ticket);
                        }
                    });
                }
                _this.FillCustomsDocumentsTickets(_this.CustomsDocumentsTickets, selectedDocId);
                _this.iCustomsDocumentsController.FillDefaultMetaData(_this.CustomsDocumentsTicketViewModels);
                _this.iCustomsDocumentsController.FillDefaultMetaData(_this.StaticCustomsDocumentsTicketViewModels);
                _this.SortCustomsDocumentTickets();
                _this.SetFilterCounts();
            });
            _this.iCustomsDocumentsController.GetCustomsInterfaceSettingsDocumentTypes(_this.EntityPM);
        });
    };
    CustomsDocumentsComponent.prototype.DownloadDocumentFile = function (documentsFilingId) {
        var _this = this;
        this.custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe(function (resp) {
            var documentFiling = resp.Result;
            _this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.Extension, documentFiling.Folder, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var documentName = documentFiling.DocumentId;
                var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
                var uri = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/Downloadpage.aspx?id=" + documentName + "&tempId=" + token;
                //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
                //    return;
                //}
                DownloadManager_1.DownloadManager.DownloadPage(documentName);
            });
        });
    };
    CustomsDocumentsComponent.prototype.SortCustomsDocumentTickets = function () {
        this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.sort(function (a, b) {
            return (a.DocumentTypeCode === b.DocumentTypeCode) ? 0 : (a.DocumentTypeCode < b.DocumentTypeCode) ? -1 : 1;
        });
    };
    CustomsDocumentsComponent.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        this.iCustomsDocumentsController.DisplayOnlyCheck().subscribe(function (resp) {
            _this.IsDisplayOnly = resp.Result.IsDisplayOnly;
            _this.DisplayOnlyMessage = resp.Result.DisplayOnlyMessage;
        });
    };
    CustomsDocumentsComponent.prototype.AddCustomsDocumentsTicket = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var windowArgs = {};
        windowArgs.CustomsDocumentsTicket = new CustomsDocumentsTicketPM_1.CustomsDocumentsTicketPM();
        windowArgs.CustomsDocumentsTicket.Tenant = SessionLocator_1.SessionLocator.Tenant;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        windowArgs.IsNewState = true;
        var entityInfo = this.iCustomsDocumentsController.GetParentAndChildrenEntityCodesAndIds();
        windowArgs.ParentEntityId = entityInfo.ParentEntityId;
        windowArgs.ParentEntityCode = entityInfo.ParentEntityCode;
        windowArgs.Child1EntityId = entityInfo.Child1EntityId;
        windowArgs.Child1EntityCode = entityInfo.Child1EntityCode;
        windowArgs.iCustomsDocumentsController = this.iCustomsDocumentsController;
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AddCustomsDocument");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnAddEditWindowClosed($event); });
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/AddEditCustomsDocumentComponent');
        this.CurrentSession.StopBusyIndicator();
    };
    CustomsDocumentsComponent.prototype.OnAddEditWindowClosed = function (event) {
        if (event == 'ok') {
            this.RefreshDocsScreen = true;
            //  this.RefreshButtonClicked();
            this.RefreshEntity();
        }
    };
    CustomsDocumentsComponent.prototype.RefreshEntity = function () {
        var refreshFrom = this.iCustomsDocumentsController.GetRefreshFrom();
        if (refreshFrom == "e") {
            if (this.CurrentSession.CurrentEditComponent) {
                this.RefreshDocsScreen = true;
                this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
            else {
                this.RefreshButtonClicked(this.SelectedDocumentId);
            }
        }
        else if (refreshFrom == "d") {
            this.RefreshButtonClicked(this.SelectedDocumentId);
        }
    };
    CustomsDocumentsComponent.prototype.EditCustomsDocumentsTicket = function (customsDocumentsTicket) {
        var _this = this;
        if (!customsDocumentsTicket.PreventEdit) {
            if (customsDocumentsTicket.DocumentsFilingId) {
                this.CurrentSession.StartBusyIndicatorLoading();
                var documentsFilingId = encodeURIComponent(customsDocumentsTicket.DocumentsFilingId);
                this.iCustomsDocumentsController.CheckRequestsInProgress(documentsFilingId).subscribe(function (response) {
                    var isThereRequests = response.Result.IsDisplayOnly;
                    var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
                    customsDocumentPMService.get(documentsFilingId).subscribe(function (resp) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!resp.HasError) {
                            var customsDoc = resp.Result;
                            _this.ApplyEditCustomsDocumentTicket(isThereRequests, customsDocumentsTicket.customsDocumentsTicketPM, customsDoc);
                        }
                    });
                });
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.ApplyEditCustomsDocumentTicket(false, customsDocumentsTicket.customsDocumentsTicketPM, null);
            }
        }
    };
    CustomsDocumentsComponent.prototype.ApplyEditCustomsDocumentTicket = function (isThereRequests, customsDocumentsTicket, customsDocument) {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent) {
            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        }
        var windowArgs = {};
        if (customsDocumentsTicket) {
            windowArgs.CustomsDocumentsTicket = customsDocumentsTicket;
            windowArgs.CustomsDocumentsTicket.Tenant = SessionLocator_1.SessionLocator.Tenant;
            if (!customsDocumentsTicket.Id) {
                windowArgs.IsNewState = true;
            }
            else {
                windowArgs.IsNewState = false;
            }
        }
        windowArgs.CustomsDocument = customsDocument;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly && isThereRequests;
        windowArgs.IsEntityDisplayOnly = this.IsDisplayOnly;
        windowArgs.IsCustomsDocumentInRequest = isThereRequests;
        var entityInfo = this.iCustomsDocumentsController.GetParentAndChildrenEntityCodesAndIds();
        windowArgs.iCustomsDocumentsController = this.iCustomsDocumentsController;
        windowArgs.ParentEntityId = entityInfo.ParentEntityId;
        windowArgs.ParentEntityCode = entityInfo.ParentEntityCode;
        windowArgs.Child1EntityId = entityInfo.Child1EntityId;
        windowArgs.Child1EntityCode = entityInfo.Child1EntityCode;
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditDocumentMetaData");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnAddEditWindowClosed($event); });
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/AddEditCustomsDocumentComponent');
    };
    CustomsDocumentsComponent.prototype.IsCustomDocumentItemEnabled = function (documentStatusCode) {
        var MyEditControlViewModelController = this.CurrentSession.CurrentEditComponent.EditComponentController;
        if (MyEditControlViewModelController.InDisplayMode
            || documentStatusCode == "8"
            || this.IsUnifaceLock()) {
            return false;
        }
        return true;
    };
    CustomsDocumentsComponent.prototype.IsUnifaceLock = function () {
    };
    CustomsDocumentsComponent.prototype.EditRelatedDocument = function (relatedDocumentViewModel) {
        var _this = this;
        if (!this.PreventEdit) {
            this.CurrentSession.StartBusyIndicatorLoading();
            var documentsFilingId = relatedDocumentViewModel.Id;
            documentsFilingId = encodeURIComponent(documentsFilingId);
            var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
            customsDocumentPMService.get(documentsFilingId).subscribe(function (resp) {
                if (!resp.HasError) {
                    relatedDocumentViewModel.CustomDocument = resp.Result;
                    if (relatedDocumentViewModel.CustomDocument) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.ApplyEditCustomsDocumentTicket(false, null, relatedDocumentViewModel.CustomDocument);
                    }
                    else {
                        //DocumentsFilingId = CurrentDocument.Id, Tenant = TenantContext.Current.Id, DeclarationId = CurrentDocument.EntityId,
                        //    ExternalEntityName = CurrentDocument.ExternalEntityName,
                        //    ExternalEntityReference = CurrentDocument.ExternalEntityReference,
                        //    DocumentId = CurrentDocument.DocumentId,
                        //    Extension = CurrentDocument.FileExtension,
                        //    CurrentEntityId = CurrentDocument.EntityId,
                        //    FileSize = CurrentDocument.FileSize,
                        var customsDocumentPM = new CustomsDocumentPM_1.CustomsDocumentPM();
                        customsDocumentPM.DocumentsFilingId = relatedDocumentViewModel.Id;
                        customsDocumentPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        customsDocumentPM.DeclarationId = _this.EntityPM.Id;
                        customsDocumentPM.CurrentEntityId = _this.EntityPM.Id;
                        customsDocumentPM.Extension = relatedDocumentViewModel.Extension;
                        customsDocumentPM.FileSize = relatedDocumentViewModel.FileSize;
                        customsDocumentPM.DocumentId = relatedDocumentViewModel.documentsFilingPM.DocumentId;
                        customsDocumentPM.ExternalEntityName = relatedDocumentViewModel.documentsFilingPM.ExternalEntityName;
                        customsDocumentPM.ExternalEntityReference = relatedDocumentViewModel.documentsFilingPM.ExternalEntityReference;
                        relatedDocumentViewModel.CustomDocument = customsDocumentPM;
                        var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
                        customsDocumentPMService.insert(customsDocumentPM).subscribe(function (resp) {
                            if (!resp.HasError) {
                                _this.CurrentSession.StopBusyIndicator();
                                _this.ApplyEditCustomsDocumentTicket(false, null, relatedDocumentViewModel.CustomDocument);
                            }
                            else {
                                _this.CurrentSession.StopBusyIndicator();
                                if (_this.CurrentSession.CurrentEditComponent) {
                                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = resp.ErrorsArray;
                                }
                                else {
                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                    messageWindow.Width = 400;
                                    messageWindow.Height = 200;
                                    messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                                    if (resp.ErrorsArray && resp.ErrorsArray.length > 0) {
                                        messageWindow.Show(resp.ErrorsArray[0]);
                                    }
                                    else {
                                        messageWindow.Show("Server Error");
                                    }
                                    messageWindow.WindowClosed.subscribe(function (event) {
                                        messageWindow.Close();
                                    });
                                }
                            }
                            //this.CurrentSession.StartBusyIndicatorSaving();
                            //                                this.CurrentSession.StopBusyIndicator();
                        });
                    }
                }
            });
        }
    };
    CustomsDocumentsComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.IsWindowMode = true;
        this.Start(windowArgs.EntityPM, windowArgs.ObjectTableName);
    };
    CustomsDocumentsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsDocumentsComponent.prototype.PreventEditRelatedDoc = function () {
        this.PreventEdit = true;
    };
    CustomsDocumentsComponent.prototype.AllowEditRelatedDoc = function () {
        this.PreventEdit = false;
    };
    CustomsDocumentsComponent.prototype.ShowCustomAnswerClicked = function (event, customsDocumentsTicket) {
        var _this = this;
        event.stopPropagation();
        //console.log(customsDocumentsTicket);
        //if (AppTool.IsNullOrEmpty(customsDocumentsTicket.customsDocumentsTicketPM.DocumentStatusCode) ||
        //    AppTool.IsNullOrEmpty(customsDocumentsTicket.customsDocumentsTicketPM.DocumentsFilingId) ||
        //    //entityPm.DocumentStatusCode!= "1" 
        //    this.ListOfStatusCode2Show.indexOf(customsDocumentsTicket.customsDocumentsTicketPM.DocumentStatusCode)==-1
        if (customsDocumentsTicket.ApprovedImageVisibility || customsDocumentsTicket.DeniedImageVisibility) {
        }
        else {
            if (!customsDocumentsTicket.HaveCustomAnswer) {
                this.EditCustomsDocumentsTicket(customsDocumentsTicket);
                return;
            }
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        var objecttable = window.ObjectTables.filter(function (d) { return d.Name == "Customs.CustomsDocument"; })[0];
        var myCommunicationLogStepListService = new CommunicationLogStepListService_1.CommunicationLogStepListService();
        //logId=1-212245&tenant=1
        myCommunicationLogStepListService
            .GetRequestComminicationIdByEntityId2(SessionLocator_1.SessionLocator.Tenant, "2715", "30", objecttable.Id, customsDocumentsTicket.customsDocumentsTicketPM.DocumentsFilingId)
            .subscribe(function (rsp) {
            var myCustomsRequestsSheet = rsp.Result;
            _this.CurrentSession.StopBusyIndicator();
            if (myCustomsRequestsSheet) {
                var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
                customsRequestMenuService.ShowModalByIdAndIntreface(myCustomsRequestsSheet.RequestComminicationId, "2715", "קלוט צרופה");
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Width = 400;
                messageWindow.Height = 200;
                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("שליחת המסמך למכס נכשל ( פירוט נוסף בגיליון הבקשות )");
            }
        });
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], CustomsDocumentsComponent.prototype, "ChildEntityId1", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], CustomsDocumentsComponent.prototype, "ChildEntityId2", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], CustomsDocumentsComponent.prototype, "ChildEntityId3", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], CustomsDocumentsComponent.prototype, "ParentEntityCode", void 0);
    CustomsDocumentsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsDocumentsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], CustomsDocumentsComponent);
    return CustomsDocumentsComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsDocumentsComponent = CustomsDocumentsComponent;
var RelatedEntityParams = /** @class */ (function () {
    function RelatedEntityParams() {
    }
    return RelatedEntityParams;
}());
exports.RelatedEntityParams = RelatedEntityParams;
//# sourceMappingURL=CustomsDocumentsComponent.js.map