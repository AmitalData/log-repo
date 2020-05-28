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
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ManageRecoTabComponent = /** @class */ (function (_super) {
    __extends(ManageRecoTabComponent, _super);
    function ManageRecoTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.EntityPM = null;
        _this.ObjectTableName = "ExternalReconciliation";
        _this.DataContext = _this;
        _this.isRTL = false;
        // Events
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        // Services
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._entityListService = new EntityListService_1.EntityListService();
        //_ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
        //_ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
        //_BankAccountPMService: BankAccountPMService = new BankAccountPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.TabSelectedEvent = null;
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
        // Set Entity
        _this.EntityPM = entityArgs.EntityPM;
        _this.SetUIProperties();
        //#region Default date filter value
        var today = new Date();
        _this.ToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1); // month backward 
        _this.FromDate = new Date(lastmonth);
        //#endregion
        _this.Listen();
        return _this;
    }
    ManageRecoTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
            //
            if (this.TabSelectedEvent == null) {
                this.TabSelectedEvent = this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                    if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "BAMR") {
                            _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                            _this.ReloadData();
                        }
                    }
                });
            }
        }
    };
    ManageRecoTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
    };
    ManageRecoTabComponent.prototype.SetUIProperties = function () {
        //if (!this.EntityPM.TypeCode) {
        //    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        //}
    };
    ManageRecoTabComponent.prototype.ngOnInit = function () {
        this.BuildColumns();
        this.ReloadData();
    };
    Object.defineProperty(ManageRecoTabComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                    this.dateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                    this.ReloadData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ManageRecoTabComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                    this.dateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                    this.ReloadData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ManageRecoTabComponent.prototype.TextChanged = function (searchtext) {
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
    ManageRecoTabComponent.prototype.ReloadData = function () {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
    };
    ManageRecoTabComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'ReconciliationNumber',
            DataTypeCode: 'String',
            //Display: 'Reconciliation No.',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ExternalReconciliation.F.ReconciliationNumber"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'DateTime',
            //Display: 'Create Date',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ExternalReconciliation.F.CreateDate"),
            Styles: { width: '130px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreatedByUserName',
            DataTypeCode: 'String',
            //Display: 'Created By',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ExternalReconciliation.F.CreatedByUserName"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'IsCancelled',
            DataTypeCode: 'boolean',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ExternalReconciliation.F.IsCancelled"),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'ManageReconciliationListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ManageReconciliationListTemplate',
        });
        //this.CustomColumnsReady.emit(this.columns);
    };
    ManageRecoTabComponent.prototype.GetRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
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
        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("GLAccountId", this.EntityPM.GLAccountId, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        //#endregion
        return this._entityListService.getByFilters("ExternalReconciliation", filters);
    };
    //#endregion
    //#region Buttons
    ManageRecoTabComponent.prototype.RefreshButtonClicked = function () {
        this.ReloadData();
    };
    //#endregion
    // returned value{ colDef, colIndex, rowData, rowIndex }
    ManageRecoTabComponent.prototype.onRowSelected = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            var lineData = item.rowData;
            var entityId = lineData.Id;
            this.OpenReco(entityId);
        }
    };
    ManageRecoTabComponent.prototype.OpenReco = function (id) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ExternalReconciliation' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    _this.ReloadData();
                });
            });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ManageRecoTabComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ManageRecoTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    ManageRecoTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ManageRecoTabComponent.html'
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], ManageRecoTabComponent);
    return ManageRecoTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ManageRecoTabComponent = ManageRecoTabComponent;
//# sourceMappingURL=ManageRecoTabComponent.js.map