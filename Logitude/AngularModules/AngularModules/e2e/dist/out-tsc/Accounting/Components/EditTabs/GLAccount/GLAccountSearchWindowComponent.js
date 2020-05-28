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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var GLAccountSearchWindowComponent = /** @class */ (function (_super) {
    __extends(GLAccountSearchWindowComponent, _super);
    function GLAccountSearchWindowComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "GLAccount";
        _this.ObjectTableId = window.ObjectTables.filter(function (f) { return f.Name === _this.ObjectTableName; })[0].Id;
        _this.DataContext = _this;
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.entityListService = new EntityListService_1.EntityListService();
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.gLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.searchFields = null;
        _this.columns = null;
        return _this;
    }
    GLAccountSearchWindowComponent.prototype.ngOnInit = function () {
        this.BuildColumns();
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        //this.MenuHeaderchangeevent.emit({ Filters: new ApiQueryFilters() , IgnoreFilter: false });
    };
    GLAccountSearchWindowComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.GLAccount;
    };
    GLAccountSearchWindowComponent.prototype.TextChanged = function (searchtext) {
        this.searchFields = searchtext;
        if (!Tools_1.AppTool.IsNullOrEmpty(searchtext)) {
            this.searchFields = this.searchFields.trim();
            if (this.searchFields != null && this.searchFields != undefined)
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        }
        else {
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        }
    };
    GLAccountSearchWindowComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        filters.GetAll = true;
        filters.GetCount = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchFields)) {
            filters.addAdditionalFilter("SearchFields", this.searchFields, null, null, "Contains", false, false, false, "string");
        }
        filters.addAdditionalFilter("ParentAccountId", "22", null, null, "IsNull", false, false, false, "string");
        filters.addAdditionalFilter("Inactive", false, null, null, "Equals", false, false, false, "boolean");
        // if (this.EntityPM.ParentAccountId != null) {
        filters.addAdditionalFilter("IsParent", "11", null, null, "Equals", true, false, false, "string");
        // }
        filters.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string");
        filters.addAdditionalFilter("AccountTypeCode", this.EntityPM.AccountTypeCode, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("ChartOfAccountsId", this.EntityPM.ChartOfAccountsId, null, null, "Equals", false, false, false, "string");
        return this.entityListService.getByFilters("GLAccount", filters);
    };
    GLAccountSearchWindowComponent.prototype.OnRowSelected = function ($event) {
        var _this = this;
        this.ValidationErrorsList = [];
        if ($event != null) {
            var entityList = $event.rowData;
            //console.log("row clicked : ", entityList, selectedEntityId);
            // var args = entityList.DisplayNumber + ',' + entityList.Id;
            this.gLAccountExtendedListService.SetParentAccountId(entityList.Id, this.EntityPM.Id).subscribe(function (myResponse) {
                if (myResponse) {
                    if (!myResponse.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit(entityList);
                    }
                    else {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                }
            });
        }
    };
    GLAccountSearchWindowComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'DisplayNumber',
            DataTypeCode: 'string',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('GLAccount.F.DisplayNumber'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
        });
        this.columns.push({
            FieldName: 'EnglishName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('GLAccount.F.EnglishName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
        });
        this.columns.push({
            FieldName: 'LocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('GLAccount.F.LocalName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
        });
        this.columns.push({
            FieldName: 'CurrencyCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('GLAccount.F.CurrencyCode'),
            IsCustomTemplate: true,
            Styles: { width: '80px' },
        });
        this.columns.push({
            FieldName: 'RevenueExpenseName',
            DataTypeCode: 'string',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('GLAccount.F.RevenueExpenseName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
        });
        this.columns.push({
            FieldName: 'ChartOfAccountsName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('GLAccount.F.ChartOfAccountsName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
        });
    };
    GLAccountSearchWindowComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], GLAccountSearchWindowComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], GLAccountSearchWindowComponent.prototype, "MenuHeaderchangeevent", void 0);
    GLAccountSearchWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GLAccountSearchWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GLAccountSearchWindowComponent);
    return GLAccountSearchWindowComponent;
}(BaseComponent_1.BaseComponent));
exports.GLAccountSearchWindowComponent = GLAccountSearchWindowComponent;
//# sourceMappingURL=GLAccountSearchWindowComponent.js.map