export class RegexSelectors {

    public static readonly CheckBoxLine = "label[id^='CheckBox_'][id$='" + "_LBL" + "']";
    public static readonly CheckBox = '[id^="CheckBox"][id$="LBL"]'
    public static readonly SearchField = "input[id^='SearchFieldsId_']"
    public static readonly MoreList = "div[id^='MenuButtons_']";
    public static readonly SaveAsOpenButton = "Button[id^='SendButtom_']";
    public static readonly NullSearch = 'input[id^="null_Search"]'
    public static readonly ComboBoxLast = "div[id^=ComboBox_]:last"
    public static readonly DropdownListItem = "div[id^=Dropdown_]:last > div > ul > li"
    public static readonly InputCheckBox = "input[id^='CheckBox_']"
    public static readonly Refresh = '[id^="Refresh_"]'
    public static readonly EmailSearchTextBox = 'input[id^=EmailSearchTextBox_TextArea]';

    public static PackageGrid(cellNumber: string): string {
        return "div[id^='edit-log-grid_'][id$='_" + cellNumber + "_0" + "']";
    }

    public static SaveClose(entityType: string): string {
        return "#" + RegexSelectors.SwitchToCustomer(entityType) + "-SaveClose"
    }

    public static AccountTab(entityType: string): string {
        return "#" + RegexSelectors.SwitchToCustomer(entityType) + "THAccounting"
    }

    private static SwitchToCustomer(entityType: string): string {
        if (entityType == "Partner") {
            return "Customer"
        }
        return entityType;
    }

    public static SpanTitle(title: string): string {
        return "span[title='" + title + "']";
    }

    public static CellWithRowAndCol(col: string, row: string): string {
        return "div[id^='edit-log-grid_'][id$='_" + col + "_" + row + "']";
    }

    public static GridFitstRow(): string {
        return "div[id^='LogGrid_'][id$='row0']";
    }

    public static WarehouseStorageMeasurement(mode: string) {
        return "#Warehouse_" + mode + "WeightMeasurementCode"
    }

    public static WarehouseStorageRounding(mode: string) {
        return "#Warehouse_" + mode + "WeightRoundingCode"
    }
}