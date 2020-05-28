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
/// <reference path="../../tools.ts" />
var core_1 = require("@angular/core");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var DocumentTypeTemplateFilter_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/DocumentTypeTemplateFilter");
var DocumentTypeTemplatePM_1 = require("../../../Common/EntityPMs/DocumentTypeTemplatePM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var StimulsoftDesignerComponent = /** @class */ (function () {
    function StimulsoftDesignerComponent(_documentTypeTemplatePMExtendedService) {
        this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        this.OnCloseWindow = new core_1.EventEmitter();
        //DocumenttypetemplateId: string;
        //CurrentDocumentOutId: string;
        //EntitiyId: string;
        //DocumenttypeCode: string;
        //DocumenttypecopyId: string;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        Stimulsoft.Base.StiLicense.key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHmx0GX2YaQY2fO4QUxViJm3MQEqlPzuUWXG/MVALbDozqE5ju" +
            "b1Lxxc9dG4qgTwOniU2gMMveQV+dJH1XkfRD1MNXb7qftfSxhKy/sz48Bbjuk1L3hTtOWwLJkGU/7cdsKzMCby7tGL" +
            "OGYgh8SwTOub9I9sRPEX2lQYcPP+Il4Xfhoo6Wuy8pZsfQ9T1qeKjawy2fkZdSnLcD6kfKqKtHQsdICN5BiXWAyXzw" +
            "act0mGsT790xrC2o/tO9hMolOEEYeJFTlsNJSorhgH6cn+TeBr/VhyDswq4OXv+op9bZc1z7dqYAYxkcWvdiQ04/L+" +
            "hJY8P23m4dqBZfxbUrOs17ZhtItSd2QWuUCyBywn6UZ9fZSOLDKl0Lk2ItxbjsHGv1Hp51puoRA/LxYOc5va7DT1Ws" +
            "R8S6if6a53D0VkMeB8gkBGwWVh8WhCH/uaOnq16tH2sicM8DpNRHUYymWcrF4QHpwZGeRiuMIkruiH7HZD+pTyPI8M" +
            "ObqwwI+EWgTu2QZmYXdH6VmzdL8T3d+pgYEz";
        Stimulsoft.Base.StiFontCollection.AddFontFile("./IDAutomationCMC7n10.ttf");
    }
    StimulsoftDesignerComponent.prototype.ngOnInit = function () {
    };
    StimulsoftDesignerComponent.prototype.SetWindowArgs = function (args) {
        this.windowArgs = args;
        this.TemplateId = this.windowArgs.TemplateId;
        this.documentTypeTemplateViewModel = this.windowArgs.ViewModel;
        this.LoadStimulDesigner();
    };
    StimulsoftDesignerComponent.prototype.LoadStimulDesigner = function () {
        //var options = new Stimulsoft.Designer.StiDesignerOptions();
        //options.appearance.fullScreenMode = true;
        //options.appearance.interfaceType = "Mouse";
        //options.toolbar.showFileMenuExit = false;
        //options.appearance.showLocalization = true;
        //Stimulsoft.System.NodeJs.useWebKit = true;
        //Stimulsoft.System.NodeJs.localizationPath = "locales";
        var _this = this;
        var options = new Stimulsoft.Designer.StiDesignerOptions();
        options.appearance.fullScreenMode = true;
        options.toolbar.showFileMenuExit = false;
        options.allowChangeWindowTitle = false;
        //options.zoomout = "100%";
        options.dictionary.businessObjectsPermissions = Stimulsoft.Designer.StiDesignerPermissions.All;
        //options.toolbar.showPreviewButton = true;
        //options.toolbar.showFileMenu = true;
        //options.components.showImage = false;
        //options.components.showShape = false;
        //options.components.showPanel = false;
        //options.components.showCheckBox = false;
        //options.components.showSubReport = false;
        //widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        options.width = "100%";
        options.height = (heighthwindow - 200).toString() + "px";
        this.designer = new Stimulsoft.Designer.StiDesigner(options, "StiDesigner", false);
        var isload = false;
        var report = new Stimulsoft.Report.StiReport();
        //report.zoomout = "100%";
        //if (this.stimulsoftArgData.EditDocumentComponent.IsManageStimul && item != null) {
        //    if (item.Jsonstring != null && item.Jsonstring != undefined) {
        //        isload = true;
        //        report.load(item.Jsonstring);
        //        this.designer.report = report;
        //        this.designer.renderHtml("designerContent");
        //    }
        //}
        // if (!isload) {
        //if (item == null || item === undefined) {
        //    if (this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel != null && this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel != undefined) {
        //        item = this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel;
        //    }
        //}
        if (this.documentTypeTemplateViewModel != null && this.documentTypeTemplateViewModel != undefined && this.documentTypeTemplateViewModel.IsHaveJsonString) {
            this._documentTypeTemplatePMExtendedService.GetTemplateBodyhtmlOrJsonByDocumentTemplateId(this.TemplateId, SessionLocator_1.SessionLocator.Tenant, false, "stmual").subscribe(function (res) {
                report.load(res.Result);
                _this.designer.report = report;
                _this.designer.renderHtml("designerContent");
            });
        }
        else {
            //console.log("Start");
            //console.log(this.DocumenttypetemplateId);
            //console.log(this.Tenant);
            this._documentTypeTemplatePMExtendedService.GetTemplateBodyByDocumentTemplateId(this.TemplateId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                report.load(res.Result);
                _this.designer.report = report;
                _this.designer.renderHtml("designerContent");
            });
        }
    };
    StimulsoftDesignerComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close("");
    };
    StimulsoftDesignerComponent.prototype.SaveButtonClicked = function () {
        //var isdisplay = false;
        //if (this.stimulsoftArgData.EditDocumentComponent.IsManageStimul) {
        //    isdisplay = true;
        //}
        var _this = this;
        var jsonStr = this.designer.report.saveToJsonString();
        //StimulsoftArg.ReportData = jsonStr;
        var filter = new DocumentTypeTemplateFilter_1.DocumentTypeTemplateFilter();
        filter.Id = this.TemplateId;
        filter.Tenant = SessionLocator_1.SessionLocator.Tenant;
        filter.Body = jsonStr;
        filter.TemplateType = "Stimul";
        this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe(function (res) {
            _this.CloseButtonClicked();
            //if (this.stimulsoftArgData.EditDocumentComponent.IsShowTemplateList) {
            //    if (this.stimulsoftArgData.EditDocumentComponent.SelectedItemFromMenu != null) {
            //        this.stimulsoftArgData.EditDocumentComponent.SelectedItemFromMenu.Jsonstring = jsonStr;
            //        if (this.stimulsoftArgData.EditDocumentComponent.SelectedItemFromMenu == this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel) {
            //            this.stimulsoftArgData.EditDocumentComponent.LoadstimulData(this.DocumenttypetemplateId, isdisplay,true);
            //        }
            //    }
            //}
            //else {
            //    this.stimulsoftArgData.EditDocumentComponent.LoadstimulData(this.DocumenttypetemplateId, isdisplay,true);
            //}
            //this.CloseButtonClicked();
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], StimulsoftDesignerComponent.prototype, "OnCloseWindow", void 0);
    StimulsoftDesignerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'StimulsoftDesigner',
            templateUrl: './StimulsoftDesignerComponent.html',
            inputs: ['stimulsoftArgData'],
            providers: [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, DocumentTypeTemplatePM_1.DocumentTypeTemplatePM],
        })
        //
        ,
        __metadata("design:paramtypes", [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService])
    ], StimulsoftDesignerComponent);
    return StimulsoftDesignerComponent;
}());
exports.StimulsoftDesignerComponent = StimulsoftDesignerComponent;
//# sourceMappingURL=StimulsoftDesignerComponent.js.map