using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public DailySpotlightClass GetCRMDailySpotlightCounts(string ownerId, string businessUnitId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            DailySpotlightClass myResult = new DailySpotlightClass()
            {
                Id = tenant
            };

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime yesterdayDate = todayDate.AddDays(-1);
            DateTime lastWeekDate = todayDate.AddDays(-7);

            if (SecurityUtility.CheckTableContactFeature("Quote", "READ", tenant))
            {
                QuoteRepository myRepository = new QuoteRepository(tenant);                
                IQueryable<Quote> iQueryable = myRepository.GetQuotes(tenant);
                iQueryable = iQueryable.Where(d => d.IsCancelled == false);

                iQueryable = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Quote>(new QueryOperations(), iQueryable, tenant);
                iQueryable = ProductPermitionsFilter.AddUserProductRestrictionFilters<Quote>(new QueryOperations(), iQueryable, tenant);

                QuoteBusinessUnitFilter myBusinessUnitFilter = new QuoteBusinessUnitFilter(tenant);
                iQueryable = myBusinessUnitFilter.RunFilter(iQueryable);

                if (!string.IsNullOrEmpty(ownerId))
                {
                    iQueryable = iQueryable.Where(d => d.SalesmanUserId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    iQueryable = iQueryable.Where(d => d.BusinessUnitId == businessUnitId);
                }

                myResult.Quotes_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.OpenDate) == todayDate).Count();
                myResult.Quotes_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.OpenDate) == yesterdayDate).Count();
                myResult.Quotes_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.OpenDate) >= lastWeekDate && DbFunctions.TruncateTime(d.OpenDate) <= yesterdayDate).Count();
            }

            if (SecurityUtility.CheckTableContactFeature("Customer", "READ", tenant))
            {
                CardRepository myRepository = new CardRepository(tenant);               
                IQueryable<Card> iQueryable = myRepository.GetCards(tenant);
                iQueryable = iQueryable.Where(d => d.InActive == false);
                iQueryable = iQueryable.Where(d => d.IsCustomer == true);
                iQueryable = iQueryable.Where(d => d.Customer != null);

                CustomerBusinessUnitFilter myBusinessUnitFilter = new CustomerBusinessUnitFilter(tenant);
                iQueryable = myBusinessUnitFilter.RunFilter(iQueryable);

                if (!string.IsNullOrEmpty(ownerId))
                {
                    iQueryable = iQueryable.Where(d => d.Customer.SalesmanUser == null || d.Customer.SalesmanUserId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    iQueryable = iQueryable.Where(d => d.Customer.SalesmanUser == null || d.Customer.SalesmanUser.BusinessUnitId == businessUnitId);
                }

                IQueryable<Card> iQueryable_CS = iQueryable.Where(d => d.Customer.CustomerStatusCode == "ACT");
                IQueryable<Card> iQueryable_PO = iQueryable.Where(d => d.Customer.CustomerStatusCode == "POT");

                myResult.Customers_Today = iQueryable_CS.Where(d => DbFunctions.TruncateTime(d.CreateDate) == todayDate).Count();
                myResult.Customers_Yesterday = iQueryable_CS.Where(d => DbFunctions.TruncateTime(d.CreateDate) == yesterdayDate).Count();
                myResult.Customers_LastWeek = iQueryable_CS.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= lastWeekDate && DbFunctions.TruncateTime(d.CreateDate) <= yesterdayDate).Count();

                myResult.PotentialCustomers_Today = iQueryable_PO.Where(d => DbFunctions.TruncateTime(d.CreateDate) == todayDate).Count();
                myResult.PotentialCustomers_Yesterday = iQueryable_PO.Where(d => DbFunctions.TruncateTime(d.CreateDate) == yesterdayDate).Count();
                myResult.PotentialCustomers_LastWeek = iQueryable_PO.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= lastWeekDate && DbFunctions.TruncateTime(d.CreateDate) <= yesterdayDate).Count();
            }

            if (SecurityUtility.CheckTableContactFeature("Activity", "READ", tenant))
            {
                ActivityRepository myRepository = new ActivityRepository(tenant);
                IQueryable<Activity> iQueryable = myRepository.GetAll(tenant);

                iQueryable = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Activity>(new QueryOperations(), iQueryable, tenant);

                ActivityBusinessUnitFilter myBusinessUnitFilter = new ActivityBusinessUnitFilter(tenant);
                iQueryable = myBusinessUnitFilter.RunFilter(iQueryable);

                if (!string.IsNullOrEmpty(ownerId))
                {
                    iQueryable = iQueryable.Where(d => d.OwnerId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    iQueryable = iQueryable.Where(d => d.BusinessUnitId == businessUnitId);
                }

                myResult.Activities_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == todayDate).Count();
                myResult.Activities_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == yesterdayDate).Count();
                myResult.Activities_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= lastWeekDate && DbFunctions.TruncateTime(d.CreateDate) <= yesterdayDate).Count();
            }

            if (SecurityUtility.CheckTableContactFeature("Opportunity", "READ", tenant))
            {
                OpportunityRepository myRepository = new OpportunityRepository(tenant);
                IQueryable<Opportunity> iQueryable = myRepository.GetAll(tenant);

                OpportunityBusinessUnitFilter myBusinessUnitFilter = new OpportunityBusinessUnitFilter(tenant);
                iQueryable = myBusinessUnitFilter.RunFilter(iQueryable);

                if (!string.IsNullOrEmpty(ownerId))
                {
                    iQueryable = iQueryable.Where(d => d.OwnerId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    iQueryable = iQueryable.Where(d => d.BusinessUnitId == businessUnitId);
                }

                myResult.Opportunities_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == todayDate).Count();
                myResult.Opportunities_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == yesterdayDate).Count();
                myResult.Opportunities_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= lastWeekDate && DbFunctions.TruncateTime(d.CreateDate) <= yesterdayDate).Count();
            }

            return myResult;
        }

        public DailySpotlightClass GetDashboardSpotlightCounts(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            DailySpotlightClass myResult = new DailySpotlightClass()
            {
                Id = tenant
            };

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime yesterdayDate = todayDate.AddDays(-1);
            DateTime lastWeekDate = todayDate.AddDays(-7);

            if (SecurityUtility.CheckTableContactFeature("Quote", "READ", tenant))
            {
                QuoteRepository myRepository = new QuoteRepository(tenant);
                IQueryable<Quote> iQueryable = myRepository.GetQuotes(tenant);
                iQueryable = iQueryable.Where(d => d.IsCancelled == false);

                iQueryable = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Quote>(new QueryOperations(), iQueryable, tenant);
                iQueryable = ProductPermitionsFilter.AddUserProductRestrictionFilters<Quote>(new QueryOperations(), iQueryable, tenant);

                QuoteBusinessUnitFilter myBusinessUnitFilter = new QuoteBusinessUnitFilter(tenant);
                iQueryable = myBusinessUnitFilter.RunFilter(iQueryable);

                myResult.Quotes_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.OpenDate) == todayDate).Count();
                myResult.Quotes_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.OpenDate) == yesterdayDate).Count();
                myResult.Quotes_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.OpenDate) >= lastWeekDate && DbFunctions.TruncateTime(d.OpenDate) <= yesterdayDate).Count();
            }

            if (SecurityUtility.CheckTableContactFeature("Shipment", "READ", tenant))
            {
                ShipmentRepository myRepository = new ShipmentRepository(tenant);
                IQueryable<Shipment> iQueryable = myRepository.GetShipments(tenant);
                iQueryable = iQueryable.Where(d => d.IsCancelled == false);
                iQueryable = iQueryable.Where(d => d.ShipmentLevelCode != "C" && d.IsOperationalClosed ==false);

                iQueryable = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), iQueryable, tenant);
                iQueryable = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), iQueryable, tenant);

                myResult.Shipments_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDateTime) == todayDate).Count();
                myResult.Shipments_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDateTime) == yesterdayDate).Count();
                myResult.Shipments_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDateTime) >= lastWeekDate && DbFunctions.TruncateTime(d.CreateDateTime) <= yesterdayDate).Count();
            }

            if (SecurityUtility.CheckTableContactFeature("ARInvoice", "READ", tenant))
            {
                ARInvoiceRepository myRepository = new ARInvoiceRepository(tenant);
                IQueryable<ARInvoice> iQueryable = myRepository.GetIQueryableInvoices(tenant);
                iQueryable = iQueryable.Where(d => d.StatusCode != "LL" && d.StatusCode != "VD");

                iQueryable = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), iQueryable, tenant);

                myResult.ARInvoices_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.InvoiceDate) == todayDate).Count();
                myResult.ARInvoices_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.InvoiceDate) == yesterdayDate).Count();
                myResult.ARInvoices_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.InvoiceDate) >= lastWeekDate && DbFunctions.TruncateTime(d.InvoiceDate) <= yesterdayDate).Count();
            }

            if (SecurityUtility.CheckTableContactFeature("Customer", "READ", tenant))
            {
                CardRepository myRepository = new CardRepository(tenant);
                IQueryable<Card> iQueryable = myRepository.GetCards(tenant);
                iQueryable = iQueryable.Where(d => d.InActive == false);
                iQueryable = iQueryable.Where(d => d.IsCustomer == true);
                iQueryable = iQueryable.Where(d => d.Customer != null);
                iQueryable = iQueryable.Where(d => d.Customer.CustomerStatusCode == "ACT");

                CustomerBusinessUnitFilter myBusinessUnitFilter = new CustomerBusinessUnitFilter(tenant);
                iQueryable = myBusinessUnitFilter.RunFilter(iQueryable);

                myResult.Customers_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == todayDate).Count();
                myResult.Customers_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == yesterdayDate).Count();
                myResult.Customers_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= lastWeekDate && DbFunctions.TruncateTime(d.CreateDate) <= yesterdayDate).Count();
            }

            return myResult;
        }

        public DailySpotlightClass GetAirlineDashboardSpotlightCounts(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            DailySpotlightClass myResult = new DailySpotlightClass()
            {
                Id = tenant
            };

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime yesterdayDate = todayDate.AddDays(-1);
            DateTime lastWeekDate = todayDate.AddDays(-7);

            if (SecurityUtility.CheckTableContactFeature("Participant", "READ", tenant))
            {
                ParticipantRepository myRepository = new ParticipantRepository(tenant);
                IQueryable<Participant> iQueryable = myRepository.GetParticipants(tenant);
                iQueryable = iQueryable.Where(d => d.Card.InActive == false);

                IQueryable<Participant> iQueryable_Registered = iQueryable.Where(d => d.Registered);

                myResult.Participations_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.Card.CreateDate) == todayDate).Count();
                myResult.Participations_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.Card.CreateDate) == yesterdayDate).Count();
                myResult.Participations_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.Card.CreateDate) >= lastWeekDate && DbFunctions.TruncateTime(d.Card.CreateDate) <= yesterdayDate).Count();

                myResult.NewParticipants_Today = iQueryable_Registered.Where(d => DbFunctions.TruncateTime(d.RegistrationDate) == todayDate).Count();
                myResult.NewParticipants_Yesterday = iQueryable_Registered.Where(d => DbFunctions.TruncateTime(d.RegistrationDate) == yesterdayDate).Count();
                myResult.NewParticipants_LastWeek = iQueryable_Registered.Where(d => DbFunctions.TruncateTime(d.RegistrationDate) >= lastWeekDate && DbFunctions.TruncateTime(d.RegistrationDate) <= yesterdayDate).Count();
            }

            if (SecurityUtility.CheckTableContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant))
            {
                LogitudeMessagesTransmissionLogRepository myRepository = new LogitudeMessagesTransmissionLogRepository(tenant);
                IQueryable<LogitudeMessagesTransmissionLog> iQueryable = myRepository.GetLogitudeMessagTransmissionLogs(tenant);

                IQueryable<LogitudeMessagesTransmissionLog> iQueryable_FWB = iQueryable.Where(d => d.MessageTypeCode == "FWB");
                IQueryable<LogitudeMessagesTransmissionLog> iQueryable_FHL = iQueryable.Where(d => d.MessageTypeCode == "FHL");
                IQueryable<LogitudeMessagesTransmissionLog> iQueryable_FFR = iQueryable.Where(d => d.MessageTypeCode == "FFR");

                myResult.FWB_Today = iQueryable_FWB.Where(d => DbFunctions.TruncateTime(d.SentDate) == todayDate).Count();
                myResult.FWB_Yesterday = iQueryable_FWB.Where(d => DbFunctions.TruncateTime(d.SentDate) == yesterdayDate).Count();
                myResult.FWB_LastWeek = iQueryable_FWB.Where(d => DbFunctions.TruncateTime(d.SentDate) >= lastWeekDate && DbFunctions.TruncateTime(d.SentDate) <= yesterdayDate).Count();

                myResult.FHL_Today = iQueryable_FHL.Where(d => DbFunctions.TruncateTime(d.SentDate) == todayDate).Count();
                myResult.FHL_Yesterday = iQueryable_FHL.Where(d => DbFunctions.TruncateTime(d.SentDate) == yesterdayDate).Count();
                myResult.FHL_LastWeek = iQueryable_FHL.Where(d => DbFunctions.TruncateTime(d.SentDate) >= lastWeekDate && DbFunctions.TruncateTime(d.SentDate) <= yesterdayDate).Count();

                myResult.FFR_Today = iQueryable_FFR.Where(d => DbFunctions.TruncateTime(d.SentDate) == todayDate).Count();
                myResult.FFR_Yesterday = iQueryable_FFR.Where(d => DbFunctions.TruncateTime(d.SentDate) == yesterdayDate).Count();
                myResult.FFR_LastWeek = iQueryable_FFR.Where(d => DbFunctions.TruncateTime(d.SentDate) >= lastWeekDate && DbFunctions.TruncateTime(d.SentDate) <= yesterdayDate).Count();
            }

            return myResult;
        }
    }
}