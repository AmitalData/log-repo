using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Helpers;
using System.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity.Core;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.QuoteModel;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentValidating
    {
        public static void Validate(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity, ICommonDataContext myCommonContext, Tenant loggedTenant, bool isPatchUpdate = false)
        {
            if (isNewEntity)
            {
                ValidateShipmentNumber(entityPM);
                ValidateProductTypePermission(entityPM, myCommonContext);
            }

            else
            {
                if (!isPatchUpdate)
                {
                    ValidateConcurrencyGUID(entityPM, entityPoco);
                }
            }

            if (entityPM.DirectionId.ToUpper() == "D")
            {
                ValidateDomesticShipment(entityPM);
            }

            if (entityPM.Ratio > 10 || entityPM.Ratio < 1)
            {
                throw new ApplicationException("Ratio must be between 1-10");
            }

            if (!loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
            {
                ValidateFromPort(entityPM, loggedTenant);
                ValidateToPort(entityPM, loggedTenant);
                ValidateCarrierPrefix(entityPM);
                ValidateAirlineRestriction(entityPM);
                ValidateShipmentBookingFields(entityPM, isNewEntity);
                ValidateCreditLimitSetting(entityPM, entityPoco, myCommonContext, loggedTenant, isNewEntity);
                ValidateConvertShipmentType(entityPM);
            }

            if (!entityPM.IsHybrid)
            {
                ValidateContainerNumbers(entityPM);
                ValidateMasterTypeDueToTransportMode(entityPM);
                ValidateMainCarriageCarrierDueToTransportMode(entityPM);
                ValidatePartnerTypes(entityPM);
                ValidateShipmentSubType(entityPM);

                if (entityPM.IsMultiUpdate || FeatureToggleHelper.HasFeatureToggle("UNV", entityPM.Tenant))
                {
                    ValidateShipmentOperationalClose(entityPM, entityPoco);
                    ValidateShipmentAccountingClose(entityPM, entityPoco);

                    ValidateShipmentOperationalReOpen(entityPM, entityPoco);
                    ValidateShipmentAccountingReOpen(entityPM, entityPoco);
                }

                if (FeatureToggleHelper.HasFeatureToggle("UNV", entityPM.Tenant))
                {
                    ValidateRoutingDates(entityPM, entityPoco);
                }
            }
        }

        private static void ValidateShipmentNumber(ShipmentPM entityPM)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(entityPM.Tenant);
            bool isShipmentNumberExist = shipmentRepository.CheckShipmentExistsByNumber(entityPM.ShipmentNumber, entityPM.Tenant);
            if (!isShipmentNumberExist) return;
            throw new ApplicationException("This shipment number is already exists");
        }

        public static string GetCustomerCreditLimitDetails(string customerId, string quoteId, bool isBuildFromQuote, int tenant)
        {
            var limitWarningMsg = "";
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            string myCreditLimitSettingsId = tenant.ToString();
            CreditLimitSetting mySettings = (from d in myCommonContext.CreditLimitSettings where d.Id == myCreditLimitSettingsId select d).FirstOrDefault();
            CustomerRepository myCustomerRepository = new CustomerRepository(myCommonContext);
            Customer myCustomer = myCustomerRepository.GetSingleCustomer(customerId, tenant, false);
            var IsCreditLimitActivated = mySettings.IsCreditLimitEnabled;
            var IsCreditLimitHasAction = (mySettings.ShipmentCreationWarning == true) ? true : false;

            if (IsCreditLimitActivated && IsCreditLimitHasAction && myCustomer != null && myCustomer.IsCreditLimitEnabled && FeatureToggleHelper.HasFeatureToggle("SWC", tenant))
            {
                if (myCustomer.CreditLimitAmount != null)
                {
                    IInvoiceContext myContext = InvoiceContext.GetContext(tenant);
                    ARInvoiceRepository invoiceRepository = new ARInvoiceRepository(myContext);
                    ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(invoiceRepository);
                    double? myResult = invoiceQuery.GetCustomerCreditLimitActualAmount(customerId, tenant);
                    double LimitAmount = myCustomer.CreditLimitAmount == null ? 0 : myCustomer.CreditLimitAmount.Value;
                    double ActualBalance = myResult == null ? 0 : myResult.Value;
                    int? WarningPercentage = myCustomer.CreditLimitWarningPercentage;

                    if (myCustomer.CreditLimitOpenBalance != null)
                    {
                        ActualBalance += myCustomer.CreditLimitOpenBalance.Value;
                    }

                    if (isBuildFromQuote && quoteId != null)
                    {
                        IQuotesContext quotesContext = QuotesContext.GetContext(tenant);
                        double? quoteSaleLocalAmount = (from d in quotesContext.QuoteCharges
                                                        where d.QuoteId == quoteId
                                                        select d.SaleTotalAmountLocal).Sum();

                        if (quoteSaleLocalAmount != null)
                        {
                            ActualBalance += quoteSaleLocalAmount.Value;
                        }
                    }

                    LimitAmount = MethodHelper.Roundd(LimitAmount, 2);
                    ActualBalance = MethodHelper.Roundd(ActualBalance, 2);

                    if (WarningPercentage != null && (ActualBalance > (WarningPercentage * LimitAmount / 100)) && ActualBalance <= LimitAmount)
                    {
                        if (mySettings.ShipmentCreationWarning)
                        {
                            var RemainingLimit = LimitAmount - ActualBalance;
                            limitWarningMsg = "The remaining credit limit for this customer is " + RemainingLimit;
                        }
                    }
                }
            }

            return limitWarningMsg;
        }
        private static void ValidateProductTypePermission(ShipmentPM entityPM, ICommonDataContext myCommonContext)
        {
            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            UserRepository userRepository = new UserRepository(myCommonContext);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, entityPM.Tenant, false);

            if (loggedUser != null)
            {
                if (loggedUser.IsProductRestricted)
                {
                    UserPermittedProductRepository productRep = new UserPermittedProductRepository(myCommonContext);
                    List<UserPermittedProduct> products = productRep.GetUserPermittedProductesByUserId(loggedUser.Id, entityPM.Tenant).ToList();

                    string productCode = null;
                    if (entityPM.DirectionId == "C")
                    {
                        productCode = "CI";
                    }
                    else
                    {
                        productCode = entityPM.TransportModeId + entityPM.DirectionId;
                    }

                    if (!products.Where(d => d.ProductTypeCode == productCode).Any())
                    {
                        throw new ApplicationException("You have no permission to add this type of products.");
                    }
                }
            }
        }
        private static void ValidateConcurrencyGUID(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (!entityPM.IsUpdatedByChampAnalyzer && !entityPM.IsDocsKPIsUpdatedFromWR)
            {
                if (!entityPM.ConcurrencyGUID.Equals(entityPoco.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPoco.ConcurrencyGUID))
                {
                    string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);

                    if (entityPoco.UpdatedByPartner != null)
                    {
                        msg = msg.Replace("another user", entityPoco.UpdatedByPartner);
                    }

                    throw new OptimisticConcurrencyException(msg);
                }
            }
            if (!entityPM.IsUpdatedOceanInsightsAnalyzer && !entityPM.IsHybrid)
            {
                ValidateOceanInsightsConcurrencyGUID(entityPM, entityPoco);
            }
        }

        private static void ValidateOceanInsightsConcurrencyGUID(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (entityPM.OIConcurrencyGUID != entityPoco.OIConcurrencyGUID && entityPM.OINewConcurrencyGUID != entityPoco.OIConcurrencyGUID)
            {
                HandelThrowExcptionForOceanInsightsConcurrency(entityPM, entityPoco);
            }
        }

        private static void HandelThrowExcptionForOceanInsightsConcurrency(ShipmentPM entityPM, Shipment entityPoco)
        {
            string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            if (entityPoco.UpdatedByPartner != null)
            {
                msg = msg.Replace("another user", entityPoco.UpdatedByPartner);
            }
            throw new OptimisticConcurrencyException(msg);
        }

        private static void ValidateDomesticShipment(ShipmentPM entityPM)
        {
            if (entityPM.DirectionId.ToUpper() == "D")
            {
                bool isInlandDomestic = entityPM.TransportModeId.ToUpper() == "I" ? true : false;

                if (isInlandDomestic)
                {
                    if (entityPM.ShipmentLevelCode == "H")
                    {
                        throw new ApplicationException("Inland domestic house shipments are not allowed");
                    }

                    else if (entityPM.ShipmentLevelCode == "C")
                    {
                        throw new ApplicationException("Inland domestic Master shipments are not allowed");
                    }

                    else
                    {
                        ValidateInlandDomesticCasualAddressFields(entityPM);
                        List<DomesticCountry> iDomesticCountries = GetInlandDomesticCountries(entityPM);

                        if (iDomesticCountries.GroupBy(g => g.CountryId).Count() > 1)
                        {
                            bool isAllPortsEC = iDomesticCountries.Where(d => d.CountryIsEC == false).Any() ? false : true;
                            bool isAllPortsNA = iDomesticCountries.Where(d => d.CountryIsNorthAmerica == false).Any() ? false : true;
                            bool isAllPortsChina = iDomesticCountries.Where(d => d.CountryIsGreaterChinese == false).Any() ? false : true;

                            if (!isAllPortsEC && !isAllPortsNA && !isAllPortsChina)
                            {
                                throw new ApplicationException("Both Addresses must be in the same country since the direction is Domestic");
                            }
                        }
                    }
                }

                else
                {
                    List<DomesticCountry> iDomesticCountries = new List<DomesticCountry>();
                    AddDomesticPort(iDomesticCountries, entityPM.MainCarriageFromPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.MainCarriageToPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.Transshipment1FromPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.Transshipment1ToPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.Transshipment2FromPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.Transshipment2ToPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.Transshipment3FromPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.Transshipment3ToPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.FinalDistenationPortId, entityPM.Tenant);

                    if (iDomesticCountries.GroupBy(g => g.CountryId).Count() > 1)
                    {
                        bool isAllPortsEC = iDomesticCountries.Where(d => d.CountryIsEC == false).Any() ? false : true;
                        bool isAllPortsNA = iDomesticCountries.Where(d => d.CountryIsNorthAmerica == false).Any() ? false : true;
                        bool isAllPortsChina = iDomesticCountries.Where(d => d.CountryIsGreaterChinese == false).Any() ? false : true;

                        if (!isAllPortsEC && !isAllPortsNA && !isAllPortsChina)
                        {
                            throw new ApplicationException("All Ports must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }
        private static void ValidateFromPort(ShipmentPM entityPM, Tenant loggedTenant)
        {
            bool isInlandDomestic = (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I");

            if (!isInlandDomestic)
            {
                if (string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
                {
                    throw new ApplicationException("Main carriage from port field Is required");
                }

                else
                {
                    if (!entityPM.IsHybrid && !loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                    {
                        PortPM myPort = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.MainCarriageFromPortId, true);
                        if (myPort != null)
                        {
                            switch (entityPM.TransportModeId)
                            {
                                case "A":
                                    {
                                        if (!myPort.IsAir)
                                        {
                                            throw new ApplicationException("Main carriage from port transport mode is different than shipment transport mode");
                                        }

                                        break;
                                    }

                                case "O":
                                    {
                                        if (!myPort.IsOcean)
                                        {
                                            throw new ApplicationException("Main carriage from port transport mode is different than shipment transport mode");
                                        }

                                        break;
                                    }

                                case "I":
                                    {
                                        if (!myPort.IsInland)
                                        {
                                            throw new ApplicationException("Main carriage from port transport mode is different than shipment transport mode");
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
        private static void ValidateToPort(ShipmentPM entityPM, Tenant loggedTenant)
        {
            bool isInlandDomestic = (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I");
            bool isCustomShipment = (entityPM.DirectionId == "C");

            if (!isInlandDomestic && !isCustomShipment)
            {
                if (string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
                {
                    throw new ApplicationException("Main carriage to port field Is required");
                }

                else
                {

                    if (!entityPM.IsHybrid && !loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                    {
                        PortPM myPort = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.MainCarriageToPortId, true);
                        if (myPort != null)
                        {
                            switch (entityPM.TransportModeId)
                            {
                                case "A":
                                    {
                                        if (!myPort.IsAir)
                                        {
                                            throw new ApplicationException("Main carriage to port transport mode is different than shipment transport mode");
                                        }

                                        break;
                                    }

                                case "O":
                                    {
                                        if (!myPort.IsOcean)
                                        {
                                            throw new ApplicationException("Main carriage to port transport mode is different than shipment transport mode");
                                        }

                                        break;
                                    }

                                case "I":
                                    {
                                        if (!myPort.IsInland)
                                        {
                                            throw new ApplicationException("Main carriage to port transport mode is different than shipment transport mode");
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
        private static void ValidateCarrierPrefix(ShipmentPM entityPM)
        {
            if (entityPM.TransportModeId == "A")
            {
                CardRepository cardRepository = new CardRepository(entityPM.Tenant);

                entityPM.MainCarriageCarrierPrefix = (entityPM.MainCarriageCarrierPrefix == null) ? null : entityPM.MainCarriageCarrierPrefix.Trim();
                entityPM.Transshipment1CarrierPrefix = (entityPM.Transshipment1CarrierPrefix == null) ? null : entityPM.Transshipment1CarrierPrefix.Trim();
                entityPM.Transshipment2CarrierPrefix = (entityPM.Transshipment2CarrierPrefix == null) ? null : entityPM.Transshipment2CarrierPrefix.Trim();
                entityPM.Transshipment3CarrierPrefix = (entityPM.Transshipment3CarrierPrefix == null) ? null : entityPM.Transshipment3CarrierPrefix.Trim();
                string flightCodeValidationMessage = entityPM.IsHybrid ? "airline prefix is not exist" : "flight Code is not exists";
                if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.MainCarriageCarrierPrefix, entityPM.Tenant))
                    {

                        throw new ApplicationException("Main Carriage "+ flightCodeValidationMessage);
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment1CarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.Transshipment1CarrierPrefix, entityPM.Tenant))
                    {
                        throw new ApplicationException("Transshipment1 " + flightCodeValidationMessage);
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment2CarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.Transshipment2CarrierPrefix, entityPM.Tenant))
                    {
                        throw new ApplicationException("Transshipment2 " + flightCodeValidationMessage);
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment3CarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.Transshipment3CarrierPrefix, entityPM.Tenant))
                    {
                        throw new ApplicationException("Transshipment3 " + flightCodeValidationMessage);
                    }
                }
            }
        }
        private static void ValidateAirlineRestriction(ShipmentPM entityPM)
        {
            if (entityPM.TransportModeId == "A")
            {
                int tenant = entityPM.Tenant;

                bool isRestrictedByAirline = false;

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                    if (tenantManagement != null)
                    {
                        isRestrictedByAirline = tenantManagement.IsRestrictedByAirline;
                    }
                }

                if (isRestrictedByAirline)
                {
                    AirlineRepository airlineRepository = new AirlineRepository(tenant);

                    if (MethodHelper.IsAirlineRestricted(entityPM.MainCarriageCarrierId, airlineRepository, tenant))
                    {
                        List<Airline> allowedAirlines = airlineRepository.GetAllAllowedAirlinesInRestriction(tenant);

                        string airlineCodes = "";

                        foreach (Airline airline in allowedAirlines)
                        {
                            if (string.IsNullOrEmpty(airlineCodes))
                            {
                                airlineCodes = airline.Card.Code;
                            }
                            else
                            {
                                airlineCodes = airlineCodes + " - " + airline.Card.Code;
                            }
                        }

                        throw new ApplicationException("You are restricted for " + airlineCodes + " Airlines only");
                    }
                }
            }
        }
        private static void ValidateShipmentBookingFields(ShipmentPM entityPM, bool isNewEntity)
        {
            if (!string.IsNullOrEmpty(entityPM.BookingId))
            {
                if (isNewEntity)
                {
                    BookingRepository myBookingRepository = new BookingRepository(entityPM.Tenant);
                    Booking myBooking = myBookingRepository.GetSingle(entityPM.BookingId, entityPM.Tenant);
                    if (myBooking != null)
                    {
                        if (entityPM.MainCarriageFromPortId != myBooking.MainCarriageFromPortId)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageFromPortId", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.Transshipment1FromPortId != myBooking.Transshipment1FromPortId)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.Transshipment1FromPortId", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.Transshipment2FromPortId != myBooking.Transshipment2FromPortId)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.Transshipment2FromPortId", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.MainCarriageFinalDestinationPortId != myBooking.MainCarriageFinalDestinationPortId)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageFinalDestinationPortId", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.MainCarriageToPortId != myBooking.MainCarriageToPortId)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageToPortId", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.MainCarriageCarrierId != myBooking.MainCarriageCarrierId)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageCarrierId", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.MainCarriageCarrierNumber != myBooking.MainCarriageCarrierNumber)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageCarrierNumber", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.Master != myBooking.Master)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.Master", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.InterlineId != myBooking.InterlineId)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.InterlineId", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.MainCarriageETD != myBooking.MainCarriageETD)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageETD", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }

                        if (entityPM.MainCarriageCarrierPrefix != myBooking.MainCarriageCarrierPrefix)
                        {
                            string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageCarrierPrefix", entityPM.Tenant);
                            throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                        }
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(entityPM.MasterShipmentDataId))
                    {
                        ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(entityPM.Tenant);
                        ShipmentMasterData myShipmentMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPM.MasterShipmentDataId);
                        if (myShipmentMasterData != null)
                        {
                            if (entityPM.MainCarriageFromPortId != myShipmentMasterData.MainCarriageFromPortId)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageFromPortId", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.Transshipment1FromPortId != myShipmentMasterData.Transshipment1FromPortId)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.Transshipment1FromPortId", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.Transshipment2FromPortId != myShipmentMasterData.Transshipment2FromPortId)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.Transshipment2FromPortId", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.MainCarriageFinalDestinationPortId != myShipmentMasterData.MainCarriageFinalDestinationPortId)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageFinalDestinationPortId", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.MainCarriageToPortId != myShipmentMasterData.MainCarriageToPortId)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageToPortId", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.MainCarriageCarrierId != myShipmentMasterData.MainCarriageCarrierId)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageCarrierId", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.MainCarriageCarrierNumber != myShipmentMasterData.MainCarriageCarrierNumber)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageCarrierNumber", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.Master != myShipmentMasterData.Master)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.Master", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.InterlineId != myShipmentMasterData.InterlineId)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.InterlineId", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.MainCarriageETD != myShipmentMasterData.MainCarriageETD)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageETD", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }

                            if (entityPM.MainCarriageCarrierPrefix != myShipmentMasterData.MainCarriageCarrierPrefix)
                            {
                                string field = TranslateTextsClass.Translate("Shipment.F.MainCarriageCarrierPrefix", entityPM.Tenant);
                                throw new ApplicationException("Can't edit " + field + " field in created shipment from booking");
                            }
                        }
                    }
                }
            }
        }
        private static void ValidateCreditLimitSetting(ShipmentPM entityPM, Shipment entityPoco, ICommonDataContext myCommonContext, Tenant loggedTenant, bool isNewEntity)
        {
            int tenant = entityPM.Tenant;
            string id = tenant.ToString();
            CreditLimitSetting mySettings = (from d in myCommonContext.CreditLimitSettings where d.Id == id select d).FirstOrDefault();
            if (mySettings != null)
            {
                if (mySettings.IsCreditLimitEnabled)
                {
                    ValidateCreditLimitPartnersRestrictions(entityPM, entityPoco, mySettings, isNewEntity);

                    if (mySettings.ShipmentCreationBlock)
                    {
                        AgentRepository myAgentRepository = new AgentRepository(myCommonContext);
                        CustomerRepository myCustomerRepository = new CustomerRepository(myCommonContext);

                        string localCurrencyCode = "";
                        if (loggedTenant.CurrencyId != null)
                        {
                            Currency localCurrency = (from d in myCommonContext.Currencies where d.Tenant == tenant && d.Id == loggedTenant.CurrencyId select d).FirstOrDefault();
                            if (localCurrency != null)
                            {
                                localCurrencyCode = localCurrency.Code;
                            }
                        }

                        string myPartnerId_PM = null;
                        string myPartnerId_DB = null;
                        string myPartnerText = null;

                        myPartnerId_PM = entityPM.CustomerId;
                        myPartnerId_DB = entityPoco.CustomerId;
                        myPartnerText = TranslateTextsClass.Translate("Shipment.F.CustomerId", tenant) + ": " + entityPM.CustomerName;
                        ValidateCreditLimitPartner(tenant, myAgentRepository, myCustomerRepository, myPartnerId_PM, myPartnerId_DB, myPartnerText, localCurrencyCode, isNewEntity, entityPM);

                        myPartnerId_PM = entityPM.AgentId;
                        myPartnerId_DB = entityPoco.AgentId;
                        myPartnerText = TranslateTextsClass.Translate("Shipment.F.AgentId", tenant) + ": " + entityPM.AgentName;
                        ValidateCreditLimitPartner(tenant, myAgentRepository, myCustomerRepository, myPartnerId_PM, myPartnerId_DB, myPartnerText, localCurrencyCode, isNewEntity, entityPM);
                    }
                }
            }
        }
        private static void ValidateCreditLimitPartnersRestrictions(ShipmentPM entityPM, Shipment entityPoco, CreditLimitSetting mySettings, bool isNewEntity)
        {
            if (entityPM.CustomerId != null)
            {
                bool isValidating = false;

                if (isNewEntity)
                {
                    isValidating = true;
                }

                else if (entityPM.CustomerId != entityPoco.CustomerId)
                {
                    isValidating = true;
                }

                if (isValidating)
                {
                    Card iCard = CardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant, true);

                    if (iCard != null)
                    {
                        string errorText_Blocking = "Credit limit setting is blocking shipment for ";

                        switch (iCard.PartnerTypeId)
                        {
                            case "CS":
                                {
                                    if (iCard.IsCustomer)
                                    {
                                        if (mySettings.CustomersShipmentsBlock)
                                        {
                                            throw new ApplicationException(errorText_Blocking + "Customers");
                                        }
                                    }

                                    else
                                    {
                                        if (mySettings.ShipperConsigneeShipmentBlock)
                                        {
                                            throw new ApplicationException(errorText_Blocking + "Shippers and Consignees");
                                        }
                                    }

                                    break;
                                }

                            case "AG":
                                {
                                    if (mySettings.AgentsShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Agents");
                                    }

                                    break;
                                }

                            case "CG":
                                {
                                    if (mySettings.CustomsAgentsShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Customs Agents");
                                    }

                                    break;
                                }

                            case "SG":
                                {
                                    if (mySettings.ShippingAgentsShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Shipping Agents");
                                    }

                                    break;
                                }

                            case "AL":
                                {
                                    if (mySettings.AirlinesShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Airlines");
                                    }

                                    break;
                                }

                            case "SL":
                                {
                                    if (mySettings.ShippingLinesShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Shipping Lines");
                                    }

                                    break;
                                }

                            case "TR":
                                {
                                    if (mySettings.TruckersShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Truckers");
                                    }

                                    break;
                                }

                            case "VD":
                                {
                                    if (mySettings.VendorsShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Vendors");
                                    }

                                    break;
                                }

                            case "WH":
                                {
                                    if (mySettings.WarehousesShipmentsBlock)
                                    {
                                        throw new ApplicationException(errorText_Blocking + "Warehouses");
                                    }

                                    break;
                                }
                        }
                    }
                }
            }
        }
        private static void ValidateCreditLimitPartner(int tenant, AgentRepository myAgentRepository, CustomerRepository myCustomerRepository, string myPartnerId, string mydbPartnerId, string myPartnerText, string localCurrencyCode, bool isNewEntity, ShipmentPM entityPM)
        {
            if (!string.IsNullOrEmpty(myPartnerId))
            {
                bool isValidatingPartner = false;
                if (isNewEntity)
                {
                    isValidatingPartner = true;
                }

                else if (myPartnerId != mydbPartnerId)
                {
                    isValidatingPartner = true;
                }

                if (isValidatingPartner)
                {
                    string errorText_Blocking = "Credit limit setting is blocking shipment for";

                    Customer myCustomer = myCustomerRepository.GetSingleCustomer(myPartnerId, tenant, false);
                    if (myCustomer != null)
                    {
                        if (myCustomer.IsCreditLimitEnabled)
                        {
                            if (myCustomer.BlockNewShipmentCreation)
                            {
                                throw new ApplicationException(errorText_Blocking + " " + myPartnerText);
                            }

                            else
                            {
                                if (myCustomer.CreditLimitAmount != null)
                                {
                                    IInvoiceContext myContext = InvoiceContext.GetContext(tenant);
                                    ARInvoiceRepository invoiceRepository = new ARInvoiceRepository(myContext);
                                    ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(invoiceRepository);
                                    double? myResult = invoiceQuery.GetCustomerCreditLimitActualAmount(myPartnerId, tenant);

                                    double LimitAmount = myCustomer.CreditLimitAmount == null ? 0 : myCustomer.CreditLimitAmount.Value;
                                    double ActualBalance = myResult == null ? 0 : myResult.Value;
                                    if (myCustomer.CreditLimitOpenBalance != null)
                                    {
                                        ActualBalance += myCustomer.CreditLimitOpenBalance.Value;
                                    }

                                    if (isNewEntity && entityPM.IsBuildFromQuote)
                                    {
                                        if (entityPM.QuoteId != null)
                                        {
                                            IQuotesContext quotesContext = QuotesContext.GetContext(tenant);

                                            double? quoteSaleLocalAmount = (from d in quotesContext.QuoteCharges
                                                                            where d.QuoteId == entityPM.QuoteId
                                                                            select d.SaleTotalAmountLocal).Sum();

                                            if (quoteSaleLocalAmount != null)
                                            {
                                                ActualBalance += quoteSaleLocalAmount.Value;
                                            }
                                        }
                                    }

                                    LimitAmount = MethodHelper.Roundd(LimitAmount, 2);
                                    ActualBalance = MethodHelper.Roundd(ActualBalance, 2);

                                    if (ActualBalance > LimitAmount)
                                    {
                                        string errorText_Exceeded = "";
                                        errorText_Exceeded += "exceeded its credit limit of " + String.Format("{0:N2}", LimitAmount) + " (" + localCurrencyCode + ").";
                                        errorText_Exceeded += " ";
                                        errorText_Exceeded += "The current balance stands on " + String.Format("{0:N2}", ActualBalance) + " (" + localCurrencyCode + ").";
                                        errorText_Exceeded = errorText_Exceeded.Replace(",", "(ᵜ)");

                                        throw new ApplicationException(myPartnerText + " " + errorText_Exceeded);
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        Agent myAgent = myAgentRepository.GetSingleAgent(tenant, myPartnerId);
                        if (myAgent != null)
                        {
                            if (myAgent.IsCreditLimitEnabled)
                            {
                                if (myAgent.BlockNewShipmentCreation)
                                {
                                    throw new ApplicationException(errorText_Blocking + " " + myPartnerText);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void ValidateFutureRoutingDates(ShipmentPM entityPM, List<ShipmentPickUpPM> list1, List<ShipmentDeliveryPM> list2)
        {
            var tenantQuery = new TenantQuery(entityPM.Tenant);
            var tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);

            var LBtenantsettingQuery = new LogBoxTenantSettingQuery(entityPM.Tenant);
            var tenantsettingPM = LBtenantsettingQuery.GetSinglePM(entityPM.Tenant);

            if (!entityPM.IsHybrid && !tenantsettingPM.IsDocumentsArchive)
            {
                string message = "Can't set Field to future date";
                DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                todayDateTime = todayDateTime.AddHours(24);

                // Pickups
                if (list1 != null)
                {
                    foreach (ShipmentPickUpPM item in list1.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete))
                    {
                        if (item.ATD > todayDateTime)
                        {
                            throw new ApplicationException(message.Replace("Field", "Pickup ATD"));
                        }

                        if (item.ATA > todayDateTime)
                        {
                            throw new ApplicationException(message.Replace("Field", "Pickup ATA"));
                        }
                    }
                }

                // PreCarriage
                if (entityPM.ShipmentLevelCode == "H" && entityPM.PreForwardingATD > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "Pre Forwarding ATD"));
                }
                else if (entityPM.PreCarriageATD > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "Pre Carriage ATD"));
                }

                if (entityPM.ShipmentLevelCode == "H" && entityPM.PreForwardingATA > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "Pre Forwarding ATA"));
                }
                else if (entityPM.PreCarriageATA > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "Pre Carriage ATA"));
                }

                // Main
                if (entityPM.MainCarriageATD > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.MainCarriage" : "Shipment.O.Routings.MainCarriageLeg1";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATD";
                    throw new ApplicationException(message.Replace("Field", field));
                }
                if (entityPM.MainCarriageATA > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.MainCarriage" : "Shipment.O.Routings.MainCarriageLeg1";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATA";
                    throw new ApplicationException(message.Replace("Field", field));
                }

                // TR1
                if (entityPM.Transshipment1ATD > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment1" : "Shipment.O.Routings.MainCarriageLeg2";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATD";
                    throw new ApplicationException(message.Replace("Field", field));
                }
                if (entityPM.Transshipment1ATA > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment1" : "Shipment.O.Routings.MainCarriageLeg2";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATA";
                    throw new ApplicationException(message.Replace("Field", field));
                }

                // TR2
                if (entityPM.Transshipment2ATD > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment2" : "Shipment.O.Routings.MainCarriageLeg3";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATD";
                    throw new ApplicationException(message.Replace("Field", field));
                }
                if (entityPM.Transshipment2ATA > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment2" : "Shipment.O.Routings.MainCarriageLeg3";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATA";
                    throw new ApplicationException(message.Replace("Field", field));
                }

                // TR3
                if (entityPM.Transshipment3ATD > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment3" : "Shipment.O.Routings.MainCarriageLeg4";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATD";
                    throw new ApplicationException(message.Replace("Field", field));
                }
                if (entityPM.Transshipment3ATA > todayDateTime)
                {
                    string textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment3" : "Shipment.O.Routings.MainCarriageLeg4";
                    string field = TranslateTextsClass.Translate(textCode, entityPM.Tenant) + " ATA";
                    throw new ApplicationException(message.Replace("Field", field));
                }

                // OnCarriage
                if (entityPM.ShipmentLevelCode == "H" && entityPM.OnForwardingATD > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "On Forwarding ATD"));
                }
                else if (entityPM.OnCarriageATD > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "On Carriage ATD"));
                }

                if (entityPM.ShipmentLevelCode == "H" && entityPM.OnForwardingATA > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "On Forwarding ATA"));
                }
                else if (entityPM.OnCarriageATA > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "On Carriage ATA"));
                }

                // Warehous 
                if (entityPM.WarehouseLegActualReleaseDate > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "Warehouse Release Date"));
                }
                if (entityPM.WarehouseLegActualEntryDate > todayDateTime)
                {
                    throw new ApplicationException(message.Replace("Field", "Warehouse Entry Date"));
                }

                // Deliveries
                if (list2 != null)
                {
                    foreach (ShipmentDeliveryPM item in list2.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete))
                    {
                        if (item.ATD > todayDateTime)
                        {
                            throw new ApplicationException(message.Replace("Field", "Delivery ATD"));
                        }

                        if (item.ATA > todayDateTime)
                        {
                            throw new ApplicationException(message.Replace("Field", "Delivery ATA"));
                        }
                    }
                }
            }
        }
        private static void ValidateMasterTypeDueToTransportMode(ShipmentPM entityPM)
        {
            if (entityPM.ShipmentLevelCode == "C")
            {
                switch (entityPM.TransportModeId)
                {
                    case "O":
                        {
                            if (string.IsNullOrEmpty(entityPM.ShipmentTypeId) || (entityPM.ShipmentTypeId != "FCLD" && entityPM.ShipmentTypeId != "LCLD" && entityPM.ShipmentTypeId != "MyGO"))
                            {
                                throw new ApplicationException("Shipment type is not allowed for ocean transport mode");
                            }
                            break;
                        }

                    case "I":
                        {
                            if (string.IsNullOrEmpty(entityPM.ShipmentTypeId) || (entityPM.ShipmentTypeId != "FTL" && entityPM.ShipmentTypeId != "LTL" && entityPM.ShipmentTypeId != "MyGI"))
                            {
                                throw new ApplicationException("Shipment type is not allowed for inland transport mode");
                            }
                            break;
                        }

                    case "A":
                        {
                            if (string.IsNullOrEmpty(entityPM.ShipmentTypeId) || entityPM.ShipmentTypeId != "Air")
                            {
                                throw new ApplicationException("Shipment type is not allowed for air transport mode");
                            }
                            break;
                        }
                }
            }
        }
        private static void ValidateMainCarriageCarrierDueToTransportMode(ShipmentPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
            {
                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                Card myCarrier = cardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, entityPM.Tenant);
                if (myCarrier != null)
                {
                    string myPartnerType = myCarrier.PartnerTypeId;
                    if (!string.IsNullOrEmpty(myPartnerType))
                    {
                        switch (entityPM.TransportModeId)
                        {
                            case "O":
                                {
                                    if (myPartnerType != "SL")
                                    {
                                        throw new ApplicationException("Main carriage carrier is not allowed for ocean transport mode");
                                    }
                                    break;
                                }

                            case "I":
                                {
                                    if (myPartnerType != "TR")
                                    {
                                        throw new ApplicationException("Main carriage carrier is not allowed for inland transport mode");
                                    }
                                    break;
                                }

                            case "A":
                                {
                                    if (myPartnerType != "AL")
                                    {
                                        throw new ApplicationException("Main carriage carrier is not allowed for air transport mode");
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }
        private static void ValidatePartnerTypes(ShipmentPM entityPM)
        {
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            Card myCard = null;
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);

            if (!string.IsNullOrEmpty(entityPM.AgentId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.AgentId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (tenantPM.AllowCustomersInAgentsLOV)
                    {
                        if (myCard.PartnerTypeId != "CS" && myCard.PartnerTypeId != "AG")
                        {
                            throw new ApplicationException("Agent partner type should be agent or customer");
                        }
                    }

                    else
                    {
                        if (myCard.PartnerTypeId != "AG")
                        {
                            throw new ApplicationException("Agent partner type should be agent");
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.IssuingCarrierAgentId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.IssuingCarrierAgentId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "AG")
                    {
                        throw new ApplicationException("Issuing Carrier partner type should be agent");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomAgentExportId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.CustomAgentExportId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "CG" && myCard.PartnerTypeId != "AG")
                    {
                        throw new ApplicationException("Customs agent export partner type should be customs agent");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomAgentImportId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.CustomAgentImportId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "CG" && myCard.PartnerTypeId != "AG")
                    {
                        throw new ApplicationException("Customs agent import partner type should be customs agent");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ShipperNotExporterId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.ShipperNotExporterId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "AG" && myCard.PartnerTypeId != "CS")
                    {
                        throw new ApplicationException("Shipper not exporter partner type should be agent or customer");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsigneeNotImporterId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.ConsigneeNotImporterId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "AG" && myCard.PartnerTypeId != "CS")
                    {
                        throw new ApplicationException("Consignee not importer partner type should be agent or customer");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.FreightForwarderId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.FreightForwarderId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "AG")
                    {
                        throw new ApplicationException("Freight forwarder partner type should be agent");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ColoaderId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.ColoaderId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "AG")
                    {
                        throw new ApplicationException("Coloader partner type should be agent");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomClearancePointId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.CustomClearancePointId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "WH")
                    {
                        throw new ApplicationException("Custom clearance point partner type should be warehouse");
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsolidatorId))
            {
                myCard = cardRepository.GetSingleCard(entityPM.ConsolidatorId, entityPM.Tenant);
                if (myCard != null)
                {
                    if (myCard.PartnerTypeId != "AG" && myCard.PartnerTypeId != "CS" && myCard.PartnerTypeId != "SG")
                    {
                        throw new ApplicationException("Consolidator partner type should be agent or customer or shipping agent");
                    }
                }
            }

            //if (!string.IsNullOrEmpty(entityPM.Notify1Id))
            //{

            //}

            //if (!string.IsNullOrEmpty(entityPM.Notify2Id))
            //{

            //}

            //if (!string.IsNullOrEmpty(entityPM.ReleasingAgentId))
            //{

            //}
        }
        private static void ValidateConvertShipmentType(ShipmentPM entityPM)
        {
            if (entityPM.ConvertShipmentToLCL || entityPM.ConvertShipmentToFCL || entityPM.ConvertShipmentToLTL || entityPM.ConvertShipmentToFTL)
            {
                if (!string.IsNullOrEmpty(entityPM.QuoteId))
                {
                    throw new ApplicationException("Cannot change shipment type when connected to a quote");
                }

                else if (entityPM.ShipmentPackages.Where(d => !string.IsNullOrEmpty(d.DeliveryId)).Any()
                    || entityPM.ShipmentPackages.Where(d => !string.IsNullOrEmpty(d.EmptyContainerReturnId)).Any())
                {
                    throw new ApplicationException("Cannot change shipment type when shipment packages are connected to a delivery or empty container return");
                }

                else if (entityPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(entityPM.MasterShipmentDataId))
                {
                    throw new ApplicationException("Cannot change shipment type when connected to a Master shipment");
                }

                else if (entityPM.ShipmentLevelCode == "C" && entityPM.ShipmentConsoleShipments.Count > 0)
                {
                    throw new ApplicationException("Cannot change shipment type when connected to house shipments ");
                }
                else if (HasPayablesAmounts(entityPM) && HasReceivablesAmounts(entityPM))
                {
                    throw new ApplicationException("Cannot change shipment type when shipment has Payables and Receivables amounts ");
                }
                else if (HasPayablesAmounts(entityPM))
                {
                    throw new ApplicationException("Cannot change shipment type when shipment has Payables amounts ");
                }
                else if (HasReceivablesAmounts(entityPM))
                {
                    throw new ApplicationException("Cannot change shipment type when shipment has Receivables amounts ");
                }
            }
        }
        private static void AddDomesticPort(List<DomesticCountry> iDomesticCountries, string iPortId, int iTenant)
        {
            if (!string.IsNullOrEmpty(iPortId))
            {
                if (!iDomesticCountries.Where(d => d.Id == iPortId).Any())
                {
                    PortPM iPort = PortQuery.GetSinglePort(iTenant, iPortId, true);

                    if (iPort != null)
                    {
                        iDomesticCountries.Add(new DomesticCountry()
                        {
                            Id = iPort.Id + "P",
                            CountryId = iPort.CountryId,
                            CountryIsEC = iPort.CountryEC,
                            CountryIsNorthAmerica = iPort.CountryIsNorthAmerica,
                            CountryIsGreaterChinese = iPort.CountryIsGreaterChinese,
                        });
                    }
                }
            }
        }
        private static List<DomesticCountry> GetInlandDomesticCountries(ShipmentPM entityPM)
        {
            List<DomesticCountry> domesticCountries = new List<DomesticCountry>();

            switch (entityPM.InlandDomesticFromTypeCode)
            {
                case "PART":
                    {
                        AddDomesticAddress(domesticCountries, entityPM.MainCarriageFromAddressId, entityPM.Tenant);
                        break;
                    }

                case "PORT":
                    {
                        AddDomesticPort(domesticCountries, entityPM.MainCarriageFromPortId, entityPM.Tenant);
                        break;
                    }

                case "CASL":
                    {
                        AddDomesticCountry(domesticCountries, entityPM.InlandDomesticFromCountryId, entityPM.Tenant);
                        break;
                    }
            }

            switch (entityPM.InlandDomesticToTypeCode)
            {
                case "PART":
                    {
                        AddDomesticAddress(domesticCountries, entityPM.MainCarriageToAddressId, entityPM.Tenant);
                        break;
                    }

                case "PORT":
                    {
                        AddDomesticPort(domesticCountries, entityPM.MainCarriageToPortId, entityPM.Tenant);
                        break;
                    }

                case "CASL":
                    {
                        AddDomesticCountry(domesticCountries, entityPM.InlandDomesticToCountryId, entityPM.Tenant);
                        break;
                    }
            }

            return domesticCountries;
        }
        private static void AddDomesticAddress(List<DomesticCountry> iDomesticCountries, string iAddressId, int iTenant)
        {
            if (!string.IsNullOrEmpty(iAddressId))
            {
                if (!iDomesticCountries.Where(d => d.Id == iAddressId).Any())
                {
                    AddressRepository addressRepository = new AddressRepository(iTenant);
                    Address iAddress = addressRepository.GetSingleAddress(iAddressId, iTenant);

                    if (iAddress != null)
                    {
                        iDomesticCountries.Add(new DomesticCountry()
                        {
                            Id = iAddress.Id + "A",
                            CountryId = iAddress.CountryId,
                            CountryIsEC = iAddress.Country.EC,
                            CountryIsNorthAmerica = iAddress.Country.IsNorthAmerica,
                            CountryIsGreaterChinese = iAddress.Country.IsGreaterChina,
                        });
                    }
                }
            }
        }
        private static void AddDomesticCountry(List<DomesticCountry> iDomesticCountries, string iCountryId, int iTenant)
        {
            if (!string.IsNullOrEmpty(iCountryId))
            {
                if (!iDomesticCountries.Where(d => d.Id == iCountryId).Any())
                {
                    CountryRepository countryRepository = new CountryRepository(iTenant);
                    Country iCountry = countryRepository.GetSingleCountry(iCountryId, iTenant);

                    if (iCountry != null)
                    {
                        iDomesticCountries.Add(new DomesticCountry()
                        {
                            Id = iCountry.Id + "C",
                            CountryId = iCountry.Id,
                            CountryIsEC = iCountry.EC,
                            CountryIsNorthAmerica = iCountry.IsNorthAmerica,
                            CountryIsGreaterChinese = iCountry.IsGreaterChina,
                        });
                    }
                }
            }
        }
        public static void ValidateContainerNumbers(ShipmentPM entityPM)
        {
            if (entityPM.ShipmentTypeId == "FCL" || entityPM.ShipmentTypeId == "FCLD")
            {
                if (entityPM.ShipmentPackages != null && entityPM.ShipmentPackages.Count() > 0)
                {
                    var IsDuplicate = entityPM.ShipmentPackages.Where(a => a.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete && a.ContainerNumber != null).GroupBy(g => g.ContainerNumber.Trim()).Any(g => g.Count() > 1);
                    if (IsDuplicate)
                    {
                        throw new ApplicationException("Cannot have 2 containers with the same number, you can use inside packages to add detailed packages");
                    }
                }
            }
        }
        private static void ValidateShipmentSubType(ShipmentPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.ShipmentSubTypeId))
            {
                ShipmentSubTypeRepository subTypeRepository = new ShipmentSubTypeRepository(entityPM.Tenant);
                ShipmentSubType subType = subTypeRepository.GetSingleShipmentSubType(entityPM.ShipmentSubTypeId, entityPM.Tenant);

                if (subType != null && subType.Code?.ToLower() != "horse")
                {
                    if (entityPM.ShipmentTypeId.ToLower() != subType.ShipmentTypeCode?.ToLower())
                    {
                        throw new ApplicationException("Sub Type is not allowed with this shipment type");
                    }
                }
            }
        }

        private static bool HasPayablesAmounts(ShipmentPM entityPM)
        {
            bool hasAnyPayableAmount = false;
            if (entityPM.ShipmentPayables != null)
            {
                hasAnyPayableAmount = entityPM.ShipmentPayables.Select(payable => payable.ExpectedAmount).Where(payable => payable != null && payable != 0.0).Any();
            }
            return hasAnyPayableAmount;
        }
        private static bool HasReceivablesAmounts(ShipmentPM entityPM)
        {
            bool hasAnyReceivableAmount = false;
            if (entityPM.ShipmentReceivables != null)
            {
                hasAnyReceivableAmount = entityPM.ShipmentReceivables.Select(receivable => receivable.TotalAmount).Where(receivable => receivable != null && receivable != 0.0).Any();
            }
            return hasAnyReceivableAmount;
        }
        private static void ValidateShipmentOperationalClose(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (entityPM.IsOperationalClosed && !entityPoco.IsOperationalClosed)
            {
                if (entityPM.ShipmentLevelCode == "H" && !entityPM.IsHouseUpdatedByMaster)            
                throw new ApplicationException("House shipments cannot be operational closed. They can only be closed by closing the connected Master shipment");
                
                OperationalCloseValidator operationalCloseValidator = new OperationalCloseValidator(entityPM, entityPoco);
                string errorMessage = operationalCloseValidator.StartValidating();
                if (!string.IsNullOrEmpty(errorMessage))
                    throw new ApplicationException(errorMessage.TrimStart(','));                
            }
        }
        private static void ValidateShipmentOperationalReOpen(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (!entityPM.IsOperationalClosed && entityPoco.IsOperationalClosed)
            {
                if (entityPM.ShipmentLevelCode == "H" && !entityPM.IsHouseUpdatedByMaster)
                    throw new ApplicationException("House shipments cannot be operational reopened. They can only be opened by opening the connected Master shipment");

                if (entityPM.IsAccountingClosed)
                    throw new ApplicationException("Can't operational reopen shipment, beacause it not accounting closed");
            }
        }
        private static void ValidateShipmentAccountingReOpen(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (!entityPM.IsMultiUpdate) return;

            if (!entityPM.IsAccountingClosed && entityPoco.IsAccountingClosed)
            {
                if (entityPM.ShipmentLevelCode == "H" && !entityPM.IsHouseUpdatedByMaster)
                    throw new ApplicationException("House shipments cannot be accounting reopened. They can only be opened by opening the connected Master shipment");
            }
        }
        private static void ValidateShipmentAccountingClose(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (!entityPM.IsMultiUpdate) return;

            if (entityPM.IsAccountingClosed && !entityPoco.IsAccountingClosed)
            {
                if (entityPM.ShipmentLevelCode == "H" && !entityPM.IsHouseUpdatedByMaster)
                    throw new ApplicationException("House shipments cannot be accounting closed. They can only be closed by closing the connected Master shipment");

                if (!entityPM.IsOperationalClosed)
                    throw new ApplicationException("Can't accouting close shipment, beacause it's not operationaly closed");

                AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(entityPM.Tenant);
                AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountingSetting(entityPM.Tenant);

                bool hasOpenPayables = CheckOpenPayables(entityPM, accountingSetting);
                bool hasOpenReceivables = CheckOpenReceivables(entityPM.ShipmentReceivables);

                if (hasOpenPayables || hasOpenReceivables)
                {
                    ValidateAcocuntingCloseDueToShipmentLevel(entityPM, hasOpenPayables, hasOpenReceivables, accountingSetting);
                }
            }
        }
        private static bool CheckOpenPayables(ShipmentPM entityPM, AccountingSetting accountingSetting)
        {
            bool hasOpenPayables = false;
            if (!accountingSetting.AllowClosureWithoutPayables && entityPM.ShipmentPayables.Count > 0)
            {
                foreach (ShipmentPayablePM shipmentPayable in entityPM.ShipmentPayables.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete && d.ShipmentPayableLineStatusCode != "ACCT" && d.ShipmentPayableLineStatusCode != "EMPT"))
                {
                    if (shipmentPayable.ShipmentPayableAmountTypeCode == "NEXP"
                        && (shipmentPayable.AccountedAmount != null && shipmentPayable.AccountedAmount != null
                        || shipmentPayable.ExpectedAmount != null && shipmentPayable.ExpectedAmount != 0))
                    {
                        hasOpenPayables = true;

                    }

                    else if (shipmentPayable.ExpectedAmount != null && shipmentPayable.ExpectedAmount != 0)
                    {
                        hasOpenPayables = true;
                    }
                }
            }

            return hasOpenPayables;
        }
        private static bool CheckOpenReceivables(List<ShipmentReceivablePM> shipmentReceivables)
        {
            if (shipmentReceivables.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete && d.ShipmentReceivableLineStatusCode != "ACCT" && d.ShipmentReceivableLineStatusCode != "EMPT"
             && d.TotalAmount != null && d.TotalAmount != 0).Any())
            {
                return true;
            }

            return false;
        }
        private static void ValidateAcocuntingCloseDueToShipmentLevel(ShipmentPM entityPM, bool hasOpenPayables, bool hasOpenReceivables, AccountingSetting accountingSetting)
        {
            if (entityPM.ShipmentLevelCode == "C")
            {
                ValidateMasterAccountingClose(entityPM, hasOpenPayables, hasOpenReceivables, accountingSetting);
            }

            else
            {
                ThrowAccountingCloseException(hasOpenPayables, hasOpenReceivables, entityPM.ShipmentLevelCode);
            }
        }
        private static void ValidateMasterAccountingClose(ShipmentPM entityPM, bool hasOpenPayables, bool hasOpenReceivables, AccountingSetting accountingSetting)
        {
            if (hasOpenPayables && hasOpenReceivables)
            {
                ThrowAccountingCloseException(hasOpenPayables, hasOpenReceivables, entityPM.ShipmentLevelCode);
            }
            else
            {
                string myResult = CheckHousesOpenAmounts(entityPM.Id, entityPM.Tenant);
                if (string.IsNullOrEmpty(myResult)) return;

                if (myResult.Contains('R'))
                {
                    hasOpenReceivables = true;
                }

                if (accountingSetting.AllowClosureWithoutPayables && myResult.Contains('P'))
                {
                    hasOpenPayables = true;
                }

                ThrowAccountingCloseException(hasOpenPayables, hasOpenReceivables, entityPM.ShipmentLevelCode);
            }
        }
        private static void ThrowAccountingCloseException(bool hasOpenPayables, bool hasOpenReceivables, string levelCode)
        {
            if (hasOpenPayables || hasOpenReceivables)
            {
                string error = "Can’t close for accounting if there are any open payables/receivables";
                if (levelCode == "C")
                {
                    error = "Can’t close for accounting if there are any open payables/receivables in the Master or one \nof the connected shipments. Please check and fix this issue and try again";
                }

                throw new ApplicationException(error);
            }
        }
        private static string CheckHousesOpenAmounts(string masterId, int tenant)
        {
            bool hasOpenPayables = false;
            bool hasOpenReceivables = false;

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM masterPM = shipmentQuery.GetSinglePM(masterId, tenant);
            if (masterPM == null) return null;

            foreach (ConsoleShipmentPM consoleShipmentPM in masterPM.ShipmentConsoleShipments)
            {
                ShipmentPM consoleShipment = shipmentQuery.GetSinglePM(consoleShipmentPM.Id, tenant);
                if (consoleShipment != null)
                {
                    hasOpenReceivables = CheckHouseReceivablesOpenAmounts(consoleShipment, hasOpenReceivables);
                    hasOpenPayables = CheckHousePayablesOpenAmounts(consoleShipment, hasOpenPayables);
                }
            }

            string myResult = "";
            if (hasOpenPayables)
            {
                myResult += "P";
            }

            if (hasOpenReceivables)
            {
                myResult += "R";
            }

            return myResult;
        }
        private static bool CheckHouseReceivablesOpenAmounts(ShipmentPM consoleShipment, bool hasOpenReceivables)
        {
            if (!hasOpenReceivables)
            {
                foreach (ShipmentReceivablePM item in consoleShipment.ShipmentReceivables)
                {
                    if (item.ShipmentReceivableLineStatusCode != "ACCT" && item.ShipmentReceivableLineStatusCode != "EMPT"
                        && item.TotalAmount != null && item.TotalAmount != 0)
                    {
                        hasOpenReceivables = true;
                        break;
                    }
                }
            }

            return hasOpenReceivables;
        }
        private static bool CheckHousePayablesOpenAmounts(ShipmentPM consoleShipment, bool hasOpenPayables)
        {
            if (!hasOpenPayables)
            {
                foreach (ShipmentPayablePM item in consoleShipment.ShipmentPayables)
                {
                    if (item.ShipmentPayableLineStatusCode != "ACCT" && item.ShipmentPayableLineStatusCode != "EMPT" && item.ShipmentPayableParentId == null)
                    {
                        if (item.ShipmentPayableAmountTypeCode == "NEXP" && item.AccountedAmount != null && item.AccountedAmount != 0)
                        {
                            hasOpenPayables = true;
                            break;
                        }

                        else if (item.ExpectedAmount != null && item.ExpectedAmount != 0)
                        {
                            hasOpenPayables = true;
                            break;
                        }
                    }
                }
            }

            return hasOpenPayables;
        }
        private static void ValidateInlandDomesticCasualAddressFields(ShipmentPM entityPM)
        {
            ValidateInlandDomesticFromCasualAddress(entityPM);
            ValidateInlandDomesticToCasualAddress(entityPM);
        }
        private static void ValidateInlandDomesticFromCasualAddress(ShipmentPM entityPM)
        {
            if (entityPM.InlandDomesticFromTypeCode != "CASL") return;

            Country myCountry = GetInlandDomesticCountry(entityPM.InlandDomesticFromCountryId, entityPM.Tenant);
            if (myCountry == null) return;

            ValidateInlandDomesticCountryStateRequired(myCountry.IsStateRequired, entityPM.InlandDomesticFromStateId, "From");
            ValidateInlandDomesticCountryHasCities(myCountry, entityPM.InlandDomesticFromCity, entityPM.IsHybrid);
        }
        private static void ValidateInlandDomesticToCasualAddress(ShipmentPM entityPM)
        {
            if (entityPM.InlandDomesticToTypeCode != "CASL") return;

            Country myCountry = GetInlandDomesticCountry(entityPM.InlandDomesticToCountryId, entityPM.Tenant);
            if (myCountry == null) return;

            ValidateInlandDomesticCountryStateRequired(myCountry.IsStateRequired, entityPM.InlandDomesticToStateId, "To");
            ValidateInlandDomesticCountryHasCities(myCountry, entityPM.InlandDomesticToCity, entityPM.IsHybrid);
        }
        private static Country GetInlandDomesticCountry(string countryId, int tenant)
        {
            CountryRepository countryRepository = new CountryRepository(tenant);
            return countryRepository.GetSingleCountry(countryId, tenant);
        }
        private static void ValidateInlandDomesticCountryStateRequired(bool isStateRequired, string stateId, string casualAddressCode)
        {
            if (isStateRequired && string.IsNullOrEmpty(stateId))
            {
                throw new ApplicationException(casualAddressCode + " State is Required");
            }
        }
        private static void ValidateInlandDomesticCountryHasCities(Country myCountry, string city, bool isHybrid)
        {
            if (myCountry.HasCitiesList && !isHybrid && !string.IsNullOrEmpty(city))
            {
                CountryCityRepository citiesRepository = new CountryCityRepository(myCountry.Tenant);
                IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                bool isCityExists = CheckIsCityExists(city, allCities);

                if (!isCityExists) throw new ApplicationException("This city doesn't exist in cities table");
            }
        }
        private static bool CheckIsCityExists(string myCity, IQueryable<CountryCity> allCities)
        {
            bool isCityExists =
                 (from d in allCities
                  where
                  (d.EnglishName != null && d.EnglishName.ToLower() == myCity.ToLower())
                  ||
                  (d.LocalName != null && d.LocalName.ToLower() == myCity.ToLower())
                  select d).Any();

            return isCityExists;
        }
        private static void ValidateRoutingDates(ShipmentPM entityPM, Shipment entityPoco)
        {
            RoutingDatesValidator routingDatesValidator = new RoutingDatesValidator(entityPM);
            routingDatesValidator.Validate();
        }
    }
    public class DomesticCountry
    {
        public string Id { get; set; }
        public string CountryId { get; set; }
        public bool CountryIsEC { get; set; }
        public bool CountryIsNorthAmerica { get; set; }
        public bool CountryIsGreaterChinese { get; set; }

    }
    public class OperationalCloseValidator
    {
        private ShipmentPM shipmentPM;
        private readonly Shipment entityPoco;
        private IRulesValidator ruleValidator;
        private int tenant;
        private ObjectFieldRepository objectFieldRepository;
        private ShipmentQuery shipmentQuery;
        public OperationalCloseValidator(ShipmentPM shipmentPM, Shipment entityPoco)
        {
            this.shipmentPM = shipmentPM;
            this.entityPoco = entityPoco;
            this.tenant = shipmentPM.Tenant;
            this.objectFieldRepository = new ObjectFieldRepository(tenant);
            this.shipmentQuery = new ShipmentQuery(tenant);
            this.InitializeRulesValidator();
        }

        private void InitializeRulesValidator()
        {
            ruleValidator = ContainerAccessor.Container.Resolve(typeof(IRulesValidator), "RulesValidator", new ParameterOverride("", 1)) as IRulesValidator;
            ruleValidator.Initialize(tenant);
        }

        public string StartValidating()
        {
            string errorMessage = ValidateShipment(shipmentPM);

            if (shipmentPM.ShipmentLevelCode == "C" && shipmentPM.ShipmentConsoleShipments.Count > 0)
            {
                string housesErrors = ValidateHouses();
                string housesPackagesErrors = ValidateHousePackages();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = housesErrors;
                }

                else
                {
                    errorMessage = errorMessage + ", " + housesErrors;
                }

                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = housesPackagesErrors;
                }

                else
                {
                    errorMessage = errorMessage + ", " + housesPackagesErrors;
                }
            }

            if (OperationalClosedChangedFromHouse())
            {
                var errorMsg = "House shipments cannot be operationally closed. They can only be closed by closing the connected Master shipment";
                errorMessage = string.IsNullOrEmpty(errorMessage) ? errorMsg : errorMessage + ", " + errorMsg;
            }

            return errorMessage;
        }

        private bool OperationalClosedChangedFromHouse()
        {
            return shipmentPM.ShipmentLevelCode == "H" && !shipmentPM.IsHouseUpdatedByMaster && (entityPoco.IsOperationalClosed != shipmentPM.IsOperationalClosed);
        }

        private string ValidateShipment(ShipmentPM shipment)
        {
            List<ObjectTableRuleField> requiredFields = ruleValidator.ValidateAllRequiredFieldRules(shipment, "Shipment", tenant);
            return GenerateErrorMessage(requiredFields);
        }
        private string ValidateHouses()
        {
            Dictionary<string, string> housesErrors = new Dictionary<string, string>();
            foreach (ConsoleShipmentPM consoleShipment in shipmentPM.ShipmentConsoleShipments)
            {
                string houseError = ValidateSingleHouse(consoleShipment);

                if (!string.IsNullOrEmpty(houseError))
                {
                    housesErrors.Add(consoleShipment.ShipmentNumber, houseError);
                }
            }

            return this.BuildHousesErrorMessage(housesErrors);
        }
        private string ValidateSingleHouse(ConsoleShipmentPM consoleShipment)
        {
            ShipmentPM house = shipmentQuery.GetSinglePMByShipmentNumber(consoleShipment.ShipmentNumber, tenant);
            house.IsOperationalClosed = true;
            return ValidateShipment(house);
        }
        private string ValidateHousePackages()
        {
            if (shipmentPM.ShipmentTypeId.ToUpper().Contains("MYG"))
                return null;

            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
            IQueryable<PackageType> packageTypes = packageTypeRepository.GetPackageTypes(tenant);
            List<ShipmentPackagePM> houseShipmentsPackaes = shipmentQuery.GetShipmentConsolidationPackages(shipmentPM.Id, tenant);

            List<LineData> FCL_ObsList1 = new List<LineData>();
            List<LineData> FCL_ObsList2 = new List<LineData>();

            List<ByPckageType> housesGroup = new List<ByPckageType>();
            List<ByPckageType> masterGroup = new List<ByPckageType>();

            foreach (ShipmentPackagePM item in houseShipmentsPackaes.Where(p => p.IsContainer))
            {
                ByPckageType existsedItem = housesGroup.Where(f => f.PackageTypeId == item.PackageTypeId).FirstOrDefault();
                if (existsedItem == null)
                {
                    existsedItem = new ByPckageType();
                    existsedItem.PackageTypeId = item.PackageTypeId;
                    existsedItem.Quantity = item.Quantity;
                    existsedItem.MeasurementId = (packageTypes.Where(f => f.Id == item.PackageTypeId).FirstOrDefault()) != null ? (packageTypes.Where(f => f.Id == item.PackageTypeId).FirstOrDefault()).MeasurementId : null;
                    housesGroup.Add(existsedItem);
                }
                else
                {
                    existsedItem.Quantity += item.Quantity;
                }
            }

            foreach (ShipmentPackagePM item in shipmentPM.ShipmentPackages.Where(d => d.IsContainer))
            {
                ByPckageType existsedItem = masterGroup.Where(f => f.PackageTypeId == item.PackageTypeId).FirstOrDefault();
                if (existsedItem == null)
                {
                    existsedItem = new ByPckageType();
                    existsedItem.PackageTypeId = item.PackageTypeId;
                    existsedItem.Quantity = item.Quantity;
                    existsedItem.MeasurementId = (packageTypes.Where(f => f.Id == item.PackageTypeId).FirstOrDefault()) != null ? (packageTypes.Where(f => f.Id == item.PackageTypeId).FirstOrDefault()).MeasurementId : null;
                    masterGroup.Add(existsedItem);
                }
                else
                {
                    existsedItem.Quantity += item.Quantity;
                }
            }

            foreach (ByPckageType houseItem in housesGroup)
            {
                PackageType packageType = packageTypes.Where(f => f.Id == houseItem.PackageTypeId).FirstOrDefault();
                if (packageType != null)
                {
                    LineData line = new LineData();
                    line.LineLabel = packageType.EnglishName;
                    line.HouseValue = houseItem.Quantity;

                    ByPckageType masterItem = masterGroup.Where(f => f.PackageTypeId == houseItem.PackageTypeId && f.MeasurementId == houseItem.MeasurementId).FirstOrDefault();
                    if (masterItem != null)
                    {
                        line.MasterValue = masterItem.Quantity;
                        line.IsEquals = (houseItem.Quantity == masterItem.Quantity);
                        List<ByPckageType> temp = new List<ByPckageType>();
                        foreach (ByPckageType p in masterGroup)
                        {
                            if (p != masterItem)
                                temp.Add(p);
                        }
                        masterGroup = temp;
                    }

                    else
                    {
                        line.MasterValue = 0;
                        line.IsEquals = false;
                    }
                    FCL_ObsList1.Add(line);
                }
            }

            foreach (ByPckageType masterItem in masterGroup)
            {
                PackageType packageType = packageTypes.Where(f => f.Id == masterItem.PackageTypeId).FirstOrDefault();
                if (packageType != null)
                {
                    LineData line = new LineData();
                    line.LineLabel = packageType.EnglishName;
                    line.HouseValue = 0;
                    line.MasterValue = masterItem.Quantity;
                    line.IsEquals = false;
                    FCL_ObsList1.Add(line);
                }
            }

            List<ShipmentPackagePM> masterPackages = shipmentPM.ShipmentPackages;
            foreach (ShipmentPackagePM houseItem in houseShipmentsPackaes.OrderBy(d => d.ShipmentId))
            {
                PackageType packageType  = packageTypes.Where(p => p.Id == houseItem.PackageTypeId).FirstOrDefault();
                if (packageType != null)
                {
                    LineData line = new LineData();
                    line.ShipmentNumber = houseItem.ShipmentNumber;
                    line.LineLabel = packageType.EnglishName;
                    line.HouseStringValue = string.IsNullOrEmpty(houseItem.ContainerNumber) ? "- - -" : houseItem.ContainerNumber;

                    if (string.IsNullOrEmpty(line.ShipmentNumber))
                    {
                        ConsoleShipmentPM dd = shipmentPM.ShipmentConsoleShipments.Where(d => d.Id == houseItem.ShipmentId).FirstOrDefault();
                        if (dd != null)
                        {
                            line.ShipmentNumber = dd.ShipmentNumber;
                        }
                    }

                    ShipmentPackagePM masterItem = masterPackages.Where(d => d.OriginalShipmentPackageId == houseItem.Id).FirstOrDefault();
                    if (masterItem != null)
                    {
                        List<ShipmentPackagePM> temp = new List<ShipmentPackagePM>();                        
                        foreach(ShipmentPackagePM p in masterPackages)
                        {
                            if (p != masterItem)
                                temp.Add(p);
                        }
                        masterPackages = temp;
                        line.MasterStringValue = string.IsNullOrEmpty(masterItem.ContainerNumber) ? "- - -" : masterItem.ContainerNumber;
                        line.IsEquals = (line.HouseStringValue == line.MasterStringValue);                        
                    }
                    else
                    {
                        line.MasterStringValue = "Not exists";
                        line.IsEquals = false;                        
                    }

                    if (!line.IsEquals)
                    {
                        FCL_ObsList2.Add(line);
                    }
                }
            }

            foreach (ShipmentPackagePM item in masterPackages)
            {
                PackageType packageType = packageTypes.Where(d => d.Id == item.PackageTypeId).FirstOrDefault();
                if (packageType != null)
                {
                    LineData line = new LineData();
                    line.ShipmentNumber = shipmentPM.ShipmentNumber;
                    line.LineLabel = packageType.EnglishName;
                    line.HouseStringValue = "Not exists";
                    line.MasterStringValue = string.IsNullOrEmpty(item.ContainerNumber) ? "- - -" : item.ContainerNumber;
                    line.IsEquals = false;
                    FCL_ObsList2.Add(line);
                }
            }

            if (FCL_ObsList1.Where(d => d.IsEquals == false).FirstOrDefault() != null || FCL_ObsList2.Where(d => d.IsEquals == false).FirstOrDefault() != null)
            {
                return "Mismatch Quantities or Container numbers";
            }

            return null;
        }
        private string GenerateErrorMessage(List<ObjectTableRuleField> requiredFields)
        {
            string errorMessage = "";
            foreach (ObjectTableRuleField field in requiredFields)
            {
                ObjectField f = objectFieldRepository.GetSingleObjectFieldByFieldCode(field.ObjectFieldCode, tenant);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", f.FullNameTextCode.Code, null, null, field.Tenant);
                }

                else
                {
                    errorMessage += ", " + TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", f.FullNameTextCode.Code, null, null, field.Tenant);
                }
            }

            return errorMessage;
        }
        private string BuildHousesErrorMessage(Dictionary<string, string> housesErrors)
        {
            string errorMessage = "";

            foreach (KeyValuePair<string, string> item in housesErrors)
            {
                string houseError = "House " + item.Key + ": " + item.Value;

                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = houseError;
                }

                else
                {
                    errorMessage = errorMessage + ", " + houseError;
                }
            }

            return errorMessage;
        }
    }
    public class ByPckageType
    {
        public int? Quantity { get; set; }
        public string PackageTypeId { get; set; }
        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
        public string MeasurementShortName { get; set; }
    }
    public class LineData
    {
        public string LineLabel { get; set; }
        public int? HouseValue { get; set; }
        public int? MasterValue { get; set; }
        public bool IsEquals { get; set; }
        public string ShipmentNumber { get; set; }
        public string MasterStringValue { get; set; }
        public string HouseStringValue { get; set; }
    }
}