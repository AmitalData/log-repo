import { ShipmentMapping } from "../mapping/ShipmentMapping";

export class RegexSelectors {

    public static readonly ShipmentPackagesTab= "li[id^='ShipmentTHPackages_']"
    public static readonly ShipmentEventTab= "li[id^='ShipmentTHEvents_']"
    public static readonly Shipment_GrossWeight= "input[id^='Shipment_GrossWeight_']"
    public static readonly ShipmentNumberInTitle= "div[data-cy^='ShipmentNumber']"

    public static DirectionRadio(direction: string): string {
        return "input[id^='DirectionRadio_'][id$='" + ShipmentMapping.GetDirectionCode(direction) + "']";
    }

    public static TransportModeRadio(transportMode: string): string{
        return "input[id^='TransportModeRadio_'][id$='" + ShipmentMapping.GetTransportModeCode(transportMode) + "']";
    }

    public static GroupageShipmentTypeRadio(transportMode: string): string{
        return "input[id^='ShipmentTypeRadio_'][id$='MyG" + ShipmentMapping.GetTransportModeCode(transportMode) + "']";
    }

    public static ShipmentTypeRadio(shipmentType: string): string{
        return "input[id^='ShipmentTypeRadio_'][id$='" + shipmentType + 'D' + "']";
    }

    public static SplitButton(packageNumber: string): string{
        return "button[data-cy^='Split_'][data-cy$='" + packageNumber + "']";
    }

    public static PackageGrid(cellNumber: string): string{
        return "div[id^='edit-log-grid_'][id$='" + cellNumber +"_0"+ "']";
    }
}