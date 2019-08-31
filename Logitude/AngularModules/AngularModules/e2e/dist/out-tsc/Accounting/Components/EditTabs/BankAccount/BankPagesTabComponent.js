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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var ReconcileExternalPageExtendedPMService_1 = require("../../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService");
var ReconcileExternalPagePMService_1 = require("../../../Services/StandardPMs/ReconcileExternalPagePMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var BankAccountPMService_1 = require("../../../Services/StandardPMs/BankAccountPMService");
var BankPagesTabComponent = /** @class */ (function (_super) {
    __extends(BankPagesTabComponent, _super);
    function BankPagesTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.EntityPM = null;
        _this.ObjectTableName = "ReconcileExternalPage";
        _this.DataContext = _this;
        // Events
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.preventSelect = false;
        // Services
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._entityListService = new EntityListService_1.EntityListService();
        _this._ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService_1.ReconcileExternalPageExtendedPMService();
        _this._ReconcileExternalPagePMService = new ReconcileExternalPagePMService_1.ReconcileExternalPagePMService();
        _this._BankAccountPMService = new BankAccountPMService_1.BankAccountPMService();
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.isValidate = false;
        //#endregion
        //#region Data
        _this.columns = null;
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe(function (response) { });
        _this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe(function (response) { });
        // Set Entity
        _this.EntityPM = entityArgs.EntityPM;
        _this.SetUIProperties();
        //#region Fill Date Default Values
        var today = new Date();
        _this.ToDate = new Date();
        _this.oldToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        _this.FromDate = new Date(lastmonth);
        _this.oldFromDate = new Date(lastmonth);
        //#endregion
        _this.Listen();
        return _this;
    }
    BankPagesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
        this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res == "noselect") {
                _this.preventSelect = true;
            }
        });
    };
    BankPagesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    BankPagesTabComponent.prototype.ngOnInit = function () {
        this.BuildColumns();
        this.ReloadData();
    };
    //#region Properties
    //deferredGLAccount: GLAccountPM;
    //get DeferredGLAccount() { return this.deferredGLAccount; }
    //set DeferredGLAccount(value: GLAccountPM) {
    //    if (this.deferredGLAccount != value) {
    //        this.deferredGLAccount = value;
    //    }
    //}
    //#endregion
    BankPagesTabComponent.prototype.SetUIProperties = function () {
        //if (!this.EntityPM.TypeCode) {
        //    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        //}
    };
    Object.defineProperty(BankPagesTabComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.oldFromDate = this.fromDate;
                this.fromDate = value;
                if (!this.isValidate)
                    this.validateDates();
                else {
                    this.isValidate = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankPagesTabComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.oldToDate = this.toDate;
                this.toDate = value;
                if (!this.isValidate)
                    this.validateDates();
                else {
                    this.isValidate = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    BankPagesTabComponent.prototype.validateDates = function () {
        var _this = this;
        if (this.FromDate >= this.ToDate) {
            this.timerToken = setTimeout(function () {
                _this.UIProperties.SetValidity("ToDate", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                _this.UIProperties.SetValidity("FromDate", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                _this.CD.detectChanges();
            }, 200);
        }
        else {
            this.timerToken = setTimeout(function () {
                _this.UIProperties.SetValidity("ToDate", _this.ObjectTableName, true, "");
                _this.UIProperties.SetValidity("FromDate", _this.ObjectTableName, true, "");
                _this.CD.detectChanges();
            }, 200);
            this.LoadData();
        }
    };
    //load data after validate date
    BankPagesTabComponent.prototype.LoadData = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
            this.dateFilter = new ApiQueryFilters_1.FilterItem("FromDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            this.RefreshButtonClicked();
        }
    };
    BankPagesTabComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(searchtext)) {
            this.timerToken = setTimeout(function () {
                _this.searchFieldFilter = new ApiQueryFilters_1.FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                _this.RefreshButtonClicked();
            }, 700);
        }
        else {
            this.searchFieldFilter = null;
            this.RefreshButtonClicked();
        }
    };
    BankPagesTabComponent.prototype.ReloadData = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
    };
    BankPagesTabComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'PageNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo"),
            Styles: { width: '95px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'FromDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.FromDate"),
            Styles: { width: '115px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ToDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.ToDate"),
            Styles: { width: '115px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StartBalance',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.StartBalance"),
            Styles: { width: '105px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CloseBalance',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.CloseBalance"),
            Styles: { width: '105px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.CreateDate"),
            Styles: { width: '115px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreatedByUserName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.CreatedByUserName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal ? 'StatusName' : 'StatusLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.StatusName"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal ? 'EntryTypeEnglishName' : 'EntryTypeLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.EntryTypeEnglishName"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Event',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingPeriod.TH.Events"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        //this.CustomColumnsReady.emit(this.columns);
    };
    BankPagesTabComponent.prototype.GetRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        //#region Filters
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        else {
            return;
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        filters.PageSize = 50;
        filters.PageIndex = 0;
        filters.GetCount = true;
        filters.SortBy = "PageNo"; //FromDate
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("BankAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        //#endregion
        return this._entityListService.getByFilters("ReconcileExternalPage", filters);
    };
    //#endregion
    //#region Buttons
    BankPagesTabComponent.prototype.RefreshButtonClicked = function () {
        this.ReloadData();
    };
    BankPagesTabComponent.prototype.AddButtonClicked = function () {
        this.OpenWindow();
    };
    //#endregion
    BankPagesTabComponent.prototype.onRowSelected = function (item) {
        if (!this.preventSelect) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
                var entity = item.rowData;
                var entityId = entity.Id;
                this.OpenWindow(entity);
            }
        }
        this.preventSelect = false;
    };
    BankPagesTabComponent.prototype.OpenWindow = function (entity) {
        var _this = this;
        if (entity === void 0) { entity = null; }
        this.CurrentSession.StartBusyIndicatorLoading();
        if (entity) {
            var entityPM;
            this._ReconcileExternalPagePMService.get(entity.Id).subscribe(function (myResult) {
                entityPM = myResult.Result;
                _this.ShowWindow(entityPM);
            });
        }
        else {
            this._ReconcileExternalPageExtendedPMService.GetDraftPage(this.EntityPM.Id).subscribe(function (myResult) {
                var draftPage = myResult.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(draftPage)) {
                    _this.CurrentSession.StopBusyIndicator();
                    var msg = new MessageWindow_1.MessageWindow();
                    //msg.Title = "Error";
                    msg.Width = 360;
                    msg.RTL = _this.isRTL;
                    //msg.ShowErrorIcon = true;
                    msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.O.CantNewBankPageDraft") + " (" + draftPage.PageNo + ") ");
                }
                else {
                    _this.ShowWindow(entity);
                }
            });
        }
    };
    BankPagesTabComponent.prototype.ShowWindow = function (entity) {
        var _this = this;
        if (entity === void 0) { entity = null; }
        // get bank account, then open window
        this.CurrentSession.StartBusyIndicatorLoading();
        this._BankAccountPMService.get(this.EntityPM.Id).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            var bankAccount = myResult.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(bankAccount)) {
                //SHOW WNIDOW
                var windowTitle = entity ? (TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo") + " " + entity.PageNo) : TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewPage");
                var windowArgs = {};
                windowArgs.entity = entity;
                windowArgs.BankAccountId = _this.EntityPM.Id;
                windowArgs.GLAccountId = bankAccount.GLAccountId;
                windowArgs.BankAccount = bankAccount;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 1000;
                logWindow.Height = 600;
                logWindow.Title = windowTitle;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(function ($event) { return _this.ReloadData(); });
                logWindow.Show('./Accounting/Components/NewEntity/AddEditRecoExPageComponent');
                //
            }
            else {
                console.error("ERROR!! no bank account found!!!!");
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], BankPagesTabComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], BankPagesTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    BankPagesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BankPagesTabComponent.html'
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], BankPagesTabComponent);
    return BankPagesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BankPagesTabComponent = BankPagesTabComponent;
//# sourceMappingURL=BankPagesTabComponent.js.map