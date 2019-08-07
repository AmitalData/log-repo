"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ApiQueryFilters_1 = require("../DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var DocumentTypeListService_1 = require("../../Common/Services/StandardLists/DocumentTypeListService");
var DocumentTypePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var Tools_1 = require("../../Infrastructure/Tools");
var DocumentOutPMService_1 = require("../../Common/Services/ExtendedPMs/DocumentOutPMService");
var DocsOutDataViewModel_1 = require("../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel");
var GeneralPrintHelper = /** @class */ (function () {
    function GeneralPrintHelper(objecttablename, documentTypeCode, entityId, childEntityId, childReference, childObjectTableId) {
        var _this = this;
        this.IsLoadPrintControl = false;
        this.IsStartPrint = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ObjectTableName = objecttablename;
        if (!Tools_1.AppTool.IsNullOrEmpty(documentTypeCode)) {
            this.DocumentTypeCode = documentTypeCode.toUpperCase();
        }
        this.CurrentObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == objecttablename; })[0].Id;
        this.EntityId = entityId == "null" || !entityId ? "" : entityId;
        this.ChildEntityId = childEntityId == "null" || !childEntityId ? "" : childEntityId;
        this.ChildObjectTableId = childObjectTableId == "null" || !childObjectTableId ? "" : childObjectTableId;
        this.ChildReference = childReference == "null" || !childReference ? "" : childReference;
        this.documentTypePMService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        this.documentOutPMService = new DocumentOutPMService_1.DocumentOutPMService();
        var documentTypeListService = new DocumentTypeListService_1.DocumentTypeListService();
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.documentTypeList = myResult.filter(function (d) { return d.Code.toUpperCase() == _this.DocumentTypeCode; })[0];
                if (_this.documentTypeList) {
                    if (_this.documentTypeList.DocumentTypeDefaultReportTemplateId) {
                        _this.IsLoadPrintControl = true;
                    }
                    else
                        _this.ShowMessage("Document type of code " + _this.DocumentTypeCode + " has no default template");
                }
                else {
                    if (_this.ObjectTableName == "APPayment") {
                        _this.ShowMessage("There is no document type for A/P Payment please go to maintenance and add it!");
                    }
                    else if (_this.ObjectTableName == "ARPayment") {
                        _this.ShowMessage("There is no document type for A/R Payment please go to maintenance and add it!");
                    }
                    else
                        _this.ShowMessage("Document type of code " + _this.DocumentTypeCode + " not exists");
                }
            }
        });
    }
    GeneralPrintHelper.prototype.ShowPrintControl = function () {
        var _this = this;
        if (this.IsLoadPrintControl && !this.IsStartPrint) {
            this.IsStartPrint = true;
            this.CurrentSession.StartBusyIndicatorLoading();
            this.documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.EntityId, SessionInfo_1.SessionInfo.LoggedUserTenant, this.ChildEntityId, this.documentTypeList.Id).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    _this.documentOutPM = myResult;
                    if (!_this.documentOutPM) {
                        _this.documentOutPMService.getCreateDocumentOut(_this.documentTypeList.Id, _this.EntityId, _this.ChildEntityId, _this.ChildReference, _this.CurrentObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                            var pmResponse = res;
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {
                                    _this.documentOutPM = myResult;
                                    _this.LoadDocumentTypePm();
                                }
                            }
                            else {
                                _this.CurrentSession.StopBusyIndicator();
                                _this.IsStartPrint = false;
                            }
                        });
                    }
                    else {
                        _this.LoadDocumentTypePm();
                    }
                }
                else {
                    _this.IsStartPrint = false;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    GeneralPrintHelper.prototype.LoadDocumentTypePm = function () {
        var _this = this;
        this.documentTypePMService.getSingleDocumentType(this.documentTypeList.Id, this.documentOutPM.Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentTypePM = myResult;
                    _this.LoadPrintControl();
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                _this.IsStartPrint = false;
            }
        });
    };
    GeneralPrintHelper.prototype.LoadPrintControl = function () {
        var _this = this;
        this.IsStartPrint = false;
        this.CurrentSession.StopBusyIndicator();
        var documentOutPmLists = new Array();
        //this.documentOutPM.NeedsRebuild = true;
        documentOutPmLists.push(this.documentOutPM);
        var SelectedInternalDocument = new DocsOutDataViewModel_1.DocsOutDataViewModel(this.documentTypePM, this.EntityId, this.documentOutPM.ChildEntityId, this.CurrentObjectTableId, this.ChildObjectTableId, this.documentOutPM.ChildEntityReference, documentOutPmLists, null, null, null);
        SelectedInternalDocument.IsNotFromDocsOutListOpenPrintControl = true;
        SelectedInternalDocument.IsAWBWizard = false;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 760;
        var heightwindwo = this.documentOutPM.IssuedDate ? 552 : 502;
        logitudeWindow.Height = heightwindwo;
        logitudeWindow.DataContext = SelectedInternalDocument;
        logitudeWindow.Title = "Print " + this.documentTypePM.Name;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            if (_this.CurrentSession.CurrentEditComponent) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
    };
    GeneralPrintHelper.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    return GeneralPrintHelper;
}());
exports.GeneralPrintHelper = GeneralPrintHelper;
//# sourceMappingURL=GeneralPrintHelper.js.map