"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TenantManagementJS = /** @class */ (function () {
    function TenantManagementJS() {
        this.PackagesCodes_PK = [];
        this.PackagesCodes_BS = [];
    }
    Object.defineProperty(TenantManagementJS.prototype, "TenantManagementLicenses", {
        get: function () {
            if (this.tenantManagementLicenses == null) {
                this.tenantManagementLicenses = [];
            }
            return this.tenantManagementLicenses;
        },
        set: function (newValue) {
            if (this.tenantManagementLicenses != newValue) {
                this.tenantManagementLicenses = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return TenantManagementJS;
}());
exports.TenantManagementJS = TenantManagementJS;
//# sourceMappingURL=TenantManagementJS.js.map