import { RegexSelectors } from './RegexSelectors';

export class BaseSelectors extends RegexSelectors {
    //#region General menus
    public static readonly OperationsMenu = '#GeneralMHOperations';
    public static readonly AccountingMenu = '#GeneralMHAccounting';
    public static readonly CustomersMenu = '#GeneralMHCustomers';
    public static readonly TicketsMenu = '#GeneralMHTicket';
    public static readonly MaintenanceMenu = "#GeneralMHMaintenance"
	public static readonly TariffMenu = "#GeneralMHTariffModule";
    public static readonly ShippingLine = "#MaintenanceItemMTSL"
    //#endregion
    //#region Maintenance
    public static readonly SystemSettings = "#CMS"
    public static readonly CustomsSettings = "#MaintenanceItemCISE"
    //#endregion
    //#region Buttons
    public static readonly RedButton = '.RedButton';
    public static readonly GreenButton = '.GreenButton';
    public static readonly Button = '.Button';
    public static readonly Row0 = '#row0';
    public static readonly UploadDocumentdbtn = "#UploadDocumentdbtn";
    public static readonly OKBtn = "#OKBtn";
    public static readonly SaveWizard = "#SaveWizard";
    public static readonly Backbutton = '#EditBackbutton';
    //#region Contains
    public static readonly ContainsApplytoall = 'Apply to all';
    public static readonly ContainsOK = 'OK';
    public static readonly ContainsShipment = "Shipment: "
    public static readonly ContainsOperations = "Operations"
    public static readonly ContainsAccounting = "Accounting"
    public static readonly ContainsBack = "Back"
    //#endregion
    //#region general
    public static readonly ToggleButtonClass = '.ToggleButton';
    public static readonly label = "label"
    public static readonly button = "button"
    public static readonly typeCheckbox = '[type="checkbox"]'
    public static readonly LogitudeWindow = ".LogitudeWindow";
    public static readonly Label = ".Label"
    public static readonly ListItem='[class="Row ag-row tooltip"]'

    public static readonly RowHover=".RowHover"
    //#endregion
    //#endregion

    public static readonly FirstElementInList = 'ul > li';
    public static readonly AddButton = "#Add";
    public static readonly SpanElement = "span";
    public static readonly DivElement = "div";
    public static readonly FirstRecentEntityItem = ".RecentEntityItem:first";
    public static readonly ToggleIcon = '[src="./Images/ToggleIcon.png"]'
    public static readonly BackBottonBodyClass = ".BackBottonBody"

    public static readonly QueryLink = ".QueryLink";

    public static readonly LogLOVFooterHyperLink = ".LogLOVFooter a";
    public static readonly DownArrowImage = "img[src='./Images/Buttons/downarrow.png']";
    public static readonly Hyperlink = ".hyperlink";

    //Abed Malakh
    public static readonly MaintenanceTransmissionsTab = "#TRANS";
    public static readonly INTTRAMaintenanceItem = "#MaintenanceItemINTTRA_S";
    public static readonly FormFieldRow = ".FormFieldRow";
    public static readonly StartsWithAddButton = "button[id^='Add']";
    public static readonly StartsWithEditButton = "button[id^='Edit']";
    public static readonly SimpleGridViewRow = ".SimpleGridViewRow";
    public static readonly SimpleGridViewHeaderDark = ".SimpleGridViewHeaderDark";
    public static readonly table = "table";
    public static readonly CheckBoxLabel = "label[id^='CheckBox']";
    public static readonly td = "td";
    public static readonly ul = "ul";
    public static readonly li = "li";
    public static readonly ContainsClose = "Close";
    public static readonly ValidationSummaryBlock = ".ValidationSummary table tr td";
    public static readonly DropDownListItem = ".DropDownListItem";
    public static readonly Value = ".Value";
    public static readonly CheckboxInput = "input[type='checkbox']";
}