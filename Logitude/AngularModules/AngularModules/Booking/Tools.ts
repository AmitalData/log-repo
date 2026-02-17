import {BookingPM} from './EntityPMs/BookingPM';
import {BookingPackagePM} from './EntityPMs/BookingPackagePM';
import {ShipmentPM} from '../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../Shipment/EntityPMs/ShipmentPackagePM';
import {DateTool} from '../Infrastructure/Tools';
import {InfraSettings} from '../Infrastructure/Utilities/InfraSettings';

export class BookingTool {
    public static IsEditingFieldsEnabled_MasterPorts(entityPM: BookingPM) {
        var myResult: boolean = true;

        if (entityPM.IsCancelled) {
            myResult = false;
        }

        else if (entityPM.BookingStatusCode != "CRT") {
            myResult = false;
        }

        return myResult;
    }
    public static IsEditingFieldsEnabled_Others(entityPM: BookingPM) {
        var myResult: boolean = true;

        if (entityPM.IsCancelled) {
            myResult = false;
        }

        else if (entityPM.BookingStatusCode == "AWB") {
            myResult = false;
        }

        return myResult;
    }

    public static CopyBooking(entityPM: BookingPM, copiedEntityPM: BookingPM) {

        entityPM.IsCopyMode = true;
        entityPM.SpaceAllocationCode = copiedEntityPM.SpaceAllocationCode;
        entityPM.MainCarriageCarrierId = copiedEntityPM.MainCarriageCarrierId;
        entityPM.MainCarriageCarrierName = copiedEntityPM.MainCarriageCarrierName;
        entityPM.FirstFlight = copiedEntityPM.FirstFlight;
        entityPM.AirlinePrefix = copiedEntityPM.AirlinePrefix;
        entityPM.InterlineId = copiedEntityPM.InterlineId;
        entityPM.MainCarriageFromPortId = copiedEntityPM.MainCarriageFromPortId;
        entityPM.MainCarriageToPortId = copiedEntityPM.MainCarriageToPortId;
        entityPM.MainFromPortCode = copiedEntityPM.MainFromPortCode;
        entityPM.MainFromPortName = copiedEntityPM.MainFromPortName;
        entityPM.MainFromPortCountryCode = copiedEntityPM.MainFromPortCountryCode;
        entityPM.MainFromPortCountryName = copiedEntityPM.MainFromPortCountryName;
        entityPM.MainToPortCode = copiedEntityPM.MainToPortCode;
        entityPM.MainToPortName = copiedEntityPM.MainToPortName;
        entityPM.MainToPortCountryCode = copiedEntityPM.MainToPortCountryCode;
        entityPM.MainToPortCountryName = copiedEntityPM.MainToPortCountryName;
        entityPM.MainCarriageCarrierPrefix = copiedEntityPM.MainCarriageCarrierPrefix;
        entityPM.MainCarriageCarrierNumber = copiedEntityPM.MainCarriageCarrierNumber;
        entityPM.MainCarriageETD = copiedEntityPM.MainCarriageETD;
        entityPM.MainCarriageSpaceAllocationCode = copiedEntityPM.MainCarriageSpaceAllocationCode;
        entityPM.MainCarriageAllotmentIdentification = copiedEntityPM.MainCarriageAllotmentIdentification;
        entityPM.Transshipment1FromPortId = copiedEntityPM.Transshipment1FromPortId;
        entityPM.Transshipment1ToPortId = copiedEntityPM.Transshipment1ToPortId;
        entityPM.Trans1FromPortCode = copiedEntityPM.Trans1FromPortCode;
        entityPM.Trans1FromPortName = copiedEntityPM.Trans1FromPortName;
        entityPM.Trans1FromPortCountryCode = copiedEntityPM.Trans1FromPortCountryCode;
        entityPM.Trans1FromPortCountryName = copiedEntityPM.Trans1FromPortCountryName;
        entityPM.Trans1ToPortCode = copiedEntityPM.Trans1ToPortCode;
        entityPM.Trans1ToPortName = copiedEntityPM.Trans1ToPortName;
        entityPM.Trans1ToPortCountryCode = copiedEntityPM.Trans1ToPortCountryCode;
        entityPM.Trans1ToPortCountryName = copiedEntityPM.Trans1ToPortCountryName;
        entityPM.Transshipment1CarrierId = copiedEntityPM.Transshipment1CarrierId;
        entityPM.Transshipment1CarrierName = copiedEntityPM.Transshipment1CarrierName;
        entityPM.Transshipment1CarrierPrefix = copiedEntityPM.Transshipment1CarrierPrefix;
        entityPM.Transshipment1CarrierNumber = copiedEntityPM.Transshipment1CarrierNumber;
        entityPM.Transshipment1ETD = copiedEntityPM.Transshipment1ETD;
        entityPM.Transshipment1SpaceAllocationCode = copiedEntityPM.Transshipment1SpaceAllocationCode;
        entityPM.Transshipment1AllotmentIdentification = copiedEntityPM.Transshipment1AllotmentIdentification;
        entityPM.Transshipment2FromPortId = copiedEntityPM.Transshipment2FromPortId;
        entityPM.Transshipment2ToPortId = copiedEntityPM.Transshipment2ToPortId;
        entityPM.Trans2FromPortCode = copiedEntityPM.Trans2FromPortCode;
        entityPM.Trans2FromPortName = copiedEntityPM.Trans2FromPortName;
        entityPM.Trans2FromPortCountryCode = copiedEntityPM.Trans2FromPortCountryCode;
        entityPM.Trans2FromPortCountryName = copiedEntityPM.Trans2FromPortCountryName;
        entityPM.Trans2ToPortCode = copiedEntityPM.Trans2ToPortCode;
        entityPM.Trans2ToPortName = copiedEntityPM.Trans2ToPortName;
        entityPM.Trans2ToPortCountryCode = copiedEntityPM.Trans2ToPortCountryCode;
        entityPM.Trans2ToPortCountryName = copiedEntityPM.Trans2ToPortCountryName;
        entityPM.Transshipment2CarrierId = copiedEntityPM.Transshipment2CarrierId;
        entityPM.Transshipment2CarrierName = copiedEntityPM.Transshipment2CarrierName;
        entityPM.Transshipment2CarrierPrefix = copiedEntityPM.Transshipment2CarrierPrefix;
        entityPM.Transshipment2CarrierNumber = copiedEntityPM.Transshipment2CarrierNumber;
        entityPM.Transshipment2ETD = copiedEntityPM.Transshipment2ETD;
        entityPM.Transshipment2SpaceAllocationCode = copiedEntityPM.Transshipment2SpaceAllocationCode;
        entityPM.Transshipment2AllotmentIdentification = copiedEntityPM.Transshipment2AllotmentIdentification;
        entityPM.MainCarriageFinalDestinationPortId = copiedEntityPM.MainCarriageFinalDestinationPortId;
        entityPM.FinalDestinationPortCode = copiedEntityPM.FinalDestinationPortCode;
        entityPM.ShipperId = copiedEntityPM.ShipperId;
        entityPM.ShipperAddressId = copiedEntityPM.ShipperAddressId;
        entityPM.ConsigneeId = copiedEntityPM.ConsigneeId;
        entityPM.ConsigneeAddressId = copiedEntityPM.ConsigneeAddressId;
        entityPM.IssuingCarrierAgentId = copiedEntityPM.IssuingCarrierAgentId;
        entityPM.IssuingCarrierAddressId = copiedEntityPM.IssuingCarrierAddressId;
        entityPM.CASSCode = copiedEntityPM.CASSCode;
        entityPM.IssuingCarrierIATACode = copiedEntityPM.IssuingCarrierIATACode;
        entityPM.GrossWeight = copiedEntityPM.GrossWeight;
        entityPM.GrossWeightInKG = copiedEntityPM.GrossWeightInKG;
        entityPM.GrossWeightEdited = copiedEntityPM.GrossWeightEdited;
        entityPM.ChargeableWeight = copiedEntityPM.ChargeableWeight;
        entityPM.ChargeableWeightInKG = copiedEntityPM.ChargeableWeightInKG;
        entityPM.ChargeableWeightEdited = copiedEntityPM.ChargeableWeightEdited;
        entityPM.Volume = copiedEntityPM.Volume;
        entityPM.VolumetricWeight = copiedEntityPM.VolumetricWeight;
        entityPM.NumberOfPackages = copiedEntityPM.NumberOfPackages;
        entityPM.DescriptionOfGoods = copiedEntityPM.DescriptionOfGoods;
        entityPM.AWBCommodityItemNumber = copiedEntityPM.AWBCommodityItemNumber;
        entityPM.IsDangerous = copiedEntityPM.IsDangerous;
        entityPM.CarrierIsChampRegistered = copiedEntityPM.CarrierIsChampRegistered;
        entityPM.CarrierIsGLSHKRegistered = copiedEntityPM.CarrierIsGLSHKRegistered;
        entityPM.TenantZeroAirlineId = copiedEntityPM.TenantZeroAirlineId;
        entityPM.TenantZeroAirlineTTY = copiedEntityPM.TenantZeroAirlineTTY;
        entityPM.TenantZeroAirlinePIMA = copiedEntityPM.TenantZeroAirlinePIMA;
        entityPM.TenantZeroAirlineChampFFR = copiedEntityPM.TenantZeroAirlineChampFFR;
        entityPM.TenantZeroAirlineChampFVR = copiedEntityPM.TenantZeroAirlineChampFVR;
        entityPM.TenantZeroAirlineGLSHKFFR = copiedEntityPM.TenantZeroAirlineGLSHKFFR;
        entityPM.TenantZeroAirlineGLSHKFVR = copiedEntityPM.TenantZeroAirlineGLSHKFVR;
        entityPM.ZeroChampNeedsRegistration = copiedEntityPM.ZeroChampNeedsRegistration;
        entityPM.ZeroGLSHKNeedsRegistration = copiedEntityPM.ZeroGLSHKNeedsRegistration;
        entityPM.TenantZeroIsManagingProduct = copiedEntityPM.TenantZeroIsManagingProduct;
        entityPM.TenantZeroIsProductMandatory = copiedEntityPM.TenantZeroIsProductMandatory;
        entityPM.ZeroIsDescOfGoodsFromList = copiedEntityPM.ZeroIsDescOfGoodsFromList;
        entityPM.CarrierIsCheckDigit = copiedEntityPM.CarrierIsCheckDigit;
        entityPM.CarrierIsLimitedLength = copiedEntityPM.CarrierIsLimitedLength;
        entityPM.AccountNumber = copiedEntityPM.AccountNumber;
    }
    public static CopyBookingPackages(entityPM: BookingPM, copiedEntityPM: BookingPM) {
        copiedEntityPM.BookingPackages.forEach(item => {
            var newItem: BookingPackagePM = new BookingPackagePM(entityPM);

            newItem.ClassNumber = item.ClassNumber;
            newItem.CommodityId = item.CommodityId;
            newItem.ContainerNumber = item.ContainerNumber;
            newItem.Description = item.Description;
            newItem.FlashPoint = item.FlashPoint;
            newItem.Harmonize = item.Harmonize;
            newItem.Height = item.Height;
            newItem.IMDGCode = item.IMDGCode;
            newItem.IsDangerous = item.IsDangerous;
            newItem.Length = item.Length;
            newItem.MarksAndNumbers = item.MarksAndNumbers;
            newItem.MaterialDescription = item.MaterialDescription;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.OriginalBookingPackageId = item.OriginalBookingPackageId,
                newItem.PackagingGroup = item.PackagingGroup;
            newItem.Quantity = item.Quantity;
            newItem.Seal = item.Seal;
            newItem.Seal2 = item.Seal2;
            newItem.SOC = item.SOC;
            newItem.Tare = item.Tare;
            newItem.Temperature = item.Temperature;
            newItem.Tenant = item.Tenant,
                newItem.UnNumber = item.UnNumber;
            newItem.Ventilation = item.Ventilation;
            newItem.Volume = item.Volume;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.Weight = item.Weight;
            newItem.Width = item.Width;

            entityPM.BookingPackages.push(newItem);
        });
    }

