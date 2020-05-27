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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ReportService_1 = require("../../Common/Services/ExtendedLists/ReportService");
var StimulsoftArg_1 = require("../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/StimulsoftArg");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var EntityPartner_1 = require("../../Infrastructure/DataContracts/EntityPartner");
var Tools_1 = require("../../Infrastructure/Tools");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var Rx_1 = require("rxjs/Rx");
require("rxjs/add/operator/map");
var ReportsPreviewComponent = /** @class */ (function () {
    function ReportsPreviewComponent(_reportService, cd) {
        this._reportService = _reportService;
        this.cd = cd;
        this.IsRunReportSucceeded = false;
        this.IsRunReportFailed = false;
        this.DataContext = this;
        this.IsResourcesReady = false;
        this.FilterConrolHeight = null;
        this.ReportsRunUsingWR = false;
        this.IsUsedReportsRunUsingWR = false;
        this.NumberOfRequests = 0;
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsHaveRunReportViewWorkerRolwToggleFeature = false;
        this.Retries = 0;
        this.isLoaderReady = false;
        this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub = null;
        this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = false;
        //Wait Result Stimul Timer
        this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
        this.StartTimerWaitingFirstStimulReportBuildsub = null;
        //Wait Result Stimul Timer
        this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
        this.StartTimerChangeBusyIndicatorMessageAfter50Secsub = null;
        this.ShowBusyIndicator = false;
        this.BusyIndicatorText = "";
        var idIndex = this.CurrentSession.GetNewId("ReportsPreviewComponent");
        this.ComponentId = "ReportsPreview_" + idIndex;
        this.FiltersAreaId = "ReportFiltersArea_" + idIndex;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        //ReportRunViewWorkerRole
        var featureToggle = SessionLocator_1.SessionLocator.FeatureToggles.filter(function (d) { return d.ToggleCode == "RRW" && d.TenantNumber == SessionLocator_1.SessionLocator.Tenant; })[0];
        if (featureToggle) {
            this.IsHaveRunReportViewWorkerRolwToggleFeature = true;
        }
    }
    ReportsPreviewComponent.prototype.ReportsPreview = function (GroupList, ReportList, reportTemplateLists, reportsRunUsingWR) {
        this.Report = ReportList;
        this.ReportGroup = GroupList;
        this.ReportsTemplateLists = reportTemplateLists;
        this.Title = SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal ? ReportList.Name : ReportList.LocalName;
        this.FilterControlName = ReportList.FilterControlName;
        this.ReportsRunUsingWR = reportsRunUsingWR;
        this.RunComponent();
    };
    ReportsPreviewComponent.prototype.ngAfterViewInit = function () {
        this.BuildStimulsoft();
    };
    ReportsPreviewComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ReportsPreviewComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.Report.Code == "CUPA") {
            if (this.customerViewContainerRef) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Report/Components/FilterReportComponent/CustomerPotentialActualFilterComponent', this.customerViewContainerRef)
                    .then(function (cmpRef) {
                });
            }
            else {
                this.RunComponentTimer();
            }
        }
        else {
            if (this.viewContainerRef) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load(this.Report.FilterHtmlComponentUrl, this.viewContainerRef)
                    .then(function (cmpRef) {
                    _this.ReportFilterConmponent = cmpRef.instance;
                    if (cmpRef.instance['InitializeComponent']) {
                        cmpRef.instance.InitializeComponent(_this);
                    }
                    if (cmpRef.instance['RunReportEvent']) {
                        cmpRef.instance.RunReportEvent.subscribe(function (s) {
                            if (s) {
                                _this.GenerateReport(s, false);
                            }
                        });
                    }
                    _this.isLoaderReady = true;
                    _this.BuildStimulsoft();
                });
            }
            else {
                this.RunComponentTimer();
            }
        }
    };
    ReportsPreviewComponent.prototype.BuildStimulsoft = function () {
        var _this = this;
        if (this.isLoaderReady) {
            var Component = document.getElementById(this.ComponentId);
            var filtersArea = document.getElementById(this.FiltersAreaId);
            if (Component && filtersArea) {
                this.FilterConrolHeight = filtersArea.clientHeight;
                this.StimulsoftArg = new StimulsoftArg_1.StimulsoftArg();
                this.StimulsoftArg.Tenant = SessionLocator_1.SessionLocator.Tenant;
                this.StimulsoftArg.ReportsPreviewComponent = this;
                this.StimulsoftArg.TypePage = "Report";
                this.StimulsoftArg.ShowStimulHeader = true;
                this.StimulsoftArg.ShowStimulFooter = true;
                this.StimulsoftArg.IsShowExportPrinttoPDF = true;
                this.StimulsoftArg.IsShowExportMicrosoftExcel = true;
                this.StimulsoftArg.IsShowSendButton = true;
                this.StimulsoftArg.EditableFieldLists = null;
                this.StimulsoftArg.DefaultTemplateId = this.Report.DefaultTemplateId;
                this.StimulsoftArg.ReportsTemplateLists = this.ReportsTemplateLists;
                this.StimulsoftArg.ShowReportsTemlatesLists = true;
                this.StimulsoftArg.ReportFilterConmponent = this.ReportFilterConmponent;
                this.StimulsoftArg.TemplateDescription = this.Report.Name;
                if (this.ReportsTemplateLists && this.ReportsTemplateLists.filter(function (d) { return d.Id == _this.Report.DefaultTemplateId; })[0]) {
                    this.StimulsoftArg.TemplateDescription = this.ReportsTemplateLists.filter(function (d) { return d.Id == _this.Report.DefaultTemplateId; })[0].Description;
                }
                this.ComputeSize(Component.clientWidth, Component.clientHeight);
                window.onresize = function (e) {
                    _this.ComputeSize(Component.clientWidth, Component.clientHeight);
                };
                this.IsResourcesReady = true;
                this.cd.detectChanges();
            }
        }
    };
    ReportsPreviewComponent.prototype.ComputeSize = function (clientWidth, clientHeight) {
        var width = clientWidth;
        var height = clientHeight;
        if (width < 1024) {
            width = 1024;
        }
        width = width - 20;
        height = height - 22 - 20 - this.FilterConrolHeight;
        this.StimulsoftArg.ScreenWidth = width;
        this.StimulsoftArg.ScreenHeight = height;
        if (this.StimulsoftArg && this.StimulsoftArg.StimulsoftViewerComponent) {
            this.StimulsoftArg.StimulsoftViewerComponent.SetScreenWidthAndHeight(this.StimulsoftArg.ScreenWidth, this.StimulsoftArg.ScreenHeight);
            this.cd.detectChanges();
        }
    };
    ReportsPreviewComponent.prototype.ValiditySelectedTemplate = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.ReportFliter.DefaultTemplateId)) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Please select a template");
            this.IsUsedReportsRunUsingWR = false;
            return;
        }
    };
    ReportsPreviewComponent.prototype.GenerateReport = function (filter, isloading) {
        var _this = this;
        if (!this.ShowBusyIndicator) {
            this.ShowBusyIndicator = true;
            this.ReportFliter = this.FillReportFilter(filter);
            if (!this.IsHaveRunReportViewWorkerRolwToggleFeature || (this.IsHaveRunReportViewWorkerRolwToggleFeature && this.ReportFliter.ProcessType != "GenerateReport")) {
                this.IsRunReportSucceeded = false;
                this.IsRunReportFailed = false;
                this.ValiditySelectedTemplate();
                this.StartBusyIndicator("Generating...");
                if (this.ReportsRunUsingWR && !this.IsHaveRunReportViewWorkerRolwToggleFeature) {
                    this.StartTimerWaitingFirststimulReportBuild();
                }
                this.NumberOfRequests += 1;
                this.ReportFliter.NumberOfRequests = this.NumberOfRequests;
                this._reportService.GenerateReportMethod(this.ReportFliter).subscribe(function (myResponse) {
                    var myResult = myResponse.Result;
                    if (myResult && !myResponse.HasError) {
                        if (myResult[0] && myResult[0].NumberOfRequest != _this.NumberOfRequests) {
                            return;
                        }
                    }
                    if (_this.IsUsedReportsRunUsingWR)
                        return;
                    _this.SetReportData(myResponse);
                    _this.StopBusyIndicator();
                });
            }
            else {
                this.StartBuildStimulReportViaWorkerRole(this.ReportFliter, true);
            }
        }
    };
    ReportsPreviewComponent.prototype.GenerateReportViewWorkerRole = function (filter) {
        var _this = this;
        this.IsRunReportSucceeded = false;
        this.IsRunReportFailed = false;
        this.ReportFliter = this.FillReportFilter(filter);
        this.ValiditySelectedTemplate();
        this.NumberOfRequests += 1;
        this._reportService.GenerateReportMethod(this.ReportFliter).subscribe(function (myResponse) {
            _this.IsUsedReportsRunUsingWR = false;
            _this.SetReportData(myResponse);
            _this.StopBusyIndicator();
        });
    };
    ReportsPreviewComponent.prototype.SetReportData = function (myResponse) {
        if (myResponse.HasError) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(myResponse.ErrorsArray[0]);
            this.IsRunReportFailed = true;
        }
        else {
            this.IsRunReportSucceeded = true;
            var myResult = myResponse.Result;
            if (myResult) {
                if (this.ReportFliter != null) {
                    this.StimulsoftArg.ReportFliter = this.ReportFliter;
                }
                this.StimulsoftArg.NumberOfPage = this.ReportFliter.NumberOfPage;
                this.StimulsoftArg.PartnersObslist = this.PartnersObslist;
                this.StimulsoftArg.EditableFieldLists = myResult;
                if (this.StimulsoftArg && this.StimulsoftArg.StimulsoftViewerComponent) {
                    this.StimulsoftArg.StimulsoftViewerComponent.SetStimualData();
                }
                this.cd.detectChanges();
            }
        }
    };
    ReportsPreviewComponent.prototype.FillReportFilter = function (filter) {
        if (this.StimulsoftArg)
            filter.DefaultTemplateId = this.StimulsoftArg.DefaultTemplateId;
        else
            filter.DefaultTemplateId = this.Report.DefaultTemplateId;
        filter.ReportsRunUsingWR = false;
        filter.Tenant = SessionLocator_1.SessionLocator.Tenant;
        filter.ReportName = this.Title;
        filter.FilterControlName = this.FilterControlName;
        filter.ReportDocumentId = this.Report.ReportDocumentId;
        filter.ReportCode = this.Report.Code;
        filter.DefaultTemplateVsersion = 1;
        filter.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
        filter.ReportId = this.Report.Id;
        if (this.ReportsTemplateLists) {
            var reportTemplate = this.ReportsTemplateLists.filter(function (d) { return d.Id == filter.DefaultTemplateId; })[0];
            if (reportTemplate) {
                filter.DefaultTemplateVsersion = reportTemplate.CurrentVersion;
                filter.ReportName = reportTemplate.Description;
            }
        }
        return filter;
    };
    ReportsPreviewComponent.prototype.SetFilterCotrolHeight = function (filterConrolHeight) {
        // Ayman: no need for this anymore
        //this.FilterConrolHeight = filterConrolHeight;
        // this.ResizeWindow(window.innerHeight, window.innerWidth);
    };
    ReportsPreviewComponent.prototype.BackButtonClicked = function () {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }
    };
    ReportsPreviewComponent.prototype.CleanPartnersObslist = function () {
        this.PartnersObslist = [];
    };
    ReportsPreviewComponent.prototype.AddPartner = function (partnerType, partnerId) {
        var entityPartner = new EntityPartner_1.EntityPartner(partnerType, partnerId, false);
        this.PartnersObslist.push(entityPartner);
    };
    ReportsPreviewComponent.prototype.SetReportFilterConmponent = function (reportFilterConmponent) {
        this.ReportFilterConmponent = reportFilterConmponent;
    };
    ReportsPreviewComponent.prototype.StartBuildStimulReportViaWorkerRole = function (filter, isUsedWorkerRoleAlalways) {
        var _this = this;
        if (isUsedWorkerRoleAlalways === void 0) { isUsedWorkerRoleAlalways = false; }
        filter.ReportsRunUsingWR = this.IsUsedReportsRunUsingWR = true;
        if (isUsedWorkerRoleAlalways) {
            this.StartBusyIndicator("Generating...");
            this.StartTimerChangeBusyIndicatorMessageAfter50Sec();
        }
        else {
            this.StartBusyIndicator("Report generating is taking longer than expected. Please wait", 400);
        }
        this._reportService.GenerateReportMethod(filter).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ReportFliter = myResponse.Result;
                _this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer();
            }
            else {
                filter.ReportsRunUsingWR = _this.IsUsedReportsRunUsingWR = false;
                _this.StopBusyIndicator();
                if (myResponse.HasError && myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }
            }
        });
    };
    //Stimul Soft Report Timer
    ReportsPreviewComponent.prototype.initializeStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = function () {
        return Rx_1.Observable.interval(2000).timeInterval();
    };
    ReportsPreviewComponent.prototype.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = function () {
        var _this = this;
        if (this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
            this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
        }
        this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = true;
        this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub = this.initializeStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer().subscribe(function (respose) {
            if ((_this.CurrentSession && _this.CurrentSession.isDestroingSession) || !_this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
                _this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
                _this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = false;
                return;
            }
            if (_this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
                _this._reportService.GetCheckIfStimulSoftReportIsBliud(_this.ReportFliter.ReportKey, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (_this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
                        if (pmResponse.HasError || (pmResponse.Result && pmResponse.Result.HasError) || (pmResponse.Result && pmResponse.Result.StatusCode == "D")) {
                            _this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
                            _this.IsUsedReportsRunUsingWR = false;
                            _this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = false;
                            _this.StopBusyIndicator();
                        }
                        if (!pmResponse.HasError) {
                            var result = pmResponse.Result;
                            if (result) {
                                if (result.HasError) {
                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                    messageWindow.Show(result.ExceptionMessage);
                                }
                                else if (result.StatusCode == "D") {
                                    _this.ReportFliter.ProcessType = "ReportsRunUsingWR";
                                    _this.GenerateReportViewWorkerRole(_this.ReportFliter);
                                }
                            }
                        }
                        else {
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                messageWindow.Show(pmResponse.ErrorsArray[0]);
                            }
                        }
                    }
                });
            }
        });
    };
    ReportsPreviewComponent.prototype.initializeStartTimerWaitingFirstStimulReportBuild = function () {
        return Rx_1.Observable.interval(50000).timeInterval();
    };
    ReportsPreviewComponent.prototype.StartTimerWaitingFirststimulReportBuild = function () {
        var _this = this;
        if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
            this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
        }
        this.IsStartTimerWaitingFirstStimulReportBuildRunning = true;
        this.StartTimerWaitingFirstStimulReportBuildsub = this.initializeStartTimerWaitingFirstStimulReportBuild().subscribe(function (res) {
            if (_this.CurrentSession && _this.CurrentSession.isDestroingSession) {
                _this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                _this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
                return;
            }
            if (_this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
                _this.StartBuildStimulReportViaWorkerRole(_this.ReportFliter);
                _this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                _this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
            }
        });
    };
    ReportsPreviewComponent.prototype.initializeStartTimerChangeBusyIndicatorMessageAfter50Sec = function () {
        return Rx_1.Observable.interval(50000).timeInterval();
    };
    ReportsPreviewComponent.prototype.StartTimerChangeBusyIndicatorMessageAfter50Sec = function () {
        var _this = this;
        if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
            this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
        }
        this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = true;
        this.StartTimerChangeBusyIndicatorMessageAfter50Secsub = this.initializeStartTimerChangeBusyIndicatorMessageAfter50Sec().subscribe(function (res) {
            if (_this.CurrentSession && _this.CurrentSession.isDestroingSession) {
                _this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
                _this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
                return;
            }
            if (_this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
                _this.StartBusyIndicator("Report generating is taking longer than expected. Please wait", 400);
                _this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
                _this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
            }
        });
    };
    ReportsPreviewComponent.prototype.StartBusyIndicator = function (message, width) {
        if (message === void 0) { message = "Generating..."; }
        if (width === void 0) { width = 200; }
        this.ShowBusyIndicator = true;
        this.BusyIndicatorText = message;
        this.WidthBusyIndicator = width;
    };
    ReportsPreviewComponent.prototype.StopBusyIndicator = function () {
        if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
            this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
            this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
        }
        if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
            this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
            this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
        }
        if (this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
            this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
            this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = false;
        }
        this.ShowBusyIndicator = false;
    };
    __decorate([
        core_1.ViewChild('FiltersLocation', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ReportsPreviewComponent.prototype, "viewContainerRef", void 0);
    __decorate([
        core_1.ViewChild('CustomerChild', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ReportsPreviewComponent.prototype, "customerViewContainerRef", void 0);
    ReportsPreviewComponent = __decorate([
        core_1.Component({
            moduleId: './Report/Components/',
            selector: 'ReportsPreviewComponent',
            templateUrl: 'ReportsPreviewComponent.html',
            providers: [ReportService_1.ReportService],
        }),
        __metadata("design:paramtypes", [ReportService_1.ReportService, core_1.ChangeDetectorRef])
    ], ReportsPreviewComponent);
    return ReportsPreviewComponent;
}());
exports.ReportsPreviewComponent = ReportsPreviewComponent;
//# sourceMappingURL=ReportsPreviewComponent.js.map