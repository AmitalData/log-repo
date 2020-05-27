"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ApiQueryFilters_1 = require("../DataContracts/ApiQueryFilters");
var DocumentTypeListService_1 = require("../../Common/Services/StandardLists/DocumentTypeListService");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var DocsOutDataViewModel_1 = require("../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel");
var DocumentOutPMService_1 = require("../../Common/Services/ExtendedPMs/DocumentOutPMService");
var DocumentTypePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var GeneralEmailSender = /** @class */ (function () {
    function GeneralEmailSender(objecttablename, documentTypeCode, entityId, entityReference, childEntityId, childEntityReference, documentFilingId, subject, attachments, eventRefreshName, entityPM, isCrm, eventTypeCode, fromMail) {
        if (eventRefreshName === void 0) { eventRefreshName = null; }
        if (entityPM === void 0) { entityPM = null; }
        if (isCrm === void 0) { isCrm = false; }
        if (eventTypeCode === void 0) { eventTypeCode = null; }
        if (fromMail === void 0) { fromMail = null; }
        this.ChildObjectTableId = "";
        this.ToSpecificeEmail = "";
        this.LoadingSendingComponent = false;
        this.LoadingSendingComponent = true;
        this.CurrentObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == objecttablename; })[0].Id;
        this.ObjectTableName = window.ObjectTables.filter(function (d) { return d.Name == objecttablename; })[0].Name;
        this.EventRefreshName = eventRefreshName;
        this.CurrentEntityId = entityId;
        this.ChildEntityId = childEntityId;
        this.ChildEntityReference = childEntityReference;
        this.Attachments = attachments;
        this.EntityReference = entityReference;
        this.Subject = subject;
        this.Replyto = "";
        this.From = fromMail != null ? fromMail : "";
        this.DocumentFilingId = documentFilingId;
        this.DocumentTypeCode = documentTypeCode;
        this.EntityPM = entityPM;
        this.IsCrm = isCrm;
        this.EventTypeCode = eventTypeCode;
        this.ToSpecificeEmail = "";
        this.ExportQuotationsToIntegratedSystem = false;
    }
    GeneralEmailSender.prototype.SendMessage = function () {
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
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.WindowHeight = sendWindowHeight;
        windowArgs.Attachments = this.Attachments;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId ? this.ChildObjectTableId : "";
        windowArgs.ChildEntityId = this.ChildEntityId ? this.ChildEntityId : "";
        windowArgs.ChildEntityReference = this.ChildEntityReference ? this.ChildEntityReference : "";
        windowArgs.EntityReference = this.EntityReference ? this.EntityReference : "";
        windowArgs.DocumentFilingId = this.DocumentFilingId ? this.DocumentFilingId : "";
        windowArgs.EntityId = this.CurrentEntityId ? this.CurrentEntityId : "";
        windowArgs.ObjecttableName = this.ObjectTableName ? this.ObjectTableName : "";
        windowArgs.ObjectTableId = this.CurrentObjectTableId ? this.CurrentObjectTableId : "";
        windowArgs.Subject = this.Subject ? this.Subject : "";
        windowArgs.ToEmail = this.Replyto ? this.Replyto : "";
        windowArgs.From = this.From ? this.From : "";
        windowArgs.PartnersObslist = this.PartnersObslist;
        windowArgs.IsUserFromReport = this.PartnersObslist ? true : false;
        windowArgs.EntityPM = this.EntityPM;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = sendWindowWidth;
        logWindow.Height = sendWindowHeight;
        logWindow.Title = "Send Message";
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./Infrastructure/Components/LogitudeComponents/GeneralSendComponent");
        this.LoadingSendingComponent = false;
    };
    GeneralEmailSender.prototype.ShowFullSendControll = function () {
        var _this = this;
        if (this.EntityPM) {
            var documentTypeListService = new DocumentTypeListService_1.DocumentTypeListService();
            var documentOutPM = null;
            var documentTypeList;
            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    documentTypeList = myResult.filter(function (d) { return d.Code == _this.DocumentTypeCode; })[0];
                    if (documentTypeList) {
                        var documentOutPMService = new DocumentOutPMService_1.DocumentOutPMService();
                        documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(_this.CurrentEntityId, SessionInfo_1.SessionInfo.LoggedUserTenant, "", documentTypeList.Id).subscribe(function (res) {
                            var pmResponse = res;
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                documentOutPM = myResult;
                                if (!documentOutPM) {
                                    documentOutPMService.getCreateDocumentOut(documentTypeList.Id, _this.CurrentEntityId, "", "", _this.CurrentObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                                        var pmResponse = res;
                                        if (!pmResponse.HasError) {
                                            var myResult = pmResponse.Result;
                                            if (myResult) {
                                                documentOutPM = myResult;
                                                _this.LoadDocumentTypePm(documentOutPM, documentTypeList);
                                            }
                                        }
                                        else
                                            _this.LoadingSendingComponent = false;
                                    });
                                }
                                else {
                                    _this.LoadDocumentTypePm(documentOutPM, documentTypeList);
                                }
                            }
                            else
                                _this.LoadingSendingComponent = false;
                        });
                    }
                    else {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show("Document Type with code (" + _this.DocumentTypeCode + ") is not found");
                    }
                }
                else
                    _this.LoadingSendingComponent = false;
            });
        }
        else
            this.LoadingSendingComponent = false;
    };
    GeneralEmailSender.prototype.LoadDocumentTypePm = function (documentOutPM, documentTypeList) {
        var _this = this;
        var documentTypePM = null;
        this.documentTypePMService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        this.documentTypePMService.getSingleDocumentType(documentTypeList.Id, documentOutPM.Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    documentTypePM = myResult;
                    _this.LoadSendDocumentComponent(documentTypePM, documentOutPM, documentTypeList);
                }
            }
            else
                _this.LoadingSendingComponent = false;
        });
    };
    GeneralEmailSender.prototype.LoadSendDocumentComponent = function (documentTypePM, documentOutPM, documentTypeList) {
        var SelectedInternalDocument = new DocsOutDataViewModel_1.DocsOutDataViewModel(documentTypePM, this.CurrentEntityId, documentOutPM.ChildEntityId, this.CurrentObjectTableId, this.ChildObjectTableId, documentOutPM.ChildEntityReference, null, null, null, this.EntityPM);
        SelectedInternalDocument.CurrentDocument = documentOutPM;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, documentTypeList.Name + " Sending");
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
        SelectedInternalDocument.PageRequestSendComponent == "GeneralComponent";
        SelectedInternalDocument.EntityId = this.CurrentEntityId ? this.CurrentEntityId : "";
        SelectedInternalDocument.IsSend = true;
        SelectedInternalDocument.TemplateType = "M";
        SelectedInternalDocument.ModeSendDocument = "Edit";
        SelectedInternalDocument.IsCrm = this.IsCrm;
        SelectedInternalDocument.EventTypeCode = this.EventTypeCode;
        SelectedInternalDocument.Subject = this.Subject;
        SelectedInternalDocument.EventRefreshName = this.EventRefreshName;
        SelectedInternalDocument.AttachmentsLists = this.Attachments;
        SelectedInternalDocument.ToSpecificeEmail = this.ToSpecificeEmail;
        SelectedInternalDocument.ExportQuotationsToIntegratedSystem = this.ExportQuotationsToIntegratedSystem;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = sendWindowWidth;
        logWindow.Height = SelectedInternalDocument.WindowHeight = sendWindowHeight;
        logWindow.Title = "Send Message";
        logWindow.DataContext = SelectedInternalDocument;
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
        this.LoadingSendingComponent = false;
    };
    return GeneralEmailSender;
}());
exports.GeneralEmailSender = GeneralEmailSender;
//# sourceMappingURL=GeneralEmailSender.js.map