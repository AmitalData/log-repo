using System;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;

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



        public ShipmentPM CustomsDataMappingAndValidating(Customs MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new ShipmentPM();
                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }
             
                if (temp == null)
                {
                    throw new ApplicationException("Shipment with ShipmentNumber " + MyEntity.ShipmentNumber + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }

                CardQueryService cardQueryService = new CardQueryService(Tenant);
                PortQueryService portQueryService = new PortQueryService(Tenant);
                ShipmentTypeQueryService ShipmentTypeShipmentTypeService = new ShipmentTypeQueryService(Tenant);
                if (MyEntity.ShipmentType != null)
                {
                    var myShipmentTypePM = ShipmentTypeShipmentTypeService.ShipmentTypeCustomDataMappingAndValidatin(MyEntity.ShipmentType, Tenant);
                    if (myShipmentTypePM != null)
                    {
                        temp.ShipmentTypeId = myShipmentTypePM.Id;
                    }
                }

                TransportModeQueryService TransportModeTransportModeService = new TransportModeQueryService(Tenant);
                if (MyEntity.TransportMode != null)
                {
                    var myTransportModePM = TransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.TransportMode, Tenant, ComputingPartnerName);
                    if (myTransportModePM != null)
                    {
                        temp.TransportModeId = myTransportModePM.Id;
                    }
                }

                if (MyEntity.Shipper != null)
                {
                    var myShipperPM = cardQueryService.CardDataMappingAndValidatin(MyEntity.Shipper, Tenant, ComputingPartnerName);
                    if (myShipperPM != null)
                    {
                        temp.ShipperId = myShipperPM.Id;
                    }
                }

                if (MyEntity.Consignee != null)
                {
                    var myConsigneePM = cardQueryService.CardDataMappingAndValidatin(MyEntity.Consignee, Tenant, ComputingPartnerName);
                    if (myConsigneePM != null)
                    {
                        temp.ConsigneeId = myConsigneePM.Id;
                    }
                }

                if (MyEntity.Customer != null)
                {
                    var myCustomerPM = cardQueryService.CardDataMappingAndValidatin(MyEntity.Customer, Tenant, ComputingPartnerName);
                    if (myCustomerPM != null)
                    {
                        temp.CustomerId = myCustomerPM.Id;
                    }
                }
                
                if (MyEntity.FromPort != null)
                {
                    var myFromPortPM = portQueryService.PortDataMappingAndValidatin(MyEntity.FromPort, Tenant, ComputingPartnerName);
                    if (myFromPortPM != null)
                    {
                        temp.FromPortId = myFromPortPM.Id;
                    }
                }

                if (MyEntity.ToPort != null)
                {
                    var myToPortPM = portQueryService.PortDataMappingAndValidatin(MyEntity.ToPort, Tenant, ComputingPartnerName);
                    if (myToPortPM != null)
                    {
                        temp.ToPortId = myToPortPM.Id;
                    }
                }

                WeightUnitQueryService GrossWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
                if (MyEntity.GrossWeightUnit != null)
                {
                    var myGrossWeightUnitPM = GrossWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.GrossWeightUnit, Tenant, ComputingPartnerName);
                    if (myGrossWeightUnitPM != null)
                    {
                        temp.GrossWeightUnitCode = myGrossWeightUnitPM.Code;
                    }
                }

                WeightUnitQueryService ChargeableWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
                if (MyEntity.ChargeableWeightUnit != null)
                {
                    var myChargeableWeightUnitPM = ChargeableWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.ChargeableWeightUnit, Tenant, ComputingPartnerName);
                    if (myChargeableWeightUnitPM != null)
                    {
                        temp.ChargeableWeightUnitCode = myChargeableWeightUnitPM.Code;
                    }
                }

                VolumeUnitQueryService VolumeUnitVolumeUnitService = new VolumeUnitQueryService(Tenant);
                if (MyEntity.VolumeUnit != null)
                {
                    var myVolumeUnitPM = VolumeUnitVolumeUnitService.VolumeUnitDataMappingAndValidatin(MyEntity.VolumeUnit, Tenant, ComputingPartnerName);
                    if (myVolumeUnitPM != null)
                    {
                        temp.VolumeUnitCode = myVolumeUnitPM.Code;
                    }
                }

                IncotermQueryService IncotermIncotermService = new IncotermQueryService(Tenant);
                if (MyEntity.Incoterm != null)
                {
                    var myIncotermPM = IncotermIncotermService.IncotermDataMappingAndValidatin(MyEntity.Incoterm, Tenant, ComputingPartnerName);
                    if (myIncotermPM != null)
                    {
                        temp.IncotermId = myIncotermPM.Id;
                    }
                }

                BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
                if (MyEntity.Branch != null)
                {
                    var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch, Tenant, ComputingPartnerName);
                    if (myBranchPM != null)
                    {
                        temp.BranchId = myBranchPM.Id;
                    }
                }

                DepartmentQueryService DepartmentDepartmentService = new DepartmentQueryService(Tenant);
                if (MyEntity.Department != null)
                {
                    var myDepartmentPM = DepartmentDepartmentService.DepartmentDataMappingAndValidatin(MyEntity.Department, Tenant, ComputingPartnerName);
                    if (myDepartmentPM != null)
                    {
                        temp.DepartmentId = myDepartmentPM.Id;
                    }
                }

                if (MyEntity.MainCarriageCarrier != null)
                {
                    var myMainCarriageCarrierPM = cardQueryService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier, Tenant, ComputingPartnerName);
                    if (myMainCarriageCarrierPM != null)
                    {
                        temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
                    }
                }

                CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant, "Shipment");
                if (MyEntity.CustomFields != null)
                {
                    customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
                }

                if (MyEntity.AirPackages != null && MyEntity.AirPackages.Count > 0)
                {
                    AirPackageQueryService AirPackageService15 = new AirPackageQueryService(Tenant);
                    temp.ShipmentPackages = AirPackageService15.AirPackageCustomDataMappingAndValidatin(MyEntity, MyEntity.AirPackages, Tenant, ComputingPartnerName);
                }

                if (MyEntity.OceanOrInlandPackages != null && MyEntity.OceanOrInlandPackages.Count > 0)
                {
                    OceanOrInlandPackageQueryService OceanOrInlandPackageService15 = new OceanOrInlandPackageQueryService(Tenant);
                    temp.ShipmentPackages = OceanOrInlandPackageService15.OceanOrInlandPackageCustomDataMappingAndValidatin(MyEntity, MyEntity.OceanOrInlandPackages, Tenant, ComputingPartnerName);
                }

                if (MyEntity.Containers != null && MyEntity.Containers.Count > 0)
                {
                    ContainerQueryService ContainerService15 = new ContainerQueryService(Tenant);
                    temp.ShipmentPackages = ContainerService15.ContainerCustomDataMappingAndValidatin(MyEntity, MyEntity.Containers, Tenant, ComputingPartnerName);
                }

                temp.ShipperReference1 = MyEntity.ShipperReference1;
                temp.ShipperReference2 = MyEntity.ShipperReference2;
                temp.ShipperReference3 = MyEntity.ShipperReference3;
                temp.ConsigneeReference1 = MyEntity.ConsigneeReference1;
                temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;
                temp.ConsigneeReference3 = MyEntity.ConsigneeReference3;
                temp.House = MyEntity.HouseNo;
                temp.HAWBDate = MyEntity.HouseDate;
                temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;
                temp.AWBCommodityItemNumber = MyEntity.Commodity;
                temp.TEU = MyEntity.TEU;
                temp.NumberOfPackages = MyEntity.NumberOfPackages;
                temp.GrossWeight = MyEntity.GrossWeight;
                temp.Volume = MyEntity.Volume;
                temp.VolumetricWeight = MyEntity.VolumetricWeight;
                temp.ChargeableWeight = MyEntity.ChargeableWeight;
                temp.ShipmentNumber = MyEntity.ShipmentNumber;
                temp.Master = MyEntity.Master;
                temp.CustomsClearanceDate = MyEntity.CustomsClearanceDate;
                temp.DeclarationNumber = MyEntity.DeclarationNumber;
                temp.ShipperName = MyEntity.ShipperName;
                temp.DeclarationXMLData = MyEntity.DeclarationXMLData;
                temp.DeclarationDate = MyEntity.DeclarationDate;
                temp.CustomerReference1 = MyEntity.CustomerReference1;
                temp.CustomerReference2 = MyEntity.CustomerReference2;
                temp.CustomerReference3 = MyEntity.CustomerReference3;
                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShipmentPM CustomsCustomDataMappingAndValidatin(Customs MyEntity, int Tenant)
        {
            try
            {
                TenantQuery tenantQuery = new TenantQuery(Tenant);
                UserQuery userQuery = new UserQuery(Tenant);
                TenantPM MyTenantPM = tenantQuery.GetSinglePM(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                ShipmentPM temp =this.CustomsDataMappingAndValidating(MyEntity, Tenant);
                temp.NewConcurrencyGUID = Guid.NewGuid().ToString();
                temp.Tenant = Tenant;
                temp.DeclarationWCOXml = MyEntity.DeclarationXMLData;
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
                temp.DeclarationDate = MyEntity.DeclarationDate;
                temp.CustomerReference1 = MyEntity.CustomerReference1;
                temp.CustomerReference2 = MyEntity.CustomerReference2;
                temp.CustomerReference3 = MyEntity.CustomerReference3;

                if (temp.DeclarationDate != null || !string.IsNullOrEmpty(temp.DeclarationNumber))
                {
                    temp.IncludesCustoms = true;
                    if (temp.DeclarationDate == null && !string.IsNullOrEmpty(temp.DeclarationNumber))
                    {
                        throw new ApplicationException("Declaration Date should be sent");
                    }
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
