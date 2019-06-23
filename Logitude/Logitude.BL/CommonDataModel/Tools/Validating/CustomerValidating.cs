using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Text.RegularExpressions;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class CustomerValidating
    {
        public static void Validate(CustomerPM entityPM, bool isNewEntity, ICommonDataContext myContext)
        {
            int tenant = entityPM.Tenant;
            TenantRepository tenantRepository = new TenantRepository(myContext);
            Tenant myTenant = tenantRepository.GetSingleTenantOnly(tenant);

            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                AddressValidating.Validate(itemPM);
            }

            if (entityPM.IsCustomer)
            {
                ValidateVAT_Required(entityPM, myContext, myTenant);
                ValidateVAT_Unique(entityPM, myContext, myTenant, isNewEntity);
                ValidateVAT_Format(entityPM, myContext, myTenant);
            }

            ValidateCreditLimit(entityPM, myContext);
        }

        private static void ValidateVAT_Required(CustomerPM entityPM, ICommonDataContext myContext, Tenant myTenant)
        {
            if (entityPM.IsCustomer)
            {
                if (string.IsNullOrEmpty(entityPM.VatNumber))
                {
                    bool isValidating = false;

                    if (myTenant.VatMandatoryTypeCode != "MNT")
                    {
                        if (entityPM.PartnerTypeId == "PO")
                        {
                            if (entityPM.SetReady)
                            {
                                isValidating = true;
                            }

                            if (myTenant.VatMandatoryForPotentialCustomers)
                            {
                                isValidating = true;
                            }
                        }

                        else
                        {
                            isValidating = true;
                        }
                    }

                    if (isValidating)
                    {
                        int tenant = entityPM.Tenant;
                        string entityCountryId = entityPM.CountryId;
                        string entityCountryName = entityPM.CountryName;

                        string vatMandatoryTypeCode = myTenant.VatMandatoryTypeCode;
                        string vatMandatoryCountryId = myTenant.VatMandatoryCountryId;

                        if (vatMandatoryTypeCode == "MFA")
                        {
                            string msg = "VAT Number is required";
                            throw new ApplicationException(msg);
                        }

                        else if (vatMandatoryTypeCode == "MSC")
                        {
                            if (!string.IsNullOrEmpty(entityCountryId))
                            {
                                if (entityCountryId == vatMandatoryCountryId)
                                {
                                    string msg = "VAT Number is required for " + entityCountryName;
                                    throw new ApplicationException(msg);
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void ValidateVAT_Unique(CustomerPM entityPM, ICommonDataContext myContext, Tenant myTenant, bool isNewEntity)
        {
            if (entityPM.IsCustomer)
            {
                if (!string.IsNullOrEmpty(entityPM.VatNumber))
                {
                    AddressRepository addressRepository = new AddressRepository(myContext);

                    List<Card> allMatchedCards
                        = (from a in myContext.Cards
                           where a.Tenant == entityPM.Tenant
                           && (a.PartnerTypeId == "CS" || a.PartnerTypeId == "PO")
                           && a.VatNumber == entityPM.VatNumber
                           select a).ToList();

                    if (allMatchedCards != null)
                    {
                        if (myTenant.VatUniqueTypeCode == "UFA")
                        {
                            bool isAlreadyExists = false;

                            if (isNewEntity)
                            {
                                if (allMatchedCards.Count > 0)
                                {
                                    isAlreadyExists = true;
                                }
                            }

                            else
                            {
                                if (allMatchedCards.Where(d => d.Id != entityPM.Id).Any())
                                {
                                    isAlreadyExists = true;
                                }
                            }

                            if (isAlreadyExists)
                            {
                                string msg = "VAT Number already exists";
                                throw new ApplicationException(msg);
                            }
                        }

                        else if (myTenant.VatUniqueTypeCode == "USC")
                        {
                            if (!isNewEntity)
                            {
                                allMatchedCards = allMatchedCards.Where(d => d.Id != entityPM.Id).ToList();
                            }

                            if (allMatchedCards != null)
                            {
                                int tenant = entityPM.Tenant;
                                string entityCountryId = entityPM.CountryId;
                                string entityCountryName = entityPM.CountryName;

                                if (!string.IsNullOrEmpty(entityCountryId))
                                {
                                    if (entityCountryId == myTenant.VatUniqueCountryId)
                                    {
                                        bool isAlreadyExists = false;

                                        foreach (Card item in allMatchedCards)
                                        {
                                            Address address = addressRepository.GetMainAddressByCardId(item.Id, tenant);
                                            if (address != null)
                                            {
                                                if (address.CountryId == entityCountryId)
                                                {
                                                    isAlreadyExists = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (isAlreadyExists)
                                        {
                                            string msg = "VAT Number already exists for " + entityCountryName;
                                            throw new ApplicationException(msg);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void ValidateVAT_Format(CustomerPM entityPM, ICommonDataContext myContext, Tenant myTenant)
        {
            if (!string.IsNullOrEmpty(entityPM.VatNumber))
            {
                if (myTenant.VatFormatTypeCode != "NOF")
                {
                    int tenant = entityPM.Tenant;
                    string entityCountryId = entityPM.CountryId;
                    string entityCountryName = entityPM.CountryName;

                    if (myTenant.VatFormatTypeCode == "FAC")
                    {
                        Regex vatFormatRegex;

                        if (myTenant.IsNumeric)
                        {
                            vatFormatRegex = new Regex("^[0-9]*$");

                            if (!vatFormatRegex.IsMatch(entityPM.VatNumber))
                            {
                                throw new ApplicationException("VAT Number must be Numeric");
                            }
                        }

                        if (myTenant.VatSize != null && myTenant.VatSize > 0)
                        {
                            if (myTenant.VatSize != entityPM.VatNumber.Length)
                            {
                                throw new ApplicationException("VAT Number size must be " + myTenant.VatSize);
                            }
                        }
                    }

                    else if (myTenant.VatFormatTypeCode == "FSC")
                    {
                        if (!string.IsNullOrEmpty(myTenant.VatFormatCountryId))
                        {
                            if (myTenant.VatFormatCountryId == entityPM.CountryId)
                            {
                                Regex vatFormatRegex;

                                if (myTenant.IsNumeric)
                                {
                                    vatFormatRegex = new Regex("^[0-9]*$");

                                    if (!vatFormatRegex.IsMatch(entityPM.VatNumber))
                                    {
                                        throw new ApplicationException("VAT Number must be Numeric for " + entityCountryName);
                                    }
                                }

                                if (myTenant.VatSize != null && myTenant.VatSize > 0)
                                {
                                    if (myTenant.VatSize != entityPM.VatNumber.Length)
                                    {
                                        throw new ApplicationException("VAT Number size must be " + myTenant.VatSize + " for " + entityCountryName);
                                    }
                                }
                            }
                        }
                    }

                    if (myTenant.CheckDigitControlAlgorithmCode == "LUHN")
                    {
                        entityPM.VatNumber = entityPM.VatNumber.Trim();

                        string numberWithoutCheckDigit = entityPM.VatNumber.Substring(0, entityPM.VatNumber.Length - 1);

                        string checkDigit = MethodHelper.CalculateLuhnAlgorithm(numberWithoutCheckDigit).ToString();
                        string lastNumber = entityPM.VatNumber.LastOrDefault().ToString();

                        if (checkDigit != lastNumber)
                        {
                            throw new ApplicationException("Luhn Algorithm: Invalid VAT Number");
                        }
                    }
                }
            }
        }
        private static void ValidateCreditLimit(CustomerPM entityPM, ICommonDataContext myContext)
        {
            if (entityPM.IsCreditLimitEnabled)
            {
                string msg = "";
                if (entityPM.CreditLimitAmount == null)
                {
                    msg = "Credit limit amount field is required";
                }

                if (entityPM.CreditLimitWarningPercentage == null)
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        msg = "Credit limit warning percentage field is required";
                    }

                    else
                    {
                        msg += ",Credit limit warning percentage field is required";
                    }
                }

                else if (entityPM.CreditLimitWarningPercentage < 0 || entityPM.CreditLimitWarningPercentage > 100)
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        msg = "Credit limit warning percentage field must be more than 0 and less than 100";
                    }

                    else
                    {
                        msg += ",Credit limit warning percentage field must be more than 0 and less than 100";
                    }
                }                

                if (!string.IsNullOrEmpty(msg))
                {
                    string id = entityPM.Tenant.ToString();
                    CreditLimitSetting mySettings = (from d in myContext.CreditLimitSettings where d.Id == id select d).FirstOrDefault();
                    if(mySettings != null)
                    {
                        if (mySettings.IsCreditLimitEnabled)
                        {
                            throw new ApplicationException(msg);
                        }
                    }
                }
            }
        }

    }
}