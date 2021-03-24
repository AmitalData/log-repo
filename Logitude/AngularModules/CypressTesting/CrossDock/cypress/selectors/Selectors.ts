export class CrossdockSelectors {

    public static readonly CrossdockWarehouse = "#WarehouseEntry_WarehouseId"
    public static readonly ShipmentEntryConnectedEntities = "#WarehouseEntryTHConnectedEntities"
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

}

