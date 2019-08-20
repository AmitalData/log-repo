"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ConsoleShipmentPM = /** @class */ (function () {
    function ConsoleShipmentPM(entityParentPM) {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(ConsoleShipmentPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "IsLCL", {
        get: function () { return this.isLCL; },
        set: function (newValue) { this.isLCL = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "IsFCL", {
        get: function () { return this.isFCL; },
        set: function (newValue) { this.isFCL = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ShipmentNumber", {
        get: function () { return this.shipmentNumber; },
        set: function (newValue) { this.shipmentNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "MasterShipmentDataId", {
        get: function () { return this.masterShipmentDataId; },
        set: function (newValue) { this.masterShipmentDataId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "FHLStatusCode", {
        get: function () { return this.fHLStatusCode; },
        set: function (newValue) { this.fHLStatusCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "FHLStatusName", {
        get: function () { return this.fHLStatusName; },
        set: function (newValue) { this.fHLStatusName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "CargonautFHLStatusCode", {
        get: function () { return this.cargonautFHLStatusCode; },
        set: function (newValue) { this.cargonautFHLStatusCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "CargonautFHLStatusName", {
        get: function () { return this.cargonautFHLStatusName; },
        set: function (newValue) { this.cargonautFHLStatusName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "FNAReason", {
        get: function () { return this.fNAReason; },
        set: function (newValue) { this.fNAReason = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "TEU", {
        get: function () { return this.tEU; },
        set: function (newValue) { this.tEU = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "Volume", {
        get: function () { return this.volume; },
        set: function (newValue) { this.volume = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "GrossWeight", {
        get: function () { return this.grossWeight; },
        set: function (newValue) { this.grossWeight = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "GrossWeightInKG", {
        get: function () { return this.grossWeightInKG; },
        set: function (newValue) { this.grossWeightInKG = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "GrossWeightPerTon", {
        get: function () { return this.grossWeightPerTon; },
        set: function (newValue) { this.grossWeightPerTon = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ChargeableWeight", {
        get: function () { return this.chargeableWeight; },
        set: function (newValue) { this.chargeableWeight = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ChargeableWeightInKG", {
        get: function () { return this.chargeableWeightInKG; },
        set: function (newValue) { if (this.chargeableWeightInKG != newValue) {
            this.chargeableWeightInKG = newValue;
            this.MarkAsDirty();
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "VolumetricWeight", {
        get: function () { return this.volumetricWeight; },
        set: function (newValue) { this.volumetricWeight = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "OAMTPayables_Local", {
        get: function () { return this.oAMTPayables_Local; },
        set: function (newValue) { this.oAMTPayables_Local = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ACCTPayables_Local", {
        get: function () { return this.aCCTPayables_Local; },
        set: function (newValue) { this.aCCTPayables_Local = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "OAMTPayables_Profit", {
        get: function () { return this.oAMTPayables_Profit; },
        set: function (newValue) { this.oAMTPayables_Profit = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ACCTPayables_Profit", {
        get: function () { return this.aCCTPayables_Profit; },
        set: function (newValue) { this.aCCTPayables_Profit = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "OAMTReceivables_Local", {
        get: function () { return this.oAMTReceivables_Local; },
        set: function (newValue) { this.oAMTReceivables_Local = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ACCTReceivables_Local", {
        get: function () { return this.aCCTReceivables_Local; },
        set: function (newValue) { this.aCCTReceivables_Local = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "OAMTReceivables_Profit", {
        get: function () { return this.oAMTReceivables_Profit; },
        set: function (newValue) { this.oAMTReceivables_Profit = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ACCTReceivables_Profit", {
        get: function () { return this.aCCTReceivables_Profit; },
        set: function (newValue) { this.aCCTReceivables_Profit = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "OAMTReceivables_Local_NoParent", {
        get: function () { return this.oAMTReceivables_Local_NoParent; },
        set: function (newValue) { this.oAMTReceivables_Local_NoParent = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ACCTReceivables_Local_NoParent", {
        get: function () { return this.aCCTReceivables_Local_NoParent; },
        set: function (newValue) { this.aCCTReceivables_Local_NoParent = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "OAMTReceivables_Profit_NoParent", {
        get: function () { return this.oAMTReceivables_Profit_NoParent; },
        set: function (newValue) { this.oAMTReceivables_Profit_NoParent = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ACCTReceivables_Profit_NoParent", {
        get: function () { return this.aCCTReceivables_Profit_NoParent; },
        set: function (newValue) { this.aCCTReceivables_Profit_NoParent = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ValueOfGoods", {
        get: function () { return this.valueOfGoods; },
        set: function (newValue) { this.valueOfGoods = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "FreightPayablesAmount", {
        get: function () { return this.freightPayablesAmount; },
        set: function (newValue) { this.freightPayablesAmount = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "FreightReceivablesAmount", {
        get: function () { return this.freightReceivablesAmount; },
        set: function (newValue) { this.freightReceivablesAmount = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "NumberOfPackages", {
        get: function () { return this.numberOfPackages; },
        set: function (newValue) { this.numberOfPackages = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "NumberOfContainers", {
        get: function () { return this.numberOfContainers; },
        set: function (newValue) { this.numberOfContainers = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "VolumeInCBM", {
        get: function () { return this.volumeInCBM; },
        set: function (newValue) { this.volumeInCBM = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "FCLDataList", {
        get: function () {
            if (this.fCLDataList == null) {
                this.fCLDataList = [];
            }
            return this.fCLDataList;
        },
        set: function (newValue) {
            if (this.fCLDataList != newValue) {
                this.fCLDataList = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsoleShipmentPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    ConsoleShipmentPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return ConsoleShipmentPM;
}());
exports.ConsoleShipmentPM = ConsoleShipmentPM;
var HouseContainerPackage = /** @class */ (function () {
    function HouseContainerPackage() {
    }
    Object.defineProperty(HouseContainerPackage.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HouseContainerPackage.prototype, "ConsoleId", {
        get: function () { return this.consoleId; },
        set: function (newValue) { this.consoleId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HouseContainerPackage.prototype, "Quantity", {
        get: function () { return this.quantity; },
        set: function (newValue) { this.quantity = newValue; },
        enumerable: true,
        configurable: true
    });
    return HouseContainerPackage;
}());
//# sourceMappingURL=ConsoleShipmentPM.js.map