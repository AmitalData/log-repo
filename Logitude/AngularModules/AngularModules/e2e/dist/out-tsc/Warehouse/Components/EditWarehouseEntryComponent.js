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
var EventTypeArgs_2 = require("../../Infrastructure/DataContracts/EventTypeArgs");
var CardListService_1 = require("../../Common/Services/StandardLists/CardListService");
var EditWarehouseEntryComponent = /** @class */ (function (_super) {
    __extends(EditWarehouseEntryComponent, _super);
    function EditWarehouseEntryComponent(entityArgs, _traceEventExtendedPMService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._traceEventExtendedPMService = _traceEventExtendedPMService;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DataContext = _this;
        _this.IsLoadPage = false;
        _this.IsContainerShipment = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.warehouseEntryPM = _this.entityArgs.EntityPM;
        if (Tools_1.AppTool.IsNullOrEmpty(_this.warehouseEntryPM.ReceivedBy)) {
            _this.warehouseEntryPM.ReceivedBy = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            _this.warehouseEntryPM.IsDirty = false;
        }
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.Listen();
        // this.RunComponent();
        _this.myCardListService = new CardListService_1.CardListService();
        _this.warehouseEntryPM.UIProperties.SetEnabled("CreatedByUserId", "WarehouseEntry", false);
        var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
        if (table) {
            _this.ObjectTableId = table.Id;
        }
        return _this;
    }
    EditWarehouseEntryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntry", 0).subscribe(function (response) {
            _this.InitializeEditWarehouseEntry();
        });
    };
    EditWarehouseEntryComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EventTypeCodeList = [];
                        _this.warehouseEntryPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        //if (this.WarehouseEntryPackagesDetailsComponent) {
                        //    var windowArgs: any = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this, IsFromShipment: true, IsEditMode: true };
                        //    this.WarehouseEntryPackagesDetailsComponent.ReloadComponent(windowArgs);
                        //}
                        _this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("UPEN", null));
                        if (_this.warehouseEntryPM.ExpectedEntryDate != _this.ExpectedEntryDateOldValue) {
                            _this.ExpectedEntryDateOldValue = _this.warehouseEntryPM.ExpectedEntryDate;
                            _this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("EXEN", _this.warehouseEntryPM.ExpectedEntryDate));
                        }
                        if (_this.warehouseEntryPM.ActualEntryDate != _this.ActualEntryDateOldValue) {
                            _this.ActualEntryDateOldValue = _this.warehouseEntryPM.ActualEntryDate;
                            _this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("ENEN", _this.warehouseEntryPM.ActualEntryDate));
                        }
                        _this.UpdateEventType();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.warehouseEntryPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    EditWarehouseEntryComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    EditWarehouseEntryComponent.prototype.InitializeEditWarehouseEntry = function () {
        if (this.warehouseEntryPM) {
            this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
            this.IsContainerShipment = !this.IsLCLEntity;
            this.ExpectedEntryDateOldValue = this.warehouseEntryPM.ExpectedEntryDate;
            this.ActualEntryDateOldValue = this.warehouseEntryPM.ActualEntryDate;
            this.SetUIProperties();
            this.IsLoadPage = true;
        }
    };
    Object.defineProperty(EditWarehouseEntryComponent.prototype, "WarehouseId", {
        //@ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
        //private timerToken: any;
        //private Retries: number = 0;
        //private GeneratedComponent: any;
        //RunComponent() {
        //    if (this.AllLocations) {
        //        if (this.AllLocations.length == 0) {
        //            this.RunComponentTimer();
        //        }
        //        else {
        //            this.LoadChildComponent();
        //        }
        //    }
        //    else {
        //        this.RunComponentTimer();
        //    }
        //}
        //RunComponentTimer() {
        //    this.Retries++;
        //    if (this.timerToken) {
        //        clearTimeout(this.timerToken);
        //    }
        //    if (this.Retries < 3) {
        //        this.timerToken = setTimeout(() => this.RunComponent(), 1);
        //    }
        //}
        //WarehouseEntryPackagesDetailsComponent: any;
        //LoadChildComponent() {
        //    let warehouseEntryPackagesDetailsComponenttLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "WEPD")[0];
        //    if (warehouseEntryPackagesDetailsComponenttLocation != null) {
        //        SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseEntryPackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
        //            .then(cmpRef => {
        //                this.WarehouseEntryPackagesDetailsComponent = cmpRef.instance;
        //                var windowArgs: any = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this, IsFromShipment: true, IsEditMode:true };
        //                cmpRef.instance.SetWindowArgs(windowArgs);
        //            });
        //    }
        //}
        get: function () {
            var warehouseId = null;
            if (this.warehouseEntryPM)
                warehouseId = this.warehouseEntryPM.WarehouseId;
            return warehouseId;
        },
        set: function (newValue) {
            var _this = this;
            if (this.warehouseEntryPM.WarehouseId != newValue) {
                this.warehouseEntryPM.WarehouseId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.warehouseEntryPM.WarehouseName = null;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.warehouseEntryPM.WarehouseName = myCardList.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    EditWarehouseEntryComponent.prototype.SetUIProperties = function () {
        this.warehouseEntryPM.UIProperties.SetEnabled("CustomerId", "WarehouseEntry", false);
        //this.warehouseEntryPM.UIProperties.SetEnabled("WarehouseId", "WarehouseEntry", false);
    };
    EditWarehouseEntryComponent.prototype.UpdateEventType = function () {
        var _this = this;
        if (this.EventTypeCodeList && this.EventTypeCodeList.length != 0) {
            var traceEventArgs = new EventTypeArgs_1.EventTypeArgs();
            traceEventArgs.EventTypeList = this.EventTypeCodeList;
            traceEventArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
            traceEventArgs.ObjectTableId = this.ObjectTableId;
            traceEventArgs.EntityId = this.warehouseEntryPM.Id;
            traceEventArgs.LoggedContactId = SessionLocator_1.SessionLocator.LoggedUserId;
            this._traceEventExtendedPMService.PutTraceEventGroup(traceEventArgs).subscribe(function (res) {
                _this.CurrentSession.FireEvent("LoadEventTabData");
            });
        }
    };
    Object.defineProperty(EditWarehouseEntryComponent.prototype, "ActualEntryDate", {
        get: function () {
            var actualEntryDate = null;
            if (this.warehouseEntryPM)
                actualEntryDate = this.warehouseEntryPM.ActualEntryDate;
            return actualEntryDate;
        },
        set: function (value) {
            if (this.warehouseEntryPM != null) {
                if (value != this.warehouseEntryPM.ActualEntryDate) {
                    this.warehouseEntryPM.ActualEntryDate = value;
                    this.OnActualEntryDateDatePickerChange(value);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    EditWarehouseEntryComponent.prototype.OnActualEntryDateDatePickerChange = function (value) {
        this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", true, null);
        if (value) {
            if (!Tools_1.DateTool.IsActualDateValid(value)) {
                var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", "Actual Entry Date");
                this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", false, errorMessage);
            }
            else {
                if (this.warehouseEntryPM.StatusCode != "ENTE")
                    this.warehouseEntryPM.StatusCode = "ENTE";
            }
        }
        else {
            if (!this.warehouseEntryPM.ActualEntryDate) {
                this.warehouseEntryPM.StatusCode = "CREA";
            }
        }
    };
    EditWarehouseEntryComponent.prototype.SetActualDateClicked = function (fieldName) {
        this.ActualEntryDate = Tools_1.DateTool.GetDateParts(this.warehouseEntryPM.ExpectedEntryDate).DateObject;
    };
    EditWarehouseEntryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EditWarehouseEntryComponent',
            templateUrl: './EditWarehouseEntryComponent.html',
            providers: [TraceEventExtendedPMService_1.TraceEventExtendedPMService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, TraceEventExtendedPMService_1.TraceEventExtendedPMService])
    ], EditWarehouseEntryComponent);
    return EditWarehouseEntryComponent;
}(BaseComponent_1.BaseComponent));
exports.EditWarehouseEntryComponent = EditWarehouseEntryComponent;
//# sourceMappingURL=EditWarehouseEntryComponent.js.map