using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class VATValidating
    {
        public static void ValidateVAT_Required(Card entityPM, Tenant myTenant)
        {
            if (string.IsNullOrEmpty(entityPM.VatNumber))
            {
                if (myTenant.VatMandatoryTypeCode != "MNT")
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
        public static void ValidateVAT_Unique(Card entityPM, ICommonDataContext myContext, Tenant myTenant, bool isNewEntity)
        {
            if (!string.IsNullOrEmpty(entityPM.VatNumber))
            {
                AddressRepository addressRepository = new AddressRepository(myContext);

                List<Card> allMatchedCards
                    = (from a in myContext.Cards
                       where a.Tenant == entityPM.Tenant
                       && (a.PartnerTypeId == entityPM.PartnerTypeId)
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
        public static void ValidateVAT_Format(Card entityPM, ICommonDataContext myContext, Tenant myTenant)
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
                                ExceptionMessage = myTenant.VatFormatTypeCode == "FAC" ? "VAT Number must be Numeric" : "VAT Number must be Numeric for " + entityCountryName;
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
    }
}
