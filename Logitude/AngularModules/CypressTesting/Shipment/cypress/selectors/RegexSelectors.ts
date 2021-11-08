import { ShipmentMapping } from "../mapping/ShipmentMapping";

export class RegexSelectors {

    public static readonly ShipmentPackagesTab = "li[id^='ShipmentTHPackages_']"
    public static readonly ShipmentEventTab = "li[id^='ShipmentTHEvents_']"
    public static readonly Shipment_GrossWeight = "input[id^='Shipment_GrossWeight_']"
    public static readonly ShipmentNumberInTitle = "div[data-cy^='ShipmentNumber']"
    public static readonly RoutingDeliveryLeg = '[data-cy^="Routing_Delivery"]'
    public static DirectionRadio(direction: string): string {
        return "input[id^='DirectionRadio_'][id$='" + ShipmentMapping.GetDirectionCode(direction) + "']";
    }

    public static TransportModeRadio(transportMode: string): string {
        return "input[id^='TransportModeRadio_'][id$='" + ShipmentMapping.GetTransportModeCode(transportMode) + "']";
    }

    public static GroupageShipmentTypeRadio(transportMode: string): string {
        return "input[id^='ShipmentTypeRadio_'][id$='MyG" + ShipmentMapping.GetTransportModeCode(transportMode) + "']";
    }

    public static ShipmentTypeRadio(shipmentType: string): string {
        return "input[id^='ShipmentTypeRadio_'][id$='" + shipmentType + 'D' + "']";
    }

    public static ShipmentTypeRadioInland(shipmentType: string): string {
        return "input[id^='ShipmentTypeRadio_'][id$='" + shipmentType + "']";
    }

    public static INTTRASettingsModeRadio(mode: string): string {
        return "#" + mode.charAt(0).toUpperCase() + mode.slice(1) + "_ModeRadio";
    }

    public static AMANACView(TransportMode: string, AMANACView: string): string {
        return "HyperlinkQuery[data-cy^=" + TransportMode + AMANACView + "]";
    }

    public static AMANACMarkeShipmentAs(MarkAs: string, ShipmentNumber: string): string {
        return "button[data-cy^=" + MarkAs + "_" + ShipmentNumber + "]";
    }

    public static AMANACShipmentNumber(ShipmentNumber: string): string {
        return "td[data-cy^=ShipmentNumber_" + ShipmentNumber + "]";
    }

    public static CheckShipment(ShipmentNumber: string): string {
        return "CheckBox[data-cy^=Check_" + ShipmentNumber + "]";
    }

    public static SplitButton(packageNumber: string): string {
        return "button[data-cy^='Split_'][data-cy$='" + packageNumber + "']";
    }

    public static PackageGrid(cellNumber: string): string {
        return "div[id^='edit-log-grid_'][id$='" + cellNumber + "_0" + "']";
    }

    public static InOutSettingsHostAddButton(settingsType: string): string {
        return "[data-cy='" + settingsType + "SettingsHostActions'] button[id^='Add']";
    }

    public static InOutSettingsHostEditButton(settingsType: string): string {
        return "[data-cy='" + settingsType + "SettingsHostActions'] button[id^='Edit']";
    }

    public static BranchINTTRAId(branchName: string) {
        return "[data-cy='" + branchName + "_INTTRAId'] input[id^='Branch_INTTRAId']";
    }

    public static BranchINTTRAAlias(branchName: string) {
        return "[data-cy='" + branchName + "_INTTRAAlias'] input[id^='Branch_INTTRAAlias']";
    }

    public static BranchINTTRAContact(branchName: string) {
        return "[data-cy='" + branchName + "_INTTRAContact'] input[id^='Branch_INTTRAContactId']";
    }

    public static INTTRARegistrationCheckBox(branchName: string, registrationCode: string) {
        return "[data-cy='" + registrationCode + "_" + branchName + "_Registration'] input[type='checkbox']";
    }

    public static OkButton(mode: string): string {
        return "#Ok" + mode + "Package";
    }

    public static ContainersView(containerView: string): string {
        return "p[data-cy^=Containers" + containerView + "View]";
    }

    public static AddContainerDelivery(ContainerNumber: string): string {
        return "div[data-cy^=AddContainerDelivery_" + ContainerNumber + "]";
    }

    public static EditContainerDelivery(ContainerNumber: string): string {
        return "img[data-cy^=EditContainerDelivery_" + ContainerNumber + "]";
    }

    public static AddContainerReturn(ContainerNumber: string): string {
        return "div[data-cy^=AddContainerReturn_" + ContainerNumber + "]";
    }

    public static EditContainerReturn(ContainerNumber: string): string {
        return "img[data-cy^=EditContainerReturn_" + ContainerNumber + "]";
    }

    public static GridFitstRow(): string {
        return "div[id^='LogGrid_'][id$='row0']";
    }

    public static legBoxItem(legName: string): string {
        return "[data-cy^=Routing_" + legName + "]";
    }

    public static ShortTitleDirectionIcon(direction: string): string {
        return ".ShortTitleDiv img[src='./Images/Directions/" + ShipmentMapping.GetDirectionCode(direction) + ".png']";
    }

    public static PartnerBoxItem(partnerType: string): string {
        return "[data-cy='BoxItem_" + partnerType + "']";
    }

    public static HouseCheckBox(houseNumber: string): string {
        return "[data-cy='CheckBox_" + houseNumber + "']";
    }
}