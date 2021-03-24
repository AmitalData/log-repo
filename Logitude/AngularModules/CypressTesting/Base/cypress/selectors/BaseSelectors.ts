import { RegexSelectors } from './RegexSelectors';

export class BaseSelectors extends RegexSelectors {
    //#region General menus
    public static readonly OperationsMenu = '#GeneralMHOperations';
    public static readonly AccountingMenu = '#GeneralMHAccounting';
    public static readonly CustomersMenu = '#GeneralMHCustomers';
    public static readonly CRMMenu = "#GeneralMHCRM"
    public static readonly TicketsMenu = '#GeneralMHTicket';
    public static readonly MaintenanceMenu = "#GeneralMHMaintenance"
	public static readonly TariffMenu = "#GeneralMHTariffModule";
    public static readonly ShippingLine = "#MaintenanceItemMTSL"
    //#endregion
    //#region CRM
    public static readonly CRMCustomers="#CRMCUS"
    //#endregion
    //#region Maintenance
    public static readonly SystemSettings = "#CMS"
    public static readonly CustomsSettings = "#MaintenanceItemCISE"
    public static readonly Warehouse = "#MaintenanceItemMTWH"
    public static readonly Vendor = "#MaintenanceItemMTVD"
    //#endregion
    //#region charge Types
    public static readonly ChargesTypeAccounting="#ChargesTypeTHAccounting"
    public static readonly ChargesTypeReceivableCreditAccount="#ChargesType_ReceivableCreditAccount"
    public static readonly ChargesTypeSaveClose="#ChargesType-SaveClose"
    //#endregion
    //#region Currency
    public static readonly CurrencyAccounting="#CurrencyTHAccounting"
    public static readonly CurrencyAccountingExternalCode="#Currency_AccountingExternalCode"
    public static readonly CurrencySaveClose="#Currency-SaveClose"
    //#endregion
    //#region Customer
    public static readonly CustomerAccounting= "#CustomerTHAccounting"
    public static readonly CustomerReceivablesAccountingCard="#Customer_ReceivablesAccountingCard"
    public static readonly CustomerSaveClose="#Customer-SaveClose"
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
    public static readonly SaveButton = '#SaveBtn';
    public static readonly ConfrimApproved = '#ConfrimApproved';
    public static readonly SettingsButton = '#Settings';

    //#endregion
    //#region Contains
    public static readonly ContainsApplytoall = 'Apply to all';
    public static readonly ContainsOK = 'OK';
    public static readonly ContainsShipment = "Shipment: "
    public static readonly ContainsOperations = "Operations"
    public static readonly ContainsAccounting = "Accounting"
    public static readonly ContainsBack = "Back"
    public static readonly ContainsEdit="Edit"
    public static readonly EUR="EUR"
    public static readonly TestCompany="TestCompany"
    public static readonly AirFreight="Air Freight"
    public static readonly ChargesType="ChargesType"
    public static readonly Currency="Currency"
    public static readonly Partner="Partner"
    public static readonly ContainClose="Close"
    public static readonly ContainsCancel = "Cancel"
    public static readonly ContainsClose = "Close"
    public static readonly ContainsExport = 'Export'
    public static readonly ContainsSendtoCustoms = "Send to Customs"
    public static readonly  ContainSave="Save"
    public static readonly  ContainPrint="Print"
    public static readonly ContainSelected= "Selected:"

    public static readonly ContainsAddFollowup = "Add Follow up"
    public static readonly ContainsFillDimensions = "Fill Dimensions"

    public static readonly ContainsAddContainerDelivery = "Add Container Delivery"
    public static readonly ContainsAddEmptyContainerReturn = "Add Empty Container Return"

    public static readonly ContainYes="Yes"
    public static readonly ContainNo="No"
    public static readonly ContainSettings="Settings"
    public static readonly ContainWarehouse="Warehouse"
    public static readonly ContainVendor="Vendor"


    //#endregion
    //#region general
    public static readonly ToggleButtonClass = '.ToggleButton';
    public static readonly label = "label"
    public static readonly button = "button"
    public static readonly typeCheckbox = '[type="checkbox"]'
    public static readonly LogitudeWindow = ".LogitudeWindow";
    public static readonly MessageWindow = ".MessageWindow"
    public static readonly LabelClass = ".Label"
    public static readonly ListItem='[class="Row ag-row tooltip"]'
    public static readonly DivListItem='div[class="Row ag-row tooltip"]'
    public static readonly MaintenanceButton=".MaintenanceButton"
    public static readonly RowHover=".RowHover"
    public static readonly MouseoverTrigger='mouseover'
    public static readonly LogitudeScrollViewer='.LogitudeScrollViewer'
    public static readonly DeleteButton='button[id^=Delete]'
    public static readonly Anchor="a"
    //#endregion

    public static readonly FirstElementInList = 'ul > li';
    public static readonly LastElement = ":last";
    public static readonly FirstElement = ":first";
    public static readonly AddButton = "#Add";
    public static readonly SpanElement = "span";
    public static readonly DivElement = 'div';
    public static readonly TextElement = 'text';
    public static readonly FirstRecentEntityItem = ".RecentEntityItem:first";
    public static readonly ToggleIcon = '[src="./Images/ToggleIcon.png"]'
    public static readonly BackBottonBodyClass = ".BackBottonBody"
    public static readonly QueryLink = ".QueryLink";
    public static readonly MTCPopup='#MTCPopup';
    public static readonly LogLOVFooterHyperLink = ".LogLOVFooter a";
    public static readonly DownArrowImage = "img[src='./Images/Buttons/downarrow.png']";
    public static readonly RefreshImg = "img[src='./Images/Buttons/Refresh.png']";
    public static readonly Hyperlink = ".hyperlink";
    public static readonly Title = ".Title";
    public static readonly input = "input"

