/**
 * Models for Bug 117820 - Tenant Parameter Validation Testing
 * Used to track and validate network requests for tenant parameter presence
 */

/**
 * Represents a captured network request with tenant validation info
 */
export interface NetworkRequest {
    /** Full URL of the request */
    url: string;
    
    /** HTTP method (GET, POST, PUT, DELETE, etc.) */
    method: string;
    
    /** Request body (if any) */
    body: any;
    
    /** Request headers */
    headers: any;
    
    /** Timestamp when request was captured */
    timestamp: Date;
    
    /** Whether the request includes a tenant parameter */
    hasTenantParameter: boolean;
    
    /** The actual tenant value found (null if not present) */
    tenantValue: number | null;
    
    /** Where the tenant parameter was found (url, body, header, or null) */
    tenantLocation?: 'url' | 'body' | 'header' | null;
}

/**
 * Result of tenant validation across all captured requests
 */
export interface TenantValidationResult {
    /** Total number of API requests captured */
    totalRequests: number;
    
    /** Number of requests that include tenant parameter */
    requestsWithTenant: number;
    
    /** List of requests missing tenant parameter */
    requestsMissingTenant: NetworkRequest[];
    
    /** Whether validation passed (all requests have tenant) */
    isValid: boolean;
    
    /** Expected tenant value (from logged user) */
    expectedTenantValue?: number;
    
    /** Requests with incorrect tenant value */
    requestsWithIncorrectTenant?: NetworkRequest[];
}

/**
 * Configuration for network monitoring
 */
export interface NetworkMonitoringConfig {
    /** Whether monitoring is currently active */
    isActive: boolean;
    
    /** Captured requests storage */
    capturedRequests: NetworkRequest[];
    
    /** Expected tenant value for validation */
    expectedTenant: number;
    
    /** Endpoints to exclude from validation (login, ping, etc.) */
    excludedEndpoints: string[];
    
    /** Start time of monitoring */
    startTime: Date;
}

/**
 * Default excluded endpoints (non-DB operations and pure infrastructure only)
 * NOTE: Views and metadata endpoints (referencestatusviews, declarationviews, ngMetaData, etc.) 
 * MUST have tenant - they are NOT excluded!
 */
export const DEFAULT_EXCLUDED_ENDPOINTS: string[] = [
    '/api/Login',
    '/api/Ping',
    '/api/Version',
    '/api/Health',
    '/signalr',
    '.js',
    '.css',
    '.png',
    '.jpg',
    '.svg',
    '.woff',
    '.ttf'
];

/**
 * Critical Export-related API endpoints that MUST include tenant
 */
export const CRITICAL_EXPORT_ENDPOINTS: string[] = [
    '/api/Declaration/SendDeclarationCancellation',  // Bug 117820 - Export Cancellations!
    '/api/Declaration/getbyfilters',
    '/api/ExportStorage/getbyfilters',
    '/api/Declaration/Closure',
    '/api/Declaration/CancelClosure',
    '/api/CustomsRequest',
    'referencestatusviews',         // Reference status views
    'GetLastTableUpdateDate',       // Last update tracking
    'declarationviews',             // Declaration views
    'ngMetaData',                   // UI metadata with tenant context
];

/**
 * Helper function to check if a URL should be excluded from validation
 */
export function shouldExcludeEndpoint(url: string): boolean {
    return DEFAULT_EXCLUDED_ENDPOINTS.some(endpoint => 
        url.toLowerCase().includes(endpoint.toLowerCase())
    );
}

/**
 * Helper function to check if endpoint is Export-critical
 */
export function isCriticalExportEndpoint(url: string): boolean {
    return CRITICAL_EXPORT_ENDPOINTS.some(endpoint =>
        url.toLowerCase().includes(endpoint.toLowerCase())
    );
}

/**
 * Helper function to extract tenant from URL query parameters
 */
export function extractTenantFromUrl(url: string): number | null {
    const tenantMatch = url.match(/[&?]tenant=(\d+)/i);
    if (tenantMatch) {
        return parseInt(tenantMatch[1]);
    }
    return null;
}

/**
 * Helper function to extract tenant from request body
 */
export function extractTenantFromBody(body: any): number | null {
    if (!body) return null;
    
    // Check for direct tenant property
    if (body.tenant !== undefined && body.tenant !== null) {
        return typeof body.tenant === 'number' ? body.tenant : parseInt(body.tenant);
    }
    
    if (body.Tenant !== undefined && body.Tenant !== null) {
        return typeof body.Tenant === 'number' ? body.Tenant : parseInt(body.Tenant);
    }
    
    // Check nested objects (like filters)
    if (body.filters && body.filters.Tenant !== undefined) {
        return body.filters.Tenant;
    }
    
    return null;
}

/**
 * Helper function to extract tenant from request headers
 */
export function extractTenantFromHeaders(headers: any): number | null {
    if (!headers) return null;
    
    // Check various header formats
    if (headers['X-Tenant']) {
        return parseInt(headers['X-Tenant']);
    }
    
    if (headers['x-tenant']) {
        return parseInt(headers['x-tenant']);
    }
    
    if (headers['Tenant']) {
        return parseInt(headers['Tenant']);
    }
    
    if (headers['tenant']) {
        return parseInt(headers['tenant']);
    }
    
    return null;
}

/**
 * Validates a network request for tenant parameter presence
 */
export function validateNetworkRequest(request: NetworkRequest): NetworkRequest {
    let tenantValue: number | null = null;
    let tenantLocation: 'url' | 'body' | 'header' | null = null;
    
    // Check URL
    tenantValue = extractTenantFromUrl(request.url);
    if (tenantValue !== null) {
        tenantLocation = 'url';
    }
    
    // Check body (if URL didn't have it)
    if (tenantValue === null) {
        tenantValue = extractTenantFromBody(request.body);
        if (tenantValue !== null) {
            tenantLocation = 'body';
        }
    }
    
    // Check headers (if neither URL nor body had it)
    if (tenantValue === null) {
        tenantValue = extractTenantFromHeaders(request.headers);
        if (tenantValue !== null) {
            tenantLocation = 'header';
        }
    }
    
    // Update request with validation results
    request.hasTenantParameter = tenantValue !== null && tenantValue > 0;
    request.tenantValue = tenantValue;
    request.tenantLocation = tenantLocation;
    
    return request;
}

/**
 * Formats a network request for error reporting
 */
export function formatRequestForReport(request: NetworkRequest): string {
    const lines: string[] = [];
    lines.push(`${request.method} ${request.url}`);
    
    if (request.body) {
        lines.push(`  Body: ${JSON.stringify(request.body, null, 2)}`);
    }
    
    lines.push(`  Tenant Parameter: ${request.hasTenantParameter ? '✓ Present' : '✗ MISSING'}`);
    
    if (request.tenantValue !== null) {
        lines.push(`  Tenant Value: ${request.tenantValue}`);
        lines.push(`  Found In: ${request.tenantLocation}`);
    }
    
    return lines.join('\n');
}

