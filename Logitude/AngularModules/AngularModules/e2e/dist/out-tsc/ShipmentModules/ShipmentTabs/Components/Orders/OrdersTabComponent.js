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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ShipmentOrderPackagePM_1 = require("../../../../Shipment/EntityPMs/ShipmentOrderPackagePM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Shipment/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AirlineListService_1 = require("../../../../Common/Services/StandardLists/AirlineListService");
var VesselListService_1 = require("../../../../Common/Services/StandardLists/VesselListService");
var PackageTypeListService_1 = require("../../../../Common/Services/StandardLists/PackageTypeListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var OrdersTabComponent = /** @class */ (function (_super) {
    __extends(OrdersTabComponent, _super);
    function OrdersTabComponent(entityArgs, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.ObjectTableName = null;
        _this.TransportModeId = null;
        _this.IsInlandDomestic = false;
        _this.ItemsSource = [];
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsEditingEnabled = false;
        _this.IsAMSClosingDateVisible = false;
        _this.IsWarehouseFields_PickupsVisible = false;
        _this.IsWarehouseFields_DeliveriesVisible = false;
        _this.DimensionsDependencyProperty1 = null;
        _this.DimensionsDependencyProperty1IsList = false;
        // Measurments
        _this.MeasurmentsButtonToolTip = null;
        _this.IsMeasurmentsHidden = true;
        // Tips
        _this.IsTipsOpened = false;
        _this.IsFirstTipLoad = true;
        // Booking Confirmation
        _this.CarrierTextCode = null;
        _this.CarrierNumberTextCode = null;
        _this.CarrierDependencyFilter1Value = null;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.TransportModeId = _this.EntityPM.TransportModeId;
        _this.InitServices();
        _this.Listen();
        return _this;
    }
    OrdersTabComponent.prototype.InitServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myAirlineListService = new AirlineListService_1.AirlineListService();
        this.myVesselListService = new VesselListService_1.VesselListService();
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
    };
    OrdersTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                    _this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                    _this.SetLabels();
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHOR" || tabCode == "JHOR") {
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.BuildShipmentPickup();
                }
            });
        }
    };
    OrdersTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
    };
    OrdersTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsInlandDomestic = Tools_2.ShipmentTool.IsInlandDomestic(this.EntityPM);
            this.entityResourceService.getEntityResourceByTableName("ShipmentOrderPackage").subscribe(function (res1) {
                _this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDelivery").subscribe(function (res2) {
                    _this.IsResourcesReady = true;
                    _this.SetLabels();
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.SetBookingConfirmation();
                    _this.BuildShipmentPickup();
                });
            });
        }
        //Tip
        var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName && (d.Tenant == SessionLocator_1.SessionLocator.Tenant || d.Tenant == 0); })[0];
        if (table) {
            var tip = window.Tips.filter(function (d) { return d.Code == "ORDT" && d.ObjectTableId == table.Id; })[0];
            if (tip) {
                var hasTip = true;
                var isVisible = tip.VisibilityDefaultValue;
                var tipVisibility = window.TipsVisibilities.filter(function (d) { return d.TipCode == tip.Code && d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0];
                if (tipVisibility)
                    isVisible = tipVisibility.IsVisible;
                if (!isVisible && hasTip)
                    this.IsTipsOpened = false;
                else
                    this.IsTipsOpened = true;
            }
        }
    };
    OrdersTabComponent.prototype.SetLabels = function () {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.TabHeaderLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Orders.Booking");
        }
        else {
            this.TabHeaderLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Orders.Order");
        }
        if (this.IsLCLEntity) {
            this.AddButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Order.AddOrderPackage");
        }
        else {
            this.AddButtonLabel = "Add Order Container";
        }
        this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");
        this.SetAttachedLabels();
    };
    OrdersTabComponent.prototype.SetAttachedLabels = function () {
        this.DimensionsColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.WeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.VolumeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.BookingVolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.BookingGrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ChargeableWeightUnitCode");
            this.BookingChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
        else {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.WtMsrUnitCode.Short");
            this.BookingChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.WtMsr.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    };
    OrdersTabComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isCarrierEnabled = false;
        var isConfirmationEnabled = false;
        var isAMSClosingDateVisible = false;
        var isWarehouseFields_PickupsVisible = false;
        var isWarehouseFields_DeliveriesVisible = false;
        if (isEditingEnabled) {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C") {
                isCarrierEnabled = true;
                isConfirmationEnabled = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                    isCarrierEnabled = false;
                }
                else {
                    if (this.TransportModeId == "A") {
                        var isTakenFromStock = (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) ? true : false;
                        if (isTakenFromStock || !Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                            isCarrierEnabled = false;
                        }
                    }
                }
            }
        }
        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (this.EntityPM.TransportModeId == "O" || this.EntityPM.TransportModeId == "I") {
                    if (this.IsInlandDomestic) {
                    }
                    else {
                        isWarehouseFields_PickupsVisible = true;
                    }
                }
            }
        }
        if (this.EntityPM.DirectionId == "I") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (this.EntityPM.TransportModeId == "O" || this.EntityPM.TransportModeId == "I") {
                    isWarehouseFields_DeliveriesVisible = true;
                }
            }
        }
        if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
            if (this.TransportModeId == "O" || this.TransportModeId == "I") {
                isAMSClosingDateVisible = true;
            }
        }
        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("OrderIsDangerouseGoods", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("EmptyPickupContainerPartnerId", "ShipmentPickUpDelivery", isEditingEnabled);
        this.UIProperties.SetEnabled("Notes", "ShipmentPickUpDelivery", isEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("BookingConfirmationNumber", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("BookingConfirmedBy", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("CutoffDate", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("MainCarriageVesselId", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("BookingConfirmationNotes", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("WarehouseLegCutOffDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegVGMCutOffDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("TerminalAvailable", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegLastFreeDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("AMSClosingDate", this.ObjectTableName, isEditingEnabled);
        this.IsEditingEnabled = isEditingEnabled;
        this.IsAMSClosingDateVisible = isAMSClosingDateVisible;
        this.IsWarehouseFields_PickupsVisible = isWarehouseFields_PickupsVisible;
        this.IsWarehouseFields_DeliveriesVisible = isWarehouseFields_DeliveriesVisible;
        this.SetUIProperties_Totals();
        this.SetUIProperties_DimFactor();
        this.SetUIProperties_DimensionsUnitCode();
    };
    OrdersTabComponent.prototype.SetUIProperties_Totals = function () {
        var isTotalsFieldEnabled = false;
        var isTotalsEditedFieldEnabled = false;
        if (this.IsEditingEnabled) {
            isTotalsFieldEnabled = true;
            isTotalsEditedFieldEnabled = true;
            if (this.EntityPM.ShipmentOrderPackages.length > 0) {
                isTotalsFieldEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
        this.UIProperties.SetEnabled("OrderChargeableWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
    };
    OrdersTabComponent.prototype.SetUIProperties_DimensionsUnitCode = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }
        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }
        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }
        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    };
    OrdersTabComponent.prototype.MeasurmentsSettingsClicked = function () {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;
        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.HideMeasurmentsSettings");
        }
        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");
        }
    };
    Object.defineProperty(OrdersTabComponent.prototype, "VolumeUnitCode", {
        get: function () { return this.EntityPM.VolumeUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.VolumeUnitCode != newValue) {
                this.EntityPM.VolumeUnitCode = newValue;
                this.EntityPM.DimensionsUnitCode = Tools_1.AppTool.GetDimentionsCodeFromVolumeCode(newValue);
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.SetUIProperties_DimensionsUnitCode();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "DimensionsUnitCode", {
        get: function () { return this.EntityPM.DimensionsUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.DimensionsUnitCode != newValue) {
                this.EntityPM.DimensionsUnitCode = newValue;
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeightUnitCode != newValue) {
                this.EntityPM.GrossWeightUnitCode = newValue;
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.EntityPM.ChargeableWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
                this.EntityPM.ChargeableWeightUnitCode = newValue;
                this.ComputeDimFactor();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "Ratio", {
        get: function () { return this.EntityPM.Ratio; },
        set: function (newValue) {
            if (this.EntityPM.Ratio != newValue) {
                this.EntityPM.Ratio = newValue;
                this.ComputeDimFactor();
                Tools_2.ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "DimFactor", {
        get: function () { return this.EntityPM.DimFactor; },
        set: function (newValue) {
            if (this.EntityPM.DimFactor != newValue) {
                this.EntityPM.DimFactor = newValue;
                this.EntityPM.Ratio = Tools_1.AppTool.GetRatioFromDimFactor(this.DimFactor, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
                Tools_2.ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    OrdersTabComponent.prototype.ComputeDimFactor = function () {
        this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    };
    OrdersTabComponent.prototype.OnMeasurmentsSettingsChanged = function () {
        this.SetAttachedLabels();
        Tools_2.ShipmentTool.RecalculateShipmentFields(this.EntityPM);
    };
    OrdersTabComponent.prototype.SetUIProperties_DimFactor = function () {
        var isDimFactorVisibile = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }
        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    };
    OrdersTabComponent.prototype.TipVisibilityChanged = function (event) {
        if (event == "true")
            this.IsTipsOpened = true;
        else
            this.IsTipsOpened = false;
        this.IsFirstTipLoad = false;
    };
    OrdersTabComponent.prototype.TipsButtonClicked = function () {
        this.IsTipsOpened = !this.IsTipsOpened;
    };
    // OrderBookingDetails    
    OrdersTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
            _this.ItemsSource.push(new ShipmentOrderPackageItem(item, _this, false));
        });
        this.SetUIProperties_Totals();
    };
    OrdersTabComponent.prototype.GrossWeightLostFocus = function (input) {
        if (this.EntityPM.ShipmentOrderPackages.length > 0) {
            var valueComputed = 0;
            var valueInserted = 0;
            this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.GrossWeight)) {
                    valueComputed += item.GrossWeight;
                }
            });
            if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
                input = Tools_1.AppTool.Replace(input, ",", "");
                valueInserted = Number(input);
            }
            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = valueInserted == 0 ? null : valueInserted;
            this.OrderGrossWeightEdited = !(valueComputed == valueInserted);
            this.OrderGrossWeight = valueInserted;
            this.ComputeTotals();
        }
    };
    OrdersTabComponent.prototype.ChargeableWeightLostFocus = function (input) {
        if (this.EntityPM.ShipmentOrderPackages.length > 0) {
            var valueComputed = 0;
            var valueInserted = 0;
            valueComputed = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
                input = Tools_1.AppTool.Replace(input, ",", "");
                valueInserted = Number(input);
            }
            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = valueInserted == 0 ? null : valueInserted;
            this.OrderChargeableWeightEdited = !(valueComputed == valueInserted);
            this.OrderChargeableWeight = Tools_1.AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            this.ComputeTotals();
        }
    };
    OrdersTabComponent.prototype.ResetGrossWeightEdited = function () {
        this.OrderGrossWeightEdited = false;
        this.ComputeTotals();
    };
    OrdersTabComponent.prototype.ResetChargeableWeightEdited = function () {
        this.OrderChargeableWeightEdited = false;
        this.ComputeTotals();
    };
    OrdersTabComponent.prototype.ResetTotalEditedValues = function () {
        this.OrderGrossWeightEdited = false;
        this.OrderChargeableWeightEdited = false;
    };
    OrdersTabComponent.prototype.ComputeTotals = function () {
        if (this.EntityPM.ShipmentOrderPackages.length == 0) {
            this.BookingNumberOfPackages = null;
            this.OrderGrossWeight = null;
            this.BookingVolume = null;
            this.OrderVolumetricWeight = null;
            this.OrderChargeableWeight = null;
            this.OrderGrossWeightEdited = false;
            this.OrderChargeableWeightEdited = false;
        }
        else {
            var myQuantity = 0;
            var myVolume = 0;
            var myGrossWeight = 0;
            var myVolumetricWeight = 0;
            this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
                if (item.Quantity != null) {
                    myQuantity += item.Quantity;
                }
                if (item.Volume != null) {
                    myVolume += item.Volume;
                }
                if (item.VolumetricWeight != null) {
                    myVolumetricWeight += item.VolumetricWeight;
                }
                if (item.GrossWeight != null) {
                    myGrossWeight += item.GrossWeight;
                }
            });
            this.BookingNumberOfPackages = myQuantity;
            this.BookingVolume = Tools_1.AppTool.Round(myVolume, 3);
            this.OrderVolumetricWeight = Tools_1.AppTool.Round(myVolumetricWeight, 3);
            if (!this.OrderGrossWeightEdited) {
                this.OrderGrossWeight = Tools_1.AppTool.Round(myGrossWeight, 3);
            }
            if (!this.OrderChargeableWeightEdited) {
                this.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }
        this.SetUIProperties_Totals();
    };
    Object.defineProperty(OrdersTabComponent.prototype, "OrderGrossWeightEdited", {
        get: function () { return this.EntityPM.OrderGrossWeightEdited; },
        set: function (value) {
            if (this.EntityPM.OrderGrossWeightEdited != value) {
                this.EntityPM.OrderGrossWeightEdited = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "OrderChargeableWeightEdited", {
        get: function () { return this.EntityPM.OrderChargeableWeightEdited; },
        set: function (value) {
            if (this.EntityPM.OrderChargeableWeightEdited != value) {
                this.EntityPM.OrderChargeableWeightEdited = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    OrdersTabComponent.prototype.AddPackage = function () {
        var itemPM = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        var itemComponent = new ShipmentOrderPackageItem(itemPM, this, true);
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Orders.AddOrderPackage");
        if (this.IsFCLEntity) {
            title = "Add Container";
        }
        this.RunPackageWindow(itemComponent, title);
    };
    OrdersTabComponent.prototype.EditPackage = function (itemComponent) {
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Orders.EditOrderPackage");
        if (this.IsFCLEntity) {
            title = "Edit Container";
        }
        this.RunPackageWindow(itemComponent, title);
    };
    OrdersTabComponent.prototype.DeletePackage = function (itemComponent) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisOrderPackage"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.RemoveOrderPackage(itemComponent.EntityPM);
                var index = _this.ItemsSource.indexOf(itemComponent);
                if (index > -1) {
                    _this.ItemsSource.splice(index, 1);
                }
                _this.ComputeTotals();
            }
        });
    };
    OrdersTabComponent.prototype.RunPackageWindow = function (itemComponent, windowTitle) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Orders/AddEditOrderPackageComponent');
    };
    OrdersTabComponent.prototype.SetBookingConfirmation = function () {
        switch (this.EntityPM.TransportModeId.toUpperCase()) {
            case "A":
                {
                    this.CarrierTextCode = "Shipment.O.Routings.Airline";
                    this.CarrierNumberTextCode = "Shipment.O.Routings.FlightNo";
                    this.CarrierDependencyFilter1Value = "AL";
                    break;
                }
            case "O":
                {
                    this.CarrierTextCode = "Shipment.O.Routings.Shippingline";
                    this.CarrierNumberTextCode = "Shipment.O.Routings.VoyageNo";
                    this.CarrierDependencyFilter1Value = "SL";
                    break;
                }
            default:
                {
                    this.CarrierTextCode = "Shipment.O.Routings.Trucker";
                    this.CarrierNumberTextCode = "Shipment.O.Routings.TruckNo";
                    this.CarrierDependencyFilter1Value = "TR";
                    break;
                }
        }
    };
    Object.defineProperty(OrdersTabComponent.prototype, "MainCarriageCarrierId", {
        // Main Carrier
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageCarrierId != value) {
                this.EntityPM.MainCarriageCarrierId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.MainCarriageCarrierPrefix = null;
                    this.MainCarriageCarrierNumber = null;
                    this.Master = null;
                    Tools_2.RoutingHelper.MainCarriageCarrierChanged(this.EntityPM, null);
                    if (this.TransportModeId == "A") {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
                            this.CarrierIsCheckDigit = false;
                            this.CarrierIsLimitedLength = false;
                            this.AirlinePrefix = null;
                        }
                        this.CarrierIsChampRegistered = false;
                        this.CarrierIsGLSHKRegistered = false;
                        Tools_2.ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
                    }
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                if (_this.TransportModeId == "A") {
                                    _this.MainCarriageCarrierPrefix = list.Code;
                                }
                                Tools_2.RoutingHelper.MainCarriageCarrierChanged(_this.EntityPM, list);
                                if (_this.TransportModeId == "A") {
                                    // dont get from chach: if user choosed from tenant0 it wont get it
                                    _this.myAirlineListService.getSingle(value).subscribe(function (myAirlineListResponse) {
                                        if (myAirlineListResponse != null) {
                                            var myAirlineList = myAirlineListResponse.Result;
                                            if (myAirlineList != null) {
                                                if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.InterlineId)) {
                                                    _this.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                                    _this.CarrierIsLimitedLength = myAirlineList.LimitedLength;
                                                    var myPrefix = null;
                                                    if (!Tools_1.AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                                        myPrefix = myAirlineList.Prefix.toString().trim();
                                                        myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, '0');
                                                    }
                                                    _this.AirlinePrefix = myPrefix;
                                                }
                                                _this.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                                _this.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;
                                                _this.myPartnersDomainService.GetAirlineByCode(myAirlineList.Code, 0).subscribe(function (myResponse) {
                                                    if (!myResponse.HasError) {
                                                        Tools_2.ShipmentTool.MapTenantZeroAirline(_this.EntityPM, myResponse.Result);
                                                    }
                                                });
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "MainCarriageCarrierCode", {
        get: function () { return this.EntityPM.MainCarriageCarrierCode; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierCode != value) {
                this.EntityPM.MainCarriageCarrierCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "MainCarriageCarrierName", {
        get: function () { return this.EntityPM.MainCarriageCarrierName; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierName != value) {
                this.EntityPM.MainCarriageCarrierName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "MainCarriageCarrierPrefix", {
        get: function () { return this.EntityPM.MainCarriageCarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierPrefix != value) {
                this.EntityPM.MainCarriageCarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "MainCarriageCarrierWebSite", {
        get: function () { return this.EntityPM.MainCarriageCarrierWebSite; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierWebSite != value) {
                this.EntityPM.MainCarriageCarrierWebSite = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.EntityPM.MainCarriageCarrierNumber; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierNumber != value) {
                this.EntityPM.MainCarriageCarrierNumber = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "CarrierIsCheckDigit", {
        get: function () { return this.EntityPM.CarrierIsCheckDigit; },
        set: function (value) {
            if (this.EntityPM.CarrierIsCheckDigit != value) {
                this.EntityPM.CarrierIsCheckDigit = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "CarrierIsLimitedLength", {
        get: function () { return this.EntityPM.CarrierIsLimitedLength; },
        set: function (value) {
            if (this.EntityPM.CarrierIsLimitedLength != value) {
                this.EntityPM.CarrierIsLimitedLength = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "CarrierIsChampRegistered", {
        get: function () { return this.EntityPM.CarrierIsChampRegistered; },
        set: function (value) {
            if (this.EntityPM.CarrierIsChampRegistered != value) {
                this.EntityPM.CarrierIsChampRegistered = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "CarrierIsGLSHKRegistered", {
        get: function () { return this.EntityPM.CarrierIsGLSHKRegistered; },
        set: function (value) {
            if (this.EntityPM.CarrierIsGLSHKRegistered != value) {
                this.EntityPM.CarrierIsGLSHKRegistered = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "AirlinePrefix", {
        get: function () { return this.EntityPM.AirlinePrefix; },
        set: function (value) {
            if (this.EntityPM.AirlinePrefix != value) {
                this.EntityPM.AirlinePrefix = value;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (value) {
            if (this.EntityPM.Master != value) {
                this.EntityPM.Master = value;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "LongMaster", {
        get: function () { return this.EntityPM.LongMaster; },
        set: function (newValue) {
            if (this.EntityPM.LongMaster != newValue) {
                this.EntityPM.LongMaster = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "BookingConfirmationNumber", {
        get: function () { return this.EntityPM.BookingConfirmationNumber; },
        set: function (newValue) {
            if (this.EntityPM.BookingConfirmationNumber != newValue) {
                this.EntityPM.BookingConfirmationNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "BookingConfirmedBy", {
        get: function () { return this.EntityPM.BookingConfirmedBy; },
        set: function (newValue) {
            if (this.EntityPM.BookingConfirmedBy != newValue) {
                this.EntityPM.BookingConfirmedBy = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "CutoffDate", {
        get: function () { return this.EntityPM.CutoffDate; },
        set: function (newValue) {
            if (this.EntityPM.CutoffDate != newValue) {
                this.EntityPM.CutoffDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "MainCarriageVesselId", {
        get: function () { return this.EntityPM.MainCarriageVesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageVesselId != value) {
                this.EntityPM.MainCarriageVesselId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.MainCarriageVesselName = null;
                }
                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.MainCarriageVesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "TerminalAvailable", {
        get: function () { return this.EntityPM.TerminalAvailable; },
        set: function (value) {
            if (this.EntityPM.TerminalAvailable != value) {
                this.EntityPM.TerminalAvailable = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "WarehouseLegLastFreeDate", {
        get: function () { return this.EntityPM.WarehouseLegLastFreeDate; },
        set: function (newValue) {
            if (this.EntityPM.WarehouseLegLastFreeDate != newValue) {
                this.EntityPM.WarehouseLegLastFreeDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "WarehouseLegCutOffDate", {
        get: function () { return this.EntityPM.WarehouseLegCutOffDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegCutOffDate != value) {
                this.EntityPM.WarehouseLegCutOffDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "WarehouseLegVGMCutOffDate", {
        get: function () { return this.EntityPM.WarehouseLegVGMCutOffDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegVGMCutOffDate != value) {
                this.EntityPM.WarehouseLegVGMCutOffDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "AMSClosingDate", {
        get: function () { return this.EntityPM.AMSClosingDate; },
        set: function (newValue) {
            if (this.EntityPM.AMSClosingDate != newValue) {
                this.EntityPM.AMSClosingDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "BookingConfirmationNotes", {
        get: function () { return this.EntityPM.BookingConfirmationNotes; },
        set: function (newValue) {
            if (this.EntityPM.BookingConfirmationNotes != newValue) {
                this.EntityPM.BookingConfirmationNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "BookingNumberOfPackages", {
        //Details
        get: function () { return this.EntityPM.BookingNumberOfPackages; },
        set: function (newValue) {
            if (this.EntityPM.BookingNumberOfPackages != newValue) {
                this.EntityPM.BookingNumberOfPackages = newValue;
                this.CurrentSession.SessionEvent.emit("UpdatePackagesTab");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "OrderGrossWeight", {
        get: function () { return this.EntityPM.OrderGrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderGrossWeight != newValue) {
                this.EntityPM.OrderGrossWeight = Tools_1.AppTool.Round(newValue, 3);
                if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                    this.EntityPM.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "BookingVolume", {
        get: function () { return this.EntityPM.BookingVolume; },
        set: function (newValue) {
            if (this.EntityPM.BookingVolume != newValue) {
                this.EntityPM.BookingVolume = Tools_1.AppTool.Round(newValue, 3);
                if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                    this.ComputeOrderVolumetricWeight();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "OrderVolumetricWeight", {
        get: function () { return this.EntityPM.OrderVolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderVolumetricWeight != newValue) {
                this.EntityPM.OrderVolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
                if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                    this.EntityPM.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "OrderChargeableWeight", {
        get: function () { return this.EntityPM.OrderChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderChargeableWeight != newValue) {
                var myResult = Tools_1.AppTool.Round(newValue, 3);
                this.EntityPM.OrderChargeableWeight = myResult;
                if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                    if (this.OrderGrossWeight == null && this.OrderVolumetricWeight == null) {
                        this.EntityPM.OrderVolumetricWeight = myResult;
                        this.EntityPM.OrderGrossWeight = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, myResult);
                        this.EntityPM.BookingVolume = Tools_1.AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, myResult, this.EntityPM.Ratio);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "OrderIsDangerouseGoods", {
        get: function () { return this.EntityPM.OrderIsDangerouseGoods; },
        set: function (newValue) {
            if (this.EntityPM.OrderIsDangerouseGoods != newValue) {
                this.EntityPM.OrderIsDangerouseGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    OrdersTabComponent.prototype.ComputeOrderVolumetricWeight = function () {
        var myResult = null;
        if (this.EntityPM.Ratio == null) {
            this.EntityPM.Ratio = Tools_1.AppTool.GetRatio(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        if (this.EntityPM.BookingVolume != null) {
            myResult = Tools_1.AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.BookingVolume, this.EntityPM.Ratio);
        }
        else if (this.OrderGrossWeight != null) {
            myResult = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.OrderGrossWeight);
        }
        this.EntityPM.OrderVolumetricWeight = myResult;
    };
    OrdersTabComponent.prototype.ComputeChargeableWeight = function () {
        this.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    };
    OrdersTabComponent.prototype.BuildShipmentPickup = function () {
        var fromPortCode = "";
        var fromPortName = "";
        var fromCountryCode = "";
        var fromCountryName = "";
        var toPortCode = "";
        var toPortName = "";
        var toCountryCode = "";
        var toCountryName = "";
        this.ShipmentPickup = this.EntityPM.ShipmentPickUps.sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; })[0];
        if (this.ShipmentPickup != null) {
            if (this.ShipmentPickup.FromPortId != null) {
                fromPortCode = this.ShipmentPickup.FromPortCode;
                fromPortName = this.ShipmentPickup.FromPortName;
                fromCountryCode = this.ShipmentPickup.FromPortCountryCode;
                fromCountryName = this.ShipmentPickup.FromPortCountryName;
            }
            else {
                fromPortCode = this.ShipmentPickup.FromAddressCountryCode;
                fromPortName = this.ShipmentPickup.FromAddressCountryName;
                fromCountryCode = this.ShipmentPickup.FromAddressCountryCode;
                fromCountryName = this.ShipmentPickup.FromAddressCountryName;
            }
            if (this.ShipmentPickup.ToPortId != null) {
                toPortCode = this.ShipmentPickup.ToPortCode;
                toPortName = this.ShipmentPickup.ToPortName;
                toCountryCode = this.ShipmentPickup.ToPortCountryCode;
                toCountryName = this.ShipmentPickup.ToPortCountryName;
            }
            else {
                toPortCode = this.ShipmentPickup.ToAddressCountryCode;
                toPortName = this.ShipmentPickup.ToAddressCountryName;
                toCountryCode = this.ShipmentPickup.ToAddressCountryCode;
                toCountryName = this.ShipmentPickup.ToAddressCountryName;
            }
        }
        this.PickupFromPortCode = fromPortCode;
        this.PickupFromPortName = fromPortName;
        this.PickupFromCountryCode = fromCountryCode;
        this.PickupFromCountryName = fromCountryName;
        this.PickupToPortCode = toPortCode;
        this.PickupToPortName = toPortName;
        this.PickupToCountryCode = toCountryCode;
        this.PickupToCountryName = toCountryName;
    };
    Object.defineProperty(OrdersTabComponent.prototype, "EmptyPickupContainerPartnerId", {
        get: function () {
            var myResult = null;
            if (this.ShipmentPickup != null) {
                myResult = this.ShipmentPickup.EmptyPickupContainerPartnerId;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.ShipmentPickup != null) {
                if (this.ShipmentPickup.EmptyPickupContainerPartnerId != newValue) {
                    this.ShipmentPickup.EmptyPickupContainerPartnerId = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "Notes", {
        get: function () {
            var myResult = null;
            if (this.ShipmentPickup != null) {
                myResult = this.ShipmentPickup.Notes;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.ShipmentPickup != null) {
                if (this.ShipmentPickup.Notes != newValue) {
                    this.ShipmentPickup.Notes = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    OrdersTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './OrdersTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], OrdersTabComponent);
    return OrdersTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OrdersTabComponent = OrdersTabComponent;
var ShipmentOrderPackageItem = /** @class */ (function (_super) {
    __extends(ShipmentOrderPackageItem, _super);
    function ShipmentOrderPackageItem(entity, fatherComponent, isNewEntity) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "ShipmentOrderPackage";
        _this.IsNewEntity = false;
        _this.IsEditingEnabled = false;
        _this.EntityPM = entity;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNewEntity;
        _this.SetUIProperties();
        return _this;
    }
    ShipmentOrderPackageItem.prototype.SetUIProperties = function () {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        if (this.IsEditingEnabled) {
            if (this.Quantity > 0) {
                isFieldEnabled = true;
                isVolumeEnabled = true;
                isDimensionEnabled = true;
                if (this.Height != null || this.Width != null || this.Length != null) {
                    isVolumeEnabled = false;
                }
                else if (this.Volume != null) {
                    isDimensionEnabled = false;
                }
            }
        }
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        var isFieldRequired1 = false;
        var isFieldRequired2 = false;
        if (this.fatherComponent.TransportModeId != "A") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId)) {
                isFieldRequired1 = true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.GrossWeight)) {
                isFieldRequired2 = true;
            }
        }
        this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, isFieldRequired1);
        this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, isFieldRequired2);
    };
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != newValue) {
                this.EntityPM.PackageTypeId = newValue;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.PackageTypeName = null;
                    this.IsContainer = false;
                }
                else {
                    var myService = new PackageTypeListService_1.PackageTypeListService();
                    myService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PackageTypeName = list.EnglishName;
                                _this.IsContainer = list.IsContainer;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeName != newValue) {
                this.EntityPM.PackageTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "IsContainer", {
        get: function () { return this.EntityPM.IsContainer; },
        set: function (newValue) {
            if (this.EntityPM.IsContainer != newValue) {
                this.EntityPM.IsContainer = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = newValue;
                this.SetUIProperties();
                this.ComputeVolume();
                this.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "Length", {
        get: function () { return this.EntityPM.Length; },
        set: function (newValue) {
            if (this.EntityPM.Length != newValue) {
                this.EntityPM.Length = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "Width", {
        get: function () { return this.EntityPM.Width; },
        set: function (newValue) {
            if (this.EntityPM.Width != newValue) {
                this.EntityPM.Width = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "Height", {
        get: function () { return this.EntityPM.Height; },
        set: function (newValue) {
            if (this.EntityPM.Height != newValue) {
                this.EntityPM.Height = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "Dimensions", {
        get: function () {
            var myDimensions;
            if (this.Length == null && this.Width == null && this.Height == null) {
                myDimensions = " - - ";
            }
            else {
                var myLength = 0;
                var myWidth = 0;
                var myHeight = 0;
                if (this.Length != null) {
                    myLength = this.Length;
                }
                if (this.Width != null) {
                    myWidth = this.Width;
                }
                if (this.Height != null) {
                    myHeight = this.Height;
                }
                myDimensions = myLength + "-" + myWidth + "-" + myHeight;
            }
            return myDimensions;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeVolumetricWeight();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.GrossWeight != myValue) {
                this.EntityPM.GrossWeight = myValue;
                this.SetUIProperties();
                this.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentOrderPackageItem.prototype.OnGrossWeightLostFocus = function (input1) {
        if (this.fatherComponent.IsLCLEntity) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
                if (this.Width == null || this.Height == null || this.Length == null) {
                    this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                    this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);
                    this.SetUIProperties();
                    this.ComputeTotals();
                }
            }
        }
    };
    Object.defineProperty(ShipmentOrderPackageItem.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentOrderPackageItem.prototype.ComputeVolume = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    };
    ShipmentOrderPackageItem.prototype.ComputeVolumetricWeight = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    };
    ShipmentOrderPackageItem.prototype.ComputeTotals = function () {
        if (!this.IsNewEntity) {
            this.fatherComponent.ComputeTotals();
        }
    };
    return ShipmentOrderPackageItem;
}(BaseComponent_1.BaseComponent));
exports.ShipmentOrderPackageItem = ShipmentOrderPackageItem;
//# sourceMappingURL=OrdersTabComponent.js.map