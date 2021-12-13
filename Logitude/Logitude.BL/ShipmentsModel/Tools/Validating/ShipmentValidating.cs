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

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentValidating
    {
        public static void Validate(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity, ICommonDataContext myCommonContext, Tenant loggedTenant)
        {
            if (isNewEntity)
            {
                ValidateProductTypePermission(entityPM, myCommonContext);
            }

            else
            {
                ValidateConcurrencyGUID(entityPM, entityPoco);
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
                //ValidateMasterNumber(entityPM);
                ValidateShipmentBookingFields(entityPM, isNewEntity);
                ValidateCreditLimitSetting(entityPM, entityPoco, myCommonContext, loggedTenant, isNewEntity);
                ValidateConvertShipmentType(entityPM);
            }
            //ValidateMultiVatPercentages(entityPM, myCommonContext);

            if (!entityPM.IsHybrid)
            {
                ValidateContainerNumbers(entityPM);
                ValidateMasterTypeDueToTransportMode(entityPM);
                ValidateMainCarriageCarrierDueToTransportMode(entityPM);
                ValidatePartnerTypes(entityPM);
                ValidateShipmentSubType(entityPM);
            }

            //List<IEntityValidator> validators = new List<IEntityValidator>();
            //validators.Add(new ShipmentReceivableValidator(entityPM));

            //foreach(IEntityValidator validator in validators)
            //{
            //    validator.Validate();
            //}

            //ShipmentReceivableValidator.Validate(entityPM.ShipmentReceivables);
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

                    if (WarningPercentage != null && (ActualBalance > (WarningPercentage * LimitAmount / 100))&& ActualBalance <= LimitAmount)
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
            if (!entityPM.IsUpdatedByChampAnalyzer)
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
                        List<DomesticCountry> iDomesticCountries = GetInlandDomesticCountries(entityPM);

                        if (iDomesticCountries.GroupBy(g => g.CountryId).Count() > 1)
                        {
                            bool isAllPortsEC = iDomesticCountries.Where(d => d.CountryIsEC == false).Any() ? false : true;
                            bool isAllPortsNA = iDomesticCountries.Where(d => d.CountryIsNorthAmerica == false).Any() ? false : true;
                            bool isAllPortsChina = iDomesticCountries.Where(d => d.CountryIsGreaterChinese == false).Any() ? false : true;

                            if (!isAllPortsEC && !isAllPortsNA &&!isAllPortsChina)
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

            if (!isInlandDomestic)
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

                if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.MainCarriageCarrierPrefix, entityPM.Tenant))
                    {
                        throw new ApplicationException("Main Carriage flight Code is not exists");
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment1CarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.Transshipment1CarrierPrefix, entityPM.Tenant))
                    {
                        throw new ApplicationException("Transshipment1 flight Code is not exists");
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment2CarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.Transshipment2CarrierPrefix, entityPM.Tenant))
                    {
                        throw new ApplicationException("Transshipment2 flight Code is not exists");
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment3CarrierPrefix))
                {
                    if (!cardRepository.IsAirlineExistsInTenant(entityPM.Transshipment3CarrierPrefix, entityPM.Tenant))
                    {
                        throw new ApplicationException("Transshipment3 flight Code is not exists");
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

        //private static void ValidateMasterNumber(ShipmentPM entityPM)
        //{
        //    if (entityPM.Tenant != 343 && entityPM.Tenant != 528 && !entityPM.IsHybrid)
        //    {
        //        if (!string.IsNullOrEmpty(entityPM.Master) && !string.IsNullOrEmpty(entityPM.AirlinePrefix) && !entityPM.IsCancelled)
        //        {
        //            if (entityPM.DirectionId == "E" && entityPM.TransportModeId == "A")
        //            {
        //                if (entityPM.ShipmentLevelCode == "C" || entityPM.ShipmentLevelCode == "D")
        //                {
        //                    int myTenant = entityPM.Tenant;
        //                    bool isMasterFieldUsed = false;

        //                    IShipmentsContext iContext = ShipmentsContext.GetContext(myTenant);

        //                    var iQueryable = (from myShipment in iContext.Shipments
        //                                      join db_Masters in iContext.ShipmentMasterDatas
        //                                      on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
        //                                      from myMasterData in ShipmentsMasters.DefaultIfEmpty()

        //                                      where myShipment.Tenant == myTenant
        //                                      && (myShipment.ShipmentLevelCode == "C" || myShipment.ShipmentLevelCode == "D")
        //                                      && myShipment.IsCancelled == false
        //                                      && myShipment.DirectionId == entityPM.DirectionId
        //                                      && myShipment.TransportModeId == entityPM.TransportModeId
        //                                      && myMasterData.Master == entityPM.Master
        //                                      && myMasterData.AirlinePrefix == entityPM.AirlinePrefix
        //                                      select myShipment);

        //                    if (!string.IsNullOrEmpty(entityPM.Id))
        //                    {
        //                        iQueryable = iQueryable.Where(d => d.Id != entityPM.Id);
        //                    }

        //                    if (iQueryable.Count() > 0)
        //                    {
        //                        isMasterFieldUsed = true;
        //                        throw new ApplicationException("Master field already used in another Shipment");
        //                    }

        //                    else
        //                    {
        //                        BookingRepository myBookingRepository = new BookingRepository(myTenant);
        //                        isMasterFieldUsed = myBookingRepository.IsMasterFieldUsed(entityPM.Master, entityPM.AirlinePrefix, entityPM.BookingId, myTenant, entityPM.DirectionId, entityPM.TransportModeId);
        //                        if (isMasterFieldUsed)
        //                        {
        //                            throw new ApplicationException("Master field already used in another Booking");
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
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

        public static void ValidateRoutingDates(ShipmentPM entityPM, List<ShipmentPickUpPM> list1, List<ShipmentDeliveryPM> list2)
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
        //public static bool IsMasterFieldUsedByAnotherShipment(string entityId, string myMasterField, string myAirlinePrefixField, string myDirectionId, string myTransportModeId, string myShipmentLevelCode, bool isCancelled, int myTenant)
        //{
        //    bool isMasterFieldUsed = false;

        //    if (myTenant != 343 && myTenant != 528)
        //    {
        //        if (!string.IsNullOrEmpty(myMasterField) && !string.IsNullOrEmpty(myAirlinePrefixField) && !isCancelled)
        //        {
        //            if (myDirectionId == "E" && myTransportModeId == "A")
        //            {
        //                if (myShipmentLevelCode == "C" || myShipmentLevelCode == "D")
        //                {
        //                    IShipmentsContext iContext = ShipmentsContext.GetContext(myTenant);

        //                    var iQueryable = (from myShipment in iContext.Shipments
        //                                      join db_Masters in iContext.ShipmentMasterDatas
        //                                      on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
        //                                      from myMasterData in ShipmentsMasters.DefaultIfEmpty()

        //                                      where myShipment.Tenant == myTenant
        //                                      && (myShipment.ShipmentLevelCode == "C" || myShipment.ShipmentLevelCode == "D")
        //                                      && myShipment.IsCancelled == false
        //                                      && myShipment.DirectionId == myDirectionId
        //                                      && myShipment.TransportModeId == myTransportModeId
        //                                      && myMasterData.Master == myMasterField
        //                                      && myMasterData.AirlinePrefix == myAirlinePrefixField
        //                                      select myShipment);

        //                    if (!string.IsNullOrEmpty(entityId))
        //                    {
        //                        iQueryable = iQueryable.Where(d => d.Id != entityId);
        //                    }

        //                    if (iQueryable.Count() > 0)
        //                    {
        //                        isMasterFieldUsed = true;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return isMasterFieldUsed;
        //}
        //public static bool IsMasterFieldUsedByAnotherBooking(string myBookingId, string myMasterField, string myAirlinePrefixField, string myDirectionId, string myTransportModeId, string myShipmentLevelCode, bool isCancelled, int myTenant)
        //{
        //    bool isMasterFieldUsed = false;

        //    if (myTenant != 343 && myTenant != 528)
        //    {
        //        if (!string.IsNullOrEmpty(myMasterField) && !string.IsNullOrEmpty(myAirlinePrefixField) && !isCancelled)
        //        {
        //            if (myDirectionId == "E" && myTransportModeId == "A")
        //            {
        //                if (myShipmentLevelCode == "C" || myShipmentLevelCode == "D")
        //                {
        //                    BookingRepository myBookingRepository = new BookingRepository(myTenant);
        //                    isMasterFieldUsed = myBookingRepository.IsMasterFieldUsed(myMasterField, myAirlinePrefixField, myBookingId, myTenant, myDirectionId, myTransportModeId);
        //                }
        //            }
        //        }
        //    }

        //    return isMasterFieldUsed;
        //}

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
            if(!string.IsNullOrEmpty(entityPM.ShipmentSubTypeId))
            {
                ShipmentSubTypeRepository subTypeRepository = new ShipmentSubTypeRepository(entityPM.Tenant);
                ShipmentSubType subType = subTypeRepository.GetSingleShipmentSubType(entityPM.ShipmentSubTypeId, entityPM.Tenant);

                if (subType != null && subType.Code?.ToLower() != "horse")
                {
                    if(entityPM.ShipmentTypeId.ToLower() != subType.ShipmentTypeCode?.ToLower())
                    {
                        throw new ApplicationException("Sub Type is not allowed with this shipment type");
                    }
                }
            }
        }
        private static bool HasPayablesAmounts(ShipmentPM entityPM)
        {
            bool hasAnyPayableAmount = false;
            if(entityPM.ShipmentPayables != null)
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
    }
    public class DomesticCountry
    {
        public string Id { get; set; }
        public string CountryId { get; set; }
        public bool CountryIsEC { get; set; }
        public bool CountryIsNorthAmerica { get; set; }
        public bool CountryIsGreaterChinese { get; set; }

    }
}