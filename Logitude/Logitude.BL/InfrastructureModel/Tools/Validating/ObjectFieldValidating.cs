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
        }

        private static void ValidateCustomTextType(ObjectFieldPM entityPM)
        {
            if (!entityPM.IsCustom) return;
            if (entityPM.DataTypeCode != "Text" && entityPM.DataTypeCode != "nText") return;
            
            if (entityPM.MaxLength > 2000)
            {
                throw new ApplicationException("Maximum length of the text is 2000");
            }

            if (entityPM.MaxLength < 0)
            {
                throw new ApplicationException("Max length number shouldn't be less than 0");
            }

            if (entityPM.MinLength > entityPM.MaxLength && entityPM.MaxLength != 0)
            {
                throw new ApplicationException("Min length number shouldn't be greater than max length number");
            }

            if (entityPM.MinLength < 0)
            {
                throw new ApplicationException("Min length number shouldn't be less than 0");
            }
        }
    }
}