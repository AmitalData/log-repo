/**
 * Selectors for Bug 117820 - Tenant Parameter Validation Testing
 * Defines CSS selectors for Export operations and menus that need tenant validation
 */

export class TenantParameterValidationSelectors {
    
    // ==================== EXPORT WORKSPACE NAVIGATION ====================
    
    /** Export workspace main menu */
    public static readonly ExportWorkspaceMenu = "#CustomsMHExportDeclaration";
    
    /** Import workspace main menu */
    public static readonly ImportWorkspaceMenu = "#GeneralMHDeclarations";
    
    /** Export declarations link/button */
    public static readonly ExportDeclarationsLink = "#CustomsMHExportDeclaration";
    
    
    // ==================== EXPORT CANCELLATION SELECTORS ====================
    
    /** Export Cancellation menu button */
    public static readonly ExportCancellationButton = "button:contains('ביטול הצהרה'), button:contains('Cancel Declaration'), #CancelDeclarationButton";
    
    /** Export Cancellation dialog window */
    public static readonly ExportCancellationDialog = ".DeclarationCancellationComponent, #DeclarationCancellationWindow";
    
    /** Cancellation reason dropdown */
    public static readonly CancellationReasonField = "#Customs\\.Declaration_CancelRequestReasonCode";
    
    /** Cancellation explanation text field */
    public static readonly CancellationExplanationField = "#Customs\\.Declaration_CancelRequestReasonExplanation";
    
    /** Send cancellation button */
    public static readonly SendCancellationButton = "button:contains('שלח'), button:contains('Send'), #SendCancellationButton";
    
    
    // ==================== EXPORT STORAGE DECLARATION SELECTORS ====================
    
    /** Export Storage Declaration menu item */
    public static readonly ExportStorageDeclarationMenu = "button:contains('הצהרת אחסנה'), button:contains('Storage Declaration'), #ExportStorageButton";
    
    /** Export Storage Declaration window */
    public static readonly ExportStorageDeclarationWindow = ".ExportStorageDeclerationComponent, #ExportStorageWindow";
    
    /** Storage declaration list/grid */
    public static readonly StorageDeclarationGrid = ".LogListComponent.storage-declarations";
    
    
    // ==================== EXPORT CLOSURE SELECTORS ====================
    
    /** Declaration Closure menu button */
    public static readonly DeclarationClosureButton = "button:contains('סגירת הצהרה'), button:contains('Declaration Closure'), #ClosureButton";
    
    /** Operational Closure button */
    public static readonly OperationalClosureButton = "button:contains('סגירה תפעולית'), button:contains('Operational Closure'), #OperationalClosureButton";
    
    /** Cancel Closure button */
    public static readonly CancelClosureButton = "button:contains('ביטול סגירה'), button:contains('Cancel Closure'), #CancelClosureButton";
    
    /** Closure dialog/window */
    public static readonly ClosureDialog = ".closure-dialog, #ClosureWindow";
    
    
    // ==================== EXPORT DECLARATION LIST SELECTORS ====================
    
    /** Export declarations grid/list */
    public static readonly ExportDeclarationsList = ".LogListComponent, .EntityListComponent";
    
    /** First declaration in list */
    public static readonly FirstDeclarationInList = ".Row:first";
    
    /** Declaration row by index (0-based with eq()) */
    public static GetDeclarationRow(index: number): string {
        return `.Row:eq(${index})`;
    }
    
    /** List data loaded indicator */
    public static readonly ListDataLoaded = 'div[id=ListDataLoaded]';
    
    /** New Export Declaration button */
    public static readonly NewExportDeclarationButton = "#NewButton_CustomsDeclaration";
    
    
    // ==================== FILTER CONTROLS ====================
    
    /** Filter panel/section */
    public static readonly FilterPanel = ".filter-panel, .LogitudeFilter";
    
    /** Search input field */
    public static readonly SearchInput = "input[type='search'], .search-input, #SearchField";
    
    /** Apply filters button */
    public static readonly ApplyFiltersButton = "button:contains('סנן'), button:contains('Filter'), #ApplyFiltersButton";
    
    /** Refresh list button */
    public static readonly RefreshListButton = "button:contains('רענן'), button:contains('Refresh'), #RefreshButton";
    
    /** Filter dropdown/select */
    public static readonly FilterDropdown = ".filter-dropdown, select.filter";
    
    /** Clear filters button */
    public static readonly ClearFiltersButton = "button:contains('נקה'), button:contains('Clear'), #ClearFiltersButton";
    
    
    // ==================== DECLARATION EDIT/VIEW ====================
    
    /** Declaration edit component */
    public static readonly DeclarationEditComponent = ".CustomsDeclarationComponent, #DeclarationEdit";
    
    /** Declaration tabs */
    public static readonly DeclarationTabs = ".declaration-tabs, .tabs-container";
    
    /** Save declaration button */
    public static readonly SaveDeclarationButton = "#CustomsDeclaration-Save";
    
    /** Declaration menu buttons container */
    public static readonly DeclarationMenuButtons = ".menu-buttons, .action-buttons";
    
    
    // ==================== CUSTOMS REQUESTS ====================
    
    /** Customs Requests menu */
    public static readonly CustomsRequestsMenu = "button:contains('בקשות מכס'), button:contains('Customs Requests')";
    
    /** Customs Requests window */
    public static readonly CustomsRequestsWindow = ".customs-requests-window";
    
    
    // ==================== GENERAL UI ELEMENTS ====================
    
    /** Busy indicator/spinner */
    public static readonly BusyIndicator = ".busy-indicator, .spinner, .loading";
    
    /** Success message */
    public static readonly SuccessMessage = ".success-message, .message-success";
    
    /** Error message */
    public static readonly ErrorMessage = ".error-message, .message-error";
    
    /** Confirmation dialog */
    public static readonly ConfirmDialog = ".confirm-dialog, .confirmation-window";
    
    /** OK button in dialogs */
    public static readonly OkButton = "button:contains('אישור'), button:contains('OK'), .ok-button";
    
    /** Cancel button in dialogs */
    public static readonly CancelButton = "button:contains('ביטול'), button:contains('Cancel'), .cancel-button";
    
    
    // ==================== HELPER METHODS ====================
    
    /**
     * Get menu button by text content
     */
    public static GetMenuButtonByText(text: string): string {
        return `button:contains('${text}'), a:contains('${text}')`;
    }
    
    /**
     * Get dialog by title
     */
    public static GetDialogByTitle(title: string): string {
        return `.LogitudeWindow[title='${title}'], .window-title:contains('${title}')`;
    }
    
    /**
     * Get any element containing specific text
     */
    public static GetElementByText(text: string): string {
        return `*:contains('${text}')`;
    }
}

