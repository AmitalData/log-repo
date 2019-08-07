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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var DocumentTypeTemplateListExtendedService_1 = require("../../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService");
var DocumentTypePMExtendedService_1 = require("../../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ServiceHelper_1 = require("../../../../../Infrastructure/Utilities/ServiceHelper");
var DocumentTypeTemplateViewModel_1 = require("../DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var SessionInfo_1 = require("../../../../../Infrastructure/Utilities/SessionInfo");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DocumentTypeTemplatePMService_1 = require("../../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var DocumentTypeTemplatePM_1 = require("../../../../../Common/EntityPMs/DocumentTypeTemplatePM");
var AddDocumentTypeTemplateFromLibraryComponent = /** @class */ (function () {
    function AddDocumentTypeTemplateFromLibraryComponent(_documentTypeTemplateListExtendedService, _dcumentTypePMExtendedService, _documentTypeTemplatePMExtendedService) {
        this._documentTypeTemplateListExtendedService = _documentTypeTemplateListExtendedService;
        this._dcumentTypePMExtendedService = _dcumentTypePMExtendedService;
        this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        this.ObjectTableId = "";
        this.EntityId = "";
        this.TransportModeId = "";
        this.ShipmentlevelCode = "";
        this.ChildEntityId = "";
        this.ChildObjectTableId = "";
        this.PageRequest = "";
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
    }
    AddDocumentTypeTemplateFromLibraryComponent.prototype.ngOnInit = function () {
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.SetWindowArgs = function (args) {
        this.DataViewModel = args.DataViewModel;
        this.CurrentDocumentType = args.CurrentDocumentType;
        this.PageRequest = args.PageRequest;
        this.ObjectTableId = args.ObjectTableId;
        this.EntityId = args.EntityId;
        this.ChildEntityId = args.ChildEntityId;
        this.ChildObjectTableId = args.ChildObjectTableId;
        this.DocumentTemplateEditorTool = this.CurrentDocumentType.DocumentTypeDefaultEditorTool;
        if (this.PageRequest != "Send" && args.DocumentTemplateEditorTool) {
            this.DocumentTemplateEditorTool = args.DocumentTemplateEditorTool;
        }
        this.DocumentTypeTemplatePMLists = [];
        this.DocumentTypeTemplateLists = [];
        this.Load();
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.Load = function () {
        var _this = this;
        this.DocumentTypeTemplateLists = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        var isfilter = FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPE") ? false : true;
        this._documentTypeTemplateListExtendedService.GetDocumentTypeTemplatesFromLibraryByDocumentTypeId(0, this.CurrentDocumentType.Id, isfilter, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach(function (item) {
                    if (_this.CurrentDocumentType.TemplateFormatCode == "P" && _this.DocumentTemplateEditorTool == "S" && _this.PageRequest != "Send") {
                        if (item.TemplateType == "P" && item.EditorTool == "S") {
                            _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                        }
                    }
                    else {
                        if (_this.PageRequest == "Send") {
                            if (item.TemplateType == "M") {
                                _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                            }
                        }
                        else {
                            if (item.TemplateType == "P" && item.EditorTool == "R") {
                                _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                            }
                        }
                    }
                });
                _this.newTemplatePm = new DocumentTypeTemplatePM_1.DocumentTypeTemplatePM();
                _this.newTemplatePm.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                _this.newTemplatePm.DocumentTypeId = _this.CurrentDocumentType.Id;
                _this.newTemplatePm.LastUpdatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                _this.newTemplatePm.LastUpdateByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.Contact;
                _this.newTemplatePm.DocumentTypeId = _this.CurrentDocumentType.Id;
                if (!_this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId) {
                    _this.newTemplatePm.IsDefault = true;
                }
                if (_this.DocumentTypeTemplateLists.length == 0) {
                    _this.IsShowMessageNoTemplate = true;
                }
                else
                    _this.IsShowMessageNoTemplate = false;
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.AddFromLibraryButtonClicked = function (item) {
        var _this = this;
        this.DocumentTypeTemplateViewModelSelected = item;
        this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = false;
        var countOfCopy = 0;
        var documentTypeTemplateList = null;
        if (this.DataViewModel && this.DataViewModel.ReportTemplates) {
            documentTypeTemplateList = this.DataViewModel.ReportTemplates.filter(function (d) { return d.OriginalTemplateId == _this.DocumentTypeTemplateViewModelSelected.Id; })[0];
            if (documentTypeTemplateList) {
                countOfCopy = this.DataViewModel.ReportTemplates.filter(function (d) { return d.OriginalTemplateId == _this.DocumentTypeTemplateViewModelSelected.Id; }).length;
            }
            this.newTemplatePm.IsDefault = this.DataViewModel.ReportTemplates.length == 0 ? true : false;
            this.newTemplatePm.Description = countOfCopy > 0 ? this.DocumentTypeTemplateViewModelSelected.Description + " [" + countOfCopy + "]" : this.DocumentTypeTemplateViewModelSelected.Description;
            ;
            if (documentTypeTemplateList || countOfCopy > 0) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.YesButtonText = "Ok";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.Width = 400;
                confirmWindow.Height = 200;
                confirmWindow.Show("Please note that this template already exists, Please confirm to add a new template");
                confirmWindow.Title = "DocumentTypeTemplate";
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.CopyDocumentTypeTemplate();
                    }
                });
            }
            else {
                this.CopyDocumentTypeTemplate();
            }
        }
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.CopyDocumentTypeTemplate = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        var documenttypetemplatePm = this.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == _this.DocumentTypeTemplateViewModelSelected.Id; })[0];
        if (!documenttypetemplatePm) {
            this._documentTypeTemplatePMExtendedService.GetSingleDocumentTypeTemplate(this.DocumentTypeTemplateViewModelSelected.Id, 0).subscribe(function (res) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var pmResponse = res;
                var myResult = pmResponse.Result;
                if (myResult) {
                    documenttypetemplatePm = myResult;
                    _this.SaveDocumentTypeTemplate(documenttypetemplatePm);
                }
                else {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    _this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
                }
            });
        }
        else {
            this.SaveDocumentTypeTemplate(documenttypetemplatePm);
        }
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.SaveDocumentTypeTemplate = function (documenttypetemplatePm) {
        var _this = this;
        this.newTemplatePm.TemplateBodyHtml = documenttypetemplatePm.TemplateBodyHtml;
        this.newTemplatePm.TemplateBody = documenttypetemplatePm.TemplateBody;
        this.newTemplatePm.TemplateType = documenttypetemplatePm.TemplateType;
        this.newTemplatePm.IsEnabledForCustomers = true;
        this.newTemplatePm.Language = documenttypetemplatePm.Language;
        this.newTemplatePm.CountryCode = documenttypetemplatePm.CountryCode;
        this.newTemplatePm.DocumentTypeCode = documenttypetemplatePm.DocumentTypeCode;
        this.newTemplatePm.InternalRemarks = documenttypetemplatePm.InternalRemarks;
        this.newTemplatePm.EditorTool = documenttypetemplatePm.EditorTool;
        this.newTemplatePm.Subject = documenttypetemplatePm.Subject;
        this.newTemplatePm.OriginalTemplateId = documenttypetemplatePm.Id;
        this.newTemplatePm.IsCopiedAtSignup = true;
        this.documentTypeTemplatePMService.insert(this.newTemplatePm).subscribe(function (res) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var pmResponse = res;
            _this.CurrentSession.CloseCurrentWindow();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.DocumentTypeTemplatePMLists.push(myResult);
                    if (_this.DataViewModel) {
                        var template = new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(myResult);
                        _this.DataViewModel.ReportTemplates.push(template);
                        _this.DataViewModel.OnSelectTemplateChange(template);
                    }
                }
            }
        });
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.PreviewFromLibraryButtonClicked = function (item) {
        this.DocumentTypeTemplateViewModelSelected = item;
        if (item.TemplateType == "P" && item.EditorTool == "S") {
            this.PreviewStimualTemplate(item, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId);
        }
        else {
            this.PreviewHtmlTemplate(item);
        }
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.PreviewStimualTemplate = function (item, currentEntityId, currentObjectTableId, childEntityId, ChildObjectTableId) {
        var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
        window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=" + item.Id + "&documentTypeTemplateName" + item.Description + "&entityId=" + currentEntityId + "&entityObjectTableId=" + currentObjectTableId + "&childEntityId=" + childEntityId + "&childObjectTableId=" + ChildObjectTableId + "&TemplateType=" + item.TemplateType + "&EditorTool=" + item.EditorTool + "&tempId=" + token);
    };
    AddDocumentTypeTemplateFromLibraryComponent.prototype.PreviewHtmlTemplate = function (item) {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Preview";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = 0;
        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ChildEntityId = this.ChildEntityId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = item.Description;
        windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
    };
    AddDocumentTypeTemplateFromLibraryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddDocumentTypeTemplateFromLibraryComponent.html',
            providers: [DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService]
        }),
        __metadata("design:paramtypes", [DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService])
    ], AddDocumentTypeTemplateFromLibraryComponent);
    return AddDocumentTypeTemplateFromLibraryComponent;
}());
exports.AddDocumentTypeTemplateFromLibraryComponent = AddDocumentTypeTemplateFromLibraryComponent;
//# sourceMappingURL=AddDocumentTypeTemplateFromLibraryComponent.js.map