import {QuotePM} from '../EntityPMs/QuotePM';
import {QuoteChargePM} from '../EntityPMs/QuoteChargePM';
import {AppTool} from '../../Infrastructure/Tools';
import {QuoteTool} from '../Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {VatTypesValidator} from '../../Infrastructure/Validators/VatTypesValidator';

export class QuoteValidator {  
    private Errors: string[] = [];
    private entityPM: QuotePM;

    constructor() {
        this.Errors = [];
    }

    Validate (entityPM: QuotePM){
        this.Errors = [];
        this.entityPM = entityPM;
        
        var objectTableName: "Quote";
        var isInlandDomestic: boolean = false;
    
        if (this.entityPM != null) {
            isInlandDomestic = this.entityPM.TransportModeId == "I" && this.entityPM.DirectionId == "D" ? true : false;

            Validator.TryValidateObject(this.entityPM, objectTableName, this.Errors);

            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            if (this.entityPM.Ratio > 10 || this.entityPM.Ratio < 1) {
                this.Errors.push("Ratio must be between 1-10");
            }

            // General Data
            if (this.entityPM.ExpirationDays < 0) {
                this.Errors.push("Expiration days cant be a minus value");
            }

            if (this.entityPM.AutomaticallyCloseDays < 0) {
                this.Errors.push("Automatically close days cant be a minus value");
            }
            
            if (AppTool.IsNullOrEmpty(this.entityPM.CustomerId) || AppTool.IsNullOrEmpty(this.entityPM.QuoteCustomerTypeCode)) {
                var field = TextCodeTranslator.Translate("Quote.F.CustomerId");
                this.Errors.push(msg.replace("%FieldName", field));
            }

            //Partners
            if (isInlandDomestic) {
                this.ValidateInlandDomestic();                
            }            
            
            //Ports
            if (!isInlandDomestic) {
                if (AppTool.IsNullOrEmpty(this.entityPM.FromPortId)) {
                    var textCode = QuoteTool.GetFromPortTextCode(this.entityPM.TransportModeId);
                    this.Errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
                }

                if (AppTool.IsNullOrEmpty(this.entityPM.ToPortId)) {
                    var textCode = QuoteTool.GetToPortTextCode(this.entityPM.TransportModeId);
                    this.Errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
                }
            }

            this.ValidateFCLDuplicatedPackages();

            //Pickup
            if (this.entityPM.IncludePickUp) {
                var validatePickupFields = false;

                if (AppTool.IsNullOrEmpty(this.entityPM.ShipperId)) {
                    validatePickupFields = true;
                }

                else if (AppTool.IsNullOrEmpty(this.entityPM.PickUpAddressId)) {
                    validatePickupFields = true;
                }

                if (validatePickupFields) {
                    if (AppTool.IsNullOrEmpty(this.entityPM.FromAddressCountryId)) {
                        this.Errors.push("Pickup Country field is required");
                    }

                    if (AppTool.IsNullOrEmpty(this.entityPM.FromAddressCity) && AppTool.IsNullOrEmpty(this.entityPM.FromAddressZipCode)) {
                        this.Errors.push("Pickup City or Pickup Zip Code is required");
                    }
                }
            }
            
            //Delivery
            if (this.entityPM.IncludeDelivery) {
                var validateDeliveryFields = false;

                if (AppTool.IsNullOrEmpty(this.entityPM.ConsigneeId)) {
                    validateDeliveryFields = true;
                }

                else if (AppTool.IsNullOrEmpty(this.entityPM.DeliveryAddressId)) {
                    validateDeliveryFields = true;
                }

                if (validateDeliveryFields) {
                    if (AppTool.IsNullOrEmpty(this.entityPM.ToAddressCountryId)) {
                        this.Errors.push("Delivery Country field is required");
                    }

                    if (AppTool.IsNullOrEmpty(this.entityPM.ToAddressCity) && AppTool.IsNullOrEmpty(this.entityPM.ToAddressZipCode)) {
                        this.Errors.push("Delivery City or Delivery Zip Code is required");
                    }
                }
            }

            //Quote Charges
            if (this.entityPM.QuoteCharges.length > 0) {

                var vatTypesIds: string[] = [];

                this.entityPM.QuoteCharges.forEach((item) => {

                    this.ValidateCharge(item);

                    if (this.entityPM.IsChargesByVAT) {
                        if (!AppTool.IsNullOrEmpty(item.VatTypeId)) {

                            if (vatTypesIds.filter(f => f == item.VatTypeId).length == 0) {
                                vatTypesIds.push(item.VatTypeId);
                            }

                            if (!item.VatIsMultiPercentage) {
                                if (AppTool.IsNullOrEmpty(item.VatPercentage)) {
                                    var field = TextCodeTranslator.Translate("QuoteCharge.F.VatPercentage");
                                    this.Errors.push(msg.replace("%FieldName", field));
                                }
                            }
                        }
                    }
                });

                var isValid = VatTypesValidator.ValidateMultiPercentages(vatTypesIds);
                if (!isValid) {
                    this.Errors.push(VatTypesValidator.GetError());
                }
            }
        }          
        
        return this.Errors;
    }
    private ValidateInlandDomestic() {
        if (AppTool.IsNullOrEmpty(this.entityPM.ConsigneeId)) {
            this.Errors.push("Consignee field is required");
        }

        if (AppTool.IsNullOrEmpty(this.entityPM.ShipperId)) {
            this.Errors.push("Shipper field is required");
        }

        this.ValidateFrom();
        this.ValidateTo();
    }
    private ValidateFrom() {
        switch (this.entityPM.InlandDomesticFromTypeCode) {
            case "PART":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.FromPartnerId)) {
                        this.Errors.push("From Partner field is required");
                    }
                    break;
                }

            case "PORT":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.FromPortId)) {
                        this.Errors.push("From Port field is required");
                    }
                    break;
                }

            case "CASL":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.InlandDomesticFromCity)) {
                        this.Errors.push("From City field is required");
                    }

                    if (AppTool.IsNullOrEmpty(this.entityPM.InlandDomesticFromCountryId)) {
                        this.Errors.push("From Country field is required");
                    }
                    break;
                }
        }
    }
    private ValidateTo() {
        switch (this.entityPM.InlandDomesticToTypeCode) {
            case "PART":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.ToPartnerId)) {
                        this.Errors.push("To Partner field is required");
                    }
                    break;
                }

            case "PORT":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.ToPortId)) {
                        this.Errors.push("To Port field is required");
                    }
                    break;
                }

            case "CASL":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.InlandDomesticToCity)) {
                        this.Errors.push("To City field is required");
                    }

                    if (AppTool.IsNullOrEmpty(this.entityPM.InlandDomesticToCountryId)) {
                        this.Errors.push("To Country field is required");
                    }
                    break;
                }
        }
    }

    private ValidateFCLDuplicatedPackages() {
        var isFCLQuote = AppTool.IsFCLEntity(this.entityPM.TransportModeId, this.entityPM.ShipmentTypeId);
        if (isFCLQuote) {

            var list: string[] = [];
            var isDuplicatedPackage: boolean = false;

            if (!AppTool.IsNullOrEmpty(this.entityPM.PackageType1Id)) {

                if (list.filter(f => f == this.entityPM.PackageType1Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(this.entityPM.PackageType1Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(this.entityPM.PackageType2Id)) {

                if (list.filter(f => f == this.entityPM.PackageType2Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(this.entityPM.PackageType2Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(this.entityPM.PackageType3Id)) {

                if (list.filter(f => f == this.entityPM.PackageType3Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(this.entityPM.PackageType3Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(this.entityPM.PackageType4Id)) {

                if (list.filter(f => f == this.entityPM.PackageType4Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(this.entityPM.PackageType4Id);
                }
            }

            if (!AppTool.IsNullOrEmpty(this.entityPM.PackageType5Id)) {

                if (list.filter(f => f == this.entityPM.PackageType5Id).length > 0) {
                    isDuplicatedPackage = true;
                }

                else {
                    list.push(this.entityPM.PackageType5Id);
                }
            }

            if (isDuplicatedPackage) {
                this.Errors.push("Cannot add the same container type twice. You can adjust the QTY for one of them");
            }

            for (var i = 1; i <= 5; i++) {
                if (!AppTool.IsNullOrEmpty(this.entityPM["PackageType" + i + "Quantity"]) && AppTool.IsNullOrEmpty(this.entityPM["PackageType" + i + "Id"])) {
                    this.Errors.push("Package type is required when Quantity is filled");
                    break;
                }
            }

            
        }
    }
    private ValidateCharge(charge: QuoteChargePM) {
        var objectTableName: "QuoteCharge";

        Validator.TryValidateObject(charge, objectTableName, this.Errors);

        if (charge.QuoteTypeCode == "A") {
            if (charge.SaleExchangeRate == null) {
                this.Errors.push(TextCodeTranslator.Translate("QuoteCharge.M.ExchangeRateIsRequired"));
            }
        }

    }

    public CheckDuplicateInCharges(entitpPM: QuotePM, charge: QuoteChargePM, errors: string[]) {
        if (entitpPM.QuoteCharges.filter(c => c.ChargesTypeCode === charge.ChargesTypeCode && c != charge).length > 0) {
            errors.push(charge.ChargesTypeName + " Charge is duplicated");
        }
    }

    public ValidateFreightQuoteCharges(quotePM: QuotePM, charge: QuoteChargePM): string {
        var errors = null;
        var numberOfFreightQuoteCharges = quotePM.QuoteCharges?.filter(d => d.ChargesGroupCode == "FRT" && d != charge).length;
        if (quotePM.TransportModeId == "I") {
            var allInItems = quotePM.QuoteCharges.filter(d => d.IsAllIN).length;
            if (numberOfFreightQuoteCharges > 0 && charge.ChargesGroupCode == "FRT" && allInItems > 0) {
                errors = "A second freight charge cannot be added while some charges are marked as 'All-In'.";
            }
        }
        else {
            if (numberOfFreightQuoteCharges > 0) {
                errors = "Freight Charge already added";
            }
        }
        
        return errors;
    }

}   
