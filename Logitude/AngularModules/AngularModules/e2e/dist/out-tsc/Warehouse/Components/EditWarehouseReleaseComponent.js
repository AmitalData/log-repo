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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var EntityArgs_1 = require("../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../Infrastructure/Tools");
var EventTypeArgs_1 = require("../../Infrastructure/DataContracts/EventTypeArgs");
var TraceEventExtendedPMService_1 = require("../../Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService");
var ShipmentPMService_1 = require("../../Shipment/Services/StandardPMs/ShipmentPMService");
var EventTypeArgs_2 = require("../../Infrastructure/DataContracts/EventTypeArgs");
var EditWarehouseReleaseComponent = /** @class */ (function (_super) {
    __extends(EditWarehouseReleaseComponent, _super);
    function EditWarehouseReleaseComponent(entityArgs, _traceEventExtendedPMService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._traceEventExtendedPMService = _traceEventExtendedPMService;
        _this.DataContext = _this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.WarehouseReleasePackagesLists = [];
        _this.IsLoadPage = false;
        _this.IsContainerShipment = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CancelReleaseChangedEvent = null;
        _this.SaveCompletedChangedEvent = null;
        _this.LoadCompletedChangedEvent = null;
        _this.IsScreenEnabled = true;
        _this.myShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        if (_this.entityArgs.EntityPM) {
            _this.warehouseReleasePM = _this.entityArgs.EntityPM;
            _this.WarehouseReleasePackagesLists = _this.entityArgs.EntityPM.WarehouseReleasePackages;
        }
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.Listen();
        _this.warehouseReleasePM.UIProperties.SetEnabled("CreatedByUserId", "WarehouseRelease", false);
        var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
        if (table) {
            _this.ObjectTableId = table.Id;
        }
        return _this;
    }
    EditWarehouseReleaseComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("WarehouseRelease", 0).subscribe(function (response) {
            if (_this.entityArgs.EntityPM) {
                _this.InitializeEditWarehouseRelease();
            }
        });
    };
    EditWarehouseReleaseComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (!this.SaveCompletedChangedEvent) {
                this.SaveCompletedChangedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EventTypeCodeList = [];
                        _this.warehouseReleasePM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("UPRE", null));
                        if (_this.warehouseReleasePM.ExpectedReleaseDate != _this.ExpectedReleaseDateOldValue) {
                            _this.ExpectedReleaseDateOldValue = _this.warehouseReleasePM.ExpectedReleaseDate;
                            _this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("EXRE", _this.warehouseReleasePM.ExpectedReleaseDate));
                        }
                        if (_this.warehouseReleasePM.ActualReleaseDate != _this.ActualReleaseDateOldValue) {
                            _this.ActualReleaseDateOldValue = _this.warehouseReleasePM.ActualReleaseDate;
                            _this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("ENRE", _this.warehouseReleasePM.ActualReleaseDate));
                        }
                        _this.UpdateEventType();
                    }
                });
            }
            if (!this.LoadCompletedChangedEvent) {
                this.LoadCompletedChangedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.warehouseReleasePM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
        if (!this.CancelReleaseChangedEvent) {
            this.CancelReleaseChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "CancelRelease") {
                    _this.SetEnableProperties();
                }
            });
        }
    };
    EditWarehouseReleaseComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.CancelReleaseChangedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedChangedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedChangedEvent);
    };
    EditWarehouseReleaseComponent.prototype.InitializeEditWarehouseRelease = function () {
        var _this = this;
        if (this.warehouseReleasePM) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.warehouseReleasePM.TransportModeId)) {
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
                if (!this.IsLCLEntity) {
                    this.IsContainerShipment = true;
                }
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.warehouseReleasePM.ShipmentId)) {
                this.CurrentSession.StartBusyIndicatorLoading();
                this.myShipmentPMService.get(this.warehouseReleasePM.ShipmentId).subscribe(function (res) {
                    var shipResponse = res;
                    _this.CurrentSession.StopBusyIndicator();
                    if (!shipResponse.HasError) {
                        _this.ShipmentPM = shipResponse.Result;
                        if (_this.ShipmentPM) {
                            _this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(_this.ShipmentPM.TransportModeId, _this.warehouseReleasePM.ShipmentTypeId);
                            if (!_this.IsLCLEntity) {
                                _this.IsContainerShipment = true;
                            }
                        }
                    }
                });
            }
            this.ActualReleaseDateOldValue = this.warehouseReleasePM.ActualReleaseDate;
            this.ExpectedReleaseDateOldValue = this.warehouseReleasePM.ExpectedReleaseDate;
            this.SetLabel();
            this.SetUIProperties();
            this.SetEnableProperties();
            this.IsLoadPage = true;
        }
    };
    EditWarehouseReleaseComponent.prototype.SetUIProperties = function () {
        this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
        this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);
    };
    EditWarehouseReleaseComponent.prototype.SetLabel = function () {
        this.VolumeLabel = "Volume (" + this.warehouseReleasePM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + this.warehouseReleasePM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseReleasePM.ChargeableWeightUnitCode + ")";
    };
    EditWarehouseReleaseComponent.prototype.SetEnableProperties = function () {
        if (this.warehouseReleasePM.StatusCode == "CARE") {
            this.warehouseReleasePM.UIProperties.SetEnabled("CustomerRef1", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("CustomerRef2", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("CreatedByUserId", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("ReleaseBy", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("MasterNumber", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("HouseNumber", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("ExpectedReleaseDate", "WarehouseRelease", false);
            this.UIProperties.SetEnabled("ActualReleaseDate", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("SpecialInstruction", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("Notes", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);
            this.IsScreenEnabled = false;
        }
    };
    EditWarehouseReleaseComponent.prototype.UpdateEventType = function () {
        var _this = this;
        if (this.EventTypeCodeList && this.EventTypeCodeList.length != 0) {
            var traceEventArgs = new EventTypeArgs_1.EventTypeArgs();
            traceEventArgs.EventTypeList = this.EventTypeCodeList;
            traceEventArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
            traceEventArgs.ObjectTableId = this.ObjectTableId;
            traceEventArgs.EntityId = this.warehouseReleasePM.Id;
            traceEventArgs.LoggedContactId = SessionLocator_1.SessionLocator.LoggedUserId;
            this._traceEventExtendedPMService.PutTraceEventGroup(traceEventArgs).subscribe(function (res) {
                _this.CurrentSession.FireEvent("LoadEventTabData");
            });
        }
    };
    EditWarehouseReleaseComponent.prototype.OnActualReleaseDateDatePickerChange = function (value) {
        this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", true, null);
        if (value) {
            if (!Tools_1.DateTool.IsActualDateValid(value)) {
                var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", "Actual Release Date");
                this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", false, errorMessage);
            }
            else {
                if (this.warehouseReleasePM.StatusCode != "RELE")
                    this.warehouseReleasePM.StatusCode = "RELE";
            }
        }
        else {
            if (!this.warehouseReleasePM.ActualReleaseDate) {
                this.warehouseReleasePM.StatusCode = "CREA";
            }
        }
    };
    Object.defineProperty(EditWarehouseReleaseComponent.prototype, "ActualReleaseDate", {
        get: function () {
            var actualReleaseDate = null;
            if (this.warehouseReleasePM)
                actualReleaseDate = this.warehouseReleasePM.ActualReleaseDate;
            return actualReleaseDate;
        },
        set: function (value) {
            if (this.warehouseReleasePM != null) {
                if (value != this.warehouseReleasePM.ActualReleaseDate) {
                    this.warehouseReleasePM.ActualReleaseDate = value;
                    this.OnActualReleaseDateDatePickerChange(value);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    EditWarehouseReleaseComponent.prototype.SetActualDateClicked = function (fieldName) {
        this.ActualReleaseDate = Tools_1.DateTool.GetDateParts(this.warehouseReleasePM.ExpectedReleaseDate).DateObject;
    };
    EditWarehouseReleaseComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EditWarehouseReleaseComponent',
            templateUrl: './EditWarehouseReleaseComponent.html',
            providers: [TraceEventExtendedPMService_1.TraceEventExtendedPMService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, TraceEventExtendedPMService_1.TraceEventExtendedPMService])
    ], EditWarehouseReleaseComponent);
    return EditWarehouseReleaseComponent;
}(BaseComponent_1.BaseComponent));
exports.EditWarehouseReleaseComponent = EditWarehouseReleaseComponent;
//# sourceMappingURL=EditWarehouseReleaseComponent.js.map