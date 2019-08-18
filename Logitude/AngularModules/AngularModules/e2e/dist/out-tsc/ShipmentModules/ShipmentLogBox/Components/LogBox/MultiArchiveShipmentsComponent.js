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
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var PortExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/PortExtendedPMService");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var EntityStatusExtendedListService_1 = require("../../../../Infrastructure/Services/ExtendedLists/EntityStatusExtendedListService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var MultiArchiveShipmentsComponent = /** @class */ (function (_super) {
    __extends(MultiArchiveShipmentsComponent, _super);
    function MultiArchiveShipmentsComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.AgentShortName = "";
        _this.IsPrivateLabel = false;
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        //EntityPm: ShipmentPM = new ShipmentPM();
        _this.DataContext = _this;
        _this.SelectedRecords = [];
        _this.SelectedItemsCountText = null; // + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
        _this.SelectedRecordsCount = 0;
        _this.AllRecordsCount = 0;
        _this.TransportationTypes = [new TransportationTypes("Ocean Haifa", "O", "HFA", "IL"), new TransportationTypes("Ocean Ashdod", "O", "ASH", "IL")];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.ShipmentSelectedEvent = new core_1.EventEmitter();
        _this.OnImporterShipmentsFilterChanged = new core_1.EventEmitter();
        _this.CustomColumnsReady = new core_1.EventEmitter();
        _this.columns = null;
        _this.items = [];
        _this.SelectedFilter = "My Shipments";
        _this.RecentImg = "./Images/LogBox/Recent.png";
        _this.SearchFilter = "";
        _this.mySelectedTransportFilter = "All";
        _this.mySelectedArchiveFilter = "O";
        _this.HasSharedDocs = true;
        _this.SearchFieldchangeevent = new core_1.EventEmitter();
        _this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingCol: "StatusDate",
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                //if (!this.SelectedFilter) {
                //    this.SelectedFilter = tempo.rowData;
                //    this.ShipmentSelectedEvent.emit(this.SelectedRow);
                //}
                return tempo;
            },
        };
        _this.GetSourceEntityEvent = null;
        _this.BusyIndicatorText = null;
        _this.ShowBusyIndicator = false;
        _this.SelectedValue = "All";
        _this.myFilters = [new FilterValue("CLD", "Days Past Clearance Date"), new FilterValue("CRD", "Days Past Create Date")];
        _this.myselectedFilter = _this.myFilters[0]; // = ["Days Past Clearance Date", "Days Past Create Date"];
        _this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        //if (SessionLocator.PrivateLableSettings) {
        //this.ValidationErrorsList = [];
        //this.AgentShortName = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
        //this.IsPrivateLabel = true;
        _this._PortExtendedPMService = new PortExtendedPMService_1.PortExtendedPMService();
        _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        _this._EntityStatusExtendedListService = new EntityStatusExtendedListService_1.EntityStatusExtendedListService();
        return _this;
        //}
    }
    MultiArchiveShipmentsComponent.prototype.ngOnInit = function () {
        this.LoadImporterShipments();
    };
    MultiArchiveShipmentsComponent.prototype.ngAfterViewInit = function () {
    };
    Object.defineProperty(MultiArchiveShipmentsComponent.prototype, "SelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (newValue) {
            if (this.mySelectedTransportFilter != newValue) {
                this.mySelectedTransportFilter = newValue;
                //if (this.filterAgrs == null) {
                //    this.filterAgrs = new ApiQueryFilters();
                //}
                //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'TransportModeId').length > 0) {
                //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'TransportModeId');
                //}
                //this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
                //this.LoadQueriesCounts();
                //this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: this.SelectedTransportFilter == "All" ? true : false });
                this.LoadImporterShipments();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiArchiveShipmentsComponent.prototype, "SelectedArchiveFilter", {
        get: function () { return this.mySelectedArchiveFilter; },
        set: function (newValue) {
            if (this.mySelectedArchiveFilter != newValue) {
                this.mySelectedArchiveFilter = newValue;
                //if (this.filterAgrs == null) {
                //    this.filterAgrs = new ApiQueryFilters();
                //}
                //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsOperationalClosed').length > 0) {
                //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsOperationalClosed');
                //}
                //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedTransportFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
                //this.LoadQueriesCounts();
                //this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: this.SelectedTransportFilter == "All" ? true : false });
                this.LoadImporterShipments();
            }
        },
        enumerable: true,
        configurable: true
    });
    MultiArchiveShipmentsComponent.prototype.SetWindowArgs = function (args) {
        this.SourceEntity = {}; //args.SourceEntity;
        this.HasSharedDocs = args.HasSharedDocs;
        if (this.SourceEntity) {
            if (this.SourceEntity.TransportModeId == "O") {
                this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
            }
            else if (this.SourceEntity.TransportModeId == "I") {
                this.TransportationTypes = [new TransportationTypes("Nitzana", "I", "NZN", "IL"), new TransportationTypes("Arava", "I", "ARV", "IL"), new TransportationTypes("Alenbi", "I", "ALN", "IL"), new TransportationTypes("Jordan", "I", "JOR", "IL")];
            }
            else if (this.SourceEntity.TransportModeId == "A") {
                this.TransportationTypes = [new TransportationTypes("Tel-Aviv", "A", "TLV", "IL")];
            }
        }
        //this.CurrentSession.SessionEvent.subscribe(($event: any) => {
        //    if ($event.Name == "GetSourceEntity") {
        //        this.GetSourceEntity($event.EntityId);
        //    }
        //}); 
    };
    MultiArchiveShipmentsComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "MyCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            IsCheckBox: true
        });
        this.columns.push({
            FieldName: "ForwarderShipmentNumber",
            DataTypeCode: 'String',
            Display: 'Shipment #',
            Styles: { width: '220px' },
            HtmlListComponentName: 'ReferenceNumberCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/ReferenceNumberCellDisplayListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ShipperName',
            DataTypeCode: 'String',
            Display: 'Supplier',
            Styles: { width: '175px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StatusName',
            DataTypeCode: 'String',
            Display: 'Status',
            Styles: { width: '120px' },
            HtmlListComponentName: 'StatusCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/StatusCellDisplayListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StatusDate',
            DataTypeCode: 'String',
            Display: 'Status Date',
            Styles: { width: '125px' },
            HtmlListComponentName: 'DateCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/DateCellDisplayListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CustomerReference1',
            DataTypeCode: 'String',
            Display: 'Order #',
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.CustomColumnsReady.emit(this.columns);
    };
    MultiArchiveShipmentsComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        //if (filters == null) {
        filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.SortBy = "StatusDate";
        filters.SortDirection = "Descending";
        //}
        if (!Tools_1.AppTool.IsNullOrEmpty(searchfields)) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "SearchFields"; });
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
            //this.searchFields = searchfields;
        }
        else {
            filters.Filter1Value = "";
            filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "SearchFields"; });
            //this.searchFields = undefined;
        }
        if (this.filterAgrs != null) {
            if (this.filterAgrs.AdditionalFilters.length > 0) {
                this.filterAgrs.AdditionalFilters.forEach(function (filter, key) {
                    if (filters.AdditionalFilters.filter(function (a) { return a.FieldName == filter.FieldName; }).length > 0) {
                        filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != filter.FieldName; });
                    }
                    filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType);
                });
            }
            else {
                var x = filters.AdditionalFilters.filter(function (a) { return a.FieldName == "ForwarderShipmentsFilter"; });
                if (x.length == 0) {
                    filters.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
                }
            }
        }
        else {
            var x = filters.AdditionalFilters.filter(function (a) { return a.FieldName == "ForwarderShipmentsFilter"; });
            if (x.length == 0) {
                filters.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
            }
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        if (sortingCol) {
            filters.SortBy = sortingCol;
        }
        if (sortingDir) {
            filters.SortDirection = sortingDir;
        }
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        return this._entityListService.getByFilters("Shipment", filters);
    };
    MultiArchiveShipmentsComponent.prototype.onRowSelected = function (CurrentRow) {
        this.SelectedRow = CurrentRow.rowData;
    };
    MultiArchiveShipmentsComponent.prototype.LoadImporterShipments = function (FilterByOrderNumber) {
        if (FilterByOrderNumber === void 0) { FilterByOrderNumber = false; }
        this.IsAllRecordSelected = false;
        this.BuildColumns();
        var last7Date = Tools_1.DateTool.AddDays((new Date()), -7);
        last7Date.setUTCHours(0, 0, 0, 0);
        var last30Date = Tools_1.DateTool.AddDays((new Date()), -30);
        last30Date.setUTCHours(0, 0, 0, 0);
        var last45Date = Tools_1.DateTool.AddDays((new Date()), -45);
        last45Date.setUTCHours(0, 0, 0, 0);
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        //if (FilterByOrderNumber == true) {
        //    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'CustomerReference1').length > 0) {
        //        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'CustomerReference1');
        //    }
        //    this.filterAgrs.addAdditionalFilter("CustomerReference1", this.CustomerReference1, null, null, "Equals", false, false, false, "String");
        //}
        if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'IsCancelled'; }).length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'IsCancelled'; });
        }
        this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        if (this.SelectedTransportFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
        }
        else {
            if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'TransportModeId'; }).length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'TransportModeId'; });
            }
        }
        if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'IsOperationalClosed'; }).length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'IsOperationalClosed'; });
        }
        this.filterAgrs.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, true, false, "string");
        if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'ForwarderShipmentsFilter'; }).length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'ForwarderShipmentsFilter'; });
        }
        if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'NotForwarderShipmentsFilter'; }).length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'NotForwarderShipmentsFilter'; });
        }
        this.filterAgrs.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
        var FilterDate = new Date();
        if (this.SelectedValue == "7d") {
            FilterDate = last7Date;
        }
        else if (this.SelectedValue == "30d") {
            FilterDate = last30Date;
        }
        else if (this.SelectedValue == "45d") {
            FilterDate = last45Date;
        }
        else {
            FilterDate = null;
        }
        if (this.mySelectedFilter.Code == "CLD" && FilterDate != null) {
            if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'CustomsClearanceDate'; }).length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'CustomsClearanceDate'; });
            }
            this.filterAgrs.addAdditionalFilter("CustomsClearanceDate", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date");
        }
        else if (FilterDate == null) {
            if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'CustomsClearanceDate'; }).length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'CustomsClearanceDate'; });
            }
            this.filterAgrs.addAdditionalFilter("CustomsClearanceDate", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date", true);
        }
        if (this.mySelectedFilter.Code == "CRD" && FilterDate != null) {
            if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'CreateDateTime'; }).length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'CreateDateTime'; });
            }
            this.filterAgrs.addAdditionalFilter("CreateDateTime", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date");
        }
        else if (FilterDate == null) {
            if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'CreateDateTime'; }).length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'CreateDateTime'; });
            }
            this.filterAgrs.addAdditionalFilter("CreateDateTime", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date", true);
        }
        this.filterAgrs.SortBy = "StatusDate";
        this.filterAgrs.SortDirection = "Descending";
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    MultiArchiveShipmentsComponent.prototype.GridAfterViewInitCompleted = function ($event) {
        this.LoadImporterShipments();
    };
    MultiArchiveShipmentsComponent.prototype.RefreshBtnClick = function () {
        this.LoadImporterShipments();
    };
    MultiArchiveShipmentsComponent.prototype.onSearchTextChangeEvent = function (event) {
        this.searchFields = event;
        this.SearchFieldchangeevent.emit(this.searchFields);
        this.SelectedRow = null;
    };
    MultiArchiveShipmentsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    MultiArchiveShipmentsComponent.prototype.GetSourceEntity = function (Id) {
        this.CurrentSession.FireEvent({ Name: 'SourceEntity', Entity: this.SourceEntity, EntityId: Id });
    };
    Object.defineProperty(MultiArchiveShipmentsComponent.prototype, "IsAllRecordSelected", {
        get: function () { return this.isAllRecordSelected; },
        set: function (value) {
            var _this = this;
            this.isAllRecordSelected = value;
            if (value == true) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("loading ..");
                this._ShipmentPMService.GetTop100ShipmentIds(this.filterAgrs).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        _this.SelectedRecordsCount = myResult.Result.length;
                        _this.SelectedRecords = myResult.Result;
                        _this.SelectedItemsCountText = _this.SelectedRecordsCount + " of " + _this.AllRecordsCount + " shipments selected";
                    }
                    else {
                        _this.ValidationErrorsList = myResult.ErrorsArray;
                    }
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                });
            }
            else {
                this.SelectedItemsCountText = "0 of " + this.AllRecordsCount + " shipments selected";
                this.SelectedRecords = [];
                this.SelectedRecordsCount = 0;
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    MultiArchiveShipmentsComponent.prototype.onCheckBoxChecked = function (event) {
        var temp = this.SelectedRecords.filter(function (a) { return a == event.rowData.Id; });
        if (event.IsChecked) {
            if (temp.length == 0) {
                this.SelectedRecords.push(event.rowData.Id);
                this.SelectedRecordsCount++;
            }
        }
        else {
            if (temp.length > 0) {
                this.SelectedRecords = this.SelectedRecords.filter(function (a) { return a != event.rowData.Id; });
                this.SelectedRecordsCount--;
            }
        }
        this.SelectedItemsCountText = this.SelectedRecordsCount + " of " + this.AllRecordsCount + " shipments selected";
    };
    MultiArchiveShipmentsComponent.prototype.onCountReady = function (count) {
        this.AllRecordsCount = count;
        this.SelectedItemsCountText = "0 of " + count + " shipments selected"; //this.SelectedItemsCountText + " * " + this.DataSource.rowCount;
    };
    MultiArchiveShipmentsComponent.prototype.SaveData = function () {
        var _this = this;
        if (this.SelectedRecords.length > 0 || this.IsAllRecordSelected == true) {
            this.StartBusyIndicator("Archiving ...");
            //if (this.IsAllRecordSelected == true) {
            //    this._ShipmentPMService.ArchiveAllShipments(this.filterAgrs).subscribe(myResult => {
            //        if (!myResult.HasError) {
            //            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            //            this.CurrentSession.SessionEvent.emit({ Name: "CustomReloadShipments" });
            //            this.CurrentSession.CurrentWindow.Close("");
            //        }
            //        else {
            //            this.ValidationErrorsList = myResult.ErrorsArray;
            //        }
            //    });
            //}
            //else {
            this._ShipmentPMService.ArchiveShipments(this.SelectedRecords).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.StopBusyIndicator();
                    _this.LoadImporterShipments();
                    _this.CurrentSession.SessionEvent.emit({ Name: "CustomReloadShipments" });
                    //this.CurrentSession.CloseCurrentWindow();//.CurrentWindow.Close("");
                }
                else {
                    _this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
            //}
        }
    };
    MultiArchiveShipmentsComponent.prototype.StartBusyIndicator = function (myText) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    };
    MultiArchiveShipmentsComponent.prototype.StopBusyIndicator = function () {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    };
    MultiArchiveShipmentsComponent.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.LoadImporterShipments();
        }
    };
    MultiArchiveShipmentsComponent.prototype.itemMouseOver = function (itemValue) {
    };
    MultiArchiveShipmentsComponent.prototype.itemMouseLeave = function (itemValue) {
    };
    Object.defineProperty(MultiArchiveShipmentsComponent.prototype, "MyFilters", {
        get: function () { return this.myFilters; },
        set: function (newValue) {
            this.myFilters = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiArchiveShipmentsComponent.prototype, "mySelectedFilter", {
        get: function () { return this.myselectedFilter; },
        set: function (newValue) {
            this.myselectedFilter = newValue;
        },
        enumerable: true,
        configurable: true
    });
    MultiArchiveShipmentsComponent.prototype.mySelectedFilterValueChanged = function (event) {
        this.mySelectedFilter = event;
        this.LoadImporterShipments();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], MultiArchiveShipmentsComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], MultiArchiveShipmentsComponent.prototype, "ShipmentSelectedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], MultiArchiveShipmentsComponent.prototype, "OnImporterShipmentsFilterChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], MultiArchiveShipmentsComponent.prototype, "CustomColumnsReady", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], MultiArchiveShipmentsComponent.prototype, "SearchFieldchangeevent", void 0);
    MultiArchiveShipmentsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MultiArchiveShipmentsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], MultiArchiveShipmentsComponent);
    return MultiArchiveShipmentsComponent;
}(BaseComponent_1.BaseComponent));
exports.MultiArchiveShipmentsComponent = MultiArchiveShipmentsComponent;
var TransportationTypes = /** @class */ (function () {
    function TransportationTypes(name, transporationType, toPortCode, CountryCode) {
        this.name = name;
        this.transporationType = transporationType;
        this.toPortCode = toPortCode;
        this.Name = name;
        this.TransporationType = transporationType;
        this.ToPortCode = toPortCode;
        this.CountryCode = CountryCode;
    }
    return TransportationTypes;
}());
exports.TransportationTypes = TransportationTypes;
var FilterValue = /** @class */ (function () {
    function FilterValue(code, name) {
        this.Code = code;
        this.Name = name;
    }
    Object.defineProperty(FilterValue.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterValue.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    return FilterValue;
}());
exports.FilterValue = FilterValue;
//# sourceMappingURL=MultiArchiveShipmentsComponent.js.map