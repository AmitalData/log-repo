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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var core_1 = require("@angular/core");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var DocumentTypeTemplateViewModel_1 = require("../DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DocumentTypeTemplateComponent = /** @class */ (function (_super) {
    __extends(DocumentTypeTemplateComponent, _super);
    function DocumentTypeTemplateComponent() {
        var _this = _super.call(this) || this;
        _this.IsShowButtonDelete = false;
        _this.TemplateTabCode = "";
        _this.IsShowDeflutCoulm = false;
        _this.IsShowOriginalTemplateColum = false;
        if (_this.documentTypeTemplatePMService == null) {
            _this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
        return _this;
    }
    DocumentTypeTemplateComponent.prototype.ngOnInit = function () {
        if (this.DocumentType) {
            if (this.TypeTab == "Document") {
                this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("DocumentType.TH.Templates");
                this.TemplateTabCode = "P";
            }
            else {
                this.TemplateTabCode = "M";
                this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("DocumentType.TH.HTMLTemplates");
            }
            this.FillDocumentTypeTemplate();
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPEPROPERTIES")) {
            this.IsShowDeflutCoulm = true;
        }
        else {
            this.IsShowDeflutCoulm = false;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "ORGINALTEMPLATE")) {
            this.IsShowOriginalTemplateColum = true;
        }
        else
            this.IsShowOriginalTemplateColum = false;
    };
    DocumentTypeTemplateComponent.prototype.FillDocumentTypeTemplate = function () {
        var _this = this;
        this.DocumentTypeTemplateLists = [];
        if (this.DocumentType && this.DocumentTypeTemplates) {
            this.DocumentTypeTemplates.filter(function (D) { return D.TemplateType == _this.TemplateTabCode; }).forEach(function (item) {
                if (item.TemplateType == "M") {
                    if (item.Id == _this.DocumentType.DocumentTypeDefaultHTMLTemplateId)
                        item.IsDefault = true;
                    else
                        item.IsDefault = false;
                }
                else {
                    if (item.Id == _this.DocumentType.DocumentTypeDefaultReportTemplateId)
                        item.IsDefault = true;
                    else
                        item.IsDefault = false;
                }
                _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
            });
        }
    };
    DocumentTypeTemplateComponent.prototype.AddTemplateButtonClicked = function () {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Maintenance";
        windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;
        ;
        windowArgs.CurrentEntityPM = this.DocumentType;
        windowArgs.DocumentTypeTemplateLists = this.DocumentTypeTemplateLists;
        windowArgs.TypeTab = this.TypeTab;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = this.TypeTab == "Document" ? "New Print Template" : "New Html Template";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/NewReportTemplateComponent');
    };
    DocumentTypeTemplateComponent.prototype.DeleteTemplateButtonClicked = function () {
    };
    DocumentTypeTemplateComponent.prototype.SetAsDefaultButtonClicked = function () {
        var _this = this;
        if (this.CurrentDocumentTypeTemplatePM && this.DocumentType) {
            if (this.TypeTab == "Document") {
                this.DocumentType.DocumentTypeDefaultReportTemplateId = this.CurrentDocumentTypeTemplatePM.Id;
                this.CurrentDocumentTypeTemplatePM.IsDefault = true;
                this.DocumentTypeTemplateLists.forEach(function (item) {
                    if (item.Id != _this.CurrentDocumentTypeTemplatePM.Id) {
                        item.IsDefault = false;
                    }
                });
            }
            else {
                this.DocumentType.DocumentTypeDefaultHTMLTemplateId = this.CurrentDocumentTypeTemplatePM.Id;
                this.CurrentDocumentTypeTemplatePM.IsDefault = true;
                this.DocumentTypeTemplateLists.forEach(function (item) {
                    if (item.Id != _this.CurrentDocumentTypeTemplatePM.Id) {
                        item.IsDefault = false;
                    }
                });
            }
        }
    };
    DocumentTypeTemplateComponent.prototype.CheckInActiveclick = function (item) {
        if (item.InActive)
            item.InActive = false;
        else
            item.InActive = true;
        this.UpdateDocumentTypeTemplate(item.Entity);
    };
    DocumentTypeTemplateComponent.prototype.EditDocumentTemplate = function (item) {
        this.SelectedDocumentTypeTemplate = item;
        if (item.Entity.EditorTool == "R") {
            var windowArgs = {};
            windowArgs.DataViewModel = this;
            windowArgs.PageType = "Maintenance";
            windowArgs.TemplateId = item.Id;
            windowArgs.Tenant = item.Entity.Tenant;
            windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplateLists;
            windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
            windowArgs.EntityId = "";
            windowArgs.ChildEntityId = "";
            windowArgs.ChildObjectTableId = "";
            if (this.DocumentType) {
                windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;
            }
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Edit Html Template";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        }
        else {
            var windowArgs = {};
            windowArgs.DataViewModel = this;
            windowArgs.TemplateId = item.Id;
            windowArgs.Tenant = item.Entity.Tenant;
            windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplateLists;
            windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
            windowArgs.EntityId = "";
            windowArgs.ChildEntityId = "";
            windowArgs.ChildObjectTableId = "";
            if (this.DocumentType) {
                windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;
            }
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Edit Print Template";
            logWindow.IsShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            window.designerClosed = false;
            //FileLoader.LoadStimulSoftResources().then((isLoaded: boolean) => {
            // logWindow.Show("./Infrastructure/Components/StimulsoftComponent/StimulsoftDesignerComponent");
            //});
            logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
            var pollTimer = window.setInterval(function () {
                if (window.sessionStorage.getItem("designerClosed")) { // !== is required for compatibility with Opera
                    window.clearInterval(pollTimer);
                    this.designerPopUpClosed();
                }
            }, 200);
        }
    };
    DocumentTypeTemplateComponent.prototype.RunStimulsoftDesigner = function (templateId) {
        var URI = Tools_1.AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo_1.SessionInfo.Token + "&tenant=" + SessionInfo_1.SessionInfo.LoggedUserTenant + "&templateId=" + templateId;
        var WindowHeight = window.innerHeight - 200;
        var WindowWidth = window.innerWidth - 100;
        var win = window.open(URI, 'Stimulsoft Designer', 'left=100, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=' + WindowWidth + ',height=' + WindowHeight);
        var pollTimer = window.setInterval(function () {
            if (win.closed !== false) { // !== is required for compatibility with Opera
                window.clearInterval(pollTimer);
                //this.designerPopUpClosed();
            }
        }, 200);
    };
    DocumentTypeTemplateComponent.prototype.designerPopUpClosed = function () {
        alert("designer closed!");
    };
    DocumentTypeTemplateComponent.prototype.ShowDefaultsDocumentTypeTemplates = function (item) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 350;
        logitudeWindow.DataContext = item.Entity;
        logitudeWindow.Title = "Defaults";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/AdvanceDocumentTypeTemplateComponent');
    };
    DocumentTypeTemplateComponent.prototype.DescriptionKeyUpMethod = function (selectedItem) {
        if (selectedItem != null && selectedItem.Entity && selectedItem.Entity.IsDirty) {
            this.UpdateDocumentTypeTemplate(selectedItem.Entity);
        }
    };
    DocumentTypeTemplateComponent.prototype.OnSelectedDocumentTypeTemplateLists = function (selectedItem) {
        this.CurrentDocumentTypeTemplatePM = selectedItem;
    };
    DocumentTypeTemplateComponent.prototype.UpdateDocumentTypeTemplate = function (item) {
        this.DocumentType.IsAir = !this.DocumentType.IsAir;
        this.DocumentType.IsAir = !this.DocumentType.IsAir;
        this.documentTypeTemplatePMService.update(item).subscribe(function (res) {
        });
    };
    DocumentTypeTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentTypeTemplate',
            templateUrl: './DocumentTypeTemplateComponent.html',
            inputs: ['DocumentType', 'DocumentTypeTemplates', 'TypeTab'],
            providers: [DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService]
        }),
        __metadata("design:paramtypes", [])
    ], DocumentTypeTemplateComponent);
    return DocumentTypeTemplateComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentTypeTemplateComponent = DocumentTypeTemplateComponent;
//# sourceMappingURL=DocumentTypeTemplateComponent.js.map