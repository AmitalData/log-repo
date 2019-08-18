"use strict";
/// <reference path="../../common/datacontracts/shipmentsharedocumentsdata.ts" />
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../Utilities/SessionLocator");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var DocumentTypePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var Tools_1 = require("../../Infrastructure/Tools");
var DocumentOutPMService_1 = require("../../Common/Services/ExtendedPMs/DocumentOutPMService");
var HtmlEditorService_1 = require("../../Common/Services/DocumentServices/HtmlEditorService");
var FroalaEditorFilters_1 = require("../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/FroalaEditorFilters");
var ExportDocumentService_1 = require("../../Common/Services/DocumentServices/ExportDocumentService");
var DocumentCopiesViewModel_1 = require("../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentCopiesViewModel");
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var ShipmentShareDocumentsData_1 = require("../../Common/DataContracts/ShipmentShareDocumentsData");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var DocumentsFilingExtendedPMService_1 = require("../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentsFilingPMService_1 = require("../../Common/Services/StandardPMs/DocumentsFilingPMService");
var SharedDocumentHelper = /** @class */ (function () {
    function SharedDocumentHelper() {
        this.IsBuildDocumentRunning = false;
        this.IsUploadDocumentRunning = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.BuildingDocumentText = "Building document...";
        this.documentTypePMExtendedService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        this.documentOutPMService = new DocumentOutPMService_1.DocumentOutPMService();
        this.htmlEditorService = new HtmlEditorService_1.HtmlEditorService();
        this.exportDocumentService = new ExportDocumentService_1.ExportDocumentService();
        this.documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this.documentsFilingPMService = new DocumentsFilingPMService_1.DocumentsFilingPMService();
        this.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0].Id;
    }
    SharedDocumentHelper.prototype.BuildDocument = function (shareDocument) {
        var _this = this;
        if (!this.IsBuildDocumentRunning) {
            this.IsBuildDocumentRunning = true;
            this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
            this.ShareDocument = shareDocument;
            if (shareDocument.DocumentOutPM) {
                this.LoadCopies();
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(shareDocument.DocumentOutId)) {
                this.documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.ShareDocument.EntityId, SessionInfo_1.SessionInfo.LoggedUserTenant, "", this.ShareDocument.DocumentTypeId).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        _this.ShareDocument.DocumentOutPM = myResult;
                        _this.LoadCopies();
                    }
                    else
                        _this.StopBusyIndicator();
                });
            }
            else {
                this.documentOutPMService.getCreateDocumentOut(this.ShareDocument.DocumentTypeId, this.ShareDocument.EntityId, this.ChildEntityId, this.ChildReference, this.ObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.ShareDocument.DocumentOutPM = myResult;
                            _this.ShareDocument.DocumentOutId = myResult.Id;
                            _this.LoadCopies();
                        }
                        else
                            _this.StopBusyIndicator();
                    }
                    else
                        _this.StopBusyIndicator();
                });
            }
        }
    };
    SharedDocumentHelper.prototype.LoadCopies = function () {
        var _this = this;
        this.ItemsSource = new Array();
        if (this.ShareDocument.DocumentOutPM != null) {
            this.documentTypePMExtendedService.getSingleDocumentType(this.ShareDocument.DocumentTypeId, this.ShareDocument.DocumentOutPM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.documentTypePM = myResult;
                        if (_this.documentTypePM != null) {
                            if (_this.documentTypePM.DocumentTypeCopies != null) {
                                _this.documentTypePM.DocumentTypeCopies.forEach(function (item) {
                                    _this.ItemsSource.push(new DocumentCopiesViewModel_1.DocumentCopiesViewModel(item, _this.ShareDocument.DocumentOutPM, _this.ShareDocument.EntityId, "", _this.ObjectTableId, "", _this.documentTypePM, ""));
                                });
                                var item = _this.ItemsSource.filter(function (d) { return d.IsSelected; })[0];
                                var anySelected = false;
                                if (!item) {
                                    _this.ItemsSource.forEach(function (item) {
                                        item.IsSelected = item.IsSelectedByDefault;
                                        anySelected = true;
                                    });
                                }
                                _this.Items = _this.ItemsSource;
                            }
                        }
                        _this.CopiesControlLoaded(_this.ItemsSource);
                    }
                    else
                        _this.StopBusyIndicator();
                }
                else {
                    _this.StopBusyIndicator();
                }
            });
        }
    };
    SharedDocumentHelper.prototype.CopiesControlLoaded = function (copies) {
        if (copies != null) {
            if (FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                copies.forEach(function (d) { return d.IsSelectedByDefault = true; });
            }
            if (this.documentTypePM.IsDocumentOneTimePrintLimited) {
                copies.forEach(function (item) { item.CurrentDocumentTypeCopy.IsSelectedByDefault = true; });
            }
            this.lastCount = copies.filter(function (d) { return d.CurrentDocumentTypeCopy.IsSelectedByDefault; }).length;
            var editorToolCode = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShareDocument.DocumentOutPM.DocumentTemplateEditorTool)) {
                editorToolCode = this.ShareDocument.DocumentOutPM.DocumentTemplateEditorTool;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.documentTypePM.DocumentTypeDefaultEditorTool)) {
                editorToolCode = this.documentTypePM.DocumentTypeDefaultEditorTool;
            }
            if (editorToolCode) {
                if (editorToolCode == "S")
                    this.BuildCurrentCopies(copies);
                if (editorToolCode == "R") {
                    this.Items = new Array();
                    this.Items = copies;
                    this.ReBluidHtmlDocument();
                }
            }
            else {
                this.ShowMessage("No templates found for this document!");
                this.StopBusyIndicator();
            }
        }
    };
    SharedDocumentHelper.prototype.BuildCurrentCopies = function (copies) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
        this.AddedDocumentTypeCopyViewModels = new Array();
        this.RemovedDocumentTypeCopyViewModels = new Array();
        var anySelected = false;
        copies.forEach(function (copy) {
            if (!_this.documentTypePM.IsDocumentOneTimePrintLimited) {
                if (copy.IsSelected || copies.length == 1) {
                    anySelected = true;
                    if (copies.length == 1) {
                        copy.IsSelected = true;
                        copy.CurrentDocumentTypeCopy.IsSelectedByDefault = true;
                        copy.IsDiableSelctedDocumentTypeCopy = true;
                    }
                    else {
                        copy.IsDiableSelctedDocumentTypeCopy = false;
                    }
                    _this.AddedDocumentTypeCopyViewModels.push(copy);
                }
                if (copy.Exists && !copy.IsSelected) {
                    var index = _this.RemovedDocumentTypeCopyViewModels.indexOf(copy, 0);
                    if (index) {
                        _this.RemovedDocumentTypeCopyViewModels.splice(index, 1);
                    }
                }
            }
            else {
                copy.IsSelected = true;
                anySelected = true;
                _this.AddedDocumentTypeCopyViewModels.push(copy);
            }
        });
        if (anySelected) {
            this.lastCount = this.AddedDocumentTypeCopyViewModels.length;
            var numberOfCopy = this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.IsSelected; }).length;
            var count = 0;
            this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.IsSelected; }).forEach(function (copy) {
                _this.exportDocumentService.getDocumentPdfFile(_this.documentTypePM.Id, _this.ShareDocument.EntityId, _this.ObjectTableId, "", "", _this.ShareDocument.DocumentOutPM.Id, _this.ShareDocument.DocumentOutPM.Tenant, copy.CurrentDocumentTypeCopy.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
                    count += 1;
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult != null) {
                            copy.Status = "Success";
                            copy.Exists = true;
                            if (numberOfCopy == count) {
                                _this.ShareDocument.DocumentOutPM.NeedsRebuild = false;
                                _this.ShareDocument.DocumentOutPM.Issued = true;
                                _this.ShareDocument.DocumentOutPM.IssuedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                _this.ShareDocument.DocumentOutPM.IsChangeIssuedDate = true;
                                _this.SaveContext();
                            }
                        }
                        else
                            _this.StopBusyIndicator();
                    }
                    else {
                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            _this.ShowMessage(pmResponse.ErrorsArray[0]);
                        }
                        _this.StopBusyIndicator();
                    }
                });
            });
            if (this.AddedDocumentTypeCopyViewModels) {
                var copies = new Array();
                this.Items.forEach(function (copy) {
                    var item = _this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.Id == copy.Id; })[0];
                    if (item)
                        copies.push(item);
                    else
                        copies.push(copy);
                });
                this.Items = copies;
            }
        }
        else {
            this.StopBusyIndicator();
            this.ShowMessage("Please select a copy then rebuild!");
        }
    };
    SharedDocumentHelper.prototype.SaveContext = function () {
        var _this = this;
        this.documentOutPMService.putDocumentOut(this.ShareDocument.DocumentOutPM).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myResult = pmResponse.Result;
                _this.documentOutPMService.getSingleDocumentOutPM(_this.ShareDocument.DocumentOutPM.Id, _this.ShareDocument.DocumentOutPM.Tenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.ShareDocument.DocumentOutPM = myResult;
                            _this.ShareDocument.Included = true;
                            _this.ShareDocument.LastUpdateDate = _this.ShareDocument.DocumentOutPM.IssuedDate;
                            _this.ShareDocument.SecurityId = _this.ShareDocument.DocumentOutPM.SecurityId;
                            _this.ShareDocument.ActionButtonLabel = "Update";
                            if (_this.ShareDocument.DocumentOutPM && _this.ShareDocument.DocumentOutPM.DocumentOutCopies) {
                                var documentOutCopy = _this.ShareDocument.DocumentOutPM.DocumentOutCopies.filter(function (d) { return d.DocumentTypeCopyId == _this.ShareDocument.DocumentTypeCopyId; })[0];
                                if (documentOutCopy) {
                                    _this.ShareDocument.DocumentId = documentOutCopy.Id;
                                }
                            }
                            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Shipment", _this.ShareDocument.DocumentTypeName + " Built");
                            _this.CurrentSession.FireEvent("RefreshDocumentOutPrint");
                            if (!_this.ShareDocument.IsReady) {
                                _this.ShareDocument.IsReady = true;
                                _this.documentOutPMService.GetCalculatedFileNameForDocumentOutCopy(_this.ShareDocument.DocumentOutPM.Id, _this.ShareDocument.DocumentTypeCopyId).subscribe(function (res) {
                                    _this.StopBusyIndicator();
                                    var pmResponse = res;
                                    if (!pmResponse.HasError) {
                                        var myResult = pmResponse.Result;
                                        if (myResult) {
                                            _this.ShareDocument.FileName = myResult;
                                        }
                                    }
                                });
                            }
                            else {
                                _this.StopBusyIndicator();
                            }
                        }
                    }
                    else
                        _this.StopBusyIndicator();
                });
            }
            else
                _this.StopBusyIndicator();
        });
    };
    SharedDocumentHelper.prototype.ReBluidHtmlDocument = function () {
        var _this = this;
        var documentTypeCopyId = this.ShareDocument.DocumentTypeId;
        var documentTypeId = this.ShareDocument.DocumentTypeId;
        var shipmentId = this.ShareDocument.EntityId;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.htmlEditorService.getEditorHtmlData(this.ShareDocument.DocumentOutPM.Id, shipmentId, this.ObjectTableId, "", "", SessionLocator_1.SessionLocator.Tenant, SessionLocator_1.SessionLocator.LoggedUserId, false, this.ShareDocument.DocumentOutPM.DocumentTemplateId, "", "Edit").subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.HtmlEditorData = "<header>" + "<height>" + "<div style='display:none'>" + myResult.HeaderHeight + "</div></height>" + myResult.HeaderHtml + "</header>" + myResult.Htmlstring + "<footer>" + "<height>" + "<div style='display:none'>" + myResult.FooterHeight + "</div></height>" + myResult.FooterHtml + "</footer>";
                    _this.HeaderHeight = myResult.HeaderHeight;
                    _this.FooterHeight = myResult.FooterHeight;
                }
                _this.StopBusyIndicator();
                _this.CurrentSession.StartBusyIndicator(_this.BuildingDocumentText);
                _this.SaveReportData(documentTypeCopyId);
            }
            else
                _this.StopBusyIndicator();
        });
    };
    SharedDocumentHelper.prototype.SaveReportData = function (documentTypeCopyId) {
        var _this = this;
        var filter = new FroalaEditorFilters_1.FroalaEditorFilters();
        filter.DocumentOutId = this.ShareDocument.DocumentOutPM.Id;
        filter.DocumentTypeCopyId = documentTypeCopyId;
        filter.Tenant = SessionLocator_1.SessionLocator.Tenant;
        filter.HtmlString = this.HtmlEditorData;
        filter.HeaderHeight = this.HeaderHeight;
        filter.FooterHeight = this.FooterHeight;
        filter.EntityId = this.ShareDocument.EntityId;
        filter.ChildEntityId = "";
        filter.DocumentTypeId = this.ShareDocument.DocumentTypeId;
        var idArray;
        this.htmlEditorService.saveEditedReportToServer(filter).subscribe(function (res) {
            _this.CurrentSession.StartBusyIndicator(_this.BuildingDocumentText);
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.docIds = myResult;
                    idArray = _this.docIds.split(',');
                    _this.AddedDocumentTypeCopyViewModels = new Array();
                    _this.RemovedDocumentTypeCopyViewModels = new Array();
                    var anySelected = false;
                    _this.Items.forEach(function (copy) {
                        if (!_this.documentTypePM.IsDocumentOneTimePrintLimited) {
                            if (copy.IsSelected) {
                                anySelected = true;
                                _this.AddedDocumentTypeCopyViewModels.push(copy);
                            }
                            if (copy.Exists && !copy.IsSelected) {
                                _this.RemovedDocumentTypeCopyViewModels.push(copy);
                            }
                        }
                        else {
                            copy.IsSelected = true;
                            anySelected = true;
                            _this.AddedDocumentTypeCopyViewModels.push(copy);
                        }
                    });
                    if (anySelected) {
                        _this.lastCount = _this.AddedDocumentTypeCopyViewModels.length;
                        var numberOfCopy = _this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.IsSelected; }).length;
                        if (numberOfCopy > 0) {
                            _this.ShareDocument.DocumentOutPM.XamlDocumentId = idArray[1];
                            _this.ShareDocument.DocumentOutPM.IssuedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                            _this.ShareDocument.DocumentOutPM.IsChangeIssuedDate = true;
                            _this.SaveContext();
                        }
                        else
                            _this.StopBusyIndicator();
                    }
                    else {
                        _this.StopBusyIndicator();
                    }
                }
                else {
                    _this.StopBusyIndicator();
                }
            }
            else
                _this.StopBusyIndicator();
        });
    };
    SharedDocumentHelper.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    SharedDocumentHelper.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
        this.IsBuildDocumentRunning = false;
    };
    SharedDocumentHelper.prototype.UploadButton = function (sareDocument, shipmentShareDocumentsData, tiggerViewModel) {
        if (!this.IsUploadDocumentRunning) {
            if (sareDocument && shipmentShareDocumentsData) {
                this.IsUploadDocumentRunning = true;
                this.TiggerViewModel = tiggerViewModel;
                this.ShareDocument = sareDocument;
                this.ShipmentShareDocumentsData = shipmentShareDocumentsData;
                this.ShareDocument.EntityId = shipmentShareDocumentsData.EntityId;
                if (Tools_1.AppTool.IsNullOrEmpty(this.ShareDocument.DocumentsFilingId)) {
                    this.CreateDocumentsFilingPM();
                }
                else {
                    if (this.ShareDocument.IsReady && this.ShareDocument.ActionButtonLabel == "Additional") {
                        var shareDocument = new ShipmentShareDocumentsData_1.ShareDocument();
                        shareDocument.DirectionCode = this.ShareDocument.DirectionCode;
                        shareDocument.DocumentTypeId = this.ShareDocument.DocumentTypeId;
                        shareDocument.DocumentTypeCode = this.ShareDocument.DocumentTypeCode;
                        shareDocument.DocumentTypeName = this.ShareDocument.DocumentTypeName;
                        shareDocument.EntityId = this.ShareDocument.EntityId;
                        this.CreateDocumentsFilingPM(shareDocument);
                    }
                    else {
                        if (!this.ShareDocument.DocumentsFilingPM) {
                            this.LoadDocumentFiling();
                        }
                        else {
                            this.ShowAttachExternal();
                        }
                    }
                }
            }
        }
    };
    SharedDocumentHelper.prototype.CreateDocumentsFilingPM = function (shareDocument) {
        var _this = this;
        if (shareDocument === void 0) { shareDocument = null; }
        if (!shareDocument)
            shareDocument = this.ShareDocument;
        this.documentsFilingExtendedPMService.CreateDocumentsFiling(shareDocument.DocumentTypeId, shareDocument.EntityId, "", "", this.ObjectTableId, "I", SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    shareDocument.DocumentsFilingPM = myResult;
                    shareDocument.DocumentsFilingId = myResult.Id;
                    _this.ShowAttachExternal(shareDocument);
                }
            }
        });
    };
    SharedDocumentHelper.prototype.LoadDocumentFiling = function () {
        var _this = this;
        this.documentsFilingPMService.get(this.ShareDocument.DocumentsFilingId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.ShareDocument.DocumentsFilingPM = myResult;
                    _this.ShareDocument.DocumentsFilingId = myResult.Id;
                    _this.ShowAttachExternal();
                }
            }
        });
    };
    SharedDocumentHelper.prototype.ShowAttachExternal = function (shareDocument) {
        var _this = this;
        if (shareDocument === void 0) { shareDocument = null; }
        if (!shareDocument)
            shareDocument = this.ShareDocument;
        var windowArgs = {};
        windowArgs.EntityId = shareDocument.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.RequsetPageName = "SharedDocument";
        windowArgs.CurrentDocument = shareDocument.DocumentsFilingPM;
        windowArgs.TiggerViewModel = this;
        windowArgs.Entity = shareDocument;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            _this.IsUploadDocumentRunning = false;
        });
    };
    SharedDocumentHelper.prototype.OnUploadComplete = function (item) {
        if (item) {
            if (item.DocumentsFilingPM) {
                item.Extension = item.DocumentsFilingPM.FileExtension;
                item.FileSize = item.DocumentsFilingPM.FileSize;
                item.DocumentId = item.DocumentsFilingPM.DocumentId;
                item.LastShareDate = item.DocumentsFilingPM.LastShareDate;
                item.LastUpdateDate = item.DocumentsFilingPM.UpdateDate;
                item.SecurityId = item.DocumentsFilingPM.SecurityId;
                if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentsFilingPM.FileName)) {
                    item.FileName = item.DocumentsFilingPM.FileName.toLowerCase().replace("." + item.Extension, "");
                }
                item.IsReady = true;
                item.Included = true;
                item.ActionButtonLabel = "Additional";
                if (this.ShipmentShareDocumentsData && this.ShipmentShareDocumentsData.ShareDocuments) {
                    if (!this.ShipmentShareDocumentsData.ShareDocuments.filter(function (d) { return d.SecurityId == item.SecurityId; })[0]) {
                        this.ShipmentShareDocumentsData.ShareDocuments.push(item);
                        if (this.TiggerViewModel) {
                            this.TiggerViewModel.ShareDocumentSelected = item;
                            this.TiggerViewModel.SortItemSource(this.ShipmentShareDocumentsData.ShareDocuments);
                        }
                    }
                }
                this.CurrentSession.FireEvent("RefreshDocIn");
            }
        }
    };
    return SharedDocumentHelper;
}());
exports.SharedDocumentHelper = SharedDocumentHelper;
//# sourceMappingURL=SharedDocumentHelper.js.map