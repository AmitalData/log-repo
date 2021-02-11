using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
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
                CurrencyEURId = GetCurrencyId("EUR"),
                IncotermLDEId = GetIncotermId("LDE"),
                MeasurementGRWTId = GetMeasurementId("GRWT"),
                ChargeTypeAFTId = GetChargeTypeId("AFT"),
                PackageTypePC1Id = GetPackageTypeId("PC1", "O", true),
                PackageTypePC2Id = GetPackageTypeId("PC2", "O", true),
                PackageTypePP1Id = GetPackageTypeId("PP1", "A", false),
                PackageTypePP2Id = GetPackageTypeId("PP2", "A", false),
                PaymentTermCashId = GetPaymentTermId("Cash"),
                VATTypeZeroId = GetVATTypeId("ZERO"),
                QuoteStageQTDRId = GetQuoteStageId("QTDR"),
                VesselPTId = GetVesselId("PT"),
                MoveTypeMTAId = GetMoveTypeId("MTA", "A" ),
                MoveTypeMTOId = GetMoveTypeId("MTO", "O"),
        };
        }

        #region Currency
        private static string GetCurrencyId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code,null);  

            string UserTenantCurrencyId = GetCurrencyIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantCurrencyId))
            {
                string ZeroTenantCurrencyId = GetCurrencyIdFromZeroTenant(apiQueryFilters);
                UserTenantCurrencyId = GetCopiedCurrencyFromTenantZero(ZeroTenantCurrencyId);
            }
            return UserTenantCurrencyId;
        }

        private static string GetCurrencyIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Currency>> response = APICaller.CallGetByFilters<IEnumerable<Currency>>(Urls.CurrencyViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }
        private static string GetCurrencyIdFromZeroTenant(ApiQueryFilters apiQueryFilters)
        {
            apiQueryFilters.Tenant = 0;
            ApiResponse<IEnumerable<Currency>> response = APICaller.CallGetByFilters<IEnumerable<Currency>>(Urls.CurrencyViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCopiedCurrencyFromTenantZero(string currencyId)
        {
            ApiResponse<Currency> response = APICaller.CallGet<Currency>(Urls.CommonDomainGetCopyCurrencyToTenant(currencyId), UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #region Incoterm
        private static string GetIncotermId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code,null);

            string UserTenantIncotermId = GetIncotermIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantIncotermId))
            {
                UserTenantIncotermId = GetCreatedIncotermFromTenantZero(code);
            }

            return UserTenantIncotermId;
        }

        private static string GetIncotermIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Incoterm>> response = APICaller.CallGetByFilters<IEnumerable<Incoterm>>(Urls.IncotermViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedIncotermFromTenantZero(string code)
        {
            Incoterm incoterm = CreateIncotermPM(code);
            ApiResponse<Incoterm> response = APICaller.CallPost<Incoterm>(incoterm, Urls.IncotermsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private static Incoterm CreateIncotermPM(string code)
        {
            Incoterm incoterm = new Incoterm();
            incoterm.Tenant = UserTenant.Tenant;
            incoterm.Code = code;
            incoterm.Name = code + " Incoterm";
            incoterm.Freight = "P";
            incoterm.OtherCharges = "P";
            return incoterm;
        }

        #endregion

        #region Measurement

        private static string GetMeasurementId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code,null);

            string UserTenantMeasurementId = GetMeasurementIdFromUserTenant(apiQueryFilters);
            return UserTenantMeasurementId;
        }

        private static string GetMeasurementIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Measurement>> response = APICaller.CallGetByFilters<IEnumerable<Measurement>>(Urls.MeasurementViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

        #region ChargeType

        private static string GetChargeTypeId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code,null);

            string UserTenantChargeTypeId = GetChargeTypeIdFromUserTenant(apiQueryFilters);
            return UserTenantChargeTypeId;
        }

        private static string GetChargeTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<ChargeType>> response = APICaller.CallGetByFilters<IEnumerable<ChargeType>>(Urls.ChargeTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

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

        #region PaymentTerm

        private static string GetPaymentTermId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(null,code);

            string UserTenantPaymentTermId = GetPaymentTermIdFromUserTenant(apiQueryFilters);
            return UserTenantPaymentTermId;
        }

        private static string GetPaymentTermIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<PaymentTerm>> response = APICaller.CallGetByFilters<IEnumerable<PaymentTerm>>(Urls.PaymentTermViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

        #region VATType

        private static string GetVATTypeId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(null,code);
            apiQueryFilters.GetAll = false;
            apiQueryFilters.ForceCacheRefresh = false;
            apiQueryFilters.GetCount = true;
            string UserTenantVATTypeId = GetVATTypeIdFromUserTenant(apiQueryFilters);
            return UserTenantVATTypeId;
        }

        private static string GetVATTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<VATType>> response = APICaller.CallGetByFilters<IEnumerable<VATType>>(Urls.VatTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
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

        #region Build ApiQueryFilters
        private static ApiQueryFilters BuildApiQueryFilters(string code ,string SearchFieldsCode)
        {
            return new ApiQueryFilters 
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "Code",
                Filter1Operator = "equals",
                Filter1Value = code ,
                Filter2Name = "SearchFields",
                Filter2Operator = "Contains",
                Filter2Value = SearchFieldsCode
            };
        }
        #endregion

    }
}