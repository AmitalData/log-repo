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
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Args_1 = require("../../../../Infrastructure/Args");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var MiscPageComponent = /** @class */ (function () {
    function MiscPageComponent() {
        var _this = this;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.isRTL = false;
        this.isScreenLoaded = false;
        this.IsYEARTRANSFERVisibile = false;
        this.IsGEN1000MENUVisibile = false;
        this.IsRECV1000MENUVisibile = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("OpenFormatReport").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("TaxReport").subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("TaxDeductionReport").subscribe(function (response) {
                    var yearTransFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "YEARTRANSFERMENU");
                    console.log("YEARTRANSFERMENU Feature:" + yearTransFeature);
                    if (yearTransFeature) {
                        _this.IsYEARTRANSFERVisibile = true;
                    }
                    var IsGEN1000MENUVisibile = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "GEN1000MENU");
                    console.log("GEN1000MENU Feature:" + IsGEN1000MENUVisibile);
                    if (IsGEN1000MENUVisibile) {
                        _this.IsGEN1000MENUVisibile = true;
                    }
                    var IsRECV1000MENUVisibile = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "RECV1000MENU");
                    console.log("GEN1000MENU Feature:" + IsRECV1000MENUVisibile);
                    if (IsRECV1000MENUVisibile) {
                        _this.IsRECV1000MENUVisibile = true;
                    }
                    _this.isScreenLoaded = true;
                    _this.CurrentSession.StopBusyIndicator();
                });
            });
        });
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    MiscPageComponent.prototype.ngAfterViewInit = function () {
        this.LoadAllScreenData();
    };
    MiscPageComponent.prototype.LoadAllScreenData = function () {
        this.ReloadUsersQuery();
    };
    MiscPageComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    MiscPageComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
    };
    MiscPageComponent.prototype.ViewAccountingQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            var tableName = "";
            var listArgs = new Args_1.ListComponentArgs();
            switch (myQueryCode) {
                case "AllOpenFormats":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("OpenFormatReport");
                        tableName = "OpenFormatReport";
                        break;
                    }
                case "ALLTAXREPORTS":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReport");
                        tableName = "TaxReport";
                        break;
                    }
                case "ALLTaxDeductionReports": {
                    displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("TaxDeductionReport");
                    tableName = "TaxDeductionReport";
                    break;
                }
                case "ACPD": {
                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe(function (response) {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 750;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.AccountingPeriods");
                        logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingPeriodsComponent');
                    });
                    break;
                }
                case "ACYT": {
                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe(function (response) {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 300;
                        logitudeWindow.Title = "Year Transfer";
                        logitudeWindow.Show('./Accounting/Components/Maintenance/YearTransferComponent');
                    });
                    break;
                }
                default: {
                    break;
                }
            }
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = tableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Misc");
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    MiscPageComponent.prototype.Generate1000 = function () {
        this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Generate1000");
            logitudeWindow.Show('./Accounting/Components/Maintenance/Generate1000Component');
        });
    };
    MiscPageComponent.prototype.Receiving1000 = function () {
        this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 650;
            logitudeWindow.Height = 350;
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Receiving1000");
            logitudeWindow.Show('./Accounting/Components/Maintenance/Receiving1000Component');
        });
    };
    MiscPageComponent.prototype.RunNewOpenFormatReportWizard = function () {
        var _this = this;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewOpenFormatReport");
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 220;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewOpenFormatReportComponent');
    };
    MiscPageComponent.prototype.RunNewTaxReport = function () {
        var _this = this;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewOpenFormatReport");
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewTaxReportComponent');
    };
    MiscPageComponent.prototype.RunNewTaxDeduction = function () {
        var _this = this;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewOpenFormatReport");
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewTaxDeductionReportComponent');
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], MiscPageComponent.prototype, "ReloadUserQueries", void 0);
    MiscPageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MiscPageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MiscPageComponent);
    return MiscPageComponent;
}());
exports.MiscPageComponent = MiscPageComponent;
//# sourceMappingURL=MiscPageComponent.js.map