using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.EntitiesInitializer
{
    public class ShipmentInitializer : IEntityInitializer
    {
        public object Create(EntityInitializerArguments args)
        {
            ShipmentPM entityPM = new ShipmentPM()
            {
                ShipmentLevelCode = args.ShipmentLevelCode,
                DirectionId = args.DirectionId,
                TransportModeId = args.TransportModeId,

                FHLStatusCode = "NSEN",
                FWBStatusCode = "NSEN",
                FHLStatusName = "Not Sent",
                FWBStatusName = "Not Sent",
                ManifestStatusCode = "NSEN",
                LocalCustomsTransmissionsStatusCode = "NSEN",
                OnCarriageAdditionalTransportModeCode = "BYTR",

                Tenant = IntegrationTestLoginParameters.Tenant,
                BranchId = CorePreparationVariables.BranchId,
                DepartmentId = CorePreparationVariables.DepartmentId,
                ProfitCurrencyId = CorePreparationVariables.TenantPM.ProfitCurrencyId,

                //AWBCurrencyId = CorePreparationVariables.TenantPM.FreightCurrencyId,
                AWBCurrencyId = ShipmentVariables.CurrencyEURId,

                //CreatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                CreatedByUserId = CorePreparationVariables.UserId,
                UpdatedByUserId = CorePreparationVariables.UserId,
                ValueOfGoodsCurrencyId = ShipmentVariables.CurrencyEURId,
                AccountManagerUserId = CorePreparationVariables.UserId,
                NewConcurrencyGUID = Guid.NewGuid().ToString(),
            };

            ShipmentVariables.ConcurrencyGUID = entityPM.NewConcurrencyGUID;

            this.InitializePorts(entityPM);
            this.InitializeUnits(entityPM);
            this.InitializePartners(entityPM);
            this.InitializePrepaidCollect(entityPM);

            //entityPM.CreateDateTime = todayDate;
            //entityPM.LastUpdateDate = todayDate;
            //entityPM.StatusDate = todayDate;

            //shipmentPM.ShipmentPackages = IntegrationShipmentPackages.ShipmentPackages();

            return entityPM;
        }

        private void InitializePorts(ShipmentPM entityPM)
        {
            entityPM.FromPortId = entityPM.MainCarriageFromPortId = ShipmentVariables.PortLHRId;
            entityPM.ToPortId = entityPM.MainCarriageToPortId = ShipmentVariables.PortJFKId;
            entityPM.OriginMainCarriageFromPortId = entityPM.FromPortId;
            entityPM.MainCarriageFinalDestinationPortId = entityPM.ToPortId;
            entityPM.OriginPreCarriageFromPortId = entityPM.PreCarriageFromPortId;
            entityPM.OriginOnCarriageToPortId = entityPM.OnCarriageToPortId;
        }
        private void InitializeUnits(ShipmentPM entityPM)
        {
            // Old
            //shipmentPM.VolumeUnitCode = CorePreparationVariables.TenantPM.VolumeUnitCode;
            //shipmentPM.DimensionsUnitCode = CorePreparationVariables.TenantPM.DimensionsUnitCode;
            //shipmentPM.GrossWeightUnitCode = CorePreparationVariables.TenantPM.GrossWeightUnitCode;
            //shipmentPM.ChargeableWeightUnitCode = CorePreparationVariables.TenantPM.ChargeableWeightUnitCode;

            string dimensionsUnitCode = CorePreparationVariables.TenantPM.DimensionsUnitCode;
            string volumeUnitCode = CorePreparationVariables.TenantPM.VolumeUnitCode;
            string grossWeightUnitCode = CorePreparationVariables.TenantPM.GrossWeightUnitCode;
            string chargeableWeightUnitCode = this.GetChargeableWeightUnitCode(entityPM.TransportModeId);

            if (entityPM.DirectionId == "D")
            {
                if (!string.IsNullOrEmpty(CorePreparationVariables.TenantPM.CountryCode))
                {
                    if (CorePreparationVariables.TenantPM.CountryCode.ToUpper() == "US")
                    {
                        dimensionsUnitCode = "Inc";
                        volumeUnitCode = "CBI";
                        grossWeightUnitCode = "LB";
                        chargeableWeightUnitCode = "LB";
                    }
                }
            }

            entityPM.DimensionsUnitCode = dimensionsUnitCode;
            entityPM.VolumeUnitCode = volumeUnitCode;
            entityPM.GrossWeightUnitCode = grossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = chargeableWeightUnitCode;
            entityPM.Ratio = this.GetRatio(entityPM.DirectionId, entityPM.TransportModeId, entityPM.ShipmentTypeId, CorePreparationVariables.TenantPM.CountryCode);
            entityPM.DimFactor = this.GetDimFactorFromRatio(entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.ChargeableWeightUnitCode);
        }
        private void InitializePartners(ShipmentPM entityPM)
        {
            entityPM.ShipperId = ShipmentVariables.ShipperExport1;
            entityPM.AgentId = ShipmentVariables.AgentId;
            entityPM.CustomerId = ShipmentVariables.ShipperExport1;
            entityPM.IssuingCarrierAgentId = ShipmentVariables.AgentId;
        }
        private void InitializePrepaidCollect(ShipmentPM entityPM)
        {
            // Old
            //shipmentPM.FreightPrepaidCollectId = "C";
            //shipmentPM.OtherPrepaidCollectId = "C";

            string freightPrepaidCollectId = null;
            string otherPrepaidCollectId = null;

            switch (entityPM.DirectionId)
            {
                case "E":
                case "R":
                    {
                        if (entityPM.ShipmentLevelCode == "C")
                        {
                            freightPrepaidCollectId = CorePreparationVariables.TenantPM.MasterExpFreigPrepaidCollectId;
                            otherPrepaidCollectId = CorePreparationVariables.TenantPM.MasterExpOtherPrepaidCollectId;
                        }

                        else
                        {
                            freightPrepaidCollectId = CorePreparationVariables.TenantPM.ExportFreightPrepaidCollectId;
                            otherPrepaidCollectId = CorePreparationVariables.TenantPM.ExportOtherPrepaidCollectId;
                        }

                        break;
                    }

                case "I":
                    {
                        if (entityPM.ShipmentLevelCode == "C")
                        {
                            freightPrepaidCollectId = CorePreparationVariables.TenantPM.MasterImpFreiPrepaidCollectId;
                            otherPrepaidCollectId = CorePreparationVariables.TenantPM.MasterImpOtherPrepaidCollectId;
                        }

                        else
                        {
                            freightPrepaidCollectId = CorePreparationVariables.TenantPM.ImportFreightPrepaidCollectId;
                            otherPrepaidCollectId = CorePreparationVariables.TenantPM.ImportOtherPrepaidCollectId;
                        }

                        break;
                    }

                default:
                    {
                        freightPrepaidCollectId = "P";
                        otherPrepaidCollectId = "P";
                        break;
                    }
            }

            entityPM.FreightPrepaidCollectId = freightPrepaidCollectId;
            entityPM.OtherPrepaidCollectId = otherPrepaidCollectId;
        }


        private string GetChargeableWeightUnitCode(string transportModeId)
        {
            string output = null;

            if (transportModeId == "A")
            {
                output = CorePreparationVariables.TenantPM.ChargeableWeightUnitCode;
            }

            else
            {
                output = CorePreparationVariables.TenantPM.WeightMeasurementUnitCode;
            }

            return output;
        }
        private double? GetRatio(string directionId, string transportModeId, string shipmentTypeId, string countryCode)
        {
            double? output = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                if (countryCode.ToUpper() == "US")
                {
                    if (directionId == "D")
                    {
                        if (transportModeId == "A")
                        {
                            output = 7;
                        }

                        else if (transportModeId == "I")
                        {
                            if (shipmentTypeId == "LTL")
                            {
                                output = 9;
                            }
                        }
                    }
                }
            }

            if (output == null)
            {
                {
                    switch (transportModeId)
                    {
                        case "A": { output = 6; break; }
                        case "O": { output = 1; break; }
                        case "I":
                            {
                                if (shipmentTypeId == "LTL")
                                {
                                    output = 3.3;
                                }

                                else
                                {
                                    output = 1;
                                }

                                break;
                            }
                        default:
                            break;
                    }
                }
            }

            return output;
        }
        private double? GetDimFactorFromRatio(double? ratio, string dimensionsUnitCode, string chargeableWeightUnitCode)
        {
            double? output = null;

            if (output != null)
            {
                double WeightFactorOfConvert = 1;
                double DimensiosFactorOfConvert = 1;

                if (!string.IsNullOrEmpty(chargeableWeightUnitCode))
                {
                    switch (chargeableWeightUnitCode.ToUpper())
                    {
                        case "KG": { WeightFactorOfConvert = 1; break; }
                        case "LB": { WeightFactorOfConvert = 0.45359237; break; }
                        case "MT": { WeightFactorOfConvert = 1000; break; }
                    }
                }

                if (!string.IsNullOrEmpty(dimensionsUnitCode))
                {
                    switch (dimensionsUnitCode.ToUpper())
                    {
                        case "CM": { DimensiosFactorOfConvert = 1; break; }
                        case "INC": { DimensiosFactorOfConvert = 2.54; break; }
                        case "FT": { DimensiosFactorOfConvert = 30.48; break; }
                    }
                }

                output = WeightFactorOfConvert * 1000 * ratio / Math.Pow(DimensiosFactorOfConvert, 3);
            }

            if (output != null)
            {
                string toString = output.ToString();
                string[] myArray = toString.Split('.');

                if (myArray.Length > 1)
                {
                    string strDigits = "0." + myArray[1];
                    double digits = Convert.ToDouble(strDigits); //strDigits.to//+strDigits;

                    if (digits < 0.5)
                    {
                        output = Math.Floor(output.Value);
                    }

                    else
                    {
                        output = Math.Ceiling(output.Value);
                    }
                }
            }

            return output;
        }
    }
}
