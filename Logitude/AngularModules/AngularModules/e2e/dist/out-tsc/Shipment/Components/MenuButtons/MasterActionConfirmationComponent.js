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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var Tools_2 = require("../../Tools");
var ShipmentValidator_1 = require("../../Validators/ShipmentValidator");
var EntityWarningsValidator_1 = require("../../../Infrastructure/Validators/EntityWarningsValidator");
var RulesValidator_1 = require("../../../Infrastructure/Validators/RulesValidator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var PackageTypeListService_1 = require("../../../Common/Services/StandardLists/PackageTypeListService");
var ShipmentDomainService_1 = require("../../Services/ShipmentDomainService");
var DateTimePipe_1 = require("../../../Controls/Pipes/DateTimePipe");
var MasterActionConfirmationComponent = /** @class */ (function (_super) {
    __extends(MasterActionConfirmationComponent, _super);
    function MasterActionConfirmationComponent() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.FatherComponent = null;
        _this.MasterLists = [];
        _this.FCLVisibility = false;
        _this.LCLVisibility = false;
        _this.GroupageVisibility = false;
        _this.ErrorList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.MasterVSHousesVisibility = false;
        _this.LCL_ObsList = [];
        _this.FCL_ObsList1 = [];
        _this.FCL_ObsList2 = [];
        _this.GRO_ObsList = [];
        _this.EnabledOkButton = false;
        return _this;
    }
    MasterActionConfirmationComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.shipmentService = new ShipmentDomainService_1.ShipmentDomainService();
        this.shipmentService.GetConnectedShipmentsByMasterIdAndTenant(args.EntityPM.Id, args.EntityPM.Tenant).subscribe(function (response) {
            if (!response.HasError && response.Result) {
                _this.FatherComponent = args;
                _this.shipmentService.GetShipmentConsolidationPackages(_this.FatherComponent.EntityPM.Id).subscribe(function (result) {
                    _this.FatherComponent.EntityPM.IsOperationalClosed = true;
                    _this.MasterViewModel = new MasterActionConfirmationViewModel(_this.FatherComponent.EntityPM, result.Result);
                    _this.MasterViewModel.IsMaster = true;
                    _this.MasterViewModel.ValidateShipment(_this.FatherComponent.EntityPM);
                    var connectedShipments = response.Result;
                    _this.MasterLists.push(_this.MasterViewModel);
                    connectedShipments.forEach(function (shipment) {
                        shipment.IsOperationalClosed = true;
                        var viewmodel = new MasterActionConfirmationViewModel(shipment);
                        _this.MasterViewModel.ConnectedShipments.push(viewmodel);
                        viewmodel.ValidateShipment(shipment);
                        _this.MasterLists.push(viewmodel);
                    });
                    var actionSucceeded = false;
                    actionSucceeded = !_this.MasterViewModel.HasErrors();
                    _this.LCL_ObsList = _this.MasterViewModel.LCL_ObsList;
                    _this.FCL_ObsList1 = _this.MasterViewModel.FCL_ObsList1;
                    _this.FCL_ObsList2 = _this.MasterViewModel.FCL_ObsList2;
                    _this.GRO_ObsList = _this.MasterViewModel.GRO_ObsList;
                    _this.EnabledOkButton = _this.ConfirmIsEnabled(_this.MasterViewModel.HasErrors());
                    _this.CurrentSession.StopBusyIndicator();
                    _this.FCLVisibility = _this.MasterViewModel.FCLVisibility && _this.MasterViewModel.MasterVSHousesVisibility;
                    _this.LCLVisibility = _this.MasterViewModel.LCLVisibility && _this.MasterViewModel.MasterVSHousesVisibility;
                    _this.GroupageVisibility = _this.MasterViewModel.GroupageVisibility && _this.MasterViewModel.MasterVSHousesVisibility;
                    _this.MasterVSHousesVisibility = _this.MasterViewModel.MasterVSHousesVisibility;
                    _this.ErrorList = _this.MasterViewModel.ErrorList;
                    if (_this.MasterViewModel.ErrorList.length > 0)
                        _this.EnabledOkButton = false;
                });
            }
        });
    };
    MasterActionConfirmationComponent.prototype.ConfirmIsEnabled = function (prop) { return !prop; };
    MasterActionConfirmationComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("confirm");
    };
    MasterActionConfirmationComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    MasterActionConfirmationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MasterActionConfirmationComponent.html',
        })
    ], MasterActionConfirmationComponent);
    return MasterActionConfirmationComponent;
}(BaseComponent_1.BaseComponent));
exports.MasterActionConfirmationComponent = MasterActionConfirmationComponent;
var MasterActionConfirmationViewModel = /** @class */ (function () {
    function MasterActionConfirmationViewModel(shipment, houseShipmentsPackaes) {
        if (houseShipmentsPackaes === void 0) { houseShipmentsPackaes = null; }
        this.errorInfoVisibility = false;
        this.mismatchError = "";
        this.ErrorList = [];
        this.DatePipe = new DateTimePipe_1.DateTimePipe();
        this.shipmentService = new ShipmentDomainService_1.ShipmentDomainService();
        this.CurrentShipment = shipment;
        this.masterPM = shipment;
        if (houseShipmentsPackaes == null)
            this.houseShipmentsPackaes = new Array();
        else
            this.houseShipmentsPackaes = houseShipmentsPackaes;
        this.LCL_ObsList = new Array();
        this.FCL_ObsList1 = new Array();
        this.FCL_ObsList2 = new Array();
        this.GRO_ObsList = new Array();
        this.LoadHousePackages();
    }
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "IsMaster", {
        get: function () { return this.isMaster; },
        set: function (value) { this.isMaster = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "MasterVSHousesVisibility", {
        get: function () {
            if (this.masterPM.ShipmentConsoleShipments.length > 0) {
                return true;
            }
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "FCLVisibility", {
        get: function () {
            if (!Tools_2.ShipmentTool.IsLCL(this.masterPM)) {
                if (!this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                    return true;
                }
            }
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "GroupageVisibility", {
        get: function () {
            if (!Tools_2.ShipmentTool.IsLCL(this.masterPM)) {
                if (this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                    return true;
                }
            }
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "LCLVisibility", {
        get: function () { return Tools_2.ShipmentTool.IsLCL(this.masterPM) ? true : false; },
        enumerable: true,
        configurable: true
    });
    MasterActionConfirmationViewModel.prototype.LoadHousePackages = function () {
        if (this.masterPM.ShipmentConsoleShipments.length > 0) {
            this.BuildData();
        }
    };
    MasterActionConfirmationViewModel.prototype.BuildLCLData = function () {
        this.LCL_ObsList = [];
        var HouseValueSum = 0;
        var MasterValueSum = 0;
        var IsEqualsSum = false;
        var LineImageSum = "";
        this.houseShipmentsPackaes.forEach(function (item) { if (item.Quantity != null)
            HouseValueSum += item.Quantity; });
        this.masterPM.ShipmentPackages.forEach(function (item) { if (item.Quantity != null)
            MasterValueSum += item.Quantity; });
        if (HouseValueSum == MasterValueSum) {
            IsEqualsSum = true;
            LineImageSum = "/Images/SimplogIcons/GreenTick.png";
        }
        else
            LineImageSum = "/Images/SimplogIcons/Warning.png";
        var line1 = new LineData();
        line1.LineLabel = "Total packages";
        line1.HouseValue = HouseValueSum;
        line1.MasterValue = MasterValueSum;
        line1.IsEquals = IsEqualsSum;
        line1.LineImage = LineImageSum;
        ////////////////////////////////////////////////////
        HouseValueSum = 0;
        MasterValueSum = 0;
        IsEqualsSum = false;
        LineImageSum = "";
        this.masterPM.ShipmentConsoleShipments.forEach(function (item) { if (item.GrossWeight != null)
            HouseValueSum += item.GrossWeight; });
        MasterValueSum = this.masterPM.GrossWeight;
        if (HouseValueSum == MasterValueSum) {
            IsEqualsSum = true;
            LineImageSum = "/Images/SimplogIcons/GreenTick.png";
        }
        else
            LineImageSum = "/Images/SimplogIcons/Warning.png";
        var line2 = new LineData();
        line2.LineLabel = "Gross Weight";
        line2.HouseValue = HouseValueSum;
        line2.MasterValue = MasterValueSum;
        line2.IsEquals = IsEqualsSum;
        line2.LineImage = LineImageSum;
        ////////////////////////////////////////////////////
        HouseValueSum = 0;
        MasterValueSum = 0;
        IsEqualsSum = false;
        LineImageSum = "";
        this.masterPM.ShipmentConsoleShipments.forEach(function (item) { if (item.VolumetricWeight != null)
            HouseValueSum += item.VolumetricWeight; });
        MasterValueSum = this.masterPM.VolumetricWeight;
        if (HouseValueSum == MasterValueSum) {
            IsEqualsSum = true;
            LineImageSum = "/Images/SimplogIcons/GreenTick.png";
        }
        else
            LineImageSum = "/Images/SimplogIcons/Warning.png";
        var line3 = new LineData();
        line3.LineLabel = "Vol Weight";
        line3.HouseValue = HouseValueSum;
        line3.MasterValue = MasterValueSum;
        line3.IsEquals = IsEqualsSum;
        line3.LineImage = LineImageSum;
        this.LCL_ObsList.push(line1);
        this.LCL_ObsList.push(line2);
        this.LCL_ObsList.push(line3);
    };
    MasterActionConfirmationViewModel.prototype.BuildFCLData = function () {
        var _this = this;
        this.FCL_ObsList1 = [];
        this.FCL_ObsList2 = [];
        var fclHousesGroup = [];
        var fclMasterGroup = [];
        var PackListservice = new PackageTypeListService_1.PackageTypeListService();
        var CachedList;
        PackListservice.getAllFromCache().subscribe(function (result) {
            if (!result.HasError) {
                CachedList = result.Result;
                _this.houseShipmentsPackaes = _this.houseShipmentsPackaes.filter(function (p) { return p.IsContainer == true; });
                _this.houseShipmentsPackaes.forEach(function (item) {
                    var existsedItem = fclHousesGroup.filter(function (f) { return f.PackageTypeId == item.PackageTypeId; })[0];
                    if (existsedItem == null) {
                        existsedItem = new Tools_2.ByPckageType();
                        existsedItem.PackageTypeId = item.PackageTypeId;
                        existsedItem.Quantity = item.Quantity;
                        existsedItem.MeasurementId = (CachedList.filter(function (f) { return f.Id == item.PackageTypeId; })[0]) != null ? (CachedList.filter(function (f) { return f.Id == item.PackageTypeId; })[0]).MeasurementId : null;
                        fclHousesGroup.push(existsedItem);
                    }
                    else {
                        existsedItem.Quantity += item.Quantity;
                    }
                });
                _this.masterPM.ShipmentPackages = _this.masterPM.ShipmentPackages.filter(function (p) { return p.IsContainer == true; });
                _this.masterPM.ShipmentPackages.forEach(function (item) {
                    var existsedItem = fclMasterGroup.filter(function (f) { return f.PackageTypeId == item.PackageTypeId; })[0];
                    if (existsedItem == null) {
                        existsedItem = new Tools_2.ByPckageType();
                        existsedItem.PackageTypeId = item.PackageTypeId;
                        existsedItem.Quantity = item.Quantity;
                        existsedItem.MeasurementId = (CachedList.filter(function (f) { return f.Id == item.PackageTypeId; })[0]) != null ? (CachedList.filter(function (f) { return f.Id == item.PackageTypeId; })[0]).MeasurementId : null;
                        fclMasterGroup.push(existsedItem);
                    }
                    else {
                        existsedItem.Quantity += item.Quantity;
                    }
                });
                fclHousesGroup.forEach(function (houseItem) {
                    var list = CachedList.filter(function (f) { return f.Id == houseItem.PackageTypeId; })[0];
                    if (list != null) {
                        var line = new LineData();
                        line.LineLabel = list.EnglishName;
                        line.HouseValue = houseItem.Quantity;
                        var masterItem = fclMasterGroup.filter(function (f) { return f.PackageTypeId == houseItem.PackageTypeId && f.MeasurementId == houseItem.MeasurementId; })[0];
                        if (masterItem != null) {
                            line.MasterValue = masterItem.Quantity;
                            line.IsEquals = (houseItem.Quantity == masterItem.Quantity);
                            line.LineImage = (houseItem.Quantity == masterItem.Quantity) ? "/Images/SimplogIcons/GreenTick.png" : "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                            var temp = [];
                            fclMasterGroup.forEach(function (p) {
                                if (p != masterItem)
                                    temp.push(p);
                            });
                            fclMasterGroup = temp;
                        }
                        else {
                            line.MasterValue = 0;
                            line.IsEquals = false;
                            line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                        }
                        _this.FCL_ObsList1.push(line);
                    }
                });
                fclMasterGroup.forEach(function (masterItem) {
                    var list = CachedList.filter(function (f) { return f.Id == masterItem.PackageTypeId; })[0];
                    if (list != null) {
                        var line = new LineData();
                        line.LineLabel = list.EnglishName;
                        line.HouseValue = 0;
                        line.MasterValue = masterItem.Quantity;
                        line.IsEquals = false;
                        line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                        _this.FCL_ObsList1.push(line);
                    }
                });
                var masterPackages = _this.masterPM.ShipmentPackages;
                _this.houseShipmentsPackaes.sort(function (a, b) { return (a.ShipmentId === b.ShipmentId) ? 0 : (a.ShipmentId < b.ShipmentId) ? -1 : 1; });
                _this.houseShipmentsPackaes.forEach(function (houseItem) {
                    var list = CachedList.filter(function (p) { return p.Id == houseItem.PackageTypeId; })[0];
                    if (list != null) {
                        var line = new LineData();
                        line.ShipmentNumber = houseItem.ShipmentNumber;
                        line.LineLabel = list.EnglishName;
                        line.HouseStringValue = Tools_1.AppTool.IsNullOrEmpty(houseItem.ContainerNumber) ? "- - -" : houseItem.ContainerNumber;
                        if (Tools_1.AppTool.IsNullOrEmpty(line.ShipmentNumber)) {
                            var dd = _this.masterPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == houseItem.ShipmentId; })[0];
                            if (dd != null) {
                                line.ShipmentNumber = dd.ShipmentNumber;
                            }
                        }
                        var masterItem = masterPackages.filter(function (d) { return d.OriginalShipmentPackageId == houseItem.Id; })[0];
                        if (masterItem != null) {
                            var temp = [];
                            masterPackages.forEach(function (p) {
                                if (p != masterItem)
                                    temp.push(p);
                            });
                            masterPackages = temp;
                            line.MasterStringValue = Tools_1.AppTool.IsNullOrEmpty(masterItem.ContainerNumber) ? "- - -" : masterItem.ContainerNumber;
                            line.IsEquals = (line.HouseStringValue == line.MasterStringValue);
                            line.LineImage = (line.HouseStringValue == line.MasterStringValue) ? "/Images/SimplogIcons/GreenTick.png" : "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                        }
                        else {
                            line.MasterStringValue = "Not exists";
                            line.IsEquals = false;
                            line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                        }
                        if (!line.IsEquals) {
                            _this.FCL_ObsList2.push(line);
                        }
                    }
                });
                masterPackages.forEach(function (item) {
                    var list = CachedList.filter(function (d) { return d.Id == item.PackageTypeId; })[0];
                    if (list != null) {
                        var line = new LineData();
                        line.ShipmentNumber = _this.masterPM.ShipmentNumber,
                            line.LineLabel = list.EnglishName,
                            line.HouseStringValue = "Not exists",
                            line.MasterStringValue = Tools_1.AppTool.IsNullOrEmpty(item.ContainerNumber) ? "- - -" : item.ContainerNumber,
                            line.IsEquals = false,
                            line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png",
                            _this.FCL_ObsList2.push(line);
                    }
                });
                if (_this.FCL_ObsList1.filter(function (d) { return d.IsEquals == false; })[0] || _this.FCL_ObsList2.filter(function (d) { return d.IsEquals == false; })[0]) {
                    _this.ErrorInfoVisibility = true;
                    _this.MismatchError = "Mismatch Quantities or Container numbers";
                    _this.ErrorList.push(_this.MismatchError);
                }
            }
        });
    };
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "ErrorInfoVisibility", {
        get: function () { return this.errorInfoVisibility; },
        set: function (value) { this.errorInfoVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "MismatchError", {
        get: function () { return this.mismatchError; },
        set: function (value) { this.mismatchError = value; },
        enumerable: true,
        configurable: true
    });
    MasterActionConfirmationViewModel.prototype.BuildGroupageData = function () {
        this.GRO_ObsList = [];
        var HouseValueSum = 0;
        var MasterValueSum = 0;
        var IsEqualsSum = false;
        var LineImageSum = "";
        this.houseShipmentsPackaes.forEach(function (item) { if (item.Quantity != null)
            HouseValueSum += item.Quantity; });
        this.masterPM.ShipmentPackages.forEach(function (item) {
            item.InsideShipmentPackages.forEach(function (d) {
                if (d.Quantity != null)
                    MasterValueSum += d.Quantity;
            });
        });
        if (HouseValueSum == MasterValueSum) {
            IsEqualsSum = true;
            LineImageSum = "/Images/SimplogIcons/GreenTick.png";
        }
        else
            LineImageSum = "/Images/SimplogIcons/Warning.png";
        var line1 = new LineData();
        line1.LineLabel = "Total packages";
        line1.HouseValue = HouseValueSum;
        line1.MasterValue = MasterValueSum;
        line1.IsEquals = IsEqualsSum;
        line1.LineImage = LineImageSum;
        ////////////////////////////////////////////////////
        HouseValueSum = 0;
        MasterValueSum = 0;
        IsEqualsSum = false;
        LineImageSum = "";
        this.masterPM.ShipmentConsoleShipments.forEach(function (item) { if (item.GrossWeight != null)
            HouseValueSum += item.GrossWeight; });
        MasterValueSum = this.masterPM.GrossWeight;
        if (HouseValueSum == MasterValueSum) {
            IsEqualsSum = true;
            LineImageSum = "/Images/SimplogIcons/GreenTick.png";
        }
        else
            LineImageSum = "/Images/SimplogIcons/Warning.png";
        var line2 = new LineData();
        line2.LineLabel = "Gross Weight";
        line2.HouseValue = HouseValueSum;
        line2.MasterValue = MasterValueSum;
        line2.IsEquals = IsEqualsSum;
        line2.LineImage = LineImageSum;
        ////////////////////////////////////////////////////
        HouseValueSum = 0;
        MasterValueSum = 0;
        IsEqualsSum = false;
        LineImageSum = "";
        this.masterPM.ShipmentConsoleShipments.forEach(function (item) { if (item.VolumetricWeight != null)
            HouseValueSum += item.VolumetricWeight; });
        MasterValueSum = this.masterPM.VolumetricWeight;
        if (HouseValueSum == MasterValueSum) {
            IsEqualsSum = true;
            LineImageSum = "/Images/SimplogIcons/GreenTick.png";
        }
        else
            LineImageSum = "/Images/SimplogIcons/Warning.png";
        var line3 = new LineData();
        line3.LineLabel = "Vol Weight";
        line3.HouseValue = HouseValueSum;
        line3.MasterValue = MasterValueSum;
        line3.IsEquals = IsEqualsSum;
        line3.LineImage = LineImageSum;
        this.GRO_ObsList.push(line1);
        this.GRO_ObsList.push(line2);
        this.GRO_ObsList.push(line3);
    };
    MasterActionConfirmationViewModel.prototype.BuildData = function () {
        if (this.masterPM.ShipmentConsoleShipments.length > 0) {
            if (Tools_2.ShipmentTool.IsLCL(this.masterPM)) {
                this.BuildLCLData();
            }
            else {
                if (!this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                    this.BuildFCLData();
                }
                else {
                    this.BuildGroupageData();
                }
            }
        }
    };
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "ShipmentNo", {
        get: function () { return this.CurrentShipment.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "FlightVoyage", {
        get: function () { return this.CurrentShipment.MainCarriageCarrierNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "MasterOrHouseNo", {
        get: function () {
            var masterOrHouse = null;
            if (this.CurrentShipment.ShipmentLevelCode == "H") {
                masterOrHouse = this.CurrentShipment.House;
            }
            else {
                masterOrHouse = this.CurrentShipment.Master;
            }
            return masterOrHouse;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "ATDATA", {
        get: function () {
            var atAandAtd = null;
            atAandAtd = (this.CurrentShipment.MainCarriageATD != null ? this.DatePipe.transform(this.CurrentShipment.MainCarriageATD) : "") + " / " + (this.CurrentShipment.MainCarriageATA != null ? this.DatePipe.transform(this.CurrentShipment.MainCarriageATA, "SD") : "");
            if (atAandAtd == " / ") {
                atAandAtd = "";
            }
            return atAandAtd;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "PPCC", {
        get: function () { return this.CurrentShipment.FreightPrepaidCollectId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "WarningsList", {
        get: function () {
            if (this.warningsList == null) {
                this.warningsList = new Array();
            }
            return this.warningsList;
        },
        set: function (value) {
            this.warningsList = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "ErrorsList", {
        get: function () {
            if (this.errorsList == null) {
                this.errorsList = new Array();
            }
            return this.errorsList;
        },
        set: function (value) {
            this.errorsList = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterActionConfirmationViewModel.prototype, "ConnectedShipments", {
        get: function () {
            if (this.connectedShipments == null) {
                this.connectedShipments = new Array();
            }
            return this.connectedShipments;
        },
        set: function (value) { this.connectedShipments = value; },
        enumerable: true,
        configurable: true
    });
    ;
    MasterActionConfirmationViewModel.prototype.HasErrors = function () {
        var hasErrors = false;
        if (!Tools_2.ShipmentTool.IsLCL(this.masterPM)) {
            if (!this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                if (this.FCL_ObsList1.filter(function (d) { return d.IsEquals == false; })[0] || this.FCL_ObsList2.filter(function (d) { return d.IsEquals == false; })[0]) {
                    hasErrors = true;
                }
            }
        }
        if (this.ErrorsList.length != 0) {
            hasErrors = true;
        }
        else {
            this.ConnectedShipments.forEach(function (model) {
                if (!model.IsMaster) {
                    if (model.HasErrors()) {
                        hasErrors = true;
                        return;
                    }
                }
            });
        }
        return hasErrors;
    };
    MasterActionConfirmationViewModel.prototype.HasWarnings = function () {
        var succeeded = true;
        if (this.WarningsList.length != 0) {
            succeeded = false;
        }
        else {
            this.ConnectedShipments.forEach(function (model) {
                {
                    if (model.HasWarnings()) {
                        succeeded = false;
                        return;
                    }
                }
            });
            return succeeded;
        }
    };
    MasterActionConfirmationViewModel.prototype.ValidateShipment = function (entityPM) {
        var _this = this;
        var validationResults = [];
        var masterObject = window.ObjectTables.filter(function (d) { return d.Name === 'Master'; })[0];
        var shipmentObject = window.ObjectTables.filter(function (d) { return d.Name === 'Shipment'; })[0];
        var validator = new ShipmentValidator_1.ShipmentValidator();
        var requiredFields = [];
        validationResults = validator.Validate(entityPM);
        validationResults.forEach(function (error) { _this.ErrorsList.push(error); });
        var succeeded = true;
        var ruleValidator = new RulesValidator_1.RulesValidator();
        var warningValidator = new EntityWarningsValidator_1.EntityWarningsValidator();
        if (entityPM.ShipmentLevelCode == "H") {
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_II", entityPM, requiredFields);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_IE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_II", entityPM, this.WarningsList);
        }
        if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C") {
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II_D", entityPM, this.WarningsList);
        }
        if (entityPM.ShipmentLevelCode == "C") {
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_War", entityPM, this.WarningsList);
        }
        var _tenantObjectFields = window.ObjectFields;
        if (requiredFields.length != 0) {
            succeeded = false;
            for (var k in requiredFields) {
                var field = requiredFields[k];
                var obField = _tenantObjectFields.filter(function (x) { return x.Id === field.ObjectFieldId; })[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
                var requiredError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
                var fieldTrans = TextCodeTranslator_1.TextCodeTranslator.Translate(obField.FullNameTextCodeCode);
                requiredError = requiredError.replace("%FieldName", fieldTrans);
                if (!this.ErrorsList.includes(requiredError))
                    this.ErrorsList.push(requiredError);
            }
        }
        if (this.ErrorsList.length != 0) {
            succeeded = false;
        }
        else {
            succeeded = true;
        }
        // this.WarningsList = validationResults;
        return succeeded;
    };
    return MasterActionConfirmationViewModel;
}());
exports.MasterActionConfirmationViewModel = MasterActionConfirmationViewModel;
var LineData = /** @class */ (function () {
    function LineData() {
    }
    Object.defineProperty(LineData.prototype, "LineLabel", {
        get: function () { return this.lineLabel; },
        set: function (value) { this.lineLabel = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineData.prototype, "HouseValue", {
        get: function () { return this.houseValue; },
        set: function (value) { this.houseValue = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineData.prototype, "MasterValue", {
        get: function () { return this.masterValue; },
        set: function (value) { this.masterValue = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineData.prototype, "LineImage", {
        get: function () { return this.lineImage; },
        set: function (value) { this.lineImage = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineData.prototype, "IsEquals", {
        get: function () { return this.isEquals; },
        set: function (value) { this.isEquals = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineData.prototype, "ShipmentNumber", {
        get: function () { return this.shipmentNumber; },
        set: function (value) { this.shipmentNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineData.prototype, "HouseStringValue", {
        get: function () { return this.houseStringValue; },
        set: function (value) { this.houseStringValue = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineData.prototype, "MasterStringValue", {
        get: function () { return this.masterStringValue; },
        set: function (value) { this.masterStringValue = value; },
        enumerable: true,
        configurable: true
    });
    return LineData;
}());
exports.LineData = LineData;
var ValidationErrorInfo = /** @class */ (function () {
    function ValidationErrorInfo() {
    }
    Object.defineProperty(ValidationErrorInfo.prototype, "MessageType", {
        get: function () { return this.messageType; },
        set: function (value) { this.messageType = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ValidationErrorInfo.prototype, "ErrorCode", {
        get: function () { return this.errorCode; },
        set: function (value) { this.errorCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ValidationErrorInfo.prototype, "ErrorMessage", {
        get: function () { return this.errorMessage; },
        set: function (value) { this.errorMessage = value; },
        enumerable: true,
        configurable: true
    });
    ValidationErrorInfo.prototype.ToString = function () {
        return this.ErrorMessage;
    };
    return ValidationErrorInfo;
}());
exports.ValidationErrorInfo = ValidationErrorInfo;
//# sourceMappingURL=MasterActionConfirmationComponent.js.map