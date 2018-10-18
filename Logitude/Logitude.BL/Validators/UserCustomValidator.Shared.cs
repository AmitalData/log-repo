using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.Validators
{
	public class UserCustomValidator
    {
        public static ValidationResult ValidateUser(UserPM user, ValidationContext context)
        {

            if (user != null)
            {
                if (user.Id == null && string.IsNullOrEmpty(user.Password))
                {
                    return new ValidationResult("Please fill the password field!");
                }

                if (user.IsDistributor && string.IsNullOrEmpty(user.DistributorCode))
                {
                    return new ValidationResult("Please fill the distributor field.");
                }

            }
            return null;
        }
}
}