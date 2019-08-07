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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var TenantImportComponent_1 = require("../../../Common/Components/Maintenance/TenantImportComponent");
var CachedDataManager_1 = require("../../Utilities/CachedDataManager");
var LogSearchWindowComponent = /** @class */ (function (_super) {
    __extends(LogSearchWindowComponent, _super);
    function LogSearchWindowComponent() {
        var _this = _super.call(this) || this;
        _this.SearchFieldchangeevent = new core_1.EventEmitter();
        _this.ItemSelected = new core_1.EventEmitter();
        _this.SearchText = "Search";
        _this.DataContext = _this;
        _this.columns = [];
        _this.columns1 = [];
        _this.ObjectFields = [];
        _this.AddButtonVisibility = false;
        _this.items = [];
        _this.Args = new CustomEntityArgs();
        _this.IsTenantZeroSearch = null;
        _this.IsAllDataVisible = null;
        _this.IsMyDataVisible = true;
        _this.ShowInActive = false;
        _this.PartnerTypes = [];
        _this.ObjectTableNamePluralName = '';
        _this.IsDataReady = true;
        _this.IsAddDisabled = true;
        _this.IsEditDisabled = true;
        _this.preventSelect = false;
        _this.LovPartnerTypes = [];
        _this.IsAddToggleVisible = false;
        _this.IsAddBtnVisible = true;
        _this.IsAddUSWarehouseVisible = false;
        _this.DisplayFieldsFromList = null;
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
        _this.DataSource2 = {
            pageSize: 20,
            rowCount2: null,
            //sortingCol: "CreateDateTime",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows2(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this._entityListService = new EntityListService_1.EntityListService;
        _this.entityPMService = new EntityPMService_1.EntityPMService;
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        //this.CurrentSession.SubscriptionAdd(
        _this.PseventRowSelectEventSub = _this.CurrentSession.PseventRowSelectEvent.subscribe(function (res) {
            if (res == _this.ObjectTableName) {
                _this.preventSelect = true;
            }
        });
        return _this;
        //);
    }
    LogSearchWindowComponent.prototype.ngOnInit = function () {
        this.ParentTableName = this.GetObjectTableName(this.ObjectTableName);
        this.BuildColumns();
    };
    LogSearchWindowComponent.prototype.ngOnDestroy = function () {
        this.PseventRowSelectEventSub.unsubscribe();
        //this.CurrentSession.PseventRowSelectEvent.unsubscribe(); // this line commented, it cause object unsubscribed error
    };
    LogSearchWindowComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.ObjectTableName = args.ObjectTableName; // lookup table
        this.ObjectTableId = args.ObjectTableId;
        this.ObjectTableNamePluralName = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural(this.ObjectTableName);
        this.ShowInActive = args.ShowInActive;
        this.PartnerTypes = args.PartnerTypes;
        this.Args = args;
        this.IsTenantZeroSearch = args.IsTenantZeroSearch;
        this.IsAllDataVisible = args.IsAllDataVisible;
        this.DependencyFilter1Value = args.DependencyFilter1Value;
        this.DependencyFilter2Value = args.DependencyFilter2Value;
        this.DependencyFilter3Value = args.DependencyFilter3Value;
        this.DependencyFilter1IsList = args.DependencyFilter1IsList;
        this.DependencyFilter2IsList = args.DependencyFilter2IsList;
        this.DependencyFilter3IsList = args.DependencyFilter3IsList;
        this.DependencyFilter1IsListExact = args.DependencyFilter1IsListExact;
        this.DependencyFilter2IsListExact = args.DependencyFilter2IsListExact;
        this.DependencyFilter3IsListExact = args.DependencyFilter3IsListExact;
        this.UseCompactSearch = args.UseCompactSearch;
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        this.QueryFilterItems = args.QueryFilterItems;
        this.HideAdd = args.HideAdd;
        this.IsAddDisabled = args.IsAddDisabled;
        this.IsEditDisabled = args.IsEditDisabled;
        if (!this.IsAddDisabled) {
            this.IsAddBtnVisible = true;
            if (this.ObjectTableName == "Warehouse") {
                if (this.TenantPM.Id != 0 && this.TenantPM.CountryCode == "US") {
                    this.IsAddUSWarehouseVisible = true;
                }
            }
        }
        this.DisplayFieldsFromList = args.DisplayFieldsFromList;
        if (this.IsTenantZeroSearch) {
            this.IsAllDataVisible = true;
            this.IsMyDataVisible = false;
        }
        this.LookUpTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (this.ObjectTableName == "Card" || this.ObjectTableName == "Carrier") {
            if (this.DependencyFilter1Value) {
                if (this.DependencyFilter1Value.toString().split(',').length > 1) {
                    //* there is more than one dep
                    //  show toggle button and filter list by dep fields
                    var types = this.DependencyFilter1Value.toString().split(',');
                    this.LovPartnerTypes = this.PartnerTypes.filter(function (d) { return types.indexOf(d.Id) > -1; });
                    this.LovPartnerTypes = this.LovPartnerTypes.filter(function (d) { return d.Id != "CC" && d.Id != "CO" && d.Id != "FL" && d.Id != "OT" && d.Id != "PO" && d.Id != "PT"; });
                    if (!this.IsAddDisabled) {
                        this.IsAddToggleVisible = true;
                        this.IsAddBtnVisible = false;
                    }
                }
                else {
                    //* there is only one dep
                    //show single add button
                    if (!this.IsAddDisabled)
                        this.IsAddBtnVisible = true;
                }
            }
            else {
                // no dependency, show all types
                this.LovPartnerTypes = this.PartnerTypes;
                this.LovPartnerTypes = this.LovPartnerTypes.filter(function (d) { return d.Id != "CC" && d.Id != "CO" && d.Id != "FL" && d.Id != "OT" && d.Id != "PO" && d.Id != "PT"; });
                if (!this.IsAddDisabled) {
                    this.IsAddToggleVisible = true;
                    this.IsAddBtnVisible = false;
                }
            }
        }
        else {
            if (!this.IsAddDisabled) {
                this.IsAddBtnVisible = true;
                if (SessionLocator_1.SessionLocator.Tenant == 65 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                    if (this.ObjectTableName == "ChargesType") {
                        this.IsAddBtnVisible = false;
                    }
                }
            }
            else {
                this.IsAddBtnVisible = false;
            }
        }
    };
    LogSearchWindowComponent.prototype.BuildColumns = function () {
        var _this = this;
        var objectTableId = this.ObjectTableId;
        var lookupFields;
        if (this.DisplayFieldsFromList != null && this.DisplayFieldsFromList != undefined) {
            var fields = this.DisplayFieldsFromList.split(',');
            lookupFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId == _this.LookUpTable.Id && fields.lastIndexOf(d.FieldName) > -1; });
        }
        else {
            lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayInSearchWindowList && d.ObjectTableId == _this.LookUpTable.Id; });
        }
        this.ObjectFields = lookupFields.sort(function (a, b) { return a.DisplayInSearchWindowListIndex - b.DisplayInSearchWindowListIndex; });
        this.columns1 = [];
        this.columns = [];
        for (var i = 0; i < this.ObjectFields.length; i++) {
            this.columns1.push({
                FieldName: this.ObjectFields[i].FieldName,
                IsCustomTemplate: true,
                DataTypeCode: this.ObjectFields[i].DataTypeCode,
                Display: TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectFields[i].ListTextCodeCode),
                Styles: { width: '100px' },
                HtmlListComponentName: this.ObjectFields[i].HtmlListComponentName,
                HtmlListComponentUrl: this.ObjectFields[i].HtmlListComponentUrl,
                ColumnHeaderTemplateName: this.ObjectFields[i].ColumnHeaderTemplateName,
                ServerSideSortable: true,
            });
            this.columns.push({
                FieldName: this.ObjectFields[i].FieldName,
                IsCustomTemplate: true,
                DataTypeCode: this.ObjectFields[i].DataTypeCode,
                Display: TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectFields[i].ListTextCodeCode),
                Styles: { width: '100px' },
                HtmlListComponentName: this.ObjectFields[i].HtmlListComponentName,
                HtmlListComponentUrl: this.ObjectFields[i].HtmlListComponentUrl,
                ColumnHeaderTemplateName: this.ObjectFields[i].ColumnHeaderTemplateName,
                ServerSideSortable: true,
            });
        }
        ////Edit Buttons
        // my == columns1
        if (this.IsEditDisabled == false) {
            this.columns1.push({
                FieldName: 'EditBtn,' + this.ObjectTableName,
                DataTypeCode: 'String',
                IsCustomTemplate: true,
                Display: '',
                Styles: { width: '100px' },
                HtmlListComponentName: 'LogSearchWindowButtonsComponent',
                HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/LogSearchWindowButtonsComponent',
                ServerSideSortable: true,
            });
        }
        this.CurrentSession.SessionEvent.subscribe(function ($event) {
            if ($event == 'ok from LSWBC') {
                _this.SearchFieldchangeevent.emit("");
            }
        });
    };
    LogSearchWindowComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        //if (filters == null) {
        if (this.QueryFilterItems != null && this.QueryFilterItems != undefined) {
            filters = this.QueryFilterItems;
        }
        else {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        //}
        filters = this.SetDependencyProperties(filters);
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = this.TenantPM.Id;
        if (filters.AdditionalFilters.filter(function (a) { return a.FieldName == "SearchFields"; }).length > 0) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "SearchFields"; });
        }
        if (searchfields) { //&& !this.UseCompactSearch
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
        }
        var parentName = this.GetObjectTableName(this.ObjectTableName);
        var parenttable = window.ObjectTables.filter(function (d) { return d.Name === parentName; })[0];
        var originalTable = this.ObjectTable;
        if (originalTable.Name == "Carrier") {
            originalTable = window.ObjectTables.filter(function (d) { return d.Name === "Card"; })[0];
        }
        var inactiveField = window.ObjectFields.filter(function (d) { return d.FieldName.toLowerCase() === "inactive" && (d.ObjectTableId === parenttable.Id || d.ObjectTableId == originalTable.Id); })[0];
        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);
        }
        //var x = filters.AdditionalFilters.filter(a => a.FieldName == "InActive");
        //if (x.length == 0) {
        //    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");
        //}
        var rowsObjectTable = this.ObjectTableName;
        //if (this.UseCompactSearch) {
        //    filters.addAdditionalFilter("CompactSearchField", searchfields, null, null, "Contains", false, false, false, null);
        //    return this._entityListService.getByCompactFilters(rowsObjectTable, filters);
        //}
        //else {
        return this._entityListService.getByFilters(rowsObjectTable, filters);
        //}
    };
    //tenent 0
    LogSearchWindowComponent.prototype.getRows2 = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        var _this = this;
        if (filters === void 0) { filters = null; }
        //if (filters == null) {
        if (this.QueryFilterItems != null && this.QueryFilterItems != undefined) {
            filters = this.QueryFilterItems;
        }
        else {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        //}
        filters = this.SetDependencyProperties(filters);
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = 0;
        if (filters.AdditionalFilters.filter(function (a) { return a.FieldName == "SearchFields"; }).length > 0) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "SearchFields"; });
        }
        if (searchfields) { //&& !this.UseCompactSearch
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
        }
        var parentName = this.GetObjectTableName(this.ObjectTableName);
        var parenttable = window.ObjectTables.filter(function (d) { return d.Name === parentName; })[0];
        var inactiveField = window.ObjectFields.filter(function (d) { return d.FieldName.toLowerCase() === "inactive" && (d.ObjectTableId === parenttable.Id || d.ObjectTableId === _this.ObjectTable.Id); })[0];
        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);
        }
        //var x = filters.AdditionalFilters.filter(a => a.FieldName == "InActive");
        //if (x.length == 0) {
        //    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");
        //}
        var rowsObjectTable = this.ObjectTableName;
        //if (this.UseCompactSearch) {
        //  filters.addAdditionalFilter("CompactSearchField", searchfields, null, null, "Contains", false, false, false, null);
        //  return this._entityListService.getByCompactFilters(rowsObjectTable, filters);
        // }
        //else {
        return this._entityListService.getByFilters(rowsObjectTable, filters);
        //  }
    };
    //#endregion 
    //#region Dependency
    LogSearchWindowComponent.prototype.SetDependencyProperties = function (apiQueryFilters) {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (table.DependencyFilter1 != null && table.DependencyFilter1 != undefined) {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                var dependencyField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter1; })[0];
                if (dependencyField != null) {
                    var fieldOperator = dependencyField.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter1IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter1IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    //var depValue1: any = resolver.ResolveValue(dependencyField.DataTypeCode, DependencyProperty1 == null ? null : DependencyProperty1.ToString());
                    //apiQueryFilters.SetFilter(table.DependencyFilter1, depValue1, dependencyField.IsCustomFilter, fieldOperator, null, dependencyField.DisplayInList);
                    apiQueryFilters.addAdditionalFilter(dependencyField.FieldName, this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value), null, null, fieldOperator, dependencyField.IsCustomFilter, dependencyField.DisplayInList, dependencyField.IsCustom, dependencyField.DataTypeCode);
                    //apiQueryFilters.Filter2Name = dependencyField.FieldName;
                    //apiQueryFilters.Filter2Operator = fieldOperator;
                    //apiQueryFilters.Filter2Value = this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value);
                }
            }
        }
        if (table.DependencyFilter2 != null && table.DependencyFilter2 != undefined) {
            if (this.DependencyFilter2Value != null && this.DependencyFilter2Value != undefined) {
                var dependencyField2 = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter2; })[0];
                if (dependencyField2 != null) {
                    var fieldOperator = dependencyField2.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter2IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter2IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    //var depValue1: any = resolver.ResolveValue(dependencyField.DataTypeCode, DependencyProperty1 == null ? null : DependencyProperty1.ToString());
                    //apiQueryFilters.SetFilter(table.DependencyFilter1, depValue1, dependencyField.IsCustomFilter, fieldOperator, null, dependencyField.DisplayInList);
                    apiQueryFilters.addAdditionalFilter(dependencyField2.FieldName, this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value), null, null, fieldOperator, dependencyField2.IsCustomFilter, dependencyField2.DisplayInList, dependencyField2.IsCustom, dependencyField2.DataTypeCode);
                    //apiQueryFilters.Filter3Name = dependencyField2.FieldName;
                    //apiQueryFilters.Filter3Operator = fieldOperator;
                    //apiQueryFilters.Filter3Value = this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value);
                }
            }
        }
        if (table.DependencyFilter3 != null && table.DependencyFilter3 != undefined) {
            if (this.DependencyFilter3Value != null && this.DependencyFilter3Value != undefined) {
                var dependencyField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter3; })[0];
                if (dependencyField != null) {
                    var fieldOperator = dependencyField.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter3IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter3IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    apiQueryFilters.addAdditionalFilter(dependencyField.FieldName, this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter3Value), null, null, fieldOperator, dependencyField.IsCustomFilter, dependencyField.DisplayInList, dependencyField.IsCustom, dependencyField.DataTypeCode);
                }
            }
        }
        return apiQueryFilters;
    };
    LogSearchWindowComponent.prototype.GetObjectTableName = function (parentObjectName) {
        var dep = this.DependencyFilter1Value != null ? this.DependencyFilter1Value.toString() : "";
        return this.GetObjectTableNameForDependency(dep, parentObjectName);
    };
    LogSearchWindowComponent.prototype.GetObjectTableNameForDependency = function (dependency, parentObjectName) {
        var partnerType = this.PartnerTypes.filter(function (p) { return p.Id.toLowerCase() == dependency.toLowerCase(); })[0];
        if (partnerType != null && partnerType != undefined) {
            var name = partnerType.Name.replace(" ", "");
            var table = window.ObjectTables.filter(function (d) { return d.Name.toLowerCase() === name.toLocaleLowerCase(); })[0];
            if (table != null) {
                return table.Name;
            }
            else {
                return parentObjectName;
            }
        }
        else {
            return parentObjectName;
        }
    };
    LogSearchWindowComponent.prototype.GetFieldValue = function (dataTypeCode, value) {
        if (value == null || value == undefined) {
            return null;
        }
        switch (dataTypeCode.toLowerCase()) {
            case "ntext":
            case "text":
                {
                    return value;
                }
            case "datetime":
                {
                    var date;
                    date = new Date(value);
                    return date;
                }
            case "boolean":
                {
                    var bb;
                    if (typeof (value) == "string") {
                        if (value.toLowerCase() == 'false') {
                            bb = false;
                        }
                        else if (value.toLowerCase() == 'true') {
                            bb = true;
                        }
                    }
                    else {
                        bb = value;
                    }
                    return bb;
                }
            case "integer":
            case "double":
            case "decimal":
                {
                    var nn;
                    nn = Number(value);
                    return nn;
                }
            default:
                {
                    return value;
                }
        }
    };
    //#endregion
    //#region Buttons + Handlers 
    LogSearchWindowComponent.prototype.AddButtonClicked = function () {
        //if (this.isAddDisabled)
        //    return;
        this.NewEntityMethod(this.ObjectTableName);
    };
    LogSearchWindowComponent.prototype.NewEntityMethod = function (objectTableName) {
        var _this = this;
        objectTableName = this.GetObjectTableName(objectTableName);
        var originalTable = window.ObjectTables.filter(function (d) { return d.Name.toLowerCase() === objectTableName.toLocaleLowerCase(); })[0];
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "NEW") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("You have no permission to add a new entity of this type.");
            return;
        }
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "Module") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Your package doesn't include this module..");
            return;
        }
        if (this.TenantPM.Id != 0) {
            if (objectTableName == "Port" || objectTableName == "Airline" || objectTableName == "Carrier" || objectTableName == "ShippingLine") {
                var windowTitle = "Add " + objectTableName;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                var args = new TenantImportComponent_1.ImportEntityArgs();
                args.ObjectTableId = this.LookUpTable.Id;
                args.ObjectTableName = objectTableName;
                logWindow.WindowArgs = args;
                logWindow.Width = 1000;
                logWindow.Height = 600;
                logWindow.Title = windowTitle;
                logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    var tablename = _this.GetObjectTableName(_this.ObjectTableName);
                    CachedDataManager_1.CachedDataManager.RefreshTableData(tablename, true);
                    _this.OnNewEntityWindowClosed($event);
                });
                return;
            }
        }
        if (originalTable.IsNewWizard) {
            this.RunNewEntityWizard(originalTable.NewWizardComponentPath, originalTable.Name);
        }
        else {
            this.RunNewGenaricEntity();
        }
    };
    LogSearchWindowComponent.prototype.RunNewEntityWizard = function (newWizardComponentPath, originalTableName) {
        //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        var _this = this;
        var componentPath = newWizardComponentPath; //this.LookUpTable.NewWizardComponentPath;
        if (componentPath != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            switch (this.ObjectTableName) {
                case "Customs.Client": {
                    logWindow.Width = 800;
                    logWindow.Height = 600;
                    logWindow.IsShowCloseButton = true;
                    break;
                }
                case "Customs.Declaration":
                case "Customs.PaymentOrder":
                    {
                        logWindow.Width = 400;
                        logWindow.Height = 300;
                        logWindow.IsShowCloseButton = true;
                        break;
                    }
                case "Customs.CustomsVendor": {
                    logWindow.IsShowCloseButton = true;
                    break;
                }
                case "User": {
                    logWindow.Width = 965;
                    logWindow.Height = 600;
                    break;
                }
            }
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(originalTableName));
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            logWindow.Show(componentPath);
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Width = 450;
            messageWindow.Height = 190;
            messageWindow.Show("Fill NewWizard Component Path and Name in ObjectTable !!");
        }
    };
    LogSearchWindowComponent.prototype.RunNewGenaricEntity = function () {
        var _this = this;
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(this.ObjectTableName).then(function (response) {
            var args = new AddEntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = _this.ObjectTableName;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(_this.ObjectTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            logWindow.Show(componentPath);
        });
    };
    LogSearchWindowComponent.prototype.OnNewEntityWindowClosed = function ($event) {
        var _this = this;
        console.log($event);
        if ($event && $event != "event") {
            this._entityListService.getSingle($event, this.ObjectTableName).then(function (res) {
                res.subscribe(function (myResponse) {
                    if (myResponse != null) {
                        // Refresh
                        _this.SearchFieldchangeevent.emit("");
                        if (_this.LookUpTable.CacheOnClient) {
                            CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                        }
                        //var list = myResponse;
                        //if (myResponse instanceof ServiceResponse) {
                        //    list = myResponse.Result;
                        //}
                        //this.SearchTextNgModel = list[this.DisplayMemberPath];
                        //this.OldSearchInput = this.SearchTextNgModel;
                        //this.DisplayValue = list[this.DisplayMemberPath];
                        //this.SelectedItem = list;
                        //this.SelectedItemObject = this.SelectedItem;
                        //this.SelectedItemChanged.emit(this.SelectedItem);
                        //this.selectedValue = this.SelectedItem[this.SelectedValuePath];
                        //this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
                        //this.ValidateField();
                    }
                });
            });
        }
    };
    LogSearchWindowComponent.prototype.CloseButtonClicked = function () {
        //this.CurrentSession.CloseCurrentWindow();
        this.CurrentSession.CloseCurrentWindowEmit(null);
    };
    LogSearchWindowComponent.prototype.TextChanged = function (searchtext) {
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
    LogSearchWindowComponent.prototype.onRowSelected = function ($event) {
        if (this.preventSelect == false) {
            if ($event != null) {
                var entityList = $event.rowData;
                var selectedEntityId = $event.rowData[this.ObjectTable.KeyPropertyPath];
                //console.log("row clicked : ", entityList, selectedEntityId);
                this.Args.SelectedItem = entityList;
                var args = selectedEntityId + ',' + entityList.Tenant;
                // Close windoew with Args
                this.CurrentSession.CloseCurrentWindowEmit(args);
            }
        }
        else {
            this.preventSelect = false;
        }
    };
    LogSearchWindowComponent.prototype.AddPartnerOfType = function (partnerType) {
        var objectTableName = this.GetObjectTableNameForDependency(partnerType.Id, this.ObjectTableName);
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
        if (objectTable.IsNewWizard) {
            this.RunNewEntityWizard(objectTable.NewWizardComponentPath, objectTable.Name);
        }
        else {
            this.RunNewGenaricEntity();
        }
    };
    //#endregion
    LogSearchWindowComponent.prototype.AddUSWarehouseClicked = function () {
        var _this = this;
        var windowTitle = "Add " + this.ObjectTableName;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var args = new TenantImportComponent_1.ImportEntityArgs();
        args.ObjectTableId = this.ObjectTable.Id;
        args.ObjectTableName = this.ObjectTableName;
        logWindow.WindowArgs = args;
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.TextChanged(_this.searchFields);
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogSearchWindowComponent.prototype, "SearchFieldchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogSearchWindowComponent.prototype, "ItemSelected", void 0);
    LogSearchWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogSearchWindow',
            templateUrl: './LogSearchWindowComponent.html',
            providers: [http_1.Http, ServiceArgs_1.ServiceArgs, EntityListService_1.EntityListService, EntityPMService_1.EntityPMService],
            inputs: ['DependencyFilter1Value', 'DependencyFilter2Value', 'DependencyFilter3Value',
                "DependencyFilter1IsList", "DependencyFilter2IsList", "DependencyFilter3IsList",
                "DependencyFilter1IsListExact", "DependencyFilter2IsListExact", "DependencyFilter3IsListExact",
                "IsTenantZeroSearch", "ShowInActive"],
        }),
        __metadata("design:paramtypes", [])
    ], LogSearchWindowComponent);
    return LogSearchWindowComponent;
}(BaseComponent_1.BaseComponent));
exports.LogSearchWindowComponent = LogSearchWindowComponent;
var CustomEntityArgs = /** @class */ (function () {
    function CustomEntityArgs() {
        this.ObjectTableName = null;
        this.ObjectTableId = null;
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
//# sourceMappingURL=LogSearchWindowComponent.js.map