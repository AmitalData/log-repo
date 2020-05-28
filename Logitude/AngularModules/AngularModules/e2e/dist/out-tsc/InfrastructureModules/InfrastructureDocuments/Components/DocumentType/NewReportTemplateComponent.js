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
require("rxjs/add/operator/map");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var core_1 = require("@angular/core");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocumentTypeTemplatePM_1 = require("../../../../Common/EntityPMs/DocumentTypeTemplatePM");
var DocumentTypeTemplateListExtendedService_1 = require("../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var DocumentTypeTemplateViewModel_1 = require("../DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewReportTemplateComponent = /** @class */ (function (_super) {
    __extends(NewReportTemplateComponent, _super);
    function NewReportTemplateComponent(_documentTypeTemplateListExtendedService, _documentTypeTemplatePMExtendedService) {
        var _this = _super.call(this) || this;
        _this._documentTypeTemplateListExtendedService = _documentTypeTemplateListExtendedService;
        _this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DocumentTemplateFileId = Guid_1.Guid.NewRandomString();
        _this.DocumentsGridVisibility = false;
        _this.LoadMrtButtonVisibility = false;
        _this.EditorTypeVisibility = false;
        _this.ShowSaveButton = true;
        _this.CloseButtonLable = "Cancel";
        _this.ValueRadioChoice = "Blank";
        _this.ValueEditorRadio = "StimulSoft";
        _this.RadioButtonChoice1Id = Guid_1.Guid.newGuid();
        _this.RadioButtonChoice2Id = Guid_1.Guid.newGuid();
        _this.RadioButtonChoice3Id = Guid_1.Guid.newGuid();
        _this.RadioButtonChoice4Id = Guid_1.Guid.newGuid();
        _this.RadioEditorChoice1Id = Guid_1.Guid.newGuid();
        _this.RadioEditorChoice2Id = Guid_1.Guid.newGuid();
        _this.NameRadioButtonChoice = Guid_1.Guid.NewRandomString();
        _this.NameRadioEditorChoice = Guid_1.Guid.NewRandomString();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsLoadPage = false;
        if (_this.documentTypeTemplatePMService == null) {
            _this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
        return _this;
    }
    NewReportTemplateComponent.prototype.ngOnInit = function () {
    };
    NewReportTemplateComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe(function (response) {
            _this.IsLoadPage = true;
            _this.DocumentTypeTemplateLists = [];
            _this.DocumentTypeTemplatePMLists = [];
            _this.ObjectTableId = args.ObjectTableId;
            _this.DataViewModel = args.DataViewModel;
            _this.PageType = args.PageType;
            _this.DocumentType = args.CurrentEntityPM;
            _this.TypeTab = args.TypeTab;
            _this.FullDocumentTypeTemplateLists = args.DocumentTypeTemplateLists;
            if (_this.TypeTab == "Document") {
                _this.ValueEditorRadio = "StimulSoft";
                _this.EditorTypeVisibility = true;
            }
            else {
                _this.EditorTypeVisibility = false;
                _this.ValueEditorRadio = "RichText";
            }
        });
    };
    NewReportTemplateComponent.prototype.OnSelectedDocumentTypeTemplateLists = function (selectedItem) {
        this.DocumentTypeTemplateViewModelSelected = selectedItem;
        this.Description = selectedItem.Description;
        this.DocumentTypeTemplateLists.forEach(function (item) {
            item.DivSelectBackgroud = "#ffffff";
        });
        selectedItem.DivSelectBackgroud = "#B6E0F5";
        this.DocumentTypeTemplateViewModelSelected = selectedItem;
    };
    NewReportTemplateComponent.prototype.GetNewStanceFromDocumentTypeTemplatePM = function () {
        var template = new DocumentTypeTemplatePM_1.DocumentTypeTemplatePM();
        template.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        template.DocumentTypeId = this.DocumentType.Id;
        template.LastUpdatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
        if (SessionInfo_1.SessionInfo.LoggedUserPM)
            template.LastUpdateByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName;
        template.Description = this.Description;
        template.Subject = this.DocumentType.Subject;
        template.DocumentTypeId = this.DocumentType.Id;
        template.IsEnabledForCustomers = true;
        template.IsCopiedAtSignup = true;
        if (this.TypeTab == "Document") {
            template.TemplateType = "P";
            template.EditorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
        }
        else {
            template.TemplateType = "M";
            template.EditorTool = "R";
        }
        if (this.ValueRadioChoice == "FromFile") {
            if (template.EditorTool == "S")
                template.TemplateBody = this.UploadTemplateBodyData;
        }
        return template;
    };
    NewReportTemplateComponent.prototype.OpenUpLoadTemplateFile = function () {
        document.getElementById(this.DocumentTemplateFileId).click();
    };
    NewReportTemplateComponent.prototype.UpLoadTemplateFileMethod = function (event) {
        var file = querySelection(this.DocumentTemplateFileId);
        //document.querySelector('#DocumentTemplateFile').files[0];
        if (file) {
            this.ArrayBufferToBase64(file, this);
        }
    };
    NewReportTemplateComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.UploadTemplateBodyData = window.btoa(binary);
        };
        reader.onerror = function (e) {
        };
        reader.readAsArrayBuffer(file);
    };
    NewReportTemplateComponent.prototype.GetCopyStanceFromDocumentTypeTemplatePM = function (selectedItem) {
        var template = new DocumentTypeTemplatePM_1.DocumentTypeTemplatePM();
        template.Tenant = this.DocumentType.Tenant;
        template.DocumentTypeId = selectedItem.DocumentTypeId;
        template.LastUpdatedByUserId = selectedItem.LastUpdatedByUserId;
        template.LastUpdateByUserName = selectedItem.LastUpdateByUserName;
        template.Description = this.Description;
        template.Subject = selectedItem.Subject;
        template.TemplateType = selectedItem.TemplateType;
        template.EditorTool = selectedItem.EditorTool;
        template.TemplateBody = selectedItem.TemplateBody;
        template.TemplateBodyHtml = selectedItem.TemplateBodyHtml;
        template.OriginalTemplateId = selectedItem.Id;
        template.OriginalTemplateName = selectedItem.Description;
        template.ReplyTo = selectedItem.ReplyTo;
        template.From = selectedItem.From;
        template.DocumentTypeId = this.DocumentType.Id;
        template.Language = selectedItem.Language;
        template.CountryCode = selectedItem.CountryCode;
        template.DocumentTypeCode = selectedItem.DocumentTypeCode;
        template.InternalRemarks = selectedItem.InternalRemarks;
        template.IsEnabledForCustomers = true;
        template.IsCopiedAtSignup = true;
        template.TemplateHeaderHtml = selectedItem.TemplateHeaderHtml;
        template.TemplateFooterHtml = selectedItem.TemplateFooterHtml;
        template.TemplateHeaderHeight = selectedItem.TemplateHeaderHeight;
        template.TemplateFooterHeight = selectedItem.TemplateFooterHeight;
        return template;
    };
    NewReportTemplateComponent.prototype.InsertDocumentTypeTemplatePm = function (newTemplatePm) {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.FullDocumentTypeTemplateLists.length == 0) {
            newTemplatePm.IsDefault = true;
        }
        this.documentTypeTemplatePMService.insert(newTemplatePm).subscribe(function (myResult) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (myResult) {
                if (myResult.HasError) {
                    myResult.ErrorsArray.forEach(function (item) {
                        _this.ValidationErrorsList.push(item);
                    });
                }
                else {
                    var templateViewModel = new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(myResult.Result);
                    if (_this.FullDocumentTypeTemplateLists.length == 0) {
                        if (_this.TypeTab == "Document") {
                            _this.DocumentType.DocumentTypeDefaultReportTemplateId = myResult.Result.Id;
                        }
                        else {
                            _this.DocumentType.DocumentTypeDefaultHTMLTemplateId = myResult.Result.Id;
                        }
                    }
                    _this.DataViewModel.DocumentTypeTemplateLists.push(templateViewModel);
                    _this.DataViewModel.EditDocumentTemplate(templateViewModel);
                    _this.CurrentSession.CurrentWindow.Close(myResult.Result.Id);
                }
            }
        }, function (error) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var dd = error;
            console.log(dd.text);
        });
    };
    NewReportTemplateComponent.prototype.RadioButtonChoice = function (choose) {
        this.ValueRadioChoice = choose;
        this.UploadTemplateBodyData = null;
        this.DocumentsGridVisibility = false;
        switch (choose) {
            case "Blank":
                {
                    this.ShowSaveButton = true;
                    this.CloseButtonLable = "Cancel";
                    break;
                }
            case "Duplicate":
                {
                    var editorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
                    this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists ? this.FullDocumentTypeTemplateLists.filter(function (d) { return d.InActive == false && d.EditorTool == editorTool; }) : [];
                    this.DocumentsGridVisibility = true;
                    this.ShowSaveButton = true;
                    this.CloseButtonLable = "Cancel";
                    break;
                }
            case "FromLibrary":
                {
                    this.ShowSaveButton = false;
                    this.DocumentsGridVisibility = true;
                    this.CloseButtonLable = "Close";
                    this.LoadDocumentTypeTemplateFromLibrary();
                    break;
                }
            case "FromFile":
                {
                    this.ShowSaveButton = true;
                    this.CloseButtonLable = "Cancel";
                    break;
                }
        }
    };
    NewReportTemplateComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewReportTemplateComponent.prototype.EditorRadioButtonChoice = function (choose) {
        this.ValueEditorRadio = choose;
        if (this.ValueRadioChoice == "FromLibrary") {
            this.LoadDocumentTypeTemplateFromLibrary();
        }
        else {
            var editorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
            this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists.filter(function (d) { return d.InActive == false && d.EditorTool == editorTool; });
        }
    };
    NewReportTemplateComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if ((this.ValueRadioChoice == "FromLibrary" || this.ValueRadioChoice == "Duplicate") && !this.DocumentTypeTemplateViewModelSelected) {
            this.ValidationErrorsList.push("Please select at least template");
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            switch (this.ValueRadioChoice) {
                case "Blank":
                    {
                        var newTemplatePm = this.GetNewStanceFromDocumentTypeTemplatePM();
                        this.InsertDocumentTypeTemplatePm(newTemplatePm);
                        break;
                    }
                case "Duplicate":
                    {
                        if (this.DocumentTypeTemplateViewModelSelected) {
                            var newTemplatePm = this.GetCopyStanceFromDocumentTypeTemplatePM(this.DocumentTypeTemplateViewModelSelected.Entity);
                            this.InsertDocumentTypeTemplatePm(newTemplatePm);
                        }
                        break;
                    }
                case "FromFile":
                    {
                        var newTemplatePm = this.GetNewStanceFromDocumentTypeTemplatePM();
                        if (newTemplatePm.EditorTool == "R") {
                            this._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(this.UploadTemplateBodyData).subscribe(function (res) {
                                var pmResponse = res;
                                if (!pmResponse.HasError) {
                                    var myResult = pmResponse.Result;
                                    if (myResult) {
                                        var htmltemplate = myResult;
                                        if (htmltemplate) {
                                            htmltemplate.HeaderHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                            htmltemplate.FooterHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";
                                            newTemplatePm.TemplateHeaderHtml = StringToBase64(htmltemplate.HeaderHtml);
                                            newTemplatePm.TemplateFooterHtml = StringToBase64(htmltemplate.FooterHtml);
                                            newTemplatePm.TemplateBodyHtml = StringToBase64(htmltemplate.BodyHtml);
                                            newTemplatePm.TemplateHeaderHeight = htmltemplate.HeaderHeight;
                                            newTemplatePm.TemplateFooterHeight = htmltemplate.FooterHeight;
                                        }
                                    }
                                }
                                _this.InsertDocumentTypeTemplatePm(newTemplatePm);
                            });
                        }
                        else
                            this.InsertDocumentTypeTemplatePm(newTemplatePm);
                        break;
                    }
            }
        }
    };
    NewReportTemplateComponent.prototype.PreviewFromLibraryButtonClicked = function (item) {
        this.DocumentTypeTemplateViewModelSelected = item;
        if (item.EditorTool == "R") {
            var windowArgs = {};
            windowArgs.DataViewModel = this;
            windowArgs.PageType = "Preview";
            windowArgs.TemplateId = item.Id;
            windowArgs.Tenant = item.Tenant;
            windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;
            windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
            windowArgs.EntityId = "";
            windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;
            windowArgs.ChildEntityId = "";
            windowArgs.ChildObjectTableId = "";
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = item.Description;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        }
        else if (item.TemplateType == "P" && item.EditorTool == "S") {
            var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
            window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=" + item.Id + "&documentTypeTemplateName" + item.Description + "&entityId=" + "" + "&entityObjectTableId=" + "" + "&childEntityId=" + "" + "&childObjectTableId=" + "" + "&TemplateType=" + item.TemplateType + "&EditorTool=" + item.EditorTool + "&tempId=" + token);
        }
    };
    NewReportTemplateComponent.prototype.AddFromLibraryButtonClicked = function (item) {
        var _this = this;
        this.DocumentTypeTemplateViewModelSelected = item;
        // this.Description = this.DocumentTypeTemplateViewModelSelected.Description;
        if (!this.FullDocumentTypeTemplateLists.filter(function (d) { return d.OriginalTemplateId == item.Id; })[0]) {
            var template = this.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == item.Id; })[0];
            if (template) {
                var newTemplatePm = this.GetCopyStanceFromDocumentTypeTemplatePM(template);
                this.InsertDocumentTypeTemplatePm(newTemplatePm);
            }
            else {
                this.GetDocumentTypeTemplatePMFromLibrary(item);
            }
        }
        else {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("Please note that this template already exists, Please confirm to add a new template");
            confirmWindow.Title = "Document Type Template";
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    var template = _this.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == item.Id; })[0];
                    if (template) {
                        var newTemplatePm = _this.GetCopyStanceFromDocumentTypeTemplatePM(template);
                        _this.InsertDocumentTypeTemplatePm(newTemplatePm);
                    }
                    else {
                        _this.GetDocumentTypeTemplatePMFromLibrary(item);
                    }
                }
            });
        }
    };
    NewReportTemplateComponent.prototype.LoadDocumentTypeTemplateFromLibrary = function () {
        var _this = this;
        this.DocumentsGridVisibility = true;
        this.DocumentTypeTemplateLists = [];
        var Isfilter = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPE")) {
            Isfilter = false;
        }
        var editorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
        this._documentTypeTemplateListExtendedService.GetDocumentTypeTemplatesFromLibraryByDocumentTypeId(0, this.DocumentType.Id, Isfilter, this.DocumentType.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.filter(function (d) { return d.EditorTool == editorTool; }).forEach(function (item) {
                        _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                    });
                }
            }
        });
    };
    NewReportTemplateComponent.prototype.GetDocumentTypeTemplatePMFromLibrary = function (item) {
        var _this = this;
        var id = item.Id + "@0";
        this.documentTypeTemplatePMService.get(id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var template = _this.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == myResult.Id; })[0];
                    if (!template) {
                        _this.DocumentTypeTemplatePMLists.push(myResult);
                        var newTemplatePm = _this.GetCopyStanceFromDocumentTypeTemplatePM(myResult);
                        _this.InsertDocumentTypeTemplatePm(newTemplatePm);
                    }
                }
            }
            else
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }, function (error) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var dd = error;
            console.log(dd.text);
        });
    };
    NewReportTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'NewReportTemplate',
            templateUrl: './NewReportTemplateComponent.html',
            providers: [DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService]
        }),
        __metadata("design:paramtypes", [DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService])
    ], NewReportTemplateComponent);
    return NewReportTemplateComponent;
}(BaseComponent_1.BaseComponent));
exports.NewReportTemplateComponent = NewReportTemplateComponent;
//# sourceMappingURL=NewReportTemplateComponent.js.map