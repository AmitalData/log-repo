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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var TaxReportLineStatusListService_1 = require("../../../Services/StandardLists/TaxReportLineStatusListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TaxReportExtendedPMService_1 = require("../../../Services/ExtendedPMs/TaxReportExtendedPMService");
var TaxReportDetailsTabComponent = /** @class */ (function (_super) {
    __extends(TaxReportDetailsTabComponent, _super);
    function TaxReportDetailsTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.EntityPM = null;
        _this.ObjectTableName = "TaxReport";
        _this.DataContext = _this;
        _this.isRTL = false;
        _this.showLocals = false;
        _this._entityListService = new EntityListService_1.EntityListService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._TaxReportExtendedPMService = new TaxReportExtendedPMService_1.TaxReportExtendedPMService();
        _this._TaxReportLineStatusListService = new TaxReportLineStatusListService_1.TaxReportLineStatusListService();
        _this.TaxReportColumnsReady = new core_1.EventEmitter();
        _this.isReady = false;
        _this.ShowErrorMsg = false;
        _this.errorsCount = 0;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Events
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        //#endregion
        //#region Filtering Methods
        _this.TaxableTransactionsCount = 0;
        _this.ExemptTransactionsCount = 0;
        _this.AllTransactionsCount = 0;
        _this.InputsEquipmentsCount = 0;
        _this.InputsOtherCount = 0;
        _this.AllCount = 0;
        _this.FilterSelectedValue = 'All';
        _this.searchText = "";
        _this.StatusItems = [];
        _this.SelectedStatusItems = [];
        //#endregion
        //#region Data
        _this.columns = null;
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.showLocals = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        _this.EntityPM = entityArgs.EntityPM;
        _this.SetUIProperty();
        _this.Listen();
        return _this;
    }
    TaxReportDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.ReloadScreen();
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    };
    TaxReportDetailsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("TaxReport").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("TaxReportLine").subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("TaxReportLineTransmitStatus").subscribe(function (response) {
                    _this._entityResourceService.getEntityResourceByTableName("Journal").subscribe(function (response) {
                        _this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(function (response) {
                            _this.isReady = true;
                            // this.GetStatuses();
                            // this.FillGrids();
                            // this.BuildColumns();
                            _this.ReloadScreen();
                            // this.ReloadData();
                        });
                    });
                });
            });
        });
    };
    TaxReportDetailsTabComponent.prototype.ReloadScreen = function () {
        this.BuildColumns();
        this.GetStatuses();
        this.CD.detectChanges();
        // this.FillGrids();
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        this.GetReportCounter();
    };
    TaxReportDetailsTabComponent.prototype.SetUIProperty = function () {
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OutputTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxableOutputAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LastUpdateDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ExemptTaxableOutput", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxReportMonth", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxableOutputAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EquipmentInputsTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OtherInputsTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AmountForPayRefund", this.ObjectTableName, false);
    };
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "VatNumber", {
        //#region Properties
        //VatNumber
        //TaxableOutputAmount
        //LastUpdateDate
        //ExemptTaxableOutput
        //TaxReportMonth
        //TaxableOutputAmount
        //EquipmentInputsTaxAmount
        //OtherInputsTaxAmount
        //AmountForPayRefund
        get: function () { return this.EntityPM.VatNumber; },
        set: function (value) {
            if (this.EntityPM.VatNumber != value) {
                this.EntityPM.VatNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "OutputTaxAmount", {
        get: function () { return this.EntityPM.OutputTaxAmount; },
        set: function (value) {
            if (this.EntityPM.OutputTaxAmount != value) {
                this.EntityPM.OutputTaxAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "TaxableOutputAmount", {
        get: function () { return this.EntityPM.TaxableOutputAmount; },
        set: function (value) {
            if (this.EntityPM.TaxableOutputAmount != value) {
                this.EntityPM.TaxableOutputAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "LastUpdateDate", {
        get: function () { return this.EntityPM.LastUpdateDate; },
        set: function (value) {
            if (this.EntityPM.LastUpdateDate != value) {
                this.EntityPM.LastUpdateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "ExemptTaxableOutput", {
        get: function () { return this.EntityPM.ExemptTaxableOutput; },
        set: function (value) {
            if (this.EntityPM.ExemptTaxableOutput != value) {
                this.EntityPM.ExemptTaxableOutput = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "TaxReportMonth", {
        get: function () { return this.EntityPM.TaxReportMonth; },
        set: function (value) {
            if (this.EntityPM.TaxReportMonth != value) {
                this.EntityPM.TaxReportMonth = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "EquipmentInputsTaxAmount", {
        get: function () { return this.EntityPM.EquipmentInputsTaxAmount; },
        set: function (value) {
            if (this.EntityPM.EquipmentInputsTaxAmount != value) {
                this.EntityPM.EquipmentInputsTaxAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "OtherInputsTaxAmount", {
        get: function () { return this.EntityPM.OtherInputsTaxAmount; },
        set: function (value) {
            if (this.EntityPM.OtherInputsTaxAmount != value) {
                this.EntityPM.OtherInputsTaxAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxReportDetailsTabComponent.prototype, "AmountForPayRefund", {
        get: function () { return this.EntityPM.AmountForPayRefund; },
        set: function (value) {
            if (this.EntityPM.AmountForPayRefund != value) {
                this.EntityPM.AmountForPayRefund = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TaxReportDetailsTabComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.FilterLines();
        }
    };
    TaxReportDetailsTabComponent.prototype.FilterLines = function () {
        // var filteredLines = [];
        // filteredLines = this.OriginalReportLines.Collection;
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        //search
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchText))
            filters.addAdditionalFilter("SearchFields", this.searchText, null, null, "Contains", false, false, false, "string");
        // filteredLines = filteredLines.filter(d => d.SearchFields.toLowerCase().includes(this.searchText.toLowerCase()));
        //update filters count
        //this.TaxableTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount > 0).length;
        //this.ExemptTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount == 0).length;
        //this.AllTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O").length;
        //this.InputsEquipmentsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == true).length;
        //this.InputsOtherCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == false).length;
        //this.AllCount = filteredLines.length;
        //toggle filters
        switch (this.FilterSelectedValue) {
            case "TaxableTransactions": {
                filters.addAdditionalFilter("OutputOrInput", "O", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("VatAmount", 0, null, null, "NotEqual", false, false, false, "string");
                break;
            }
            case "ExemptTransactions": {
                filters.addAdditionalFilter("OutputOrInput", "O", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("VatAmount", 0, null, null, "Equals", false, false, false, "string");
                break;
            }
            case "AllTransactions": {
                filters.addAdditionalFilter("OutputOrInput", "O", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "InputsEquipments": {
                filters.addAdditionalFilter("OutputOrInput", "I", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("IsEquipment", true, null, null, "Equals", false, false, false, "string");
                break;
            }
            case "InputsOther": {
                filters.addAdditionalFilter("OutputOrInput", "I", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("IsEquipment", false, null, null, "Equals", false, false, false, "string");
                break;
            }
            case "All": {
                break;
            }
        }
        //filter statuses
        if (this.SelectedStatusItems.length > 0) {
            var statusesListString = "";
            this.SelectedStatusItems.forEach(function (item) { statusesListString += item + ","; });
            statusesListString = statusesListString.slice(0, -1); // trim last comma
            filters.addAdditionalFilter("StatusCode", statusesListString, null, null, "InList", false, false, false, "string");
            //filteredLines = filteredLines.filter(d => this.SelectedStatusItems.includes(d.StatusCode));
        }
        this.ListFilters = filters;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
    };
    TaxReportDetailsTabComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        this.timerToken = setTimeout(function () {
            _this.searchText = searchtext;
            _this.FilterLines();
        }, 500);
        if (!Tools_1.AppTool.IsNullOrEmpty(searchtext)) {
            this.timerToken = setTimeout(function () {
                _this.searchFieldFilter = new ApiQueryFilters_1.FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                _this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }, 700);
        }
        else {
            this.searchFieldFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        }
    };
    //#endregion
    //#region Data
    TaxReportDetailsTabComponent.prototype.FillGrids = function () {
        // var lines = [];
        // this.OriginalReportLines = new ObservableCollection([]);
        // if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
        //     for (let item of this.ReportLines.Collection.sort((a, b) => { return (a.Line === b.Line) ? 0 : (a.Line < b.Line) ? -1 : 1 })) {
        //         lines.push(item));
        //     }
        // }
        // // this.ReportLines.InsertCollection(lines);
        // this.OriginalReportLines.InsertCollection(lines);
        //calculate sums
        //this.TaxableTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount > 0).length;
        //this.ExemptTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount == 0).length;
        //this.AllTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O").length;
        //this.InputsEquipmentsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == true).length;
        //this.InputsOtherCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == false).length;
        //this.AllCount = lines.length;
        var lines = this.ReportLines.Collection;
        this.errorsCount = lines.filter(function (d) { return d.TaxReportLinePM.StatusCode != "6" && d.TaxReportLinePM.TransmitStatusCode == "1"; }).length;
        this.ShowErrorMsg = this.errorsCount > 0;
    };
    TaxReportDetailsTabComponent.prototype.GetStatuses = function () {
        var _this = this;
        this._TaxReportLineStatusListService.getAll().subscribe(function (myResult) {
            _this.StatusItems = myResult.Result;
        });
    };
    TaxReportDetailsTabComponent.prototype.PushStatus = function (status) {
        this.SelectedStatusItems.push(status.Code);
        this.FilterLines();
    };
    TaxReportDetailsTabComponent.prototype.PopStatus = function (status) {
        var itemIndex = this.SelectedStatusItems.indexOf(status.Code);
        if (itemIndex > -1)
            this.SelectedStatusItems.splice(itemIndex, 1);
        this.FilterLines();
    };
    TaxReportDetailsTabComponent.prototype.GetLinesWithErrorsCount = function () {
        var _this = this;
        this._TaxReportExtendedPMService.getErrorsCount(this.EntityPM.Id).subscribe(function (myResult) {
            var __errorsCount = myResult.Result;
            _this.ShowErrorMsg = __errorsCount >= 1;
            _this.errorsCount = __errorsCount;
        });
    };
    TaxReportDetailsTabComponent.prototype.ReloadData = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        this.GetReportCounter();
    };
    TaxReportDetailsTabComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'TransmitStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Included"),
            Styles: { width: '70px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Line',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.Line"),
            Styles: { width: '50px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'LineTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.LineTypeCode"),
            Styles: { width: '50px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'VatNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.VatNumber"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Reference',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.Reference"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ReferecneGroup',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.ReferecneGroup"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ReferenceDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.ReferenceDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'VatableInvoiceAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.VatableInvoiceAmount"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'VatAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.VatAmount"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal ? 'StatusEnglishName' : 'StatusLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.StatusEnglishName"),
            Styles: { width: '300px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReportLine.F.JournalNumber"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Buttons;' + this.EntityPM.StatusCode,
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            ServerSideSortable: true,
            IsCustomTemplate: true,
        });
        this.TaxReportColumnsReady.emit(this.columns);
        //this.CustomColumnsReady.emit(this.columns);
    };
    TaxReportDetailsTabComponent.prototype.GetRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        //#region Filters
        if (this.ListFilters)
            var filters = this.ListFilters;
        else
            var filters = new ApiQueryFilters_1.ApiQueryFilters;
        //add report id
        filters.addAdditionalFilter('TaxReportId', this.EntityPM.Id, null, null, 'Equals', false, false, false, 'string');
        //if (this.dateFilter) {
        //     filters.AdditionalFilters.push(this.dateFilter);
        // } else {
        //     return;
        // }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetCount = true;
        filters.SortBy = "Line";
        filters.SortDirection = "Ascending";
        // filters.addAdditionalFilter("BankAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        //#endregion
        return this._entityListService.getExtendedByFilters("TaxReportLine", filters);
    };
    TaxReportDetailsTabComponent.prototype.GetReportCounter = function () {
        var _this = this;
        this._TaxReportExtendedPMService.GetReportLinesCounter(this.EntityPM.Id).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                var result = myResult.Result;
                _this.reportCounters = result.Result;
                console.log("GetReportLinesCounter", mm);
            }
            else {
            }
        });
        this.GetLinesWithErrorsCount();
    };
    //#endregion
    TaxReportDetailsTabComponent.prototype.RefreshButtonClicked = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        //this.ListFilters = new ApiQueryFilters();
        //this.FilterSelectedValue = 'All';
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        this.GetReportCounter();
    };
    TaxReportDetailsTabComponent.prototype.EditLine = function (entity) {
        var _this = this;
        if (entity) {
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.EditLine") + " " + entity.Line;
            var windowArgs = {};
            windowArgs.TaxReportPM = this.EntityPM;
            windowArgs.TaxReportLinePM = entity;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 450;
            logWindow.Height = 350;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function (event) {
                if (event == "ok")
                    _this.ReloadScreen();
            });
            logWindow.Show('./Accounting/Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent');
        }
    };
    TaxReportDetailsTabComponent.prototype.GetErrorMsg = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.TaxReportErrorMsg");
        return msg.replace("#Number", this.errorsCount.toString());
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TaxReportDetailsTabComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TaxReportDetailsTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    TaxReportDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TaxReportDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], TaxReportDetailsTabComponent);
    return TaxReportDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TaxReportDetailsTabComponent = TaxReportDetailsTabComponent;
var TaxReportLinesCounter = /** @class */ (function () {
    function TaxReportLinesCounter() {
    }
    return TaxReportLinesCounter;
}());
exports.TaxReportLinesCounter = TaxReportLinesCounter;
//# sourceMappingURL=TaxReportDetailsTabComponent.js.map