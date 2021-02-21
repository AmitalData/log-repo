import { ShipmentMapping } from "../mapping/ShipmentMapping";

export class RegexSelectors {
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
}