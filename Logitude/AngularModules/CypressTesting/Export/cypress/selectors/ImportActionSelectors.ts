/**
 * Import Action Selectors
 * Contains all selectors related to opening Declaration Form from Import Declarations
 */

import { SearchSelectors } from './SearchSelectors';

export class ImportActionSelectors {
    // Grid and first row selection (reuse from SearchSelectors)
    public static readonly GridRows = SearchSelectors.GridRows;
    public static readonly FirstRow = SearchSelectors.FirstRow;
    
    // Forms dropdown button (top left) - "טפסים"
    public static readonly FormsDropdown = "menubuttonscomponent td:nth-child(7) > div, #MenuButtons";
    
    // Declaration Form menu item - "טופס הצהרה" (appears after clicking Forms dropdown)
    // Note: Selector will be filtered in action to match exactly "טופס הצהרה" and not "בקשה לטופס הצהרה"
    public static readonly DeclarationForm = "#CustomsDeclarationBPrintDeclarationForm > div, .QueryLink, button";
    
    // Popup dialog
    public static readonly PopupDialog = ".LogitudeWindow, .MessageWindow";
    
    // OK/Confirm button in popup
    public static readonly PopupConfirmButton = "button:contains('אישור'), .RedButton:contains('אישור'), button[id^='MessageWindow_Ok'], button[id^='ConfirmWindow_Yes']";
}