    public static BuildShipment(entityPM: BookingPM) {
        var shipmentPM: ShipmentPM = new ShipmentPM();

        //General
        shipmentPM.IsBuildFromBooking = true;
        shipmentPM.BookingId = entityPM.Id;
        shipmentPM.Tenant = entityPM.Tenant;
        shipmentPM.ShipmentTypeId = null;
        shipmentPM.DirectionId = entityPM.DirectionCode;
        shipmentPM.TransportModeId = entityPM.TransportModeCode;
        shipmentPM.Notes = entityPM.Notes;
        shipmentPM.MAWBTakenFromStack = entityPM.MAWBTakenFromStack;
        shipmentPM.MAWBReturnedToStack = entityPM.MAWBReturnedToStack;
        shipmentPM.InterlineId = entityPM.InterlineId;
        shipmentPM.AirlinePrefix = entityPM.AirlinePrefix;
        shipmentPM.LongMaster = entityPM.LongMaster;
        shipmentPM.MAWBOBLDate = DateTool.GetCurrentDateTimeAsUtc();

        //Totals
        shipmentPM.GrossWeightEdited = entityPM.GrossWeightEdited;
        shipmentPM.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;
        shipmentPM.NumberOfPackages = entityPM.NumberOfPackages;
        shipmentPM.DescriptionOfGoods = entityPM.DescriptionOfGoods;
        shipmentPM.AWBCommodityItemNumber = entityPM.AWBCommodityItemNumber;
        shipmentPM.Ratio = entityPM.Ratio;
        shipmentPM.Volume = entityPM.Volume;
        shipmentPM.DimFactor = entityPM.DimFactor;
        shipmentPM.GrossWeight = entityPM.GrossWeight;
        shipmentPM.VolumetricWeight = entityPM.VolumetricWeight;
        shipmentPM.ChargeableWeight = entityPM.ChargeableWeight;
        shipmentPM.GrossWeightInKG = entityPM.GrossWeightInKG;
        shipmentPM.GrossWeightPerTon = entityPM.GrossWeightInKG == null ? null : entityPM.GrossWeightInKG / 1000;
        shipmentPM.ChargeableWeightInKG = entityPM.ChargeableWeightInKG;
        shipmentPM.VolumeUnitCode = entityPM.VolumeUnitCode;
        shipmentPM.DimensionsUnitCode = entityPM.DimensionsUnitCode;
        shipmentPM.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
        shipmentPM.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;

        //Routing
        shipmentPM.Master = entityPM.Master;
        shipmentPM.Routing = entityPM.Routing;

        shipmentPM.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
        shipmentPM.MainCarriageIsFromStack = entityPM.MainCarriageIsFromStack;
        shipmentPM.MainCarriageFromPortId = entityPM.MainCarriageFromPortId;
        shipmentPM.MainCarriageToPortId = entityPM.MainCarriageToPortId;
        shipmentPM.MainCarriageFinalDestinationPortId = entityPM.MainCarriageFinalDestinationPortId;
        shipmentPM.MainCarriageCarrierPrefix = entityPM.MainCarriageCarrierPrefix;
        shipmentPM.MainCarriageCarrierNumber = entityPM.MainCarriageCarrierNumber;
        shipmentPM.MainCarriageETD = entityPM.MainCarriageETD;
        shipmentPM.AccountNumber = entityPM.AccountNumber;

        shipmentPM.AWBPlace = entityPM.MainFromPortName + " " + entityPM.MainFromPortCountryName;

        shipmentPM.Transshipment1FromPortId = entityPM.Transshipment1FromPortId;
        shipmentPM.Transshipment1ToPortId = entityPM.Transshipment1ToPortId;
        shipmentPM.Transshipment1CarrierId = entityPM.Transshipment1CarrierId;
        shipmentPM.Transshipment1ETD = entityPM.Transshipment1ETD;
        shipmentPM.Transshipment1CarrierPrefix = entityPM.Transshipment1CarrierPrefix;
        shipmentPM.Transshipment1CarrierNumber = entityPM.Transshipment1CarrierNumber;

        shipmentPM.Transshipment2FromPortId = entityPM.Transshipment2FromPortId;
        shipmentPM.Transshipment2ToPortId = entityPM.Transshipment2ToPortId;
        shipmentPM.Transshipment2CarrierId = entityPM.Transshipment2CarrierId;
        shipmentPM.Transshipment2ETD = entityPM.Transshipment2ETD;
        shipmentPM.Transshipment2CarrierPrefix = entityPM.Transshipment2CarrierPrefix;
        shipmentPM.Transshipment2CarrierNumber = entityPM.Transshipment2CarrierNumber;

        //Partners
        shipmentPM.ShipperId = entityPM.ShipperId;
        shipmentPM.ShipperName = entityPM.ShipperName;
        shipmentPM.ShipperReference1 = entityPM.ShipperReference;

        shipmentPM.ConsigneeId = entityPM.ConsigneeId;
        shipmentPM.ConsigneeName = entityPM.ConsigneeName;
        shipmentPM.ConsigneeReference1 = entityPM.ConsigneeReference;

        shipmentPM.CustomerId = entityPM.ShipperId;
        shipmentPM.CustomerReference1 = entityPM.ShipperReference;
        shipmentPM.CustomerName = entityPM.ShipperName;
        shipmentPM.ShipmentCustomerTypeCode = "SHI";

        shipmentPM.IssuingCarrierAgentId = entityPM.IssuingCarrierAgentId;
        shipmentPM.IssuingCarrierAddressId = entityPM.IssuingCarrierAddressId;
        shipmentPM.CASSCode = entityPM.CASSCode;
        shipmentPM.IssuingCarrierIATACode = entityPM.IssuingCarrierIATACode;
        shipmentPM.ViaColoader = (entityPM.IssuingCarrierAgentId == InfraSettings.TenantPM.AgentId) ? false : true;

        //AWB Fields
        shipmentPM.AWBComments = entityPM.SpecialServicesRequest;
        shipmentPM.AWBSpecialHandlingCodeId1 = entityPM.AWBSpecialHandlingCodeId1;
        shipmentPM.AWBSpecialHandlingCodeId2 = entityPM.AWBSpecialHandlingCodeId2;
        shipmentPM.AWBSpecialHandlingCodeId3 = entityPM.AWBSpecialHandlingCodeId3;
        shipmentPM.AWBSpecialHandlingCodeId4 = entityPM.AWBSpecialHandlingCodeId4;
        shipmentPM.AWBSpecialHandlingCodeId5 = entityPM.AWBSpecialHandlingCodeId5;
        shipmentPM.AWBSpecialHandlingCodeId6 = entityPM.AWBSpecialHandlingCodeId6;
        shipmentPM.AWBSpecialHandlingCodeId7 = entityPM.AWBSpecialHandlingCodeId7;
        shipmentPM.AWBSpecialHandlingCodeId8 = entityPM.AWBSpecialHandlingCodeId8;
        shipmentPM.AWBSpecialHandlingCodeId9 = entityPM.AWBSpecialHandlingCodeId9;
        shipmentPM.AWBCarrierTarrifReference = entityPM.AWBCarrierTarrifReference;

        //IsDangerous
        shipmentPM.IsDangerous = entityPM.IsDangerous;
        shipmentPM.DangerousClassNumber = entityPM.DangerousClassNumber;
        shipmentPM.DangerousUnNumber = entityPM.DangerousUnNumber;
        shipmentPM.DangerousPackagingGroup = entityPM.DangerousPackagingGroup;
        shipmentPM.DangerousIMDGCode = entityPM.DangerousIMDGCode;
        shipmentPM.DangerousFlashPoint = entityPM.DangerousFlashPoint;
        shipmentPM.DangerousMaterialDescription = entityPM.DangerousMaterialDescription;
        shipmentPM.MainHarmonize = entityPM.MainHarmonize;

        //Packages
        entityPM.BookingPackages.forEach(item => {
            var newItem: ShipmentPackagePM = new ShipmentPackagePM(shipmentPM);

            newItem.Tenant = item.Tenant;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.PackageTypeName = item.PackageTypeName;
            newItem.Description = item.Description;
            newItem.ContainerNumber = item.ContainerNumber;
            newItem.ShipperSeal = item.Seal;
            newItem.CarrierSeal = item.Seal2;
            newItem.Quantity = item.Quantity;
            newItem.Weight = item.Weight;
            newItem.Volume = item.Volume;
            newItem.Height = item.Height;
            newItem.Length = item.Length;
            newItem.Width = item.Width;
            newItem.Tare = item.Tare;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.UnNumber = item.UnNumber;
            newItem.ClassNumber = item.ClassNumber;
            newItem.Temperature = item.Temperature;
            newItem.Ventilation = item.Ventilation;
            newItem.SOC = item.SOC;
            newItem.MarksAndNumbers = item.MarksAndNumbers;
            newItem.PackagingGroup = item.PackagingGroup;
            newItem.IMDGCode = item.IMDGCode;
            newItem.FlashPoint = item.FlashPoint;
            newItem.Harmonize = item.Harmonize;
            newItem.MaterialDescription = item.MaterialDescription;
            newItem.IsDangerous = item.IsDangerous;
            newItem.CommodityId = item.CommodityId;

            //    PackageTypeList list = PackageTypeDataProvider.GetCachedList<PackageTypeList>().Where(r => r.Id == item.PackageTypeId).FirstOrDefault();
            //if (list != null) {
            //    newPackagePM.PackageTypeName = list.EnglishName;

            shipmentPM.ShipmentPackages.push(newItem);
        });

        return shipmentPM;
    }
}