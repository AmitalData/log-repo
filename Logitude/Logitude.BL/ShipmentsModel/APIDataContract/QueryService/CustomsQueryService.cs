using System;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.Helpers;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public partial class CustomsQueryService
    {
        public Customs CustomsCustomDataMapping(ShipmentPM MyEntityPM, int Tenant)
        {
            try
            {
                var temp = CustomsDataMapping(MyEntityPM, Tenant);
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

        public ShipmentPM CustomsCustomDataMappingAndValidatin(Customs MyEntity, int Tenant)
        {
            try
            {
                TenantQuery tenantQuery = new TenantQuery(Tenant);
                UserQuery userQuery = new UserQuery(Tenant);
                TenantPM MyTenantPM = tenantQuery.GetSinglePM(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                ShipmentPM temp =CustomsDataMappingAndValidatin(MyEntity, Tenant);
                temp.NewConcurrencyGUID = Guid.NewGuid().ToString();
                temp.Tenant = Tenant;
                temp.DeclarationXMLData = MyEntity.DeclarationXMLData;
                temp.DeclarationNumber = MyEntity.DeclarationNumber;
                temp.ShipmentLevelCode = "D";
                temp.MainCarriageFromPortId = temp.FromPortId;
                temp.MainCarriageToPortId = temp.ToPortId;
                temp.FHLStatusCode = "NSEN";
                temp.FWBStatusCode = "NSEN";
                temp.FHLStatusName = "Not Sent";
                temp.FWBStatusName = "Not Sent";
                temp.ManifestStatusCode = "NSEN";
                temp.MainCarriageCarrierCode = MyEntity.MainCarriageCarrier != null ? MyEntity.MainCarriageCarrier.Code : null ;
                temp.DirectionId = "C";
                temp.LocalCustomsTransmissionsStatusCode = "NSEN";
                temp.IsOperationalClosed = false;
                temp.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.StatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.AWBCurrencyId = MyTenantPM.FreightCurrencyId;
                temp.ProfitCurrencyId = MyTenantPM.ProfitCurrencyId;
                temp.ValueOfGoodsCurrencyId = MyTenantPM.FreightCurrencyId;
                temp.CreatedByUserId = MyUserPM.Id;                              
                temp.VolumeUnitCode = MyTenantPM.VolumeUnitCode;
                temp.DimensionsUnitCode = MyTenantPM.DimensionsUnitCode;
                temp.GrossWeightUnitCode = MyTenantPM.GrossWeightUnitCode;
                temp.ChargeableWeightUnitCode = MyTenantPM.ChargeableWeightUnitCode;
                temp.Master = MyEntity.Master;
                temp.ShipperName = MyEntity.ShipperName;
                
                if (temp.CustomsClearanceDate != null || !string.IsNullOrEmpty(temp.DeclarationNumber))
                {
                    temp.IncludesCustoms = true;
                    if (temp.CustomsClearanceDate == null && !string.IsNullOrEmpty(temp.DeclarationNumber))
                    {
                        throw new ApplicationException("Declaration Date should be sent");
                    }
                    temp.DeclarationDate = temp.CustomsClearanceDate;
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

                temp.Ratio = this.GetRatio(temp.DirectionId, temp.TransportModeId, temp.ShipmentTypeId, MyTenantPM.CountryCode);
                temp.DimFactor = this.GetDimFactorFromRatio(temp.Ratio, temp.DimensionsUnitCode, temp.ChargeableWeightUnitCode);

                if (string.IsNullOrEmpty(temp.BranchId))
                {
                    temp.BranchId = MyUserPM.BranchId;
                }

                if (string.IsNullOrEmpty(temp.DepartmentId))
                {
                    temp.DepartmentId = MyUserPM.DepartmentId;
                }               

                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double? GetRatio(string CustomsionId, string transportModeId, string shipmentTypeId, string countryCode)
        {
            double? myResult = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                if (countryCode.ToUpper() == "US")
                {
                    if (CustomsionId == "D")
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
    }
}
