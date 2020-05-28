"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var Tools_2 = require("../Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var VatTypesValidator_1 = require("../../Infrastructure/Validators/VatTypesValidator");
var QuoteValidator = /** @class */ (function () {
    function QuoteValidator() {
    }
    QuoteValidator.prototype.Validate = function (entityPM) {
        var _this = this;
        var errors = [];
        var objectTableName;
        var isInlandDomestic = false;
        if (entityPM != null) {
            isInlandDomestic = entityPM.TransportModeId == "I" && entityPM.DirectionId == "D" ? true : false;
            Validator_1.Validator.TryValidateObject(entityPM, objectTableName, errors);
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            if (entityPM.Ratio > 10 || entityPM.Ratio < 1) {
                errors.push("Ratio must be between 1-10");
            }
            // General Data
            if (entityPM.ExpirationDays < 0) {
                errors.push("Expiration days cant be a minus value");
            }
            if (entityPM.AutomaticallyCloseDays < 0) {
                errors.push("Automatically close days cant be a minus value");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.CustomerId) || Tools_1.AppTool.IsNullOrEmpty(entityPM.QuoteCustomerTypeCode)) {
                var field = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.CustomerId");
                errors.push(msg.replace("%FieldName", field));
            }
            //Partners
            if (isInlandDomestic) {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ConsigneeId)) {
                    errors.push("Consignee field is required");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ShipperId)) {
                    errors.push("Shipper field is required");
                }
            }
            //switch (entityPM.DirectionId.toUpperCase()) {
            //    case "E":
            //    case "R":
            //        {
            //            if (AppTool.IsNullOrEmpty(entityPM.ShipperId)) {
            //                errors.push("Shipper field is required");
            //            }
            //            break;
            //        }
            //    case "I":
            //        {
            //            if (AppTool.IsNullOrEmpty(entityPM.ConsigneeId)) {
            //                errors.push("Consignee field is required");
            //            }
            //            break;
            //        }
            //    case "D":
            //        {
            //            if (AppTool.IsNullOrEmpty(entityPM.ShipperId)) {
            //                errors.push("Shipper field is required");
            //            }
            //            if (isInlandDomestic) {
            //                if (AppTool.IsNullOrEmpty(entityPM.ConsigneeId)) {
            //                    errors.push("Consignee field is required");
            //                }
            //            }
            //            break;
            //        }
            //}
            //Ports
            if (!isInlandDomestic) {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.FromPortId)) {
                    var textCode = Tools_2.QuoteTool.GetFromPortTextCode(entityPM.TransportModeId);
                    errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(textCode)));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ToPortId)) {
                    var textCode = Tools_2.QuoteTool.GetToPortTextCode(entityPM.TransportModeId);
                    errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(textCode)));
                }
                //if (entityPM.DirectionId == "D") {
                //    if (!AppTool.IsNullOrEmpty(entityPM.FromPortId) && !AppTool.IsNullOrEmpty(entityPM.ToPortId)) {
                //        if (entityPM.FromCountryId != entityPM.ToCountryId) {
                //            errors.push("Both Ports must be in the same country since the direction is Domestic");
                //        }
                //    }
                //}
            }
            //InlandDomestic
            //if (isInlandDomestic) {
            //    if (!AppTool.IsNullOrEmpty(entityPM.ShipperId) && !AppTool.IsNullOrEmpty(entityPM.ConsigneeId)) {
            //        if (entityPM.FromCountryId != entityPM.ToCountryId) {
            //            if (entityPM.FromCountryIsEC == false || entityPM.ToCountryIsEC == false) {
            //                errors.push("Both Addresses must be in the same country since the direction is Domestic");
            //            }
            //        }
            //    }
            //}
            this.ValidateFCLDuplicatedPackages(entityPM, errors);
            //Pickup
            if (entityPM.IncludePickUp) {
                var validatePickupFields = false;
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ShipperId)) {
                    validatePickupFields = true;
                }
                else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.PickUpAddressId)) {
                    validatePickupFields = true;
                }
                if (validatePickupFields) {
                    if (Tools_1.AppTool.IsNullOrEmpty(entityPM.FromAddressCountryId)) {
                        errors.push("Pickup Country field is required");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(entityPM.FromAddressCity) && Tools_1.AppTool.IsNullOrEmpty(entityPM.FromAddressZipCode)) {
                        errors.push("Pickup City or Pickup Zip Code is required");
                    }
                }
            }
            //Delivery
            if (entityPM.IncludeDelivery) {
                var validateDeliveryFields = false;
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ConsigneeId)) {
                    validateDeliveryFields = true;
                }
                else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.DeliveryAddressId)) {
                    validateDeliveryFields = true;
                }
                if (validateDeliveryFields) {
                    if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ToAddressCountryId)) {
                        errors.push("Delivery Country field is required");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ToAddressCity) && Tools_1.AppTool.IsNullOrEmpty(entityPM.ToAddressZipCode)) {
                        errors.push("Delivery City or Delivery Zip Code is required");
                    }
                }
            }
            //Quote Charges
            if (entityPM.QuoteCharges.length > 0) {
                var vatTypesIds = [];
                entityPM.QuoteCharges.forEach(function (item) {
                    _this.ValidateCharge(item, errors);
                    if (entityPM.IsChargesByVAT) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.VatTypeId)) {
                            if (vatTypesIds.filter(function (f) { return f == item.VatTypeId; }).length == 0) {
                                vatTypesIds.push(item.VatTypeId);
                            }
                            if (!item.VatIsMultiPercentage) {
                                if (Tools_1.AppTool.IsNullOrEmpty(item.VatPercentage)) {
                                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteCharge.F.VatPercentage");
                                    errors.push(msg.replace("%FieldName", field));
                                }
                            }
                        }
                    }
                });
                var isValid = VatTypesValidator_1.VatTypesValidator.ValidateMultiPercentages(vatTypesIds);
                if (!isValid) {
                    errors.push(VatTypesValidator_1.VatTypesValidator.GetError());
                }
            }
        }
        return errors;
    };
    QuoteValidator.prototype.ValidateFCLDuplicatedPackages = function (entityPM, errors) {
        var isFCLQuote = Tools_1.AppTool.IsFCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId);
        if (isFCLQuote) {
            var list = [];
            var isDuplicatedPackage = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PackageType1Id)) {
                if (list.filter(function (f) { return f == entityPM.PackageType1Id; }).length > 0) {
                    isDuplicatedPackage = true;
                }
                else {
                    list.push(entityPM.PackageType1Id);
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PackageType2Id)) {
                if (list.filter(function (f) { return f == entityPM.PackageType2Id; }).length > 0) {
                    isDuplicatedPackage = true;
                }
                else {
                    list.push(entityPM.PackageType2Id);
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PackageType3Id)) {
                if (list.filter(function (f) { return f == entityPM.PackageType3Id; }).length > 0) {
                    isDuplicatedPackage = true;
                }
                else {
                    list.push(entityPM.PackageType3Id);
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PackageType4Id)) {
                if (list.filter(function (f) { return f == entityPM.PackageType4Id; }).length > 0) {
                    isDuplicatedPackage = true;
                }
                else {
                    list.push(entityPM.PackageType4Id);
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PackageType5Id)) {
                if (list.filter(function (f) { return f == entityPM.PackageType5Id; }).length > 0) {
                    isDuplicatedPackage = true;
                }
                else {
                    list.push(entityPM.PackageType5Id);
                }
            }
            if (isDuplicatedPackage) {
                errors.push("Cannot add the same container type twice. You can adjust the QTY for one of them");
            }
        }
    };
    QuoteValidator.prototype.ValidateCharge = function (charge, errors) {
        var objectTableName;
        Validator_1.Validator.TryValidateObject(charge, objectTableName, errors);
        if (charge.QuoteTypeCode == "A") {
            if (charge.SaleExchangeRate == null) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteCharge.M.ExchangeRateIsRequired"));
            }
        }
    };
    return QuoteValidator;
}());
exports.QuoteValidator = QuoteValidator;
//# sourceMappingURL=QuoteValidator.js.map