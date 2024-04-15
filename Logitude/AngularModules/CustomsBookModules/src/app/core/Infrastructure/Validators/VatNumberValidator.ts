import {AppTool, FormatTool} from '../Tools';
import {InfraSettings} from '../Utilities/InfraSettings';

export class VatNumberValidator {
    public static ValidateVatFormat(myArgs: VATValidatorArgs) {
        if (!AppTool.IsNullOrEmpty(myArgs.VATNumber)) {

            var myTenant = InfraSettings.TenantPM;

            if (myTenant.VatFormatTypeCode != "NOF") {
                if (myTenant.VatFormatTypeCode == "FAC") {
                    if (myTenant.IsNumeric) {
                        if (!FormatTool.IsNumeric(myArgs.VATNumber)) {
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
                    if (!AppTool.IsNullOrEmpty(myTenant.VatFormatCountryId)) {
                        if (myTenant.VatFormatCountryId == myArgs.CountryId) {

                            if (myTenant.IsNumeric) {

                                if (!FormatTool.IsNumeric(myArgs.VATNumber)) {
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
    }
    public static ValidateVatMandatory(args: VATValidatorArgs) {
        if (args.IsCustomer) {
            if (AppTool.IsNullOrEmpty(args.VATNumber)) {
                if (InfraSettings.TenantPM.VatMandatoryTypeCode != "MNT") {
                    var isValidatingField = false;

                    if (args.PartnerTypeId == "PO") {
                        if (args.SetReady) {
                            isValidatingField = true;
                        }

                        if (InfraSettings.TenantPM.VatMandatoryForPotentialCustomers) {
                            isValidatingField = true;
                        }
                    }

                    else {
                        isValidatingField = true;
                    }

                    if (isValidatingField) {
                        if (InfraSettings.TenantPM.VatMandatoryTypeCode == "MFA") {
                            args.Errors.push("VAT Number is required");
                        }

                        else if (InfraSettings.TenantPM.VatMandatoryTypeCode == "MSC") {
                            if (args.CountryId == InfraSettings.TenantPM.VatMandatoryCountryId) {
                                args.Errors.push("VAT Number is required for " + args.CountryEnglishName);
                            }
                        }
                    }                   
                }
            }
        }
    }
}

export class VATValidatorArgs {
    public VATNumber: string = null;
    public CountryId: string = null;
    public CountryName: string = null;
    public CountryEnglishName: string = null;
    public PartnerTypeId: string = null;
    public IsCustomer: boolean = false;
    public SetReady: boolean = false;
    public Errors: string[] = [];
}
