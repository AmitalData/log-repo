import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { ShipmentPM } from '../EntityPMs/ShipmentPM';
import { ShipmentPickUpPM } from '../EntityPMs/ShipmentPickUpPM';
import { Validator } from '../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { RoutingHelper } from '../Tools';

export class ShipmentPickupValidator {
    private errors: string[] = [];
    private EntityPM: ShipmentPickUpPM;
    private ShipmentPM: ShipmentPM;
    private ObjectTableName: string = "ShipmentPickUpDelivery";
    private message: string;
    constructor() {
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    Validate = (entityPM: ShipmentPickUpPM, shipmentPM: ShipmentPM): any[] => {
        this.errors = [];
        this.EntityPM = entityPM;
        this.ShipmentPM = shipmentPM;
        this.ValidateItem();
        return this.errors;
    }

    private ValidateItem() {

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.errors);

        // From
        switch (this.EntityPM.PickUpDeliveryFromTypeCode) {
            case "PART": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerCardId)) {
                    this.errors.push(this.message.replace("%FieldName", "From Partner"));
                }
                break;
            }

            case "PORT": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.FromPortId)) {
                    this.errors.push(this.message.replace("%FieldName", "From Port"));
                }
                break;
            }

            case "CASL": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.FromAddressCity) && AppTool.IsNullOrEmpty(this.EntityPM.FromAddressZipCode)) {
                    this.errors.push("From City or from Zip Code is required");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.FromAddressCountryId)) {
                    this.errors.push(this.message.replace("%FieldName", "From Country"));
                }
                break;
            }
        }

        // To
        switch (this.EntityPM.PickUpDeliveryToTypeCode) {
            case "PART": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerCardId)) {
                    this.errors.push(this.message.replace("%FieldName", "To Partner"));
                }
                break;
            }

            case "PORT": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.ToPortId)) {
                    this.errors.push(this.message.replace("%FieldName", "To Port"));
                }
                break;
            }

            case "CASL": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.ToAddressCity) && AppTool.IsNullOrEmpty(this.EntityPM.ToAddressZipCode)) {
                    this.errors.push("To City or to Zip Code is required");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.ToAddressCountryId)) {
                    this.errors.push(this.message.replace("%FieldName", "To Country"));
                }
                break;
            }
        }

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.EntityPM.ATD)) {
            this.errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATD")));
        }

        if (!DateTool.IsActualDateValid(this.EntityPM.ATA)) {
            this.errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATA")));
        }

        // Series Dates
        var ETD: number = DateTool.GetDateParts(this.EntityPM.ETD).DateTicks;
        var ETA: number = DateTool.GetDateParts(this.EntityPM.ETA).DateTicks;
        var ATD: number = DateTool.GetDateParts(this.EntityPM.ATD).DateTicks;
        var ATA: number = DateTool.GetDateParts(this.EntityPM.ATA).DateTicks;

        var isWarehouseLegExists: boolean = (this.ShipmentPM.WarehouseLegWarehouseId != null && this.ShipmentPM.DirectionId != "I") ? true : false;
        var WarehouseLegEED: number = isWarehouseLegExists ? DateTool.GetDateParts(this.ShipmentPM.WarehouseLegExpectedEntryDate).DateTicks : 0;        
        var WarehouseLegAED: number = isWarehouseLegExists ? DateTool.GetDateParts(this.ShipmentPM.WarehouseLegActualEntryDate).DateTicks : 0;

        var isPreCarriageExists: boolean = (this.ShipmentPM.PreCarriageFromPortId != null && this.ShipmentPM.PreCarriageToPortId != null) ? true : false;
        var PreCarriageETD: number = isPreCarriageExists ? DateTool.GetDateParts(this.ShipmentPM.PreCarriageETD).DateTicks : 0;
        var PreCarriageATD: number = isPreCarriageExists ? DateTool.GetDateParts(this.ShipmentPM.PreCarriageATD).DateTicks : 0;

        var isPreForwardingExists: boolean = (this.ShipmentPM.PreForwardingFromPortId != null && this.ShipmentPM.PreForwardingToPortId != null) ? true : false;
        var PreForwardingETD: number = isPreForwardingExists ? DateTool.GetDateParts(this.ShipmentPM.PreForwardingETD).DateTicks : 0;
        var PreForwardingATD: number = isPreForwardingExists ? DateTool.GetDateParts(this.ShipmentPM.PreForwardingATD).DateTicks : 0;

        var isMainCarriageExists: boolean = true;
        var MainCarriageETD: number = DateTool.GetDateParts(this.ShipmentPM.MainCarriageETD).DateTicks;
        var MainCarriageATD: number = DateTool.GetDateParts(this.ShipmentPM.MainCarriageATD).DateTicks;

        // Self
        if (!RoutingHelper.IsRoutingLegDatesValid(ETD, ETA)) {
            this.errors.push("Expected departure must be less than Expected arrival");
        }

        if (!RoutingHelper.IsRoutingLegDatesValid(ATD, ATA)) {
            this.errors.push("Actual departure must be less than Actual arrival");
        }

        // Next
        if (isWarehouseLegExists) {

            var isFirstPickup: boolean = true;

            var firsPickup = RoutingHelper.GetFirstPickup(this.ShipmentPM.ShipmentPickUps);
            if (firsPickup) {
                isFirstPickup = false;

                if (firsPickup.Id == this.EntityPM.Id) {
                    isFirstPickup = true;
                }
            }

            if (isFirstPickup) {
                if (RoutingHelper.IsDateSeriesBiggerNotEqual(ETA, WarehouseLegEED)) {
                    this.errors.push("Expected arrival must be equal or less than Warehouse expected entry");
                }

                if (RoutingHelper.IsDateSeriesBiggerNotEqual(ATA, WarehouseLegAED)) {
                    this.errors.push("Actual arrival must be equal or less than Warehouse actual entry");
                }
            }
        }

        else if (isPreForwardingExists) {
            if (RoutingHelper.IsDateSeriesBigger(ETA, PreForwardingETD)) {
                this.errors.push("Expected arrival must be less than pre forwarding expected departure");
            }

            if (RoutingHelper.IsDateSeriesBigger(ATA, PreForwardingATD)) {
                this.errors.push("Actual arrival must be less than pre forwarding actual departure");
            }
        }

        else if (isPreCarriageExists) {
            if (RoutingHelper.IsDateSeriesBigger(ETA, PreCarriageETD)) {
                this.errors.push("Expected arrival must be less than pre carriage expected departure");
            }

            if (RoutingHelper.IsDateSeriesBigger(ATA, PreCarriageATD)) {
                this.errors.push("Actual arrival must be less than pre carriage actual departure");
            }
        }

        else if (isMainCarriageExists) {
            if (RoutingHelper.IsDateSeriesBigger(ETA, MainCarriageETD)) {
                this.errors.push("Expected arrival must be less than main carriage expected departure");
            }

            if (RoutingHelper.IsDateSeriesBigger(ATA, MainCarriageATD)) {
                this.errors.push("Actual arrival must be less than main carriage actual departure");
            }
        }
    }
}
