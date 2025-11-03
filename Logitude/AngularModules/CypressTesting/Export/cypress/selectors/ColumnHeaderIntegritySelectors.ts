/**
 * Selectors for Bug 118310 - Column Header Integrity Testing
 * Defines CSS selectors for Export and Import declaration grids and column headers
 */

export class ColumnHeaderIntegritySelectors {
    
    // ==================== EXPORT DECLARATION SELECTORS ====================
    
    /** Main Export declaration container */
    public static readonly ExportDeclarationContainer = "#CustomsMHExportDeclaration";
    
    /** Export declaration grid/list */
    public static readonly ExportDeclarationGrid = ".LogListComponent";
    
    /** Export declaration new button */
    public static readonly NewExportDeclarationButton = "#NewButton_CustomsDeclaration";
    
    /** Export declaration save button */
    public static readonly ExportDeclarationSaveButton = "#CustomsDeclaration-Save";
    
    /** Export declaration OK button (for new declaration) */
    public static readonly ExportDeclarationOkButton = "#NewExportDeclarationOkButton";
    
    
    // ==================== IMPORT DECLARATION SELECTORS ====================
    
    /** Main Import declaration container */
    public static readonly ImportDeclarationContainer = "#CustomsMHImportDeclaration";
    
    /** Import declaration grid/list */
    public static readonly ImportDeclarationGrid = ".LogListComponent";
    
    /** Import declaration new button */
    public static readonly NewImportDeclarationButton = "#NewButton_CustomsDeclaration";
    
    /** Import declaration save button */
    public static readonly ImportDeclarationSaveButton = "#CustomsDeclaration-Save";
    
    /** Import declaration OK button (for new declaration) */
    public static readonly ImportDeclarationOkButton = "#NewImportDeclarationOkButton";
    
    
    // ==================== COLUMN HEADER SELECTORS ====================
    
    /** All column headers in any grid */
    public static readonly AllColumnHeaders = ".LogListComponent thead th";
    
    /** Column header cells */
    public static readonly ColumnHeaderCells = ".LogListComponent thead th div";
    
    /** Column header text elements */
    public static readonly ColumnHeaderText = ".LogListComponent thead th div span";
    
    /** Specific column header by index */
    public static GetColumnHeaderByIndex(index: number): string {
        return `.LogListComponent thead th:nth-child(${index})`;
    }
    
    /** Specific column header by text content (partial match) */
    public static GetColumnHeaderByText(text: string): string {
        return `.LogListComponent thead th:contains("${text}")`;
    }
    
    
    // ==================== CRITICAL COLUMN HEADERS ====================
    
    /** Certificate of Origin column header - תעודת מקור */
    public static readonly CertificateOfOriginColumnHeader = ".LogListComponent thead th:contains('תעודת מקור')";
    
    /** Exporter/Importer column header - יצואן/יבואן */
    public static readonly ExporterImporterColumnHeader = ".LogListComponent thead th";
    
    /** Status column header - סטטוס */
    public static readonly StatusColumnHeader = ".LogListComponent thead th:contains('סטטוס')";
    
    /** Customs Office column header - בית מכס */
    public static readonly CustomsOfficeColumnHeader = ".LogListComponent thead th:contains('בית מכס')";
    
    
    // ==================== NAVIGATION & REFRESH SELECTORS ====================
    
    /** Refresh button in declaration view */
    public static readonly RefreshButton = "#RefreshButton, .refresh-button, button[title='Refresh']";
    
    /** Declaration list/workspace */
    public static readonly DeclarationList = ".EntityListComponent";
    
    /** First declaration row in list */
    public static readonly FirstDeclarationRow = ".Row:first";
    
    /** Declaration row by index (0-based with eq()) */
    public static GetDeclarationRowByIndex(index: number): string {
        return `.Row:eq(${index})`;
    }
    
    /** List data loaded indicator */
    public static readonly ListDataLoaded = 'div[id=ListDataLoaded]';
    
    
    // ==================== WORKSPACE NAVIGATION ====================
    
    /** Export workspace menu item */
    public static readonly ExportWorkspaceMenu = "#CustomsMHExportDeclaration";
    
    /** Import workspace menu item */
    public static readonly ImportWorkspaceMenu = "#GeneralMHDeclarations";
    
    /** Customs menu */
    public static readonly CustomsMenu = "#CustomsMenu";
    
    
    // ==================== FORM FIELDS (FOR DECLARATION CREATION) ====================
    
    /** Export file number field */
    public static readonly ExportFileNumber = "#textboxdiv_Customs\\.Declaration_ExportFile";
    
    /** Import file number field */
    public static readonly ImportFileNumber = "#textboxdiv_Customs\\.Declaration_CustomFileNo";
    
    /** Customer field */
    public static readonly CustomerField = "#Customs\\.Declaration_CustomerId";
    
    /** Direction field */
    public static readonly DirectionField = "#Customs\\.Declaration_Direction";
    
    
    // ==================== HELPER METHODS ====================
    
    /**
     * Get selector for a specific column header by exact text match
     */
    public static GetExactColumnHeaderByText(exactText: string): string {
        return `.LogListComponent thead th div span:contains("${exactText}")`;
    }
    
    /**
     * Get all grid rows
     */
    public static readonly AllGridRows = ".LogListComponent tbody tr";
    
    /**
     * Get grid cell by row and column index
     */
    public static GetGridCell(rowIndex: number, colIndex: number): string {
        return `.LogListComponent tbody tr:nth-child(${rowIndex}) td:nth-child(${colIndex})`;
    }
}

