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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ShipmentDomainService_1 = require("../../../Shipment/Services/ShipmentDomainService");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var PortExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/PortExtendedPMService");
var ShipmentPMService_1 = require("../../../Shipment/Services/StandardPMs/ShipmentPMService");
var EntityStatusExtendedListService_1 = require("../../../Infrastructure/Services/ExtendedLists/EntityStatusExtendedListService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var ForwarderChooseShipmentsComponent = /** @class */ (function (_super) {
    __extends(ForwarderChooseShipmentsComponent, _super);
    function ForwarderChooseShipmentsComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.AgentShortName = "";
        _this.IsPrivateLabel = false;
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        _this.DataContext = _this;
        _this.TransportationTypes = [new TransportationTypes("Ocean Haifa", "O", "HFA", "IL"), new TransportationTypes("Ocean Ashdod", "O", "ASH", "IL")];
        _this.EntityObjectTableName = "";
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
        _this.SearchFieldchangeevent = new core_1.EventEmitter();
        _this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingCol: "StatusDate",
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.GetSourceEntityEvent = null;
        _this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            _this.ValidationErrorsList = [];
            _this.AgentShortName = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            _this.IsPrivateLabel = true;
            _this._PortExtendedPMService = new PortExtendedPMService_1.PortExtendedPMService();
            _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
            _this._EntityStatusExtendedListService = new EntityStatusExtendedListService_1.EntityStatusExtendedListService();
        }
        return _this;
    }
    ForwarderChooseShipmentsComponent.prototype.ngOnInit = function () {
        this.LoadImporterShipments();
    };
    ForwarderChooseShipmentsComponent.prototype.ngAfterViewInit = function () {
    };
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "SelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (newValue) {
            if (this.mySelectedTransportFilter != newValue) {
                this.mySelectedTransportFilter = newValue;
                this.LoadImporterShipments();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "SelectedArchiveFilter", {
        get: function () { return this.mySelectedArchiveFilter; },
        set: function (newValue) {
            if (this.mySelectedArchiveFilter != newValue) {
                this.mySelectedArchiveFilter = newValue;
                this.LoadImporterShipments();
            }
        },
        enumerable: true,
        configurable: true
    });
    ForwarderChooseShipmentsComponent.prototype.SetWindowArgs = function (args) {
        this.EntityObjectTableName = args.EntityObjectTableName;
        this.EntityId = args.EntityId;
        this.SourceEntity = args.SourceEntity;
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
            if (this.SourceEntity.MainCarriageToPortCode == "ASH") {
                this.SelectedTransportationTypes = new TransportationTypes("Ocean Ashdod", "O", "ASH", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "HFA") {
                this.SelectedTransportationTypes = new TransportationTypes("Ocean Haifa", "O", "HFA", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "ETH") {
                this.SelectedTransportationTypes = new TransportationTypes("Ocean Eilat", "O", "ETH", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "NZN") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Nitzana", "I", "NZN", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "ARV") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Arava", "I", "ARV", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "ALN") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Alenbi", "I", "ALN", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "JOR") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Jordan", "I", "JOR", "IL");
            }
            else {
                this.SelectedTransportationTypes = new TransportationTypes("Air Tel-Aviv", "A", "TLV", "IL");
            }
        }
    };
    ForwarderChooseShipmentsComponent.prototype.BuildColumns = function () {
        this.columns = [];
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
        //this.columns.push({
        //    FieldName: this.SourceEntity.Id,
        //    DataTypeCode: 'String',
        //    Display: '',
        //    Styles: { width: '260px' },
        //    HtmlListComponentName: 'ActionButtonsListTemplate',
        //    HtmlListComponentUrl: './Shipment/Components/ListTemplates/ConnectButtonsListTemplate',
        //    IsCustomTemplate: true,
        //    EnableHoverVisibility: true,
        //});
        this.CustomColumnsReady.emit(this.columns);
    };
    ForwarderChooseShipmentsComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.SortBy = "StatusDate";
        filters.SortDirection = "Descending";
        if (!Tools_1.AppTool.IsNullOrEmpty(searchfields)) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "SearchFields"; });
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
        }
        else {
            filters.Filter1Value = "";
            filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "SearchFields"; });
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
    ForwarderChooseShipmentsComponent.prototype.onRowSelected = function (CurrentRow) {
        this.SelectedRow = CurrentRow.rowData;
        this.Close();
    };
    ForwarderChooseShipmentsComponent.prototype.LoadImporterShipments = function (FilterByOrderNumber) {
        if (FilterByOrderNumber === void 0) { FilterByOrderNumber = false; }
        this.BuildColumns();
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        if (FilterByOrderNumber == true) {
            if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'CustomerReference1'; }).length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'CustomerReference1'; });
            }
            this.filterAgrs.addAdditionalFilter("CustomerReference1", this.CustomerReference1, null, null, "Equals", false, false, false, "String");
        }
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
        if (this.SelectedArchiveFilter != "All") {
            this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedArchiveFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", this.SelectedArchiveFilter == "All" ? true : false);
        }
        else {
            if (this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName == 'IsOperationalClosed'; }).length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(function (a) { return a.FieldName != 'IsOperationalClosed'; });
            }
        }
        //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ForwarderShipmentsFilter').length > 0) {
        //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ForwarderShipmentsFilter');
        //}
        //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'NotForwarderShipmentsFilter').length > 0) {
        //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'NotForwarderShipmentsFilter');
        //}
        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
        //this.filterAgrs.SortBy = "StatusDate";
        //this.filterAgrs.SortDirection = "Descending";
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    ForwarderChooseShipmentsComponent.prototype.GridAfterViewInitCompleted = function ($event) {
        this.LoadImporterShipments();
    };
    ForwarderChooseShipmentsComponent.prototype.RefreshBtnClick = function () {
        this.LoadImporterShipments();
    };
    ForwarderChooseShipmentsComponent.prototype.onSearchTextChangeEvent = function (event) {
        this.searchFields = event;
        this.SearchFieldchangeevent.emit(this.searchFields);
        this.SelectedRow = null;
    };
    ForwarderChooseShipmentsComponent.prototype.CloseButtonClicked = function () {
        this.Close();
    };
    ForwarderChooseShipmentsComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ForwarderChooseShipmentsComponent.prototype.GetSourceEntity = function (Id) {
        this.CurrentSession.FireEvent({ Name: 'SourceEntity', Entity: this.SourceEntity, EntityId: Id });
    };
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "SelectedTransportationTypes", {
        get: function () {
            return this.selectedTransportationTypes;
        },
        set: function (newValue) {
            this.selectedTransportationTypes = newValue;
            this.TransportModeId = newValue.TransporationType;
            this.ToPortId = newValue.ToPortCode;
        },
        enumerable: true,
        configurable: true
    });
    ForwarderChooseShipmentsComponent.prototype.onTransportationTypeChange = function ($event) {
        this.SelectedTransportationTypes = $event;
    };
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "ShipperName", {
        get: function () { return this.SourceEntity.ShipperName; },
        set: function (newValue) { this.SourceEntity.ShipperName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "CustomerReference1", {
        get: function () { return this.SourceEntity.CustomerReference1; },
        set: function (newValue) { this.SourceEntity.CustomerReference1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "CustomerId", {
        get: function () { return this.SourceEntity.CustomerId; },
        set: function (newValue) { this.SourceEntity.CustomerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "StatusDate", {
        get: function () { return this.SourceEntity.StatusDate; },
        set: function (newValue) { this.SourceEntity.StatusDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "StatusId", {
        get: function () { return this.SourceEntity.StatusId; },
        set: function (newValue) { this.SourceEntity.StatusId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "ForwarderPartnerId", {
        get: function () { return this.SourceEntity.ForwarderPartnerId; },
        set: function (newValue) { this.SourceEntity.ForwarderPartnerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "DepartmentId", {
        get: function () { return this.SourceEntity.DepartmentId; },
        set: function (newValue) { this.SourceEntity.DepartmentId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "BranchId", {
        get: function () { return this.SourceEntity.BranchId; },
        set: function (newValue) { this.SourceEntity.BranchId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "ShipmentLevelCode", {
        get: function () { return this.SourceEntity.ShipmentLevelCode; },
        set: function (newValue) { this.SourceEntity.ShipmentLevelCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "DirectionId", {
        get: function () { return this.SourceEntity.DirectionId; },
        set: function (newValue) { this.SourceEntity.DirectionId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "TransportModeId", {
        get: function () { return this.SourceEntity.TransportModeId; },
        set: function (newValue) { this.SourceEntity.TransportModeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "ToPortId", {
        get: function () { return this.SourceEntity.ToPortId; },
        set: function (newValue) { this.SourceEntity.ToPortId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.SourceEntity.OtherPrepaidCollectId; },
        set: function (newValue) { this.SourceEntity.OtherPrepaidCollectId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.SourceEntity.FreightPrepaidCollectId; },
        set: function (newValue) { this.SourceEntity.FreightPrepaidCollectId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "FromPortId", {
        get: function () { return this.SourceEntity.FromPortId; },
        set: function (newValue) { this.SourceEntity.FromPortId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForwarderChooseShipmentsComponent.prototype, "Notes", {
        get: function () { return this.SourceEntity.Notes; },
        set: function (newValue) { this.SourceEntity.Notes = newValue; },
        enumerable: true,
        configurable: true
    });
    ForwarderChooseShipmentsComponent.prototype.CreateButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        }
        if (this.ValidationErrorsList.length == 0) {
            this._ShipmentPMService.GetSingleByCustomerReference1(this.CustomerReference1).subscribe(function (myResult) {
                if (myResult.Result) {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Title = "Warning !";
                    confirmWindow.Width = 300;
                    confirmWindow.Height = 150;
                    confirmWindow.YesButtonText = "Continue";
                    confirmWindow.NoButtonText = "Cancel";
                    confirmWindow.Show("Shipment (" + myResult.Result.ForwarderShipmentNumber + ") with the same order number is alreay exist in the query");
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.ContinueCreateShipmentProcess();
                        }
                        else {
                            _this.LoadImporterShipments(true);
                        }
                    });
                }
                else {
                    _this.ContinueCreateShipmentProcess();
                }
            });
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    ForwarderChooseShipmentsComponent.prototype.ContinueCreateShipmentProcess = function () {
        var _this = this;
        this._PortExtendedPMService.getSinglePort(this.SelectedTransportationTypes.ToPortCode, this.SelectedTransportationTypes.CountryCode, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            if (myResult.Result) {
                _this.ToPortId = myResult.Result.Id;
                if (Tools_1.AppTool.IsNullOrEmpty(_this.SourceEntity.FromPortId)) {
                    _this._PortExtendedPMService.getSinglePort("---", "IL", SessionLocator_1.SessionLocator.Tenant).subscribe(function (Result) {
                        _this.FromPortId = Result.Result.Id;
                        _this.SaveData();
                    });
                }
                else {
                    _this.SaveData();
                }
            }
            else {
                _this.SaveData();
            }
        });
    };
    ForwarderChooseShipmentsComponent.prototype.SaveData = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Shipment Creation in Progress ...");
            this.SourceEntity.IsImporterShipment = true;
            this.SourceEntity.MainCarriageFromPortId = this.SourceEntity.FromPortId;
            this.SourceEntity.MainCarriageToPortId = this.SourceEntity.ToPortId;
            this.SourceEntity.MainCarriageToPortId = this.SourceEntity.ToPortId;
            this._EntityStatusExtendedListService.getSingle("INPS").subscribe(function (Status) {
                _this.SourceEntity.StatusId = Status.Result.Id;
                _this._ShipmentPMService.update(_this.SourceEntity).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.SessionEvent.emit({ Name: "ReloadShipments" });
                        _this.CurrentSession.CurrentWindow.Close("");
                    }
                    else {
                        _this.ValidationErrorsList = myResult.ErrorsArray;
                    }
                });
            });
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ForwarderChooseShipmentsComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ForwarderChooseShipmentsComponent.prototype, "ShipmentSelectedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ForwarderChooseShipmentsComponent.prototype, "OnImporterShipmentsFilterChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ForwarderChooseShipmentsComponent.prototype, "CustomColumnsReady", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ForwarderChooseShipmentsComponent.prototype, "SearchFieldchangeevent", void 0);
    ForwarderChooseShipmentsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ForwarderChooseShipmentsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], ForwarderChooseShipmentsComponent);
    return ForwarderChooseShipmentsComponent;
}(BaseComponent_1.BaseComponent));
exports.ForwarderChooseShipmentsComponent = ForwarderChooseShipmentsComponent;
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
//# sourceMappingURL=ForwarderChooseShipmentsComponent.js.map