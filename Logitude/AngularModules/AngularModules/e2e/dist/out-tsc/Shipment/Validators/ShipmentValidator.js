"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var Tools_2 = require("../Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ShipmentValidator = /** @class */ (function () {
    function ShipmentValidator() {
        var _this = this;
        this.Errors = [];
        this.IsInlandDomestic = false;
        this.IsLCLEntity = false;
        this.IsFCLEntity = false;
        this.Validate = function (entityPM) {
            _this.Errors = [];
            _this.entityPM = entityPM;
            if (entityPM && !SessionLocator_1.SessionLocator.TenantPM.IsDocumentsArchive) {
                _this.IsInlandDomestic = _this.entityPM.TransportModeId == "I" && _this.entityPM.DirectionId == "D" ? true : false;
                _this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(_this.entityPM.TransportModeId, _this.entityPM.ShipmentTypeId);
                _this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(_this.entityPM.TransportModeId, _this.entityPM.ShipmentTypeId);
                Validator_1.Validator.TryValidateObject(_this.entityPM, "Shipment", _this.Errors);
                if (entityPM.Ratio > 10 || entityPM.Ratio < 1) {
                    _this.Errors.push("Ratio must be between 1-10");
                }
                _this.ValidatePartners();
                _this.ValidatePorts();
                _this.ValidateInlandDomestic();
                if (Tools_1.AppTool.IsNullOrEmpty(_this.entityPM.Id)) {
                    _this.ValidatePickup();
                    _this.ValidateDelivery();
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.DeclarationNumber)) {
                    if (entityPM.DeclarationDate == null) {
                        _this.Errors.push("Declaration Date field is required");
                    }
                }
                _this.ValidatePackages();
                _this.ValidatePickups();
                _this.ValidateDeliveries();
                _this.ValidatePayables();
                _this.ValidateReceivables();
                Tools_2.RoutingHelper.ValidateRoutingsActualDates(entityPM, _this.Errors);
                Tools_2.RoutingHelper.ValidateRoutingsSeriesDates(entityPM, _this.Errors);
            }
            return _this.Errors;
        };
        this.Errors = [];
        this.message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    ShipmentValidator.prototype.ValidatePartners = function () {
        if (this.entityPM.ShipmentCustomerTypeCode == "IGT") {
            if (this.entityPM.TransportModeId != "A") {
                this.Errors.push("Can't add issuing carrier agent partner for this specific transport mode");
            }
        }
        if (this.entityPM.ShipmentLevelCode == "C") {
            //if (AppTool.IsNullOrEmpty(this.entityPM.AgentId)) {
            //    this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate("Master.F.AgentId")));
            //}
        }
        else {
            //if ((this.entityPM.DirectionId.toUpperCase() == "E" || this.entityPM.DirectionId.toUpperCase() == "R") && AppTool.IsNullOrEmpty(this.entityPM.ShipperId)) {
            //    this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ShipperId")));
            //}
            //else if (this.entityPM.DirectionId.toUpperCase() == "I" && AppTool.IsNullOrEmpty(this.entityPM.ConsigneeId)) {
            //    this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ConsigneeId")));
            //}
            //else if (this.entityPM.DirectionId.toUpperCase() == "D" && (AppTool.IsNullOrEmpty(this.entityPM.ShipperId) || AppTool.IsNullOrEmpty(this.entityPM.ConsigneeId))) {
            //    if (AppTool.IsNullOrEmpty(this.entityPM.ShipperId)) {
            //        this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ShipperId")));
            //    }
            if (this.IsInlandDomestic) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.ConsigneeId)) {
                    this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ConsigneeId")));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.ShipperId)) {
                    this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ShipperId")));
                }
            }
            //}
            //else {
            if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.CustomerId) || Tools_1.AppTool.IsNullOrEmpty(this.entityPM.ShipmentCustomerTypeCode)) {
                this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.CustomerId")));
            }
            //}
        }
    };
    ShipmentValidator.prototype.ValidatePorts = function () {
        if (!this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.MainCarriageFromPortId)) {
                var textCode = Tools_2.ShipmentTool.GetFromPortTextCode(this.entityPM.TransportModeId, this.entityPM.ShipmentLevelCode);
                this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(textCode)));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.MainCarriageToPortId)) {
                var textCode = Tools_2.ShipmentTool.GetToPortTextCode(this.entityPM.TransportModeId, this.entityPM.ShipmentLevelCode);
                this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(textCode)));
            }
        }
    };
    ShipmentValidator.prototype.ValidateInlandDomestic = function () {
        if (this.IsInlandDomestic) {
            if (this.entityPM.ShipmentLevelCode == "C") {
                this.Errors.push("Master inland domestic are not allowed");
            }
            else if (this.entityPM.ShipmentLevelCode == "H") {
                this.Errors.push("House inland domestic shipments are not allowed");
            }
        }
    };
    ShipmentValidator.prototype.ValidatePickup = function () {
        if (this.entityPM.ShipmentLevelCode != "C") {
            if (this.entityPM.IncludePickUp) {
                var validatePickupFields = false;
                if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.PickUpAddressId)) {
                    validatePickupFields = true;
                }
                if (validatePickupFields) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.FromAddressCountryId)) {
                        this.Errors.push("Pickup Country is required");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.FromAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.entityPM.FromAddressZipCode)) {
                        this.Errors.push("Pickup City or Pickup Zip Code is required");
                    }
                }
            }
        }
    };
    ShipmentValidator.prototype.ValidateDelivery = function () {
        if (this.entityPM.ShipmentLevelCode != "C") {
            if (this.entityPM.IncludeDelivery) {
                var validateDeliveryFields = false;
                if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.DeliveryAddressId)) {
                    validateDeliveryFields = true;
                }
                if (validateDeliveryFields) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.ToAddressCountryId)) {
                        this.Errors.push("Delivery Country is required");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.ToAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.entityPM.ToAddressZipCode)) {
                        this.Errors.push("Delivery City or Delivery Zip Code is required");
                    }
                }
            }
        }
    };
    ShipmentValidator.prototype.ValidatePackages = function () {
        //if (!AppTool.IsNullOrEmpty(this.entityPM.AWBCommodityItemNumber)) {
        //    if (!FormatTool.Validate_CommodityNo(this.entityPM.AWBCommodityItemNumber)) {
        //        var fieldName:string = TextCodeTranslator.Translate("Shipment.F.AWBCommodityItemNumber");
        //        this.Errors.push(fieldName + " must be 4-7 numeric");
        //    }
        //}
        var _this = this;
        this.entityPM.ShipmentPackages.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, "ShipmentPackage", _this.Errors);
            if (_this.entityPM.TransportModeId != 'A') {
                if (Tools_1.AppTool.IsNullOrEmpty(item.PackageTypeId)) {
                    if (_this.IsLCLEntity) {
                        _this.Errors.push("Package Type is required");
                    }
                    else {
                        _this.Errors.push("Container Type is required");
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(item.Weight)) {
                    _this.Errors.push("Gross Weight is required");
                }
            }
        });
    };
    ShipmentValidator.prototype.ValidatePickups = function () {
        var _this = this;
        this.entityPM.ShipmentPickUps.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, "ShipmentPickUpDelivery", _this.Errors);
            // Validate item Logic
        });
    };
    ShipmentValidator.prototype.ValidateDeliveries = function () {
        var _this = this;
        this.entityPM.ShipmentDeliveries.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, "ShipmentPickUpDelivery", _this.Errors);
            // Validate item Logic
        });
    };
    ShipmentValidator.prototype.ValidatePayables = function () {
        var _this = this;
        var vatTypesIds = [];
        this.entityPM.ShipmentPayables.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, "ShipmentPayable", _this.Errors);
            if (item.ShipmentPayableAmountTypeCode == "ACCU") {
                if (Tools_1.AppTool.IsNullOrEmpty(item.MeasurementId)) {
                    _this.Errors.push("Measurement Weight is required");
                }
            }
            if (item.VatTypeId != null) {
                if (vatTypesIds.filter(function (f) { return f == item.VatTypeId; }).length == 0) {
                    vatTypesIds.push(item.VatTypeId);
                }
            }
        });
    };
    ShipmentValidator.prototype.ValidateReceivables = function () {
        var _this = this;
        var vatTypesIds = [];
        this.entityPM.ShipmentReceivables.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, "ShipmentReceivable", _this.Errors);
            if (item.TotalAmount != null && item.UnitPrice != null && item.Quantity != null) {
                if (item.Rate == null) {
                    _this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentReceivable.M.ExchangeRateIsRequired"));
                }
            }
            if (item.VatTypeId != null) {
                if (vatTypesIds.filter(function (f) { return f == item.VatTypeId; }).length == 0) {
                    vatTypesIds.push(item.VatTypeId);
                }
            }
        });
    };
    return ShipmentValidator;
}());
exports.ShipmentValidator = ShipmentValidator;
//# sourceMappingURL=ShipmentValidator.js.map