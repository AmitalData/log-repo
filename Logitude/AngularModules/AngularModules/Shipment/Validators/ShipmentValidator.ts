import {ShipmentPM} from '../EntityPMs/ShipmentPM';
import {AppTool, FormatTool} from '../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import { ShipmentPickupValidator } from './ShipmentPickupValidator';
import { ShipmentDeliveryValidator } from './ShipmentDeliveryValidator';

export interface IShipmentValidator {
    Validate(shipmentPM: ShipmentPM): any[];
}

export class ShipmentValidator implements IShipmentValidator {
    private Errors: string[] = [];
    private entityPM: ShipmentPM;
    private IsInlandDomestic: boolean = false;
    private IsStandAloneShipment: boolean = false;
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
            this.IsStandAloneShipment = this.entityPM.IsStandalonePickupDelivery;
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
            
            //this.ValidateProductItems();
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

            if (this.IsInlandDomestic && !this.IsStandAloneShipment) {
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

            this.ValidateFrom();
            this.ValidateTo();
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
        if (this.IsFCLEntity) {
            for (var i = 1; i <= 5; i++) {
                if (!AppTool.IsNullOrEmpty(this.entityPM["Quantity" + i]) && AppTool.IsNullOrEmpty(this.entityPM["PackageTypeId" + i])) {
                    this.Errors.push("Package type is required when Quantity is filled");
                    break;
                }
            }
        }

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
            if(this.entityPM.ShipmentTypeId == "FCLD" && this.entityPM.DirectionId == "C"){
                this.Errors.push(this.ValidateContainerNumber(item.ContainerNumber));
            }
            
        });
    }

    ValidateContainerNumber(containerNumber: string): string {
        const pattern = /^[A-Za-z]{4}\d{7}$/;
    
        if (!pattern.test(containerNumber)) {
            return TextCodeTranslator.Translate("ShipmentPackage.O.NotValidContainerNumber");
        }
    
        return null;
    }

    private ValidatePickups() {

        var validator = new ShipmentPickupValidator();

        this.entityPM.ShipmentPickUps.forEach(item => {
            var errors: string[] = validator.Validate(item, this.entityPM);

            errors.forEach(i => {
                this.Errors.push(i);
            });
        });
    }
    private ValidateDeliveries() {
        var validator = new ShipmentDeliveryValidator();

        this.entityPM.ShipmentDeliveries.forEach(item => {
            var errors: string[] = validator.Validate(item, this.entityPM);

            errors.forEach(i => {
                this.Errors.push(i);
            });
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
    private ValidateProductItems() {
        this.entityPM.ShipmentProductItems.filter(d => !d.IsEmptyLine).forEach(item => {
            Validator.TryValidateObject(item, "ShipmentProductItem", this.Errors);

            if (AppTool.IsNullOrEmpty(item.SKU)) {
                this.Errors.push("Product Item SKU is required");
            }
        });
    }
    private ValidateFrom() {
        switch (this.entityPM.InlandDomesticFromTypeCode) {
            case "PART":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.MainCarriageFromPartnerId)) {
                        this.Errors.push("From Partner field is required");
                    }
                    break;
                }

            case "PORT":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.MainCarriageFromPortId)) {
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
                    if (AppTool.IsNullOrEmpty(this.entityPM.MainCarriageToPartnerId)) {
                        this.Errors.push("To Partner field is required");
                    }
                    break;
                }

            case "PORT":
                {
                    if (AppTool.IsNullOrEmpty(this.entityPM.MainCarriageToPortId)) {
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
}

