/**
 * Model for Bug 118310 - Column Header Integrity Testing
 * Used to define expected column headers and validation criteria for Import/Export declarations
 */

export interface ColumnHeaderIntegrityDetails {
    DeclarationType: 'Import' | 'Export';
    ExpectedHeaders: string[];
    CriticalHeaders: CriticalHeader[];
}

export interface CriticalHeader {
    name: string;
    expectedText: string;
}

/**
 * Standard Export Declaration Headers
 */
export const ExportDeclarationHeaders: string[] = [
    'תעודת מקור',        // Certificate of Origin
    'יצואן',            // Exporter
    'מס\' תיק יצוא',    // Export File Number
    'תאריך פתיחה',       // Opening Date
    'בית מכס',          // Customs Office
    'סטטוס',            // Status
    'מספר הצהרה',        // Declaration Number
    'שם לקוח',          // Customer Name
];

/**
 * Standard Import Declaration Headers
 */
export const ImportDeclarationHeaders: string[] = [
    'תעודת מקור',        // Certificate of Origin
    'יבואן',            // Importer
    'מס\' תיק',         // File Number
    'תאריך פתיחה',       // Opening Date
    'בית מכס',          // Customs Office
    'סטטוס',            // Status
    'מספר הצהרה',        // Declaration Number
    'שם לקוח',          // Customer Name
];

/**
 * Critical headers that must be tested for corruption
 */
export const CriticalHeadersToTest: CriticalHeader[] = [
    { name: 'תעודת מקור', expectedText: 'תעודת מקור' },
    { name: 'יצואן', expectedText: 'יצואן' },
    { name: 'בית מכס', expectedText: 'בית מכס' },
    { name: 'סטטוס', expectedText: 'סטטוס' },
];

/**
 * Base64 pattern to detect encoded strings in headers
 */
export const BASE64_PATTERN = /^[A-Za-z0-9+/=]{4,}$/;

/**
 * Helper function to check if a string is Base64 encoded
 */
export function isBase64Encoded(str: string): boolean {
    if (!str || str.length < 4) return false;
    return BASE64_PATTERN.test(str.trim());
}

/**
 * Helper function to validate Hebrew text (not corrupted)
 */
export function isValidHebrewText(str: string): boolean {
    if (!str) return false;
    // Check if string contains Hebrew characters or common English text
    const hebrewPattern = /[\u0590-\u05FF]/;
    const englishPattern = /^[a-zA-Z\s\'\-]+$/;
    return hebrewPattern.test(str) || englishPattern.test(str);
}

