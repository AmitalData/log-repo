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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class CustomerValidating
    {
        public static void Validate(CustomerPM entityPM, bool isNewEntity, ICommonDataContext myContext)
        {
            ValidateVatNumber(entityPM);

            int tenant = entityPM.Tenant;
            TenantRepository tenantRepository = new TenantRepository(myContext);
            Tenant myTenant = tenantRepository.GetSingleTenantOnly(tenant);

            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                AddressValidating.Validate(itemPM);
            }

            if (entityPM.IsCustomer && !entityPM.IsLogBox)// to allow batches for logbox
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
                    List<Card> allMatchedCards
                        = (from a in myContext.Cards.Include("Customer")
                           where a.Tenant == entityPM.Tenant
                           && a.VatNumber == entityPM.VatNumber
                           && (a.PartnerTypeId == "CS" || a.PartnerTypeId == "PO")
                           && !a.InActive
                           && a.Customer != null && a.Customer.CustomerStatusCode != "INA"
                           select a).ToList();
                    
                    if (allMatchedCards != null)
                    {
                        if(myTenant.VatUniqueTypeCode != "UNT")
                        {
                            bool doValidation = false;
                            if ((myTenant.VatUniquePartnerTypeCode  == "POT" && entityPM.PartnerTypeId == "PO") || myTenant.VatUniquePartnerTypeCode == "ALL")
                            {
                                doValidation = true;
                            }

                            else if (myTenant.VatUniquePartnerTypeCode == "CUS" && entityPM.PartnerTypeId == "CS")
                            {
                                allMatchedCards = allMatchedCards.Where(d => d.PartnerTypeId == "CS" && d.Customer != null && d.Customer.CustomerStatusCode == "ACT").ToList();
                                doValidation = true;
                            }  
                            
                            if(doValidation)
                            {
                                ValidateVAT_UniqueCountry(entityPM, allMatchedCards, myTenant, isNewEntity, myContext);
                            }
                        } 
                    }
                }
            }
        }
        private static void ValidateVAT_UniqueCountry(CustomerPM entityPM, List<Card> allMatchedCards, Tenant myTenant, bool isNewEntity, ICommonDataContext myContext)
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
                            AddressRepository addressRepository = new AddressRepository(myContext);

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
        private static void ValidateVAT_Format(CustomerPM entityPM, ICommonDataContext myContext, Tenant myTenant)
        {
            if (!string.IsNullOrEmpty(entityPM.VatNumber))
            {
                bool isValidated = false;
                string ExceptionMessage = "";
                Regex vatFormatRegex;

                if (myTenant.VatFormatTypeCode != "NOF")
                {
                    int tenant = entityPM.Tenant;
                    string entityCountryId = entityPM.CountryId;
                    string entityCountryName = entityPM.CountryName;

                    if (myTenant.VatFormatTypeCode == "FAC")
                    {
                        isValidated = true;   
                        
                    }

                    else if (myTenant.VatFormatTypeCode == "FSC")
                    {
                        if (!string.IsNullOrEmpty(myTenant.VatFormatCountryId))
                        {
                            if (myTenant.VatFormatCountryId == entityPM.CountryId)
                            {
                                isValidated = true;                                
                            }
                        }
                    }

                    if (isValidated)
                    {
                        if (myTenant.IsNumeric)
                        {
                            vatFormatRegex = new Regex("^[0-9]*$");

                            if (!vatFormatRegex.IsMatch(entityPM.VatNumber))
                            {
                                ExceptionMessage= myTenant.VatFormatTypeCode == "FAC" ? "VAT Number must be Numeric" : "VAT Number must be Numeric for " + entityCountryName;
                                throw new ApplicationException(ExceptionMessage);
                            }
                        }

                        if (myTenant.VatSize != null && myTenant.VatSize > 0)
                        {
                            if (myTenant.VatSize != entityPM.VatNumber.Length)
                            {
                                ExceptionMessage = myTenant.VatFormatTypeCode == "FAC" ? "VAT Number size must be " + myTenant.VatSize : "VAT Number size must be " + myTenant.VatSize + " for " + entityCountryName;
                                throw new ApplicationException(ExceptionMessage);
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
        private static void ValidateVatNumber(CustomerPM entityPM)
        {
            bool isAccountingActivated = CheckFullAccountingActivated(entityPM.Tenant);

            if (isAccountingActivated && !string.IsNullOrEmpty(entityPM.VatNumber))
            {
                var isValid = LuhnAlgorithm.IsVatNumberValid(entityPM.VatNumber);
                var isZeros = Int32.Parse(entityPM.VatNumber) == 0;
                if (!isValid || isZeros)
                    throw new ApplicationException(TranslateTextsClass.Translate("General.O.WrongVatNumber", entityPM.Tenant));
            }
        }

        private static bool CheckFullAccountingActivated(int tenantNumber)
        {
            var tenant = TenantQuery.GetSingleTenantPM(tenantNumber);
            var isAccountingActivated = tenant.AccountingActivated;
            return isAccountingActivated;
        }
    }
}