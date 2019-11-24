import {QuotePM} from '../EntityPMs/QuotePM';
import {QuoteChargePM} from '../EntityPMs/QuoteChargePM';
import {AppTool} from '../../Infrastructure/Tools';
import {QuoteTool} from '../Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {VatTypesValidator} from '../../Infrastructure/Validators/VatTypesValidator';

export class QuoteValidator {  
    
    public Validate(entityPM: QuotePM) {
        var errors = [];        
        var objectTableName: "Quote";
        var isInlandDomestic: boolean = false;
    
        if (entityPM != null) {
            isInlandDomestic = entityPM.TransportModeId == "I" && entityPM.DirectionId == "D" ? true : false;

            Validator.TryValidateObject(entityPM, objectTableName, errors);

            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

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
            
            if (AppTool.IsNullOrEmpty(entityPM.CustomerId) || AppTool.IsNullOrEmpty(entityPM.QuoteCustomerTypeCode)) {
                var field = TextCodeTranslator.Translate("Quote.F.CustomerId");
                errors.push(msg.replace("%FieldName", field));
            }

            //Partners
            if (isInlandDomestic) {
                if (AppTool.IsNullOrEmpty(entityPM.ConsigneeId)) {
                    errors.push("Consignee field is required");
                }

                if (AppTool.IsNullOrEmpty(entityPM.ShipperId)) {
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
                if (AppTool.IsNullOrEmpty(entityPM.FromPortId)) {
                    var textCode = QuoteTool.GetFromPortTextCode(entityPM.TransportModeId);
                    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
                }

                if (AppTool.IsNullOrEmpty(entityPM.ToPortId)) {
                    var textCode = QuoteTool.GetToPortTextCode(entityPM.TransportModeId);
                    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
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

                if (AppTool.IsNullOrEmpty(entityPM.ShipperId)) {
                    validatePickupFields = true;
                }

                else if (AppTool.IsNullOrEmpty(entityPM.PickUpAddressId)) {
                    validatePickupFields = true;
                }

                if (validatePickupFields) {
                    if (AppTool.IsNullOrEmpty(entityPM.FromAddressCountryId)) {
                        errors.push("Pickup Country field is required");
                    }

                    if (AppTool.IsNullOrEmpty(entityPM.FromAddressCity) && AppTool.IsNullOrEmpty(entityPM.FromAddressZipCode)) {
                        errors.push("Pickup City or Pickup Zip Code is required");
                    }
                }
            }
            
            //Delivery
            if (entityPM.IncludeDelivery) {
                var validateDeliveryFields = false;

                if (AppTool.IsNullOrEmpty(entityPM.ConsigneeId)) {
                    validateDeliveryFields = true;
                }

                else if (AppTool.IsNullOrEmpty(entityPM.DeliveryAddressId)) {
                    validateDeliveryFields = true;
                }

                if (validateDeliveryFields) {
                    if (AppTool.IsNullOrEmpty(entityPM.ToAddressCountryId)) {
                        errors.push("Delivery Country field is required");
                    }

                    if (AppTool.IsNullOrEmpty(entityPM.ToAddressCity) && AppTool.IsNullOrEmpty(entityPM.ToAddressZipCode)) {
                        errors.push("Delivery City or Delivery Zip Code is required");
                    }
                }
            }

            //Quote Charges
            if (entityPM.QuoteCharges.length > 0) {

                var vatTypesIds: string[] = [];

                entityPM.QuoteCharges.forEach((item) => {

                    this.ValidateCharge(item, errors);

                    if (entityPM.IsChargesByVAT) {
                        if (!AppTool.IsNullOrEmpty(item.VatTypeId)) {

                            if (vatTypesIds.filter(f => f == item.VatTypeId).length == 0) {
                                vatTypesIds.push(item.VatTypeId);
                            }

                            if (!item.VatIsMultiPercentage) {
                                if (AppTool.IsNullOrEmpty(item.VatPercentage)) {
                                    var field = TextCodeTranslator.Translate("QuoteCharge.F.VatPercentage");
                                    errors.push(msg.replace("%FieldName", field));
                                }
                            }
                        }
                    }
                });

                var isValid = VatTypesValidator.ValidateMultiPercentages(vatTypesIds);
                if (!isValid) {
                    errors.push(VatTypesValidator.GetError());
                }
            }
        }          
        
        return errors;
    }
    private ValidateFCLDuplicatedPackages(entityPM: QuotePM, errors: string[]) {
        var isFCLQuote = AppTool.IsFCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId);
        if (isFCLQuote) {

            var list: string[] = [];
            var isDuplicatedPackage: boolean = false;

            if (!AppTool.IsNullOrEmpty(entityPM.PackageType1Id)) {

                if (list.filter(f => f == entityPM.PackageType1Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(entityPM.PackageType1Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(entityPM.PackageType2Id)) {

                if (list.filter(f => f == entityPM.PackageType2Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(entityPM.PackageType2Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(entityPM.PackageType3Id)) {

                if (list.filter(f => f == entityPM.PackageType3Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(entityPM.PackageType3Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(entityPM.PackageType4Id)) {

                if (list.filter(f => f == entityPM.PackageType4Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(entityPM.PackageType4Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(entityPM.PackageType5Id)) {

                if (list.filter(f => f == entityPM.PackageType5Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(entityPM.PackageType5Id);
                }
            }

            if (isDuplicatedPackage) {
                errors.push("Cannot add the same container type twice. You can adjust the QTY for one of them");
            }

            for (var i = 1; i <= 5; i++) {
                if (!AppTool.IsNullOrEmpty(entityPM["PackageType" + i + "Quantity"]) && AppTool.IsNullOrEmpty(entityPM["PackageType" + i + "Id"])) {
                    errors.push("Package Quantity line " + i + " is not allowed without package type");
                }
            }

            
        }
    }
    private ValidateCharge(charge: QuoteChargePM, errors: string[]) {
        var objectTableName: "QuoteCharge";

        Validator.TryValidateObject(charge, objectTableName, errors);

        if (charge.QuoteTypeCode == "A") {
            if (charge.SaleExchangeRate == null) {
                errors.push(TextCodeTranslator.Translate("QuoteCharge.M.ExchangeRateIsRequired"));
            }
        }

    }
}
