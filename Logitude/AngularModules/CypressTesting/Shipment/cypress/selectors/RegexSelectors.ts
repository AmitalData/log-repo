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

    public static INTTRASettingsModeRadio(mode: string): string{
        return "#" + mode.charAt(0).toUpperCase() + mode.slice(1) + "_ModeRadio";
    }

    public static ContainersView(containerView: string): string{
        return "p[data-cy^=Containers" + containerView + "View]";
    }

    public static AddContainerDelivery(ContainerNumber: string): string{
        return "div[data-cy^=AddContainerDelivery_" + ContainerNumber + "]";
    }

    public static EditContainerDelivery(ContainerNumber: string): string{
        return "img[data-cy^=EditContainerDelivery_" + ContainerNumber + "]";
    }

    public static AddContainerReturn(ContainerNumber: string): string{
        return "div[data-cy^=AddContainerReturn_" + ContainerNumber + "]";
    }

    public static EditContainerReturn(ContainerNumber: string): string{
        return "img[data-cy^=EditContainerReturn_" + ContainerNumber + "]";
    }

    public static GridFitstRow(): string{
        return "div[id^='LogGrid_'][id$='row0']";
    }
}