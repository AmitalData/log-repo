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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var WarehouseEntryPM_1 = require("../../../Warehouse/EntityPMs/WarehouseEntryPM");
var Tools_1 = require("../../../Infrastructure/Tools");
var AddressListService_1 = require("../../../Common/Services/StandardLists/AddressListService");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var Args_1 = require("../../../Infrastructure/Args");
var AddressPM_1 = require("../../../Common/EntityPMs/AddressPM");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var WarehouseHelper_1 = require("../../Helpers/WarehouseHelper");
var NewShipmentComponent_1 = require("../../../Shipment/Components/NewEntity/NewShipmentComponent");
var PortListService_1 = require("../../../Common/Services/StandardLists/PortListService");
var NewFullWarehouseEntryComponent = /** @class */ (function (_super) {
    __extends(NewFullWarehouseEntryComponent, _super);
    function NewFullWarehouseEntryComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DirectionsList = [];
        _this.TransportModesList = [];
        _this.ShipmentTypesList = [];
        _this.ShipmentLevelsList = [];
        _this.IsLCLEntity = false;
        _this.ShipmentLevelCode = "";
        _this.ShowShipmentLevels = true;
        _this.ControlColumnWidth = 220;
        _this.ObjectTableName = "WarehouseEntry";
        _this.CardDependencyProperty1 = "CS";
        _this.CardDependencyProperty1IsList = false;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 117;
        _this.IsConsigneeMyCustomer = false;
        _this.CustomerHelpText = "Indicates who the customer is, so that Logitude knows to refer to the relevant partner for statistics, billing and shared logistics. For Export, the Shipper is selected automatically. For Import, the Consignee is selected automatically.";
        _this.WarehouseEntryPackagesLists = [];
        _this.warehouseEntryPM = new WarehouseEntryPM_1.WarehouseEntryPM();
        _this.IsFromShipment = false;
        _this.IsLoadPage = false;
        _this.warehouseHelper = new WarehouseHelper_1.WarehouseHelper();
        _this.IsInlandDomestic = false;
        _this.ScreenOpacity = 0.7;
        _this.IsScreenEnabled = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isAddShipperButtonDisabled = false;
        _this.isAddConsigneeButtonDisabled = false;
        _this.ShipmentTypeName = null;
        _this.isTransportModesListEnabled = false;
        _this.customerAddressId = null;
        _this.customerContactId = null;
        _this.isShipperMyCustomer = true;
        _this.FromPortList = null;
        _this.ToPortList = null;
        _this.SessionIndex = SessionLocator_1.SessionLocator.Index;
        _this.warehouseEntryPM = _this.warehouseHelper.GetNewWarehouseEntry(_this);
        _this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        var table = window.ObjectTables.filter(function (d) { return d.Name == "WarehouseEntry"; })[0];
        if (table)
            _this.ObjectTableId = table.Id;
        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            _this.CardDependencyProperty1 = "CS,AG";
            _this.CardDependencyProperty1IsList = true;
        }
        _this.InitializeServices();
        return _this;
    }
    NewFullWarehouseEntryComponent.prototype.ngOnInit = function () { };
    NewFullWarehouseEntryComponent.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myAddressListService = new AddressListService_1.AddressListService();
        this.myPortListService = new PortListService_1.PortListService();
    };
    NewFullWarehouseEntryComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntry").subscribe(function (response) {
            _this.Start(args);
        });
    };
    NewFullWarehouseEntryComponent.prototype.Start = function (args) {
        if (args) {
            this.ShowShipmentLevels = false;
            this.WarehouseEntryPackagesLists = args.WarehouseEntryPackagesLists;
            if (!this.WarehouseEntryPackagesLists)
                this.WarehouseEntryPackagesLists = [];
            this.SetValue(args);
            this.BuildFiltersLists();
            this.OnFiltersChanged();
            this.IsLoadPage = true;
        }
    };
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "IsAddShipperButtonDisabled", {
        get: function () {
            if (!this.IsScreenEnabled || (this.IsShipperMyCustomer && !this.IsInlandDomestic)) {
                this.isAddShipperButtonDisabled = true;
            }
            else
                this.isAddShipperButtonDisabled = false;
            return this.isAddShipperButtonDisabled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "IsAddConsigneeButtonDisabled", {
        get: function () {
            if (!this.IsScreenEnabled || (!this.IsShipperMyCustomer && !this.IsInlandDomestic)) {
                this.isAddConsigneeButtonDisabled = true;
            }
            else
                this.isAddConsigneeButtonDisabled = false;
            return this.isAddConsigneeButtonDisabled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "DirectionId", {
        get: function () { return this.warehouseEntryPM.DirectionId; },
        set: function (newValue) {
            if (this.warehouseEntryPM.DirectionId != newValue) {
                this.warehouseEntryPM.DirectionId = newValue;
                this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
                this.OnFiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "TransportModeId", {
        get: function () { return this.warehouseEntryPM.TransportModeId; },
        set: function (newValue) {
            if (this.warehouseEntryPM.TransportModeId != newValue) {
                this.warehouseEntryPM.TransportModeId = newValue;
                this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
                this.FromPortId = null;
                this.ToPortId = null;
                this.OnFiltersChanged();
                this.BuildShipmentTypes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ShipmentTypeId", {
        get: function () { return this.warehouseEntryPM.ShipmentTypeId; },
        set: function (newValue) {
            if (this.warehouseEntryPM.ShipmentTypeId != newValue) {
                this.warehouseEntryPM.ShipmentTypeId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipmentTypeName = null;
                }
                else {
                    var item = this.ShipmentTypesList.filter(function (f) { return f.Code == newValue; })[0];
                    if (item) {
                        this.ShipmentTypeName = item.Name;
                    }
                }
                this.OnFiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "IsTransportModesListEnabled", {
        get: function () { return this.isTransportModesListEnabled; },
        set: function (value) {
            if (this.isTransportModesListEnabled != value) {
                this.isTransportModesListEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ShipperId", {
        get: function () {
            return this.warehouseEntryPM.ShipperId;
        },
        set: function (newValue) {
            var _this = this;
            if (this.warehouseEntryPM.ShipperId != newValue) {
                this.warehouseEntryPM.ShipperId = newValue;
                this.warehouseEntryPM.FromPartnerId = newValue;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipperName = null;
                    this.FromAddressId = null;
                    this.ShipperPartnerTypeId = null;
                    if (this.IsInlandDomestic && this.IsShipperMyCustomer) {
                        this.CustomerId = null;
                    }
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            _this.FromAddressId = myCardList.MainAddressId;
                            _this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                            _this.ShipperName = myCardList.EnglishName;
                            if (myCardList) {
                                // if (this.IsInlandDomestic) {
                                _this.FullShipperConsignee();
                                // }
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ShipperName", {
        get: function () { return this.warehouseEntryPM.ShipperName; },
        set: function (newValue) {
            if (this.warehouseEntryPM.ShipperName != newValue) {
                this.warehouseEntryPM.ShipperName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "FromAddressId", {
        get: function () { return this.warehouseEntryPM.FromAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.warehouseEntryPM.FromAddressId != newValue) {
                this.warehouseEntryPM.FromAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipperAddressList = null;
                }
                else {
                    this.myAddressListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ShipperAddressList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ShipperAddressList", {
        get: function () { return this.myShipperAddressList; },
        set: function (newValue) {
            this.myShipperAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ConsigneeId", {
        get: function () { return this.warehouseEntryPM.ConsigneeId; },
        set: function (newValue) {
            var _this = this;
            if (this.warehouseEntryPM.ConsigneeId != newValue) {
                this.warehouseEntryPM.ConsigneeId = newValue;
                this.warehouseEntryPM.ToPartnerId = newValue;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ToAddressId = null;
                    this.ConsigneePartnerTypeId = null;
                    this.ConsigneeName = null;
                    if (this.IsInlandDomestic && !this.IsShipperMyCustomer) {
                        this.CustomerId = null;
                    }
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.ToAddressId = myCardList.MainAddressId;
                                _this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                                _this.ConsigneeName = myCardList.EnglishName;
                                //  if (this.IsInlandDomestic) {
                                _this.FullShipperConsignee();
                                // }
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ConsigneeName", {
        get: function () { return this.warehouseEntryPM.ConsigneeName; },
        set: function (newValue) {
            if (this.warehouseEntryPM.ConsigneeName != newValue) {
                this.warehouseEntryPM.ConsigneeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ToAddressId", {
        get: function () { return this.warehouseEntryPM.ToAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.warehouseEntryPM.ToAddressId != newValue) {
                this.warehouseEntryPM.ToAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ConsigneeAddressList = null;
                }
                else {
                    this.myAddressListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ConsigneeAddressList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ConsigneeAddressList", {
        get: function () { return this.myConsigneeAddressList; },
        set: function (newValue) {
            this.myConsigneeAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "CustomerId", {
        get: function () { return this.warehouseEntryPM.CustomerId; },
        set: function (newValue) {
            var _this = this;
            if (this.warehouseEntryPM.CustomerId != newValue) {
                this.warehouseEntryPM.CustomerId = newValue;
                this.FullShipperConsignee();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.CustomerPartnerTypeId = null;
                    this.CustomerContactId = null;
                    this.warehouseEntryPM.CustomerName = null;
                    this.CustomerAddressId = null;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.CustomerPartnerTypeId = myCardList.PartnerTypeId;
                                _this.CustomerContactId = myCardList.PrimaryContactId;
                                _this.warehouseEntryPM.CustomerName = myCardList.EnglishName;
                                _this.CustomerAddressId = myCardList.MainAddressId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "CustomerAddressId", {
        get: function () { return this.customerAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.customerAddressId != newValue) {
                this.customerAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.CustomerAddressList = null;
                }
                else {
                    this.myAddressListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.CustomerAddressList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "CustomerAddressList", {
        get: function () { return this.myCustomerAddressList; },
        set: function (newValue) {
            this.myCustomerAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "CustomerContactId", {
        get: function () { return this.customerContactId; },
        set: function (newValue) {
            if (this.customerContactId != newValue) {
                this.customerContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "CustomerRef1", {
        get: function () { return this.warehouseEntryPM.CustomerRef1; },
        set: function (newValue) {
            if (this.warehouseEntryPM.CustomerRef1 != newValue) {
                this.warehouseEntryPM.CustomerRef1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "CustomerRef2", {
        get: function () { return this.warehouseEntryPM.CustomerRef2; },
        set: function (newValue) {
            if (this.warehouseEntryPM.CustomerRef2 != newValue) {
                this.warehouseEntryPM.CustomerRef2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "IsShipperMyCustomer", {
        get: function () { return this.isShipperMyCustomer; },
        set: function (newValue) {
            if (this.isShipperMyCustomer != newValue) {
                this.isShipperMyCustomer = newValue;
                this.FullShipperConsignee(true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "TruckerReference", {
        get: function () { return this.warehouseEntryPM.TruckerReference; },
        set: function (newValue) {
            if (this.warehouseEntryPM.TruckerReference != newValue) {
                this.warehouseEntryPM.TruckerReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "TruckerId", {
        get: function () { return this.warehouseEntryPM.TruckerId; },
        set: function (newValue) {
            if (this.warehouseEntryPM.TruckerId != newValue) {
                this.warehouseEntryPM.TruckerId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "Manufacturer", {
        get: function () { return this.warehouseEntryPM.Manufacturer; },
        set: function (newValue) {
            if (this.warehouseEntryPM.Manufacturer != newValue) {
                this.warehouseEntryPM.Manufacturer = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "FromPortId", {
        get: function () { return this.warehouseEntryPM.FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.warehouseEntryPM.FromPortId != value) {
                this.warehouseEntryPM.FromPortId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.FromPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ToPortId", {
        get: function () { return this.warehouseEntryPM.ToPortId; },
        set: function (value) {
            var _this = this;
            if (this.warehouseEntryPM.ToPortId != value) {
                this.warehouseEntryPM.ToPortId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ToPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ReceivedBy", {
        get: function () { return this.warehouseEntryPM.ReceivedBy; },
        set: function (newValue) {
            if (this.warehouseEntryPM.ReceivedBy != newValue) {
                this.warehouseEntryPM.ReceivedBy = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "MasterNumber", {
        get: function () { return this.warehouseEntryPM.MasterNumber; },
        set: function (newValue) {
            if (this.warehouseEntryPM.MasterNumber != newValue) {
                this.warehouseEntryPM.MasterNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "HouseNumber", {
        get: function () { return this.warehouseEntryPM.HouseNumber; },
        set: function (newValue) {
            if (this.warehouseEntryPM.HouseNumber != newValue) {
                this.warehouseEntryPM.HouseNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "Notes", {
        get: function () { return this.warehouseEntryPM.Notes; },
        set: function (newValue) {
            if (this.warehouseEntryPM.Notes != newValue) {
                this.warehouseEntryPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "WarehouseId", {
        get: function () { return this.warehouseEntryPM.WarehouseId; },
        set: function (newValue) {
            if (this.warehouseEntryPM.WarehouseId != newValue) {
                this.warehouseEntryPM.WarehouseId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "EntryReference", {
        get: function () { return this.warehouseEntryPM.EntryReference; },
        set: function (newValue) {
            if (this.warehouseEntryPM.EntryReference != newValue) {
                this.warehouseEntryPM.EntryReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ActualEntryDate", {
        get: function () { return this.warehouseEntryPM.ActualEntryDate; },
        set: function (newValue) {
            if (this.warehouseEntryPM.ActualEntryDate != newValue) {
                this.warehouseEntryPM.ActualEntryDate = newValue;
                this.OnActualEntryDateDatePickerChange(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "ExpectedEntryDate", {
        get: function () { return this.warehouseEntryPM.ExpectedEntryDate; },
        set: function (newValue) {
            if (this.warehouseEntryPM.ExpectedEntryDate != newValue) {
                this.warehouseEntryPM.ExpectedEntryDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "SpecialInstruction", {
        get: function () { return this.warehouseEntryPM.SpecialInstruction; },
        set: function (newValue) {
            if (this.warehouseEntryPM.SpecialInstruction != newValue) {
                this.warehouseEntryPM.SpecialInstruction = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "TotalVolume", {
        get: function () { return this.warehouseEntryPM.TotalVolume; },
        set: function (newValue) {
            if (this.warehouseEntryPM.TotalVolume != newValue) {
                this.warehouseEntryPM.TotalVolume = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "TotalGrossWeight", {
        get: function () { return this.warehouseEntryPM.TotalGrossWeight; },
        set: function (newValue) {
            if (this.warehouseEntryPM.TotalGrossWeight != newValue) {
                this.warehouseEntryPM.TotalGrossWeight = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "TotalPieces", {
        get: function () { return this.warehouseEntryPM.TotalPieces; },
        set: function (newValue) {
            if (this.warehouseEntryPM.TotalPieces != newValue) {
                this.warehouseEntryPM.TotalPieces = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFullWarehouseEntryComponent.prototype, "QuantityLabel", {
        get: function () {
            var quantityLabel = "";
            if (this.IsLCLEntity) {
                quantityLabel = "Number of Packages";
            }
            else
                quantityLabel = "Number of Containers";
            return quantityLabel;
        },
        enumerable: true,
        configurable: true
    });
    NewFullWarehouseEntryComponent.prototype.BuildFiltersLists = function () {
        this.DirectionsList = [];
        this.TransportModesList = [];
        var direct_E = new NewShipmentComponent_1.FilterClass("E", "Export");
        var direct_I = new NewShipmentComponent_1.FilterClass("I", "Import");
        var direct_D = new NewShipmentComponent_1.FilterClass("D", "Domestic");
        var direct_R = new NewShipmentComponent_1.FilterClass("R", "Drop");
        var transport_A = new NewShipmentComponent_1.FilterClass("A", "Air");
        var transport_O = new NewShipmentComponent_1.FilterClass("O", "Ocean");
        var transport_I = new NewShipmentComponent_1.FilterClass("I", "Inland");
        var isAirExportOnly = false;
        this.DirectionsList.push(direct_E);
        this.DirectionsList.push(direct_I);
        this.DirectionsList.push(direct_D);
        this.DirectionsList.push(direct_R);
        this.TransportModesList.push(transport_A);
        this.TransportModesList.push(transport_O);
        this.TransportModesList.push(transport_I);
        this.BuildShipmentTypes();
        this.BuildShipmentLevels();
    };
    NewFullWarehouseEntryComponent.prototype.BuildShipmentTypes = function () {
        this.ShipmentTypesList = [];
        if (this.DirectionId && this.TransportModeId) {
            switch (this.TransportModeId) {
                case "O": {
                    this.ShipmentTypesList.push(new NewShipmentComponent_1.FilterClass("FCLD", "FCL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new NewShipmentComponent_1.FilterClass("LCLD", "LCL", "./Images/CellIcons/Package.png"));
                    break;
                }
                case "I": {
                    this.ShipmentTypesList.push(new NewShipmentComponent_1.FilterClass("FTL", "FTL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new NewShipmentComponent_1.FilterClass("LTL", "LTL", "./Images/CellIcons/Package.png"));
                    break;
                }
            }
        }
    };
    NewFullWarehouseEntryComponent.prototype.BuildShipmentLevels = function () {
        this.ShipmentLevelsList = [];
        if (this.ShowShipmentLevels) {
            this.ShipmentLevelsList.push(new NewShipmentComponent_1.FilterClass("D", "Direct"));
            this.ShipmentLevelsList.push(new NewShipmentComponent_1.FilterClass("H", "House"));
        }
    };
    NewFullWarehouseEntryComponent.prototype.FullShipperConsignee = function (emptyShipmentConsignee) {
        //if (!this.IsInlandDomestic) {
        //    if (emptyShipmentConsignee) {
        //        this.ShipperId = null;
        //        this.ConsigneeId = null;
        //    }
        if (emptyShipmentConsignee === void 0) { emptyShipmentConsignee = false; }
        //    if (this.isShipperMyCustomer) {
        //        this.ShipperId = this.CustomerId;
        //    } else this.ConsigneeId = this.CustomerId;
        //}
        // else {
        if (this.isShipperMyCustomer) {
            this.CustomerId = this.ShipperId;
        }
        else
            this.CustomerId = this.ConsigneeId;
        //}
    };
    NewFullWarehouseEntryComponent.prototype.OnFiltersChanged = function () {
        this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
        this.IsTransportModesListEnabled = Tools_1.AppTool.IsNullOrEmpty(this.DirectionId) ? false : true;
        this.SetScreenEnabled();
        this.SetPartners();
        this.SetUIProperties();
        this.SetLabels();
    };
    NewFullWarehouseEntryComponent.prototype.SetPartners = function () {
        switch (this.DirectionId) {
            case "I": {
                this.IsShipperMyCustomer = false;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
                    this.ConsigneeId = this.CustomerId;
                    if (this.ShipperId == this.CustomerId) {
                        this.ShipperId = null;
                    }
                }
                break;
            }
            default: {
                this.IsShipperMyCustomer = true;
                // this.warehouseEntryPM.ShipmentCustomerTypeCode = "SHI";
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
                    this.ShipperId = this.CustomerId;
                    if (this.ConsigneeId == this.CustomerId) {
                        this.ConsigneeId = null;
                    }
                }
                break;
            }
        }
    };
    NewFullWarehouseEntryComponent.prototype.SetLabels = function () {
        this.CarrierDependencyProperty1 = "TR";
        this.FromPortText = "Origin";
        this.ToPortText = "Destination";
        switch (this.TransportModeId) {
            case "A": {
                //this.FromPortText = "Gateway";
                // this.ToPortText = "Destination";
                this.CarrierTextCode = "Shipment.S.NewShipment.Airline";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.FlightNo";
                //  this.CarrierDependencyProperty1 = "AL";
                this.MasterTextCode = "Shipment.S.NewShipment.MAWB";
                this.HouseTextCode = "Shipment.S.NewShipment.HAWB";
                break;
            }
            case "O": {
                // this.FromPortText = "Loading Port";
                // this.ToPortText = "Discharge Port";
                this.CarrierTextCode = "Shipment.S.NewShipment.Shippingline";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.VoyageNo";
                //  this.CarrierDependencyProperty1 = "SL";
                this.MasterTextCode = "Shipment.S.NewShipment.OBL";
                this.HouseTextCode = "Shipment.S.NewShipment.FBL";
                break;
            }
            case "I": {
                // this.FromPortText = "From";
                // this.ToPortText = "To";
                this.CarrierTextCode = "Shipment.S.NewShipment.Trucker";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.TruckerNo";
                // this.CarrierDependencyProperty1 = "TR";
                this.MasterTextCode = "Shipment.S.NewShipment.CMR/RWB#";
                this.HouseTextCode = "Shipment.F.House";
                break;
            }
            default: {
                // this.FromPortText = "From";
                // this.ToPortText = "To";
                this.CarrierTextCode = "Shipment.S.NewShipment.Carrier";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.No";
                this.MasterTextCode = "Shipment.S.NewShipment.MAWB";
                this.HouseTextCode = "Shipment.S.NewShipment.HAWB";
                break;
            }
        }
    };
    NewFullWarehouseEntryComponent.prototype.SetValue = function (args) {
        var _this = this;
        if (this.warehouseEntryPM) {
            var myCommonDomain = new CommonDomainService_1.CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var warehouseId = myResponse.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(warehouseId)) {
                        _this.WarehouseId = warehouseId;
                    }
                }
            });
        }
    };
    NewFullWarehouseEntryComponent.prototype.ValidatePorts = function () {
        if (!this.IsInlandDomestic) {
            if (this.warehouseEntryPM.DirectionId == "D") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.FromPortId) && !Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ToPortId)) {
                    var fromCountryId = null;
                    var fromCountryIsEC = null;
                    var toCountryId = null;
                    var toCountryIsEC = null;
                    if (this.FromPortList != null) {
                        fromCountryId = this.FromPortList.CountryId;
                        fromCountryIsEC = this.FromPortList.CountryEC;
                    }
                    if (this.ToPortList != null) {
                        toCountryId = this.ToPortList.CountryId;
                        toCountryIsEC = this.ToPortList.CountryEC;
                    }
                    if (fromCountryId != toCountryId) {
                        if (fromCountryIsEC == false || toCountryIsEC == false) {
                            this.ValidationErrorsList.push("Both Ports must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }
    };
    NewFullWarehouseEntryComponent.prototype.ValidateInlandDomestic = function () {
        if (this.IsInlandDomestic) {
            if (this.warehouseEntryPM.ShipmentLevelCode != "C") {
                var fromCountryId = null;
                var fromCountryIsEC = null;
                var toCountryId = null;
                var toCountryIsEC = null;
                if (this.ShipperAddressList != null) {
                    fromCountryId = this.ShipperAddressList.CountryId;
                    fromCountryIsEC = this.ShipperAddressList.CountryEC;
                }
                if (this.ConsigneeAddressList != null) {
                    toCountryId = this.ConsigneeAddressList.CountryId;
                    toCountryIsEC = this.ConsigneeAddressList.CountryEC;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId) && !Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId)) {
                    if (fromCountryId != toCountryId) {
                        if (fromCountryIsEC == false || toCountryIsEC == false) {
                            this.ValidationErrorsList.push("Both Addresses must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }
    };
    NewFullWarehouseEntryComponent.prototype.SetActualDateClicked = function (fieldName) {
        this.ActualEntryDate = Tools_1.DateTool.GetDateParts(this.warehouseEntryPM.ExpectedEntryDate).DateObject;
    };
    NewFullWarehouseEntryComponent.prototype.SetCustomer = function (myCode) {
        if (myCode == 'S') {
            this.IsShipperMyCustomer = true;
        }
        else {
            this.IsShipperMyCustomer = false;
        }
    };
    NewFullWarehouseEntryComponent.prototype.SetScreenEnabled = function () {
        var isScreenEnabled = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DirectionId) && !Tools_1.AppTool.IsNullOrEmpty(this.TransportModeId)) {
            if (this.TransportModeId == "A") {
                isScreenEnabled = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentTypeId)) {
                isScreenEnabled = true;
            }
        }
        this.IsScreenEnabled = isScreenEnabled;
        this.ScreenOpacity = isScreenEnabled ? 1 : 0.7;
        // Customer
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("CustomerReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("CustomerReference2", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TruckerReference", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TruckerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ReceivedBy", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MasterNumber", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("HouseNumber", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("WarehouseId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("EntryReference", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ActualEntryDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ExpectedEntryDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("SpecialInstruction", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalPieces", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalGrossWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalVolume", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Manufacturer", this.ObjectTableName, isScreenEnabled);
    };
    NewFullWarehouseEntryComponent.prototype.FillMorePackagesDetails = function () {
        var logeWindow = new LogitudeWindow_1.LogitudeWindow();
        logeWindow.Width = 960;
        logeWindow.Height = 500;
        logeWindow.Title = "Packages Details";
        logeWindow.WindowArgs = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this, ShowPackageSummary: true, IsFromFullWarehouseEntryComponent: true };
        logeWindow.Show("./Warehouse/Components/WarehouseEntryPackagesDetailsComponent");
    };
    NewFullWarehouseEntryComponent.prototype.SetPackagesDetailsEnable = function () {
        var isEnablePackageArea = true;
        if (this.warehouseEntryPM.WarehouseEntryPackages && this.warehouseEntryPM.WarehouseEntryPackages.length > 0) {
            isEnablePackageArea = false;
        }
        this.UIProperties.SetEnabled("TotalPieces", this.ObjectTableName, isEnablePackageArea);
        this.UIProperties.SetEnabled("TotalGrossWeight", this.ObjectTableName, isEnablePackageArea);
        this.UIProperties.SetEnabled("TotalVolume", this.ObjectTableName, isEnablePackageArea);
    };
    NewFullWarehouseEntryComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Port();
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
    };
    NewFullWarehouseEntryComponent.prototype.SetUIProperties_Port = function () {
        var isFromRequired = false;
        var isToRequired = false;
        if (!this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.FromPortId)) {
                isFromRequired = true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ToPortId)) {
                isToRequired = true;
            }
        }
        this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isToRequired);
    };
    NewFullWarehouseEntryComponent.prototype.SetUIProperties_Shipper = function () {
        var isFieldRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
            if (this.IsShipperMyCustomer) {
                isFieldRequired = true;
            }
            else if (this.DirectionId == "E" || this.DirectionId == "D" || this.DirectionId == "R" || this.DirectionId == null || this.IsInlandDomestic) {
                isFieldRequired = true;
            }
        }
        this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, isFieldRequired);
    };
    NewFullWarehouseEntryComponent.prototype.SetUIProperties_Consignee = function () {
        var isFieldRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            if (!this.IsShipperMyCustomer) {
                isFieldRequired = true;
            }
            else if (this.DirectionId == "I") {
                isFieldRequired = true;
            }
            else if (this.IsInlandDomestic) {
                isFieldRequired = true;
            }
        }
        this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, isFieldRequired);
    };
    NewFullWarehouseEntryComponent.prototype.OnActualEntryDateDatePickerChange = function (value) {
        this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", true, null);
        if (!Tools_1.DateTool.IsActualDateValid(value)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", "Actual Entry Date");
            this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", false, errorMessage);
        }
    };
    // Add|Edit Partner
    NewFullWarehouseEntryComponent.prototype.AddPartnerClicked = function (myPartnerCode) {
        var _this = this;
        var title = "";
        var isCustomer = false;
        if (myPartnerCode == "Shipper") {
            title = "New Shipper";
            isCustomer = this.IsShipperMyCustomer;
        }
        else if (myPartnerCode == "Consignee") {
            title = "New Consignee";
            isCustomer = !this.IsShipperMyCustomer;
        }
        else if (myPartnerCode == "Customer") {
            title = "New Customer";
            isCustomer = true;
        }
        var args = new Args_1.NewEntityArgs();
        if (!isCustomer) {
            args.Perspective = "ShippersAndConsignees";
        }
        var logeWindow = new LogitudeWindow_1.LogitudeWindow();
        logeWindow.Width = 960;
        logeWindow.Height = 600;
        logeWindow.Title = title;
        logeWindow.WindowArgs = args;
        logeWindow.Show("./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent");
        logeWindow.ComponentLoaded.subscribe(function (comp) {
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (myPartnerCode == "Shipper")
                        _this.ShipperId = comp.EntityPM.Id;
                    else if (myPartnerCode == "Consignee")
                        _this.ConsigneeId = comp.EntityPM.Id;
                    else if (myPartnerCode == "Customer")
                        _this.CustomerId = comp.EntityPM.Id;
                }
            });
        });
    };
    NewFullWarehouseEntryComponent.prototype.AddAddressClicked = function (myAddressCode) {
        var _this = this;
        var entityPM = null;
        var myPartnerTypeId = null;
        var isCustomer;
        switch (myAddressCode) {
            case "Shipper": {
                entityPM = new AddressPM_1.AddressPM();
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ShipperId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.IsShipperMyCustomer;
                break;
            }
            case "Consignee": {
                entityPM = new AddressPM_1.AddressPM();
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ConsigneeId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !this.IsShipperMyCustomer;
                break;
            }
            case "Customer": {
                entityPM = new AddressPM_1.AddressPM();
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.CustomerId;
                myPartnerTypeId = this.CustomerPartnerTypeId;
                break;
            }
        }
        if (entityPM != null) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "Shipper": {
                            _this.FromAddressId = null;
                            _this.FromAddressId = entityPM.Id;
                            break;
                        }
                        case "Consignee": {
                            _this.ToAddressId = null;
                            _this.ToAddressId = entityPM.Id;
                            break;
                        }
                        case "Customer": {
                            _this.CustomerAddressId = null;
                            _this.CustomerAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    };
    NewFullWarehouseEntryComponent.prototype.EditAddressClicked = function (myAddressCode) {
        var _this = this;
        var myAddressId = null;
        var myPartnerTypeId = null;
        var isCustomer;
        switch (myAddressCode) {
            case "Shipper": {
                myAddressId = this.FromAddressId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.isShipperMyCustomer;
                break;
            }
            case "Consignee": {
                myAddressId = this.ToAddressId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !this.isShipperMyCustomer;
                break;
            }
            case "Customer": {
                myAddressId = this.CustomerAddressId;
                myPartnerTypeId = this.CustomerPartnerTypeId;
                isCustomer = this.isShipperMyCustomer;
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "Shipper": {
                            _this.FromAddressId = null;
                            _this.FromAddressId = myAddressId;
                            break;
                        }
                        case "Consignee": {
                            _this.ToAddressId = null;
                            _this.ToAddressId = myAddressId;
                            break;
                        }
                        case "Customer": {
                            _this.CustomerAddressId = null;
                            _this.CustomerAddressId = myAddressId;
                            break;
                        }
                    }
                }
            });
        }
    };
    NewFullWarehouseEntryComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("WarehouseEntry", this.warehouseEntryPM);
        if (errorsArray.length > 0) {
            errorsArray.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
        if (this.IsInlandDomestic) {
            this.warehouseEntryPM.FromPortId = null;
            this.warehouseEntryPM.ToPortId = null;
        }
        else {
            this.warehouseEntryPM.FromAddressId = null;
            this.warehouseEntryPM.ToAddressId = null;
            this.warehouseEntryPM.FromPartnerId = null;
            this.warehouseEntryPM.ToPartnerId = null;
        }
        if (!this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.FromPortId)) {
                this.ValidationErrorsList.push(this.FromPortText + " field is required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ToPortId)) {
                this.ValidationErrorsList.push(this.ToPortText + " field is required");
            }
            if (this.IsShipperMyCustomer) {
                this.warehouseEntryPM.ShipperReference1 = this.CustomerRef1;
                this.warehouseEntryPM.ShipperReference2 = this.CustomerRef2;
            }
            else {
                this.warehouseEntryPM.ConsigneeReference1 = this.CustomerRef1;
                this.warehouseEntryPM.ConsigneeReference2 = this.CustomerRef2;
            }
        }
        this.ValidatePartners();
        this.ValidationErrorsList = this.ValidationErrorsList.filter(function (d) { return d != "Customer field is required"; });
        if (this.ValidationErrorsList.length == 0) {
            this.ValidateInlandDomestic();
            this.ValidatePorts();
        }
        if (this.ValidationErrorsList.length == 0) {
            if (!this.warehouseEntryPM.WarehouseEntryPackages)
                this.warehouseEntryPM.WarehouseEntryPackages = [];
            this.warehouseHelper.CreateWarehouseEntry(this.warehouseEntryPM, this);
        }
    };
    NewFullWarehouseEntryComponent.prototype.ValidatePartners = function () {
        if (this.warehouseEntryPM.ShipmentLevelCode != "C") {
            if ((this.warehouseEntryPM.DirectionId.toUpperCase() == "E" || this.warehouseEntryPM.DirectionId.toUpperCase() == "R") && Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId)) {
                this.ValidationErrorsList.push("Shipper" + " field is required");
            }
            else if (this.warehouseEntryPM.DirectionId.toUpperCase() == "I" && Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId)) {
                this.ValidationErrorsList.push("Consignee" + " field is required");
            }
            else if (this.warehouseEntryPM.DirectionId.toUpperCase() == "D" && (Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId) || Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId))) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId)) {
                    this.ValidationErrorsList.push("Shipper" + " field is required");
                }
                if (this.IsInlandDomestic) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId)) {
                        this.ValidationErrorsList.push("Consignee" + " field is required");
                    }
                }
            }
        }
    };
    NewFullWarehouseEntryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewFullWarehouseEntryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'NewFullWarehouseEntryComponent',
            templateUrl: './NewFullWarehouseEntryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewFullWarehouseEntryComponent);
    return NewFullWarehouseEntryComponent;
}(BaseComponent_1.BaseComponent));
exports.NewFullWarehouseEntryComponent = NewFullWarehouseEntryComponent;
//# sourceMappingURL=NewFullWarehouseEntryComponent.js.map