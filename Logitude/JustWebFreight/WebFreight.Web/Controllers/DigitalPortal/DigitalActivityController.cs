using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
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

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalActivityController : ApiController
    {
        [HttpGet]
        [Route("DigitalActivity/GetDigitalCardLogDetails")]
        public HttpResponseMessage GetDigitalCardLogDetails(string partnerTypeId, string dateParameter, int tenant)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                var cardRepository = new CardRepository(tenant);
                var contactRepository = new ContactRepository(tenant);

                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                var contactLogRep = new ContactActivityLogRepository();

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

                var filterdList = contactLogRep.GetContactActivityLogs(tenant)
                                               .Where(d => d.IsSharedLogisticsContact 
                                                           && d.PartnerTypeId == partnerTypeId 
                                                           && d.LogDateTime >= date1 
                                                           && d.LogDateTime <= date2
                                                           && d.Module.StartsWith("Digital Portal"))
                                               .ToList();
                var result = new List<CardLogDetails>();
                int i = 0;
                result = filterdList.GroupBy(a => new  { a.CardId, a.ContactId })
                .Select(g => new CardLogDetails
                {
                    Id = (i += 1),
                    CardId = g.Key.CardId,
                    ContactId = g.Key.ContactId,
                }).ToList();

                var contactIds = new List<string>();
                var cardIds = new List<string>();

                var items = result.Where(d => !string.IsNullOrWhiteSpace(d.CardId) 
                                              || !string.IsNullOrWhiteSpace(d.ContactId))
                                  .ToList();

                foreach (CardLogDetails item in items)
                {
                    if (!string.IsNullOrEmpty(item.CardId))
                    {
                        var cardId = cardIds.Where(d => d == item.CardId)
                                            .FirstOrDefault();

                        if (string.IsNullOrWhiteSpace(cardId))
                        {
                            cardIds.Add(item.CardId);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(item.ContactId))
                    {
                        var contactId = contactIds.Where(d => d == item.ContactId)
                                                  .FirstOrDefault();

                        if (string.IsNullOrWhiteSpace(contactId))
                        {
                            contactIds.Add(item.ContactId);
                        }
                    }
                }

                var cardLists = new List<CardList>();
                if (cardIds.Count > 0)
                {
                    var cardQuery = new CardQuery(tenant);
                    cardLists = cardQuery.GetCardListsByListIds(cardIds, tenant);
                }

                var contactLists = new List<ContactList>();
                if (contactIds.Count > 0)
                {
                    var contactQuery = new ContactQuery(tenant);
                    contactLists = contactQuery.GetContactListsByListIds(contactIds, tenant)
                                               .ToList();
                }

                foreach (CardLogDetails item in result)
                {
                    if (!string.IsNullOrWhiteSpace(item.CardId) && cardLists != null)
                    {
                        CardList cardList = cardLists.Where(d => d.Id == item.CardId)
                                                     .FirstOrDefault();

                        if (cardList != null)
                        {
                            item.CardName = cardList.EnglishName;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(item.ContactId) && contactLists != null)
                    {
                        ContactList contactList = contactLists.Where(d => d.Id == item.ContactId)
                                                              .FirstOrDefault();
                        if (contactList != null)
                        {
                            item.ContactName = contactList.EnglishName;
                        }
                    }

                    if (item.CardLogActivityDetails == null)
                    {
                        item.CardLogActivityDetails = new List<CardLogActivityDetails>();
                    }

                    int j = 0;
                    item.CardLogActivityDetails = filterdList.Where(r => r.CardId == item.CardId 
                                                                         && r.ContactId == item.ContactId)
                                                             .Select(r => new CardLogActivityDetails()
                                                             {
                                                                 Id = (j += 1),
                                                                 Activity = r.Activity,
                                                                 CardId = r.CardId,
                                                                 Module = r.Module,
                                                                 PartnerTypeId = r.PartnerTypeId,
                                                                 ContactId = r.ContactId,
                                                                 GMTLogDateTime = r.GMTLogDateTime,
                                                             })
                                                             .ToList();

                    item.NumberOfActivities = item.CardLogActivityDetails.Count();
                }

                foreach (var item in result)
                {
                    item.CardLogActivityDetails = new List<CardLogActivityDetails>();
                }

                var results = result.Where(a => a.NumberOfActivities > 0 && !string.IsNullOrWhiteSpace(a.CardName))
                                    .OrderByDescending(d => d.NumberOfActivities)
                                    .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalActivity/GetCardLogActivityDetailsList")]
        public HttpResponseMessage GetCardLogActivityDetailsList(string cardId, string contactId, string partnerTypeId, string dateParameter, int tenant)
        {

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                var contactLogRep = new ContactActivityLogRepository();

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

                int j = 0;
                var list = contactLogRep.GetContactActivityLogs(tenant)
                                        .Where(r => r.IsSharedLogisticsContact
                                                    && r.PartnerTypeId == partnerTypeId
                                                    && r.LogDateTime >= date1
                                                    && r.LogDateTime <= date2
                                                    && r.Module.StartsWith("Digital Portal") 
                                                    && r.CardId == cardId 
                                                    && r.ContactId == contactId)
                                      .ToList()
                                      .Select(r => new CardLogActivityDetails
                                      {
                                          Id = j += 1,
                                          Activity = r.Activity,
                                          CardId = r.CardId,
                                          Module = r.Module,
                                          PartnerTypeId = r.PartnerTypeId,
                                          ContactId = r.ContactId,
                                          GMTLogDateTime = r.GMTLogDateTime,
                                      })
                                      .OrderByDescending(d => d.GMTLogDateTime)
                                      .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalActivity/GetDigitalSharedLogisticsSummaryData")]
        public HttpResponseMessage GetDigitalSharedLogisticsSummaryData(int tenant)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();
                SharedLogisticsSummary result = new SharedLogisticsSummary();
                result.Id = 1;
                IQueryable<ContactActivityLog> allSharedLogisticsList = contactLogRep.GetSharedLogisticsContactLogs(tenant)
                                                                                     .Where(d => (d.PartnerTypeId == "CS" || d.PartnerTypeId == "AG")
                                                                                               && d.Module.StartsWith("Digital Portal"));

                if (allSharedLogisticsList.Count() > 0)
                {

                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                    DateTime todayDate1 = todayDate;
                    DateTime todayDate2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    DateTime lastWeekDate = todayDate.AddDays(-7);
                    DateTime yesterdayDate = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    DateTime lastMonthDate = todayDate.AddDays(-30);


                    result.TodayCustomersCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "CS" 
                                                                                && d.LogDateTime >= todayDate1 
                                                                                && d.LogDateTime <= todayDate2)
                                                                       .GroupBy(d => d.CardId)
                                                                       .Count(); 

                    result.LastWeekCustomersCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "CS" 
                                                                                   && d.LogDateTime >= lastWeekDate 
                                                                                   && d.LogDateTime <= yesterdayDate)
                                                                          .GroupBy(d => d.CardId)
                                                                          .Count();

                    result.LastMonthCustomersCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "CS" 
                                                                                    && d.LogDateTime >= lastMonthDate 
                                                                                    && d.LogDateTime <= yesterdayDate)
                                                                           .GroupBy(d => d.CardId)
                                                                           .Count(); 

                    result.TodayAgentsCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "AG" 
                                                                             && d.LogDateTime >= todayDate1 
                                                                             && d.LogDateTime <= todayDate2)
                                                                    .GroupBy(d => d.CardId)
                                                                    .Count();

                    result.LastWeekAgentsCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "AG" 
                                                                                && d.LogDateTime >= lastWeekDate 
                                                                                && d.LogDateTime <= yesterdayDate)
                                                                       .GroupBy(d => d.CardId)
                                                                       .Count();

                    result.LastMonthAgentsCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "AG" 
                                                                                 && d.LogDateTime >= lastMonthDate 
                                                                                 && d.LogDateTime <= yesterdayDate)
                                                                        .GroupBy(d => d.CardId)
                                                                        .Count(); 
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Execution Timeout Expired")) 
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new SharedLogisticsSummary());
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}