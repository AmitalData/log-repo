using System.ComponentModel.DataAnnotations;
using System.Linq;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.Validators
{
    public class ShipmentValidator
    {
        public static ValidationResult IsShipmentValid(ShipmentPM shipmentPM, ValidationContext context)
        {
            if (shipmentPM != null)
            {
                string str = null;
                //return ValidationResult.Success;
                if (!string.IsNullOrEmpty(shipmentPM.TransportModeId))
                {
                    if (shipmentPM.TransportModeId.ToUpper() == "A")
                    {
                        if (shipmentPM.ShipmentReceivables.Where(d => d.AWBPrint && d.CurrencyId != shipmentPM.AWBCurrencyId).Any())
                        {
                            str = TextCodesTranslator.TranslateText("Shipment.M.AllAWBPrintReceivablesMustMatchShipmentAWBCurrency", shipmentPM.Tenant);
                        }

                        if (shipmentPM.ShipmentPayables.Where(d => d.AWBPrint && d.CurrencyId != shipmentPM.AWBCurrencyId).Any())
                        {
                            if (string.IsNullOrEmpty(str))
                            {
                                str = TextCodesTranslator.TranslateText("Shipment.M.AllAWBPrintPayablesMustMatchShipmentAWBCurrency", shipmentPM.Tenant);
                            }

                            else
                            {
                                str = TextCodesTranslator.TranslateText("Shipment.M.AllAWBPrintPayablesMustMatchShipmentAWBCurrency", shipmentPM.Tenant);
                            }
                        }
                        if (shipmentPM.ShipmentAWBPrintOnlies.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete && d.CurrencyId != shipmentPM.AWBCurrencyId).Any())
                        {
                            str = TextCodesTranslator.TranslateText("Shipment.M.AllAWBPrintOnliesMustMatchShipmentAWBCurrency", shipmentPM.Tenant);
                        }

                    }
                }

                return (str == null) ? ValidationResult.Success : new ValidationResult(str);
            }
            return ValidationResult.Success;
        }
        

    }
}