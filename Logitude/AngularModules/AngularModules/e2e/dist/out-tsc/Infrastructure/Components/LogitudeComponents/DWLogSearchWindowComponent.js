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
//import {NgForm, NgStyle, NgFormControl, CORE_DIRECTIVES, FORM_DIRECTIVES,  FormBuilder, ControlGroup, Validators, Control} from '@angular/common';
//import {Http, HTTP_PROVIDERS, Response} from '@angular/http';
var http_1 = require("@angular/http");
var ServiceArgs_1 = require("../../../Infrastructure/DataContracts/ServiceArgs");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var EntityPMService_1 = require("../../../Infrastructure/Services/EntityPMService");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var Tools_1 = require("../../../Infrastructure/Tools");
var DWQueryBuilderComponent_1 = require("../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ComponentArgs_1 = require("../../../Infrastructure/DataContracts/ComponentArgs");
var ParameterComponentArgs_1 = require("../../../Infrastructure/DataContracts/ParameterComponentArgs");
;
var DWLogSearchWindowComponent = /** @class */ (function (_super) {
    __extends(DWLogSearchWindowComponent, _super);
    function DWLogSearchWindowComponent() {
        var _this = _super.call(this) || this;
        _this.SearchFieldchangeevent = new core_1.EventEmitter();
        _this.ItemSelected = new core_1.EventEmitter();
        _this.SearchText = "Search";
        _this.DataContext = _this;
        _this.columns = [];
        _this.ObjectFields = [];
        _this.AddButtonVisibility = false;
        _this.items = [];
        _this.Args = new CustomEntityArgs();
        _this.ShowInActive = false;
        _this.PartnerTypes = [];
        _this.ObjectTableNamePluralName = '';
        _this.IsAddDisabled = true;
        _this.IsEditDisabled = true;
        _this.preventSelect = false;
        _this.LovPartnerTypes = [];
        _this.IsAddToggleVisible = false;
        _this.IsAddBtnVisible = true;
        _this.IsAddUSWarehouseVisible = false;
        _this.DisplayFieldsFromList = null;
        _this.SecondListHeaderItems = [];
        _this.SecondListValueItems = [];
        _this.MultiSelectedValueLists = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataSource = {
            pageSize: 20,
            rowCount: null,
            //sortingCol: "CreateDateTime",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this._entityListService = new EntityListService_1.EntityListService;
        _this.entityPMService = new EntityPMService_1.EntityPMService;
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.PseventRowSelectEventSub = _this.CurrentSession.PseventRowSelectEvent.subscribe(function (res) {
            if (res == _this.ObjectTableName) {
                _this.preventSelect = true;
            }
        });
        _this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res && res.ComponentName == "DWLogSearchAddFieldsComponent" && res.IsFirstRequest && res.Item) {
                var item = res.Item;
                res.IsFirstRequest = false;
                var newItem = new DWQueryBuilderComponent_1.MultiSelectedValue();
                var key = "";
                var i = 0;
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.CurrentSession.Sessionkey)) {
                    if (ComponentArgs_1.ComponentArgs && ComponentArgs_1.ComponentArgs.ComponentLists) {
                        var sessionkey = _this.CurrentSession.Sessionkey + "DWLogSearchWindow";
                        var Component = ComponentArgs_1.ComponentArgs.ComponentLists.filter(function (d) { return d.key == sessionkey; })[0];
                        if (Component) {
                            var myComponent = Component.Component;
                            if (myComponent) {
                                myComponent.SecondListHeaderItems.forEach(function (field) {
                                    if (i != 0)
                                        key = i.toString();
                                    var valueDetails = new DWQueryBuilderComponent_1.ValueDetails();
                                    valueDetails.Header = field;
                                    valueDetails.Row = item["Field" + key];
                                    newItem["Value" + key] = valueDetails;
                                    i += 1;
                                });
                                myComponent.SecondListValueItems.push(newItem);
                            }
                        }
                    }
                }
            }
        });
        return _this;
    }
    DWLogSearchWindowComponent.prototype.ngOnInit = function () {
        //this.ParentTableName = this.GetObjectTableName(this.ObjectTableName);
        this.BuildColumns();
        //this.ColumnsReady.emit("");
    };
    DWLogSearchWindowComponent.prototype.ngOnDestroy = function () {
        this.PseventRowSelectEventSub.unsubscribe();
        //this.CurrentSession.PseventRowSelectEvent.unsubscribe(); // this line commented, it cause object unsubscribed error
    };
    DWLogSearchWindowComponent.prototype.SetWindowArgs = function (args) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            this.CurrentSession.Sessionkey = Guid_1.Guid.newGuid();
        }
        ComponentArgs_1.ComponentArgs.AddComponent(new ParameterComponentArgs_1.ParameterComponentArgs(this.CurrentSession.Sessionkey + "DWLogSearchWindow", this));
        this.ObjectTableName = args.ObjectTableName; // lookup table
        this.ObjectFieldName = args.DisplayFieldsFromList;
        this.LOVAdditionalColumns = this.BuildAdditionalColumns(args.LOVAdditionalColumns);
        this.ViewModel = args.DataContext;
        if (this.ViewModel) {
            this.MultiSelectedValueLists = this.ViewModel.MultiSelectedValueLists;
        }
        if (!this.MultiSelectedValueLists) {
            this.MultiSelectedValueLists = [];
        }
        this.SecondListHeaderItems = [];
        this.SecondListValueItems = [];
        this.Args = args;
        this.BuildSecondListHeader();
        this.BuildSecondListValues();
    };
    DWLogSearchWindowComponent.prototype.BuildAdditionalColumns = function (columns) {
        var _this = this;
        var result = "";
        if (columns) {
            var headerLists = [];
            var additionalColumns = columns.split(',');
            if (additionalColumns.length > 0) {
                additionalColumns.forEach(function (field) {
                    if (field != _this.ObjectFieldName) {
                        if (!headerLists.filter(function (d) { return d == field; })[0]) {
                            headerLists.push(field);
                        }
                    }
                });
                headerLists.forEach(function (field) {
                    result += field + ",";
                });
                result += "@";
                result = result.replace(",@", "").replace("@", "");
            }
        }
        return result;
    };
    DWLogSearchWindowComponent.prototype.BuildColumns = function () {
        var _this = this;
        this.columns = [];
        var AdditionalColumns = [];
        if (this.LOVAdditionalColumns) {
            AdditionalColumns = this.LOVAdditionalColumns.split(',');
        }
        this.columns.push({
            FieldName: 'Field',
            DataTypeCode: 'text',
            Display: this.ObjectFieldName.replace('[', '').replace(']', ''),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'DWLogSearchWindowFieldsComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/DWLogSearchWindowFieldsComponent',
        });
        if (AdditionalColumns.length > 0) {
            var index = 1;
            AdditionalColumns.forEach(function (field) {
                _this.columns.push({
                    FieldName: 'Field' + index,
                    DataTypeCode: 'text',
                    Display: field.replace('[', '').replace(']', ''),
                    Styles: { width: '120px' },
                    IsCustomTemplate: true,
                    HtmlListComponentName: 'DWLogSearchWindowFieldsComponent',
                    HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/DWLogSearchWindowFieldsComponent',
                });
                index++;
            });
        }
        this.columns.push({
            FieldName: "Add",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '40px' },
            HtmlListComponentName: 'DWLogSearchAddFieldsComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/DWLogSearchAddFieldsComponent',
        });
    };
    DWLogSearchWindowComponent.prototype.TextChanged = function (searchtext) {
        if (searchtext != null && searchtext != undefined) {
            this.searchFields = searchtext;
            this.searchFields = this.searchFields.trim();
            if (this.searchFields != null && this.searchFields != undefined)
                this.SearchFieldchangeevent.emit(this.searchFields);
        }
        else {
            this.SearchFieldchangeevent.emit("");
        }
    };
    DWLogSearchWindowComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        //if (filters == null) {
        if (filters === void 0) { filters = null; }
        if (this.QueryFilterItems != null && this.QueryFilterItems != undefined) {
            filters = this.QueryFilterItems;
        }
        else {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        //}
        filters.Filter1Name = this.ObjectTableName;
        filters.Filter2Name = this.ObjectFieldName;
        filters.Filter3Name = this.LOVAdditionalColumns;
        if (this.LOVAdditionalColumns) {
            filters.Filter3Name = this.LOVAdditionalColumns;
        }
        //else {
        //    filters.Filter3Name = null;
        //}
        if (searchfields) {
            filters.Filter2Value = searchfields;
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.ObjectTableName = this.ObjectTableName;
        //if (filters.AdditionalFilters.filter(a => a.FieldName == "SearchFields").length > 0) {
        //    filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
        //}
        //if (searchfields) {
        //    filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
        //}
        return this._entityListService.getDWDimByFilters(this.ObjectTableName, filters);
    };
    DWLogSearchWindowComponent.prototype.onRowSelected = function ($event) {
        if (this.preventSelect == false) {
            if ($event != null) {
                var entityList = $event.rowData;
                var selectedEntity = $event.rowData["Field"];
                // this.CurrentSession.CloseCurrentWindowEmit(selectedEntity);
            }
        }
        else {
            this.preventSelect = false;
        }
    };
    DWLogSearchWindowComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    DWLogSearchWindowComponent.prototype.OkButtonClicked = function () {
        var textValue = this.GetTextValue(this.SecondListValueItems);
        if (this.ViewModel) {
            this.ViewModel.MultiSelectedValueLists = this.SecondListValueItems;
        }
        this.CurrentSession.CloseCurrentWindowEmit(textValue);
    };
    DWLogSearchWindowComponent.prototype.BuildSecondListHeader = function () {
        var _this = this;
        var test = [];
        if (this.ObjectFieldName) {
            this.SecondListHeaderItems.push(this.ObjectFieldName.replace('[', '').replace(']', ''));
        }
        var additionalColumns = [];
        if (this.LOVAdditionalColumns) {
            additionalColumns = this.LOVAdditionalColumns.split(',');
            if (additionalColumns.length > 0) {
                additionalColumns.forEach(function (field) {
                    _this.SecondListHeaderItems.push(field.replace('[', '').replace(']', ''));
                });
            }
        }
        if (this.MultiSelectedValueLists && this.MultiSelectedValueLists.length > 0) {
            var items = this.MultiSelectedValueLists[0];
            var i = "";
            var j = 0;
            while (items["Value" + i]) {
                var columnName = items["Value" + i].Header;
                if (!this.SecondListHeaderItems.filter(function (d) { return d == columnName; })[0]) {
                    this.SecondListHeaderItems.push(columnName);
                }
                j += 1;
                i = j.toString();
            }
        }
    };
    DWLogSearchWindowComponent.prototype.BuildSecondListValues = function () {
        var _this = this;
        this.SecondListValueItems = [];
        this.MultiSelectedValueLists.forEach(function (item) {
            var multiSelectedValue = new DWQueryBuilderComponent_1.MultiSelectedValue();
            var i = "";
            var j = 0;
            _this.SecondListHeaderItems.forEach(function (header) {
                var valueDetails = new DWQueryBuilderComponent_1.ValueDetails();
                valueDetails.Header = header;
                valueDetails.Row = _this.ResolveValue(item, header);
                multiSelectedValue["Value" + i] = valueDetails;
                j += 1;
                i = j.toString();
            });
            _this.SecondListValueItems.push(multiSelectedValue);
        });
    };
    DWLogSearchWindowComponent.prototype.ResolveValue = function (Values, header) {
        var i = "";
        var j = 0;
        var result = "";
        while (Values["Value" + i]) {
            if (Values["Value" + i].Header == header) {
                result = Values["Value" + i].Row;
                return result;
            }
            j += 1;
            i = j.toString();
        }
        return result;
    };
    DWLogSearchWindowComponent.prototype.GetTextValue = function (multiSelectedValueLists) {
        var textValue = "";
        if (multiSelectedValueLists) {
            multiSelectedValueLists.forEach(function (field) {
                if (field["Value"]) {
                    if (textValue)
                        textValue += ";;";
                    var rowValues = field["Value"];
                    if (rowValues)
                        textValue += rowValues.Row;
                }
            });
        }
        textValue += "@@";
        textValue = textValue.replace(";@@", "");
        textValue = textValue.replace("@@", "");
        return textValue;
    };
    DWLogSearchWindowComponent.prototype.RemoveItemFromSecondList = function (item) {
        if (item != null) {
            var index = this.SecondListValueItems.indexOf(item);
            if (index > -1) {
                this.SecondListValueItems.splice(index, 1);
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWLogSearchWindowComponent.prototype, "SearchFieldchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWLogSearchWindowComponent.prototype, "ItemSelected", void 0);
    DWLogSearchWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DWLogSearchWindow',
            templateUrl: './DWLogSearchWindowComponent.html',
            providers: [http_1.Http, ServiceArgs_1.ServiceArgs, EntityListService_1.EntityListService, EntityPMService_1.EntityPMService],
        }),
        __metadata("design:paramtypes", [])
    ], DWLogSearchWindowComponent);
    return DWLogSearchWindowComponent;
}(BaseComponent_1.BaseComponent));
exports.DWLogSearchWindowComponent = DWLogSearchWindowComponent;
var CustomEntityArgs = /** @class */ (function () {
    function CustomEntityArgs() {
        this.ObjectTableName = null;
        this.ObjectTableId = null;
        this.LOVAdditionalColumns = null;
        this.SelectedItem = null;
        this.ShowInActive = false;
        this.IsTenantZeroSearch = null;
        this.IsAllDataVisible = null;
        this.PartnerTypes = [];
        this.IsAddDisabled = true;
        this.IsEditDisabled = true;
        this.DisplayFieldsFromList = null;
    }
    return CustomEntityArgs;
}());
exports.CustomEntityArgs = CustomEntityArgs;
var AddEntityArgs = /** @class */ (function () {
    function AddEntityArgs() {
    }
    return AddEntityArgs;
}());
exports.AddEntityArgs = AddEntityArgs;
//# sourceMappingURL=DWLogSearchWindowComponent.js.map