/**
 * Search Selectors for Import Declarations
 * Contains all selectors related to searching and filtering declarations
 */

export class SearchSelectors {
    // Search field and controls
    public static readonly SearchField = "searchtextbox input[id*=SearchFieldsId], input[placeholder*='תיקים'], input[placeholder*='חיפוש']";
    public static readonly ClearSearchButton = "searchtextbox button.ClearButton, button[title*='נקה'], .search-clear, button:has(> [class*='Clear'])";
    public static readonly SearchButton = "button[title*='חפש'], .search-button";
    
    // Grid and results
    public static readonly GridContainer = "[id*='LogGrid'], .grid-container";
    public static readonly GridRows = "[id*='LogGrid'][id*='row'], [id*='LogGrid'] tr[id*='row']";
    public static readonly FirstRow = "[id*='LogGrid'][id*='row0']:first";
    
    // Column selectors - based on actual grid structure from screenshot
    // Columns from right to left: תאריך חישוב מיסים | תיק עמלות | לקוח | בית מכס | מספר הצהרה | יבוא מכסי | סוג המארז
    public static readonly FileNumberColumn = "[id*='LogGrid'] [id*='row']"; // File number appears in row data
    public static readonly CustomerColumn = "[id*='LogGrid'] [id*='row']"; // Customer column (לקוח)
    public static readonly DeclarationNumberColumn = "[id*='LogGrid'] [id*='row']"; // Declaration number (מספר הצהרה)
    public static readonly CargoColumn = "[id*='LogGrid'] [id*='row']"; // Cargo/Package type column
    
    // No results message
    public static readonly NoResultsMessage = ".no-results, .empty-grid, [id*='NoResults']";
}

