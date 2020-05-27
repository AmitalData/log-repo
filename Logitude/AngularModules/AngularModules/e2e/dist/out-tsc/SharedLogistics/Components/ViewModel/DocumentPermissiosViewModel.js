"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var DocumentPermissiosViewModel = /** @class */ (function () {
    function DocumentPermissiosViewModel(zeroDocumentType, currentDocument) {
        this.entityPM_TenantZero = zeroDocumentType;
        this.entityPM = currentDocument;
        this.CustomerChooseCheckedBoxId = Guid_1.Guid.newGuid();
        this.CustomerSuggestedCheckedBoxId = Guid_1.Guid.newGuid();
    }
    Object.defineProperty(DocumentPermissiosViewModel.prototype, "DocumentTypeName", {
        get: function () { return this.entityPM_TenantZero ? this.entityPM_TenantZero.Name : this.entityPM.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentPermissiosViewModel.prototype, "CustomerSuggestedIsChecked", {
        get: function () { return this.entityPM_TenantZero ? this.entityPM_TenantZero.IsCustomerView : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentPermissiosViewModel.prototype, "AgentSuggestedIsChecked", {
        get: function () { return this.entityPM_TenantZero ? this.entityPM_TenantZero.IsAgentView : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentPermissiosViewModel.prototype, "CustomerChooseIsChecked", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.IsCustomerView;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.IsCustomerView = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentPermissiosViewModel.prototype, "AgentChooseIsChecked", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.IsAgentView;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.IsAgentView = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return DocumentPermissiosViewModel;
}());
exports.DocumentPermissiosViewModel = DocumentPermissiosViewModel;
//# sourceMappingURL=DocumentPermissiosViewModel.js.map