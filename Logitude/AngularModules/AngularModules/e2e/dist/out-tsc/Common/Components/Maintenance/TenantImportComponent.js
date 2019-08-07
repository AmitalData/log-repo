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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CachedDataManager_1 = require("../../../Infrastructure/Utilities/CachedDataManager");
var TenantImportComponent = /** @class */ (function (_super) {
    __extends(TenantImportComponent, _super);
    function TenantImportComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.SearchText = "Search";
        _this.DataContext = _this;
        _this.columns = [];
        _this.ObjectFields = [];
        _this.AddButtonVisibility = false;
        _this.PortsList = [];
        _this.AirLinesList = [];
        _this.ShippingLinesList = [];
        _this.items = [];
        _this.IsVisible = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.SearchFieldchangeevent = new core_1.EventEmitter();
        _this.IsNewEntityButtonVisible = false;
        _this.NewEntityButtonLabel = "New";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsDataReady = false;
        _this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        return _this;
    }
    TenantImportComponent.prototype.SetWindowArgs = function (args) {
        this.ObjectTableName = args.ObjectTableName;
        this.ObjectTableId = args.ObjectTableId;
        if (this.ObjectTableName == "ShippingLine") {
            this.AddButtonVisibility = true;
        }
        else {
            this.AddButtonVisibility = false;
        }
        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline") {
            this.IsNewEntityButtonVisible = true;
        }
        this.NewEntityButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName));
    };
    TenantImportComponent.prototype.ngOnInit = function () {
        var _this = this;
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline") {
            ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Carrier"; })[0];
        }
        this._entityResourceService.getEntityResourceByTableName(ObjectTable.Name, 0).subscribe(function (response) {
            _this.IsVisible = true;
            _this.BuildColumns();
        });
    };
    TenantImportComponent.prototype.ngAfterViewInit = function () {
    };
    TenantImportComponent.prototype.BuildColumns = function () {
        var objectTableId = this.ObjectTableId;
        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline") {
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Carrier"; })[0];
            if (ObjectTable != null) {
                objectTableId = ObjectTable.Id;
            }
        }
        this.ObjectFields = window.ObjectFields.filter(function (f) { return f.DisplayInSearchWindowList == true && f.ObjectTableId == objectTableId; });
        this.columns = [];
        this.columns.push({
            FieldName: this.ObjectTableName,
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '60px' },
            HtmlListComponentName: 'btnComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/btnComponent',
        });
        for (var i = 0; i < this.ObjectFields.length; i++) {
            this.columns.push({
                FieldName: this.ObjectFields[i].FieldName,
                IsCustomTemplate: true,
                DataTypeCode: this.ObjectFields[i].DataTypeCode,
                Display: TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectFields[i].ListTextCodeCode),
                Styles: { width: '100px' },
                HtmlListComponentName: this.ObjectFields[i].HtmlListComponentName,
                HtmlListComponentUrl: this.ObjectFields[i].HtmlListComponentUrl,
                ColumnHeaderTemplateName: this.ObjectFields[i].ColumnHeaderTemplateName,
            });
        }
        if (this.ObjectTableName == "Port") {
            if (this.columns.length > 0) {
                this.columns[0].Styles = { width: '65px' };
                if (this.columns.length > 1) {
                    this.columns[1].Styles = { width: '50px' };
                    if (this.columns.length > 2) {
                        this.columns[2].Styles = { width: '100px' };
                        if (this.columns.length > 3) {
                            this.columns[3].Styles = { width: '50px' };
                            if (this.columns.length > 4) {
                                this.columns[4].Styles = { width: '170px' };
                            }
                        }
                    }
                }
            }
        }
        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline" || this.ObjectTableName == "Warehouse") {
            if (this.columns.length > 0) {
                this.columns[0].Styles = { width: '65px' };
                if (this.columns.length > 1) {
                    this.columns[1].Styles = { width: '50px' };
                    if (this.columns.length > 2) {
                        this.columns[2].Styles = { width: '170px' };
                        if (this.columns.length > 3) {
                            this.columns[3].Styles = { width: '50px' };
                        }
                    }
                }
            }
        }
        if ((this.ObjectTableName == "Airline" || this.ObjectTableName == "ShippingLine") && this.TenantPM.Id != 0) {
            this.columns.push({
                FieldName: this.ObjectTableName,
                DataTypeCode: 'String',
                Display: '',
                IsCustomTemplate: true,
                Styles: { width: '80px' },
                HtmlListComponentName: 'btnComponent',
                HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/btnUpdateComponent',
            });
        }
    };
    TenantImportComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = this.TenantPM.Id;
        var x = filters.AdditionalFilters.filter(function (a) { return a.FieldName == "InActive"; });
        if (x.length == 0) {
            filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");
        }
        var rowsObjectTable = this.ObjectTableName;
        if (this.ObjectTableName == "Airline") {
            var x = filters.AdditionalFilters.filter(function (a) { return a.FieldName == "PartnerTypeId" && a.FieldValue == "AL"; });
            if (x.length == 0) {
                filters.addAdditionalFilter("PartnerTypeId", "AL", null, null, "Equals", false, true, false, "Text");
            }
            rowsObjectTable = "Carrier";
        }
        if (this.ObjectTableName == "ShippingLine") {
            var x = filters.AdditionalFilters.filter(function (a) { return a.FieldName == "PartnerTypeId" && a.FieldValue == "SL"; });
            if (x.length == 0) {
                filters.addAdditionalFilter("PartnerTypeId", "SL", null, null, "Equals", false, true, false, "Text");
            }
            rowsObjectTable = "Carrier";
        }
        if (this.ObjectTableName == "Warehouse") {
            var x = filters.AdditionalFilters.filter(function (a) { return a.FieldName == "PartnerTypeId" && a.FieldValue == "WH"; });
            if (x.length == 0) {
                filters.addAdditionalFilter("PartnerTypeId", "WH", null, null, "Equals", false, true, false, "Text");
            }
        }
        return this._entityListService.getCustomByFilters(rowsObjectTable, filters);
    };
    TenantImportComponent.prototype.LoadCachedLists = function () {
        if (this.ObjectTableName == "Port") {
            this.LoadPortListMethod();
        }
        if (this.ObjectTableName == "Airline") {
            this.LoadAirLineListMethod();
        }
        if (this.ObjectTableName == "ShippingLine") {
            this.LoadShippingLineListMethod();
        }
        if (this.ObjectTableName == "Warehouse") {
            this.LoadWarehouseListMethod();
        }
    };
    // Lists 
    TenantImportComponent.prototype.LoadPortListMethod = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetAll = true;
        this._entityListService.getByFilters("Port", filters).then(function (res) {
            res.subscribe(function (resp) {
                if (resp.Data) {
                    _this.PortsList = resp.Data;
                }
                else {
                    _this.PortsList = resp;
                }
                _this.IsDataReady = true;
            });
        });
    };
    TenantImportComponent.prototype.LoadAirLineListMethod = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("PartnerTypeId", "AL", null, null, "Equals", false, true, false, "Text");
        this._entityListService.getByFilters("Carrier", filters).then(function (res) {
            res.subscribe(function (resp) {
                if (resp.Data) {
                    _this.AirLinesList = resp.Data;
                }
                else {
                    _this.AirLinesList = resp;
                }
                _this.IsDataReady = true;
            });
        });
    };
    TenantImportComponent.prototype.LoadShippingLineListMethod = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("PartnerTypeId", "SL", null, null, "Equals", false, true, false, "Text");
        this._entityListService.getByFilters("Carrier", filters).then(function (res) {
            res.subscribe(function (resp) {
                if (resp.Data) {
                    _this.ShippingLinesList = resp.Data;
                }
                else {
                    _this.ShippingLinesList = resp;
                }
                _this.IsDataReady = true;
            });
        });
    };
    TenantImportComponent.prototype.LoadWarehouseListMethod = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("PartnerTypeId", "WH", null, null, "Equals", false, true, false, "Text");
        this._entityListService.getByFilters("Card", filters).then(function (res) {
            res.subscribe(function (resp) {
                if (resp.Data) {
                    _this.AirLinesList = resp.Data;
                }
                else {
                    _this.AirLinesList = resp;
                }
                _this.IsDataReady = true;
            });
        });
    };
    //Commands 
    TenantImportComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TenantImportComponent.prototype.TextChanged = function (searchtext) {
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    };
    TenantImportComponent.prototype.AddShippingLineButton = function () {
        this._entityResourceService.getEntityResourceByTableName("ShippingLine", 0).subscribe(function (response) {
            // Add Shipping Line
            var windowTitle = "New Shipping Line";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewShippingLineComponent');
        });
    };
    TenantImportComponent.prototype.AddNewEntityClicked = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            _this.CurrentSession.CloseCurrentWindow();
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = _this.NewEntityButtonLabel;
            logWindow.WindowClosed.subscribe(function (event) {
                _this.CurrentSession.FireEvent("NewAirlineShippingLineClosed");
                CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
            });
            if (_this.ObjectTableName == "ShippingLine") {
                logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewShippingLineComponent');
            }
            else {
                logWindow.Show('./CommonModules/CommonAirline/Components/NewEntity/NewAirlineComponent');
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TenantImportComponent.prototype, "SearchFieldchangeevent", void 0);
    TenantImportComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'TenantImportComponent',
            templateUrl: './TenantImportComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], TenantImportComponent);
    return TenantImportComponent;
}(BaseComponent_1.BaseComponent));
exports.TenantImportComponent = TenantImportComponent;
var ImportEntityArgs = /** @class */ (function () {
    function ImportEntityArgs() {
        this.ObjectTableName = null;
        this.ObjectTableId = null;
    }
    return ImportEntityArgs;
}());
exports.ImportEntityArgs = ImportEntityArgs;
//# sourceMappingURL=TenantImportComponent.js.map