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
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var DocumentTypeTemplateViewModel_1 = require("./DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FroalaEditorSetting_1 = require("./DocsOut/FroalaEditorSetting");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var DocumentTypeTemplateFilter_1 = require("./DocsOut/Filters/DocumentTypeTemplateFilter");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var HtmlEditorService_1 = require("../../../../Common/Services/DocumentServices/HtmlEditorService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ReportsTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService");
var HtmlDocumentPreviewComponent = /** @class */ (function () {
    function HtmlDocumentPreviewComponent(_documentTypeTemplatePMExtendedService, cd, _htmlEditorService) {
        this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        this.cd = cd;
        this._htmlEditorService = _htmlEditorService;
        this.DocumentTemplateFileId = Guid_1.Guid.NewRandomString();
        this.IsShowUploadAndDownloadButtons = false;
        this.IsShowAreaDataField = true;
        this.IsPreviewMode = false;
        this.LableSaveButton = "Save";
        this.IsShowButtonCancel = true;
        this.EntityId = "";
        this.ChildEntityId = "";
        this.ChildObjectTableId = "";
        this.IsShowHeaderAndFooterButton = false;
        this.Mode = "Preview";
        this.ObjectType = "PM";
        this.IsFillData = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsOpenHeaderAndFooter = false;
        this.OldDataTemplateByte = null;
        this.IsDownLoadButtonClick = false;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
    }
    HtmlDocumentPreviewComponent.prototype.ngOnInit = function () {
    };
    HtmlDocumentPreviewComponent.prototype.ngAfterViewInit = function () {
        if (this.IsFillData && this.template) {
            //if (this.froalaEditorSetting.froalaEditorComponent) {
            //    this.froalaEditorSetting.froalaEditorComponent.ResourcesLoaded.subscribe(s => {
            //        //this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            //    });
            //}
        }
    };
    HtmlDocumentPreviewComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
        this.DataViewModel = args.DataViewModel;
        this.PageType = args.PageType;
        this.TemplateId = args.TemplateId;
        this.Tenant = args.Tenant;
        this.EntityId = args.EntityId;
        this.ObjectTableId = args.ObjectTableId ? args.ObjectTableId : "";
        this.ChildEntityId = args.ChildEntityId ? args.ChildEntityId : "";
        this.ChildObjectTableId = args.ChildObjectTableId ? args.ChildObjectTableId : "";
        if (args.ObjectType)
            this.ObjectType = args.ObjectType;
        this.TemplatePMLists = args.DocumentTypeTemplatePMLists;
        if (this.TemplatePMLists) {
            if (this.ObjectType == "DocumentTypeTemplateViewModel") {
                var docViewModel = this.TemplatePMLists.filter(function (d) { return d.Id == _this.TemplateId; })[0];
                if (docViewModel) {
                    this.template = docViewModel.Entity;
                }
            }
            else {
                this.template = this.TemplatePMLists.filter(function (d) { return d.Id == _this.TemplateId; })[0];
            }
        }
        else
            this.TemplatePMLists = [];
        if (this.TemplateId)
            this.Run(args);
    };
    HtmlDocumentPreviewComponent.prototype.Run = function (args) {
        var _this = this;
        this.froalaEditorSetting.PageType = "HtmlDocumentPreview";
        this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
        this.SubjectId = Guid_1.Guid.newGuid();
        this.FromId = Guid_1.Guid.newGuid();
        this.ReplyToId = Guid_1.Guid.newGuid();
        this.CCId = Guid_1.Guid.newGuid();
        this.IsShowButtonSaveAs = true;
        this.froalaEditorSetting.IsDisableEdit = false;
        if (this.PageType) {
            // Preview
            if (this.PageType == "Preview") {
                this.froalaEditorSetting.Height = window.innerHeight - 190;
                this.froalaEditorSetting.IsDisableEdit = true;
                this.IsShowButtonSaveAs = false;
                this.IsShowButtonCancel = false;
                this.IsShowAreaDataField = false;
                this.IsPreviewMode = true;
                this.froalaEditorSetting.IsDisableEdit = true;
                this.LableSaveButton = "Close";
            }
            // Edit 
            else if (this.PageType == "Maintenance") {
                this.froalaEditorSetting.Height = window.innerHeight - 310;
                this.IsShowButtonSaveAs = false;
                this.Mode = "Edit";
                this.IsShowUploadAndDownloadButtons = true;
            }
            else if (this.PageType == "Send" || this.PageType == "ManageTemplate") {
                this.froalaEditorSetting.Height = window.innerHeight - 310;
                if (this.PageType == "ManageTemplate")
                    this.IsShowHeaderAndFooterButton = true;
                this.Mode = "Edit";
                this.IsShowUploadAndDownloadButtons = true;
            }
            // Signature
            else if (this.PageType == "Signature") {
                this.IsShowAreaDataField = false;
                this.IsShowButtonSaveAs = false;
                this.froalaEditorSetting.Height = window.innerHeight - 240;
            }
            //Report Template
            else if (this.PageType == "ReportTemplate") {
                this.IsShowAreaDataField = true;
                this.IsShowUploadAndDownloadButtons = true;
                if (args.ReportComponentArea == "Maintenance") {
                    this.IsShowButtonSaveAs = false;
                }
                this.froalaEditorSetting.Height = window.innerHeight - 310;
            }
        }
        this.froalaEditorSetting.Height = this.froalaEditorSetting.Height - 20;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        if (this.IsPreviewMode) {
            this.LoadHtmlTemplateData();
        }
        else {
            if (this.template) {
                this.FillData();
            }
            else {
                if (this.PageType == "ReportTemplate") {
                    this.template = args.ReportTemplatePM;
                    if (this.template) {
                        this.FillProp();
                        if (args.IsNewEntity) {
                            if (this.template.TemplateData && this.template.TemplateData.length > 0) {
                                var htmlBody = Base64ToString(this.template.TemplateData);
                                this.froalaEditorSetting.HtmlString = htmlBody;
                                if (this.froalaEditorSetting.froalaEditorComponent) {
                                    this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlBody);
                                    this.ReloadFroalaEditor();
                                }
                            }
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                        else
                            this.LoadReportTemplateDate();
                    }
                    else
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                }
                else if (this.PageType == "Signature") {
                    this.LoadSignatureData();
                }
                else {
                    this._documentTypeTemplatePMExtendedService.GetSingleDocumentTypeTemplate(this.TemplateId, this.Tenant).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                _this.template = myResult;
                                if (_this.TemplatePMLists) {
                                    var template = _this.TemplatePMLists.filter(function (d) { return d.Id == _this.template.Id; })[0];
                                    if (!template) {
                                        if (_this.ObjectType == "DocumentTypeTemplateViewModel") {
                                            _this.TemplatePMLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(_this.template));
                                        }
                                        else
                                            _this.TemplatePMLists.push(_this.template);
                                    }
                                }
                                _this.FillData();
                            }
                        }
                    });
                }
            }
        }
    };
    HtmlDocumentPreviewComponent.prototype.LoadReportTemplateDate = function () {
        var _this = this;
        var reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService_1.ReportsTemplatePMExtendedService();
        reportsTemplatePMExtendedService.GetMessageReportsTemplateBodyByReportTemplateIdAndVersion(this.template.Id, this.template.CurrentVersion).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                var htmlBody = "";
                if (myResult && myResult.length > 0) {
                    htmlBody = Base64ToString(myResult);
                    _this.froalaEditorSetting.HtmlString = htmlBody;
                    if (_this.froalaEditorSetting.froalaEditorComponent) {
                        _this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlBody);
                        _this.ReloadFroalaEditor();
                    }
                }
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    };
    HtmlDocumentPreviewComponent.prototype.LoadSignatureData = function () {
        var _this = this;
        this._documentTypeTemplatePMExtendedService.GetTemplateBodyhtmlOrJsonByDocumentTemplateId(this.TemplateId, this.Tenant, true, this.PageType).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    if (!myResult)
                        myResult = "";
                    if (_this.froalaEditorSetting.froalaEditorComponent) {
                        _this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult);
                        _this.ReloadFroalaEditor();
                    }
                }
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    HtmlDocumentPreviewComponent.prototype.FillData = function () {
        if (this.template) {
            this.TemplateHeaderHtml = this.template.TemplateHeaderHtml;
            this.TemplateFooterHtml = this.template.TemplateFooterHtml;
            this.TemplateFooterHeight = this.template.TemplateFooterHeight;
            this.TemplateHeaderHeight = this.template.TemplateHeaderHeight;
            if (this.template && Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableId)) {
                this.ObjectTableId = this.template.ObjectTableId;
            }
            if (this.PageType == "Maintenance" || this.PageType == "Send" || this.PageType == "ManageTemplate") {
                if (this.template.TemplateType == "P")
                    this.IsShowHeaderAndFooterButton = true;
                else {
                    if ((this.template.TemplateHeaderHtml || this.template.TemplateFooterHtml))
                        this.IsShowHeaderAndFooterButton = true;
                }
            }
            this.DocumentTypeCode = this.template.DocumentTypeCode;
            var htmlBody = "";
            if (this.template.TemplateBodyHtml) {
                htmlBody = Base64ToString(this.template.TemplateBodyHtml);
            }
            else
                htmlBody = "";
            this.froalaEditorSetting.HtmlString = htmlBody;
            if (this.froalaEditorSetting.froalaEditorComponent) {
                this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlBody);
            }
            else {
                this.IsFillData = true;
            }
            this.Subject = this.template.Subject;
            if (this.Mode == "Edit") {
                this.FillProp();
            }
            this.ReloadFroalaEditor();
            if (this.froalaEditorSetting.froalaEditorComponent) {
                this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            }
            else {
                this.OldDataTemplateByte = StringToBase64(htmlBody);
            }
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    HtmlDocumentPreviewComponent.prototype.FillProp = function () {
        if (this.template) {
            this.Subject = !Tools_1.AppTool.IsNullOrEmpty(this.template.Subject) ? this.template.Subject : "";
            this.From = !Tools_1.AppTool.IsNullOrEmpty(this.template.From) ? this.template.From : "";
            this.ReplyTo = !Tools_1.AppTool.IsNullOrEmpty(this.template.ReplyTo) ? this.template.ReplyTo : "";
            this.CC = !Tools_1.AppTool.IsNullOrEmpty(this.template.CC) ? this.template.CC : "";
            if (this.From) {
                this.IsShowFromInputBox = true;
                this.froalaEditorSetting.Height -= 30;
            }
            if (this.ReplyTo) {
                this.IsShowReplyToInputBox = true;
                this.froalaEditorSetting.Height -= 30;
            }
            if (this.CC) {
                this.IsShowCCInputBox = true;
                this.froalaEditorSetting.Height -= 30;
            }
        }
    };
    HtmlDocumentPreviewComponent.prototype.LoadHtmlTemplateData = function () {
        var _this = this;
        this._htmlEditorService.getEditorHtmlData("", this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant, SessionInfo_1.SessionInfo.LoggedUserId, false, this.TemplateId, this.Subject).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult.Htmlstring);
                    _this.ReloadFroalaEditor();
                    _this.OldDataTemplateByte = StringToBase64(_this.froalaEditorSetting.froalaEditorComponent.getHtml());
                }
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    HtmlDocumentPreviewComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.DestroyfroalaEditor();
        var id = "";
        if (this.template) {
            id = this.template.Id;
        }
        this.CurrentSession.CurrentWindow.Close(id);
    };
    HtmlDocumentPreviewComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.DestroyfroalaEditor();
        this.CurrentSession.CurrentWindow.Close("");
    };
    HtmlDocumentPreviewComponent.prototype.DestroyfroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
        }
    };
    HtmlDocumentPreviewComponent.prototype.SaveReportTemplateData = function () {
        var _this = this;
        var reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService_1.ReportsTemplatePMExtendedService();
        this.template.TemplateData = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
        reportsTemplatePMExtendedService.SaveReportTemplateMessageBody(this.template).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.template = myResult;
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            _this.CloseButtonClicked();
        });
    };
    HtmlDocumentPreviewComponent.prototype.SaveSignatureData = function () {
        var _this = this;
        var filter = new DocumentTypeTemplateFilter_1.DocumentTypeTemplateFilter();
        filter.Id = this.TemplateId;
        filter.Tenant = this.Tenant;
        filter.Body = this.froalaEditorSetting.froalaEditorComponent.getHtml();
        filter.TemplateType = "HTML";
        filter.Subject = this.Subject;
        filter.Processtype = this.PageType;
        this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe(function (res) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.CloseButtonClicked();
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }
        });
    };
    HtmlDocumentPreviewComponent.prototype.AddCCLinkClick = function () {
        this.IsShowCCInputBox = true;
        this.froalaEditorSetting.Height -= 30;
        this.ReloadFroalaEditor();
    };
    HtmlDocumentPreviewComponent.prototype.AddReplyToLinkClick = function () {
        this.IsShowReplyToInputBox = true;
        this.froalaEditorSetting.Height -= 30;
        this.ReloadFroalaEditor();
    };
    HtmlDocumentPreviewComponent.prototype.AddFromLinkClick = function () {
        this.IsShowFromInputBox = true;
        this.froalaEditorSetting.Height -= this.IsShowReplyToInputBox ? 15 : 30;
        this.ReloadFroalaEditor();
    };
    HtmlDocumentPreviewComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        if (this.PageType != "Preview") {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            if (this.PageType == "Signature") {
                this.SaveSignatureData();
            }
            else {
                if (!this.CheckIsValidEmail(this.From)) {
                    this.ShowMessage("From email is Invalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }
                if (!this.CheckIsValidEmail(this.ReplyTo)) {
                    this.ShowMessage("Reply-to email is iInvalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }
                if (!this.CheckIsValidEmails(this.CC)) {
                    this.ShowMessage("Some of Cc e-mails are Invalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }
                this.template.Subject = this.Subject;
                this.template.From = this.From;
                this.template.ReplyTo = this.ReplyTo;
                this.template.CC = this.CC;
                if (this.PageType == "ReportTemplate") {
                    this.SaveReportTemplateData();
                }
                else {
                    this.template.TemplateBodyHtml = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
                    this.template.TemplateTechnologyCode = "AG";
                    this.template.TemplateHeaderHtml = this.TemplateHeaderHtml;
                    this.template.TemplateHeaderHeight = this.TemplateHeaderHeight;
                    this.template.TemplateFooterHtml = this.TemplateFooterHtml;
                    this.template.TemplateFooterHeight = this.TemplateFooterHeight;
                    this.documentTypeTemplatePMService.update(this.template).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            var template = myResult;
                            if (_this.ObjectType == "DocumentTypeTemplateViewModel") {
                                template = new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(myResult);
                            }
                            if (_this.TemplatePMLists) {
                                _this.TemplatePMLists = _this.TemplatePMLists.filter(function (d) { return d.Id != _this.TemplateId; });
                                _this.TemplatePMLists.push(template);
                            }
                            _this.CloseButtonClicked();
                        }
                        else {
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                            }
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    });
                }
            }
        }
        else {
            this.CloseButtonClicked();
        }
    };
    HtmlDocumentPreviewComponent.prototype.ReloadFroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    };
    HtmlDocumentPreviewComponent.prototype.SavaAsButtonClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 150;
        logWindow.DataContext = this;
        logWindow.Title = "Save as Template";
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SaveAsTemplateComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                _this.CurrentSession.CurrentWindow.Close($event);
            }
        });
    };
    HtmlDocumentPreviewComponent.prototype.AddDataField = function (type) {
        var _this = this;
        var tableName = "";
        var tableId = !Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId;
        var table = window.ObjectTables.filter(function (d) { return d.Id == tableId; })[0];
        if (table)
            tableName = table.Name;
        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(function (response) {
            if (table) {
                _this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(function (response) {
                    _this.ViewDataField(type, _this.objecttypeField, tableId);
                });
            }
            else
                _this.ViewDataField(type, "", tableId);
        });
    };
    HtmlDocumentPreviewComponent.prototype.ViewHeaderAndFooter = function (type) {
        var windowArgs = {};
        this.IsOpenHeaderAndFooter = true;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.DocumentTypeTemplatePM = this.template;
        windowArgs.PageRequse = this.PageType;
        windowArgs.DataViewModel = this;
        windowArgs.Mode = "Edit";
        windowArgs.PageType = type;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = window.innerWidth - 200;
        logWindow.Height = 325;
        logWindow.Title = type;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HeaderAndFooterComponent');
    };
    HtmlDocumentPreviewComponent.prototype.ViewDataField = function (type, objectTypeField, tableId) {
        var _this = this;
        var windowArgs = {};
        windowArgs.ObjectTableId = tableId;
        windowArgs.ObjectTypeField = objectTypeField;
        windowArgs.InSertDataFieldType = type;
        windowArgs.DocumentTypeCode = this.DocumentTypeCode;
        if (this.PageType == "Signature")
            windowArgs.ObjectTableId = null;
        this.InSertDataFieldType = type;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        logWindow.Title = "Insert Data Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                if (type == "Subject") {
                    _this.Subject = insertAtSubject(_this.SubjectId, $event);
                }
                else if (type == "From") {
                    _this.From = $event;
                }
                else if (type == "ReplyTo") {
                    _this.ReplyTo = $event;
                }
                else if (type == "CC") {
                    if (_this.CC && $event) {
                        _this.CC += ";";
                    }
                    _this.CC += $event;
                }
                else if (type == "FroalaEditor") {
                    _this.froalaEditorSetting.froalaEditorComponent.InSertHtml($event);
                    _this.ReloadFroalaEditor();
                }
            }
        });
    };
    HtmlDocumentPreviewComponent.prototype.DownloadButtonClicked = function () {
        var _this = this;
        if (!this.IsDownLoadButtonClick) {
            this.IsDownLoadButtonClick = true;
            var templateByte = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            if (this.PageType != "ReportTemplate") {
                this.template.TemplateBodyHtml = templateByte;
                this.documentTypeTemplatePMService.update(this.template).subscribe(function (res) {
                    _this.IsOpenHeaderAndFooter = false;
                    _this.IsDownLoadButtonClick = false;
                    _this.OldDataTemplateByte = templateByte;
                    _this.DownloadTemplate();
                });
            }
            else {
                var reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService_1.ReportsTemplatePMExtendedService();
                this.template.TemplateData = templateByte;
                reportsTemplatePMExtendedService.SaveReportTemplateMessageBody(this.template).subscribe(function (res) {
                    _this.IsOpenHeaderAndFooter = false;
                    _this.IsDownLoadButtonClick = false;
                    _this.OldDataTemplateByte = templateByte;
                    _this.DownloadTemplate();
                });
            }
            //if (templateByte != this.OldDataTemplateByte || this.IsOpenHeaderAndFooter) {
            //    var confirmWindow: ConfirmWindow = new ConfirmWindow();
            //    confirmWindow.Width = 400;
            //    confirmWindow.Show("You have unsaved changes, Please save your work first!");
            //    confirmWindow.YesButtonText = "Save";
            //    confirmWindow.NoButtonText = "Cancel";
            //    confirmWindow.WindowClosed.subscribe((event: any) => {
            //        if (confirmWindow.Yes) {
            //            this.CurrentSession.StartBusyIndicatorSaving();
            //            if (this.PageType != "ReportTemplate") {
            //                this.template.TemplateBodyHtml = templateByte;
            //                this.documentTypeTemplatePMService.update(this.template).subscribe(res => {
            //                    this.IsOpenHeaderAndFooter = false;
            //                    this.IsDownLoadButtonClick = false;
            //                    this.OldDataTemplateByte = templateByte;
            //                    this.CurrentSession.StopBusyIndicator();
            //                });
            //            }
            //            else {
            //                var reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
            //                this.template.TemplateData = templateByte;
            //                reportsTemplatePMExtendedService.SaveReportTemplateMessageBody(this.template).subscribe(res => {
            //                    this.IsOpenHeaderAndFooter = false;
            //                    this.IsDownLoadButtonClick = false;
            //                    this.OldDataTemplateByte = templateByte;
            //                    this.CurrentSession.StopBusyIndicator();
            //                });
            //            }
            //        }
            //        if (confirmWindow.No) {
            //            this.IsDownLoadButtonClick = false;
            //        }
            //    });
            //}
            //else {
            //    this.DownloadTemplate();
            //}
        }
    };
    HtmlDocumentPreviewComponent.prototype.DownloadTemplate = function () {
        if (this.template) {
            var fileName = this.template.Description;
            var id = this.template.Id;
            var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
            var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DownloadTemplateDocumentPage.aspx?id=" + id + "&tempId=" + token + "&fileName=" + fileName + "&type=" + this.PageType;
            window.open(url);
        }
        this.IsDownLoadButtonClick = false;
    };
    HtmlDocumentPreviewComponent.prototype.UploadButtonClicked = function () {
        document.getElementById(this.DocumentTemplateFileId).click();
    };
    HtmlDocumentPreviewComponent.prototype.UpLoadTemplateFileMethod = function (event) {
        var file = querySelection(this.DocumentTemplateFileId);
        if (file) {
            this.ArrayBufferToBase64(file, this);
        }
    };
    HtmlDocumentPreviewComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            if (viewmodel && binary) {
                viewmodel._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(window.btoa(binary)).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            var htmltemplate = myResult;
                            if (htmltemplate) {
                                htmltemplate.HeaderHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                htmltemplate.FooterHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";
                                var html = htmltemplate.BodyHtml;
                                if (viewmodel.PageType == "ReportTemplate") {
                                    html = htmltemplate.HeaderHtml + html + htmltemplate.FooterHtml;
                                }
                                if (viewmodel.froalaEditorSetting.froalaEditorComponent) {
                                    viewmodel.froalaEditorSetting.froalaEditorComponent.SetHtml(html);
                                    viewmodel.ReloadFroalaEditor();
                                }
                                if (viewmodel.PageType != "ReportTemplate") {
                                    if (viewmodel.PageType == "Maintenance" || viewmodel.PageType == "Send" || viewmodel.PageType == "ManageTemplate") {
                                        if ((!Tools_1.AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) || !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml)))
                                            viewmodel.IsShowHeaderAndFooterButton = true;
                                    }
                                    viewmodel.TemplateHeaderHtml = StringToBase64(htmltemplate.HeaderHtml);
                                    viewmodel.TemplateFooterHtml = StringToBase64(htmltemplate.FooterHtml);
                                    viewmodel.TemplateHeaderHeight = htmltemplate.HeaderHeight;
                                    viewmodel.TemplateFooterHeight = htmltemplate.FooterHeight;
                                }
                            }
                        }
                    }
                });
            }
        };
        reader.onerror = function (e) {
        };
        reader.readAsArrayBuffer(file);
    };
    HtmlDocumentPreviewComponent.prototype.CheckIsValidEmail = function (email) {
        var IsOk = true;
        var EMAIL_REGEXP1 = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (email) {
            if (email.charAt(0) != "[" || email.charAt(email.length - 1) != "]" || email.indexOf("][") > -1) {
                if (email) {
                    if (!EMAIL_REGEXP1.test(email)) {
                        IsOk = false;
                        return;
                    }
                    else if (!EMAIL_REGEXP2.test(email)) {
                        IsOk = false;
                        return;
                    }
                }
            }
        }
        return IsOk;
    };
    HtmlDocumentPreviewComponent.prototype.CheckIsValidEmails = function (mailsList) {
        var IsOk = true;
        var EMAIL_REGEXP1 = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach(function (item) {
                if (item) {
                    if (item.charAt(0) != "[" || item.charAt(item.length - 1) != "]" || item.indexOf("][") > -1) {
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
                    }
                }
            });
        }
        if (IsOk) {
        }
        return IsOk;
    };
    HtmlDocumentPreviewComponent.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    HtmlDocumentPreviewComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'HtmlDocumentPreview',
            templateUrl: './HtmlDocumentPreviewComponent.html',
            providers: [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, HtmlEditorService_1.HtmlEditorService]
        }),
        __metadata("design:paramtypes", [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, core_1.ChangeDetectorRef, HtmlEditorService_1.HtmlEditorService])
    ], HtmlDocumentPreviewComponent);
    return HtmlDocumentPreviewComponent;
}());
exports.HtmlDocumentPreviewComponent = HtmlDocumentPreviewComponent;
//# sourceMappingURL=HtmlDocumentPreviewComponent.js.map