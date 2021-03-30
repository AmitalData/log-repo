export class CrossdockSelectors {

    public static readonly CrossdockWarehouseEntry = "#WarehouseEntry_WarehouseId"
    public static readonly ShipmentEntryConnectedEntities = "#WarehouseEntryTHConnectedEntities"
    public static readonly ShipmentEntryConnectedReleases="#WarehouseReleaseTHConnectedEntities"
    public static readonly CrossdockExpectedEntryDate = "#date_WarehouseEntry_ExpectedEntryDate"
    public static readonly CrossdockExpectedEntryDateDiv = "#datepickerinputdiv_date_WarehouseEntry_ExpectedEntryDate"
    public static readonly CrossdockExpectedEntryTime = "#time_WarehouseEntry_ExpectedEntryDate"
    public static readonly CrossdockActualEntryDate = "#date_WarehouseEntry_ActualEntryDate"
    public static readonly CrossdockActualEntryTime = "#time_WarehouseEntry_ActualEntryDate"
    public static readonly WarehouseCustomerCell = "[data-cy='Customer']"
    public static readonly WarehouseFromCell = "[data-cy='From']"
    public static readonly WarehouseToCell = "[data-cy='To']"
    public static readonly WarehouseConnectedToCell = "[data-cy='connectedTo']"
    public static readonly WarehouseStatusCell = "[data-cy='EntityStatus']"
    public static readonly WarehouseEntryCancellButton = "#WarehouseEntryBCancelEntry"
    public static readonly WarehouseEntryGeneralTab = "#WarehouseEntryTHGeneral"
    public static readonly WarehouseEntryPackagesTab = "#WarehouseEntryTHPackages"
    public static readonly ContainsDatePickerDisabled = "DatePickerInputDivDisabled"
    public static readonly WarehousePackageEditButton = "button[id^='Edit']"
    public static readonly WarehouseEntrySaveButton= "#WarehouseEntry-Save"
    public static readonly CrossdockWarehouseRelease = "#WarehouseRelease_WarehouseId"
    public static readonly CrossdockExpectedReleaseDate = "#date_WarehouseRelease_ExpectedReleaseDate"
    public static readonly CrossdockExpectedReleaseTime ="#time_WarehouseRelease_ExpectedReleaseDate"
    public static readonly WarehouseReleaseSaveButton="#WarehouseRelease-Save"
    public static readonly WarehouseReleaseCancellButton = "#WarehouseReleaseBCancelRelease"
    public static readonly CrossdockExpectedReleaseDateDiv="#datepickerinputdiv_date_WarehouseRelease_ExpectedReleaseDate"
    public static readonly CrossdockActualReleaseDateDiv="#datepickerinputdiv_date_WarehouseRelease_ActualReleaseDate"
    public static readonly CrossdockExpectedReleaseTimeDiv="#datepickerinputdiv_time_WarehouseRelease_ExpectedReleaseDate"
    public static readonly CrossdockActualReleaseTimeDiv="#datepickerinputdiv_time_WarehouseRelease_ActualReleaseDate"
    public static readonly WarehouseReleaseGeneralTab ="#WarehouseReleaseTHGeneral"
    public static readonly CrossdockActualReleaseDate="#date_WarehouseRelease_ActualReleaseDate"
    public static readonly CrossdockActualReleaseTime="#time_WarehouseRelease_ActualReleaseDate"
    public static readonly WarehouseReleaseCreateDeliveryButton="#WarehouseReleaseBCreateDelivery"
    
    
    // public static readonly


    public static CrossdockNewButton(type: string) {
        return "[data-cy='NewCrossDock_" + type + "']"
    }

    public static EntityNumberLink( entityNumber: string) {
        return "[data-cy='Hyperlink_" + entityNumber + "']"
    }

    public static CrossdockStatus(entityNumber: string) {
        return "[data-cy='Status_" + entityNumber + "']"
    }
    public static CrossdockDate(entityNumber: string) {
        return "[data-cy='Date_" + entityNumber + "']"
    }
    public static CheckBoxPackageWarehouseEntryNumber(WarehouseEntryNumber:string){
        return  "[data-cy='CheckBox_" + WarehouseEntryNumber+ "']"

    }
    public static ShipmentNumber(ShipmentNumber: string) {
        return "[data-cy='ShipmentNumber_" + ShipmentNumber + "']"
    }
    public static CrossdockConnectedTo(entityNumber: string) {
        return "[data-cy='ConnectedTo_" + entityNumber + "']"
    }
    
}

