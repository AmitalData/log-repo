using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.Validators
{
    public class CodeExistanceValidator
    {
        public static ValidationResult IsCodeAvailable(PortPM portObject, ValidationContext context)
        {
            bool valid = false;
            if (valid)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("Port.M.TheCodeAlreadyExists", portObject.Tenant));
            }

            return null;
        }
    }
}
