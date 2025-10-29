export class PaymentBlockingSelectors {
    // Navigation and filtering
    public static ImportDeclarationsMenu = "#CustomsMHExportDeclaration"; // Using same as export for now
    public static DeclarationStatusFilter = "select[name*='status'], [id*='status'], .status-filter";
    public static SearchResultsGrid = "[id*='LogGrid'], .grid-container";
    public static FirstResultRow = "[id*='LogGrid'][id*='row0']:first, [id*='LogGrid'] [id*='row']:first, .grid-row:first";
    public static StatusDropdown = ".status-dropdown, select[title*='סטטוס']";
    
    // File search
    public static FileNumberSearch = "searchtextbox input[id*=SearchFieldsId]";
    public static SearchButton = "button[title*='Search'], .search-button";
    public static FileNumberResult = "[id*='LogGrid'] [id*='row']:first";
    
    // Tab navigation
    public static GeneralTab = "a[href*='general'], .tab-general, [title*='כללי'], li:contains('כללי'), button:contains('כללי')";
    public static TabContainer = ".tab-container, .tabs";
    
    // IsChanged field reset
    public static SendDropdownArrow = "button[title*='שלח'] .dropdown-arrow, .send-button .arrow, button[title*='שלח'], [id*='Send'][title*='שלח']";
    public static ScenarioMenu = ".scenario-menu, .dropdown-menu";
    public static ScenarioOption = "li[data-scenario*='הצהרת יבוא תקינה'], .scenario-option";
    public static ConfirmButton = "button[title*='אישור'], .confirm-button";
    
    // System messages
    public static SystemMessage = ".system-message, .red-message, .warning-message";
    public static MessageCloseButton = ".message-close, .close-button";
    public static ValidationSuccessMessage = ".success-message, .validation-success";
    
    // Field changes
    public static CustomsOfficeField = "select[name*='customsOffice'], [id*='customsOffice']";
    public static GoodsDescriptionField = "textarea[name*='description'], [id*='description']";
    public static CargoQuantityField = "input[name*='quantity'], [id*='quantity']";
    public static CargoSerialSection = ".cargo-serial-section, [id*='cargoSerial']";
    
    // Buttons
    public static SaveButton = "button[title*='שמור'], .save-button";
    public static PaymentSubmissionButton = "button[title*='הגשת תשלום'], button[title*='תשלום'], .payment-submission-button, button:contains('הגשת תשלום'), button:contains('תשלום')";
    public static CancelButton = "button[title*='ביטול'], .cancel-button, [id*='Cancel'], button:contains('ביטול')";
    
    // Payment submission screen
    public static PaymentScreen = ".payment-submission-screen, [id*='payment']";
    public static SendPaymentButton = "button[title*='שלח']:disabled, .send-payment-button:disabled";
    public static WarningMessage = ".warning-message, .yellow-message";
    public static DeclarationChangesWarning = "[data-message*='שינויים בהצהרה'], .declaration-changes-warning";
}
