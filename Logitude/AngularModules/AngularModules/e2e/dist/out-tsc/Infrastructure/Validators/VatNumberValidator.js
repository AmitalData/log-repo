"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var InfraSettings_1 = require("../Utilities/InfraSettings");
var VatNumberValidator = /** @class */ (function () {
    function VatNumberValidator() {
    }
    VatNumberValidator.ValidateVatFormat = function (myArgs) {
        if (!Tools_1.AppTool.IsNullOrEmpty(myArgs.VATNumber)) {
            var myTenant = InfraSettings_1.InfraSettings.TenantPM;
            if (myTenant.VatFormatTypeCode != "NOF") {
                if (myTenant.VatFormatTypeCode == "FAC") {
                    if (myTenant.IsNumeric) {
                        if (!Tools_1.FormatTool.IsNumeric(myArgs.VATNumber)) {
                            myArgs.Errors.push("VAT Number must be Numeric");
                        }
                    }
                    if (myTenant.VatSize != null && myTenant.VatSize > 0) {
                        if (myTenant.VatSize != myArgs.VATNumber.length) {
                            myArgs.Errors.push("VAT Number size must be " + myTenant.VatSize);
                        }
                    }
                }
                else if (myTenant.VatFormatTypeCode == "FSC") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(myTenant.VatFormatCountryId)) {
                        if (myTenant.VatFormatCountryId == myArgs.CountryId) {
                            if (myTenant.IsNumeric) {
                                if (!Tools_1.FormatTool.IsNumeric(myArgs.VATNumber)) {
                                    myArgs.Errors.push("VAT Number must be Numeric for " + myArgs.CountryName);
                                }
                            }
                            if (myTenant.VatSize != null && myTenant.VatSize > 0) {
                                if (myTenant.VatSize != myArgs.VATNumber.length) {
                                    myArgs.Errors.push("VAT Number size must be " + myTenant.VatSize + " for " + myArgs.CountryName);
                                }
                            }
                        }
                    }
                }
            }
        }
    };
    VatNumberValidator.ValidateVatMandatory = function (args) {
        if (args.IsCustomer) {
            if (Tools_1.AppTool.IsNullOrEmpty(args.VATNumber)) {
                if (InfraSettings_1.InfraSettings.TenantPM.VatMandatoryTypeCode != "MNT") {
                    var isValidatingField = false;
                    if (args.PartnerTypeId == "PO") {
                        if (args.SetReady) {
                            isValidatingField = true;
                        }
                        if (InfraSettings_1.InfraSettings.TenantPM.VatMandatoryForPotentialCustomers) {
                            isValidatingField = true;
                        }
                    }
                    else {
                        isValidatingField = true;
                    }
                    if (isValidatingField) {
                        if (InfraSettings_1.InfraSettings.TenantPM.VatMandatoryTypeCode == "MFA") {
                            args.Errors.push("VAT Number is required");
                        }
                    }
                    else if (InfraSettings_1.InfraSettings.TenantPM.VatMandatoryTypeCode == "MSC") {
                        if (args.CountryId == InfraSettings_1.InfraSettings.TenantPM.VatMandatoryCountryId) {
                            args.Errors.push("VAT Number is required for " + args.CountryEnglishName);
                        }
                    }
                }
            }
        }
    };
    return VatNumberValidator;
}());
exports.VatNumberValidator = VatNumberValidator;
var VATValidatorArgs = /** @class */ (function () {
    function VATValidatorArgs() {
        this.VATNumber = null;
        this.CountryId = null;
        this.CountryName = null;
        this.CountryEnglishName = null;
        this.PartnerTypeId = null;
        this.IsCustomer = false;
        this.SetReady = false;
        this.Errors = [];
    }
    return VATValidatorArgs;
}());
exports.VATValidatorArgs = VATValidatorArgs;
//# sourceMappingURL=VatNumberValidator.js.map