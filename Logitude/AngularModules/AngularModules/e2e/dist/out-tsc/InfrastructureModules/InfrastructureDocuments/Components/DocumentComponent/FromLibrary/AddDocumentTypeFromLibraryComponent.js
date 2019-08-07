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
var DocumentTypeListExtendedService_1 = require("../../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService");
var DocumentTypePMExtendedService_1 = require("../../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ServiceHelper_1 = require("../../../../../Infrastructure/Utilities/ServiceHelper");
var DocumentTypeTemplateViewModel_1 = require("../DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var SessionInfo_1 = require("../../../../../Infrastructure/Utilities/SessionInfo");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var AddDocumentTypeFromLibraryComponent = /** @class */ (function () {
    function AddDocumentTypeFromLibraryComponent(_documentTypeTemplateListExtendedService, _dcumentTypePMExtendedService, _documentTypeListExtendedService) {
        this._documentTypeTemplateListExtendedService = _documentTypeTemplateListExtendedService;
        this._dcumentTypePMExtendedService = _dcumentTypePMExtendedService;
        this._documentTypeListExtendedService = _documentTypeListExtendedService;
        this.ObjectTableId = "";
        this.EntityId = "";
        this.TransportModeId = "";
        this.ShipmentlevelCode = "";
        this.ChildEntityId = "";
        this.ChildObjectTableId = "";
        this.PageRequest = "";
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddDocumentTypeFromLibraryComponent.prototype.ngOnInit = function () {
    };
    AddDocumentTypeFromLibraryComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe(function (response) {
            _this.DataViewModel = args.DataViewModel;
            _this.ObjectTableId = args.ObjectTableId;
            _this.EntityId = args.EntityId;
            _this.TransportModeId = args.TransportModeId;
            _this.ShipmentlevelCode = args.ShipmentlevelCode;
            _this.ChildEntityId = args.ChildEntityId;
            _this.ChildObjectTableId = args.ChildObjectTableId;
            _this.PageRequest = args.PageRequest;
            _this.IsLoadTextCode = true;
            _this.DocumentTypeTemplatePMLists = [];
            _this.DocumentTypeTemplateLists = [];
            _this.FullDocumentTypeTemplateLists = [];
            _this.Load();
        });
    };
    AddDocumentTypeFromLibraryComponent.prototype.onSearchTextChangeEvent = function (search) {
        if (search) {
            if (search != "Search") {
                this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists.filter(function (d) { return d.Description.toUpperCase().indexOf(search.toUpperCase()) > -1 || d.DocumentTypeName.toUpperCase().indexOf(search.toUpperCase()) > -1; });
            }
        }
        else {
            this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists;
        }
        if (this.DocumentTypeTemplateLists && this.DocumentTypeTemplateLists.length == 0) {
            this.IsShowMessageNoDocument = true;
        }
        else
            this.IsShowMessageNoDocument = false;
    };
    AddDocumentTypeFromLibraryComponent.prototype.Load = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        var isfilter = FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPE") ? false : true;
        this._documentTypeTemplateListExtendedService.GetDocumentTypeTemplatesFromLibrary(this.ObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant, isfilter, this.TransportModeId, this.ShipmentlevelCode).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.DocumentTypeTemplateLists = [];
                myResult.forEach(function (item) {
                    _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                    _this.FullDocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                });
                //if (myResult) {
                //    var temp = new GroupByPipe().transform(myResult, "DocumentTypeId");
                //}
                if (_this.DocumentTypeTemplateLists.length == 0) {
                    _this.IsShowMessageNoDocument = true;
                }
                else
                    _this.IsShowMessageNoDocument = false;
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    AddDocumentTypeFromLibraryComponent.prototype.OnSelectedDocumentTypeTemplateLists = function (item) {
        this.DocumentTypeTemplateViewModelSelected = item;
    };
    AddDocumentTypeFromLibraryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddDocumentTypeFromLibraryComponent.prototype.AddFromLibraryButtonClicked = function (item) {
        this.DocumentTypeTemplateViewModelSelected = item;
        this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = false;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("saving");
        this.CopyDocumentTypeAndTemplate();
    };
    AddDocumentTypeFromLibraryComponent.prototype.CopyDocumentTypeAndTemplate = function () {
        var _this = this;
        this._documentTypeTemplateListExtendedService.CopyDocumentTypeAndDocumentTypTemplate(this.DocumentTypeTemplateViewModelSelected.Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var documenttypecode = pmResponse.Result;
                _this._documentTypeListExtendedService.getDocumentTypeListByCode(documenttypecode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (_this.PageRequest == "DocOut" && _this.DataViewModel) {
                            _this.DataViewModel.DocumentTypes.push(myResult);
                            _this.DataViewModel.FillDocumentTypes();
                            var selectedInternalDocument = _this.DataViewModel.StaticDocumentsList.filter(function (s) { return s.DocumentTypeCode == documenttypecode; })[0];
                            if (selectedInternalDocument) {
                                if (_this.DocumentTypeTemplateViewModelSelected.TemplateType == "P") {
                                    _this.DataViewModel.PrintButtonClick(selectedInternalDocument);
                                }
                                else if (_this.DocumentTypeTemplateViewModelSelected.TemplateType == "M") {
                                    _this.DataViewModel.SendButtonClick(selectedInternalDocument);
                                }
                            }
                        }
                        // this.DocumentTypeTemplatePMLists.push(myResult);
                        _this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CloseButtonClicked();
                    }
                    else {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
                        _this.CloseButtonClicked();
                    }
                });
            }
            else {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
            }
        });
        //    
    };
    AddDocumentTypeFromLibraryComponent.prototype.PreviewFromLibraryButtonClicked = function (item) {
        this.DocumentTypeTemplateViewModelSelected = item;
        if (item.TemplateType == "P" && item.EditorTool == "S") {
            this.PreviewStimualTemplate(item, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId);
        }
        else {
            this.PreviewHtmlTemplate(item, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId);
        }
    };
    AddDocumentTypeFromLibraryComponent.prototype.PreviewStimualTemplate = function (item, currentEntityId, currentObjectTableId, childEntityId, ChildObjectTableId) {
        var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
        window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=" + item.Id + "&documentTypeTemplateName" + item.Description + "&entityId=" + currentEntityId + "&entityObjectTableId=" + currentObjectTableId + "&childEntityId=" + childEntityId + "&childObjectTableId=" + ChildObjectTableId + "&TemplateType=" + item.TemplateType + "&EditorTool=" + item.EditorTool + "&tempId=" + token);
    };
    AddDocumentTypeFromLibraryComponent.prototype.PreviewHtmlTemplate = function (item, currentEntityId, currentObjectTableId, childEntityId, ChildObjectTableId) {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Preview";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = 0;
        windowArgs.EntityId = currentEntityId;
        windowArgs.ObjectTableId = currentObjectTableId;
        windowArgs.ChildEntityId = childEntityId;
        windowArgs.ChildObjectTableId = ChildObjectTableId;
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
    AddDocumentTypeFromLibraryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddDocumentTypeFromLibraryComponent.html',
            providers: [DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, DocumentTypeListExtendedService_1.DocumentTypeListExtendedService]
        }),
        __metadata("design:paramtypes", [DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, DocumentTypeListExtendedService_1.DocumentTypeListExtendedService])
    ], AddDocumentTypeFromLibraryComponent);
    return AddDocumentTypeFromLibraryComponent;
}());
exports.AddDocumentTypeFromLibraryComponent = AddDocumentTypeFromLibraryComponent;
//# sourceMappingURL=AddDocumentTypeFromLibraryComponent.js.map