using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public partial class DirectQueryService
    {
        public Direct DirectCustomDataMapping(ShipmentPM MyEntityPM, int Tenant)
        {
            try
            {
                var temp = DirectDataMapping(MyEntityPM, Tenant);
                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetShipmentIdByNumber(string shipmentNumber, int tenant)
        {
            return query.GetEntitiyIdByShipmentNumber(shipmentNumber, tenant);
        }
        public ShipmentPM DirectCustomDataMappingAndValidatin(Direct MyEntity, int Tenant, string ComputingPartnerCode = "")
        {
            try
            {
                TenantQuery tenantQuery = new TenantQuery(Tenant);
                UserQuery userQuery = new UserQuery(Tenant);
                ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(Tenant);

                TenantPM MyTenantPM = tenantQuery.GetSinglePM(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                ShipmentPM temp = DirectDataMappingAndValidatin(MyEntity, Tenant, ComputingPartnerCode);
                temp.NewConcurrencyGUID = Guid.NewGuid().ToString();
                temp.Tenant = Tenant;
                temp.ShipmentLevelCode = "D";

                if (!IsInlandDomesticShipment(temp))
                {
                    temp.MainCarriageFromPortId = temp.FromPortId;
                    temp.MainCarriageToPortId = temp.ToPortId;
                    temp.FinalDistenationPortId = temp.ToPortId;
                }

                temp.MainCarriageFinalDestinationPortId = temp.ToPortId;
                temp.FHLStatusCode = "NSEN";
                temp.FWBStatusCode = "NSEN";
                temp.FHLStatusName = "Not Sent";
                temp.FWBStatusName = "Not Sent";
                temp.ManifestStatusCode = "NSEN";
                temp.LocalCustomsTransmissionsStatusCode = "NSEN";
                temp.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.StatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.AWBCurrencyId =  MyTenantPM.FreightCurrencyId;
                temp.ProfitCurrencyId = MyTenantPM.ProfitCurrencyId;
                temp.Master = MyEntity.Master;
                temp.OnCarriageAdditionalTransportModeCode = "BYTR";

                if (string.IsNullOrEmpty(temp.VolumeUnitCode))
                {
                    temp.VolumeUnitCode = MyTenantPM.VolumeUnitCode;
                }

                if (string.IsNullOrEmpty(temp.DimensionsUnitCode))
                {
                    temp.DimensionsUnitCode = MyTenantPM.DimensionsUnitCode;
                }

                if (string.IsNullOrEmpty(temp.GrossWeightUnitCode))
                {
                    temp.GrossWeightUnitCode = MyTenantPM.GrossWeightUnitCode;
                }

                if (string.IsNullOrEmpty(temp.ChargeableWeightUnitCode))
                {
                    temp.ChargeableWeightUnitCode = MyTenantPM.ChargeableWeightUnitCode;
                }
                
                if (string.IsNullOrEmpty(temp.ValueOfGoodsCurrencyId))
                {
                    temp.ValueOfGoodsCurrencyId = MyTenantPM.FreightCurrencyId;
                }

                switch (temp.DirectionId)
                {
                    case "E":
                    case "R":
                        {
                            temp.FreightPrepaidCollectId = MyTenantPM.ExportFreightPrepaidCollectId;
                            temp.OtherPrepaidCollectId = MyTenantPM.ExportOtherPrepaidCollectId;
                            break;
                        }

                    case "I":
                        {
                            temp.FreightPrepaidCollectId = MyTenantPM.ImportFreightPrepaidCollectId;
                            temp.OtherPrepaidCollectId = MyTenantPM.ImportOtherPrepaidCollectId;
                            break;
                        }

                    case "D":
                        {
                            temp.FreightPrepaidCollectId = "P";
                            temp.OtherPrepaidCollectId = "P";

                            if (!string.IsNullOrEmpty(MyTenantPM.CountryCode))
                            {
                                if (MyTenantPM.CountryCode.ToUpper() == "US")
                                {
                                    temp.DimensionsUnitCode = "Inc";
                                    temp.VolumeUnitCode = "CBI";
                                    temp.GrossWeightUnitCode = "LB";
                                    temp.ChargeableWeightUnitCode = "LB";
                                }
                            }

                            break;
                        }
                }

                if (temp.Ratio == null || temp.Ratio == 0)
                {
                    temp.Ratio = this.GetRatio(temp.DirectionId, temp.TransportModeId, temp.ShipmentTypeId, MyTenantPM.CountryCode);
                }

                temp.DimFactor = this.GetDimFactorFromRatio(temp.Ratio, temp.DimensionsUnitCode, temp.ChargeableWeightUnitCode);

                if (string.IsNullOrEmpty(temp.CreatedByUserId))
                {
                    temp.CreatedByUserId = MyUserPM.Id;
                }

                if (string.IsNullOrEmpty(temp.UpdatedByUserId))
                {
                    temp.UpdatedByUserId = MyUserPM.Id;
                }

                if (string.IsNullOrEmpty(temp.BranchId))
                {
                    temp.BranchId = MyUserPM.BranchId;
                }

                if (string.IsNullOrEmpty(temp.DepartmentId))
                {
                    temp.DepartmentId = MyUserPM.DepartmentId;
                }

                this.UpdatePickups(temp);
                this.UpdateDeliveries(temp);
                
                foreach (ShipmentReceivablePM item in temp.ShipmentReceivables)
                {
                    if(string.IsNullOrEmpty(item.CurrencyId))
                    {
                        var chergeType = chargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, Tenant);
                        if (chergeType != null && !string.IsNullOrEmpty(chergeType.ReceivablesDefaultCurrencyId))
                        {
                            item.CurrencyId = chergeType.ReceivablesDefaultCurrencyId;
                        }
                    }
                }

                foreach (ShipmentPayablePM item in temp.ShipmentPayables)
                {
                    if (string.IsNullOrEmpty(item.CurrencyId))
                    {
                        var chergeType = chargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, Tenant);
                        if (chergeType != null && !string.IsNullOrEmpty(chergeType.PayablesDefaultCurrencyId))
                        {
                            item.CurrencyId = chergeType.PayablesDefaultCurrencyId;
                        }
                    }
                }
                
                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePickups(ShipmentPM shipment)
        {
            if (shipment == null)
                return;

            if (shipment.ShipmentPickUps == null)
                return;

            if (shipment.ShipmentPickUps.Count == 0)
                return;

            this.UpdateUpdatedShipmentPickUps(shipment);
        }
        public void UpdateDeliveries(ShipmentPM shipment)
        {
            if (shipment == null)
                return;

            if (shipment.ShipmentDeliveries == null)
                return;

            if (shipment.ShipmentDeliveries.Count == 0)
                return;

            this.UpdateUpdatedShipmentDeliveries(shipment);
        }
        private void UpdateUpdatedShipmentPickUps(ShipmentPM shipment)
        {
            List<ShipmentPickUpPM> updatedShipmentPickUps = shipment.ShipmentPickUps.FindAll(pickUp => pickUp.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete);
            foreach (ShipmentPickUpPM pickUp in updatedShipmentPickUps)
            {
                pickUp.PickUpDeliveryTypeCode = "PICK";
                this.ValidateAndSetPickupFromSide(pickUp);
                this.ValidateAndSetPickupToSide(pickUp);
            }
        }
        private void UpdateUpdatedShipmentDeliveries(ShipmentPM shipment)
        {
            List<ShipmentDeliveryPM> updatedShipmentDelivery = shipment.ShipmentDeliveries.FindAll(delivery => delivery.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete);
            foreach (ShipmentDeliveryPM delivery in updatedShipmentDelivery)
            {
                delivery.PickUpDeliveryTypeCode = "DELV";
                this.ValidateAndSetDeliveryFromSide(delivery);
                this.ValidateAndSetDeliveryToSide(delivery);
            }
        }
        private bool IsInlandDomesticShipment(ShipmentPM entityPM)
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }
        private double? GetRatio(string directionId, string transportModeId, string shipmentTypeId, string countryCode)
        {
            double? myResult = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                if (countryCode.ToUpper() == "US")
                {
                    if (directionId == "D")
                    {
                        if (transportModeId == "A")
                        {
                            myResult = 7;
                        }

                        else if (transportModeId == "I")
                        {
                            if (shipmentTypeId == "LTL")
                            {
                                myResult = 9;
                            }
                        }
                    }
                }
            }

            if (myResult == null)
            {
                {
                    switch (transportModeId)
                    {
                        case "A": { myResult = 6; break; }
                        case "O": { myResult = 1; break; }
                        case "I":
                            {
                                if (shipmentTypeId == "LTL")
                                {
                                    myResult = 3.3;
                                }

                                else
                                {
                                    myResult = 1;
                                }

                                break;
                            }
                        default:
                            break;
                    }
                }
            }

            return myResult;
        }
        private double? GetDimFactorFromRatio(double? ratio, string dimentionCode, string weightCode)
        {
            double? myRatio = null;

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            double? myResult = null;

            if (myRatio != null)
            {
                double WeightFactorOfConvert = 1;
                double DimensiosFactorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { WeightFactorOfConvert = 1; break; }
                        case "LB": { WeightFactorOfConvert = 0.45359237; break; }
                        case "MT": { WeightFactorOfConvert = 1000; break; }
                    }
                }

                if (!string.IsNullOrEmpty(dimentionCode))
                {
                    switch (dimentionCode.ToUpper())
                    {
                        case "CM": { DimensiosFactorOfConvert = 1; break; }
                        case "INC": { DimensiosFactorOfConvert = 2.54; break; }
                        case "FT": { DimensiosFactorOfConvert = 30.48; break; }
                    }
                }

                myResult = WeightFactorOfConvert * 1000 * myRatio / Math.Pow(DimensiosFactorOfConvert, 3);
            }

            if (myResult != null)
            {
                string toString = myResult.ToString();
                string[] myArray = toString.Split('.');

                if (myArray.Length > 1)
                {
                    string strDigits = "0." + myArray[1];
                    double? digits = Convert.ToDouble(strDigits);

                    if (digits < 0.5)
                    {
                        myResult = Math.Floor(myResult.Value);
                    }

                    else
                    {
                        myResult = Math.Ceiling(myResult.Value);
                    }
                }
            }

            return myResult;
        }
        private void ValidateAndSetPickupFromSide(ShipmentPickUpPM item)
        {
            if (!string.IsNullOrEmpty(item.PickUpDeliveryFromTypeCode))
            {
                switch (item.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (string.IsNullOrEmpty(item.FromPartnerCardId))
                            {
                                throw new ApplicationException("Pickup from partner is missing");
                            }
                            break;
                        }

                    case "PORT":
                        {
                            if (string.IsNullOrEmpty(item.FromPortId))
                            {
                                throw new ApplicationException("Pickup from port is missing");
                            }
                            break;
                        }

                    case "CASL":
                        {
                            if (string.IsNullOrEmpty(item.FromAddressCity) || string.IsNullOrEmpty(item.FromAddressCountryId))
                            {
                                throw new ApplicationException("Pickup from city or country is missing");
                            }
                            break;
                        }
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(item.FromPartnerCardId))
                {
                    item.PickUpDeliveryFromTypeCode = "PART";
                }
                else if (!string.IsNullOrEmpty(item.FromPortId))
                {
                    item.PickUpDeliveryFromTypeCode = "PORT";
                }

                else if (!string.IsNullOrEmpty(item.FromAddressCity) && !string.IsNullOrEmpty(item.FromAddressCountryId))
                {
                    item.PickUpDeliveryFromTypeCode = "CASL";
                }
            }
        }
        private void ValidateAndSetPickupToSide(ShipmentPickUpPM item)
        {
            if (!string.IsNullOrEmpty(item.PickUpDeliveryToTypeCode))
            {
                switch (item.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (string.IsNullOrEmpty(item.ToPartnerCardId))
                            {
                                throw new ApplicationException("Pickup to partner is missing");
                            }
                            break;
                        }

                    case "PORT":
                        {
                            if (string.IsNullOrEmpty(item.ToPortId))
                            {
                                throw new ApplicationException("Pickup to port is missing");
                            }
                            break;
                        }

                    case "CASL":
                        {
                            if (string.IsNullOrEmpty(item.ToAddressCity) || string.IsNullOrEmpty(item.ToAddressCountryId))
                            {
                                throw new ApplicationException("Pickup to city or country is missing");
                            }
                            break;
                        }
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                {
                    item.PickUpDeliveryToTypeCode = "PART";
                }
                else if (!string.IsNullOrEmpty(item.ToPortId))
                {
                    item.PickUpDeliveryToTypeCode = "PORT";
                }

                else if (!string.IsNullOrEmpty(item.ToAddressCity) && !string.IsNullOrEmpty(item.ToAddressCountryId))
                {
                    item.PickUpDeliveryToTypeCode = "CASL";
                }
            }
        }
        private void ValidateAndSetDeliveryFromSide(ShipmentDeliveryPM item)
        {
            if (!string.IsNullOrEmpty(item.PickUpDeliveryFromTypeCode))
            {
                switch (item.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (string.IsNullOrEmpty(item.FromPartnerCardId))
                            {
                                throw new ApplicationException("Delivery from partner is missing");
                            }
                            break;
                        }

                    case "PORT":
                        {
                            if (string.IsNullOrEmpty(item.FromPortId))
                            {
                                throw new ApplicationException("Delivery from port is missing");
                            }
                            break;
                        }

                    case "CASL":
                        {
                            if (string.IsNullOrEmpty(item.FromAddressCity) || string.IsNullOrEmpty(item.FromAddressCountryId))
                            {
                                throw new ApplicationException("Delivery from city or country is missing");
                            }
                            break;
                        }
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(item.FromPartnerCardId))
                {
                    item.PickUpDeliveryFromTypeCode = "PART";
                }
                else if (!string.IsNullOrEmpty(item.FromPortId))
                {
                    item.PickUpDeliveryFromTypeCode = "PORT";
                }

                else if (!string.IsNullOrEmpty(item.FromAddressCity) && !string.IsNullOrEmpty(item.FromAddressCountryId))
                {
                    item.PickUpDeliveryFromTypeCode = "CASL";
                }
            }
        }
        private void ValidateAndSetDeliveryToSide(ShipmentDeliveryPM item)
        {
            if (!string.IsNullOrEmpty(item.PickUpDeliveryToTypeCode))
            {
                switch (item.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (string.IsNullOrEmpty(item.ToPartnerCardId))
                            {
                                throw new ApplicationException("Delivery to partner is missing");
                            }
                            break;
                        }

                    case "PORT":
                        {
                            if (string.IsNullOrEmpty(item.ToPortId))
                            {
                                throw new ApplicationException("Delivery to port is missing");
                            }
                            break;
                        }

                    case "CASL":
                        {
                            if (string.IsNullOrEmpty(item.ToAddressCity) || string.IsNullOrEmpty(item.ToAddressCountryId))
                            {
                                throw new ApplicationException("Delivery to city or country is missing");
                            }
                            break;
                        }
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                {
                    item.PickUpDeliveryToTypeCode = "PART";
                }
                else if (!string.IsNullOrEmpty(item.ToPortId))
                {
                    item.PickUpDeliveryToTypeCode = "PORT";
                }

                else if (!string.IsNullOrEmpty(item.ToAddressCity) && !string.IsNullOrEmpty(item.ToAddressCountryId))
                {
                    item.PickUpDeliveryToTypeCode = "CASL";
                }
            }
        }
    }
}