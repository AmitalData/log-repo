using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.Validators
{
    public class ShipmentPackageValidator
    {
        public static ValidationResult IsShipmentPackageValid(ShipmentPackagePM shipmentPackagePM, ValidationContext context)
        {
            List<string> errorsList = new List<string>();

            /* Air Shipments */
            if (shipmentPackagePM.ShipmentPM != null)
            {
                if (shipmentPackagePM.ShipmentPM.TransportModeId.ToUpper() == "A")
                {

                }

                else if (shipmentPackagePM.ShipmentPM.TransportModeId.ToUpper() != "A")
                {
                    if (string.IsNullOrEmpty(shipmentPackagePM.PackageTypeId))
                    {
                        errorsList.Add(TextCodesTranslator.TranslateText("ShipmentPackage.M.PackageTypeIsRequired", shipmentPackagePM.Tenant));
                    }

                    if (shipmentPackagePM.Weight == null)
                    {
                        errorsList.Add(TextCodesTranslator.TranslateText("ShipmentPackage.M.WeightIsRequired", shipmentPackagePM.Tenant));
                    }
                }
            }

            if (errorsList.Count == 0)
            {
                return ValidationResult.Success;
            }

            else
            {
                string errorString = String.Empty;
                foreach (string error in errorsList)
                {
                    errorString = errorString + error + ",";
                }

                errorString = errorString.Remove(errorString.Length - 1);
                return new ValidationResult(errorString);
            }
        }
    }
}
