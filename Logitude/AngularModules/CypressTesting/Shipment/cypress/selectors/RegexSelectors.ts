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

    public static InOutSettingsHostAddButton(settingsType: string): string{
        return "[data-cy='" + settingsType + "SettingsHostActions'] button[id^='Add']";
    }

    public static InOutSettingsHostEditButton(settingsType: string): string{
        return "[data-cy='" + settingsType + "SettingsHostActions'] button[id^='Edit']";
    }

    public static BranchINTTRAId(branchName: string){
        return "[data-cy='" + branchName + "_INTTRAId'] #Branch_INTTRAId";
    }

    public static BranchINTTRAAlias(branchName: string){
        return "[data-cy='" + branchName + "_INTTRAAlias'] #Branch_INTTRAAlias";
    }

    public static BranchINTTRAContact(branchName: string){
        return "[data-cy='" + branchName + "_INTTRAContact'] #Branch_INTTRAContactId";
    }

    public static INTTRARegistrationCheckBox(branchName: string, registrationCode: string){
        return "[data-cy='" + registrationCode + "_" + branchName + "_Registration'] input[type='checkbox']";
    }
}