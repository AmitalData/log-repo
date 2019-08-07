"use strict";
/// <reference path="../../infrastructure/locators/servicelocator.ts" />
/// <reference path="../../infrastructure/utilities/infragenericfilter.ts" />
/// <reference path="../../shipment/entitypms/shipmentpm.ts" />
Object.defineProperty(exports, "__esModule", { value: true });
var PackageTypeListService_1 = require("../../Common/Services/StandardLists/PackageTypeListService");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var CardListService_1 = require("../../Common/Services/StandardLists/CardListService");
var Tools_1 = require("../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var WarehouseEntryPackagePM_1 = require("../../Warehouse/EntityPMs/WarehouseEntryPackagePM");
var WarehouseEntryPM_1 = require("../../Warehouse/EntityPMs/WarehouseEntryPM");
var WarehouseEntryPMService_1 = require("../../Warehouse/Services/StandardPMs/WarehouseEntryPMService");
var EventTypeArgs_1 = require("../../Infrastructure/DataContracts/EventTypeArgs");
var TraceEventExtendedPMService_1 = require("../../Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService");
var ClassLevelValidator_1 = require("../../Infrastructure/Validators/ClassLevelValidator");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var WarehouseHelper = /** @class */ (function () {
    function WarehouseHelper() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    WarehouseHelper.prototype.SetShipmentWarehouseLeg = function (shipmentPM, warehouseEntity, type) {
        var _this = this;
        if (shipmentPM && warehouseEntity && !Tools_1.AppTool.IsNullOrEmpty(type)) {
            var isShipmentDirty = shipmentPM.IsDirty;
            if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.WarehouseLegWarehouseId)) {
                if (shipmentPM.DirectionId != "C" && (shipmentPM.ShipmentLevelCode == "D" || shipmentPM.ShipmentLevelCode == "H")) {
                    var myService = new CardListService_1.CardListService();
                    myService.getSingle(warehouseEntity.WarehouseId).subscribe(function (myResponse) {
                        if (myResponse != null) {
                            shipmentPM.WarehouseLegWarehouseId = warehouseEntity.WarehouseId;
                            if (type == "Release") {
                                shipmentPM.WarehouseLegActualReleaseDate = warehouseEntity.ActualReleaseDate;
                                shipmentPM.WarehouseLegExpectedReleaseDate = warehouseEntity.ExpectedReleaseDate;
                            }
                            else {
                                shipmentPM.WarehouseLegActualEntryDate = warehouseEntity.ActualEntryDate;
                                shipmentPM.WarehouseLegExpectedEntryDate = warehouseEntity.ExpectedEntryDate;
                            }
                            if (!myResponse.HasError) {
                                var result = myResponse.Result;
                                if (result) {
                                    var isFirmCodeVisible = false;
                                    if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.CountryCode)) {
                                        isFirmCodeVisible = (SessionLocator_1.SessionLocator.TenantPM.CountryCode.toUpperCase()) == "US" ? true : false;
                                    }
                                    shipmentPM.WarehouseLegAddressId = result.MainAddressId;
                                    shipmentPM.WarehouseLegTerminalName = result.EnglishName;
                                    shipmentPM.WarehouseLegTerminalCode = result.FirmCode;
                                    if (isFirmCodeVisible) {
                                        shipmentPM.WarehouseLegTerminalCode = result.FirmCode;
                                    }
                                    if (_this.CurrentSession.CurrentEditComponent) {
                                        _this.CurrentSession.CurrentEditComponent.SaveChanges();
                                    }
                                    if (!isShipmentDirty)
                                        shipmentPM.IsDirty = false;
                                    _this.CurrentSession.FireEvent("RefreshWareHouseLeg");
                                }
                            }
                        }
                    });
                }
            }
        }
    };
    WarehouseHelper.prototype.ShowNewWarehouseEntryComponent = function (windowArgs) {
        var shipmentPackages = [];
        var entityPM = windowArgs.EntityPM;
        var entityChildPM = windowArgs.EntityChildPM;
        if (windowArgs.PageRequest == "ShipmentPickUp") {
            shipmentPackages = entityChildPM ? entityChildPM.ShipmentPickUpDeliveryPackages : [];
        }
        else
            shipmentPackages = entityPM ? entityPM.ShipmentPackages : [];
        if (shipmentPackages && shipmentPackages.length > 0) {
            windowArgs.WarehouseEntryPackagesLists = this.FullWarehouseEntryPackagePM(shipmentPackages, entityPM);
        }
        windowArgs.ShipmentPM = entityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 620;
        logWindow.Title = "New Cross Dock Entry";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/NewWarehouseEntryComponent");
    };
    WarehouseHelper.prototype.FullWarehouseEntryPackagePM = function (shipmentPackages, entityPM, packageType) {
        if (packageType === void 0) { packageType = null; }
        var warehouseEntryPackagesLists = [];
        if (shipmentPackages && shipmentPackages.length > 0) {
            shipmentPackages.forEach(function (item) {
                var warehouseEntryPackagePM = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(null);
                warehouseEntryPackagePM.Width = item.Width;
                warehouseEntryPackagePM.Height = item.Height;
                warehouseEntryPackagePM.Length = item.Length;
                warehouseEntryPackagePM.Description = item.Description;
                warehouseEntryPackagePM.Quantity = item.Quantity;
                warehouseEntryPackagePM.Volume = item.Volume;
                warehouseEntryPackagePM.Weight = item.Weight;
                warehouseEntryPackagePM.ContainerNumber = item.ContainerNumber;
                warehouseEntryPackagePM.Seal = item.Seal;
                warehouseEntryPackagePM.Tenant = item.Tenant;
                warehouseEntryPackagePM.Harmonize = item.Harmonize;
                warehouseEntryPackagePM.Instock = item.Quantity;
                warehouseEntryPackagePM.PackageTypeId = item.PackageTypeId;
                warehouseEntryPackagePM.PackageTypeName = item.PackageTypeName;
                warehouseEntryPackagePM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
                warehouseEntryPackagePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                warehouseEntryPackagePM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                warehouseEntryPackagePM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                warehouseEntryPackagePM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                warehouseEntryPackagePM.VolumetricWeight = item.VolumetricWeight;
                if (packageType == "ShipmentPackages")
                    warehouseEntryPackagePM.IsContainer = item.IsContainer;
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.PackageTypeId)) {
                        var myService = new PackageTypeListService_1.PackageTypeListService();
                        myService.getSingleFromCache(item.PackageTypeId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list != null) {
                                    warehouseEntryPackagePM.IsContainer = list.IsContainer;
                                }
                            }
                        });
                    }
                    else {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                            warehouseEntryPackagePM.IsContainer = true;
                        }
                    }
                }
                warehouseEntryPackagePM.Id = "1-1";
                warehouseEntryPackagePM.WarehouseEntryId = "1-1";
                var height = warehouseEntryPackagePM.Height ? warehouseEntryPackagePM.Height.toString() : "";
                var width = warehouseEntryPackagePM.Width ? warehouseEntryPackagePM.Width.toString() : "";
                var length = warehouseEntryPackagePM.Length ? warehouseEntryPackagePM.Length.toString() : "";
                warehouseEntryPackagePM.Dimensions = length + "-" + width + "-" + length;
                if (!Tools_1.AppTool.IsNullOrEmpty(warehouseEntryPackagePM.ContainerNumber) && warehouseEntryPackagePM.IsContainer) {
                    var error = Tools_1.FormatTool.ValidateContainerNumber(warehouseEntryPackagePM.ContainerNumber);
                    warehouseEntryPackagePM.ContainerNumberWarning = error;
                }
                warehouseEntryPackagesLists.push(warehouseEntryPackagePM);
            });
        }
        return warehouseEntryPackagesLists;
    };
    WarehouseHelper.prototype.CreateWarehouseEntry = function (entityPM, viewModel) {
        var _this = this;
        if (entityPM != null && viewModel != null) {
            viewModel.ValidationErrorsList = [];
            if (this.validator == null) {
                this.validator = new ClassLevelValidator_1.ClassLevelValidator();
            }
            var errorsArray = this.validator.Validate("WarehouseEntry", entityPM);
            if (errorsArray.length > 0) {
                errorsArray.forEach(function (item) {
                    viewModel.ValidationErrorsList.push(item);
                });
            }
            if (entityPM.WarehouseEntryPackages.length == 0) {
                viewModel.ValidationErrorsList.push("You should at least add one package");
            }
            // Actual Dates
            if (!Tools_1.DateTool.IsActualDateValid(entityPM.ActualEntryDate)) {
                viewModel.ValidationErrorsList.push(Tools_1.DateTool.ActualDateMessage.replace("Field", "Actual Entry Date"));
            }
            if (viewModel.ValidationErrorsList.length == 0) {
                if (entityPM.WarehouseEntryPackages.length > 0 && entityPM.ConnectedToShipment) {
                    entityPM.WarehouseEntryPackages.forEach(function (item) {
                        item.IsConnectedToShipment = true;
                    });
                }
                this.CurrentSession.StartBusyIndicatorSaving();
                if (this._warehouseEntryPMService == null)
                    this._warehouseEntryPMService = new WarehouseEntryPMService_1.WarehouseEntryPMService();
                if (this.traceEventExtendedPMService == null)
                    this.traceEventExtendedPMService = new TraceEventExtendedPMService_1.TraceEventExtendedPMService();
                var eventTypeCodeList = [];
                eventTypeCodeList.push(new EventTypeArgs_1.EventTypeClass("CREN", null));
                if (entityPM.ExpectedEntryDate)
                    eventTypeCodeList.push(new EventTypeArgs_1.EventTypeClass("EXEN", entityPM.ExpectedEntryDate));
                if (entityPM.ActualEntryDate)
                    eventTypeCodeList.push(new EventTypeArgs_1.EventTypeClass("ENEN", entityPM.ActualEntryDate));
                if (eventTypeCodeList.filter(function (d) { return d.Code == "ENEN"; })[0])
                    entityPM.StatusCode = "ENTE";
                this._warehouseEntryPMService.insert(entityPM).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Cross Docs", "Create Entry");
                        entityPM = pmResponse.Result;
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            if (viewModel.IsFromShipment && !viewModel.IsNotSetWarehouseIdForWarehouseLegShipment) {
                                _this.SetShipmentWarehouseLeg(viewModel.ShipmentPM, entityPM, "Entry");
                            }
                            _this.UpdateEventType(viewModel.ObjectTableId, entityPM.Id, eventTypeCodeList);
                        }
                        else
                            _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        pmResponse.ErrorsArray.forEach(function (item) {
                            viewModel.ValidationErrorsList.push(item);
                        });
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    };
    WarehouseHelper.prototype.UpdateEventType = function (objectTableId, entityId, eventTypeClass) {
        var _this = this;
        if (eventTypeClass && eventTypeClass.length != 0) {
            var traceEventArgs = new EventTypeArgs_1.EventTypeArgs();
            traceEventArgs.EventTypeList = eventTypeClass;
            traceEventArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
            traceEventArgs.ObjectTableId = objectTableId;
            traceEventArgs.EntityId = entityId;
            traceEventArgs.LoggedContactId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.traceEventExtendedPMService.PutTraceEventGroup(traceEventArgs).subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CurrentWindow.Close("Refresh");
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");
        }
    };
    WarehouseHelper.prototype.SetLabel = function (viewModel) {
        if (viewModel != null) {
            viewModel.VolumeLabel = "Volume (" + SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode + ")";
            viewModel.GrossWeightLabel = "Gross Weight (" + SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode + ")";
            viewModel.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode + ")";
        }
    };
    WarehouseHelper.prototype.GetNewWarehouseEntry = function (viewModel) {
        var warehouseEntryPM = new WarehouseEntryPM_1.WarehouseEntryPM();
        if (viewModel != null) {
            warehouseEntryPM.ReceivedBy = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            warehouseEntryPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
            warehouseEntryPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            warehouseEntryPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            warehouseEntryPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            warehouseEntryPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            warehouseEntryPM.StatusCode = "CREA";
            warehouseEntryPM.Id = "1-1";
            warehouseEntryPM.GrossWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode;
            warehouseEntryPM.VolumeUnitCode = SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode;
            warehouseEntryPM.DimensionsUnitCode = SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode;
            warehouseEntryPM.ChargeableWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.ChargeableWeightUnitCode;
            warehouseEntryPM.EntryNumber = "123";
            //if (viewModel.IsFromShipment) {
            warehouseEntryPM.TotalVolume = 0;
            warehouseEntryPM.TotalGrossWeight = 0;
            warehouseEntryPM.TotalPieces = 0;
            // }
        }
        return warehouseEntryPM;
    };
    return WarehouseHelper;
}());
exports.WarehouseHelper = WarehouseHelper;
//# sourceMappingURL=WarehouseHelper.js.map