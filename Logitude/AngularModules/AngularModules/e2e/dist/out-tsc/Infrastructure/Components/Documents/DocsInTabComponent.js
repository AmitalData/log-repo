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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var DocumentTypeListExtendedService_1 = require("../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var Tools_1 = require("../../../Infrastructure/Tools");
var DocumentTypeListService_1 = require("../../../Common/Services/StandardLists/DocumentTypeListService");
var DocumentsFilingPMService_1 = require("../../../Common/Services/StandardPMs/DocumentsFilingPMService");
var ImageLibraryService_1 = require("../../../Common/Services/Others/ImageLibraryService");
var ServiceArgs_1 = require("../../../Infrastructure/DataContracts/ServiceArgs");
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var DocsInDataViewModel_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsIn/ViewModel/DocsInDataViewModel");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var GeneralDocumentFollowUpHelper_1 = require("../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var BaseComponent_1 = require("../LogitudeComponents/BaseComponent");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var CardPMService_1 = require("\"../../../Common/Services/StandardPMs/CardPMService");
var ServiceLocator_1 = require("../../Locators/ServiceLocator");
var DocsInTabComponent = /** @class */ (function (_super) {
    __extends(DocsInTabComponent, _super);
    function DocsInTabComponent(_documentTypeListService, _imageLibraryService, entityArgs, _documentTypeListExtendedService, _documentsFilingExtendedPMService) {
        var _this = _super.call(this) || this;
        _this._documentTypeListService = _documentTypeListService;
        _this._imageLibraryService = _imageLibraryService;
        _this.entityArgs = entityArgs;
        _this._documentTypeListExtendedService = _documentTypeListExtendedService;
        _this._documentsFilingExtendedPMService = _documentsFilingExtendedPMService;
        _this.DataContext = _this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.EntityId = "";
        _this.ChildEntityId = "";
        _this.ObjectTableId = "";
        _this.ChildObjectTableId = "";
        _this.TransportModeId = "";
        _this.ShipmentlevelCode = "";
        _this.ChildEntityReference = "";
        _this.IsClickToUpload = false;
        _this.DownloadAllVisibile = false;
        _this.HasDocuments = false;
        _this.additional = null;
        _this.DeleteAttachmentButtonEnable = false;
        _this.AllDocumentTypeList = [];
        _this.IsLoadDocumentsFilingListsComplete = false;
        _this.IsLoadDocumentTypeListsComplete = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.LoadCompletedEvent = null;
        _this.TabSelectedEvent = null;
        _this.RefreshDocInEvent = null;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.TabHeaderTextCode = "DocsIn.O.DocsIn"; // entityArgs.ObjectTableName + ".TH.DocsIn";
        if (_this.documentsFilingPMService == null) {
            _this.documentsFilingPMService = new DocumentsFilingPMService_1.DocumentsFilingPMService();
        }
        _this.Listen();
        _this.CurrentSession.StartBusyIndicatorLoading();
        return _this;
    }
    DocsInTabComponent.prototype.DownloadAllClick = function () {
        var _this = this;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Shipment", "Docs In Downloaded");
        var service = new CardPMService_1.CardPMService();
        service.get(SessionLocator_1.SessionLocator.LoggedUserPM.Id).subscribe(function (res) {
            if (!res.HasError) {
                var link = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "/WebPages/SharedDownloadPage.aspx?id=" + SessionLocator_1.SessionLocator.Tenant + ":" + null + ":ship:" + _this.EntityId + ":" + res.Result.PartnerTypeId + ":" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
                var win = window.open(link, '_blank');
                win.focus();
            }
        });
    };
    DocsInTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        if (table) {
            this.ObjectTableName = table.Name;
        }
        else
            this.ObjectTableName = "Shipment";
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "DOCSINDOWNLOADDOCUMENTS") && this.ObjectTableName == "Shipment") {
            this.DownloadAllVisibile = true;
        }
        // Ayman:
        // we need this for Translation
        // Please don't remove it
        this.TabHeaderTextCode = this.ObjectTableName + ".TH.DocsIn";
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("DocsIn").subscribe(function (response) {
                _this.IsStardLoadPage = true;
                _this.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                if (_this.EntityPM && _this.EntityPM.FollowUps) {
                    switch (_this.ObjectTableName.toLowerCase()) {
                        case "shipment":
                        case "master":
                            {
                                _this.IsShowFollowColum = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups");
                                break;
                            }
                        case "quote":
                            {
                                _this.IsShowFollowColum = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups");
                                break;
                            }
                    }
                }
                _this.LoadData();
            });
        });
    };
    DocsInTabComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.RefreshDocInEvent) {
            this.RefreshDocInEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "RefreshDocIn") {
                    _this.RefreshButtonClicked();
                }
            });
        }
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHDI") {
                    _this.additional = null;
                    _this.LoadAllDocumentTypeList();
                }
            });
        }
    };
    DocsInTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        //AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    DocsInTabComponent.prototype.onSearchTextChangeEvent = function (search) {
        if (search) {
            if (search != "Search" && this.StaticDocumentsList) {
                this.DocumentsList = this.StaticDocumentsList.filter(function (d) { return d.Name && d.Name && d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1; });
                this.DocumentsList = this.SortItemSource(this.DocumentsList);
            }
        }
        else
            this.DocumentsList = this.StaticDocumentsList;
        this.BuildItemsSource();
    };
    DocsInTabComponent.prototype.LoadData = function () {
        this.IsLoadDocumentsFilingListsComplete = false;
        this.IsLoadDocumentTypeListsComplete = false;
        this.LoadAllDocumentTypeList();
        this.LoadDocumentsFilingPM(null);
    };
    DocsInTabComponent.prototype.RefreshButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.LoadData();
    };
    //LoadDocumentTypeLists() {
    //    var objecttableid: string = "";
    //    if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) && this.ChildObjectTableId) objecttableid = this.ChildObjectTableId;
    //    else objecttableid = this.ObjectTableId;
    //    this._documentTypeListExtendedService.getDocumentTypeListsByEnityIdAndTenant(this.TransportModeId, this.ShipmentlevelCode, objecttableid, this.Tenant).subscribe(res => {
    //        var pmResponse: ServiceResponse = res;
    //        if (!pmResponse.HasError) {
    //            var myResult = pmResponse.Result;
    //            if (myResult) {
    //                this.DocumentTypes = myResult;
    //            }
    //        }
    //        this.IsLoadDocumentTypeListsComplete = true;
    //        this.LoadComplete();
    //    });
    //}
    DocsInTabComponent.prototype.LoadAllDocumentTypeList = function () {
        var _this = this;
        this.DocumentTypes = [];
        this.AllDocumentTypeList = [];
        this.IsLoadDocumentTypeListsComplete = false;
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = this.Tenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.AllDocumentTypeList = pmResponse.Result;
            }
            _this.FillDocumentTypes();
            _this.IsLoadDocumentTypeListsComplete = true;
            _this.LoadComplete();
            _this.CheckHasDocuments();
        });
    };
    DocsInTabComponent.prototype.LoadDocumentsFilingPM = function (theAdditional) {
        var _this = this;
        this.IsLoadDocumentsFilingListsComplete = false;
        this.StaticDocumentsList = [];
        this.additional = theAdditional;
        var getDocsIn = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId) && Tools_1.AppTool.IsNullOrEmpty(this.ChildEntityId)) {
            getDocsIn = false;
        }
        if (getDocsIn) {
            this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(this.EntityId, this.ChildEntityId, this.ObjectTableId, "I", SessionInfo_1.SessionInfo.LoggedUserTenant, false).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.externalDocs = myResult;
                    }
                }
                _this.IsLoadDocumentsFilingListsComplete = true;
                _this.LoadComplete();
            });
        }
        else {
            this.IsLoadDocumentsFilingListsComplete = true;
            this.LoadComplete();
        }
    };
    DocsInTabComponent.prototype.FillDocInList = function () {
        var _this = this;
        if (!this.StaticDocumentsList) {
            this.StaticDocumentsList = [];
        }
        if (this.DocumentTypes != null) {
            this.DocumentTypes.forEach(function (docType) {
                var exists = _this.StaticDocumentsList.filter(function (d) { return d.Id == docType.Id; })[0];
                if (!exists) {
                    var docVeiwModel = new DocsInDataViewModel_1.DocsInDataViewModel(null, _this, docType, _this.EntityId, _this.ChildEntityId, _this.ChildEntityReference, _this.externalDocs, _this.ObjectTableId);
                    _this.StaticDocumentsList.push(docVeiwModel);
                }
            });
        }
        if (this.EntityPM && this.EntityPM.FollowUps && this.EntityPM.FollowUps.length > 0 && this.AllDocumentTypeList) {
            this.EntityPM.FollowUps.filter(function (d) { return d.Area == "DocIn" && !d.Done && !Tools_1.AppTool.IsNullOrEmpty(d.DocumentTypeId); }).forEach(function (follow) {
                var docType = _this.AllDocumentTypeList.filter(function (d) { return d.Id == follow.DocumentTypeId && d.IsDocIn; })[0];
                if (docType) {
                    var exists = null;
                    if (!Tools_1.AppTool.IsNullOrEmpty(follow.ExternalDocumentId)) {
                        exists = _this.StaticDocumentsList.filter(function (d) { return d.Id == docType.Id && d.CurrentDocument && follow.ExternalDocumentId == d.CurrentDocument.Id; })[0];
                    }
                    if (!exists) {
                        exists = _this.StaticDocumentsList.filter(function (d) { return d.Id == docType.Id; })[0];
                    }
                    if (!exists) {
                        var docVeiwModel = new DocsInDataViewModel_1.DocsInDataViewModel(null, _this, docType, _this.EntityId, _this.ChildEntityId, _this.ChildEntityReference, _this.externalDocs, _this.ObjectTableId);
                        docVeiwModel.HasFollowUp = true;
                        _this.StaticDocumentsList.push(docVeiwModel);
                    }
                    else {
                        if (!Tools_1.AppTool.IsNullOrEmpty(follow.ExternalDocumentId)) {
                            exists.HasFollowUp = true;
                        }
                        else {
                            var doc = _this.StaticDocumentsList.filter(function (d) { return d.DocumentTypeId == exists.DocumentTypeId && d.CurrentDocument != null; }).sort(function (a, b) { return (b.CurrentDocument.CreateDate > a.CurrentDocument.CreateDate) ? -1 : ((a.CurrentDocument.CreateDate < b.CurrentDocument.CreateDate) ? 1 : 0); })[0];
                            if (doc) {
                                doc.HasFollowUp = true;
                            }
                            else
                                exists.HasFollowUp = true;
                        }
                    }
                }
            });
            this.CheckHasDocuments();
        }
        this.DocumentsList = this.SortItemSource(this.StaticDocumentsList);
        this.SelectedExternalViewModel = this.DocumentsList[0];
        if (this.SelectedExternalViewModel) {
            if (!this.SelectedExternalViewModel.Received) {
                this.SelectedExternalViewModel.SetReceivedButtonVisibility = true;
            }
            else {
                this.SelectedExternalViewModel.SetReceivedButtonVisibility = false;
            }
            if (!this.SelectedExternalViewModel.DocumentHasFile) {
                this.SelectedExternalViewModel.SetAttachedButtonVisibility = true;
                this.SelectedExternalViewModel.DownloadButtonVisibility = false;
            }
            else {
                this.SelectedExternalViewModel.SetAttachedButtonVisibility = false;
                this.SelectedExternalViewModel.DownloadButtonVisibility = true;
            }
        }
        if (this.additional != null) {
            var additionalView = this.StaticDocumentsList.filter(function (d) { return d.ExternalDocumentId == _this.additional.Id; })[0];
            if (additionalView != null) {
                additionalView.UploadButtonClicked();
            }
        }
        this.CurrentSession.StopBusyIndicator();
        this.BuildItemsSource();
    };
    DocsInTabComponent.prototype.FillDocumentTypes = function () {
        if (this.AllDocumentTypeList) {
            var objecttableid = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId) && this.ChildObjectTableId)
                objecttableid = this.ChildObjectTableId;
            else
                objecttableid = this.ObjectTableId;
            this.DocumentTypes = this.AllDocumentTypeList.filter(function (d) { return d.ObjectTableId == objecttableid && d.IsDocIn && !d.InActive; });
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TransportModeId)) {
                switch (this.TransportModeId) {
                    case "A":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(function (a) { return a.IsAir == true; });
                            break;
                        }
                    case "O":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(function (a) { return a.IsOcean == true; });
                            break;
                        }
                    case "I":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(function (a) { return a.IsInland == true; });
                            break;
                        }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentlevelCode)) {
                switch (this.ShipmentlevelCode) {
                    case "C":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(function (a) { return a.IsMaster == true; });
                            break;
                        }
                    case "M":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(function (a) { return a.IsMaster == true; });
                            break;
                        }
                    case "D":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(function (a) { return a.IsDirect == true; });
                            break;
                        }
                    case "H":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(function (a) { return a.IsHouse == true; });
                            break;
                        }
                }
            }
        }
    };
    DocsInTabComponent.prototype.SortItemSource = function (ItemsSource) {
        ItemsSource.sort(function (a, b) {
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
        return ItemsSource;
    };
    DocsInTabComponent.prototype.OnMouseOver = function (item) {
        var _this = this;
        var selectedId = this.SelectedExternalViewModel ? this.SelectedExternalViewModel.Id : null;
        this.DocumentsList.forEach(function (item) {
            if (item.Id != selectedId) {
                item.DownloadButtonVisibility = false;
                item.SetAttachedButtonVisibility = false;
                item.SetReceivedButtonVisibility = false;
            }
            else {
                if (item.DocumentId) {
                    if (_this.SelectedExternalViewModel && _this.SelectedExternalViewModel.DocumentId) {
                        if (item.CurrentDocument && _this.SelectedExternalViewModel && _this.SelectedExternalViewModel.CurrentDocument) {
                            if (item.CurrentDocument.DocumentId != _this.SelectedExternalViewModel.CurrentDocument.DocumentId) {
                                item.DownloadButtonVisibility = false;
                                item.SetAttachedButtonVisibility = false;
                                item.SetReceivedButtonVisibility = false;
                            }
                        }
                    }
                }
            }
        });
        if (!item.Received) {
            item.SetReceivedButtonVisibility = true;
        }
        else {
            item.SetReceivedButtonVisibility = false;
        }
        if (!item.DocumentHasFile) {
            item.SetAttachedButtonVisibility = true;
            item.DownloadButtonVisibility = false;
        }
        else {
            item.SetAttachedButtonVisibility = false;
            item.DownloadButtonVisibility = true;
        }
    };
    DocsInTabComponent.prototype.OnMouseleave = function (item) {
        var _this = this;
        var selectedId = this.SelectedExternalViewModel ? this.SelectedExternalViewModel.Id : null;
        this.DocumentsList.forEach(function (item) {
            if (item.Id != selectedId) {
                item.DownloadButtonVisibility = false;
                item.SetAttachedButtonVisibility = false;
                item.SetReceivedButtonVisibility = false;
            }
            else {
                if (item.DocumentId) {
                    if (_this.SelectedExternalViewModel && _this.SelectedExternalViewModel.DocumentId) {
                        if (item.CurrentDocument && _this.SelectedExternalViewModel.CurrentDocument) {
                            if (item.CurrentDocument.DocumentId != _this.SelectedExternalViewModel.CurrentDocument.DocumentId) {
                                item.DownloadButtonVisibility = false;
                                item.SetAttachedButtonVisibility = false;
                                item.SetReceivedButtonVisibility = false;
                            }
                        }
                    }
                }
            }
        });
    };
    DocsInTabComponent.prototype.OnFollowMouseOver = function (item) {
        this.DocumentsList.forEach(function (item) {
            item.ShowFollowUp = false;
        });
        item.ShowFollowUp = true;
    };
    DocsInTabComponent.prototype.OnFollowMouseleave = function (item) {
        this.DocumentsList.forEach(function (item) {
            item.ShowFollowUp = false;
        });
    };
    DocsInTabComponent.prototype.AddFollowUp = function (item) {
        var generalFollowUpHelper = new GeneralDocumentFollowUpHelper_1.GeneralDocumentFollowUpHelper(this.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildEntityReference, "DocIn", item, this.EntityPM);
        generalFollowUpHelper.AddFollowUp();
    };
    DocsInTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var itemsCollection = [];
        this.HasDocuments = false;
        this.DocumentsList.forEach(function (item) {
            if (item.DataContext.DocumentHasFile) {
                _this.HasDocuments = true;
            }
            itemsCollection.push(item);
        });
        this.ItemsSource.Clear();
        this.ItemsSource.AppendCollection(itemsCollection);
    };
    DocsInTabComponent.prototype.OnSelectedDocumentInList = function (item) {
        var _this = this;
        this.SelectedExternalViewModel = item;
        if (this.SelectedExternalViewModel.DocumentHasFile) {
            this.DeleteAttachmentButtonEnable = true;
        }
        else {
            this.DeleteAttachmentButtonEnable = false;
        }
        if (this.SelectedExternalViewModel.Received && !this.SelectedExternalViewModel.DocumentHasFile) {
            this.UndoReceivedButtonEnable = true;
        }
        else {
            this.UndoReceivedButtonEnable = false;
        }
        this.DocumentsList.forEach(function (item) {
            if (item.Id != _this.SelectedExternalViewModel.Id) {
                item.DownloadButtonVisibility = false;
                item.SetAttachedButtonVisibility = false;
                item.SetReceivedButtonVisibility = false;
            }
        });
    };
    DocsInTabComponent.prototype.DeleteAttachmentButtonClicked = function () {
        var _this = this;
        if (this.SelectedExternalViewModel != null && this.SelectedExternalViewModel.CurrentDocument) {
            this._documentsFilingExtendedPMService.GetDocumentById(this.SelectedExternalViewModel.CurrentDocument.DocumentId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var document = pmResponse.Result;
                    if (document) {
                        if (!_this.IsDeleteAttachment) {
                            _this.IsDeleteAttachment = true;
                            _this.CurrentSession.StartBusyIndicator("Saving...");
                            _this._imageLibraryService.RemoveFile(document.Id, document.Tenant).subscribe(function (result) {
                                _this.externalDocs = _this.externalDocs.filter(function (d) { return d.Id != _this.SelectedExternalViewModel.ExternalDocumentId; });
                                _this.SelectedExternalViewModel.RemoveDocument();
                                _this.DeleteAttachmentButtonEnable = false;
                                _this.UndoReceivedButtonEnable = true;
                                _this.IsDeleteAttachment = false;
                                _this.CheckHasDocuments();
                                _this.CurrentSession.StopBusyIndicator();
                            });
                        }
                    }
                }
            });
        }
    };
    DocsInTabComponent.prototype.CheckHasDocuments = function () {
        var _this = this;
        this.HasDocuments = false;
        if (this.DocumentsList) {
            this.DocumentsList.forEach(function (item) {
                if (item.DataContext.DocumentHasFile) {
                    _this.HasDocuments = true;
                }
            });
        }
    };
    DocsInTabComponent.prototype.UndoReceivedButtonClicked = function () {
        if (this.SelectedExternalViewModel != null) {
            this.SelectedExternalViewModel.UndoReceived();
        }
    };
    DocsInTabComponent.prototype.LoadComplete = function () {
        var _this = this;
        if (this.IsLoadDocumentsFilingListsComplete && this.IsLoadDocumentsFilingListsComplete) {
            this.StaticDocumentsList = [];
            if (this.externalDocs != null) {
                this.externalDocs.forEach(function (docin) {
                    if (_this.AllDocumentTypeList != null) {
                        var docType = _this.AllDocumentTypeList.filter(function (d) { return d.Id == docin.DocumentTypeId && d.InActive == false; })[0];
                        if (docType) {
                            var docVeiwModel = new DocsInDataViewModel_1.DocsInDataViewModel(docin, _this, docType, _this.EntityId, docin.ChildEntityId, docin.ChildEntityReference, _this.externalDocs, _this.ObjectTableId);
                            _this.StaticDocumentsList.push(docVeiwModel);
                        }
                    }
                });
                this.FillDocInList();
            }
            this.CurrentSession.StopBusyIndicator();
        }
    };
    DocsInTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: "DocsInTabControl",
            templateUrl: './DocsInTabComponent.html',
            inputs: ['EntityPM', 'EntityId', 'ChildEntityId', 'ObjectTableId', 'ChildObjectTableId', 'TransportModeId', 'ShipmentlevelCode', 'ChildEntityReference'],
            providers: [DocumentTypeListExtendedService_1.DocumentTypeListExtendedService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService, DocumentsFilingPMService_1.DocumentsFilingPMService, ServiceArgs_1.ServiceArgs, ImageLibraryService_1.ImageLibraryService, DocumentTypeListService_1.DocumentTypeListService],
        }),
        __metadata("design:paramtypes", [DocumentTypeListService_1.DocumentTypeListService, ImageLibraryService_1.ImageLibraryService, EntityArgs_1.EntityArgs, DocumentTypeListExtendedService_1.DocumentTypeListExtendedService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService])
    ], DocsInTabComponent);
    return DocsInTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DocsInTabComponent = DocsInTabComponent;
//# sourceMappingURL=DocsInTabComponent.js.map