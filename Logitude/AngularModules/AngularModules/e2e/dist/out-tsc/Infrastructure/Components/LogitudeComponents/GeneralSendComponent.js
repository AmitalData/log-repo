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
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SendHtmlDocumentFilter_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/SendHtmlDocumentFilter");
var FroalaEditorSetting_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting");
var DocumentOutPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var DocumentOutCopyViewModel_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentOutCopyViewModel");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var DocumentTypeListExtendedService_1 = require("../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var AttachmentsList_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var EntityPartner_1 = require("../../../Infrastructure/DataContracts/EntityPartner");
var HtmlEditorService_1 = require("../../../Common/Services/DocumentServices/HtmlEditorService");
var AttachmentDocment_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/AttachmentDocment");
var DocumentExtendedService_1 = require("../../../Common/Services/ExtendedPMs/DocumentExtendedService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../Infrastructure/Tools");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var ReportPMService_1 = require("../../../Common/Services/StandardPMs/ReportPMService");
var ReportsTemplatePMService_1 = require("../../../Common/Services/StandardPMs/ReportsTemplatePMService");
var ReportsTemplatePMExtendedService_1 = require("../../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService");
var GeneralSendComponent = /** @class */ (function () {
    function GeneralSendComponent(_documentOutPMService, _documentTypeListExtendedService, _documentExtendedService, _documentsFilingExtendedPMService, _htmlEditorService, cd) {
        this._documentOutPMService = _documentOutPMService;
        this._documentTypeListExtendedService = _documentTypeListExtendedService;
        this._documentExtendedService = _documentExtendedService;
        this._documentsFilingExtendedPMService = _documentsFilingExtendedPMService;
        this._htmlEditorService = _htmlEditorService;
        this.cd = cd;
        this.IsShowAddReportTemplateButton = false;
        this.IsUserFromReport = false;
        this.Subject = null;
        this.ToEmail = "";
        this.OnCloseAttachmentDocsInEvent = new core_1.EventEmitter();
        this.OnCloseSendToContactsEvent = new core_1.EventEmitter();
        this.Bcc = "";
        this.Cc = "";
        this.IsShowCcBox = false;
        this.IsShowBccBox = false;
        this.IsHideBccCcLinkArea = false;
        this.IsEnableLinkAttachExternal = true;
        this.IsEnableLinkDocOout = true;
        this.IsEnableLinkDocIn = true;
        this.AreaAttachmentWidth = "600px";
        this.CountTd = 1;
        this.Count = 0;
        this.IsShowTemplateList = false;
        this.IsShowTemplateArea = false;
        this.IsCheckedInActive = false;
        this.AttrTitleShowTemplateList = "Expand";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.order = 0;
        this.IsCloseSendToContact = false;
        this.IsOpenWidnow = false;
        this.IsHideAttachmentLinkArea = false;
        this.RefreshTemplateId = "";
        this.AttachmentListId = Guid_1.Guid.newGuid();
        this.AttachmentsLists = new Array();
        this.ShowInactiveCheckBoxKey = Guid_1.Guid.newGuid();
    }
    GeneralSendComponent.prototype.ngOnInit = function () {
    };
    GeneralSendComponent.prototype.ngAfterViewInit = function () {
        if (this.AttachmentsLists) {
            this.BliudAttachmentList(this.AttachmentsLists, false);
        }
    };
    GeneralSendComponent.prototype.GetAttachmentList = function (name, documentId, fileSize, tenant) {
        var attachmentlog = new AttachmentsList_1.AttachmentsList();
        attachmentlog.Tenant = tenant;
        attachmentlog.FileSize = fileSize;
        attachmentlog.ShowRemoveLink = false;
        attachmentlog.Id = documentId;
        attachmentlog.DocumentTypeCopyNameWithDocumentTypeName = name;
        return attachmentlog;
    };
    GeneralSendComponent.prototype.CheckIsValidEmails = function (mailsList) {
        var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var IsOk = true;
        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach(function (item) {
                if (item) {
                    if (!EMAIL_REGEXP.test(item)) {
                        IsOk = false;
                        return;
                    }
                }
            });
        }
        return IsOk;
    };
    GeneralSendComponent.prototype.SendDocumentHtml = function () {
        var _this = this;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjecttableName, "SendDocByEmail");
        var filter = new SendHtmlDocumentFilter_1.SendHtmlDocumentFilter();
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Sending...");
        filter.HtmlString = this.froalaEditorSetting.froalaEditorComponent.getHtml();
        filter.InternalDocumentId = this.DocumentOutId ? this.DocumentOutId : null;
        filter.ExternalDocumentId = this.DocumentFilingId ? this.DocumentFilingId : null;
        filter.ReplyTo = this.ReplyTo ? this.ReplyTo : "";
        filter.From = this.From ? this.From : "";
        filter.ToEmail = this.ToEmail ? this.ToEmail : "";
        filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filter.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
        filter.ObjectTableId = this.ObjectTableId;
        filter.EntityReference = this.EntityReference ? this.EntityReference : "";
        filter.EntityId = this.EntityId;
        filter.Subject = this.Subject ? this.Subject : "";
        filter.Cc = this.Cc ? this.Cc : "";
        filter.Bcc = this.Bcc ? this.Bcc : "";
        filter.Attachments = "";
        if (!filter.ToEmail) {
            this.ShowMessage("Please specify at least one recepient", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }
        if (!this.CheckIsValidEmails(filter.ToEmail)) {
            this.ShowMessage("some of To e- mails are Invalid", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }
        if (filter.Cc != null && !this.CheckIsValidEmails(filter.Cc)) {
            this.ShowMessage("Some of Cc e-mails are Invalid", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }
        if (filter.Bcc != null && !this.CheckIsValidEmails(filter.Bcc)) {
            this.ShowMessage("Some of Bcc e-mails are Invalid", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }
        var Byte = 1024;
        var totalsize = 0;
        if (this.AttachmentsLists) {
            this.AttachmentsLists.forEach(function (item) {
                filter.Attachments += item.Id + ",";
                if (item.FileSize != null) {
                    totalsize += item.FileSize / (Byte * Byte);
                }
            });
        }
        if (totalsize > 15) {
            this.ShowMessage("The maximum size of documents you can attach is 15 MB. Please send the documents in separated emails", "Attachment Limit");
            //   this.ShowMessage("The file you are trying to send exceeds the 15 MB attachment limit.", "Attachment Limit");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }
        this._htmlEditorService.sendDocumentHtml(filter).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
                _this.CloseButtonClicked();
            }
            else {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }
        });
    };
    GeneralSendComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    GeneralSendComponent.prototype.ShowAttachDocsOut = function () {
        var _this = this;
        this.IsEnableLinkDocOout = false;
        if (!this.DocTypeLists) {
            this._documentTypeListExtendedService.getDocumentTypesListByObjectTableAndTenant(SessionInfo_1.SessionInfo.LoggedUserTenant, this.ObjectTableId).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.DocTypeLists = myResult;
                        _this.AttachDocsOut(_this.DocTypeLists);
                    }
                    else
                        _this.IsEnableLinkDocOout = true;
                }
            });
        }
        else {
            this.AttachDocsOut(this.DocTypeLists);
        }
    };
    GeneralSendComponent.prototype.AttachDocsOut = function (DocTypeLists) {
        var _this = this;
        this.IsShowAtachmentDocOut = false;
        this.DocumentCopiesList = new Array();
        this.DocumentOutLists = new Array();
        this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, this.ObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.DocumentOutLists = myResult;
                    _this.DocumentOutLists.forEach(function (documentout) {
                        if (documentout.DocumentOutCopies) {
                            documentout.DocumentOutCopies.forEach(function (copy) {
                                _this.currentDocTypeList = _this.DocTypeLists.filter(function (d) { return d.Id == documentout.DocumentTypeId; })[0];
                                if (_this.currentDocTypeList && _this.currentDocTypeList.IsDocumentOneTimePrintLimited == false && _this.currentDocTypeList.LimitedPrintCopyId != copy.DocumentTypeCopyId && copy.LastPrintedByUserId == null) {
                                    _this.DocumentCopiesList.push(new DocumentOutCopyViewModel_1.DocumentOutCopyViewModel(copy));
                                }
                            });
                        }
                    });
                }
            }
            _this.ViewAttachDocsOut(_this.DocumentCopiesList);
        });
    };
    GeneralSendComponent.prototype.ViewAttachDocsOut = function (items) {
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
    GeneralSendComponent.prototype.ShowAttachExternal = function () {
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
    GeneralSendComponent.prototype.OnUploadComplete = function (uploader) {
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
    GeneralSendComponent.prototype.ShowAttachDocsIn = function () {
        var _this = this;
        this.IsEnableLinkDocIn = false;
        this._documentsFilingExtendedPMService.getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, this.ObjectTableId, "I", SessionInfo_1.SessionInfo.LoggedUserTenant, true).subscribe(function (res) {
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
    GeneralSendComponent.prototype.BliudAttachmentList = function (attachmentsLists, removeHeightList) {
        var _this = this;
        if (removeHeightList === void 0) { removeHeightList = true; }
        if (removeHeightList) {
            this.RemoveHeightAttachmentsListsFromWindow(this);
        }
        if (attachmentsLists.length > 0) {
            attachmentsLists.forEach(function (item) {
                var attach = _this.AttachmentsLists.filter(function (d) { return d.Id == item.Id; })[0];
                if (attach == null) {
                    _this.AttachmentsLists.push(item);
                }
            });
            this.AttachmentsLists = this.AttachmentsLists.reverse();
            this.CreateAttachmentList();
        }
        else {
        }
    };
    GeneralSendComponent.prototype.AddBccClick = function () {
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
    GeneralSendComponent.prototype.AddCcClick = function () {
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
    GeneralSendComponent.prototype.RemoveHeightCcAndBCcFromWindow = function () {
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
    GeneralSendComponent.prototype.RemoveHeightAttachmentsListsFromWindow = function (sendControl) {
        if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 2 && sendControl.AttachmentsLists.length < 5) {
            sendControl.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 22;
        }
        else if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 4) {
            this.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 45;
        }
    };
    GeneralSendComponent.prototype.CreateAttachmentList = function () {
        var _this = this;
        if (this.AttachmentsLists.length > 2 && this.AttachmentsLists.length < 5) {
            this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 22;
        }
        else if (this.AttachmentsLists.length > 4) {
            this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 45;
        }
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
    GeneralSendComponent.prototype.AddAttachment = function (item) {
        var _this = this;
        var att = new AttachmentDocment_1.AttachmentDocment(item.DocumentTypeCopyNameWithDocumentTypeName, item.FileSize, item.Id, this.order++);
        this._documentExtendedService.GetDocumentById(item.Id, item.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentPM = myResult;
                    att.FileSize = _this.documentPM.FileSize;
                    att.FileName = !Tools_1.AppTool.IsNullOrEmpty(_this.documentPM.CalculatedFileName) ? _this.documentPM.CalculatedFileName : _this.documentPM.FileName;
                    _this.CreateAttachment(att, _this.documentPM, item.ShowRemoveLink, _this);
                }
            }
        });
    };
    GeneralSendComponent.prototype.CreateAttachment = function (att, documentPM, showRemoveLink, SendControl) {
        this.Count += 1;
        var name = att.FileName;
        name += "." + documentPM.Extension;
        var cellText = document.createTextNode(name);
        var div = document.createElement("div");
        div.setAttribute("style", "width:auto;text-align:left;font-weight:bold;height:22px;");
        div.appendChild(cellText);
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
    GeneralSendComponent.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    GeneralSendComponent.prototype.ReloadFroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    };
    GeneralSendComponent.prototype.OpenEmailsBox = function () {
        var _this = this;
        if (!this.IsOpenWidnow) {
            this.IsOpenWidnow = true;
            if (!this.PartnersObslist) {
                this._documentOutPMService.GetEntityPartners(this.EntityId, this.ObjecttableName).subscribe(function (res) {
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
    GeneralSendComponent.prototype.ShowSendToEmail = function () {
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
        windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;
        windowArgs.IsUserFromReport = this.IsUserFromReport;
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
    GeneralSendComponent.prototype.OnWindowClosed = function () {
        this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
    };
    GeneralSendComponent.prototype.ComputeAttachmentListWidth = function () {
        if (window.innerWidth > 1200) {
            this.AreaAttachmentWidth = "600px";
        }
    };
    GeneralSendComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.WindowHeight = args.WindowHeight;
        this.AttachmentsLists = args.Attachments;
        this.ChildObjectTableId = args.ChildObjectTableId;
        this.ChildEntityId = args.ChildEntityId;
        this.ChildEntityReference = args.ChildEntityReference;
        this.EntityReference = args.EntityReference;
        this.IsUserFromReport = args.IsUserFromReport;
        this.DocumentFilingId = args.DocumentFilingId;
        this.EntityId = args.EntityId;
        this.ObjecttableName = args.ObjecttableName;
        this.ObjectTableId = args.ObjectTableId;
        this.Subject = args.Subject;
        this.ReplyTo = args.ReplyTo;
        this.From = args.From;
        this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
        this.froalaEditorSetting.PageType = "Send";
        this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
        this.froalaEditorSetting.Height = this.WindowHeight - 203;
        if (this.EntityReference == "StimualReport") {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ReportsTemplate", "REPORTTEMPLATEMESSAGE"))
                this.IsShowAddReportTemplateButton = true;
            this.IsHideAttachmentLinkArea = true;
            this.EntityReference = "";
            this.PartnersObslist = args.PartnersObslist;
            this.reportsTemplatePMService = new ReportsTemplatePMService_1.ReportsTemplatePMService();
            this.reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService_1.ReportsTemplatePMExtendedService();
            this.reportPMService = new ReportPMService_1.ReportPMService();
            this.IsShowTemplateArea = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            this.reportPMService.get(this.EntityId).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    _this.EntityPM = pmResponse.Result;
                    _this.LoadTemplateLists(null);
                }
            });
        }
    };
    //Template List Area
    GeneralSendComponent.prototype.ShowHideTemplateList = function () {
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
    GeneralSendComponent.prototype.OnSelectTemplateChange = function (selectedItem) {
        if (selectedItem != this.SelectedTemplate) {
            this.SelectedTemplate = selectedItem;
            this.LoadHtmlTemplateData(selectedItem.Id, selectedItem.CurrentVersion);
        }
    };
    GeneralSendComponent.prototype.LoadHtmlTemplateData = function (id, version) {
        var _this = this;
        if (this.SelectedTemplate != null && this.SelectedTemplate.IsLoad) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedTemplate.HtmlData);
            this.From = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.From) ? this.SelectedTemplate.From : "";
            this.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.ReplyTo) ? this.SelectedTemplate.ReplyTo : "";
            this.Cc = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.Cc) ? this.SelectedTemplate.Cc : "";
            this.Subject = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.Subject) ? this.SelectedTemplate.Subject : "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Cc))
                this.AddCcClick();
            this.ReloadFroalaEditor();
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            this.From = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.From) ? this.SelectedTemplate.EntityPM.From : "";
            this.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.ReplyTo) ? this.SelectedTemplate.EntityPM.ReplyTo : "";
            this.Cc = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.CC) ? this.SelectedTemplate.EntityPM.CC : "";
            this.Subject = !Tools_1.AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.Subject) ? this.SelectedTemplate.EntityPM.Subject : "";
            this.reportsTemplatePMExtendedService.GetReportTemplateEditorHtmlData(id, version, SessionInfo_1.SessionInfo.LoggedUserId, this.Subject, this.From, this.ReplyTo, this.Cc).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.From = _this.SelectedTemplate.From = !Tools_1.AppTool.IsNullOrEmpty(myResult.From) ? myResult.From : "";
                        _this.ReplyTo = _this.SelectedTemplate.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(myResult.ReplyTo) ? myResult.ReplyTo : "";
                        _this.Cc = _this.SelectedTemplate.Cc = !Tools_1.AppTool.IsNullOrEmpty(myResult.Cc) ? myResult.Cc : "";
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.Cc))
                            _this.AddCcClick();
                        if (!Tools_1.AppTool.IsNullOrEmpty(myResult.Subject)) {
                            _this.Subject = _this.SelectedTemplate.Subject = myResult.Subject;
                        }
                        else {
                            _this.Subject = _this.SelectedTemplate.Subject = _this.SelectedTemplate.Description;
                        }
                        _this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult.Htmlstring);
                        if (_this.SelectedTemplate != null) {
                            _this.SelectedTemplate.HtmlData = myResult.Htmlstring;
                            _this.SelectedTemplate.IsLoad = true;
                        }
                        _this.ReloadFroalaEditor();
                    }
                }
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });
        }
    };
    GeneralSendComponent.prototype.LoadTemplateLists = function (selectId) {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.SelectId = selectId;
        this.ReportTemplates = new Array();
        this.AllReportTemplates = new Array();
        this.reportsTemplatePMExtendedService.GetReportsTemplatePMsByReportId(this.EntityPM.Id, "M").subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach(function (item) {
                        if (!item.InActive || item.IsDefault) {
                            _this.ReportTemplates.push(new TemplateClassData(item, _this.EntityPM));
                        }
                        _this.AllReportTemplates.push(new TemplateClassData(item, _this.EntityPM));
                    });
                    _this.Title = "Templates (" + _this.ReportTemplates.length + ")";
                    if (_this.ReportTemplates.length > 0) {
                        if (_this.SelectId) {
                            _this.SelectedTemplate = _this.ReportTemplates.filter(function (r) { return r.Id == _this.SelectId; })[0];
                        }
                        else
                            _this.SelectedTemplate = _this.ReportTemplates.filter(function (r) { return r.Id == _this.EntityPM.DefaultMessageTemplateId; })[0];
                        if (!_this.SelectedTemplate)
                            _this.SelectedTemplate = _this.ReportTemplates[0];
                    }
                    if (_this.SelectedTemplate != null) {
                        _this.Subject = _this.SelectedTemplate.Description;
                        _this.LoadHtmlTemplateData(_this.SelectedTemplate.Id, _this.SelectedTemplate.CurrentVersion);
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
    GeneralSendComponent.prototype.SetTemplateAsDeflut = function (selectitem) {
        if (!selectitem.InActive) {
            if (selectitem != null) {
                if (!this.IsTemplateDefualt(selectitem)) {
                    this.EntityPM.DefaultMessageTemplateId = selectitem.Id;
                    this.UpdateReportPM();
                }
            }
        }
        else {
            this.ShowMessage("Please note that you can't set an inactive template as default");
        }
    };
    GeneralSendComponent.prototype.SetTemplateAsInactive = function (selectitem) {
        var _this = this;
        if (selectitem != null) {
            if (!this.IsTemplateDefualt(selectitem)) {
                if (!selectitem.InActive)
                    selectitem.InActive = true;
                else
                    selectitem.InActive = false;
                var item = this.AllReportTemplates.filter(function (d) { return d.Id == selectitem.Id; })[0];
                if (item) {
                    item.InActive = selectitem.InActive;
                    this.UpdateReportTemplatePM(item.EntityPM);
                }
                else {
                    this.reportsTemplatePMService.get(selectitem.Id).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                myResult.InActive = selectitem.InActive;
                                _this.UpdateReportTemplatePM(myResult);
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
    GeneralSendComponent.prototype.EditTemplate = function (item, isNew) {
        var _this = this;
        if (isNew === void 0) { isNew = false; }
        var windowArgs = {};
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "ReportTemplate";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = item.EntityPM.Tenant;
        windowArgs.ObjectType = "ReportsTemplatePM";
        windowArgs.ReportTemplatePM = item.EntityPM;
        windowArgs.IsNewEntity = isNew;
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
                if (!_this.SelectedTemplate)
                    _this.SelectedTemplate = _this.ReportTemplates.filter(function (d) { return d.Id == _this.RefreshTemplateId; })[0];
                if (_this.SelectedTemplate) {
                    if (_this.SelectedTemplate.Id != _this.RefreshTemplateId)
                        _this.SelectedTemplate = _this.ReportTemplates.filter(function (d) { return d.Id == _this.RefreshTemplateId; })[0];
                }
                if (_this.SelectedTemplate) {
                    _this.SelectedTemplate.IsLoad = false;
                    _this.LoadHtmlTemplateData(_this.SelectedTemplate.Id, _this.SelectedTemplate.CurrentVersion);
                }
                else {
                    _this.LoadTemplateLists(_this.RefreshTemplateId);
                }
            }
        });
    };
    GeneralSendComponent.prototype.CheckboxClick = function () {
        if (this.IsCheckedInActive) {
            this.IsCheckedInActive = false;
            this.RefreshTemplateList(false);
        }
        else {
            this.IsCheckedInActive = true;
            this.RefreshTemplateList(true);
        }
    };
    GeneralSendComponent.prototype.RefreshTemplateList = function (isactive) {
        if (isactive) {
            this.ReportTemplates = this.AllReportTemplates;
        }
        else {
            this.ReportTemplates = this.AllReportTemplates.filter(function (d) { return d.InActive == false; });
        }
        this.Title = "Templates (" + this.ReportTemplates.length + ")";
    };
    GeneralSendComponent.prototype.IsTemplateDefualt = function (selectitem) {
        var IsDefualt = false;
        if (selectitem.Id == this.EntityPM.DefaultMessageTemplateId) {
            IsDefualt = true;
        }
        return IsDefualt;
    };
    GeneralSendComponent.prototype.UpdateReportPM = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.reportPMService.update(this.EntityPM).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    _this.EntityPM = result;
                }
            }
        });
    };
    GeneralSendComponent.prototype.UpdateReportTemplatePM = function (item) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.reportsTemplatePMService.update(item).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    item = result;
                }
            }
        });
    };
    GeneralSendComponent.prototype.AddReportTemplateButtonClicked = function () {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.TemplateType = "M";
        windowArgs.Area = "GeneralSendComponent";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.Title = "New Message Template";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Report/Components/NewReportsTemplateComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
        });
    };
    GeneralSendComponent.prototype.BuildViewModel = function (item) {
        return new TemplateClassData(item, this.EntityPM);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], GeneralSendComponent.prototype, "OnCloseAttachmentDocsInEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], GeneralSendComponent.prototype, "OnCloseSendToContactsEvent", void 0);
    GeneralSendComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'GeneralSendControl',
            templateUrl: './GeneralSendComponent.html',
            providers: [DocumentOutPMService_1.DocumentOutPMService, DocumentTypeListExtendedService_1.DocumentTypeListExtendedService, HtmlEditorService_1.HtmlEditorService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService, DocumentExtendedService_1.DocumentExtendedService],
        }),
        __metadata("design:paramtypes", [DocumentOutPMService_1.DocumentOutPMService, DocumentTypeListExtendedService_1.DocumentTypeListExtendedService, DocumentExtendedService_1.DocumentExtendedService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService, HtmlEditorService_1.HtmlEditorService, core_1.ChangeDetectorRef])
    ], GeneralSendComponent);
    return GeneralSendComponent;
}());
exports.GeneralSendComponent = GeneralSendComponent;
var TemplateClassData = /** @class */ (function () {
    function TemplateClassData(entityPM, report) {
        this.entityPM = entityPM;
        this.IsLoad = false;
        this.HtmlData = "";
        this.Subject = "";
        this.Cc = "";
        this.ReplyTo = "";
        this.From = "";
        this.ReportPM = report;
        this.EntityPM = entityPM;
        this.Id = entityPM.Id;
        this.CurrentVersion = this.EntityPM.CurrentVersion;
    }
    Object.defineProperty(TemplateClassData.prototype, "Description", {
        get: function () {
            var description = "";
            if (this.EntityPM) {
                description = this.EntityPM.Description;
            }
            return description;
        },
        set: function (newValue) {
            if (this.EntityPM.description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TemplateClassData.prototype, "InActive", {
        get: function () {
            var inActive = false;
            if (this.EntityPM) {
                inActive = this.EntityPM.InActive;
            }
            return inActive;
        },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TemplateClassData.prototype, "LableSetactive", {
        get: function () {
            var lableSetactive = "Mark as inactive";
            if (this.EntityPM) {
                if (this.EntityPM.InActive) {
                    lableSetactive = "Mark as active";
                }
            }
            return lableSetactive;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TemplateClassData.prototype, "IsDefault", {
        get: function () {
            var isDefault = false;
            ;
            if (this.EntityPM) {
                if (this.EntityPM.Id == this.ReportPM.DefaultMessageTemplateId) {
                    isDefault = true;
                }
            }
            return isDefault;
        },
        enumerable: true,
        configurable: true
    });
    return TemplateClassData;
}());
exports.TemplateClassData = TemplateClassData;
//# sourceMappingURL=GeneralSendComponent.js.map