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
}