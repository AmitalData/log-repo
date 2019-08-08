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
var DocumentTypeTemplatePM_1 = require("../../../../Common/EntityPMs/DocumentTypeTemplatePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var ServiceArgs_1 = require("../../../../Infrastructure/DataContracts/ServiceArgs");
var ReportsTemplatePM_1 = require("../../../../Common/EntityPMs/ReportsTemplatePM");
var ReportsTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService");
var SaveAsTemplateComponent = /** @class */ (function () {
    function SaveAsTemplateComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
    }
    SaveAsTemplateComponent.prototype.ngOnInit = function () {
    };
    SaveAsTemplateComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        if (this.DataContext) {
            this.PageType = this.DataContext.PageType;
            this.SelectedTemplate = this.DataContext.template;
        }
    };
    SaveAsTemplateComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SaveAsTemplateComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (!this.Description) {
            this.ValidationErrorsList.push("Description field is required");
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            var newTemplatePM = null;
            if (this.PageType == "ReportTemplate") {
                newTemplatePM = new ReportsTemplatePM_1.ReportsTemplatePM();
                newTemplatePM.Description = this.Description;
                newTemplatePM.TemplateData = StringToBase64(this.DataContext.froalaEditorSetting.froalaEditorComponent.getHtml());
                newTemplatePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                newTemplatePM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                newTemplatePM.TemplateType = "M";
                newTemplatePM.ReportId = this.SelectedTemplate.ReportId;
                var reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService_1.ReportsTemplatePMExtendedService();
                reportsTemplatePMExtendedService.CreateReportTemplate(newTemplatePM).subscribe(function (res) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var result = pmResponse.Result;
                        if (result) {
                            _this.CurrentSession.CurrentWindow.Close(result.Id);
                        }
                    }
                    else {
                        pmResponse.ErrorsArray.forEach(function (item) {
                            _this.ValidationErrorsList.push(item);
                        });
                    }
                });
            }
            else {
                newTemplatePM = new DocumentTypeTemplatePM_1.DocumentTypeTemplatePM();
                newTemplatePM.Description = this.Description;
                newTemplatePM.DocumentTypeId = this.SelectedTemplate.DocumentTypeId;
                newTemplatePM.LastUpdateDate = this.SelectedTemplate.LastUpdateDate;
                newTemplatePM.LastUpdatedByUserId = SessionInfo_1.SessionInfo.LoggedUserPM.Id;
                newTemplatePM.TemplateType = this.SelectedTemplate.TemplateType;
                newTemplatePM.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                newTemplatePM.EditorTool = this.SelectedTemplate.EditorTool;
                newTemplatePM.HorizontalShift = this.SelectedTemplate.HorizontalShift;
                newTemplatePM.VerticalShift = this.SelectedTemplate.VerticalShift;
                newTemplatePM.Subject = this.DataContext.Subject;
                newTemplatePM.IsCopiedAtSignup = true;
                newTemplatePM.IsEnabledForCustomers = true;
                newTemplatePM.CountryCode = this.SelectedTemplate.CountryCode;
                newTemplatePM.InternalRemarks = this.SelectedTemplate.InternalRemarks;
                newTemplatePM.Language = this.SelectedTemplate.Language;
                newTemplatePM.OriginalTemplateId = this.SelectedTemplate.Id;
                newTemplatePM.TemplateTechnologyCode = "AG";
                newTemplatePM.TemplateHeaderHeight = this.SelectedTemplate.TemplateHeaderHeight;
                newTemplatePM.TemplateHeaderHtml = this.SelectedTemplate.TemplateHeaderHtml;
                newTemplatePM.TemplateFooterHeight = this.SelectedTemplate.TemplateFooterHeight;
                newTemplatePM.TemplateFooterHtml = this.SelectedTemplate.TemplateFooterHtml;
                if (this.DataContext) {
                    newTemplatePM.TemplateBodyHtml = StringToBase64(this.DataContext.froalaEditorSetting.froalaEditorComponent.getHtml());
                }
                this.documentTypeTemplatePMService.insert(newTemplatePM).subscribe(function (myResult) {
                    if (myResult) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (myResult.HasError) {
                            myResult.ErrorsArray.forEach(function (item) {
                                _this.ValidationErrorsList.push(item);
                            });
                        }
                        else {
                            if (_this.DataContext && myResult) {
                                if (_this.DataContext.DocumentTypeTemplatePMLists) {
                                    _this.DataContext.DocumentTypeTemplatePMLists.push(myResult.Result);
                                }
                                _this.CurrentSession.CurrentWindow.Close(myResult.Result.Id);
                            }
                        }
                    }
                });
            }
        }
    };
    SaveAsTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SaveAsTemplate',
            templateUrl: './SaveAsTemplateComponent.html',
            providers: [DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, ServiceArgs_1.ServiceArgs]
        }),
        __metadata("design:paramtypes", [])
    ], SaveAsTemplateComponent);
    return SaveAsTemplateComponent;
}());
exports.SaveAsTemplateComponent = SaveAsTemplateComponent;
//# sourceMappingURL=SaveAsTemplateComponent.js.map