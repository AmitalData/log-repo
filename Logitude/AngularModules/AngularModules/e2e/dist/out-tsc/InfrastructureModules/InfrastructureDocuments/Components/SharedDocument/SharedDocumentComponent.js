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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var ShipmentShareDocumentsData_1 = require("../../../../Common/DataContracts/ShipmentShareDocumentsData");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AgentSharedDocumentExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/AgentSharedDocumentExtendedService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var AttachmentsList_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var SharedDocumentHelper_1 = require("../../../../Infrastructure/Helpers/SharedDocumentHelper");
var SharedDocumentComponent = /** @class */ (function () {
    function SharedDocumentComponent(_documentTypePMExtendedService, _agentSharedDocumentExtendedService) {
        this._documentTypePMExtendedService = _documentTypePMExtendedService;
        this._agentSharedDocumentExtendedService = _agentSharedDocumentExtendedService;
        this.AgentName = "";
        this.Master = "";
        this.IsShowMessageNoDocument = false;
        this.OnCloseSharedWithAgentsEvent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Mode = "";
        this.SelectedShipmentShareDocumentsDataLists = [];
    }
    SharedDocumentComponent.prototype.ngOnInit = function () {
    };
    SharedDocumentComponent.prototype.SetWindowArgs = function (args) {
        this.Mode = args.Mode;
        this.EntityPM = args.EntityPM;
        this.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0].Id;
        if (this.EntityPM) {
            this.AgentName = this.EntityPM.AgentName;
            this.Master = this.EntityPM.LongMaster;
            this.ShipmentNumber = this.EntityPM.ShipmentNumber;
            this.LoadData();
        }
        this.OkButtonLable = this.Mode == "Attachment" ? "OK" : "Share";
        if (this.Mode == "Attachment") {
            this.OnCloseSharedWithAgentsEvent = args.OnCloseSharedWithAgentsEvent;
            this.AttachmentsLists = [];
        }
    };
    SharedDocumentComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ShipmentShareDocumentsDataLists = [];
        this._documentTypePMExtendedService.GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel(this.EntityPM.Id, this.EntityPM.AgentId, this.EntityPM.ShipmentNumber, this.ObjectTableId, this.EntityPM.ShipmentLevelCode, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myList = pmResponse.Result;
                _this.ShipmentShareDocumentsDataLists = myList;
                if (_this.ShipmentShareDocumentsDataLists.length == 0) {
                    _this.IsShowMessageNoDocument = true;
                }
                else {
                    _this.ShipmentShareDocumentsDataLists = _this.ShipmentShareDocumentsDataLists.filter(function (d) { return d.ShareDocuments && d.ShareDocuments.length > 0; });
                }
                if (_this.ShipmentShareDocumentsDataLists) {
                    _this.ShipmentShareDocumentsDataLists.forEach(function (item) {
                        if (item.ShareDocuments && !Tools_1.AppTool.IsNullOrEmpty(item.AgentSharedManifestRef)) {
                            item.ShareDocuments.forEach(function (doc) {
                                doc.Included = doc.IsReady;
                            });
                            _this.SortItemSource(item.ShareDocuments);
                        }
                    });
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    SharedDocumentComponent.prototype.OnmMouseOver = function (item) {
        this.ShipmentShareDocumentsDataLists.forEach(function (item) {
            item.ShareDocuments.forEach(function (shareDocument) {
                shareDocument.VisibleUploadButton = false;
                shareDocument.VisibleBliudDocumentButton = false;
                shareDocument.VisibleViewButton = false;
            });
        });
        item.VisibleViewButton = true;
        if (item.DirectionCode == "I") {
            item.VisibleUploadButton = true;
        }
        else {
            item.VisibleBliudDocumentButton = true;
        }
        if (this.ShareDocumentSelected) {
            this.ShareDocumentSelected.VisibleViewButton = true;
            if (this.ShareDocumentSelected.DirectionCode == "I") {
                this.ShareDocumentSelected.VisibleUploadButton = true;
            }
            else {
                this.ShareDocumentSelected.VisibleBliudDocumentButton = true;
            }
        }
    };
    SharedDocumentComponent.prototype.OnmMouseleave = function (item) {
        this.ShipmentShareDocumentsDataLists.forEach(function (item) {
            item.ShareDocuments.forEach(function (shareDocument) {
                shareDocument.VisibleUploadButton = false;
                shareDocument.VisibleBliudDocumentButton = false;
                shareDocument.VisibleViewButton = false;
            });
        });
        if (this.ShareDocumentSelected) {
            this.ShareDocumentSelected.VisibleViewButton = true;
            if (this.ShareDocumentSelected.DirectionCode == "I") {
                this.ShareDocumentSelected.VisibleUploadButton = true;
            }
            else {
                this.ShareDocumentSelected.VisibleBliudDocumentButton = true;
            }
        }
    };
    SharedDocumentComponent.prototype.ViewButtonClick = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item.SecurityId)) {
            if (item.DirectionCode == "I") {
                DownloadManager_1.DownloadManager.DownloadPage("", item.SecurityId);
            }
            else
                DownloadManager_1.DownloadManager.DownloadPage(item.DocumentId, item.SecurityId);
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            if (item.DirectionCode == "I") {
                messageWindow.Show("Please upload a document first");
            }
            else
                messageWindow.Show("Please Bliud a document first");
        }
    };
    SharedDocumentComponent.prototype.BuildButtonClick = function (item) {
        if (!this.sharedDocumentHelper || (this.sharedDocumentHelper && !this.sharedDocumentHelper.IsBuildDocumentRunning)) {
            this.sharedDocumentHelper = new SharedDocumentHelper_1.SharedDocumentHelper();
            this.sharedDocumentHelper.BuildDocument(item);
        }
    };
    SharedDocumentComponent.prototype.UploadButtonClick = function (item, shipmentShareDocumentsData) {
        if (!this.sharedDocumentHelper || (this.sharedDocumentHelper && !this.sharedDocumentHelper.IsUploadDocumentRunning)) {
            this.sharedDocumentHelper = new SharedDocumentHelper_1.SharedDocumentHelper();
            this.sharedDocumentHelper.UploadButton(item, shipmentShareDocumentsData, this);
        }
    };
    SharedDocumentComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedDocumentComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.SelectedShipmentShareDocumentsDataLists = [];
        if (this.ShipmentShareDocumentsDataLists) {
            this.ShipmentShareDocumentsDataLists.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.AgentSharedManifestRef) && item.ShareDocuments.filter(function (d) { return d.Included == true; }).length > 0) {
                    var shipmentShareDocumentsData = new ShipmentShareDocumentsData_1.ShipmentShareDocumentsData();
                    shipmentShareDocumentsData.AgentId = item.AgentId;
                    shipmentShareDocumentsData.AgentSharedManifestRef = item.AgentSharedManifestRef;
                    shipmentShareDocumentsData.TenantAgent = item.TenantAgent;
                    shipmentShareDocumentsData.EntityId = item.EntityId;
                    shipmentShareDocumentsData.ShipmentLevelCode = item.ShipmentLevelCode;
                    shipmentShareDocumentsData.ShipmentNumber = item.ShipmentNumber;
                    shipmentShareDocumentsData.ShareDocuments = item.ShareDocuments.filter(function (d) { return d.Included; });
                    _this.SelectedShipmentShareDocumentsDataLists.push(shipmentShareDocumentsData);
                }
            });
            if (this.Mode == "Attachment") {
                this.AttachmentsLists = new Array();
                this.SelectedShipmentShareDocumentsDataLists.forEach(function (item) {
                    item.ShareDocuments.forEach(function (doc) {
                        var item = new AttachmentsList_1.AttachmentsList();
                        item.Id = doc.DocumentId;
                        item.DocumentFilingId = doc.DocumentsFilingId;
                        item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        item.FileSize = doc.FileSize;
                        item.FileExtension = doc.Extension;
                        item.DocumentTypeCopyNameWithDocumentTypeName = doc.DocumentTypeName;
                        item.ShowRemoveLink = true;
                        _this.AttachmentsLists.push(item);
                    });
                });
                this.OnCloseSharedWithAgentsEvent.emit(this.AttachmentsLists);
                this.CloseButtonClicked();
            }
            else {
                if (this.SelectedShipmentShareDocumentsDataLists.length > 0) {
                    this.CurrentSession.StartBusyIndicator("Sharing Documnents...");
                    this._agentSharedDocumentExtendedService.PostSharedDocuments(this.SelectedShipmentShareDocumentsDataLists, this.EntityPM.Id).subscribe(function (res) {
                        var pmResponse = res;
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Title = "Share Document";
                        if (!pmResponse.HasError) {
                            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", "Share Documents");
                            messageWindow.Show("Selected documents were shared successfully.");
                            _this.CloseButtonClicked();
                        }
                        else {
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray[0]) {
                                messageWindow.Show(pmResponse.ErrorsArray[0].toString());
                            }
                        }
                        _this.CurrentSession.StopBusyIndicator();
                    });
                }
                else {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = "Share Document";
                    messageWindow.Show("Please select at least one document.");
                }
            }
        }
        else {
            if (this.Mode == "Attachment") {
                this.OnCloseSharedWithAgentsEvent.emit(this.AttachmentsLists);
                this.CloseButtonClicked();
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Title = "Share Document";
                messageWindow.Show("Please definition at least one document.");
            }
        }
    };
    SharedDocumentComponent.prototype.SortItemSource = function (ItemsSource) {
        ItemsSource.sort(function (a, b) {
            if (a.DocumentTypeName.toLowerCase() < b.DocumentTypeName.toLowerCase()) {
                return -1;
            }
            else if (a.DocumentTypeName.toLowerCase() > b.DocumentTypeName.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
        return ItemsSource;
    };
    SharedDocumentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedDocumentComponent',
            templateUrl: './SharedDocumentComponent.html',
            providers: [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, AgentSharedDocumentExtendedService_1.AgentSharedDocumentExtendedService],
        }),
        __metadata("design:paramtypes", [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, AgentSharedDocumentExtendedService_1.AgentSharedDocumentExtendedService])
    ], SharedDocumentComponent);
    return SharedDocumentComponent;
}());
exports.SharedDocumentComponent = SharedDocumentComponent;
//# sourceMappingURL=SharedDocumentComponent.js.map