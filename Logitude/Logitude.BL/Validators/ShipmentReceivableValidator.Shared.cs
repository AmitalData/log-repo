using System.ComponentModel.DataAnnotations;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.Validators
{
    public class ShipmentReceivableValidator
    {
        public static ValidationResult IsShipmentReceivableValid(ShipmentReceivablePM shipmentReceivablePM, ValidationContext context)
        {
            if (shipmentReceivablePM.TotalAmount != null && shipmentReceivablePM.UnitPrice != null && shipmentReceivablePM.Quantity != null)
            {
                if (shipmentReceivablePM.Rate == null)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("ShipmentReceivable.M.ExchangeRateIsRequired", shipmentReceivablePM.Tenant));
                }
            }

            return ValidationResult.Success;
        }
    }
}
