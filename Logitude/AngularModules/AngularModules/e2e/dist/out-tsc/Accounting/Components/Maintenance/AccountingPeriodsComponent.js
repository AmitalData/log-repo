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
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var AccountingPeriodListService_1 = require("../../Services/StandardLists/AccountingPeriodListService");
var AccountingPeriodPMService_1 = require("../../Services/StandardPMs/AccountingPeriodPMService");
var AccountingPeriodExtendedListService_1 = require("../../Services/ExtendedLists/AccountingPeriodExtendedListService");
var AccountingPeriodExtendedPMService_1 = require("../../Services/ExtendedPMs/AccountingPeriodExtendedPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var AccountingPeriodsComponent = /** @class */ (function (_super) {
    __extends(AccountingPeriodsComponent, _super);
    function AccountingPeriodsComponent(_entityResourceService, entityArgs) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingPeriod";
        _this.ShowPrompt = false;
        _this.isRTL = false;
        _this.hasReadPermision = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.noPeriodMsg = "";
        _this.accountingPeriodListService = new AccountingPeriodListService_1.AccountingPeriodListService();
        _this.accountingPeriodPMService = new AccountingPeriodPMService_1.AccountingPeriodPMService();
        _this._AccountingPeriodExtendedListService = new AccountingPeriodExtendedListService_1.AccountingPeriodExtendedListService();
        _this._AccountingPeriodExtendedPMService = new AccountingPeriodExtendedPMService_1.AccountingPeriodExtendedPMService();
        // this.UIProperties.SetEnabled("Year", this.ObjectTableName, true);
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.hasReadPermision = FeatureLocator_1.FeatureLocator.HasEntityPermessions(_this.ObjectTableName, 'READ', false);
        if (_this.hasReadPermision)
            _this.UIProperties.SetEnabled("Year", _this.ObjectTableName, true);
        else
            _this.UIProperties.SetEnabled("Year", _this.ObjectTableName, false);
        return _this;
    }
    Object.defineProperty(AccountingPeriodsComponent.prototype, "Year", {
        get: function () { return this.year; },
        set: function (value) {
            if (this.year != value) {
                this.year = value;
                this.ShowPrompt = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountingPeriodsComponent.prototype.GetPeriods = function (year) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(year) && this.hasReadPermision) {
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            filters.GetAll = true;
            filters.addAdditionalFilter("Year", year, null, null, "Equal", false, false, false, "number");
            this.accountingPeriodListService.getByFilters(filters).subscribe(function (myResult) {
                var result = myResult.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(result) && result.length > 0) {
                    _this.ShowPrompt = false;
                    _this.PeriodsList = result;
                }
                else {
                    _this.PeriodsList = [];
                    _this.ShowPrompt = true;
                    var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.PeriodLinesIsNotCreated");
                    _this.noPeriodMsg = msg.replace("%Year", _this.Year + "");
                }
            });
        }
    };
    AccountingPeriodsComponent.prototype.BrowseButtonClicked = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.year)) {
            this.Year = new Date().getFullYear();
        }
        if (this.year > (new Date().getFullYear())) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.FutureYear")); //"Future Year!");
        }
        else if (this.year < 1900) {
        }
        else {
            this.ValidationErrorsList = [];
            this.GetPeriods(this.year);
        }
    };
    AccountingPeriodsComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingPeriodsComponent.prototype.EditPeriod = function (period) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(period)) {
            var windowArgs = new Args();
            windowArgs.EntityId = period.Id;
            if (period.PeriodTypeCode == "2") { // 2-Invoice
                var row = this.PeriodsList.find(function (d) { return d.PeriodTypeCode == "1"; }); // 1-Accounting
                if (row != null) {
                    windowArgs.AccountingRow = row; // attach accounting period to window to use it in logic
                }
            }
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 530;
            logWindow.Height = 400;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.AccountingPeriods");
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.GetPeriods(_this.year); });
            logWindow.Show('./Accounting/Components/Maintenance/EditAccountingPeriodComponent');
        }
    };
    AccountingPeriodsComponent.prototype.ViewPeriodEvents = function (id) {
        var _this = this;
        this.accountingPeriodPMService.get(id).subscribe(function (myResult) {
            var result = myResult.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                var entityPM = result;
                var windowArgs = new EntityArgs_1.EntityArgs();
                windowArgs.ObjectTableName = _this.ObjectTableName;
                windowArgs.EntityPM = entityPM;
                //this.entityArgs = new EntityArgs();
                _this.entityArgs.EntityPM = entityPM;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 950;
                logWindow.Height = 600;
                logWindow.Title = "Accounting Period Events";
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(function ($event) { return _this.GetPeriods(_this.year); });
                logWindow.Show('./Accounting/Components/Maintenance/AccountingPeriodEventComponent');
            }
            else {
            }
        });
    };
    AccountingPeriodsComponent.prototype.CreateRecord = function () {
        var _this = this;
        this._AccountingPeriodExtendedPMService.createDefaultPeriods(this.year).subscribe(function (myResult) {
            var result = myResult.Result;
            _this.BrowseButtonClicked();
        });
    };
    AccountingPeriodsComponent.prototype.OnKeyUp = function (key) {
        if (!Tools_1.AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.BrowseButtonClicked();
            }
        }
    };
    AccountingPeriodsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AccountingPeriodsComponent',
            templateUrl: './AccountingPeriodsComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EntityArgs_1.EntityArgs])
    ], AccountingPeriodsComponent);
    return AccountingPeriodsComponent;
}(BaseComponent_1.BaseComponent));
exports.AccountingPeriodsComponent = AccountingPeriodsComponent;
var Args = /** @class */ (function () {
    function Args() {
    }
    return Args;
}());
exports.Args = Args;
//# sourceMappingURL=AccountingPeriodsComponent.js.map