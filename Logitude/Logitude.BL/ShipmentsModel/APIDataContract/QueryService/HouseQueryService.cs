using System;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.Helpers;
using System.Collections.Generic;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public partial class HouseQueryService
    {
        public List<House> HouseCustomDataMapping(ShipmentPM masterPM, List<ConsoleShipmentPM> ConsoleShipments, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                ShipmentQuery entityQuery = new ShipmentQuery(Tenant);
                var MyList = new List<House>();
                foreach (ConsoleShipmentPM item in ConsoleShipments)
                {
                    ShipmentPM houseShipment = entityQuery.GetSinglePM(item.Id, Tenant);

                    var temp = new House();
                    temp.Id = item.Id;
                    temp.ShipperReference1 = houseShipment.ShipperReference1;
                    temp.ShipperReference2 = houseShipment.ShipperReference2;
                    temp.ConsigneeReference1 = houseShipment.ConsigneeReference1;
                    temp.ConsigneeReference2 = houseShipment.ConsigneeReference2;
                    temp.HouseNo = houseShipment.House;
                    temp.HouseDate = houseShipment.HAWBDate;
                    temp.DescriptionOfGoods = houseShipment.DescriptionOfGoods;
                    temp.Commodity = houseShipment.AWBCommodityItemNumber;
                    temp.TEU = item.TEU;
                    temp.NumberOfPackages = item.NumberOfPackages;
                    temp.GrossWeight = item.GrossWeight;
                    temp.Volume = item.Volume;
                    temp.VolumetricWeight = houseShipment.VolumetricWeight;
                    temp.ChargeableWeight = item.ChargeableWeight;
                    temp.ShipmentNumber = item.ShipmentNumber;
                    temp.IsCancelled = houseShipment.IsCancelled;
                    temp.IsOperationalClosed = houseShipment.IsOperationalClosed;
                    temp.IsAccountingClosed = houseShipment.IsAccountingClosed;
                    temp.MasterShipmentDataId = item.MasterShipmentDataId;

                    CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant, "Shipment");
                    temp.CustomFields = customFieldService.CustomFieldCustomDataMapping(item, Tenant);

                    if (houseShipment.ShipmentTypeId != null)
                    {
                        ShipmentTypeQueryService ShipmentTypeService0 = new ShipmentTypeQueryService(Tenant);
                        temp.ShipmentType = ShipmentTypeService0.ShipmentTypeCustomDataMapping(houseShipment.ShipmentTypeId, Tenant);
                    }

                    if (houseShipment.TransportModeId != null)
                    {
                        TransportModeQueryService TransportModeService1 = new TransportModeQueryService(Tenant);
                        temp.TransportMode = TransportModeService1.GetTransportModeById(houseShipment.TransportModeId, Tenant);
                    }

                    if (houseShipment.ShipperId != null)
                    {
                        CardQueryService CardService2 = new CardQueryService(Tenant);
                        temp.Shipper = CardService2.GetCardById(houseShipment.ShipperId, Tenant);
                    }                    

                    if (houseShipment.ConsigneeId != null)
                    {
                        CardQueryService CardService3 = new CardQueryService(Tenant);
                        temp.Consignee = CardService3.GetCardById(houseShipment.ConsigneeId, Tenant);
                    }
                    
                    if (houseShipment.CustomerId != null)
                    {
                        CardQueryService CardService4 = new CardQueryService(Tenant);
                        temp.Customer = CardService4.GetCardById(houseShipment.CustomerId, Tenant);
                    }

                    if (houseShipment.FromPortId != null)
                    {
                        PortQueryService PortService5 = new PortQueryService(Tenant);
                        temp.FromPort = PortService5.GetPortById(houseShipment.FromPortId, Tenant);
                    }

                    if (houseShipment.ToPortId != null)
                    {
                        PortQueryService PortService6 = new PortQueryService(Tenant);
                        temp.ToPort = PortService6.GetPortById(houseShipment.ToPortId, Tenant);
                    }

                    if (houseShipment.GrossWeightUnitCode != null)
                    {
                        WeightUnitQueryService WeightUnitService7 = new WeightUnitQueryService(Tenant);
                        temp.GrossWeightUnit = WeightUnitService7.GetWeightUnitByCode(houseShipment.GrossWeightUnitCode, Tenant);
                    }

                    if (houseShipment.ChargeableWeightUnitCode != null)
                    {
                        WeightUnitQueryService WeightUnitService8 = new WeightUnitQueryService(Tenant);
                        temp.ChargeableWeightUnit = WeightUnitService8.GetWeightUnitByCode(houseShipment.ChargeableWeightUnitCode, Tenant);
                    }

                    if (houseShipment.VolumeUnitCode != null)
                    {
                        VolumeUnitQueryService VolumeUnitService9 = new VolumeUnitQueryService(Tenant);
                        temp.VolumeUnit = VolumeUnitService9.GetVolumeUnitByCode(houseShipment.VolumeUnitCode, Tenant);
                    }

                    if (houseShipment.IncotermId != null)
                    {
                        IncotermQueryService IncotermService10 = new IncotermQueryService(Tenant);
                        temp.Incoterm = IncotermService10.GetIncotermById(houseShipment.IncotermId, Tenant);
                    }
                    
                    if (houseShipment.ShipmentPackages != null && houseShipment.ShipmentPackages.Count > 0)
                    {
                        AirPackageQueryService AirPackageService11 = new AirPackageQueryService(Tenant);
                        temp.AirPackages = AirPackageService11.AirPackageCustomDataMapping(houseShipment, houseShipment.ShipmentPackages, Tenant);
                    }

                    if (houseShipment.ShipmentPackages != null && houseShipment.ShipmentPackages.Count > 0)
                    {
                        OceanOrInlandPackageQueryService OceanOrInlandPackageService11 = new OceanOrInlandPackageQueryService(Tenant);
                        temp.OceanOrInlandPackages = OceanOrInlandPackageService11.OceanOrInlandPackageCustomDataMapping(houseShipment, houseShipment.ShipmentPackages, Tenant);
                    }

                    if (houseShipment.ShipmentPackages != null && houseShipment.ShipmentPackages.Count > 0)
                    {
                        ContainerQueryService ContainerService11 = new ContainerQueryService(Tenant);
                        temp.Containers = ContainerService11.ContainerCustomDataMapping(houseShipment, houseShipment.ShipmentPackages, Tenant);
                    }
                    
                    if (houseShipment.BranchId != null)
                    {
                        BranchQueryService BranchService11 = new BranchQueryService(Tenant);
                        temp.Branch = BranchService11.GetBranchById(houseShipment.BranchId, Tenant);
                    }

                    if (houseShipment.DepartmentId != null)
                    {
                        DepartmentQueryService DepartmentService12 = new DepartmentQueryService(Tenant);
                        temp.Department = DepartmentService12.GetDepartmentById(houseShipment.DepartmentId, Tenant);
                    }
                    
                    if (houseShipment.DirectionId != null)
                    {
                        DirectionQueryService DirectionService13 = new DirectionQueryService(Tenant);
                        temp.Direction = DirectionService13.GetDirectionById(houseShipment.DirectionId, Tenant);
                    }
                    
                    if (houseShipment.CreatedByUserId != null)
                    {
                        UserQueryService UserService14 = new UserQueryService(Tenant);
                        temp.CreatedByUser = UserService14.GetUserById(houseShipment.CreatedByUserId, Tenant);
                    }

                    if (houseShipment.ShipmentPickUps != null && houseShipment.ShipmentPickUps.Count > 0)
                    {
                        PickUpQueryService PickUpService15 = new PickUpQueryService(Tenant);
                        temp.PickUps = PickUpService15.PickUpDataMapping(houseShipment.ShipmentPickUps, Tenant);
                    }

                    if (houseShipment.ShipmentDeliveries != null && houseShipment.ShipmentDeliveries.Count > 0)
                    {
                        DeliveryQueryService DeliveryService15 = new DeliveryQueryService(Tenant);
                        temp.Deliveries = DeliveryService15.DeliveryDataMapping(houseShipment.ShipmentDeliveries, Tenant);
                    }
                    
                    MyList.Add(temp);
                }

                return MyList;
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

        public ShipmentPM HouseCustomDataMappingAndValidatin(House MyEntity, int Tenant,string ComputingPartnerCode = "", bool IsUpdate = false)
        {
            try
            {
                TenantQuery tenantQuery = new TenantQuery(Tenant);
                UserQuery userQuery = new UserQuery(Tenant);
                TenantPM MyTenantPM = tenantQuery.GetSinglePM(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                ShipmentPM temp = HouseDataMappingAndValidatin(MyEntity, Tenant, ComputingPartnerCode, IsUpdate);
                temp.NewConcurrencyGUID = Guid.NewGuid().ToString();
                temp.Tenant = Tenant;
                temp.ShipmentLevelCode = "H";
                temp.MainCarriageFromPortId = temp.FromPortId;
                temp.MainCarriageToPortId = temp.ToPortId;
                temp.MainCarriageToPortId = temp.ToPortId;
                temp.FinalDistenationPortId = temp.ToPortId;
                temp.MainCarriageFinalDestinationPortId = temp.ToPortId;
                temp.FHLStatusCode = "NSEN";
                temp.FWBStatusCode = "NSEN";
                temp.FHLStatusName = "Not Sent";
                temp.FWBStatusName = "Not Sent";
                temp.ManifestStatusCode = "NSEN";
                temp.LocalCustomsTransmissionsStatusCode = "NSEN";
                temp.IsOperationalClosed = false;
                temp.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.StatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.AWBCurrencyId = MyTenantPM.FreightCurrencyId;
                temp.ProfitCurrencyId = MyTenantPM.ProfitCurrencyId;
                temp.OnForwardingAdditionalTransportModeCode = "BYTR";

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

                if(string.IsNullOrEmpty(temp.ValueOfGoodsCurrencyId))
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

                foreach (ShipmentPickUpPM item in temp.ShipmentPickUps)
                {
                    item.PickUpDeliveryTypeCode = "PICK";

                    if(!string.IsNullOrEmpty(item.FromPartnerCardId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.FromPortId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PORT";
                    }

                    if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                    {
                        item.PickUpDeliveryToTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.ToPortId))
                    {
                        item.PickUpDeliveryToTypeCode = "PORT";
                    }
                }

                foreach (ShipmentDeliveryPM item in temp.ShipmentDeliveries)
                {
                    item.PickUpDeliveryTypeCode = "DELV";

                    if (!string.IsNullOrEmpty(item.FromPartnerCardId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.FromPortId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PORT";
                    }

                    if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                    {
                        item.PickUpDeliveryToTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.ToPortId))
                    {
                        item.PickUpDeliveryToTypeCode = "PORT";
                    }
                }

                ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(Tenant);

                foreach (ShipmentReceivablePM item in temp.ShipmentReceivables)
                {
                    if (string.IsNullOrEmpty(item.CurrencyId))
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

        public List<ConsoleShipmentPM> HouseCustomDataMappingAndValidatin(Master myMaster, List<House> MyEntity, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            try
            {
                TenantQuery tenantQuery = new TenantQuery(Tenant);
                UserQuery userQuery = new UserQuery(Tenant);
                TenantPM MyTenantPM = tenantQuery.GetSinglePM(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                var MyList = new List<ConsoleShipmentPM>();
                foreach (var item in MyEntity)
                {
                    ShipmentPM temp = HouseCustomDataMappingAndValidatin(item, Tenant, ComputingPartnerName, IsUpdate);
                    temp.NewConcurrencyGUID = Guid.NewGuid().ToString();
                    temp.Tenant = Tenant;
                    temp.ShipmentLevelCode = "H";
                    temp.MainCarriageFromPortId = temp.FromPortId;
                    temp.MainCarriageToPortId = temp.ToPortId;
                    temp.MainCarriageToPortId = temp.ToPortId;
                    temp.FinalDistenationPortId = temp.ToPortId;
                    temp.MainCarriageFinalDestinationPortId = temp.ToPortId;
                    temp.FHLStatusCode = "NSEN";
                    temp.FWBStatusCode = "NSEN";
                    temp.FHLStatusName = "Not Sent";
                    temp.FWBStatusName = "Not Sent";
                    temp.ManifestStatusCode = "NSEN";
                    temp.LocalCustomsTransmissionsStatusCode = "NSEN";
                    temp.IsOperationalClosed = false;
                    temp.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
                    temp.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                    temp.StatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                    temp.AWBCurrencyId = MyTenantPM.FreightCurrencyId;
                    temp.ProfitCurrencyId = MyTenantPM.ProfitCurrencyId;
                    temp.ValueOfGoodsCurrencyId = MyTenantPM.FreightCurrencyId;
                    temp.VolumeUnitCode = MyTenantPM.VolumeUnitCode;
                    temp.DimensionsUnitCode = MyTenantPM.DimensionsUnitCode;
                    temp.GrossWeightUnitCode = MyTenantPM.GrossWeightUnitCode;
                    temp.ChargeableWeightUnitCode = MyTenantPM.ChargeableWeightUnitCode;
                    temp.OnForwardingAdditionalTransportModeCode = "BYTR";

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

                    foreach (ShipmentPickUpPM pickUpItem in temp.ShipmentPickUps)
                    {
                        pickUpItem.PickUpDeliveryTypeCode = "PICK";

                        if (!string.IsNullOrEmpty(pickUpItem.FromPartnerCardId))
                        {
                            pickUpItem.PickUpDeliveryFromTypeCode = "PART";
                        }
                        else if (!string.IsNullOrEmpty(pickUpItem.FromPortId))
                        {
                            pickUpItem.PickUpDeliveryFromTypeCode = "PORT";
                        }

                        if (!string.IsNullOrEmpty(pickUpItem.ToPartnerCardId))
                        {
                            pickUpItem.PickUpDeliveryToTypeCode = "PART";
                        }
                        else if (!string.IsNullOrEmpty(pickUpItem.ToPortId))
                        {
                            pickUpItem.PickUpDeliveryToTypeCode = "PORT";
                        }
                    }

                    foreach (ShipmentDeliveryPM deliveryItem in temp.ShipmentDeliveries)
                    {
                        deliveryItem.PickUpDeliveryTypeCode = "DELV";

                        if (!string.IsNullOrEmpty(deliveryItem.FromPartnerCardId))
                        {
                            deliveryItem.PickUpDeliveryFromTypeCode = "PART";
                        }
                        else if (!string.IsNullOrEmpty(deliveryItem.FromPortId))
                        {
                            deliveryItem.PickUpDeliveryFromTypeCode = "PORT";
                        }

                        if (!string.IsNullOrEmpty(deliveryItem.ToPartnerCardId))
                        {
                            deliveryItem.PickUpDeliveryToTypeCode = "PART";
                        }
                        else if (!string.IsNullOrEmpty(deliveryItem.ToPortId))
                        {
                            deliveryItem.PickUpDeliveryToTypeCode = "PORT";
                        }
                    }

                    //MyList.Add(temp);
                }

                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}