using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.SystemLogsModel.Queries;

namespace WebFreight.Web.Controllers.ShardLogistics
{
    public class SharedLogisticsController : ApiController
    {
        public HttpResponseMessage GetSharedLogisticsStatistics(int tenant)
        {

            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);

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
                SecurityUtility.AuthenticationOnTenant(tenant);

                ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();
                SharedLogisticsSummary result = new SharedLogisticsSummary();
                result.Id = 1;
                IQueryable<ContactActivityLog>allSharedLogisticsList = contactLogRep.GetSharedLogisticsContactLogs(tenant).Where( d=>d.PartnerTypeId == "CS" || d.PartnerTypeId == "AG");

                if (allSharedLogisticsList.Count() > 0)
                {

                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                    DateTime todayDate1 = todayDate;
                    DateTime todayDate2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    DateTime lastWeekDate = todayDate.AddDays(-7);
                    DateTime yesterdayDate = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    DateTime lastMonthDate = todayDate.AddDays(-30);


                    result.TodayCustomersCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "CS" && d.LogDateTime >= todayDate1 && d.LogDateTime <= todayDate2).GroupBy(d => d.CardId).Count();  //TodayCustomersList.GroupBy(d => d.CardId).Count();
                    result.LastWeekCustomersCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "CS" && d.LogDateTime >= lastWeekDate && d.LogDateTime <= yesterdayDate).GroupBy(d => d.CardId).Count();//LastWeekCustomersList.GroupBy(d => d.CardId).Count();
                    result.LastMonthCustomersCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "CS" && d.LogDateTime >= lastMonthDate && d.LogDateTime <= yesterdayDate).GroupBy(d => d.CardId).Count();  //LastMonthCustomersList.GroupBy(d => d.CardId).Count();
                    result.TodayAgentsCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "AG" && d.LogDateTime >= todayDate1 && d.LogDateTime <= todayDate2).GroupBy(d => d.CardId).Count(); //TodayAgentsList.GroupBy(d => d.CardId).Count();
                    result.LastWeekAgentsCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "AG" && d.LogDateTime >= lastWeekDate && d.LogDateTime <= yesterdayDate).GroupBy(d => d.CardId).Count();//LastWeekAgentsList.GroupBy(d => d.CardId).Count();
                    result.LastMonthAgentsCount = allSharedLogisticsList.Where(d => d.PartnerTypeId == "AG" && d.LogDateTime >= lastMonthDate && d.LogDateTime <= yesterdayDate).GroupBy(d => d.CardId).Count(); //LastMonthAgentsList.GroupBy(d => d.CardId).Count();
                }

   
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Execution Timeout Expired")) // Added by Rabaia, Temp
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new SharedLogisticsSummary());
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetLastLoginPartners(int tenant)
        {

            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);

                List<LastLoginPartners> result = new List<LastLoginPartners>();

                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                DateTime lastMonthDate = todayDate.AddDays(-30);

                SharedLogisticsContactLastLoginRepository sharedRepository = new SharedLogisticsContactLastLoginRepository(tenant);
                List<SharedLogisticsContactLastLogin> lastLoginsList = sharedRepository.GetSharedLogisticsContactLastLogins(tenant).OrderByDescending(d => d.LoginDateTime).Take(10).ToList();

                if (lastLoginsList.Count > 0)
                {
                    List<string> cardIds = lastLoginsList.Select(d => d.CardId).ToList();
                    List<string> contactIds = lastLoginsList.Select(d => d.ContactId).ToList();

                    CardQuery cardQuery = new CardQuery(tenant);
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    List<CardList> cards = cardQuery.GetCardListsByCardIds(cardIds, tenant);
                    List<ContactList> contacts = contactQuery.GetContactListsByListIds(contactIds, tenant).ToList();
                    int i = 0;
                    foreach (SharedLogisticsContactLastLogin item in lastLoginsList)
                    {
                        CardList card = cards.Where(d => d.Id == item.CardId).FirstOrDefault();
                        ContactList contact = contacts.Where(d => d.Id == item.ContactId).FirstOrDefault();
                        result.Add(new LastLoginPartners()
                        {
                            Id = (i += 1),
                            CardId = item.CardId,
                            CardName = card != null ? card.EnglishName : "",
                            ContactId = item.ContactId,
                            ContactName = contact != null ? contact.EnglishName : "",
                            PartnerTypeName = card == null ? "" : card.PartnerTypeName,
                            LastAccess = item.LoginDateTime,
                            Via = item.Via,
                        });

                    }
                }


                //ContactActivityLogQuery contactActivityLogQuery = new ContactActivityLogQuery();
                //List<LastLoginPartners> temp = contactActivityLogQuery.GetlastMonthLoginPartners(tenant).OrderByDescending(d => d.LogDateTime).Take(10).ToList();

                //if (temp.Count > 0)
                //{
                //    List<string> cardIds = temp.Select(d => d.CardId).ToList();
                //    List<string> contactIds = temp.Select(d => d.ContactId).ToList();

                //    CardQuery cardQuery = new CardQuery(tenant);
                //    ContactQuery contactQuery = new ContactQuery(tenant);
                //    List<CardList> cards = cardQuery.GetCardListsByCardIds(cardIds, tenant);
                //    List<ContactList> contacts = contactQuery.GetContactListsByListIds(contactIds, tenant).ToList();
                //    int i = 0;
                //    foreach (LastLoginPartners item in temp)
                //    {
                //        CardList card = cards.Where(d => d.Id == item.CardId).FirstOrDefault();
                //        ContactList contact = contacts.Where(d => d.Id == item.ContactId).FirstOrDefault();
                //        result.Add(new LastLoginPartners()
                //        {
                //            Id = (i += 1),
                //            CardId = item.CardId,
                //            CardName = card != null ? card.EnglishName : "",
                //            ContactId = item.ContactId,
                //            ContactName = contact != null ? contact.EnglishName : "",
                //            PartnerTypeName = card == null ? "" : card.PartnerTypeName,
                //            LastAccess = item.LogDateTime,
                //            Via = item.Via,
                //        });

                //    }
                //}
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Execution Timeout Expired"))// Added by Rabaia, Temp
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new List<LastLoginPartners>());
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCardLogDetails(string partnerTypeId, string dateParameter, int tenant)
        {

            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);

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



                foreach (CardLogDetails item in result.Where(d => !string.IsNullOrEmpty(d.CardId) || !string.IsNullOrEmpty(d.ContactId)))
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
                    contactLists = contactQuery.GetContactListsByListIds(contactIds, tenant).ToList();
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
                SecurityUtility.AuthenticationOnTenant(tenant);

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
                SecurityUtility.AuthenticationOnTenant(tenant);

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
                SecurityUtility.AuthenticationOnTenant(tenant);

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

        public HttpResponseMessage GetSharedShipments(string partnerId, string partnerType, string directionId, string transportModeId, 
            string levelCode, string searchField, int pageSize, int pageIndex, bool? isOperationalClosed)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                //SecurityUtility.CheckSharedContactAuthentication(tenant, filters.PartnerId);

                partnerId = this.FixFilter(partnerId);
                partnerType = this.FixFilter(partnerType);
                directionId = this.FixFilter(directionId);
                transportModeId = this.FixFilter(transportModeId);
                levelCode = this.FixFilter(levelCode);
                searchField = this.FixFilter(searchField);

                ShipmentFilters filters = new ShipmentFilters()
                {
                    PartnerId = partnerId,
                    PartnerType = partnerType,
                    DirectionId = directionId,
                    TransportModeId = transportModeId,
                    ShipmentLevelCode = levelCode,
                    SearchField = searchField,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    IsOperationalClosed = isOperationalClosed,
                };

                filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

                List<ShipmentList> listQuery = new List<ShipmentList>();
                List<ShipmentList> ReslutFollowShipments = new List<ShipmentList>();
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

                ObjectTableRepository rep = new ObjectTableRepository(tenant);
                ObjectTable table = rep.GetObjectTableByName("Shipment", 0, true);
                string email = HttpContext.Current.User.Identity.Name;

                if (string.IsNullOrEmpty(filters.ContactId))
                {
                    ContactRepository contactRepository = new ContactRepository(tenant);
                    Contact contact = contactRepository.GetSingleContactByEmail(email, tenant, true);

                    if (contact != null)
                    {
                        filters.ContactId = contact.Id;
                    }
                }

                ContactsUnseenEntitieRepository contactsUnseenRepository = new ContactsUnseenEntitieRepository(tenant);
                SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);

                QueryOperations queryOperations = new QueryOperations();
                string value = "";
                string name = "";
                if (filters.PartnerType == "CS")
                {
                    value = "D,H,A"; name = "CustomerId";
                }

                else if (filters.PartnerType == "AG")
                {
                    value = "D,C";
                    name = "AgentId";
                }

                queryOperations.SetFilter(name, filters.PartnerId, false, "Equals", null, false);
                queryOperations.SetFilter("ShipmentLevelCode", value, false, "InList", null, false);

                if (!string.IsNullOrEmpty(filters.SearchField))
                {
                    queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, false);
                }

                #region  AllShipment

                if (!filters.IsShipmentTracking)
                {

                    if (filters.DirectionId == "I")
                    {
                        queryOperations.SetFilter("DirectionId", "I,C", false, "InList", null, false);
                    }
                    else
                    {
                        queryOperations.SetFilter("DirectionId", filters.DirectionId, false, "Equals", null, true);
                    }

                    queryOperations.SetFilter("IsOperationalClosed", filters.IsOperationalClosed, false, "Equals", null, true);

                    if (!string.IsNullOrEmpty(filters.TransportModeId))
                    {
                        queryOperations.SetFilter("TransportModeId", filters.TransportModeId, false, "Equals", null, true);
                    }

                    if (!string.IsNullOrEmpty(filters.QueryType))
                    {
                        if (string.IsNullOrEmpty(filters.DirectionId) || string.IsNullOrWhiteSpace(filters.DirectionId))
                        {
                            filters.DirectionId = null;
                        }

                        queryOperations.SetFilter("DeparturesArrivalsMobileFilter", filters.QueryType, true, "Equals", filters.DirectionId, false);
                    }

                    GenericFilter filter = new GenericFilter();
                    GenericSort sortClass = new GenericSort();
                    ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);

                    IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);

                    shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                    shipments = (from a in shipments
                                 where ((a.DirectionId == "C" && a.CustomConnectToShipment == false) || a.DirectionId != "C") && !a.IsCancelled
                                 select a);

                    QueryOperations nonListQueryOperation = new QueryOperations();
                    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

                    QueryOperations listQueryOperation = new QueryOperations();
                    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                    shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
                    int skippedShipments = queryOperations.PageIndex;

                    var query2 = this.BuildShipmentList(currentTenant, shipments);

                    query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);

                    if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                    {
                        PropertyInfo propInfo = typeof(ShipmentList).GetProperty(queryOperations.SortByColumnName);
                        List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

                        ObjectField objectField = (from a in shipmentObjectFields
                                                   where a.FieldName == queryOperations.SortByColumnName
                                                   select a).FirstOrDefault();

                        if (objectField != null)
                        {
                            if (!objectField.IsCustom)
                            {
                                switch (objectField.DataTypeCode.ToLower())
                                {
                                    case "text":
                                        {
                                            query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                                            break;
                                        }
                                    case "double":
                                        {
                                            query2 = sortClass.GetSorterQuery<ShipmentList, double>(queryOperations, query2);
                                            break;
                                        }
                                    case "datetime":
                                        {
                                            query2 = sortClass.GetSorterQuery<ShipmentList, DateTime>(queryOperations, query2);
                                            break;
                                        }
                                    case "integer":
                                        {
                                            query2 = sortClass.GetSorterQuery<ShipmentList, int>(queryOperations, query2);
                                            break;
                                        }
                                    case "lookup":
                                        {
                                            query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                                            break;
                                        }
                                    case "boolean":
                                        {
                                            query2 = sortClass.GetSorterQuery<ShipmentList, bool>(queryOperations, query2);
                                            break;
                                        }
                                    default:
                                        {
                                            query2 = query2.OrderByDescending(d => d.StatusDate);
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                            }
                        }
                    }
                    else
                    {
                        query2 = query2.OrderByDescending(d => d.StatusDate);
                    }

                    query2 = query2.Skip(filters.PageIndex);
                    query2 = query2.Take(filters.PageSize);

                    listQuery = query2.ToList();

                    this.BuildUnssenFollowedShipment(tenant, listQuery, table, filters.ContactId, contactsUnseenRepository, sharedFollowedShipmentRepository, null);
                }

                #endregion

                #region Follow Shipment

                else
                {
                    GenericFilter filter = new GenericFilter();
                    GenericSort sortClass = new GenericSort();
                    ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);

                    IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
                    shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                    shipments = (from a in shipments
                                 where ((a.DirectionId == "C" && a.CustomConnectToShipment == false) || a.DirectionId != "C") && !a.IsCancelled
                                 select a);

                    QueryOperations nonListQueryOperation = new QueryOperations();
                    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

                    QueryOperations listQueryOperation = new QueryOperations();
                    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                    shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
                    int skippedShipments = queryOperations.PageIndex;

                    List<string> SharedFollowedShipmentListIds = (from a in sharedFollowedShipmentRepository.context.SharedFollowedShipments
                                                                  where a.ContactId == filters.ContactId && a.Tenant == tenant
                                                                  orderby a.TrackDate descending
                                                                  select a.ShipmentId).Skip(filters.PageIndex).Take(filters.PageSize).ToList();

                    IQueryable<ShipmentDataView> shipmentDataView = (from s in shipments
                                                                     where SharedFollowedShipmentListIds.Contains(s.Id)
                                                                     select s);


                    var query2 = this.BuildShipmentList(currentTenant, shipmentDataView);


                    query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);
                    query2 = query2.OrderByDescending(d => d.StatusDate);
                    listQuery = query2.ToList();

                    this.BuildUnssenFollowedShipment(tenant, listQuery, table, filters.ContactId, contactsUnseenRepository, sharedFollowedShipmentRepository, SharedFollowedShipmentListIds);
                }

                #endregion

                return Request.CreateResponse(HttpStatusCode.OK, listQuery);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private IQueryable<ShipmentList> BuildShipmentList(TenantPM currentTenant, IQueryable<ShipmentDataView> shipments)
        {
            var query2 = from f in shipments
                         select new ShipmentList()
                         {
                             CarrierLastStatusDate = f.CarrierLastStatusDate,
                             CarrierLastStatusName = f.CarrierLastStatusName,
                             CarrierLastStatusCode = f.CarrierLastStatusCode,
                             IsOperationalClosed = f.IsOperationalClosed,
                             ShipmentViewId = f.Id,
                             Id = f.Id,
                             DirectionId = f.DirectionId,
                             DirectionName = f.DirectionName,
                             TransportModeName = f.TransportModeName,
                             MasterShipmentNumber = f.MasterShipmentNumber,
                             House = f.House,
                             CreateDateTime = f.CreateDateTime,
                             ShipmentNumber = f.ShipmentNumber,
                             ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,
                             TransportModeId = f.TransportModeId,
                             Field1 = f.Field1,
                             Field2 = f.Field2,
                             Field3 = f.Field3,
                             Field4 = f.Field4,
                             Field5 = f.Field5,
                             Field6 = f.Field6,
                             Field7 = f.Field7,
                             Field9 = f.Field9,
                             Field8 = f.Field8,
                             Field11 = f.Field11,
                             Field12 = f.Field12,
                             Field13 = f.Field13,
                             Field14 = f.Field14,
                             Field15 = f.Field15,
                             Field16 = f.Field16,
                             Field17 = f.Field17,
                             Field18 = f.Field18,
                             Field19 = f.Field19,
                             Field20 = f.Field20,
                             Field21 = f.Field21,
                             Field22 = f.Field22,
                             Field23 = f.Field23,
                             Field24 = f.Field24,
                             Field25 = f.Field25,
                             Field26 = f.Field26,
                             Field27 = f.Field27,
                             Field28 = f.Field28,
                             Field29 = f.Field29,
                             Field30 = f.Field30,
                             Field31 = f.Field31,
                             Field32 = f.Field32,
                             Field33 = f.Field33,
                             Field34 = f.Field34,
                             Field35 = f.Field35,
                             Field36 = f.Field36,
                             Field37 = f.Field37,
                             Field38 = f.Field38,
                             Field39 = f.Field39,
                             Field40 = f.Field40,
                             CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                             Field10 = f.Field10,
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             Master = f.Master,
                             OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                             OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                             AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                             OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                             ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                             ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                             ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                             ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                             ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                             ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                             EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                             EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                             BranchId = f.BranchId,
                             DepartmentId = f.DepartmentId,
                             MainCarriageETA = f.MainCarriageETA,
                             MainCarriageATD = f.MainCarriageATD,
                             LocalCurrencyCode = currentTenant.CurrencyCode,
                             ProfitCurrencyCode = currentTenant.ProfitCurrencyCode,
                             NextETA = f.NextETA,
                             NextETD = f.NextETD,
                             NextLegName = f.NextLegName,
                             Routing = f.Routing,
                             MasterShipmentDataId = f.MasterShipmentDataId,
                             BranchName = f.BranchName,
                             GrossWeightInKG = f.GrossWeightInKG,
                             ShipmentLevelCode = f.ShipmentLevelCode,
                             ShipmentLevelName = f.ShipmentLevelName,
                             VolumetricWeight = f.VolumetricWeight,
                             AirlinePrefix = f.AirlinePrefix,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             MainCarriageATA = f.MainCarriageATA,
                             MainCarriageETD = f.MainCarriageETD,
                             IncotermId = f.IncotermId,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                             IncotermCode = f.IncotermCode,
                             MainCarriageCarrierId = f.MainCarriageCarrierId,
                             AsAgreedFreight = f.AsAgreedFreight,
                             AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                             AccountNumber = f.AccountNumber,
                             AWBPrint = f.AWBPrint,
                             FHLStatusCode = f.FHLStatusCode,
                             FHLStatusName = f.FHLStatusName,
                             FWBStatusCode = f.FWBStatusCode,
                             FWBStatusName = f.FWBStatusName,
                             LocalCustomsTransmissionsStatusCode = f.LocalCustomsTransmissionsStatusCode,
                             LocalCustomsTransmissionsStatusName = f.LocalCustomsTransmissionsStatusName,
                             LocalCustomsTransmissionsStatusDate = f.LocalCustomsTransmissionsStatusDate,
                             LocalCustomsTransmissionsStatusError = f.LocalCustomsTransmissionsStatusError,
                             FNAReason = f.FNAReason,
                             FinalArrivalDate = f.FinalArrivalDate,
                             Shipper = f.ShipperName,
                             Consignee = f.ConsigneeName,
                             ShipperReference1 = f.ShipperReference1,
                             ShipperReference2 = f.ShipperReference2,
                             ConsigneeReference1 = f.ConsigneeReference1,
                             ConsigneeReference2 = f.ConsigneeReference2,
                             CustomerId = f.CustomerId,
                             CustomerName = f.CustomerName,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                             FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                             FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                             FromPortCountry = f.MainCarriageFromPortCountryName,
                             MainCarriageFromPortId = f.MainCarriageFromPortId,
                             MainCarriageFromPortName = f.MainCarriageFromPortName,
                             ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                             ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                             ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                             ToCountryCode = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationCountryCode) ? f.MainCarriageFinalDestinationCountryCode : f.ToPortCountryCode,
                             ToPortCountry = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationCountryName) ? f.MainCarriageFinalDestinationCountryName : f.ToPortCountryName,
                             FromCountryCode = f.ShipmentLevelCode == "H" && string.IsNullOrEmpty(f.MasterShipmentDataId) ? f.FromPortCountryCode : f.MainCarriageFromPortCountryCode,
                             ChargeableWeightUnitCode = f.ChargeableWeightUnitCode,
                             NumberOfPackages = f.NumberOfPackages,
                             NumberOfContainers = f.NumberOfContainers,
                             BookingNumberOfPackages = f.BookingNumberOfPackages,
                             OrderChargeableWeight = f.OrderChargeableWeight,
                             MainCarriageFromCity = f.MainCarriageFromCity,
                             MainCarriageFromCountryCode = f.MainCarriageFromCountryCode,
                             MainCarriageToCity = f.MainCarriageToCity,
                             MainCarriageToCountryCode = f.MainCarriageToCountryCode,
                             MainCarriageCarrierCode = f.MainCarriageCarrierCode,
                             MainCarriageCarrierName = f.MainCarriageCarrierName,
                             MainCarriageCarrierNumber = f.MainCarriageCarrierNumber,
                             AgentName = f.AgentName,
                             AgentReference1 = f.AgentReference1,
                             AgentReference2 = f.AgentReference2,
                             LastUpdateDate = f.LastUpdateDate,
                             LastFSRStatusRequestDate = f.LastFSRStatusRequestDate,
                             CarrierNumber = f.TransportModeId == "A" ? (f.MainCarriageCarrierCode + f.MainCarriageCarrierNumber) : f.TransportModeId == "O" ? (f.MainCarriageVesselName + "/" + f.MainCarriageCarrierNumber) : f.TransportModeId == "I" ? (f.MainCarriageCarrierNumber) : null,
                             LastStatusLogDate = f.LastStatusLogDate,
                             HasException = f.HasException,
                             ExceptionDate = f.ExceptionDate,
                             ExceptionDescription = f.ExceptionDescription,
                             ExceptionResolvedDescription = f.ExceptionResolvedDescription,
                             LastExceptionDescription = f.LastExceptionDescription,
                             MainCarriageFinalDestinationETA = f.MainCarriageFinalDestinationETA,
                             MainCarriageFinalDestinationATA = f.MainCarriageFinalDestinationATA,
                             StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                             StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                             StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                             StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                             LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                             DescriptionOfGoods = f.DescriptionOfGoods,
                         };

            return query2;
        }
        private void BuildUnssenFollowedShipment(int tenant, List<ShipmentList> listQuery, ObjectTable table, string contactId, ContactsUnseenEntitieRepository contactsUnseenRepository, SharedFollowedShipmentRepository sharedFollowedShipmentRepository, List<string> SharedFollowedShipmentListIds)
        {
            List<string> trackedIds = (from a in listQuery select a.Id).ToList();
            IQueryable<string> contactsUnseenEntitieList = contactsUnseenRepository.GetContactsUnseenEntitiesByContactAndObjectTable(contactId, table.Id, tenant, trackedIds);

            if (SharedFollowedShipmentListIds == null)
            {
                SharedFollowedShipmentListIds = sharedFollowedShipmentRepository.GetSharedFollowedShipmentByContactId(contactId, tenant, trackedIds);
            }

            if (contactsUnseenEntitieList.Count() > 0 || SharedFollowedShipmentListIds.Count() > 0)
            {
                foreach (ShipmentList item in listQuery)
                {
                    item.IsOccurChange = contactsUnseenEntitieList.Contains(item.Id) ? true : false;
                    item.IsShipmentTracking = SharedFollowedShipmentListIds.Contains(item.Id) ? true : false;
                }
            }
        }
        private string FixFilter(string filter)
        {
            string myResult = filter;

            if (myResult != null)
            {
                switch (myResult.ToLower())
                {
                    case "all":
                    case "null":
                    case "undefined":
                        {
                            myResult = null;
                            break;
                        }
                }
            }

            return myResult;
        }
    }
}