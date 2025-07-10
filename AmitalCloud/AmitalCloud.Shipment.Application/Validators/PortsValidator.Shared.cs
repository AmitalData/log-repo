using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.EntityValidators
{
    public class PortsValidator
    {
        public static ValidationResult IsPortTypeValid(PortPM port, ValidationContext context)
        {            
            bool valid = ((port.IsAir)||(port.IsOcean)||(port.IsInland));

            if (!valid)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("Port.M.ChoosePortTransportation", port.Tenant));
            }

            return null;
        }
    }    
}