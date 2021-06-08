import { RegexSelectors } from '../selectors/RegexSelectors';

export class PackageTypeSelectors extends RegexSelectors {

    public static readonly PackageTypeCode = "#PackageType_Code"
    public static readonly PackageTypeName = "#PackageType_EnglishName"
    public static readonly PackageTypeLocalName = "#PackageType_LocalName"
    public static readonly PackageTypeTEU = "#PackageType_TEU"
    public static readonly PackageTypeContainerSize = "#PackageType_ContainerSize"
    public static readonly PackageTypeVolume = "#PackageType_Volume"
    public static readonly PackageTypePrintAs = "#PackageType_PrintAs"
    public static readonly PackageTypeAirCheckBox = "#PackageType_IsAir"
    public static readonly InActivePackageTypeCheckBox = "#PackageType_InActive"
    public static readonly PackageTypeSaveButton = "#PackageType-Save"
    public static readonly PackageTypeEventsTab = "#PackageTypeTHEvents"
    public static readonly PackageTypeSaveCloseButton = "#PackageType-SaveClose";
    public static readonly MaintenanceItemPackageType = "#MaintenanceItemMTPK"
    public static readonly CodeDigitCount = 4
}
