using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShardLogistics
{
    public class SharedLogisticsController : ApiController
    {
        public HttpResponseMessage GetSharedLogisticsStatistics(int tenant)
        {

            try
            {
                Authentication();

                SharedLogisticsStatusStatistics dataClass = new SharedLogisticsStatusStatistics() { Id = "0001" };

                CardQuery cardQuery = new CardQuery(tenant);
                IQueryable<CardList> cards = cardQuery.GetCustomerCardPMsByTenant(tenant);
                IQueryable<CardList> agents = null;

                CustomerRepository CustomerRepository = new CustomerRepository(tenant);
                IQueryable<CustomersDataView> customers = CustomerRepository.GetCustomersDataViews(tenant);

                if (customers != null && customers.Count() > 0)
                {
                    customers = customers.Where(d => d.CustomerStatusCode == "ACT" && d.IsCustomer && !d.InActive);

                    dataClass.InvitedCustomersCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 2).Count();
                    dataClass.NotInvitedCustomersCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 1).Count();
                    dataClass.ActivatedCustomersCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 3 && !d.IsActiveForMobile).Count();
                    dataClass.ActivatedCustomersForMobileCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 3 && d.IsActiveForMobile).Count();
                }

                if (cards != null)
                {
                    cards = cards.Where(d => d.PartnerTypeId != "PO");

                    agents = cards.Where(d => d.PartnerTypeId == "AG" && !d.InActive);
                }

                if (agents != null && agents.Count() > 0)
                {
                    dataClass.InvitedAgentsCount = agents.Where(d => d.SharedLogisticsInvitationStatusCode == 2).Count();
                    dataClass.NotInvitedAgentsCount = agents.Where(d => d.SharedLogisticsInvitationStatusCode == 1).Count();
                    dataClass.ActivatedAgentsCount = agents.Where(d => d.SharedLogisticsInvitationStatusCode == 3).Count();
                }
                return Request.CreateResponse(HttpStatusCode.OK, dataClass);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSharedLogisticsSummaryData(int tenant)
        {
            try
            {
                Authentication();
                CardRepository cardRepository = new CardRepository(tenant);
                ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();

                SharedLogisticsSummary result = new SharedLogisticsSummary();
                IQueryable<ContactActivityLog> AllSharedLogisticsList = contactLogRep.GetSharedLogisticsContactLogs(tenant);

                List<SharedLogisticsCardLog> customersList = new List<SharedLogisticsCardLog>();
                List<SharedLogisticsCardLog> agnetsList = new List<SharedLogisticsCardLog>();
                foreach (ContactActivityLog log in AllSharedLogisticsList)
                {
                    if (!string.IsNullOrEmpty(log.PartnerTypeId))
                    {
                        if (log.PartnerTypeId == "CS") customersList.Add(new SharedLogisticsCardLog() { LogDateTime = log.LogDateTime, CardId = log.CardId });
                        else if (log.PartnerTypeId == "AG") agnetsList.Add(new SharedLogisticsCardLog() { LogDateTime = log.LogDateTime, CardId = log.CardId });
                    }

                }

                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                DateTime todayDate1 = todayDate;
                DateTime todayDate2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                DateTime lastWeekDate = todayDate.AddDays(-7);
                DateTime yesterdayDate = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                DateTime lastMonthDate = todayDate.AddDays(-30);

                List<SharedLogisticsCardLog> TodayCustomersList = customersList.Where(d => d.LogDateTime >= todayDate1 && d.LogDateTime <= todayDate2).ToList(); ;
                List<SharedLogisticsCardLog> LastWeekCustomersList = customersList.Where(d => d.LogDateTime >= lastWeekDate && d.LogDateTime <= yesterdayDate).ToList();
                List<SharedLogisticsCardLog> LastMonthCustomersList = customersList.Where(d => d.LogDateTime >= lastMonthDate && d.LogDateTime <= yesterdayDate).ToList();

                result.TodayCustomersCount = TodayCustomersList.GroupBy(d => d.CardId).Count();
                result.LastWeekCustomersCount = LastWeekCustomersList.GroupBy(d => d.CardId).Count();
                result.LastMonthCustomersCount = LastMonthCustomersList.GroupBy(d => d.CardId).Count();

                List<SharedLogisticsCardLog> TodayAgentsList = agnetsList.Where(d => d.LogDateTime >= todayDate1 && d.LogDateTime <= todayDate2).ToList();
                List<SharedLogisticsCardLog> LastWeekAgentsList = agnetsList.Where(d => d.LogDateTime >= lastWeekDate && d.LogDateTime <= yesterdayDate).ToList();
                List<SharedLogisticsCardLog> LastMonthAgentsList = agnetsList.Where(d => d.LogDateTime >= lastMonthDate && d.LogDateTime <= yesterdayDate).ToList();

                result.TodayAgentsCount = TodayAgentsList.GroupBy(d => d.CardId).Count();
                result.LastWeekAgentsCount = LastWeekAgentsList.GroupBy(d => d.CardId).Count();
                result.LastMonthAgentsCount = LastMonthAgentsList.GroupBy(d => d.CardId).Count();

                result.Id = 1;


                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetLastLoginPartners(int tenant)
        {

            try
            {
                Authentication();


                List<LastLoginPartners> result = new List<LastLoginPartners>();
                ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();
                List<ContactActivityLog> AllSharedLogisticsList = contactLogRep.GetContactActivityLogs(tenant).Where(d => d.IsSharedLogisticsContact && (d.Activity == "Customer Access")).ToList(); // || d.Activity == "Agent Access"
                CardRepository cardRepository = new Simplog.Data.CommonDataModel.Repositories.CardRepository(tenant);
                ContactRepository contactRepository = new Simplog.Data.CommonDataModel.Repositories.ContactRepository(tenant);

                int i = 0;
                List<LastLoginPartners> temp = (from r in AllSharedLogisticsList
                                                group r by new { r.CardId, r.ContactId, r.Via }
                                                    into g
                                                    select new LastLoginPartners()
                                                    {
                                                        Id = (i += 1),
                                                        CardId = g.Key.CardId,
                                                        ContactId = g.Key.ContactId,
                                                        Via = g.Key.Via,

                                                    }).ToList();

                foreach (LastLoginPartners item in temp)
                {

                    ContactActivityLog log = AllSharedLogisticsList.Where(c => c.ContactId == item.ContactId && c.CardId == item.CardId).OrderByDescending(d => d.GMTLogDateTime).FirstOrDefault();

                    if (log != null)
                    {
                        Card card = cardRepository.GetSingleCardWithoutInclude(log.CardId, tenant);
                        Contact contact = contactRepository.GetSingleContact(log.ContactId, tenant);

                        result.Add(new LastLoginPartners()
                        {
                            Id = item.Id,
                            CardId = log.CardId,
                            CardName = card != null ? card.EnglishName : "",
                            ContactId = log.ContactId,
                            ContactName = contact != null ? contact.EnglishName : "",
                            PartnerTypeName = card == null ? "" : (card.PartnerType == null ? "" : card.PartnerType.Name),
                            LastAccess = log.GMTLogDateTime,
                            Via = log.Via,
                        });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, result.OrderByDescending(d => d.LastAccess).Take(10).ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage  GetCardLogDetails(string partnerTypeId, string dateParameter, int tenant)
        {

            try
            {
                Authentication();
                CardRepository cardRepository = new CardRepository(tenant);
                ContactRepository contactRepository = new ContactRepository(tenant);

                List<CardLogDetails> result = new List<CardLogDetails>();
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();

                DateTime? date1 = null;
                DateTime? date2 = null;

                switch (dateParameter)
                {
                    case "T":
                        {
                            date1 = todayDate;
                            date2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                            break;
                        }
                    case "W":
                        {
                            date1 = todayDate.AddDays(-7);
                            date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                            break;
                        }
                    case "M":
                        {
                            date1 = todayDate.AddDays(-30);
                            date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                            break;
                        }
                }
                

                List<ContactActivityLog> filterdList = contactLogRep.GetContactActivityLogs(tenant).Where(d => d.IsSharedLogisticsContact && d.PartnerTypeId == partnerTypeId && d.LogDateTime >= date1 && d.LogDateTime <= date2).ToList();

           

                int i = 0;
                result = (from item in filterdList
                          group item by new { item.CardId, item.ContactId }
                              into g
                              select new CardLogDetails()
                              {
                                  Id = (i += 1),
                                  CardId = g.Key.CardId,
                                  ContactId = g.Key.ContactId,
                              }).ToList();



                List<string> contactIds = new List<string>();
                List<string> cardIds = new List<string>();
                
        

                foreach (CardLogDetails item in result.Where(d=>!string.IsNullOrEmpty(d.CardId) || !string.IsNullOrEmpty(d.ContactId)))
                {

                    if (!string.IsNullOrEmpty(item.CardId))
                    {
                        var cardId = cardIds.Where(d => d == item.CardId).FirstOrDefault();
                        if (string.IsNullOrEmpty(cardId)) cardIds.Add(item.CardId);
                    }

                    if (!string.IsNullOrEmpty(item.ContactId))
                    {
                        var contactId = contactIds.Where(d => d == item.ContactId).FirstOrDefault();
                        if (string.IsNullOrEmpty(contactId)) contactIds.Add(item.ContactId);
                    }
                }


                List<CardList> cardLists = new List<CardList>();
                if (cardIds.Count > 0)
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    cardLists = cardQuery.GetCardListsByListIds(cardIds, tenant);
                }



                List<ContactList> contactLists = new List<ContactList>();
                if (contactIds.Count > 0)
                {
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    contactLists = contactQuery.GetContactListsByListIds(contactIds, tenant);
                }


                foreach (CardLogDetails item in result)
                {
                    if (!string.IsNullOrEmpty(item.CardId) && cardLists != null)
                    {
                        CardList cardList = cardLists.Where(d => d.Id == item.CardId).FirstOrDefault();
                        if (cardList != null) item.CardName = cardList.EnglishName;

                    }

                    if (!string.IsNullOrEmpty(item.ContactId) && contactLists != null)
                    {
                        ContactList contactList = contactLists.Where(d => d.Id == item.ContactId).FirstOrDefault();
                        if (contactList != null) item.ContactName = contactList.EnglishName;

                    }


                    if (item.CardLogActivityDetails == null)
                    {
                        item.CardLogActivityDetails = new List<CardLogActivityDetails>();
                    }

                    int j = 0;
                    item.CardLogActivityDetails = (from r in filterdList
                                                   where r.CardId == item.CardId && r.ContactId == item.ContactId
                                                   select new CardLogActivityDetails()
                                                   {
                                                       Id = (j += 1),
                                                       Activity = r.Activity,
                                                       CardId = r.CardId,
                                                       Module = r.Module,
                                                       PartnerTypeId = r.PartnerTypeId,
                                                       ContactId = r.ContactId,
                                                       GMTLogDateTime = r.GMTLogDateTime,
                                                   }).ToList();

                    item.NumberOfActivities = item.CardLogActivityDetails.Count();
                }
                return Request.CreateResponse(HttpStatusCode.OK, result.OrderByDescending(d => d.NumberOfActivities).ToList());


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCardLogActivityDetailsList(string cardId, string contactId, string partnerTypeId, string dateParameter, int tenant)
        {

            try
            {
                Authentication();

                List<CardLogActivityDetails> list = new List<CardLogActivityDetails>();
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();

                DateTime? date1 = null;
                DateTime? date2 = null;

                switch (dateParameter)
                {
                    case "T":
                        {
                            date1 = todayDate;
                            date2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                            break;
                        }
                    case "W":
                        {
                            date1 = todayDate.AddDays(-7);
                            date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                            break;
                        }
                    case "M":
                        {
                            date1 = todayDate.AddDays(-30);
                            date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                            break;
                        }
                }

                List<ContactActivityLog> filterdList = contactLogRep.GetContactActivityLogs(tenant).Where(d => d.IsSharedLogisticsContact && d.PartnerTypeId == partnerTypeId && d.LogDateTime >= date1 && d.LogDateTime <= date2).ToList();

                int j = 0;
                list = (from r in filterdList
                        where r.CardId == cardId && r.ContactId == contactId
                        select new CardLogActivityDetails()
                        {
                            Id = (j += 1),
                            Activity = r.Activity,
                            CardId = r.CardId,
                            Module = r.Module,
                            PartnerTypeId = r.PartnerTypeId,
                            ContactId = r.ContactId,
                            GMTLogDateTime = r.GMTLogDateTime,
                        }).OrderByDescending(d => d.GMTLogDateTime).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomerTenantAccessRequestStatusCount(int tenant)
        {
            try
            {
                Authentication();

                CustomerTenantAccessRequestStatusCount dataClass = new CustomerTenantAccessRequestStatusCount() { Id = "0001" };

                var customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                IQueryable<CustomerTenantAccessList> CustomerTenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessListByTenant(tenant);



                if (CustomerTenantAccess != null && CustomerTenantAccess.Count() > 0)
                {
                    dataClass.WaitingCount = CustomerTenantAccess.Where(d => d.Status.ToUpper() == "W").Count();
                    dataClass.InProgressCount = CustomerTenantAccess.Where(d => d.Status.ToUpper() == "IP").Count();
                    dataClass.AcceptedCount = CustomerTenantAccess.Where(d => d.Status.ToUpper() == "A").Count();
                    dataClass.InactiveCount = CustomerTenantAccess.Where(d => d.Status.ToUpper() == "IA").Count();


                }



                return Request.CreateResponse(HttpStatusCode.OK, dataClass);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
           
        }

        public HttpResponseMessage GetLastCustomerRequest(int tenant)
        {
            try
            {
                Authentication();
                var customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                List<CustomerTenantAccessList> list = customerTenantAccessQuery.GetLastCustomerRequests(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            } 
        }

         private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        }
    }
}