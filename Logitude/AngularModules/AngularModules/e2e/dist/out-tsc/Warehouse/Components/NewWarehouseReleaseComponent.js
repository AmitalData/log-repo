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
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var WarehouseEntryPackagePMExtendedService_1 = require("../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService");
var WarehouseReleasePM_1 = require("../../Warehouse/EntityPMs/WarehouseReleasePM");
var WarehouseReleasePMExtendedService_1 = require("../../Warehouse/Services/ExtendedPMs/WarehouseReleasePMExtendedService");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../Infrastructure/Tools");
var EventTypeArgs_1 = require("../../Infrastructure/DataContracts/EventTypeArgs");
var TraceEventExtendedPMService_1 = require("../../Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService");
var EventTypeArgs_2 = require("../../Infrastructure/DataContracts/EventTypeArgs");
var ClassLevelValidator_1 = require("../../Infrastructure/Validators/ClassLevelValidator");
var CommonDomainService_1 = require("../../Common/Services/CommonDomainService");
var WarehouseHelper_1 = require("../Helpers/WarehouseHelper");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var NewWarehouseReleaseComponent = /** @class */ (function (_super) {
    __extends(NewWarehouseReleaseComponent, _super);
    function NewWarehouseReleaseComponent(_warehouseReleasePMExtendedService, _traceEventExtendedPMService, warehouseEntryPackagePMExtendedService) {
        var _this = _super.call(this) || this;
        _this._warehouseReleasePMExtendedService = _warehouseReleasePMExtendedService;
        _this._traceEventExtendedPMService = _traceEventExtendedPMService;
        _this.warehouseEntryPackagePMExtendedService = warehouseEntryPackagePMExtendedService;
        _this.WarehouseReleasePackagesLists = [];
        _this.warehouseReleasePM = new WarehouseReleasePM_1.WarehouseReleasePM();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.AllWarehouseEntryPackagesLists = [];
        _this.CustomWarehouseEntryPackagesLists = [];
        _this.IsChangeWarehouseIdOrCustomerId = false;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsLoadPage = false;
        _this.IsLCLEntity = true;
        _this.IsLoadWarehouse = false;
        _this.IsChoosePackageOpen = false;
        _this.IsPackageOpen = false;
        _this.WarehouseId = "";
        _this.EventTypeCodeList = [];
        _this.GetNewInstance();
        _this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        var table = window.ObjectTables.filter(function (d) { return d.Name == "WarehouseRelease"; })[0];
        if (table) {
            _this.ObjectTableId = table.Id;
        }
        return _this;
    }
    NewWarehouseReleaseComponent.prototype.ngOnInit = function () {
    };
    NewWarehouseReleaseComponent.prototype.GetNewInstance = function () {
        this.warehouseReleasePM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        this.warehouseReleasePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.warehouseReleasePM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.warehouseReleasePM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.warehouseReleasePM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.warehouseReleasePM.StatusCode = "CREA";
        this.warehouseReleasePM.Id = "1-1";
        this.warehouseReleasePM.GrossWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode;
        this.warehouseReleasePM.VolumeUnitCode = SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode;
        this.warehouseReleasePM.DimensionsUnitCode = SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode;
        this.warehouseReleasePM.ChargeableWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.ChargeableWeightUnitCode;
        this.warehouseReleasePM.TotalVolume = 0;
        this.warehouseReleasePM.TotalGrossWeight = 0;
        this.warehouseReleasePM.TotalPieces = 0;
        this.warehouseReleasePM.ReleaseNumber = "123";
    };
    NewWarehouseReleaseComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("WarehouseRelease").subscribe(function (response) {
            _this.Start(args);
        });
    };
    NewWarehouseReleaseComponent.prototype.Start = function (args) {
        this.ShipmentPM = args.ShipmentPM;
        this.SetLabel();
        this.SetValue(args);
        this.IsLoadPage = true;
    };
    NewWarehouseReleaseComponent.prototype.SetValue = function (args) {
        var _this = this;
        if (this.warehouseReleasePM) {
            if (this.ShipmentPM) {
                if (this.ShipmentPM.ShipmentLevelCode == "D")
                    this.warehouseReleasePM.CustomerId = this.ShipmentPM.CustomerId;
                this.warehouseReleasePM.ShipmentId = this.ShipmentPM.Id;
                this.warehouseReleasePM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
                this.warehouseReleasePM.HouseNumber = this.ShipmentPM.House;
                this.warehouseReleasePM.MasterNumber = this.ShipmentPM.LongMaster;
                this.warehouseReleasePM.ShipmentLevelCode = this.ShipmentPM.ShipmentLevelCode;
                this.warehouseReleasePM.TransportModeId = this.ShipmentPM.TransportModeId;
                this.warehouseReleasePM.ShipmentTypeId = this.ShipmentPM.ShipmentTypeId;
                this.warehouseReleasePM.DirectionId = this.ShipmentPM.DirectionId;
            }
            this.TransportModeId = this.warehouseReleasePM.TransportModeId;
            this.DirectionId = this.warehouseReleasePM.DirectionId;
            this.FromPortId = this.ShipmentPM ? this.ShipmentPM.MainCarriageFromPortId ? this.ShipmentPM.MainCarriageFromPortId : this.ShipmentPM.FromPortId : "";
            this.ToPortId = this.ShipmentPM.ShipmentLevelCode == "H" ? this.ShipmentPM.MainCarriageFinalDestinationPortId : this.ShipmentPM.FinalDistenationPortId;
            this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
            var myCommonDomain = new CommonDomainService_1.CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var warehouseId = myResponse.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(warehouseId) && Tools_1.AppTool.IsNullOrEmpty(args.WarehouseId)) {
                        _this.warehouseReleasePM.WarehouseId = warehouseId;
                    }
                    else {
                        _this.warehouseReleasePM.WarehouseId = args.WarehouseId;
                    }
                }
                else {
                    _this.warehouseReleasePM.WarehouseId = args.WarehouseId;
                }
            });
            this.warehouseReleasePM.ExpectedReleaseDate = args.ExpectedReleaseDate;
            this.warehouseReleasePM.ActualReleaseDate = args.ActualReleaseDate;
            this.ActualReleaseDateOldValue = this.warehouseReleasePM.ActualReleaseDate;
            this.ExpectedReleaseDateOldValue = this.warehouseReleasePM.ExpectedReleaseDate;
        }
    };
    NewWarehouseReleaseComponent.prototype.SetLabel = function () {
        this.VolumeLabel = "Volume (" + SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode + ")";
        this.ChargeableWeightLabel = "ChargeableWeight (" + SessionLocator_1.SessionLocator.TenantPM.ChargeableWeightUnitCode + ")";
        this.WeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.ShipmentPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.ShipmentPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.ShipmentPM.ChargeableWeightUnitCode);
        this.PackageTypeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.PackageTypeId");
    };
    NewWarehouseReleaseComponent.prototype.LoadAllWarehouseEntryPackagesLists = function () {
        var _this = this;
        if (this.ShipmentPM) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(this.ShipmentPM.Id, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.ShipmentPM.Tenant).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    _this.AllWarehouseEntryPackagesLists = pmResponse.Result;
                }
            });
        }
    };
    NewWarehouseReleaseComponent.prototype.CustomerValueChange = function (item) {
        if (item) {
            this.RefreshWarehouseEntryPackagesLists("CustomerId", item);
        }
    };
    NewWarehouseReleaseComponent.prototype.WarehouseValueChange = function (item) {
        if (item) {
            this.RefreshWarehouseEntryPackagesLists("WarehouseId", item);
        }
    };
    NewWarehouseReleaseComponent.prototype.RefreshWarehouseEntryPackagesLists = function (fieldName, item) {
        var _this = this;
        if (this.CustomWarehouseEntryPackagesLists && this.CustomWarehouseEntryPackagesLists.length > 0) {
            var lists = [];
            if (fieldName == "WarehouseId") {
                lists = this.CustomWarehouseEntryPackagesLists.filter(function (d) { return d.WarehouseId == item.Id && d.CustomerId == _this.warehouseReleasePM.CustomerId; });
            }
            else {
                lists = this.CustomWarehouseEntryPackagesLists.filter(function (d) { return d.CustomerId == item.Id && d.WarehouseId == _this.warehouseReleasePM.WarehouseId; });
            }
            if (!lists || (lists && lists.length == 0)) {
                this.IsChangeWarehouseIdOrCustomerId = true;
                this.WarehouseReleasePackagesLists = [];
            }
        }
        else {
            this.IsChangeWarehouseIdOrCustomerId = true;
            this.WarehouseReleasePackagesLists = [];
        }
    };
    NewWarehouseReleaseComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewWarehouseReleaseComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("WarehouseRelease", this.warehouseReleasePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
        if (this.WarehouseReleasePackagesLists.length == 0) {
            this.ValidationErrorsList.push("You should at least choose one package");
        }
        else {
            if (this.warehouseReleasePM.ActualReleaseDate != null) {
                var releasePackagesLists = this.WarehouseReleasePackagesLists.filter(function (d) { return d.ActualReleaseDate != null; });
                var isValidReleasePackages = true;
                if (releasePackagesLists.length > 0) {
                    releasePackagesLists.forEach(function (item) {
                        if (Tools_1.DateTool.IsDateBigger(item.ActualReleaseDate, _this.warehouseReleasePM.ActualReleaseDate)) {
                            isValidReleasePackages = false;
                            return;
                        }
                    });
                    if (!isValidReleasePackages) {
                        this.ValidationErrorsList.push("Actual Release Date must be greater or equal to Actual Entry Date.");
                    }
                }
            }
        }
        var errors = [];
        // Actual Dates
        if (!Tools_1.DateTool.IsActualDateValid(this.warehouseReleasePM.ActualReleaseDate)) {
            this.ValidationErrorsList.push(Tools_1.DateTool.ActualDateMessage.replace("Field", "Actual Release Date"));
        }
        if (this.ValidationErrorsList.length == 0) {
            var message = "Can't set Field to future date";
            var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            if (this.warehouseReleasePM.ActualReleaseDate) {
                if (this.warehouseReleasePM.ActualReleaseDate.valueOf() > todayDateTime.valueOf()) {
                    this.ValidationErrorsList.push(message.replace("Field", "Actual Release Date"));
                }
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            if (this.WarehouseReleasePackagesLists.length > 0) {
                this.WarehouseReleasePackagesLists.forEach(function (item) {
                    _this.warehouseReleasePM.AddWarehouseReleasePackage(item);
                });
            }
            this.ComputeAndFullTotalPackage();
            this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("CRRE", null));
            if (this.warehouseReleasePM.ExpectedReleaseDate)
                this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("EXRE", this.warehouseReleasePM.ExpectedReleaseDate));
            if (this.warehouseReleasePM.ActualReleaseDate)
                this.EventTypeCodeList.push(new EventTypeArgs_2.EventTypeClass("ENRE", this.warehouseReleasePM.ActualReleaseDate));
            if (this.EventTypeCodeList.filter(function (d) { return d.Code == "ENRE"; })[0])
                this.warehouseReleasePM.StatusCode = "RELE";
            this._warehouseReleasePMExtendedService.Insert(this.warehouseReleasePM).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Cross Docs", "Create Release");
                    _this.warehouseReleasePM = pmResponse.Result;
                    _this.CurrentSession.FireEvent("CrossDockReleases");
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        var warehouseHelper = new WarehouseHelper_1.WarehouseHelper();
                        warehouseHelper.SetShipmentWarehouseLeg(_this.ShipmentPM, _this.warehouseReleasePM, "Release");
                        _this.UpdateEventType();
                    }
                }
                else {
                    pmResponse.ErrorsArray.forEach(function (item) {
                        _this.ValidationErrorsList.push(item);
                    });
                }
            });
        }
    };
    NewWarehouseReleaseComponent.prototype.ComputeAndFullTotalPackage = function () {
        var totalPieces = 0;
        var totalVolume = 0;
        var totalGrossWeight = 0;
        if (this.WarehouseReleasePackagesLists && this.WarehouseReleasePackagesLists.length > 0) {
            this.WarehouseReleasePackagesLists.forEach(function (item) {
                if (item.Quantity)
                    totalPieces += item.Quantity;
                if (item.Volume)
                    totalVolume += item.Volume;
                if (item.Weight)
                    totalGrossWeight += item.Weight;
            });
        }
        this.warehouseReleasePM.TotalPieces = totalPieces;
        this.warehouseReleasePM.TotalVolume = totalVolume;
        this.warehouseReleasePM.TotalGrossWeight = totalGrossWeight;
        this.warehouseReleasePM.GrossWeightUnitCode = "KG";
        this.warehouseReleasePM.VolumeUnitCode = "CBM";
    };
    NewWarehouseReleaseComponent.prototype.ChoosePackage = function (packageType) {
        var _this = this;
        this.IsChoosePackageOpen = true;
        if (!this.IsPackageOpen) {
            this.IsPackageOpen = true;
            this.IsChoosePackageOpen = true;
            if (this.CustomerId != this.warehouseReleasePM.CustomerId || this.WarehouseId != this.warehouseReleasePM.WarehouseId) {
                var shipmentId = this.ShipmentPM ? this.ShipmentPM.Id : "";
                this.AllWarehouseEntryPackagesLists = [];
                this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(shipmentId, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.ShipmentPM.Tenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        _this.AllWarehouseEntryPackagesLists = pmResponse.Result;
                        _this.OpenChoosePackage(packageType);
                    }
                });
            }
            else {
                this.OpenChoosePackage(packageType);
            }
        }
    };
    NewWarehouseReleaseComponent.prototype.OpenChoosePackage = function (packageType) {
        var _this = this;
        this.WarehouseId = this.warehouseReleasePM.WarehouseId;
        this.CustomerId = this.warehouseReleasePM.CustomerId;
        this.IsPackageOpen = false;
        if (this.IsChangeWarehouseIdOrCustomerId) {
            this.AllWarehouseEntryPackagesLists.forEach(function (item) {
                item.ReleaseQTY = 0;
            });
            this.IsChangeWarehouseIdOrCustomerId = false;
        }
        var windowArgs = {};
        windowArgs.WarehouseReleasePM = this.warehouseReleasePM;
        windowArgs.WarehouseEntryPackagesLists = this.AllWarehouseEntryPackagesLists;
        windowArgs.ViewModelTrigger = this;
        windowArgs.PackageType = packageType;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1150;
        logWindow.Height = 550;
        logWindow.Title = "Choose Packages";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/WarehouseReleaseChoosePackagesComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (_this.WarehouseReleasePackagesLists.length > 0 && _this.IsChoosePackageOpen) {
                _this.IsChoosePackageOpen = false;
                _this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
                _this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);
            }
        });
    };
    NewWarehouseReleaseComponent.prototype.OnActualReleaseDateDatePickerChange = function (value) {
        this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", true, null);
        if (!Tools_1.DateTool.IsActualDateValid(value)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", "Actual Release Date");
            this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", false, errorMessage);
        }
    };
    Object.defineProperty(NewWarehouseReleaseComponent.prototype, "ActualReleaseDate", {
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
    NewWarehouseReleaseComponent.prototype.SetActualDateClicked = function (fieldName) {
        this.ActualReleaseDate = Tools_1.DateTool.GetDateParts(this.warehouseReleasePM.ExpectedReleaseDate).DateObject;
    };
    NewWarehouseReleaseComponent.prototype.EditPackage = function (warehouseReleasePackagePM) {
    };
    NewWarehouseReleaseComponent.prototype.DeletePackage = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this package");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var index = _this.WarehouseReleasePackagesLists.indexOf(item);
                if (index != -1)
                    _this.WarehouseReleasePackagesLists.splice(index, 1);
                var entry = _this.AllWarehouseEntryPackagesLists.filter(function (d) { return d.Id == item.EntryPackageId; })[0];
                if (entry) {
                    entry.ReleaseQTY = 0;
                    entry.IsSelected = false;
                }
                if (_this.WarehouseReleasePackagesLists.length == 0) {
                    _this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", true);
                    _this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", true);
                }
            }
        });
    };
    NewWarehouseReleaseComponent.prototype.UpdateEventType = function () {
        var _this = this;
        if (this.EventTypeCodeList && this.EventTypeCodeList.length != 0) {
            var traceEventArgs = new EventTypeArgs_1.EventTypeArgs();
            traceEventArgs.EventTypeList = this.EventTypeCodeList;
            traceEventArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
            traceEventArgs.ObjectTableId = this.ObjectTableId;
            traceEventArgs.EntityId = this.warehouseReleasePM.Id;
            traceEventArgs.LoggedContactId = SessionLocator_1.SessionLocator.LoggedUserId;
            this._traceEventExtendedPMService.PutTraceEventGroup(traceEventArgs).subscribe(function (res) {
                _this.CurrentSession.CurrentWindow.Close("Refresh");
            });
        }
        else {
            this.CurrentSession.CurrentWindow.Close("Refresh");
        }
    };
    NewWarehouseReleaseComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'NewWarehouseReleaseComponent',
            templateUrl: './NewWarehouseReleaseComponent.html',
            providers: [WarehouseReleasePMExtendedService_1.WarehouseReleasePMExtendedService, TraceEventExtendedPMService_1.TraceEventExtendedPMService, WarehouseEntryPackagePMExtendedService_1.WarehouseEntryPackagePMExtendedService],
        }),
        __metadata("design:paramtypes", [WarehouseReleasePMExtendedService_1.WarehouseReleasePMExtendedService, TraceEventExtendedPMService_1.TraceEventExtendedPMService, WarehouseEntryPackagePMExtendedService_1.WarehouseEntryPackagePMExtendedService])
    ], NewWarehouseReleaseComponent);
    return NewWarehouseReleaseComponent;
}(BaseComponent_1.BaseComponent));
exports.NewWarehouseReleaseComponent = NewWarehouseReleaseComponent;
//# sourceMappingURL=NewWarehouseReleaseComponent.js.map