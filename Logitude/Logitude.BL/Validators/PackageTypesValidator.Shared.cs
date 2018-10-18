using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel
{
    public class PackageTypesValidator
    {
        public static ValidationResult IsPackageTypeValid(PackageTypePM packageType, ValidationContext context)
        {
            bool valid = ((packageType.IsAir) || (packageType.IsOcean) || (packageType.IsInland));

            if (!valid)
            {   
                return new ValidationResult(TextCodesTranslator.TranslateText("PackageType.M.ChoosePackageTypeTransportation", packageType.Tenant));
            }

            return null;
        }
    }
}
