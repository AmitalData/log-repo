using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System.Data;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Data.Entity.Core;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.QuoteModel.Tools.Validating
{
    public class QuotetValidating
    {
        public static void Validate(QuotePM entityPM, Quote entityPoco, bool isNewEntity, ICommonDataContext myCommonContext)
        {
            bool isInlandDomestic = (entityPM.DirectionId == "D" || entityPM.TransportModeId == "I");

            if (isInlandDomestic)
            {

            }

            else
            {
                AddressValidating.ValidateQuotePickupDelivery(entityPM);
                ValidateFromPort(entityPM);
                ValidateToPort(entityPM);
            }

            if (isNewEntity)
            {
                ValidateProductTypePermission(entityPM);
            }

            else
            {
                ValidateConcurrencyGUID(entityPM, entityPoco);
            }

            if (entityPM.Ratio > 10 || entityPM.Ratio < 1)
            {
                throw new ApplicationException("Ratio must be between 1-10");
            }

            ValidateAirlineRestriction(entityPM);
            ValidateMultiVatPercentages(entityPM, myCommonContext);
            ValidateConvertQuote(entityPM);
            ValidateFCLDuplicatedPackages(entityPM);
            ValidateDomesticQuote(entityPM);
            ValidateQuoteCharges(entityPM);
            ValidateShipmentSubType(entityPM);
            ValidateRegionalTax(entityPM);
        }

        private static void ValidateConvertQuote(QuotePM entityPM)
        {
            if (entityPM.ConvertToLCL || entityPM.ConvertToFCL)
            {
                IShipmentsContext MyContext = ShipmentsContext.GetContext(entityPM.Tenant);
                bool ExistConnectedShipments = MyContext.Shipments.Where(p => p.Tenant == entityPM.Tenant && p.QuoteId == entityPM.Id).FirstOrDefault() != null;
                if (ExistConnectedShipments)
                {
                    throw new ApplicationException("Cannot change quote type when connected to shipments");
                }

            }
        }
        private static void ValidateAirlineRestriction(QuotePM entityPM)
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
        private static void ValidateConcurrencyGUID(QuotePM entityPM, Quote entityPoco)
        {
            if (!entityPM.ConcurrencyGUID.Equals(entityPoco.ConcurrencyGUID))
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                throw new OptimisticConcurrencyException(msg);
            }
        }
        private static void ValidateProductTypePermission(QuotePM entityPM)
        {
            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            UserRepository userRepository = new UserRepository(entityPM.Tenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, entityPM.Tenant, false);

            if (loggedUser != null)
            {
                if (loggedUser.IsProductRestricted)
                {
                    UserPermittedProductRepository productRep = new UserPermittedProductRepository(entityPM.Tenant);
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
        private static void ValidateFromPort(QuotePM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.FromPortId))
            {
                throw new ApplicationException("From port field Is required");
            }

            else
            {
                if (!entityPM.IsHybrid)
                {
                    PortPM myPort = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.FromPortId, true);
                    if (myPort != null)
                    {
                        switch (entityPM.TransportModeId)
                        {
                            case "A":
                                {
                                    if (!myPort.IsAir)
                                    {
                                        throw new ApplicationException("From port transport mode is different than quote transport mode");
                                    }

                                    break;
                                }

                            case "O":
                                {
                                    if (!myPort.IsOcean)
                                    {
                                        throw new ApplicationException("From port transport mode is different than quote transport mode");
                                    }

                                    break;
                                }

                            case "I":
                                {
                                    if (!myPort.IsInland)
                                    {
                                        throw new ApplicationException("From port transport mode is different than quote transport mode");
                                    }

                                    break;
                                }
                        }
                    }
                }
            }
        }
        private static void ValidateToPort(QuotePM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.ToPortId))
            {
                throw new ApplicationException("To port field Is required");
            }

            else
            {
                if (!entityPM.IsHybrid)
                {
                    PortPM myPort = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.ToPortId, true);
                    if (myPort != null)
                    {
                        switch (entityPM.TransportModeId)
                        {
                            case "A":
                                {
                                    if (!myPort.IsAir)
                                    {
                                        throw new ApplicationException("To port transport mode is different than quote transport mode");
                                    }

                                    break;
                                }

                            case "O":
                                {
                                    if (!myPort.IsOcean)
                                    {
                                        throw new ApplicationException("To port transport mode is different than quote transport mode");
                                    }

                                    break;
                                }

                            case "I":
                                {
                                    if (!myPort.IsInland)
                                    {
                                        throw new ApplicationException("To port transport mode is different than quote transport mode");
                                    }

                                    break;
                                }
                        }
                    }
                }
            }
        }
        private static void ValidateMultiVatPercentages(QuotePM entityPM, ICommonDataContext myCommonContext)
        {
            List<string> allVatsIds = (from d in entityPM.QuoteCharges
                                       where d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete
                                       && d.VatTypeId != null
                                       group d by d.VatTypeId into g
                                       select g.Key).ToList();

            if (allVatsIds.Count > 0)
            {
                AccountingSetting accountingSetting = (from d in myCommonContext.AccountingSettings
                                                       where d.Id == entityPM.Tenant
                                                       select d).FirstOrDefault();
                if (accountingSetting != null)
                {
                    if (!accountingSetting.EnableMultiPercentageVATTypes)
                    {
                        List<VatType> allVats = (from f in myCommonContext.VatTypes
                                                 where allVatsIds.Contains(f.Id)
                                                 && f.Tenant == entityPM.Tenant
                                                 select f).ToList();

                        if (allVats.Where(d => d.IsMultiPercentage).Any())
                        {
                            throw new ApplicationException("Your accounting settings doesn't enable Multi-percentage VATs");
                        }
                    }
                }
            }
        }
        private static void ValidateFCLDuplicatedPackages(QuotePM entityPM)
        {
            bool isFCLQuote = false;

            if (entityPM.TransportModeId != null)
            {
                entityPM.TransportModeId = entityPM.TransportModeId.ToUpper();
            }

            //if (entityPM.ShipmentTypeId != null)
            //{
            //    entityPM.ShipmentTypeId = entityPM.ShipmentTypeId.ToUpper();
            //}

            if (entityPM.TransportModeId == "O" && (entityPM.ShipmentTypeId == "FCLD" || entityPM.ShipmentTypeId == "MYGO"))
            {
                isFCLQuote = true;
            }

            else if (entityPM.TransportModeId == "I" && (entityPM.ShipmentTypeId == "FTL" || entityPM.ShipmentTypeId == "MYGI"))
            {
                isFCLQuote = true;
            }

            if (isFCLQuote)
            {
                List<string> list = new List<string>();

                if (!string.IsNullOrEmpty(entityPM.PackageType1Id))
                {
                    list.Add(entityPM.PackageType1Id);
                }

                if (!string.IsNullOrEmpty(entityPM.PackageType2Id))
                {
                    list.Add(entityPM.PackageType2Id);
                }

                if (!string.IsNullOrEmpty(entityPM.PackageType3Id))
                {
                    list.Add(entityPM.PackageType3Id);
                }

                if (!string.IsNullOrEmpty(entityPM.PackageType4Id))
                {
                    list.Add(entityPM.PackageType4Id);
                }

                if (!string.IsNullOrEmpty(entityPM.PackageType5Id))
                {
                    list.Add(entityPM.PackageType5Id);
                }

                var listGroup = (from d in list
                                 group d by d into g
                                 select new
                                 {
                                     PackageTypeId = g,
                                     Count = g.Count(),
                                 }).ToList();

                if (listGroup.Where(d => d.Count > 1).Any())
                {
                    throw new ApplicationException("Cannot add the same container type twice. You can adjust the QTY for one of them");
                }
            }
        }
        private static void ValidateDomesticQuote(QuotePM entityPM)
        {
            if (entityPM.DirectionId.ToUpper() == "D")
            {
                bool isInlandDomestic = entityPM.TransportModeId.ToUpper() == "I" && entityPM.DirectionId == "D" ? true : false;

                if (isInlandDomestic)
                {
                    List<DomesticCountry> iDomesticCountries = new List<DomesticCountry>();
                    AddDomesticAddress(iDomesticCountries, entityPM.FromPartnerAddressId, entityPM.Tenant);
                    AddDomesticAddress(iDomesticCountries, entityPM.ToPartnerAddressId, entityPM.Tenant);

                    if (iDomesticCountries.GroupBy(g => g.CountryId).Count() > 1)
                    {
                        bool isAllPortsEC = iDomesticCountries.Where(d => d.CountryIsEC == false).Any() ? false : true;
                        bool isAllPortsNA = iDomesticCountries.Where(d => d.CountryIsNorthAmerica == false).Any() ? false : true;

                        if (!isAllPortsEC && !isAllPortsNA)
                        {
                            throw new ApplicationException("Both Addresses must be in the same country since the direction is Domestic");
                        }
                    }
                }
                else
                {
                    List<DomesticCountry> iDomesticCountries = new List<DomesticCountry>();
                    AddDomesticPort(iDomesticCountries, entityPM.FromPortId, entityPM.Tenant);
                    AddDomesticPort(iDomesticCountries, entityPM.ToPortId, entityPM.Tenant);

                    if (iDomesticCountries.GroupBy(g => g.CountryId).Count() > 1)
                    {
                        bool isAllPortsEC = iDomesticCountries.Where(d => d.CountryIsEC == false).Any() ? false : true;
                        bool isAllPortsNA = iDomesticCountries.Where(d => d.CountryIsNorthAmerica == false).Any() ? false : true;

                        if (!isAllPortsEC && !isAllPortsNA)
                        {
                            throw new ApplicationException("All Ports must be in the same country since the direction is Domestic");
                        }
                    }
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
                            Id = iPort.Id,
                            CountryId = iPort.CountryId,
                            CountryIsEC = iPort.CountryEC,
                            CountryIsNorthAmerica = iPort.CountryIsNorthAmerica,
                        });
                    }
                }
            }
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
                            Id = iAddress.Id,
                            CountryId = iAddress.CountryId,
                            CountryIsEC = iAddress.Country.EC,
                            CountryIsNorthAmerica = iAddress.Country.IsNorthAmerica,
                        });
                    }
                }
            }
        }
        private static void ValidateQuoteCharges(QuotePM entityPM)
        {
            ValidateQuoteCharges_SaleCurrencyMode(entityPM);

            string freightLineCostCurrencyId = null;
            string freightLineSaleCurrencyId = null;
            QuoteChargePM freightCharge = entityPM.QuoteCharges.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete && d.ChargesGroupCode == "FRT").FirstOrDefault();
            if (freightCharge != null)
            {
                freightLineCostCurrencyId = freightCharge.CostCurrencyId;
                freightLineSaleCurrencyId = freightCharge.SaleCurrencyId;
            }

            foreach (QuoteChargePM item in entityPM.QuoteCharges)
            {
                if (!string.IsNullOrEmpty(item.CostMeasurementCode))
                {
                    if (item.CostMeasurementCode == "PRFR" && !string.IsNullOrEmpty(item.CostCurrencyId) && !string.IsNullOrEmpty(freightLineCostCurrencyId))
                    {
                        if (item.CostTotalAmount != null && item.CostTotalAmount != 0)
                        {
                            if (item.CostCurrencyId != freightLineCostCurrencyId)
                            {
                                throw new ApplicationException("Charges Type " + item.ChargesTypeCode + " cost currency must be the same as the freight currency in the case of Percent of Freight");
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(item.SaleMeasurementCode))
                {
                    if (item.SaleMeasurementCode == "PRFR" && !string.IsNullOrEmpty(item.SaleCurrencyId) && !string.IsNullOrEmpty(freightLineSaleCurrencyId))
                    {
                        if (item.SaleTotalAmount != null && item.SaleTotalAmount != 0)
                        {
                            if (item.SaleCurrencyId != freightLineSaleCurrencyId)
                            {
                                throw new ApplicationException("Charges Type " + item.ChargesTypeCode + " sale currency must be the same as the freight currency in the case of Percent of Freight");
                            }
                        }
                    }
                }

                switch (item.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        {
                            if (item.CostCurrencyId == null)
                            {
                                if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH")
                                {
                                    throw new ApplicationException("Tenant freight currency is required");
                                }

                                else
                                {
                                    throw new ApplicationException("Tenant other Charges currency is required");
                                }
                            }

                            break;
                        }
                }
            }
        }

        private static void ValidateQuoteCharges_SaleCurrencyMode(QuotePM entityPM)
        {
            List<QuoteChargePM> lines = entityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

            QuoteChargePM freightCharge = lines.Where(d => d.ChargesGroupCode == "FRT").FirstOrDefault();

            if (lines.Count > 0)
            {
                foreach (QuoteChargePM item in lines)
                {
                    if (item.CostCurrencyId == item.SaleCurrencyId)
                    {
                        if (item.CostExchangeRate != item.SaleExchangeRate)
                        {
                            throw new ApplicationException("Charges of same cost and sale currency should have same exchange rate");
                        }
                    }

                    if (item.IsAllIN)
                    {
                        if (freightCharge != null)
                        {
                            if (item.SaleCurrencyId != freightCharge.SaleCurrencyId)
                            {
                                throw new ApplicationException("All in charges must be same as freight Charge sale currency");
                            }
                        }
                    }

                    if (entityPM.IsSaleCurrencySameAsCost)
                    {
                        if (item.CostCurrencyId != item.SaleCurrencyId)
                        {
                            throw new ApplicationException("All charges sale currency must be same as cost currency");
                        }
                    }

                    else if (entityPM.IsMultiCurrency)
                    {

                    }

                    else
                    {
                        if (item.SaleCurrencyId != entityPM.SaleCurrencyId)
                        {
                            throw new ApplicationException("All charges sale currency must be fixed to quote sale currency");
                        }
                    }
                }
            }
        }

        private static void ValidateShipmentSubType(QuotePM entityPM)
        {
            if (!entityPM.IsHybrid)
            {
                if (!string.IsNullOrEmpty(entityPM.ShipmentSubTypeId))
                {
                    ShipmentSubTypeRepository subTypeRepository = new ShipmentSubTypeRepository(entityPM.Tenant);
                    ShipmentSubType subType = subTypeRepository.GetSingleShipmentSubType(entityPM.ShipmentSubTypeId, entityPM.Tenant);

                    if (subType != null)
                    {
                        if (entityPM.ShipmentTypeId.ToLower() != subType.ShipmentTypeCode.ToLower())
                        {
                            throw new ApplicationException("Sub Type is not allowed with this shipment type");
                        }
                    }
                }
            }
        }
        private static void ValidateRegionalTax(QuotePM entityPM)
        {
            List<QuoteChargePM> allRegionalTaxLines = entityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.IsRegionalTax).ToList();

            if (allRegionalTaxLines.Count > 0)
            {
                if (allRegionalTaxLines.Where(d => d.VatIsMultiPercentage).Any())
                {
                    throw new ApplicationException("Can't set regional Tax for multi VAT");
                }

                else if (entityPM.RegionalTaxId == null)
                {
                    throw new ApplicationException("Quote regional tax field is required");
                }

                else
                {
                    var groupedIds = (from d in allRegionalTaxLines
                                      where d.VatTypeId != null
                                      group d by d.VatTypeId into g
                                      select g.Key).ToList();

                    if (groupedIds.Count > 1)
                    {
                        throw new ApplicationException("Can't set regional tax for different VATs");
                    }

                    else
                    {
                        var groupedPercentages = (from d in allRegionalTaxLines
                                                  where d.VatTypeId != null
                                                  group d by new { d.VatTypeId, d.VatPercentage } into g
                                                  select g.Key).ToList();

                        if (groupedPercentages.Count > 1)
                        {
                            throw new ApplicationException("Can't set different regional Tax percentages");
                        }
                    }
                }
            }
        }

    }

    public class DomesticCountry
    {
        public string Id { get; set; }
        public string CountryId { get; set; }
        public bool CountryIsEC { get; set; }
        public bool CountryIsNorthAmerica { get; set; }
    }
}