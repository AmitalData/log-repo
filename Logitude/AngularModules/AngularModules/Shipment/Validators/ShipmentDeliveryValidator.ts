import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { ShipmentPM } from '../EntityPMs/ShipmentPM';
import { ShipmentDeliveryPM } from '../EntityPMs/ShipmentDeliveryPM';
import { Validator } from '../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { RoutingHelper } from '../Tools';

export class ShipmentDeliveryValidator {
    private errors: string[] = [];
    private entityPM: ShipmentDeliveryPM;
    private ShipmentPM: ShipmentPM;
    private ObjectTableName: string = "ShipmentPickUpDelivery";
    private message: string;
    constructor() {
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    Validate = (entityPM: ShipmentDeliveryPM, shipmentPM: ShipmentPM): any[] => {
        this.errors = [];
        this.entityPM = entityPM;
        this.ShipmentPM = shipmentPM;
        this.ValidateItem();
        return this.errors;
    }

    private ValidateItem() {

        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, this.errors);

        // From
        switch (this.entityPM.PickUpDeliveryFromTypeCode) {
            case "PART": {
                if (AppTool.IsNullOrEmpty(this.entityPM.FromPartnerCardId)) {
                    this.errors.push(this.message.replace("%FieldName", "From Partner"));
                }
                break;
            }

            case "PORT": {
                if (AppTool.IsNullOrEmpty(this.entityPM.FromPortId)) {
                    this.errors.push(this.message.replace("%FieldName", "From Port"));
                }
                break;
            }

            case "CASL": {
                if (AppTool.IsNullOrEmpty(this.entityPM.FromAddressCity) && AppTool.IsNullOrEmpty(this.entityPM.FromAddressZipCode)) {
                    this.errors.push("From City or from Zip Code is required");
                }

                if (AppTool.IsNullOrEmpty(this.entityPM.FromAddressCountryId)) {
                    this.errors.push(this.message.replace("%FieldName", "From Country"));
                }
                break;
            }
        }

        // To
        switch (this.entityPM.PickUpDeliveryToTypeCode) {
            case "PART": {
                if (AppTool.IsNullOrEmpty(this.entityPM.ToPartnerCardId)) {
                    this.errors.push(this.message.replace("%FieldName", "To Partner"));
                }
                break;
            }

            case "PORT": {
                if (AppTool.IsNullOrEmpty(this.entityPM.ToPortId)) {
                    this.errors.push(this.message.replace("%FieldName", "To Port"));
                }
                break;
            }

            case "CASL": {
                if (AppTool.IsNullOrEmpty(this.entityPM.ToAddressCity) && AppTool.IsNullOrEmpty(this.entityPM.ToAddressZipCode)) {
                    this.errors.push("To City or to Zip Code is required");
                }

                if (AppTool.IsNullOrEmpty(this.entityPM.ToAddressCountryId)) {
                    this.errors.push(this.message.replace("%FieldName", "To Country"));
                }
                break;
            }
        }

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.entityPM.ATD)) {
            this.errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATD")));
        }

        if (!DateTool.IsActualDateValid(this.entityPM.ATA)) {
            this.errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATA")));
        }

        // Series Dates
        var ETD: number = DateTool.GetDateParts(this.entityPM.ETD).DateTicks;
        var ETA: number = DateTool.GetDateParts(this.entityPM.ETA).DateTicks;
        var ATD: number = DateTool.GetDateParts(this.entityPM.ATD).DateTicks;
        var ATA: number = DateTool.GetDateParts(this.entityPM.ATA).DateTicks;

        var MainCarriageETA: number = DateTool.GetDateParts(this.ShipmentPM.MainCarriageETA).DateTicks;
        var MainCarriageATA: number = DateTool.GetDateParts(this.ShipmentPM.MainCarriageATA).DateTicks;

        var isTransshipment1Exists: boolean = (this.ShipmentPM.Transshipment1FromPortId != null && this.ShipmentPM.Transshipment1ToPortId != null) ? true : false;
        var Transshipment1ETA: number = isTransshipment1Exists ? DateTool.GetDateParts(this.ShipmentPM.Transshipment1ETA).DateTicks : 0;
        var Transshipment1ATA: number = isTransshipment1Exists ? DateTool.GetDateParts(this.ShipmentPM.Transshipment1ATA).DateTicks : 0;

        var isTransshipment2Exists: boolean = (this.ShipmentPM.Transshipment2FromPortId != null && this.ShipmentPM.Transshipment2ToPortId != null) ? true : false;
        var Transshipment2ETA: number = isTransshipment2Exists ? DateTool.GetDateParts(this.ShipmentPM.Transshipment2ETA).DateTicks : 0;
        var Transshipment2ATA: number = isTransshipment2Exists ? DateTool.GetDateParts(this.ShipmentPM.Transshipment2ATA).DateTicks : 0;

        var isTransshipment3Exists: boolean = (this.ShipmentPM.Transshipment3FromPortId != null && this.ShipmentPM.Transshipment3ToPortId != null) ? true : false;
        var Transshipment3ETA: number = isTransshipment3Exists ? DateTool.GetDateParts(this.ShipmentPM.Transshipment3ETA).DateTicks : 0;
        var Transshipment3ATA: number = isTransshipment3Exists ? DateTool.GetDateParts(this.ShipmentPM.Transshipment3ATA).DateTicks : 0;

        var isOnCarriageExists: boolean = (this.ShipmentPM.OnCarriageFromPortId != null && this.ShipmentPM.OnCarriageToPortId != null) ? true : false;
        var OnCarriageETA: number = isOnCarriageExists ? DateTool.GetDateParts(this.ShipmentPM.OnCarriageETA).DateTicks : 0;
        var OnCarriageATA: number = isOnCarriageExists ? DateTool.GetDateParts(this.ShipmentPM.OnCarriageATA).DateTicks : 0;

        var isOnForwardingExists: boolean = (this.ShipmentPM.OnForwardingFromPortId != null && this.ShipmentPM.OnForwardingToPortId != null) ? true : false;
        var OnForwardingETA: number = isOnForwardingExists ? DateTool.GetDateParts(this.ShipmentPM.OnForwardingETA).DateTicks : 0;
        var OnForwardingATA: number = isOnForwardingExists ? DateTool.GetDateParts(this.ShipmentPM.OnForwardingATA).DateTicks : 0;

        var isWarehouseLeg2Exists: boolean = (this.ShipmentPM.WarehouseLeg2WarehouseId != null) ? true : false;
        var WarehouseLeg2EED: number = isWarehouseLeg2Exists ? DateTool.GetDateParts(this.ShipmentPM.WarehouseLeg2ExpectedEntryDate).DateTicks : 0;
        var WarehouseLeg2AED: number = isWarehouseLeg2Exists ? DateTool.GetDateParts(this.ShipmentPM.WarehouseLeg2ActualEntryDate).DateTicks : 0;

       // Self
        if (!RoutingHelper.IsRoutingLegDatesValid(ETD, ETA)) {
            this.errors.push("Expected departure must be less than Expected arrival");
        }

        if (!RoutingHelper.IsRoutingLegDatesValid(ATD, ATA)) {
            this.errors.push("Actual departure must be less than Actual arrival");
        }

        // Previous
        if (isWarehouseLeg2Exists) {
            if (RoutingHelper.IsDateSeriesSmaller(ETD, WarehouseLeg2EED)) {
                this.errors.push("Expected departure must be bigger than Destination Warehouse expected entry");
            }

            if (RoutingHelper.IsDateSeriesSmaller(ATD, WarehouseLeg2AED)) {
                this.errors.push("Actual departure must be bigger than Destination Warehouse actual entry");
            }
        }

        else if (isOnForwardingExists) {
            if (RoutingHelper.IsDateSeriesSmaller(ETD, OnForwardingETA)) {
                this.errors.push("Expected departure must be bigger than On-Forwarding expected arrival");
            }

            if (RoutingHelper.IsDateSeriesSmaller(ATD, OnForwardingATA)) {
                this.errors.push("Actual departure must be bigger than On-Forwarding actual arrival");
            }
        }

        else if (isOnCarriageExists) {
            if (RoutingHelper.IsDateSeriesSmaller(ETD, OnCarriageETA)) {
                this.errors.push("Expected departure must be bigger than On-Carriage expected arrival");
            }

            if (RoutingHelper.IsDateSeriesSmaller(ATD, OnCarriageATA)) {
                this.errors.push("Actual departure must be bigger than On-Carriage actual arrival");
            }
        }

        else if (isTransshipment3Exists) {
            if (RoutingHelper.IsDateSeriesSmaller(ETD, Transshipment3ETA)) {
                this.errors.push("Expected departure must be bigger than Transshipment3 expected arrival");
            }

            if (RoutingHelper.IsDateSeriesSmaller(ATD, Transshipment3ATA)) {
                this.errors.push("Actual departure must be bigger than Transshipment3 actual arrival");
            }
        }

        else if (isTransshipment2Exists) {
            if (RoutingHelper.IsDateSeriesSmaller(ETD, Transshipment2ETA)) {
                this.errors.push("Expected departure must be bigger than Transshipment2 expected arrival");
            }

            if (RoutingHelper.IsDateSeriesSmaller(ATD, Transshipment2ATA)) {
                this.errors.push("Actual departure must be bigger than Transshipment2 actual arrival");
            }
        }

        else if (isTransshipment1Exists) {
            if (RoutingHelper.IsDateSeriesSmaller(ETD, Transshipment1ETA)) {
                this.errors.push("Expected departure must be bigger than Transshipment1 expected arrival");
            }

            if (RoutingHelper.IsDateSeriesSmaller(ATD, Transshipment1ATA)) {
                this.errors.push("Actual departure must be bigger than Transshipment1 actual arrival");
            }
        }

        else {
            if (RoutingHelper.IsDateSeriesSmaller(ETD, MainCarriageETA)) {
                this.errors.push("Expected departure must be bigger than Main-Carriage expected arrival");
            }

            if (RoutingHelper.IsDateSeriesSmaller(ATD, MainCarriageATA)) {
                this.errors.push("Actual departure must be bigger than Main-Carriage actual arrival");
            }
        }
    }

}
