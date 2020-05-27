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
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DocumentTypeTemplateViewModel_1 = require("./DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SendHtmlDocumentFilter_1 = require("./DocsOut/Filters/SendHtmlDocumentFilter");
var FroalaEditorSetting_1 = require("./DocsOut/FroalaEditorSetting");
var DocumentOutPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var DocumentOutCopyViewModel_1 = require("./DocsOut/ViewModel/DocumentOutCopyViewModel");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AttachmentsList_1 = require("./DocsOut/Filters/AttachmentsList");
var DocumentTypeListService_1 = require("../../../../Common/Services/StandardLists/DocumentTypeListService");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var ServiceArgs_1 = require("../../../../Infrastructure/DataContracts/ServiceArgs");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityPartner_1 = require("../../../../Infrastructure/DataContracts/EntityPartner");
var DocumentTypeTemplateListExtendedService_1 = require("../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService");
var HtmlEditorService_1 = require("../../../../Common/Services/DocumentServices/HtmlEditorService");
var CommunicationAttachmentExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/CommunicationAttachmentExtendedPMService");
var CommunicationLogExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/CommunicationLogExtendedPMService");
var CommunicationLogPMViewModel_1 = require("./DocsOut/ViewModel/CommunicationLogPMViewModel");
var AttachmentDocment_1 = require("./DocsOut/ViewModel/AttachmentDocment");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var DocumentExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentExtendedService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var SendDocumentComponent = /** @class */ (function () {
    function SendDocumentComponent(_communicationLogExtendedPMService, _communicationAttachmentExtendedPMService, _documentOutPMService, _documentExtendedService, _documentsFilingExtendedPMService, _documentTypeTemplateListExtendedService, _htmlEditorService, _documentTypePMService, cd, _documentTypeListService) {
        this._communicationLogExtendedPMService = _communicationLogExtendedPMService;
        this._communicationAttachmentExtendedPMService = _communicationAttachmentExtendedPMService;
        this._documentOutPMService = _documentOutPMService;
        this._documentExtendedService = _documentExtendedService;
        this._documentsFilingExtendedPMService = _documentsFilingExtendedPMService;
        this._documentTypeTemplateListExtendedService = _documentTypeTemplateListExtendedService;
        this._htmlEditorService = _htmlEditorService;
        this._documentTypePMService = _documentTypePMService;
        this.cd = cd;
        this._documentTypeListService = _documentTypeListService;
        this.IsDisableAddTemplateFromLibrary = false;
        this.OnCloseAttachmentDocsInEvent = new core_1.EventEmitter();
        this.OnCloseSendToContactsEvent = new core_1.EventEmitter();
        this.OnCloseSharedWithAgentsEvent = new core_1.EventEmitter();
        this.IsShowEditHtml = false;
        this.AreaAttachmentWidth = "600px";
        this.AttrTitleShowTemplateList = "Expand";
        this.ComponentSendTokey = "";
        this.PageType = "Send";
        this.IsShowTemplateList = false;
        this.Subject = null;
        this.ToEmail = "";
        this.Bcc = "";
        this.Cc = "";
        this.IsEnableLinkAttachExternal = true;
        this.IsShowCcBox = false;
        this.IsShowBccBox = false;
        this.IsHideBccCcLinkArea = false;
        this.IsCheckedInActive = false;
        this.IsEnableLinkDocOout = true;
        this.IsEnableLinkDocsSharedWithAgents = true;
        this.IsEnableLinkDocIn = true;
        this.CountTd = 1;
        this.Count = 0;
        this.IsSendEditMode = true;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.From = "";
        this.ReplyTo = "";
        this.IsShowLinkDocsSharedWithAgents = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.order = 0;
        this.IsCloseSendToContact = false;
        this.IsOpenWidnow = false;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "UPDATE") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "HTMLEMAIL")) {
            this.IsShowEditHtml = true;
        }
        this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
        this.froalaEditorSetting.PageType = "Send";
        this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
        this.ShowInactiveCheckBoxKey = Guid_1.Guid.newGuid();
        this.AttachmentListId = Guid_1.Guid.newGuid();
        this.AttachmentsLists = new Array();
        this.DocumentTypeTemplatePMLists = [];
    }
    SendDocumentComponent.prototype.ngOnInit = function () {
    };
    SendDocumentComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        if (this.SelectedInternalDocument && this.IsSendEditMode && this.SelectedInternalDocument.TemplateType.toUpperCase() != "M") {
            var copy = this.SelectedInternalDocument.CurrentDocument.DocumentOutCopies.filter(function (d) { return d.Id == _this.SelectedInternalDocument.documentOutCopyId; })[0];
            if (copy) {
                var attachment = new AttachmentsList_1.AttachmentsList();
                attachment.Tenant = this.SelectedInternalDocument.CurrentDocument.Tenant;
                var documentType = this.SelectedInternalDocument.DocumentType;
                if (documentType) {
                    var attachmentName = documentType.Name;
                    if (attachmentName != copy.DocoumentTypeCopyName) {
                        attachmentName = attachmentName + "-" + copy.DocoumentTypeCopyName;
                    }
                }
                attachment.DocumentTypeCopyNameWithDocumentTypeName = attachmentName;
                attachment.FileSize = this.SelectedInternalDocument.CurrentDocument.FileSize;
                attachment.ShowRemoveLink = true;
                attachment.Id = copy.Id;
                var attachmentsList = new Array();
                attachmentsList.push(attachment);
                this.AttachmentsLists = attachmentsList;
                this.BliudAttachmentList(attachmentsList, false, false);
            }
        }
        if (this.SelectedInternalDocument.AttachmentsLists) {
            this.BliudAttachmentList(this.SelectedInternalDocument.AttachmentsLists, false, false);
        }
    };
    SendDocumentComponent.prototype.SetDataContext = function (dataContext) {
        var _this = this;
        this.SelectedInternalDocument = dataContext;
        if (this.SelectedInternalDocument.ModeSendDocument == "preview") {
            this.IsSendEditMode = false;
            this.froalaEditorSetting.IsDisableEdit = true;
        }
        if (this.IsSendEditMode) {
            this.froalaEditorSetting.Height = this.SelectedInternalDocument.WindowHeight - 210;
            this.froalaEditorSetting.IsDisableEdit = false;
        }
        if (!dataContext.DocumentTypePM && !Tools_1.AppTool.IsNullOrEmpty(dataContext.Id)) {
            this.CurrentSession.StartBusyIndicator("Loading...");
            this._documentTypePMService.GetSinglePMWithOutInclude(dataContext.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    dataContext.DocumentTypePM = pmResponse.Result;
                }
                _this.CurrentSession.StopBusyIndicator();
                _this.Start(dataContext);
            });
        }
        else
            this.Start(dataContext);
    };
    SendDocumentComponent.prototype.Start = function (item) {
        var _this = this;
        this.SelectedInternalDocument = item;
        this.EntityId = this.SelectedInternalDocument.EntityId;
        this.DocumentOutCopyId = this.SelectedInternalDocument.documentOutCopyId;
        this.CurrentDocument = this.SelectedInternalDocument.CurrentDocument;
        this.CurrentDocumentType = this.SelectedInternalDocument.DocumentTypePM;
        this.ObjectTableId = this.SelectedInternalDocument.CurrentObjectTableId;
        this.EventRefreshName = this.SelectedInternalDocument.EventRefreshName;
        if (this.CurrentDocumentType) {
            this.DocumentTypeId = this.CurrentDocumentType.Id;
        }
        if (this.CurrentDocument) {
            this.DocumentOutId = this.CurrentDocument.Id;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedInternalDocument.ToSpecificeEmail)) {
            this.ToEmail = this.SelectedInternalDocument.ToSpecificeEmail;
        }
        this.ChildEntityId = this.SelectedInternalDocument.ChildEntityId ? this.SelectedInternalDocument.ChildEntityId : "";
        this.ChildObjectTableId = this.SelectedInternalDocument.ChildObjectTableId ? this.SelectedInternalDocument.ChildObjectTableId : "";
        if (Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId)) {
            if (this.SelectedInternalDocument && this.SelectedInternalDocument.DocumentTypePM) {
                var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
                var tableId = this.SelectedInternalDocument.DocumentTypePM.ObjectTableId;
                if (tableId != this.ObjectTableId) {
                    this.ChildObjectTableId = tableId;
                }
            }
        }
        this.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        this.ObjecttableName = table.Name;
        if (this.IsSendEditMode) {
            if (this.ObjecttableName == "Shipment") {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedDocument", "DOCSSHAREDVIAEMAIL")) {
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "AgentSharedManifest")) {
                        if (this.SelectedInternalDocument.EntityPM && this.SelectedInternalDocument.EntityPM.DirectionId == "E" && (this.SelectedInternalDocument.EntityPM.ShipmentLevelCode == "C" || this.SelectedInternalDocument.EntityPM.ShipmentLevelCode == "D")) {
                            this.IsShowLinkDocsSharedWithAgents = true;
                        }
                    }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedInternalDocument.Subject)) {
                this.Subject = this.SelectedInternalDocument.Subject;
            }
            else {
                this.Subject = this.CurrentDocument.DocumentTypeSubject != null ? this.CurrentDocument.DocumentTypeSubject : this.CurrentDocument.DocumentTypeName;
            }
            this.LoadDocumentTypeTemplates(null);
        }
        else {
            this.ViewCommunicationLog();
        }
    };
    SendDocumentComponent.prototype.ViewCommunicationLog = function () {
        var _this = this;
        if (this.SelectedInternalDocument.SelectedCommunicationLogViewMode) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.Cc = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.CC;
            this.ToEmail = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.To;
            this.Subject = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Subject;
            this.Bcc = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.CurrentEntityPm.BCC;
            var froalaheight = this.SelectedInternalDocument.WindowHeight - 140;
            if (this.Cc) {
                froalaheight -= 22;
                this.IsShowCcBox = true;
            }
            if (this.Bcc) {
                froalaheight -= 22;
                this.IsShowBccBox = true;
            }
            this.IsHideBccCcLinkArea = true;
            this.froalaEditorSetting.IsDisableEdit = true;
            this.froalaEditorSetting.Height = froalaheight;
            this._htmlEditorService.getSentMessageHtmlBody(this.SelectedInternalDocument.SelectedCommunicationLogViewMode.CurrentEntityPm.DocumentId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult);
                        _this.ReloadFroalaEditor();
                    }
                    _this._communicationAttachmentExtendedPMService.getCommunicationAttachmentsByCommunicationLogId(_this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                _this.CommunicationAttachmentPMs = myResult;
                                var attachmentsLogList = new Array();
                                if (_this.CommunicationAttachmentPMs && _this.CommunicationAttachmentPMs.length > 0) {
                                    var logAttachments = _this.CommunicationAttachmentPMs.filter(function (a) { return a.CommunicationLogId == _this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Id; });
                                    // IsViewGeneralAttachment
                                    var numberOfAttachment = logAttachments.length;
                                    var count = 0;
                                    if (_this.SelectedInternalDocument.IsViewGeneralAttachment) {
                                        logAttachments.forEach(function (attachment) {
                                            _this._documentExtendedService.GetDocumentById(attachment.DocumentId, attachment.Tenant).subscribe(function (res) {
                                                var pmResponse = res;
                                                count += 1;
                                                if (!pmResponse.HasError) {
                                                    var myResult = pmResponse.Result;
                                                    if (myResult) {
                                                        var document = myResult;
                                                        var fileName = !Tools_1.AppTool.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName : document.FileName;
                                                        var attachmentlog = _this.GetAttachmentList(fileName, document.Id, document.FileSize, attachment.Tenant);
                                                        if (attachmentlog) {
                                                            attachmentsLogList.push(attachmentlog);
                                                        }
                                                        if (count == numberOfAttachment) {
                                                            _this.AttachmentsLists = attachmentsLogList;
                                                            if (attachmentsLogList) {
                                                                _this.BliudAttachmentList(attachmentsLogList);
                                                            }
                                                        }
                                                    }
                                                }
                                                if (count == numberOfAttachment) {
                                                    _this.CurrentSession.StopBusyIndicator();
                                                }
                                            });
                                        });
                                    }
                                    else {
                                        //start region
                                        var documentOutCopies = new Array();
                                        if (_this.SelectedInternalDocument.DocsOutTabComponent) {
                                            _this.documentInPMs = _this.SelectedInternalDocument.DocsOutTabComponent.DocumentInPMs;
                                        }
                                        if (_this.SelectedInternalDocument.DocsOutItemsList) {
                                            _this.SelectedInternalDocument.DocsOutItemsList.forEach(function (dataView) {
                                                if (dataView.CurrentDocument != null) {
                                                    if (dataView.CurrentDocument.DocumentOutCopies != null) {
                                                        dataView.CurrentDocument.DocumentOutCopies.forEach(function (copy) {
                                                            documentOutCopies.push(copy);
                                                        });
                                                    }
                                                }
                                            });
                                        }
                                        logAttachments.forEach(function (attachment) {
                                            if (attachment.CommunicationLogId == _this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Id) {
                                                var copy = documentOutCopies.filter(function (d) { return d.Id == attachment.DocumentId; })[0];
                                                var attachmentlog = null;
                                                if (copy != null) {
                                                    if (copy.DocoumentTypeCopyName != _this.SelectedInternalDocument.CurrentDocument.DocumentTypeName) {
                                                        attachmentlog = _this.GetAttachmentList(copy.DocumentTypeCopyNameWithDocumentTypeName, attachment.DocumentId, copy.FileSize, copy.Tenant);
                                                    }
                                                    else {
                                                        attachmentlog = _this.GetAttachmentList(copy.DocoumentTypeCopyName, attachment.DocumentId, copy.FileSize, copy.Tenant);
                                                    }
                                                }
                                                else {
                                                    var documentIn = null;
                                                    if (_this.documentInPMs) {
                                                        documentIn = _this.documentInPMs.filter(function (d) { return d.DocumentId == attachment.DocumentId; })[0];
                                                    }
                                                    if (documentIn != null) {
                                                        attachmentlog = _this.GetAttachmentList(documentIn.DocumentTypeName, attachment.DocumentId, documentIn.FileSize, documentIn.Tenant);
                                                    }
                                                    else if (_this.SelectedInternalDocument.DocsOutTabComponent && _this.SelectedInternalDocument.DocsOutTabComponent.DocumentOuts) {
                                                        var documentout = _this.SelectedInternalDocument.DocsOutTabComponent.DocumentOuts.filter(function (d) { return d.Id == attachment.DocumentId; })[0];
                                                        if (documentout != null) {
                                                            attachmentlog = _this.GetAttachmentList(documentout.DocumentTypeName, attachment.DocumentId, documentout.FileSize, documentout.Tenant);
                                                        }
                                                    }
                                                }
                                                if (attachmentlog) {
                                                    attachmentsLogList.push(attachmentlog);
                                                }
                                            }
                                        });
                                        _this.AttachmentsLists = attachmentsLogList;
                                        if (attachmentsLogList) {
                                            _this.BliudAttachmentList(attachmentsLogList);
                                        }
                                        _this.CurrentSession.StopBusyIndicator();
                                        //end region 
                                    }
                                }
                                else {
                                    _this.CurrentSession.StopBusyIndicator();
                                }
                            }
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                        }
                    });
                }
                else {
                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                        _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                    }
                }
                _this.CurrentSession.StopBusyIndicator();
                _this.ShowBusyIndicator = false;
            });
            //getSentMessageHtmlBody
        }
    };
    //  (copy.DocoumentTypeCopyName, attachment.DocumentId, copy.FileSize, SelectedInternalDocument.Tenant, false);
    SendDocumentComponent.prototype.GetAttachmentList = function (name, documentId, fileSize, tenant) {
        var attachmentlog = new AttachmentsList_1.AttachmentsList();
        attachmentlog.Tenant = tenant;
        attachmentlog.FileSize = fileSize;
        attachmentlog.ShowRemoveLink = false;
        attachmentlog.Id = documentId;
        attachmentlog.DocumentTypeCopyNameWithDocumentTypeName = name;
        return attachmentlog;
    };
    SendDocumentComponent.prototype.LoadHtmlTemplateData = function (templateId) {
        var _this = this;
        if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedDocumentTypeTemplateViewModel.HtmlData);
            this.From = this.SelectedDocumentTypeTemplateViewModel.From;
            this.ReplyTo = this.SelectedDocumentTypeTemplateViewModel.ReplyTo;
            this.Subject = this.SelectedDocumentTypeTemplateViewModel.TemplateSubject;
            this.Cc = this.SelectedDocumentTypeTemplateViewModel.TemplateCc;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Cc))
                this.AddCcClick();
            this.ReloadFroalaEditor();
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            var docoutId = this.DocumentOutId;
            if (templateId)
                docoutId = "";
            if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.Entity) {
                this.From = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedDocumentTypeTemplateViewModel.Entity.From) ? this.SelectedDocumentTypeTemplateViewModel.Entity.From : "";
                this.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedDocumentTypeTemplateViewModel.Entity.ReplyTo) ? this.SelectedDocumentTypeTemplateViewModel.Entity.ReplyTo : "";
                this.Cc = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedDocumentTypeTemplateViewModel.Entity.CC) ? this.SelectedDocumentTypeTemplateViewModel.Entity.CC : "";
            }
            this._htmlEditorService.getEditorHtmlData(docoutId, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant, SessionInfo_1.SessionInfo.LoggedUserId, true, templateId, "", "", this.From, this.ReplyTo, this.Cc).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult.Htmlstring);
                        if (_this.SelectedDocumentTypeTemplateViewModel != null) {
                            _this.SelectedDocumentTypeTemplateViewModel.HtmlData = myResult.Htmlstring;
                            _this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                            _this.SelectedDocumentTypeTemplateViewModel.From = !Tools_1.AppTool.IsNullOrEmpty(myResult.From) ? myResult.From : "";
                            _this.SelectedDocumentTypeTemplateViewModel.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(myResult.ReplyTo) ? myResult.ReplyTo : "";
                            _this.SelectedDocumentTypeTemplateViewModel.TemplateCc = !Tools_1.AppTool.IsNullOrEmpty(myResult.Cc) ? myResult.Cc : "";
                            if (!Tools_1.AppTool.IsNullOrEmpty(myResult.Subject)) {
                                _this.SelectedDocumentTypeTemplateViewModel.TemplateSubject = myResult.Subject;
                            }
                            else {
                                _this.SelectedDocumentTypeTemplateViewModel.TemplateSubject = _this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject != null ? _this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject : _this.SelectedInternalDocument.CurrentDocument.DocumentTypeName;
                            }
                        }
                        _this.From = !Tools_1.AppTool.IsNullOrEmpty(myResult.From) ? myResult.From : "";
                        _this.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(myResult.ReplyTo) ? myResult.ReplyTo : "";
                        _this.Cc = !Tools_1.AppTool.IsNullOrEmpty(myResult.Cc) ? myResult.Cc : "";
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.Cc))
                            _this.AddCcClick();
                        if (!Tools_1.AppTool.IsNullOrEmpty(myResult.Subject)) {
                            _this.Subject = myResult.Subject;
                        }
                        else {
                            _this.Subject = _this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject != null ? _this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject : _this.SelectedInternalDocument.CurrentDocument.DocumentTypeName;
                        }
                        _this.ReloadFroalaEditor();
                    }
                }
                else {
                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                        _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                    }
                }
                _this.CurrentSession.StopBusyIndicator();
                _this.ShowBusyIndicator = false;
            });
        }
    };
    SendDocumentComponent.prototype.LoadDocumentTypeTemplates = function (selectId) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.SelectId = selectId;
        this.ReportTemplates = new Array();
        this.DocumenttypetemplateLists = new Array();
        this._documentTypeTemplateListExtendedService.getDocumentTypeTemplateListsForDocumentType(this.CurrentDocumentType.Id, this.CurrentDocumentType.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var selectId = _this.SelectId ? _this.SelectId : _this.CurrentDocument.EmailTemplateId;
                    myResult.forEach(function (item) {
                        if (item.DocumentTypeId == _this.DocumentTypeId && item.TemplateType == "M") {
                            if (!item.InActive || item.IsDefault || item.Id == selectId) {
                                _this.ReportTemplates.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                            }
                            _this.DocumenttypetemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                        }
                    });
                    _this.Title = "Templates (" + _this.ReportTemplates.length + ")";
                    if (_this.ReportTemplates.length > 0) {
                        if (selectId) {
                            _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (r) { return r.Id == selectId; })[0];
                        }
                        if (!_this.SelectedDocumentTypeTemplateViewModel) {
                            _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates[0];
                        }
                    }
                    if (_this.SelectedDocumentTypeTemplateViewModel != null) {
                        _this.Subject = _this.SelectedDocumentTypeTemplateViewModel.Subject;
                        _this.CurrentDocument.EmailTemplateId = _this.SelectedDocumentTypeTemplateViewModel.Id;
                        _this.LoadHtmlTemplateData(_this.SelectedDocumentTypeTemplateViewModel.Id);
                    }
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }
        });
    };
    SendDocumentComponent.prototype.OnSelectTemplateChange = function (selectedItem) {
        if (selectedItem != this.SelectedDocumentTypeTemplateViewModel) {
            this.SelectedDocumentTypeTemplateViewModel = selectedItem;
            this.CurrentDocument.EmailTemplateId = selectedItem.Id;
            this.LoadHtmlTemplateData(selectedItem.Id);
        }
    };
    SendDocumentComponent.prototype.ShowHideTemplateList = function () {
        if (this.IsShowTemplateList) {
            this.IsShowTemplateList = false;
            this.AttrTitleShowTemplateList = "Expand";
        }
        else {
            this.IsShowTemplateList = true;
            this.AttrTitleShowTemplateList = "Hide";
        }
        var element = document.getElementById(this.AttachmentListId);
        this.ComputeAttachmentListWidth();
        if (this.AttachmentsLists.length > 4) {
            element.setAttribute("style", "height:60px;margin-left:5px;overflow-y:scroll;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }
        else {
            element.setAttribute("style", "height:auto;margin-left:5px;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }
    };
    SendDocumentComponent.prototype.CheckboxClick = function () {
        if (this.IsCheckedInActive) {
            this.IsCheckedInActive = false;
            this.RefreshDocumentTemplateList(false);
        }
        else {
            this.IsCheckedInActive = true;
            this.RefreshDocumentTemplateList(true);
        }
    };
    SendDocumentComponent.prototype.RefreshDocumentTemplateList = function (isactive) {
        var _this = this;
        //ReportTemplates
        // DocumenttypetemplateLists
        if (isactive) {
            this.ReportTemplates = this.DocumenttypetemplateLists;
        }
        else {
            this.ReportTemplates = this.DocumenttypetemplateLists.filter(function (d) { return d.InActive == false || d.IsDefault == true || (_this.SelectedDocumentTypeTemplateViewModel && _this.SelectedDocumentTypeTemplateViewModel.Id == d.Id); });
        }
        this.Title = "Templates (" + this.ReportTemplates.length + ")";
    };
    SendDocumentComponent.prototype.CheckIsValidEmails = function (mailsList) {
        var IsOk = true;
        var EMAIL_REGEXP1 = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach(function (item) {
                if (item) {
                    if (!EMAIL_REGEXP1.test(item)) {
                        IsOk = false;
                        return;
                    }
                    else if (!EMAIL_REGEXP2.test(item)) {
                        IsOk = false;
                        return;
                    }
                }
            });
        }
        if (IsOk) {
        }
        return IsOk;
    };
    SendDocumentComponent.prototype.SendDocumentHtml = function () {
        var _this = this;
        this.IsSendDocumentSucceeded = false;
        this.IsSendDocumentFailed = false;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjecttableName, "SendDocByEmail");
        var filter = new SendHtmlDocumentFilter_1.SendHtmlDocumentFilter();
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Sending...");
        filter.HtmlString = this.froalaEditorSetting.froalaEditorComponent.getHtml();
        var html = filter.HtmlString;
        var start = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body>";
        var end = "</body></html>";
        filter.HtmlString = start + filter.HtmlString + end;
        filter.InternalDocumentId = this.DocumentOutId;
        filter.ToEmail = this.ToEmail;
        filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filter.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
        filter.ObjectTableId = this.ObjectTableId;
        filter.EntityReference = "";
        filter.EntityId = this.EntityId;
        filter.Subject = this.Subject;
        filter.Cc = this.Cc;
        filter.Bcc = this.Bcc;
        filter.Attachments = "";
        filter.ExportQuotationsToIntegratedSystem = this.SelectedInternalDocument.ExportQuotationsToIntegratedSystem;
        filter.ObjectTableName = this.ObjecttableName;
        if (this.SelectedInternalDocument.IsCrm) {
            filter.EventTypeCode = this.SelectedInternalDocument.EventTypeCode;
            filter.CustomerId = this.SelectedInternalDocument.EntityPM ? this.SelectedInternalDocument.EntityPM.CustomerId : "";
            filter.DocumentTypeCode = this.SelectedInternalDocument.DocumentTypePM.Code;
            filter.IsCRM = this.SelectedInternalDocument.IsCrm;
            if (this.SelectedInternalDocument.ObjectTableName == "Customer") {
                filter.CustomerId = this.SelectedInternalDocument.EntityPM ? this.SelectedInternalDocument.EntityPM.Id : "";
                filter.EntityId = "";
            }
            var div = document.createElement("div");
            div.innerHTML = html;
            document.body.appendChild(div);
            filter.HtmlPlainString = GetPlainTextFromHtml(div);
            document.body.removeChild(div);
        }
        filter.From = !Tools_1.AppTool.IsNullOrEmpty(this.From) ? this.From : "";
        filter.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(this.ReplyTo) ? this.ReplyTo : "";
        if (!filter.ToEmail) {
            this.ShowMessage("Please specify at least one recepient", "Logitude Message");
            this.CurrentSession.StopBusyIndicator();
            return;
        }
        if (!this.CheckIsValidEmails(filter.ToEmail)) {
            this.ShowMessage("Some of To e- mails are Invalid", "Logitude Message");
            this.CurrentSession.StopBusyIndicator();
            return;
        }
        if (filter.Cc != null && !this.CheckIsValidEmails(filter.Cc)) {
            this.ShowMessage("Some of Cc e-mails are Invalid", "Logitude Message");
            this.CurrentSession.StopBusyIndicator();
            return;
        }
        if (filter.Bcc != null && !this.CheckIsValidEmails(filter.Bcc)) {
            this.ShowMessage("Some of Bcc e-mails are Invalid", "Logitude Message");
            this.CurrentSession.StopBusyIndicator();
            return;
        }
        var Byte = 1024;
        var totalsize = 0;
        this.AttachmentsLists.forEach(function (item) {
            filter.Attachments += item.Id + ",";
            if (item.FileSize != null) {
                totalsize += item.FileSize / (Byte * Byte);
            }
        });
        if (totalsize > 15) {
            this.ShowMessage("The maximum size of documents you can attach is 15 MB. Please send the documents in separated emails", "Attachment Limit");
            //this.ShowMessage("The file you are trying to send exceeds the 15 MB attachment limit.", "Attachment Limit");
            this.CurrentSession.StopBusyIndicator();
            return;
        }
        this._htmlEditorService.sendDocumentHtml(filter).subscribe(function (res) {
            var response = res;
            if (!response.HasError) {
                _this.IsSendDocumentSucceeded = true;
                _this._communicationLogExtendedPMService.getCommunicationLogPMsByEntityIdAndDocumentOutId(_this.EntityId, _this.CurrentDocument.Id, _this.CurrentDocument.Tenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        _this.CommunicationLogs = new Array();
                        myResult.forEach(function (item) {
                            _this.CommunicationLogs.push(new CommunicationLogPMViewModel_1.CommunicationLogPMViewModel(item));
                        });
                        _this.SelectedInternalDocument.CommunicationLogObsList = _this.CommunicationLogs.filter(function (d) { return d.CurrentEntityPm.DocumentOutId == _this.SelectedInternalDocument.CurrentDocument.Id; });
                        _this._documentOutPMService.getSingleDocumentOutPM(_this.SelectedInternalDocument.CurrentDocument.Id, _this.SelectedInternalDocument.CurrentDocument.Tenant).subscribe(function (res) {
                            var pmResponse = res;
                            if (!pmResponse.HasError) {
                                var updated = pmResponse.Result;
                                if (updated) {
                                    _this.SelectedInternalDocument.CurrentDocument = updated;
                                    _this.SelectedInternalDocument.Issued = true;
                                    _this.SelectedInternalDocument.IssuedByUserName = _this.SelectedInternalDocument.CurrentDocument.IssuedByUserName = updated.IssuedByUserName;
                                    _this.SelectedInternalDocument.IssuedDate = _this.SelectedInternalDocument.CurrentDocument.IssuedDate = updated.IssuedDate;
                                }
                            }
                            if (_this.SelectedInternalDocument.CommunicationLogObsList && _this.SelectedInternalDocument.CommunicationLogObsList.length > 0) {
                                _this.SelectedInternalDocument.HasTree = true;
                                _this.SelectedInternalDocument.SetCommunicationLogListHeight();
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EventRefreshName)) {
                                _this.CurrentSession.FireEvent(_this.EventRefreshName);
                            }
                            _this.CurrentSession.CurrentWindow.Close("SendEnd");
                        });
                    }
                    else {
                        _this.CloseButtonClicked();
                    }
                });
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                _this.IsSendDocumentFailed = true;
                if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                    _this.ShowMessage(response.ErrorsArray[0], "Logitude Message");
                }
            }
        });
    };
    SendDocumentComponent.prototype.CloseButtonClicked = function () {
        //this._documentOutPMService.putDocumentOut(this.CurrentDocument).subscribe(res => {
        //});
        this.CurrentSession.CloseCurrentWindow();
    };
    SendDocumentComponent.prototype.ShowAttachDocsOut = function () {
        var _this = this;
        this.IsEnableLinkDocOout = false;
        this.DocTypeLists = [];
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = this.CurrentDocument.Tenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.DocTypeLists = myResult;
                _this.AttachDocsOut();
            }
        });
    };
    SendDocumentComponent.prototype.AttachDocsOut = function () {
        var _this = this;
        this.IsShowAtachmentDocOut = false;
        this.DocumentCopiesList = new Array();
        //if (this.SelectedInternalDocument.PageRequestSendComponent == "DocOut") {
        //    if (this.SelectedInternalDocument.DocsOutItemsList) {
        //        var DocumentviewmodelList = this.SelectedInternalDocument.DocsOutItemsList;
        //        this.SelectedInternalDocument.DocsOutItemsList.forEach((item) => {
        //            if (item.CurrentDocument != null && item.CurrentDocument.DocumentOutCopies) {
        //                item.CurrentDocument.DocumentOutCopies.forEach((copy) => {
        //                    this.currentDocTypeList = this.DocTypeLists.filter(d=> d.Id == item.CurrentDocument.DocumentTypeId)[0];
        //                    if (this.currentDocTypeList) {
        //                        if (!(this.currentDocTypeList.IsDocumentOneTimePrintLimited == true && this.currentDocTypeList.LimitedPrintCopyId == copy.DocumentTypeCopyId && !AppTool.IsNullOrEmpty(copy.LastPrintedByUserId))) {
        //                            this.DocumentCopiesList.push(new DocumentOutCopyViewModel(copy));
        //                        }
        //                    }
        //                });
        //            }
        //        });
        //    }
        //    this.ViewAttachDocsOut(this.DocumentCopiesList);
        //}
        //else {
        this.DocumentOutLists = new Array();
        var childEntityId = this.ChildEntityId;
        var mychildObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ChildObjectTableId; })[0];
        var childObjectTableName = "";
        if (mychildObjectTable) {
            if (mychildObjectTable.Name == "ARInvoice" || mychildObjectTable.Name == "APInvoice")
                childEntityId = "";
        }
        var table = "";
        if (Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId)) {
            childEntityId = "";
        }
        this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, childEntityId, this.ObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.DocumentOutLists = myResult;
                    _this.DocumentOutLists.forEach(function (documentout) {
                        if (documentout.DocumentOutCopies) {
                            documentout.DocumentOutCopies.forEach(function (copy) {
                                _this.currentDocTypeList = _this.DocTypeLists.filter(function (d) { return d.Id == documentout.DocumentTypeId; })[0];
                                if (_this.currentDocTypeList) {
                                    if (!(_this.currentDocTypeList.IsDocumentOneTimePrintLimited == true && _this.currentDocTypeList.LimitedPrintCopyId == copy.DocumentTypeCopyId && !Tools_1.AppTool.IsNullOrEmpty(copy.LastPrintedByUserId))) {
                                        _this.DocumentCopiesList.push(new DocumentOutCopyViewModel_1.DocumentOutCopyViewModel(copy));
                                    }
                                }
                            });
                        }
                    });
                }
            }
            _this.ViewAttachDocsOut(_this.DocumentCopiesList);
        });
        //}
    };
    SendDocumentComponent.prototype.ViewAttachDocsOut = function (items) {
        if (items.length > 0) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Attach Internal Document";
            logWindow.Width = 800;
            logWindow.Height = 500;
            logWindow.DataContext = this;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachDocsOutComponent");
        }
        else {
            this.ShowMessage("No Internal documents found");
            this.IsEnableLinkDocOout = true;
        }
    };
    SendDocumentComponent.prototype.ShowAttachExternal = function () {
        var _this = this;
        this.IsCloseAttachmentUploader = false;
        this.IsEnableLinkAttachExternal = false;
        var windowArgs = {};
        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.RequsetPageName = "SendControl";
        windowArgs.TiggerViewModel = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 360;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            _this.IsEnableLinkAttachExternal = true;
        });
    };
    SendDocumentComponent.prototype.OnUploadComplete = function (uploader) {
        this.IsEnableLinkAttachExternal = true;
        if (uploader.IsUploadDone && uploader.CurrentDocument) {
            var item = new AttachmentsList_1.AttachmentsList();
            item.Id = uploader.CurrentDocument.DocumentId;
            item.Tenant = uploader.CurrentDocument.Tenant;
            item.FileSize = uploader.CurrentDocument.FileSize;
            item.DocumentTypeCopyNameWithDocumentTypeName = uploader.CurrentDocument.DocumentTypeName;
            item.FileExtension = uploader.CurrentDocument.FileExtension;
            item.ShowRemoveLink = true;
            var attachmentsLists = this.AttachmentsLists;
            if (!attachmentsLists)
                attachmentsLists = new Array();
            attachmentsLists = attachmentsLists.filter(function (d) { return d.Id != item.Id; });
            attachmentsLists.push(item);
            this.BliudAttachmentList(attachmentsLists);
        }
    };
    SendDocumentComponent.prototype.ShowAttachDocsIn = function () {
        var _this = this;
        this.IsEnableLinkDocIn = false;
        var childEntityId = this.ChildEntityId;
        var mychildObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ChildObjectTableId; })[0];
        var childObjectTableName = "";
        if (mychildObjectTable) {
            if (mychildObjectTable.Name == "ARInvoice" || mychildObjectTable.Name == "APInvoice")
                childEntityId = "";
        }
        this._documentsFilingExtendedPMService.getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(this.EntityId, childEntityId, this.ObjectTableId, "I", this.CurrentDocumentType.Tenant, true).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentInPMs = myResult;
                    if (_this.documentInPMs.length > 0) {
                        _this.IsCloseAttachmentDocsIn = false;
                        _this.OnCloseAttachmentDocsInEvent.subscribe(function ($event) {
                            if (!_this.IsCloseAttachmentDocsIn && $event) {
                                _this.IsCloseAttachmentDocsIn = true;
                                _this.IsEnableLinkDocIn = true;
                                _this.BliudAttachmentList($event);
                                if (_this.AttachmentsLists.length != null && _this.AttachmentsLists.length > 0) {
                                    _this.IsShowAttachmentList = true;
                                }
                                _this.ReloadFroalaEditor();
                            }
                        });
                        var windowArgs = {};
                        windowArgs.DocumentsFilingList = _this.documentInPMs;
                        windowArgs.OnCloseAttachmentDocsInEvent = _this.OnCloseAttachmentDocsInEvent;
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 800;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = "Attach Docs In";
                        logitudeWindow.WindowArgs = windowArgs;
                        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentDocsInComponent");
                        logitudeWindow.WindowClosed.subscribe(function ($event) {
                            _this.IsEnableLinkDocIn = true;
                        });
                    }
                    else {
                        _this.ShowMessage("No Docs In found");
                        _this.IsEnableLinkDocIn = true;
                    }
                }
            }
            else
                _this.IsEnableLinkDocIn = true;
        });
    };
    SendDocumentComponent.prototype.ShowDocsSharedWithAgentsAttachment = function () {
        var _this = this;
        if (this.IsEnableLinkDocsSharedWithAgents) {
            this.IsEnableLinkDocsSharedWithAgents = false;
            this.IsCloseAttachDocsShareWithAgents = false;
            this.OnCloseSharedWithAgentsEvent.subscribe(function ($event) {
                if (!_this.IsCloseAttachDocsShareWithAgents && $event) {
                    _this.IsCloseAttachDocsShareWithAgents = true;
                    _this.IsEnableLinkDocsSharedWithAgents = true;
                    _this.BliudAttachmentList($event);
                    if (_this.AttachmentsLists.length != null && _this.AttachmentsLists.length > 0) {
                        _this.IsShowAttachmentList = true;
                    }
                    _this.ReloadFroalaEditor();
                }
            });
            var windowArgs = {};
            windowArgs.OnCloseSharedWithAgentsEvent = this.OnCloseSharedWithAgentsEvent;
            windowArgs.Mode = "Attachment";
            windowArgs.EntityPM = this.SelectedInternalDocument != null ? this.SelectedInternalDocument.EntityPM : null;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 600;
            logWindow.Title = "Attach Documents Shared with Agents";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.IsEnableLinkDocsSharedWithAgents = true;
            });
        }
    };
    SendDocumentComponent.prototype.BliudAttachmentList = function (attachmentsLists, removeHeightList, isChangeHeightFroalaEditor) {
        var _this = this;
        if (removeHeightList === void 0) { removeHeightList = true; }
        if (isChangeHeightFroalaEditor === void 0) { isChangeHeightFroalaEditor = true; }
        if (removeHeightList) {
            this.RemoveHeightAttachmentsListsFromWindow(this);
        }
        if (attachmentsLists.length > 0) {
            attachmentsLists.forEach(function (item) {
                _this.AttachmentsLists = _this.AttachmentsLists.filter(function (d) { return d.Id != item.Id; });
                _this.AttachmentsLists.push(item);
            });
            this.AttachmentsLists = this.AttachmentsLists.reverse();
            this.CreateAttachmentList(isChangeHeightFroalaEditor);
        }
    };
    SendDocumentComponent.prototype.AddBccClick = function () {
        if (!this.IsShowBccBox) {
            this.IsShowBccBox = true;
            if (this.IsShowCcBox) {
                this.IsHideBccCcLinkArea = true;
                this.froalaEditorSetting.Height -= 11;
            }
            else {
                this.froalaEditorSetting.Height -= 26;
            }
            this.ReloadFroalaEditor();
        }
    };
    SendDocumentComponent.prototype.AddCcClick = function () {
        if (!this.IsShowCcBox) {
            this.IsShowCcBox = true;
            if (this.IsShowBccBox) {
                this.IsHideBccCcLinkArea = true;
                this.froalaEditorSetting.Height -= 11;
            }
            else {
                this.froalaEditorSetting.Height -= 26;
            }
            this.ReloadFroalaEditor();
        }
    };
    SendDocumentComponent.prototype.RemoveHeightCcAndBCcFromWindow = function () {
        if (this.IsShowBccBox && this.IsShowCcBox)
            this.froalaEditorSetting.Height += 37;
        else if (this.IsShowCcBox)
            this.froalaEditorSetting.Height += 26;
        else if (this.IsShowBccBox)
            this.froalaEditorSetting.Height += 26;
        this.IsShowBccBox = false;
        this.IsShowCcBox = false;
        this.ReloadFroalaEditor();
    };
    SendDocumentComponent.prototype.RemoveHeightAttachmentsListsFromWindow = function (sendControl) {
        if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 2 && sendControl.AttachmentsLists.length < 5) {
            sendControl.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 22;
        }
        else if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 4) {
            this.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 45;
        }
    };
    SendDocumentComponent.prototype.CreateAttachmentList = function (isChangeHeightFroalaEditor) {
        var _this = this;
        if (isChangeHeightFroalaEditor === void 0) { isChangeHeightFroalaEditor = true; }
        if (this.AttachmentsLists.length > 2 && this.AttachmentsLists.length < 5) {
            if (this.IsSendEditMode)
                this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 22;
            else
                this.froalaEditorSetting.Height = this.froalaEditorSetting.Height - 35;
        }
        else if (this.AttachmentsLists.length > 4) {
            if (this.IsSendEditMode)
                this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 45;
            else
                this.froalaEditorSetting.Height = this.SelectedInternalDocument.WindowHeight - 70;
        }
        if (isChangeHeightFroalaEditor)
            this.ReloadFroalaEditor();
        var element = document.getElementById(this.AttachmentListId);
        element.innerHTML = "";
        this.table = document.createElement("table");
        this.tbdy = document.createElement('tbody');
        this.tbdy2 = document.createElement('tbody');
        this.tr = document.createElement("tr");
        this.tr.setAttribute("style", "height:22px;vertical-align:top;");
        this.td = document.createElement("td");
        this.div = document.createElement("div"); //Create left div
        this.div.setAttribute("style", "width:auto");
        this.table2 = document.createElement("table");
        this.tr2 = document.createElement("tr");
        this.order = 0;
        this.Count = 0;
        this.AttachmentsLists.forEach(function (item) {
            _this.AddAttachment(item);
        });
        var element = document.getElementById(this.AttachmentListId);
        this.ComputeAttachmentListWidth();
        if (this.AttachmentsLists.length > 4) {
            element.setAttribute("style", "height:65px;margin-left:5px;vertical-align:top;overflow-y:scroll;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }
        else {
            element.setAttribute("style", "height:auto;margin-left:5px;vertical-align:top;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }
        this.table2.appendChild(this.tbdy2);
        this.div.appendChild(this.table2);
        this.td.appendChild(this.div);
        this.tr.appendChild(this.td);
        this.tbdy.appendChild(this.tr);
        this.table.appendChild(this.tbdy);
        element.appendChild(this.table);
        //  
    };
    SendDocumentComponent.prototype.AddAttachment = function (item) {
        var _this = this;
        var att = new AttachmentDocment_1.AttachmentDocment(item.DocumentTypeCopyNameWithDocumentTypeName, item.FileSize, item.Id, this.order++);
        this._documentExtendedService.GetDocumentById(item.Id, item.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentPM = myResult;
                    att.FileSize = _this.documentPM.FileSize;
                    att.FileName = !Tools_1.AppTool.IsNullOrEmpty(_this.documentPM.CalculatedFileName) ? _this.documentPM.CalculatedFileName : item.DocumentTypeCopyNameWithDocumentTypeName;
                    _this.CreateAttachment(att, _this.documentPM, item.ShowRemoveLink, _this);
                }
            }
        });
    };
    SendDocumentComponent.prototype.CreateAttachment = function (att, documentPM, showRemoveLink, SendControl) {
        this.Count += 1;
        var name = att.FileName;
        name += "." + documentPM.Extension;
        if (!SendControl.IsSendEditMode)
            name += " , ";
        var cellText = document.createTextNode(name);
        var div = document.createElement("div");
        div.setAttribute("style", "width:auto;text-align:left;font-weight:bold;height:22px;");
        div.appendChild(cellText);
        if (SendControl.IsSendEditMode) {
            var a = document.createElement('a');
            var linkText = document.createTextNode("[Remove],");
            a.appendChild(linkText);
            a.title = " [Remove]";
            a.id = att.DocumentId;
            a.setAttribute("style", "width:auto;margin-left:5px");
            a.onclick = function () {
                SendControl.RemoveHeightAttachmentsListsFromWindow(SendControl);
                SendControl.AttachmentsLists = SendControl.AttachmentsLists.filter(function (d) { return d.Id != a.id; });
                if (SendControl.AttachmentsLists != null && SendControl.AttachmentsLists.length > 0) {
                    htmlComponentProparitiesTrue(a);
                    SendControl.CreateAttachmentList();
                }
                else {
                    var element = document.getElementById(SendControl.AttachmentListId);
                    element.innerHTML = "";
                    htmlComponentProparitiesFalse(a);
                    //this.IsShowAttachmentList = false;
                }
            };
            div.appendChild(a);
        }
        if (this.CountTd == 2) {
            var td = document.createElement("td");
            td.setAttribute("style", "width:auto;text-align:left;");
            td.appendChild(div);
            var tdspace = document.createElement("td");
            tdspace.setAttribute("style", "width:10px;text-align:left;");
            this.tr2.appendChild(tdspace);
            this.tr2.appendChild(td);
            this.tbdy2.appendChild(this.tr2);
            this.tr2 = document.createElement("tr");
            this.tr2.setAttribute("style", "height:22px;vertical-align:top;");
            this.CountTd = 1;
        }
        else {
            var td = document.createElement("td");
            td.setAttribute("style", "width:30px;text-align:left;");
            td.appendChild(div);
            this.tr2.appendChild(td);
            if (this.Count == this.AttachmentsLists.length) {
                this.tbdy2.appendChild(this.tr2);
                this.tr2 = document.createElement("tr");
                this.tr2.setAttribute("style", "height:22px;vertical-align:top;");
                this.CountTd = 1;
            }
            else {
                this.CountTd = this.CountTd + 1;
            }
        }
        this.IsShowAttachmentList = true;
    };
    SendDocumentComponent.prototype.ShowHtmlDocumentPreview = function (item) {
        var _this = this;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Send";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = item.Tenant;
        windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.EntityId = this.EntityId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Html Template";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.RefreshTemplateId = $event;
            if (_this.RefreshTemplateId) {
                if (!_this.SelectedDocumentTypeTemplateViewModel)
                    _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (d) { return d.Id == _this.RefreshTemplateId; })[0];
                if (_this.SelectedDocumentTypeTemplateViewModel) {
                    if (_this.SelectedDocumentTypeTemplateViewModel.Id != _this.RefreshTemplateId)
                        _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (d) { return d.Id == _this.RefreshTemplateId; })[0];
                }
                if (_this.SelectedDocumentTypeTemplateViewModel) {
                    var documentTemplate = _this.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == _this.SelectedDocumentTypeTemplateViewModel.Id; })[0];
                    if (documentTemplate) {
                        _this.SelectedDocumentTypeTemplateViewModel.Entity = documentTemplate;
                        _this.SelectedDocumentTypeTemplateViewModel.From = documentTemplate.From;
                        _this.SelectedDocumentTypeTemplateViewModel.ReplyTo = documentTemplate.ReplyTo;
                        _this.SelectedDocumentTypeTemplateViewModel.Subject = documentTemplate.Subject;
                        _this.SelectedDocumentTypeTemplateViewModel.TemplateTechnologyCode = documentTemplate.TemplateTechnologyCode;
                    }
                    _this.ShowBusyIndicator = true;
                    _this.CurrentSession.StartBusyIndicatorLoading();
                    _this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
                    _this.LoadHtmlTemplateData(_this.RefreshTemplateId);
                }
                else {
                    _this.LoadDocumentTypeTemplates(_this.RefreshTemplateId);
                }
            }
        });
    };
    SendDocumentComponent.prototype.SetTemplateAsDeflut = function (selectitem) {
        if (!selectitem.InActive) {
            if (selectitem != null) {
                if (!this.IsTemplateDefualt(selectitem)) {
                    if (!this.IsSend) {
                        if (this.CurrentDocumentType.TemplateFormatCode == "P" && selectitem.TemplateType == "P") {
                            this.CurrentDocumentType.DocumentTypeDefaultReportTemplateId = selectitem.Id;
                            this.CurrentDocumentType.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                        }
                        else if (this.CurrentDocumentType.TemplateFormatCode == "M" && selectitem.TemplateType == "M") {
                            this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId = selectitem.Id;
                            this.CurrentDocumentType.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                        }
                    }
                    else {
                        if (selectitem.TemplateType == "M") {
                            this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId = selectitem.Id;
                            this.CurrentDocumentType.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                        }
                    }
                    this.ReportTemplates.forEach(function (template) {
                        if (template.Id != selectitem.Id) {
                            template.IsDefault = false;
                        }
                        else {
                            template.IsDefault = true;
                        }
                    });
                    this._documentTypePMService.putDocumentType(this.CurrentDocumentType).subscribe(function (res) {
                    });
                }
            }
        }
        else {
            this.ShowMessage("Please note that you can't set an inactive template as default");
        }
        //////End////
    };
    SendDocumentComponent.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    SendDocumentComponent.prototype.SetTemplateAsInactive = function (selectitem) {
        var _this = this;
        if (selectitem != null) {
            if (!this.IsTemplateDefualt(selectitem)) {
                if (!selectitem.InActive) {
                    selectitem.InActive = true;
                    selectitem.LableSetactive = "Mark as active";
                }
                else {
                    selectitem.InActive = false;
                    selectitem.LableSetactive = "Mark as inactive";
                }
                var item = this.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == selectitem.Id; })[0];
                if (item) {
                    item.InActive = selectitem.InActive;
                    this.UpdateDocumentTypeTemplate(item);
                }
                else {
                    this.documentTypeTemplatePMService.get(selectitem.Id).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                myResult.InActive = selectitem.InActive;
                                _this.DocumentTypeTemplatePMLists.push(myResult);
                                _this.UpdateDocumentTypeTemplate(myResult);
                            }
                        }
                    });
                }
            }
            else {
                this.ShowMessage("Please note that you can't Inactive the default template, please change the default template first");
            }
        }
    };
    SendDocumentComponent.prototype.IsTemplateDefualt = function (selectitem) {
        var IsDefualt = false;
        if (selectitem.TemplateType == "P") {
            if (selectitem.Id == this.CurrentDocumentType.DocumentTypeDefaultReportTemplateId) {
                IsDefualt = true;
            }
        }
        else if (selectitem.TemplateType == "M") {
            if (selectitem.Id == this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId) {
                IsDefualt = true;
            }
        }
        return IsDefualt;
    };
    SendDocumentComponent.prototype.UpdateDocumentTypeTemplate = function (item) {
        this.documentTypeTemplatePMService.update(item).subscribe(function (myResult) {
        });
    };
    SendDocumentComponent.prototype.ReloadFroalaEditor = function () {
        if (this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    };
    SendDocumentComponent.prototype.OpenEmailsBox = function () {
        var _this = this;
        if (!this.IsOpenWidnow) {
            this.IsOpenWidnow = true;
            if (!this.PartnersObslist) {
                var childEntityId = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedInternalDocument.ChildEntityId) ? this.SelectedInternalDocument.ChildEntityId : "";
                var childObjectTableName = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedInternalDocument.ChildObjectTableName) ? this.SelectedInternalDocument.ChildObjectTableName : "";
                this._documentOutPMService.GetEntityPartners(this.EntityId, this.ObjecttableName, childEntityId, childObjectTableName).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        _this.PartnersObslist = [];
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            myResult.forEach(function (item) {
                                _this.PartnersObslist.push(new EntityPartner_1.EntityPartner(item.PartnerType, item.PartnerId, item.IsUser));
                            });
                        }
                        _this.ShowSendToEmail();
                    }
                    else {
                        _this.IsOpenWidnow = false;
                    }
                });
            }
            else {
                this.ShowSendToEmail();
            }
        }
    };
    SendDocumentComponent.prototype.ShowSendToEmail = function () {
        var _this = this;
        this.IsCloseSendToContact = false;
        this.OnCloseSendToContactsEvent.subscribe(function ($event) {
            if (!_this.IsCloseSendToContact && $event) {
                _this.IsCloseSendToContact = true;
                _this.ToEmail = "";
                _this.Cc = "";
                _this.Bcc = "";
                _this.RemoveHeightCcAndBCcFromWindow();
                _this.IsHideBccCcLinkArea = false;
                if ($event.ToEmailLists && $event.ToEmailLists.length > 0) {
                    $event.ToEmailLists.forEach(function (item) { _this.ToEmail += item + ";"; });
                }
                if ($event.CcEmailLists && $event.CcEmailLists.length > 0) {
                    $event.CcEmailLists.forEach(function (item) { _this.Cc += item + ";"; });
                    if (!_this.IsShowCcBox)
                        _this.AddCcClick();
                }
                else
                    _this.IsShowCcBox = false;
                if ($event.BccEmailLists && $event.BccEmailLists.length > 0) {
                    $event.BccEmailLists.forEach(function (item) { _this.Bcc += item + ";"; });
                    if (!_this.IsShowBccBox)
                        _this.AddBccClick();
                }
                else
                    _this.IsShowBccBox = false;
                if (!_this.Bcc && !_this.Cc) {
                    _this.IsShowBccBox = false;
                    _this.IsShowCcBox = false;
                    _this.IsHideBccCcLinkArea = false;
                    _this.ReloadFroalaEditor();
                }
            }
        });
        var windowArgs = {};
        windowArgs.PartnersObslist = this.PartnersObslist;
        windowArgs.ToEmail = this.ToEmail;
        windowArgs.Cc = this.Cc;
        windowArgs.Bcc = this.Bcc;
        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Contacts List";
        logWindow.Width = window.innerWidth - 100;
        logWindow.Height = window.innerHeight - 100;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SendMessageContacts/SendToContactsComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.IsOpenWidnow = false;
        });
    };
    SendDocumentComponent.prototype.OnWindowClosed = function () {
        this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
    };
    SendDocumentComponent.prototype.ComputeAttachmentListWidth = function () {
        if (!this.IsShowTemplateList || window.innerWidth > 1200) {
            var width = window.innerWidth - 400;
            if (width > 700) {
                this.AreaAttachmentWidth = "600px";
            }
            else {
                this.AreaAttachmentWidth = (width + "px");
            }
        }
        else {
            var width = window.innerWidth - 700;
            if (width > 700) {
                this.AreaAttachmentWidth = "600px";
            }
            else {
                this.AreaAttachmentWidth = width + "px";
            }
        }
    };
    SendDocumentComponent.prototype.AddTemplateFromLibrary = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe(function (response) {
            _this.IsDisableAddTemplateFromLibrary = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            var windowArgs = {};
            windowArgs.DataViewModel = _this;
            windowArgs.ObjectTableId = _this.ObjectTableId;
            windowArgs.EntityId = _this.EntityId;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("DocumentTypeTemplate.O.NewHTMLTemplate");
            if (_this.CurrentDocumentType) {
                windowArgs.CurrentDocumentType = _this.CurrentDocumentType;
            }
            if (_this.SelectedInternalDocument) {
                windowArgs.ChildEntityId = _this.ChildEntityId;
            }
            windowArgs.ChildObjectTableId = ""; //this.ChildObjectTableId;
            windowArgs.PageRequest = "Send";
            logWindow.Width = 1000;
            logWindow.Height = 550;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeTemplateFromLibraryComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.IsDisableAddTemplateFromLibrary = false;
            });
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SendDocumentComponent.prototype, "OnCloseAttachmentDocsInEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SendDocumentComponent.prototype, "OnCloseSendToContactsEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SendDocumentComponent.prototype, "OnCloseSharedWithAgentsEvent", void 0);
    SendDocumentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SendDocument',
            templateUrl: './SendControl.html',
            providers: [CommunicationLogExtendedPMService_1.CommunicationLogExtendedPMService, CommunicationAttachmentExtendedPMService_1.CommunicationAttachmentExtendedPMService, DocumentOutPMService_1.DocumentOutPMService, ServiceArgs_1.ServiceArgs, DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, HtmlEditorService_1.HtmlEditorService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService, DocumentExtendedService_1.DocumentExtendedService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, DocumentTypeListService_1.DocumentTypeListService],
            inputs: ['selectedInternalDocument']
        }),
        __metadata("design:paramtypes", [CommunicationLogExtendedPMService_1.CommunicationLogExtendedPMService, CommunicationAttachmentExtendedPMService_1.CommunicationAttachmentExtendedPMService, DocumentOutPMService_1.DocumentOutPMService, DocumentExtendedService_1.DocumentExtendedService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService, DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, HtmlEditorService_1.HtmlEditorService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, core_1.ChangeDetectorRef, DocumentTypeListService_1.DocumentTypeListService])
    ], SendDocumentComponent);
    return SendDocumentComponent;
}());
exports.SendDocumentComponent = SendDocumentComponent;
//# sourceMappingURL=SendDocumentComponent.js.map