    //#region Should Condition 
    public static readonly BeEmpty = 'be.empty'
    public static readonly NotBeEmpty = 'not.be.empty'
    public static readonly BeDisabled = 'be.disabled'
    public static readonly NotBeDisabled = 'not.be.disabled'
    public static readonly HaveClass = 'have.class'
    public static readonly HaveValue = 'have.value'
    public static readonly NotHaveClass = 'not.have.class'
    public static readonly Exist = 'exist'
    public static readonly NotExist = 'not.exist'
    //#endregion

    public static readonly MaintenanceTransmissionsTab = "#TRANS";
    public static readonly INTTRAMaintenanceItem = "#MaintenanceItemINTTRA_S";
    public static readonly FormFieldRow = ".FormFieldRow";
    public static readonly StartsWithAddButton = "button[id^='Add']";
    public static readonly StartsWithEditButton = "button[id^='Edit']";
    public static readonly SimpleGridViewHeaderDark = ".SimpleGridViewHeaderDark";
    public static readonly table = "table";
    public static readonly CheckBoxLabel = "label[id^='CheckBox']";
    public static readonly td = "td";
    public static readonly ul = "ul";
    public static readonly li = "li";
    public static readonly ValidationSummaryBlock = ".ValidationSummary table tr td";
    public static readonly DropDownListItem = ".DropDownListItem";
    public static readonly DropDownList=".DropDownList"
    public static readonly Value = ".Value";
    public static readonly CheckboxInput = "input[type='checkbox']";
    public static readonly DownArrow='[src="Images/ToggleIcon.png"]'


    //#region Customs settings
    public static readonly LocalCustomsInterfaceCode = "#CustomsInterfaceSetting_LocalCustomsInterfaceCode";

    //#endregion
    public static readonly HeaderScreen= ".HeaderScreen"
    public static readonly HeaderScreenLable=".HeaderScreenLable"
    public static readonly tr="tr"
    public static readonly HeaderScreenValue=".HeaderScreenValue"
    public static readonly RowCellClass=".row-cell"
    public static readonly RowClass='.Row'
    public static readonly TextTrimming='.TextTrimming'
    public static readonly SimpleGridViewRow= ".SimpleGridViewRow"
    public static readonly EditPng='[src="./Images/Buttons/Edit.png"]'
    public static readonly GridViewCell='.GridViewCell'
    public static readonly HyperlinkButtonControl='.HyperlinkButtonControl'
    public static readonly ColorGreenClass=".ColorGreen"
    public static readonly buttonspan= "button span"
    public static readonly FillParentClass=".FillParent"
    public static readonly ConfirmWindow="[class=ConfirmWindow]"
    public static readonly CurvedEditArea=".CurvedEditArea"
    public static readonly ListDataLoaded='div[id=ListDataLoaded]'
    public static readonly ErrorPopUp = ".ErrorPopUp";
   

    public static readonly ShortTitleDiv = ".ShortTitleDiv";

    public static readonly EventNotes = "#EventNotes";
    public static readonly ConfirmWindowButton = "button[id^='ConfirmWindow_Yes']";
    public static readonly EventItemBox = ".EventItemBox";
    public static readonly textarea = "textarea";
   
    public static readonly QueryListToggleButton = "querylist .ToggleButton";
    public static readonly QueryListToggleButtonItem = "querylist .ToggleButton .ToggleButtonMenu li";
    public static readonly SearchTextboxInput = "searchtextbox input";
    public static readonly MenuButtons = "#MenuButtons";
    public static readonly ComboBox = ".ComboBox";
    public static readonly ToggleIconImage = "img[src='Images/ToggleIcon.png']";
    public static readonly SelectedComboboxItem = ".ComboBoxDropdown li[class='SelectedComboboxItem']";

    //#region Warehouse
    public static readonly WarehouseStoragePricingStepFrom ="#WarehouseStoragePricing_StepFrom"
    public static readonly WarehouseStoragePricingDays ="#WarehouseStoragePricing_Days"
    public static readonly WarehouseStoragePricingStepTo ="#WarehouseStoragePricing_StepTo"
    public static readonly WarehouseStoragePricingSalePrice ="#WarehouseStoragePricing_SalePrice"
    public static readonly DaysColumn ="1"
    public static readonly StepToColumn ="2"
    public static readonly StepFromColumn ="0"
    public static readonly SalePriceColumn ="3"
    public static readonly RowNo0 ="0"
    public static readonly RowNo1 ="1"
    public static readonly ColNo1 ="1"
    public static readonly ColNo8 ="8"
    public static readonly ColNo5 ="5"
    public static readonly WarehouseTypeCode ="#Warehouse_TypeCode"
    public static readonly WarehouseChargeStorage ="#Warehouse_ChargeStorage"
    public static readonly WarehouseCurrency ="#Warehouse_CurrencyId"
    public static readonly WarehouseStorageFreeDays ="#Warehouse_StorageFreeDays"
    public static readonly WarehouseStorageDefaults ="Storage Defaults"
    public static readonly WarehouseSaveCloseBtn ="#Warehouse-SaveClose"

    public static readonly RightBorderRadius = ".RightBorderRadius:last";
    public static readonly TabSummaryValue = ".LogitudeTabSummary:last .TabSummaryValue";
    //#region  colors
    public static readonly RedColor="rgb(255, 0, 0)"
//#endregion
}