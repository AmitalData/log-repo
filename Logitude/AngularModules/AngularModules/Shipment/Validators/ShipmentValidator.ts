import {ShipmentPM} from '../EntityPMs/ShipmentPM';
import {AppTool, FormatTool} from '../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

export interface IShipmentValidator {
    Validate(shipmentPM: ShipmentPM): any[];
}

export class ShipmentValidator implements IShipmentValidator {
    private Errors: string[] = [];
    private entityPM: ShipmentPM;
    private IsInlandDomestic: boolean = false;
    private IsLCLEntity: boolean = false;
    private IsFCLEntity: boolean = false;
    private message: string;
    constructor() {
        this.Errors = [];
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    Validate = (entityPM: ShipmentPM): any[] => {        
        this.Errors = [];
        this.entityPM = entityPM;

        if (entityPM && !SessionLocator.TenantPM.IsDocumentsArchive) {
            this.IsInlandDomestic = this.entityPM.TransportModeId == "I" && this.entityPM.DirectionId == "D" ? true : false;
            this.IsLCLEntity = AppTool.IsLCLEntity(this.entityPM.TransportModeId, this.entityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.entityPM.TransportModeId, this.entityPM.ShipmentTypeId);

            Validator.TryValidateObject(this.entityPM, "Shipment", this.Errors);

            if (entityPM.Ratio > 10 || entityPM.Ratio < 1) {
                this.Errors.push("Ratio must be between 1-10");
            }

            this.ValidatePartners(); 
            this.ValidatePorts();                       
            this.ValidateInlandDomestic();

            if (AppTool.IsNullOrEmpty(this.entityPM.Id)) {
                this.ValidatePickup();
                this.ValidateDelivery();
            }

            if (!AppTool.IsNullOrEmpty(entityPM.DeclarationNumber)) {
                if (entityPM.DeclarationDate == null) {
                    this.Errors.push("Declaration Date field is required");
                }
            }

            this.ValidatePackages();
            this.ValidatePickups();
            this.ValidateDeliveries();
            this.ValidatePayables();
            this.ValidateReceivables();
            RoutingHelper.ValidateRoutingsActualDates(entityPM, this.Errors);
            RoutingHelper.ValidateRoutingsSeriesDates(entityPM, this.Errors);
        }

        return this.Errors;
    }

    private ValidatePartners() {
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
                if (AppTool.IsNullOrEmpty(this.entityPM.ConsigneeId)) {
                    this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ConsigneeId")));
                }

                if (AppTool.IsNullOrEmpty(this.entityPM.ShipperId)) {
                    this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ShipperId")));
                }
            }
            //}

            //else {
            if (AppTool.IsNullOrEmpty(this.entityPM.CustomerId) || AppTool.IsNullOrEmpty(this.entityPM.ShipmentCustomerTypeCode)) {
                this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.CustomerId")));
            }
            //}
        }
    }
    private ValidatePorts() {
        if (!this.IsInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.entityPM.MainCarriageFromPortId)) {
                var textCode = ShipmentTool.GetFromPortTextCode(this.entityPM.TransportModeId, this.entityPM.ShipmentLevelCode);
                this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
            }

            if (AppTool.IsNullOrEmpty(this.entityPM.MainCarriageToPortId)) {
                var textCode = ShipmentTool.GetToPortTextCode(this.entityPM.TransportModeId, this.entityPM.ShipmentLevelCode);
                this.Errors.push(this.message.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
            }
        }
    }   
    private ValidateInlandDomestic() {
        if (this.IsInlandDomestic) {
            if (this.entityPM.ShipmentLevelCode == "C") {
                this.Errors.push("Master inland domestic are not allowed");
            }

            else if (this.entityPM.ShipmentLevelCode == "H") {
                this.Errors.push("House inland domestic shipments are not allowed");
            }
        }
    }
    private ValidatePickup() {
        if (this.entityPM.ShipmentLevelCode != "C") {
            if (this.entityPM.IncludePickUp) {
                var validatePickupFields: boolean = false;

                if (AppTool.IsNullOrEmpty(this.entityPM.PickUpAddressId)) {
                    validatePickupFields = true;
                }

                if (validatePickupFields) {
                    if (AppTool.IsNullOrEmpty(this.entityPM.FromAddressCountryId)) {
                        this.Errors.push("Pickup Country is required");
                    }

                    if (AppTool.IsNullOrEmpty(this.entityPM.FromAddressCity) && AppTool.IsNullOrEmpty(this.entityPM.FromAddressZipCode)) {
                        this.Errors.push("Pickup City or Pickup Zip Code is required");
                    }
                }
            }
        }
    }
    private ValidateDelivery() {
        if (this.entityPM.ShipmentLevelCode != "C") {
            if (this.entityPM.IncludeDelivery) {
                var validateDeliveryFields: boolean = false;

                if (AppTool.IsNullOrEmpty(this.entityPM.DeliveryAddressId)) {
                    validateDeliveryFields = true;
                }

                if (validateDeliveryFields) {
                    if (AppTool.IsNullOrEmpty(this.entityPM.ToAddressCountryId)) {
                        this.Errors.push("Delivery Country is required");
                    }

                    if (AppTool.IsNullOrEmpty(this.entityPM.ToAddressCity) && AppTool.IsNullOrEmpty(this.entityPM.ToAddressZipCode)) {
                        this.Errors.push("Delivery City or Delivery Zip Code is required");
                    }
                }
            }
        }
    }

    private ValidatePackages() {

        //if (!AppTool.IsNullOrEmpty(this.entityPM.AWBCommodityItemNumber)) {
        //    if (!FormatTool.Validate_CommodityNo(this.entityPM.AWBCommodityItemNumber)) {
        //        var fieldName:string = TextCodeTranslator.Translate("Shipment.F.AWBCommodityItemNumber");
        //        this.Errors.push(fieldName + " must be 4-7 numeric");
        //    }
        //}

        this.entityPM.ShipmentPackages.forEach(item => {
            Validator.TryValidateObject(item, "ShipmentPackage", this.Errors);

            if (this.entityPM.TransportModeId != 'A') {
                if (AppTool.IsNullOrEmpty(item.PackageTypeId)) {

                    if (this.IsLCLEntity) {
                        this.Errors.push("Package Type is required");
                    }

                    else {
                        this.Errors.push("Container Type is required");
                    }
                }

                if (AppTool.IsNullOrEmpty(item.Weight)) {
                    this.Errors.push("Gross Weight is required");
                }
            }
        });
    }
    private ValidatePickups() {
        this.entityPM.ShipmentPickUps.forEach(item => {
            Validator.TryValidateObject(item, "ShipmentPickUpDelivery", this.Errors);

            // Validate item Logic
        });
    }
    private ValidateDeliveries() {
        this.entityPM.ShipmentDeliveries.forEach(item => {
            Validator.TryValidateObject(item, "ShipmentPickUpDelivery", this.Errors);

            // Validate item Logic
        });
    }
    private ValidatePayables() {

        var vatTypesIds: string[] = [];

        this.entityPM.ShipmentPayables.forEach(item => {

            Validator.TryValidateObject(item, "ShipmentPayable", this.Errors);

            if (item.ShipmentPayableAmountTypeCode == "ACCU") {
                if (AppTool.IsNullOrEmpty(item.MeasurementId)) {
                    this.Errors.push("Measurement Weight is required");
                }
            }

            if (item.VatTypeId != null) {
                if (vatTypesIds.filter(f => f == item.VatTypeId).length == 0) {
                    vatTypesIds.push(item.VatTypeId);
                }
            }
        });
    }
    private ValidateReceivables() {

        var vatTypesIds: string[] = [];

        this.entityPM.ShipmentReceivables.forEach(item => {
            Validator.TryValidateObject(item, "ShipmentReceivable", this.Errors);

            if (item.TotalAmount != null && item.UnitPrice != null && item.Quantity != null) {
                if (item.Rate == null) {
                    this.Errors.push(TextCodeTranslator.Translate("ShipmentReceivable.M.ExchangeRateIsRequired"));
                }
            }

            if (item.VatTypeId != null) {
                if (vatTypesIds.filter(f => f == item.VatTypeId).length == 0) {
                    vatTypesIds.push(item.VatTypeId);
                }
            }
        });
    }
}

