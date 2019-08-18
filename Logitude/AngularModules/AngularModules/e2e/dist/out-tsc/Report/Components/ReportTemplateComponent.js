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
var EntityArgs_1 = require("../../Infrastructure/DataContracts/EntityArgs");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var ReportsTemplatePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var ReportsTemplatePMService_1 = require("../../Common/Services/StandardPMs/ReportsTemplatePMService");
var ReportTemplateComponent = /** @class */ (function () {
    function ReportTemplateComponent(entityArgs, _elementRef) {
        this.entityArgs = entityArgs;
        this._elementRef = _elementRef;
        this.IsChange = false;
        this.IsEnableEditUserReportTemplate = false;
        this.IsEnableEditAllReportTemplate = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.ReportsTemplatePMLists = [];
        this.MessageReportsTemplatePMLists = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsVisibile = false;
        this.IsShowNewButton = false;
        this.reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService_1.ReportsTemplatePMExtendedService();
        this.reportsTemplatePMService = new ReportsTemplatePMService_1.ReportsTemplatePMService();
        this.ReportsTemplatePMLists = [];
    }
    ReportTemplateComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityPM = this.entityArgs.EntityPM;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ReportsTemplate", "NEW"))
            this.IsShowNewButton = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ReportsTemplate", "SYSTEMTEMPLATEEDIT")) {
            this.IsEnableEditAllReportTemplate = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ReportsTemplate", "UPDATE")) {
            this.IsEnableEditUserReportTemplate = true;
        }
        this._entityResourceService.getEntityResourceByTableName("ReportsTemplate", 0).subscribe(function (response) {
            _this.IsVisibile = true;
            _this.EntityPM = _this.entityArgs.EntityPM;
            if (_this.EntityPM) {
                _this.LoadReportsTemplatePMLists();
            }
        });
    };
    ReportTemplateComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    ReportTemplateComponent.prototype.LoadReportsTemplatePMLists = function () {
        var _this = this;
        this.ReportsTemplatePMLists = [];
        this.MessageReportsTemplatePMLists = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.reportsTemplatePMExtendedService.GetReportsTemplatePMsByReportId(this.EntityPM.Id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    result.forEach(function (item) {
                        if (item.TemplateType == "R") {
                            _this.ReportsTemplatePMLists.push(item);
                        }
                        else if (item.TemplateType == "M") {
                            _this.MessageReportsTemplatePMLists.push(item);
                        }
                    });
                    if (_this.ReportsTemplatePMLists.length > 0) {
                        var tempate = _this.ReportsTemplatePMLists.filter(function (d) { return d.Id == _this.EntityPM.DefaultTemplateId; })[0];
                        if (tempate) {
                            tempate.IsDefault = true;
                            tempate.IsDirty = false;
                        }
                    }
                    if (_this.MessageReportsTemplatePMLists.length > 0) {
                        var tempate = _this.MessageReportsTemplatePMLists.filter(function (d) { return d.Id == _this.EntityPM.DefaultMessageTemplateId; })[0];
                        if (tempate) {
                            tempate.IsDefault = true;
                            tempate.IsDirty = false;
                        }
                    }
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    //Description
    ReportTemplateComponent.prototype.DescriptionKeyUpMethod = function (item) {
        if (item != null && item.IsDirty) {
            this.UpdateReportsTemplatePM(item);
        }
    };
    //Inactive
    ReportTemplateComponent.prototype.CheckInActiveclick = function (item) {
        if (item.InActive)
            item.InActive = false;
        else
            item.InActive = true;
        this.UpdateReportsTemplatePM(item);
    };
    ReportTemplateComponent.prototype.UpdateReportsTemplatePM = function (item) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.IsChange = true;
        this.reportsTemplatePMService.update(item).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    item = result;
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0]);
                }
            }
        });
    };
    ReportTemplateComponent.prototype.Refresh = function () {
        this.LoadReportsTemplatePMLists();
    };
    ReportTemplateComponent.prototype.EditReportsTemplate = function (item) {
        var _this = this;
        if (item) {
            if (this.IsEnableEditAllReportTemplate || (!item.IsSystem && this.IsEnableEditUserReportTemplate)) {
                var windowArgs = {};
                windowArgs.DataViewModel = this;
                windowArgs.ReportTemplateId = item.Id;
                windowArgs.Tenant = item.Tenant;
                var widthwindow = window.innerWidth;
                var heighthwindow = window.innerHeight;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Report Template";
                logWindow.IsShowCloseButton = true;
                logWindow.WindowArgs = windowArgs;
                window.designerClosed = false;
                logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event) {
                        _this.IsChange = true;
                        _this.Refresh();
                    }
                });
            }
        }
    };
    ReportTemplateComponent.prototype.EditMessageReportsTemplate = function (item, isNew) {
        if (isNew === void 0) { isNew = false; }
        if (item) {
            if (this.IsEnableEditAllReportTemplate || (!item.IsSystem && this.IsEnableEditUserReportTemplate)) {
                var windowArgs = {};
                windowArgs.DataViewModel = this;
                windowArgs.PageType = "ReportTemplate";
                windowArgs.TemplateId = item.Id;
                windowArgs.Tenant = item.Tenant;
                windowArgs.ObjectType = "ReportsTemplatePM";
                windowArgs.IsNewEntity = isNew;
                windowArgs.ReportTemplatePM = item;
                windowArgs.ReportComponentArea = "Maintenance";
                var widthwindow = window.innerWidth;
                var heighthwindow = window.innerHeight;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Message Template";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
            }
        }
    };
    ReportTemplateComponent.prototype.RestoreVersionButtonClicked = function (item) {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.ReportsTemplatePM = item;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 668;
        logWindow.Height = 500;
        logWindow.Title = "Version History";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Report/Components/ReportsTemplateRestoreComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
            }
        });
    };
    ReportTemplateComponent.prototype.SetAsDefaultButtonClicked = function (type) {
        var _this = this;
        var currentTemplate = type == "R" ? this.CurrentReportsTemplatePM : this.CurrentMessageReportsTemplatePM;
        if (currentTemplate) {
            if (!currentTemplate.InActive) {
                var tempate = type == "R" ? this.ReportsTemplatePMLists.filter(function (d) { return d.Id == _this.EntityPM.DefaultTemplateId; })[0] : this.MessageReportsTemplatePMLists.filter(function (d) { return d.Id == _this.EntityPM.DefaultMessageTemplateId; })[0];
                if (tempate) {
                    tempate.IsDefault = false;
                }
                this.IsChange = true;
                currentTemplate.IsDefault = true;
                if (type == "R")
                    this.EntityPM.DefaultTemplateId = currentTemplate.Id;
                else if (type == "M")
                    this.EntityPM.DefaultMessageTemplateId = currentTemplate.Id;
            }
            else
                this.ShowMessage("Please note that you can't set an inactive template as default");
        }
    };
    ReportTemplateComponent.prototype.AddReportTemplateButtonClicked = function (type) {
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.TemplateType = type;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.Title = type == "R" ? "New Report Template" : "New Message Template";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Report/Components/NewReportsTemplateComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
            }
        });
    };
    ReportTemplateComponent.prototype.CopyButtonClicked = function (item) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("copy template...");
        this.IsChange = true;
        this.reportsTemplatePMExtendedService.GetCopyReportsTemplate(item.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (!_this.ReportsTemplatePMLists) {
                        _this.ReportsTemplatePMLists = [];
                    }
                    _this.ReportsTemplatePMLists.push(result);
                    _this.CurrentReportsTemplatePM = result;
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ReportTemplateComponent = __decorate([
        core_1.Component({
            moduleId: './Report/Components/',
            selector: 'ReportTemplate',
            templateUrl: 'ReportTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ElementRef])
    ], ReportTemplateComponent);
    return ReportTemplateComponent;
}());
exports.ReportTemplateComponent = ReportTemplateComponent;
//# sourceMappingURL=ReportTemplateComponent.js.map