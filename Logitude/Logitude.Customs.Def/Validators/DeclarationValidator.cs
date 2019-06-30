

using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.CustomsMessaging.Common.Gen;

namespace Logitude.Customs.Def.Validators
{
    public class DeclarationValidator
    {

        public static ValidationResult IsTransportModeValid(
           DeclarationPM declaration,
           System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            bool valid = true;

            
            CustomsHouseTypePM houseType = GetHouseTypewithAdditional(declaration.DeclarationOfficeCode, declaration.Tenant);

            if (houseType != null && declaration.TransportModeId != null && houseType.TransportModeId != null && houseType.TransportModeId != declaration.TransportModeId)
            {
                valid = false;
                return new ValidationResult(TranslateTextsClass.Translate("Customs.Declaration.O.Match", declaration.Tenant,true));
            }

            return null;
        }

        public static ValidationResult IsImporterCodeValid(
        DeclarationPM declaration,
        System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            bool isValid = true;

            if (declaration.ImporterCode != null )
            {
                if (declaration.ImporterCode.Contains("P") || declaration.ImporterCode.Contains("F"))
                {

                    if (!string.IsNullOrEmpty(declaration.ImporterPassportNumber))
                    {
                        if (declaration.ImporterPassportNumber.Length > 15)
                        {
                            isValid = false;
                            return new ValidationResult(TranslateTextsClass.Translate("Customs.Declaration.O.TooLongCode", declaration.Tenant, true));

                        }
                    }
                }
                else if( declaration.ImporterCode.Length != 9)
                {
                    isValid = false;
                    return new ValidationResult("מספר יבואן חייב להיות 9 תווים");

                }
                //Check Importer digit
                else if (declaration.ImporterCode.Length == 9 && string.IsNullOrWhiteSpace(declaration.ImporterId))
                {
                    string digit = declaration.ImporterCode.Substring(8);
                    int checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(declaration.ImporterCode);

                    if (digit != checkDigit.ToString())
                    {
                        isValid = false;
                        string errorText = TranslateTextsClass.Translate("Customs.Declaration.O.CorrectDigit", declaration.Tenant, true) + checkDigit.ToString();
                        return new ValidationResult(errorText);
                    }
                }
            }

            return null;
        }


        public static CustomsHouseTypePM GetHouseTypewithAdditional(string code, int tenant)
        {
            CustomsHouseTypePM houseType = null;
            if (!string.IsNullOrWhiteSpace(code))
            {
                CustomsHouseTypeRepository repository = new CustomsHouseTypeRepository(tenant);
                CustomsHouseType type = repository.GetSingle(new CustomsHouseTypeKeys() { Code = code });
                CustomsHouseTypeAdditionalRepository additionalRepository = new CustomsHouseTypeAdditionalRepository(tenant);
                CustomsHouseTypeAdditional additional = additionalRepository.GetSingleAdditionalByCode(code, tenant);
                if (type == null)
                {
                    throw new Exception("CustomsHouseType code does not exist in DB !!! code =" + code);
                }
                houseType = new CustomsHouseTypePM()
                {
                    Code = type.Code,
                    EnglishName = type.EnglishName,
                    LocalName = type.LocalName,

                    Tenant = tenant
                };
                if (additional != null)
                {
                    houseType.TransportModeId = additional.TransportModeId;
                    houseType.TransportModeName = additional.CustomsTransportMode != null ? additional.CustomsTransportMode.LocalName : null;
                    houseType.UnloadPortCode = additional.UnloadPortCode;
                    houseType.UnloadPortName = additional.UnloadingSiteType != null ? additional.UnloadingSiteType.LocalName : null;
                }
            }
            return houseType;
        }

    }
}