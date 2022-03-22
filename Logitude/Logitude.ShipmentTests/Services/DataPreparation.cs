using Logitude.ShipmentTests.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.ShipmentTests.Services
{
    public class DataPreparation
    {
        public static ShipmentVariables GetShipmentVariables()
        {
            return new ShipmentVariables
            {
                PackageTypePC1Id = GetPackageTypeId("PC1", "O", true),
                PackageTypePC2Id = GetPackageTypeId("PC2", "O", true),
                PackageTypePP1Id = GetPackageTypeId("PP1", "A", false),
                PackageTypePP2Id = GetPackageTypeId("PP2", "A", false),
                QuoteStageQTDRId = GetQuoteStageId("QTDR"),
                VesselPTId = GetVesselId("PT"),
                MoveTypeMTAId = GetMoveTypeId("MTA", "A" ),
                MoveTypeTSMId = GetMoveTypeId("TSM", "A" ),
                MoveTypeMTOId = GetMoveTypeId("MTO", "O"),
                ShipmentSubTypeTSSTId = GetShipmentSubTypeId("TSST","Air")
        };
        }

        #region PackageType

        private static string GetPackageTypeId(string code, string transportModeCode, bool isContainer)
        {
            ApiQueryFilters apiQueryFilters= BuildApiQueryFilters(null,code);
            string UserTenantPackageTypeId = GetPackageTypeIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantPackageTypeId))
            {
                UserTenantPackageTypeId = GetCreatedPackageTypeFromTenantZero(code,transportModeCode,isContainer);
            }
            return UserTenantPackageTypeId;
        }

        private static string GetPackageTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<PackageType>> response = APICaller.CallGetByFilters<IEnumerable<PackageType>>(Urls.PackageTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedPackageTypeFromTenantZero(string code, string transportModeCode, bool isContainer)
        {
            PackageType packageType = CreatePackageType(code, transportModeCode, isContainer);
            ApiResponse<PackageType> response = APICaller.CallPost<PackageType>(packageType, Urls.PackageTypesController, UserTenant.Token);
            return response.Data?.Id;

        }

        private static PackageType CreatePackageType(string packageTypeCode, string transportModeCode, bool isContainer)
        {
            PackageType PackageType = new PackageType();
            PackageType.Tenant = UserTenant.Tenant;
            PackageType.Code = packageTypeCode;
            PackageType.EnglishName = isContainer ? "ContainerId" : "PackageId" + packageTypeCode;
            PackageType.PrintAs = packageTypeCode;
            PackageType.IsAir = transportModeCode == "A" ? true : false;
            PackageType.IsOcean = transportModeCode == "O" ? true : false;
            PackageType.IsInland = transportModeCode == "I" ? true : false;
            PackageType.IsContainer = isContainer;
            return PackageType;
        }

        #endregion

        #region QuoteStage

        private static string GetQuoteStageId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(null,code);
            apiQueryFilters.GetAll = false;
            apiQueryFilters.ForceCacheRefresh = false;
            apiQueryFilters.GetCount = true;

            string UserTenantQuoteStageId = GetQuoteStageIdFromUserTenant(apiQueryFilters);
            return UserTenantQuoteStageId;
        }

        private static string GetQuoteStageIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<QuoteStage>> response = APICaller.CallGetByFilters<IEnumerable<QuoteStage>>(Urls.QuoteStageViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

        #region Vessel

        private static string GetVesselId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);

            string UserTenantVesselId = GetVesselIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantVesselId))
            {
                UserTenantVesselId = GetCreatedVesselFromTenantZero(code);
            }

            return UserTenantVesselId;
        }

        private static string GetVesselIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Vessel>> response = APICaller.CallGetByFilters<IEnumerable<Vessel>>(Urls.VesselViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedVesselFromTenantZero(string code)
        {
            Vessel vessel = CreateVesselPM(code);
            ApiResponse<Vessel> response = APICaller.CallPost<Vessel>(vessel, Urls.VesselsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private static Vessel CreateVesselPM(string vesselCode)
        {
            Vessel vessel = new Vessel();
            vessel.Tenant = UserTenant.Tenant;
            vessel.Code = vesselCode;
            vessel.EnglishName = " vesselId" + vesselCode;
            vessel.IMOCode = "IMOCode " + vesselCode;
            return vessel;
        }

        #endregion

        #region MoveType

        private static string GetMoveTypeId(string code, string moveTypeTransportMode)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(null,code); 

            string UserTenantMoveTypeId = GetMoveTypeIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantMoveTypeId))
            {
                UserTenantMoveTypeId = GetCreatedMoveTypeFromTenantZero(code, moveTypeTransportMode);
            }

            return UserTenantMoveTypeId;
        }

        private static string GetMoveTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<MoveType>> response = APICaller.CallGetByFilters<IEnumerable<MoveType>>(Urls.MoveTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedMoveTypeFromTenantZero(string moveTypeCode, string moveTypeTransportMode)
        {
            MoveType moveTypePM = CreateMoveTypePM(moveTypeCode, moveTypeTransportMode);
            ApiResponse<MoveType> response = APICaller.CallPost<MoveType>(moveTypePM, Urls.MoveTypesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private static MoveType CreateMoveTypePM(string moveTypeCode, string moveTypeTransportMode)
        {
            MoveType moveTypePM = new MoveType();
            moveTypePM.Tenant = UserTenant.Tenant;
            moveTypePM.Code = moveTypeCode;
            moveTypePM.MoveTypeEnglishName = "TestMoveTypeId" + moveTypeCode;
            moveTypePM.MoveTypeLocalName = moveTypeCode + " Move Type LocalName";
            moveTypePM.TransportModeId = moveTypeTransportMode;
            moveTypePM.IsAir = moveTypeTransportMode == "A" ? true : false;
            moveTypePM.IsOcean = moveTypeTransportMode == "O" ? true : false;
            moveTypePM.IsInland = moveTypeTransportMode == "I" ? true : false;
            return moveTypePM;
        }

        #endregion

        #region Shipment Sub Type
        private static string GetShipmentSubTypeId(string code , string shipmentTypeCpde)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);

            string UserTenantShipmentSubId = GetShipmentSubIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantShipmentSubId))
            {
                UserTenantShipmentSubId = GetCreatedShipmentSubFromTenantZero(code, shipmentTypeCpde);
            }

            return UserTenantShipmentSubId;
        }

        private static string GetShipmentSubIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<ShipmentSubTypePM>> response = APICaller.CallGetByFilters<IEnumerable<ShipmentSubTypePM>>(Urls.ShipmentSubTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedShipmentSubFromTenantZero(string code , string shipmentTypeCpde)
        {
            ShipmentSubTypePM ShipmentSub = CreateShipmentSubPM(code , shipmentTypeCpde);
            ApiResponse<ShipmentSubTypePM> response = APICaller.CallPost<ShipmentSubTypePM>(ShipmentSub, Urls.ShipmentSubTypesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private static ShipmentSubTypePM CreateShipmentSubPM(string code,string shipmentTypeCpde)
        {
            ShipmentSubTypePM ShipmentSub = new ShipmentSubTypePM();
            ShipmentSub.Tenant = UserTenant.Tenant;
            ShipmentSub.Code = code;
            ShipmentSub.Name = "Test Shipment Sub Type";
            ShipmentSub.ShipmentTypeCode = shipmentTypeCpde;
            ShipmentSub.CreateDate = DateTime.Now;
            ShipmentSub.UpdateDate = DateTime.Now;
            ShipmentSub.CreatedByUserId = UserTenant.UserId;
            ShipmentSub.UpdatedByUserId = UserTenant.UserId;
            return ShipmentSub;
        }
        #endregion

        #region Build ApiQueryFilters
        private static ApiQueryFilters BuildApiQueryFilters(string code ,string SearchFieldsCode)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Operator("equals")
                .Filter1Value(code)
                .Filter2Name("SearchFields")
                .Filter2Operator("Contains")
                .Filter2Value(SearchFieldsCode)
                .Build();
        }
        #endregion

    }
}