"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var APInvoiceMultipleShortPM = /** @class */ (function () {
    function APInvoiceMultipleShortPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.isMultipleEntities = true;
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "ShipmentId", {
        get: function () { return this.shipmentId; },
        set: function (newValue) { if (this.shipmentId != newValue) {
            this.shipmentId = newValue;
            this.MarkAsDirty("ShipmentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "StatusCode", {
        get: function () { return this.statusCode; },
        set: function (newValue) { if (this.statusCode != newValue) {
            this.statusCode = newValue;
            this.MarkAsDirty("StatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "VendorId", {
        get: function () { return this.vendorId; },
        set: function (newValue) { if (this.vendorId != newValue) {
            this.vendorId = newValue;
            this.MarkAsDirty("VendorId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "ProfitCurrencyId", {
        get: function () { return this.profitCurrencyId; },
        set: function (newValue) { if (this.profitCurrencyId != newValue) {
            this.profitCurrencyId = newValue;
            this.MarkAsDirty("ProfitCurrencyId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "InvoiceCurrencyId", {
        get: function () { return this.invoiceCurrencyId; },
        set: function (newValue) { if (this.invoiceCurrencyId != newValue) {
            this.invoiceCurrencyId = newValue;
            this.MarkAsDirty("InvoiceCurrencyId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "InvoiceCurrencyExchangeRate", {
        get: function () { return this.invoiceCurrencyExchangeRate; },
        set: function (newValue) { if (this.invoiceCurrencyExchangeRate != newValue) {
            this.invoiceCurrencyExchangeRate = newValue;
            this.MarkAsDirty("InvoiceCurrencyExchangeRate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.profitCurrencyExchangeRate; },
        set: function (newValue) { if (this.profitCurrencyExchangeRate != newValue) {
            this.profitCurrencyExchangeRate = newValue;
            this.MarkAsDirty("ProfitCurrencyExchangeRate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "SubTotalInLocalCurrency", {
        get: function () { return this.subTotalInLocalCurrency; },
        set: function (newValue) { if (this.subTotalInLocalCurrency != newValue) {
            this.subTotalInLocalCurrency = newValue;
            this.MarkAsDirty("SubTotalInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "SubTotalInInvoiceCurrency", {
        get: function () { return this.subTotalInInvoiceCurrency; },
        set: function (newValue) { if (this.subTotalInInvoiceCurrency != newValue) {
            this.subTotalInInvoiceCurrency = newValue;
            this.MarkAsDirty("SubTotalInInvoiceCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "IsMultipleEntities", {
        get: function () { return this.isMultipleEntities; },
        set: function (newValue) { if (this.isMultipleEntities != newValue) {
            this.isMultipleEntities = newValue;
            this.MarkAsDirty("IsMultipleEntities");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShortPM.prototype, "InvoiceLines", {
        get: function () {
            if (this.invoiceLines == null) {
                this.invoiceLines = [];
            }
            return this.invoiceLines;
        },
        set: function (newValue) {
            if (this.invoiceLines != newValue) {
                this.invoiceLines = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceMultipleShortPM.prototype.AddInvoiceLinePM = function (item) {
        if (item != null) {
            var index = this.InvoiceLines.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.InvoiceLines.push(item);
                this.MarkAsDirty();
            }
        }
    };
    APInvoiceMultipleShortPM.prototype.RemoveInvoiceLinePM = function (item) {
        if (item != null) {
            var index = this.InvoiceLines.indexOf(item);
            if (index > -1) {
                this.InvoiceLines.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    APInvoiceMultipleShortPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "APInvoiceMultipleShort");
        }
    };
    APInvoiceMultipleShortPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    APInvoiceMultipleShortPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], APInvoiceMultipleShortPM.prototype, "PropertyChanged", void 0);
    return APInvoiceMultipleShortPM;
}());
exports.APInvoiceMultipleShortPM = APInvoiceMultipleShortPM;
var APInvoiceLineShortPM = /** @class */ (function () {
    function APInvoiceLineShortPM(_entityParentPM) {
        this.PropertyChanged = new core_1.EventEmitter();
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(APInvoiceLineShortPM.prototype, "APInvoiceId", {
        get: function () { return this.aPInvoiceId; },
        set: function (newValue) { if (this.aPInvoiceId != newValue) {
            this.aPInvoiceId = newValue;
            this.MarkAsDirty("APInvoiceId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "LineNumber", {
        get: function () { return this.lineNumber; },
        set: function (newValue) { if (this.lineNumber != newValue) {
            this.lineNumber = newValue;
            this.MarkAsDirty("LineNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "InvoiceCurrencyAmount", {
        get: function () { return this.invoiceCurrencyAmount; },
        set: function (newValue) { if (this.invoiceCurrencyAmount != newValue) {
            this.invoiceCurrencyAmount = newValue;
            this.MarkAsDirty("InvoiceCurrencyAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (newValue) { if (this.notes != newValue) {
            this.notes = newValue;
            this.MarkAsDirty("Notes");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ChargesTypeId", {
        get: function () { return this.chargesTypeId; },
        set: function (newValue) { if (this.chargesTypeId != newValue) {
            this.chargesTypeId = newValue;
            this.MarkAsDirty("ChargesTypeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (newValue) { if (this.vatTypeId != newValue) {
            this.vatTypeId = newValue;
            this.MarkAsDirty("VatTypeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "VendorId", {
        get: function () { return this.vendorId; },
        set: function (newValue) { if (this.vendorId != newValue) {
            this.vendorId = newValue;
            this.MarkAsDirty("VendorId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "OpenAmount", {
        get: function () { return this.openAmount; },
        set: function (newValue) { if (this.openAmount != newValue) {
            this.openAmount = newValue;
            this.MarkAsDirty("OpenAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ForiegnCurrencyId", {
        get: function () { return this.foriegnCurrencyId; },
        set: function (newValue) { if (this.foriegnCurrencyId != newValue) {
            this.foriegnCurrencyId = newValue;
            this.MarkAsDirty("ForiegnCurrencyId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ForiegnCurrencyAmount", {
        get: function () { return this.foriegnCurrencyAmount; },
        set: function (newValue) { if (this.foriegnCurrencyAmount != newValue) {
            this.foriegnCurrencyAmount = newValue;
            this.MarkAsDirty("ForiegnCurrencyAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ForiegnExchangeRate", {
        get: function () { return this.foriegnExchangeRate; },
        set: function (newValue) { if (this.foriegnExchangeRate != newValue) {
            this.foriegnExchangeRate = newValue;
            this.MarkAsDirty("ForiegnExchangeRate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "LocalCurrencyAmount", {
        get: function () { return this.localCurrencyAmount; },
        set: function (newValue) { if (this.localCurrencyAmount != newValue) {
            this.localCurrencyAmount = newValue;
            this.MarkAsDirty("LocalCurrencyAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ProfitCurrencyAmount", {
        get: function () { return this.profitCurrencyAmount; },
        set: function (newValue) { if (this.profitCurrencyAmount != newValue) {
            this.profitCurrencyAmount = newValue;
            this.MarkAsDirty("ProfitCurrencyAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "EntityId", {
        get: function () { return this.entityId; },
        set: function (newValue) { if (this.entityId != newValue) {
            this.entityId = newValue;
            this.MarkAsDirty("EntityId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "EntityPayableId", {
        get: function () { return this.entityPayableId; },
        set: function (newValue) { if (this.entityPayableId != newValue) {
            this.entityPayableId = newValue;
            this.MarkAsDirty("EntityPayableId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "RefundAmount", {
        get: function () { return this.refundAmount; },
        set: function (newValue) { if (this.refundAmount != newValue) {
            this.refundAmount = newValue;
            this.MarkAsDirty("RefundAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ChargesTypeCode", {
        get: function () { return this.chargesTypeCode; },
        set: function (newValue) { if (this.chargesTypeCode != newValue) {
            this.chargesTypeCode = newValue;
            this.MarkAsDirty("ChargesTypeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ChargesTypeName", {
        get: function () { return this.chargesTypeName; },
        set: function (newValue) { if (this.chargesTypeName != newValue) {
            this.chargesTypeName = newValue;
            this.MarkAsDirty("ChargesTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "VatTypeName", {
        get: function () { return this.vatTypeName; },
        set: function (newValue) { if (this.vatTypeName != newValue) {
            this.vatTypeName = newValue;
            this.MarkAsDirty("VatTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { if (this.objectTableId != newValue) {
            this.objectTableId = newValue;
            this.MarkAsDirty("ObjectTableId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "EntityReference", {
        get: function () { return this.entityReference; },
        set: function (newValue) { if (this.entityReference != newValue) {
            this.entityReference = newValue;
            this.MarkAsDirty("EntityReference");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "VendorName", {
        get: function () { return this.vendorName; },
        set: function (newValue) { if (this.vendorName != newValue) {
            this.vendorName = newValue;
            this.MarkAsDirty("VendorName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "OtherInvoicesAmounts", {
        get: function () { return this.otherInvoicesAmounts; },
        set: function (newValue) { if (this.otherInvoicesAmounts != newValue) {
            this.otherInvoicesAmounts = newValue;
            this.MarkAsDirty("OtherInvoicesAmounts");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ExpectedAmount", {
        get: function () { return this.expectedAmount; },
        set: function (newValue) { if (this.expectedAmount != newValue) {
            this.expectedAmount = newValue;
            this.MarkAsDirty("ExpectedAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "CorrectionAmount", {
        get: function () { return this.correctionAmount; },
        set: function (newValue) { if (this.correctionAmount != newValue) {
            this.correctionAmount = newValue;
            this.MarkAsDirty("CorrectionAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "CorrectionNote", {
        get: function () { return this.correctionNote; },
        set: function (newValue) { if (this.correctionNote != newValue) {
            this.correctionNote = newValue;
            this.MarkAsDirty("CorrectionNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "CorrectionByUserId", {
        get: function () { return this.correctionByUserId; },
        set: function (newValue) { if (this.correctionByUserId != newValue) {
            this.correctionByUserId = newValue;
            this.MarkAsDirty("CorrectionByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "CorrectionDate", {
        get: function () { return this.correctionDate; },
        set: function (newValue) { if (this.correctionDate != newValue) {
            this.correctionDate = newValue;
            this.MarkAsDirty("CorrectionDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "AmountTypeCode", {
        get: function () { return this.amountTypeCode; },
        set: function (newValue) { if (this.amountTypeCode != newValue) {
            this.amountTypeCode = newValue;
            this.MarkAsDirty("AmountTypeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "VatPercentage", {
        get: function () { return this.vatPercentage; },
        set: function (newValue) { if (this.vatPercentage != newValue) {
            this.vatPercentage = newValue;
            this.MarkAsDirty("VatPercentage");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ForiegnCurrencyCode", {
        get: function () { return this.foriegnCurrencyCode; },
        set: function (newValue) { if (this.foriegnCurrencyCode != newValue) {
            this.foriegnCurrencyCode = newValue;
            this.MarkAsDirty("ForiegnCurrencyCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { if (this.changeSetOp != newValue) {
            this.changeSetOp = newValue;
            this.MarkAsDirty("ChangeSetOp");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "DebitAccount", {
        get: function () { return this.debitAccount; },
        set: function (newValue) { if (this.debitAccount != newValue) {
            this.debitAccount = newValue;
            this.MarkAsDirty("DebitAccount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) { if (this.description != newValue) {
            this.description = newValue;
            this.MarkAsDirty("Description");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "LocalDescription", {
        get: function () { return this.localDescription; },
        set: function (newValue) { if (this.localDescription != newValue) {
            this.localDescription = newValue;
            this.MarkAsDirty("LocalDescription");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ExternalVATCard", {
        get: function () { return this.externalVATCard; },
        set: function (newValue) { if (this.externalVATCard != newValue) {
            this.externalVATCard = newValue;
            this.MarkAsDirty("ExternalVATCard");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ExternalTAXItemId", {
        get: function () { return this.externalTAXItemId; },
        set: function (newValue) { if (this.externalTAXItemId != newValue) {
            this.externalTAXItemId = newValue;
            this.MarkAsDirty("ExternalTAXItemId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "ChargeTypeGLAccountId", {
        get: function () { return this.chargeTypeGLAccountId; },
        set: function (newValue) { if (this.chargeTypeGLAccountId != newValue) {
            this.chargeTypeGLAccountId = newValue;
            this.MarkAsDirty("ChargeTypeGLAccountId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "AuthorizedSignatory", {
        get: function () { return this.authorizedSignatory; },
        set: function (newValue) { if (this.authorizedSignatory != newValue) {
            this.authorizedSignatory = newValue;
            this.MarkAsDirty("AuthorizedSignatory");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineShortPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "APInvoiceLine");
        }
    };
    APInvoiceLineShortPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    APInvoiceLineShortPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], APInvoiceLineShortPM.prototype, "PropertyChanged", void 0);
    return APInvoiceLineShortPM;
}());
exports.APInvoiceLineShortPM = APInvoiceLineShortPM;
//# sourceMappingURL=APInvoiceMultipleShortPM.js.map