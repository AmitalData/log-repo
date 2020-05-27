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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var DocumentsFilingViewsExtService_1 = require("../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService");
var DocumentsFilingsQueryComponent = /** @class */ (function (_super) {
    __extends(DocumentsFilingsQueryComponent, _super);
    function DocumentsFilingsQueryComponent() {
        var _this = _super.call(this) || this;
        //***********************properties*************************//
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.AllowPointerEvents = 'all';
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.preventSelect = false;
        //**********************************************************//
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            //sortingCol: "CreateDateTime",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.columns = null;
        _this.BuildColumns();
        _this.CurrentSession.SubscriptionAdd(_this.CurrentSession.PseventRowSelectEvent.subscribe(function (res) {
            if (res == "document") {
                _this.preventSelect = true;
            }
        }));
        return _this;
    }
    Object.defineProperty(DocumentsFilingsQueryComponent.prototype, "DocumentTypeId", {
        get: function () { return this.documentTypeId; },
        set: function (value) {
            if (this.documentTypeId != value) {
                this.documentTypeId = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.DocumentTypeIdFilter = new ApiQueryFilters_1.FilterItem("DocumentTypeId", value, null, null, "Equals", false, false, false, "string", false);
                }
                else {
                    this.DocumentTypeIdFilter = null;
                }
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingsQueryComponent.prototype, "FromCreateDate", {
        get: function () { return this.fromCreateDate; },
        set: function (value) {
            if (this.fromCreateDate != value) {
                this.fromCreateDate = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (this.ToCreateDate) {
                        var filterValue = new Date();
                        filterValue.setUTCDate(this.ToCreateDate.getUTCDate());
                        filterValue.setUTCMonth(this.ToCreateDate.getUTCMonth());
                        filterValue.setUTCFullYear(this.ToCreateDate.getUTCFullYear());
                        filterValue.setHours(23);
                        filterValue.setMinutes(59);
                        this.FromCreateDateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", value, this.ToCreateDate, null, "Between", false, false, false, "datetime", false);
                    }
                    else {
                        this.FromCreateDateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", value, null, null, "GreaterThanOrEqual", false, false, false, "datetime", false);
                    }
                }
                else {
                    this.FromCreateDateFilter = null;
                }
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingsQueryComponent.prototype, "CustomsDocId", {
        get: function () { return this.customsDocId; },
        set: function (value) {
            if (this.customsDocId != value) {
                this.customsDocId = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.CustomsDocIdFilter = new ApiQueryFilters_1.FilterItem("CustomsDocId", value, null, null, "StartsWith", false, false, false, "string", false);
                }
                else {
                    this.CustomsDocIdFilter = null;
                }
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingsQueryComponent.prototype, "ToCreateDate", {
        get: function () { return this.toCreateDate; },
        set: function (value) {
            if (this.toCreateDate != value) {
                this.toCreateDate = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    var filterValue = new Date();
                    filterValue.setUTCDate(value.getUTCDate());
                    filterValue.setUTCMonth(value.getUTCMonth());
                    filterValue.setUTCFullYear(value.getUTCFullYear());
                    filterValue.setHours(23);
                    filterValue.setMinutes(59);
                    if (this.FromCreateDate) {
                        this.FromCreateDateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", this.FromCreateDate, filterValue, null, "Between", false, false, false, "datetime", false);
                    }
                    else {
                        this.ToCreateDateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", filterValue, null, null, "LessThanOrEqual", false, false, false, "datetime", false);
                    }
                }
                else {
                    this.ToCreateDateFilter = null;
                }
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    DocumentsFilingsQueryComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.entityListService = new EntityListService_1.EntityListService();
    };
    DocumentsFilingsQueryComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectedItem.Id);
    };
    DocumentsFilingsQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    DocumentsFilingsQueryComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        if (searchtext != null || searchtext != undefined) {
            this.timerToken = setTimeout(function () {
                _this.SearchFieldsFilter = new ApiQueryFilters_1.FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                _this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() }); // refresh grid
            }, 700);
        }
        else {
            this.SearchFieldsFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() }); // refresh grid
        }
    };
    DocumentsFilingsQueryComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        this.AllowPointerEvents = 'none';
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        var hasFileFilter = new ApiQueryFilters_1.FilterItem("HasFile", true, null, null, "Equals", false, false, false, "string", false);
        filters.AdditionalFilters.push(hasFileFilter);
        if (this.CustomsDocIdFilter) {
            filters.AdditionalFilters.push(this.CustomsDocIdFilter);
        }
        if (this.DocumentTypeIdFilter) {
            filters.AdditionalFilters.push(this.DocumentTypeIdFilter);
        }
        if (this.FromCreateDateFilter) {
            filters.AdditionalFilters.push(this.FromCreateDateFilter);
        }
        if (this.ToCreateDateFilter) {
            filters.AdditionalFilters.push(this.ToCreateDateFilter);
        }
        if (this.SearchFieldsFilter) {
            filters.AdditionalFilters.push(this.SearchFieldsFilter);
        }
        filters.PageSize = 30;
        filters.PageIndex = 0; // decremented 1 in the service
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        //filters.addAdditionalFilter("AccountingDate", true, null, null, "Between", false, false, false, "datetime");
        return new Promise(function (resolve, reject) {
            var service = new DocumentsFilingViewsExtService_1.DocumentsFilingViewsExtService();
            resolve(service.getByFilters(filters));
        });
        //return this.entityListService.getByFilters("DocumentsFiling",filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);
    };
    DocumentsFilingsQueryComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'Extension',
            DataTypeCode: 'text',
            Display: '',
            Styles: { width: '33px' },
            HtmlListComponentName: 'DocumentsFilingTemplateComponent',
            HtmlListComponentUrl: './Customs/Components/Templates/DocumentsFilingTemplateComponent',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Extension'
        });
        this.columns.push({
            FieldName: 'Code',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('DocumentsFiling.F.Code'),
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Code'
        });
        this.columns.push({
            FieldName: 'DocumentTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.CustomsDocument.CH.NameListLable'),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'DocumentTypeName'
        });
        this.columns.push({
            FieldName: 'Description',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('DocumentsFiling.F.Description'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            ServerSideSortable: true,
            SortByName: 'Description'
        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('DocumentsFiling.F.CreateDate'),
            Styles: { width: '105px' },
            HtmlListComponentName: 'DocumentsFilingTemplateComponent',
            HtmlListComponentUrl: './Customs/Components/Templates/DocumentsFilingTemplateComponent',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CreateDate'
        });
        this.columns.push({
            FieldName: 'OwnerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('DocumentsFiling.F.OwnerName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            ServerSideSortable: true,
            SortByName: 'OwnerName'
        });
        this.columns.push({
            FieldName: 'ExternalEntityReference',
            DataTypeCode: 'String',
            Display: 'מספר ישות',
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ExternalEntityReference'
        });
        this.columns.push({
            FieldName: 'CustomsDocId',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.CustomsDocument.F.CustomsDocId'),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CustomsDocId'
        });
    };
    DocumentsFilingsQueryComponent.prototype.OnRowSelected = function (item) {
        if (!this.preventSelect) {
            this.SelectedItem = item.rowData;
            this.CurrentSession.CloseCurrentWindowEmit(this.SelectedItem.Id);
        }
        else {
            this.preventSelect = false;
        }
    };
    DocumentsFilingsQueryComponent.prototype.OnDataLoaded = function (rows) {
        this.CurrentSession.StopBusyIndicator();
        this.AllowPointerEvents = 'all';
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DocumentsFilingsQueryComponent.prototype, "onQueryChangeEvent", void 0);
    DocumentsFilingsQueryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DocumentsFilingsQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DocumentsFilingsQueryComponent);
    return DocumentsFilingsQueryComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentsFilingsQueryComponent = DocumentsFilingsQueryComponent;
//# sourceMappingURL=DocumentsFilingsQueryComponent.js.map