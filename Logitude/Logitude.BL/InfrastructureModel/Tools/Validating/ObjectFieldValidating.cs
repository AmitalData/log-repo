using System;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.Tools.Validating
{
    public class ObjectFieldValidating
    {
        public static void Validate(ObjectFieldPM entityPM)
        {
            ValidateCustomTextType(entityPM);
            ValidateCustomNumberType(entityPM);
        }

        private static void ValidateCustomTextType(ObjectFieldPM entityPM)
        {
            if (!entityPM.IsCustom) return;
            if (entityPM.DataTypeCode != "Text" && entityPM.DataTypeCode != "nText") return;
            
            if (entityPM.MaxLength > 2000)
            {
                throw new ApplicationException("Maximum length of the text is 2000");
            }

            if (entityPM.MinLength > 2000)
            {
                throw new ApplicationException("Minimum length of the text is 2000");
            }

            if (entityPM.MaxLength < 0)
            {
                throw new ApplicationException("Max length number shouldn't be less than 0");
            }
            if (entityPM.MaxLength == 0)
            {
                throw new ApplicationException("Max length Field is Required");
            }
            if (entityPM.MinLength > entityPM.MaxLength)
            {
                throw new ApplicationException("Min length number shouldn't be more than Max length number");
            }

            if (entityPM.MinLength < 0)
            {
                throw new ApplicationException("Min length number shouldn't be less than 0");
            }
        }

        private static void ValidateCustomNumberType(ObjectFieldPM entityPM)
        {
            if (!entityPM.IsCustom) return;
            if (entityPM.DataTypeCode != "Decimal") return;

            if (entityPM.NumberOfDigits > 12)
            {
                throw new ApplicationException("Maximum length of the Number is 12");
            }
            if (entityPM.NumberOfDigits < 0)
            {
                throw new ApplicationException("Length of the Number shouldn't be less than 0");
            }
            if(entityPM.NumberOfDigits == 0)
            {
                throw new ApplicationException("Length Field is Required");
            }
            if (entityPM.DigitsAfterPoint > 3)
            {
                throw new ApplicationException("Maximum decimal digits is 3");
            }
            if (entityPM.DigitsAfterPoint < 0)
            {
                throw new ApplicationException("Decimal digits of the Number shouldn't be less than 0");
            }

        }
    }
}