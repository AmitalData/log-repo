using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.CRMModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.QuoteModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class CRMDomainController : ApiController
    {
        #region Ticket

        public HttpResponseMessage GetUpdateCorrespondence(string entityId, bool rightToLeft)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                CRMDomainService crmDomain = new CRMDomainService();
                CorrespondencePM entityPM = crmDomain.GetSingleCorrespondencePM(entityId, tenant);
                entityPM.RightToLeft = rightToLeft;
                crmDomain.UpdateCorrespondence(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetActiveSLAbyTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                SLAHeaderQueryService mySLAHeaderQueryService = new SLAHeaderQueryService(tenant);
                List<SLAHeaderPM> myList = mySLAHeaderQueryService.GetActiveSLAbyTenant(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetConnectContactCards(string companyId, string contactId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ICommonDataContext objectContext = CommonDataContext.GetContext(authToken.Tenant);
                CardContactRepository cardContactRepository = new CardContactRepository(objectContext);

                CardContact cardContact = new CardContact()
                {
                    CardId = companyId,
                    ContactId = contactId,
                    Id = IdCounter.GetNumber("CardContact", authToken.Tenant).ToString(),
                    Tenant = authToken.Tenant,
                };
                cardContactRepository.Add(cardContact);

                IQueryable<Contact> myContacts = cardContactRepository.GetContactsByCardId(companyId);
                if (myContacts == null || (myContacts != null && myContacts.Count() == 0))
                {
                    CardQuery cardQuery = new CardQuery(authToken.Tenant);
                    CardPM card = cardQuery.GetSinglePM(companyId, authToken.Tenant);
                    card.PrimaryContactId = contactId;
                    CardService cardService = new CardService(objectContext, card);
                    cardService.Update();
                }

                objectContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetContactCards(string companyId, string contactId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    companyId = this.FixFilter(companyId);
                    contactId = this.FixFilter(contactId);

                    CardContactRepository myCardContactRepository = new CardContactRepository(tenant);
                    CardContact myContact = myCardContactRepository.GetSingleCardContact(contactId, companyId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myContact);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage InserNewTicket(TicketPM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    CRMDomainService crmDomain = new CRMDomainService();
                    crmDomain.InsertTicket(entityPM);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetOwnerEmployeeGroup(string ownerId, string employeeGroupId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    CRMDomainService crmDomain = new CRMDomainService();
                    string myOwnerId = crmDomain.CheckOwnerEmployeeGroup(ownerId, employeeGroupId, tenant);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myOwnerId);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetRecentTickets(string ownerId, string employeeGroupId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                CRMDomainService domain = new CRMDomainService();
                List<TicketList> myResult = domain.GetRecentTickets(ownerId, employeeGroupId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTicketCorrespondences(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

                CRMDomainService ticketDomain = new CRMDomainService();
                List<CorrespondencePM> myResult = ticketDomain.GetCorrespondencesList(entityId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTicketsCounts(string ownerId, string employeeGroupId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                ownerId = this.FixFilter(ownerId);
                employeeGroupId = this.FixFilter(employeeGroupId);

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Ticket", "READ", tenant);

                CRMDomainService ticketDomain = new CRMDomainService();
                CRMSummary myResult = ticketDomain.GetTicketsSummary(ownerId, employeeGroupId, tenant, loggedUserEmail);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetEmployeeGroupsPMList()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);

                CRMDomainService ticketDomain = new CRMDomainService();
                var myResult = ticketDomain.GetEmployeeGroupsPMList(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUsersByEmployeeGroupIds(string employeeIds)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                List<string> ids = employeeIds.Split(':').ToList();
                UserQuery query = new UserQuery(tenant);
                var myResult = query.GetUserListByUserIds(ids, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetContactListsByEmailsString(string emails)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                ContactQuery query = new ContactQuery(tenant);
                var myResult = query.GetContactListsByEmailsString(emails, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUserListsByEmailsString(string emails)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                UserQuery query = new UserQuery(tenant);
                var myResult = query.GetUserListsByEmailsString(emails, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTicketEscalationListsByTicketId(string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    CRMDomainService crmDomain = new CRMDomainService();
                    var results = crmDomain.GetTicketEscalationListsByTicketId(entityId, tenant);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, results);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTicketOverViewStatisticsSummary(string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    CRMDomainService crmDomain = new CRMDomainService();
                    var results = crmDomain.GetTicketOverViewStatisticsSummary(entityId, tenant);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, results);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetBusinessHours(string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    CRMDomainService crmDomain = new CRMDomainService();
                    var results = crmDomain.CalculatingBusinessHours(entityId, tenant);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, results);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleSLAHeaderPMByTenant()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    CRMDomainService crmDomain = new CRMDomainService();
                    var results = crmDomain.GetSingleSLAHeaderPMByTenant(tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, results);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTicketOwnerPermission(string ownerId, string ownerName)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                TicketUpdateService service = new TicketUpdateService(tenant);
                service.CheckOwnerFeature(tenant, ownerId, ownerName);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        #endregion 

        public HttpResponseMessage GetUpcomigActivities(string ownerId, string businessUnitId, string activityTypeCodeFilter, string RecordsTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

                ownerId = this.FixFilter(ownerId);
                businessUnitId = this.FixFilter(businessUnitId);
                RecordsTypeCode = this.FixFilter(RecordsTypeCode);
                activityTypeCodeFilter = this.FixFilter(activityTypeCodeFilter);

                ICRMContext myContext = CRMContext.GetContext(tenant);
                ActivityListQueryService queryService = new ActivityListQueryService(myContext);
                IQueryable<ActivityList> first = queryService.GetUpcomigEntityLists(ownerId, businessUnitId, tenant, activityTypeCodeFilter, RecordsTypeCode).AsQueryable();
                IQueryable<ActivityList> list = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);
                List<ActivityList> unsortingList = list.ToList();
                List<ActivityList> result = new List<ActivityList>();
                foreach (ActivityList item in unsortingList.OrderBy(o => o.SortByDate))
                {
                    result.Add(item);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCompleteActivity(string activityId, bool post, string summary)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                CRMDomainService crmDomain = new CRMDomainService();
                crmDomain.CompleteActivity(activityId, post, summary, tenant);
                ICRMContext crmContext = CRMContext.GetContext(tenant);
                ActivityListQueryService query = new ActivityListQueryService(crmContext);
                var list = query.GetSingle(activityId);
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutCompleteActivity(MeetingSummary args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                ActivityList list = null;
                if (args != null)
                {
                    string activityId = args.ActivityId;
                    bool post = args.Post;
                    string summary = args.Summary;
                    CRMDomainService crmDomain = new CRMDomainService();
                    crmDomain.CompleteActivity(activityId, post, summary, tenant);
                    ICRMContext crmContext = CRMContext.GetContext(tenant);
                    ActivityListQueryService query = new ActivityListQueryService(crmContext);
                    list = query.GetSingle(activityId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetReopenActivity(string activityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                CRMDomainService crmDomain = new CRMDomainService();
                crmDomain.ReopenActivity(activityId, tenant);
                ICRMContext crmContext = CRMContext.GetContext(tenant);
                ActivityListQueryService query = new ActivityListQueryService(crmContext);
                var list = query.GetSingle(activityId);
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetActivitiesSummary(string activityTypeCodeFilter, string ownerId, string businessUnitId, string RecordsTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

                ownerId = this.FixFilter(ownerId);
                businessUnitId = this.FixFilter(businessUnitId);
                RecordsTypeCode = this.FixFilter(RecordsTypeCode);
                activityTypeCodeFilter = this.FixFilter(activityTypeCodeFilter);

                ICRMContext myContext = CRMContext.GetContext(tenant);
                ActivityRepository activityRepository = new ActivityRepository(myContext);
                IQueryable<Activity> dataSource = activityRepository.GetAll(tenant);

                dataSource = dataSource.Where(d => d.IsOpen);

                dataSource = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Activity>(new QueryOperations(), dataSource, tenant);

                ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
                dataSource = filter.RunFilter(dataSource);

                if (!string.IsNullOrEmpty(activityTypeCodeFilter))
                {
                    dataSource = dataSource.Where(d => d.ActivityTypeCode == activityTypeCodeFilter);
                }

                CRMSummary myResult = new CRMSummary() { Id = tenant };

                if (RecordsTypeCode == "C")
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        dataSource = dataSource.Where(d => d.CreatedByUserId == ownerId);
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        dataSource = dataSource.Where(d => d.OwnerId == ownerId);
                    }

                    if (!string.IsNullOrEmpty(businessUnitId))
                    {
                        dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
                    }
                }

                string loggedUserId = this.GetLoggedUserId(loggedUserEmail, tenant);

                myResult.MyOpenDataCount = dataSource.Where(d => d.OwnerId == loggedUserId).Count();
                myResult.AllOpenDataCount = dataSource.Count();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCRMDailySpotlightCounts(string ownerId, string businessUnitId, string RecordsTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ownerId = this.FixFilter(ownerId);
                businessUnitId = this.FixFilter(businessUnitId);
                RecordsTypeCode = this.FixFilter(RecordsTypeCode);

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

                    if (RecordsTypeCode == "C")
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.CreatedByUserId == ownerId);
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.SalesmanUserId == ownerId);
                        }

                        if (!string.IsNullOrEmpty(businessUnitId))
                        {
                            iQueryable = iQueryable.Where(d => d.BusinessUnitId == businessUnitId);
                        }
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

                    if (RecordsTypeCode == "C")
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.CreatedByUserId == ownerId);
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.Customer.SalesmanUserId == ownerId);
                        }

                        if (!string.IsNullOrEmpty(businessUnitId))
                        {
                            iQueryable = iQueryable.Where(d => d.Customer.SalesmanUser.BusinessUnitId == businessUnitId);
                        }
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

                    if (RecordsTypeCode == "C")
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.CreatedByUserId == ownerId);
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.OwnerId == ownerId);
                        }

                        if (!string.IsNullOrEmpty(businessUnitId))
                        {
                            iQueryable = iQueryable.Where(d => d.BusinessUnitId == businessUnitId);
                        }
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

                    if (RecordsTypeCode == "C")
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.CreatedByUserId == ownerId);
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            iQueryable = iQueryable.Where(d => d.OwnerId == ownerId);
                        }

                        if (!string.IsNullOrEmpty(businessUnitId))
                        {
                            iQueryable = iQueryable.Where(d => d.BusinessUnitId == businessUnitId);
                        }
                    }

                    myResult.Opportunities_Today = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == todayDate).Count();
                    myResult.Opportunities_Yesterday = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) == yesterdayDate).Count();
                    myResult.Opportunities_LastWeek = iQueryable.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= lastWeekDate && DbFunctions.TruncateTime(d.CreateDate) <= yesterdayDate).Count();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetRecentOpportunities(string ownerId, string businessUnitId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    CRMDomainService crmDomain = new CRMDomainService();
                    List<OpportunityList> myResult = crmDomain.GetRecentOpportunities(ownerId, businessUnitId, tenant);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetActivitiesDashBoard(string OwnerId, string BusinessUnitId, string activityTypeCodeFilter, string RecordsTypeCode)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

                    OwnerId = this.FixFilter(OwnerId);
                    BusinessUnitId = this.FixFilter(BusinessUnitId);
                    RecordsTypeCode = this.FixFilter(RecordsTypeCode);
                    activityTypeCodeFilter = this.FixFilter(activityTypeCodeFilter);

                    ICRMContext myContext = CRMContext.GetContext(tenant);
                    ActivityQueryService activityQuery = new ActivityQueryService(myContext);

                    List<CRMChartingClass> data = activityQuery.GetActivitiesDashBoard(OwnerId, BusinessUnitId, tenant, activityTypeCodeFilter, RecordsTypeCode);
                    List<ChartingDataClass> myResult = new List<ChartingDataClass>();
                    foreach (CRMChartingClass item in data)
                    {
                        myResult.Add(new ChartingDataClass()
                        {
                            Id = item.Id,
                            DateTimeProperty = item.DateTimeProperty,
                            StringProperty = item.StringProperty,
                            IntegerProperty = item.IntegerProperty,
                            LabelProperty = item.LabelProperty,
                            TypeIndex = item.TypeIndex,
                            OwnerId = OwnerId,
                            BusinessUnitId = BusinessUnitId,
                            RecordsTypeCode = RecordsTypeCode,
                            DataTypeCode = item.DataTypeCode,
                        });
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetOpportunitiesSummary(string ownerId, string businessUnitId, string RecordsTypeCode)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);
                    RecordsTypeCode = this.FixFilter(RecordsTypeCode);

                    OpportunityRepository opportunityRepository = new OpportunityRepository(tenant);
                    IQueryable<Opportunity> dataSource = opportunityRepository.GetAll(tenant).Where(d => d.IsCancelled == false);

                    OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
                    dataSource = filter.RunFilter(dataSource);

                    CRMSummary myResult = new CRMSummary() { Id = tenant };

                    if (RecordsTypeCode == "C")
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            dataSource = dataSource.Where(d => d.CreatedByUserId == ownerId);
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(ownerId))
                        {
                            dataSource = dataSource.Where(d => d.OwnerId == ownerId);
                        }

                        if (!string.IsNullOrEmpty(businessUnitId))
                        {
                            dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
                        }
                    }

                    dataSource = dataSource.Where(d => d.IsClosed == false);

                    string loggedUserId = this.GetLoggedUserId(loggedUserEmail, tenant);

                    myResult.AllOpenDataCount = dataSource.Count();
                    myResult.MyOpenDataCount = dataSource.Where(d => d.OwnerId == loggedUserId).Count();
                    myResult.OpenByStageCount = dataSource.Where(d => d.LastStageDate != null).Count();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetStageFunnelData(string ownerId, string businessUnitId, string filterCode, string RecordsTypeCode)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);
                    RecordsTypeCode = this.FixFilter(RecordsTypeCode);

                    ICRMContext crmContext = CRMContext.GetContext(tenant);
                    OpportunityQueryService opportunityQuery = new OpportunityQueryService(crmContext);

                    List<CRMChartingClass> data = opportunityQuery.GetStageFunnelData(ownerId, businessUnitId, filterCode, tenant, RecordsTypeCode);

                    List<ChartingDataClass> result = new List<ChartingDataClass>();

                    foreach (CRMChartingClass item in data)
                    {
                        result.Add(new ChartingDataClass()
                        {
                            Id = item.Id,
                            LabelProperty = item.LabelProperty,
                            DecimalProperty = item.DecimalProperty,
                            IntegerProperty = item.IntegerProperty,
                            OwnerId = ownerId,
                            BusinessUnitId = businessUnitId,
                            GroupedId = item.GroupedId,
                            DataTypeCode = filterCode,
                            RecordsTypeCode = RecordsTypeCode,
                        });
                    }

                    List<ChartingDataClass> myResult = result.OrderBy(d => d.IntegerProperty).ToList();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetQuotesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    QuotesDomainService crmDomain = new QuotesDomainService();

                    List<ChartingDataClass> myResult = crmDomain.GetQuotesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetQuotesGroupBySalesmanCustom(string FromDate, string ToDate, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (FromDate == "null")
                        FromDate = null;

                    if (ToDate == "null")
                        ToDate = null;




                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                    if (FromDate == null)
                    {
                        FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }


                    DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                    if (ToDateOBJ == null)
                    {
                        ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    QuotesDomainService crmDomain = new QuotesDomainService();

                    List<ChartingDataClass> myResult = crmDomain.GetQuotesGroupBySalesmanCustom(FromDateOBJ, ToDateOBJ, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomersGroupBySalesmanCustom(string FromDate, string ToDate, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (FromDate == "null")
                        FromDate = null;

                    if (ToDate == "null")
                        ToDate = null;

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                    if (FromDate == null)
                    {
                        FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                    if (ToDateOBJ == null)
                    {
                        ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }
                    PartnersDomainService crmDomain = new PartnersDomainService();
                    List<ChartingDataClass> myResult = crmDomain.GetCustomersGroupBySalesmanCustom(FromDateOBJ, ToDateOBJ, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCustomersGroupBySalesman(int days, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    PartnersDomainService crmDomain = new PartnersDomainService();

                    List<ChartingDataClass> myResult = crmDomain.GetCustomersGroupBySalesman(days, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetActivitiesGroupBySalesmanCustom(string FromDate, string ToDate, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (FromDate == "null")
                        FromDate = null;

                    if (ToDate == "null")
                        ToDate = null;

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                    if (FromDate == null)
                    {
                        FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                    if (ToDateOBJ == null)
                    {
                        ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }
                    CRMDomainService crmDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = crmDomain.GetActivitiesGroupBySalesmanCustom(FromDateOBJ, ToDateOBJ, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetActivitiesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService crmDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = crmDomain.GetActivitiesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetOpportunitiesGroupBySalesmanCustom(string FromDate, string ToDate, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    if (FromDate == "null")
                        FromDate = null;

                    if (ToDate == "null")
                        ToDate = null;

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                    if (FromDate == null)
                    {
                        FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                    if (ToDateOBJ == null)
                    {
                        ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    CRMDomainService crmDomain = new CRMDomainService();
                    List<ChartingDataClass> myResult = crmDomain.GetOpportunitiesGroupBySalesmanCustom(FromDateOBJ, ToDateOBJ, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetOpportunitiesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, bool isTopTen)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    CRMDomainService crmDomain = new CRMDomainService();
                    List<ChartingDataClass> myResult = crmDomain.GetOpportunitiesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetQuotesChartDataCustom(string FromDate, string ToDate, string ownerId, string businessUnitId, string chartCode)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (FromDate == "null")
                        FromDate = null;

                    if (ToDate == "null")
                        ToDate = null;
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;
                    DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                    if (FromDate == null)
                    {
                        FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                    if (ToDateOBJ == null)
                    {
                        ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    QuotesDomainService QuoteDomain = new QuotesDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetQuotesChartDataCustom(FromDateOBJ, ToDateOBJ, ownerId, businessUnitId, chartCode, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetQuotesChartData(string code, string ownerId, string businessUnitId, string chartCode)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    QuotesDomainService QuoteDomain = new QuotesDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetQuotesChartData(code, ownerId, businessUnitId, chartCode, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetOpportunitiesChartDataCustom(string FromDate, string ToDate, string ownerId, string businessUnitId, string chartCode)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (FromDate == "null")
                        FromDate = null;

                    if (ToDate == "null")
                        ToDate = null;

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;
                    DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                    if (FromDate == null)
                    {
                        FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }


                    DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                    if (ToDateOBJ == null)
                    {
                        ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpportunitiesChartDataCustom(FromDateOBJ, ToDateOBJ, ownerId, businessUnitId, chartCode, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetOpportunitiesChartData(string code, string ownerId, string businessUnitId, string chartCode)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpportunitiesChartData(code, ownerId, businessUnitId, chartCode, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetActivitiesChartDataCustom(string FromDate, string ToDate, string ownerId, string businessUnitId, string chartCode)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (FromDate == "null")
                        FromDate = null;

                    if (ToDate == "null")
                        ToDate = null;

                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                    if (FromDate == null)
                    {
                        FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }


                    DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                    if (ToDateOBJ == null)
                    {
                        ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    CRMDomainService QuoteDomain = new CRMDomainService();
                    List<ChartingDataClass> myResult = QuoteDomain.GetActivitiesChartDataCustom(FromDateOBJ, ToDateOBJ, ownerId, businessUnitId, chartCode, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetActivitiesChartData(string code, string ownerId, string businessUnitId, string chartCode)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    businessUnitId = this.FixFilter(businessUnitId);
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    CRMDomainService QuoteDomain = new CRMDomainService();
                    List<ChartingDataClass> myResult = QuoteDomain.GetActivitiesChartData(code, ownerId, businessUnitId, chartCode, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetOpenedTicketsGroupByClassification(string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpenedTicketsGroupByClassification(code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetOpenedTicketsGroupBySeverity(string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpenedTicketsGroupBySeverity(code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetOpenedTicketsGroupByOwner(string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpenedTicketsGroupByOwner(code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetOpenedTicketsBySLAViolation(int selectedIndex, string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpenedTicketsBySLAViolation(selectedIndex, code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetOpenedTicketsByOpenedStage(int selectedIndex, string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpenedTicketsByOpenedStage(selectedIndex, code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetClosedTicketsGroupByClassification(string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetClosedTicketsGroupByClassification(code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetClosedTicketsGroupBySeverity(string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetClosedTicketsGroupBySeverity(code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetClosedTicketsGroupByType(string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetClosedTicketsGroupByType(code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetClosedTicketsBySLAViolation(int selectedIndex, string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetClosedTicketsBySLAViolation(selectedIndex, code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetClosedTicketsBySolvedStage(int selectedIndex, string code, string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetClosedTicketsBySolvedStage(selectedIndex, code, ownerId, employeeGroupId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetOpenTicketsGroupByClassification(string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;

                    int tenant = authToken.Tenant;

                    CRMDomainService QuoteDomain = new CRMDomainService();

                    List<ChartingDataClass> myResult = QuoteDomain.GetOpenTicketsGroupByClassification(ownerId, employeeGroupId, tenant, false);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetOpenTicketsByDueTime(string ownerId, string employeeGroupId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ownerId = this.FixFilter(ownerId);
                    employeeGroupId = this.FixFilter(employeeGroupId);
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    CRMDomainService QuoteDomain = new CRMDomainService();
                    List<ChartingDataClass> myResult = QuoteDomain.GetOpenTicketsByDueTime(ownerId, employeeGroupId, tenant, true);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetTicketOverviewPerformance(string ticketId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    CRMDomainService QuoteDomain = new CRMDomainService();
                    List<ChartingDataClass> myResult = QuoteDomain.GetTicketOverviewPerformance(ticketId, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetActivitiesByOpportunityId(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                CRMDomainService domainService = new CRMDomainService();
                List<ActivityList> myResult = domainService.GetActivitiesByOpportunityId(entityId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetActivitiesByTicketId(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                CRMDomainService domainService = new CRMDomainService();
                List<ActivityList> myResult = domainService.GetActivitiesByTicketId(entityId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTicketOwnerPermission(string entityId, bool rightToLeft)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                CRMDomainService domainService = new CRMDomainService();
                domainService.UpdateCorrespondenceInvokeOperation(entityId, authToken.Tenant, rightToLeft);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCommunicationLogs(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                CRMDomainService domainService = new CRMDomainService();
                var results = domainService.GetCommunicationLogs(entityId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //public HttpResponseMessage GetUpdatingActivityMettingSummary(string mettingSummary, bool post, string activityId)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        string loggedUserEmail = authToken.Email;
        //        int tenant = authToken.Tenant;

        //        CRMDomainService crmDomain = new CRMDomainService();
        //        crmDomain.CompleteActivity(activityId, post, mettingSummary, tenant);

        //        return Request.CreateResponse(HttpStatusCode.OK, "");
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}

        public HttpResponseMessage GetOccasionsSummary()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Occasion", "READ", tenant);

                    ICRMContext myContext = CRMContext.GetContext(tenant);
                    OccasionSummary myResult = new OccasionSummary() { Id = tenant };
                    myResult.AllOccasionsCount = (from d in myContext.Occasions where d.Tenant == tenant select d).Count();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
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
        private string GetLoggedUserId(string loggedUserEmail, int tenant)
        {
            string loggedUserId = null;
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContactPM = contactQuery.GetContactByNameAndTenant(loggedUserEmail, tenant, true);
            if (loggedContactPM == null)
            {
                loggedContactPM = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            }

            if (loggedContactPM != null)
            {
                loggedUserId = loggedContactPM.Id;
            }

            return loggedUserId;
        }

        [HttpGet]
        public HttpResponseMessage GetOccasionContactsByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Contact",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Contacts",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                OccasionContactArgs args = this.AnalyzeOccasionFilters(filters);                
                
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                IQueryable<Customer> customers = this.GetFilteredCustomers(args, commonDataContext, tenant);
                IQueryable<CardContact> contacts = this.GetCustomerContacts(customers, commonDataContext, tenant);
                
                if (!string.IsNullOrEmpty(args.OccasionId))
                {
                    List<string> contactsIds = this.GetContactsIdsFromOccasion(args.OccasionId, tenant);
                    if (contactsIds != null)
                    {
                        contacts = contacts.Where(d => contactsIds.Contains(d.ContactId));
                    }
                }
                
                if (!string.IsNullOrEmpty(args.ProductTypes))
                {
                    List<string> myproductsTypesList = this.GetList(args.ProductTypes, commonDataContext, authToken.Tenant);                    
                    if (myproductsTypesList.Count() > 0)
                    {
                        List<CardContactProduct> cardContactProducts = commonDataContext.CardContactProducts.Where(d => myproductsTypesList.Contains(d.ProductTypeCode)).ToList();
                        List<string> cardContactsIds = cardContactProducts.Select(s => s.CardContactId).ToList();
                        contacts = contacts.Where(d => cardContactsIds.Contains(d.Id));
                    }
                }

                if (!string.IsNullOrEmpty(args.AdditionalServices))
                {
                    List<string> myAdditionalServicesList = this.GetList(args.AdditionalServices, commonDataContext, authToken.Tenant);
                    if (myAdditionalServicesList.Count() > 0)
                    {
                        List<CardContactAdditionalService> cardContactAdditionalServices = commonDataContext.CardContactAdditionalServices.Where(d => myAdditionalServicesList.Contains(d.AdditionalServiceId)).ToList();
                        List<string> cardContactsIds = cardContactAdditionalServices.Select(s => s.CardContactId).ToList();
                        contacts = contacts.Where(d => cardContactsIds.Contains(d.Id));
                    }
                }

                List<OccasionContactSearchresult> myResult = new List<OccasionContactSearchresult>();
                if (contacts != null && contacts.Count() > 0)
                {
                    myResult = this.BuildFilteredContacts(contacts, commonDataContext, authToken.Tenant);                    
                }
                
                if (!string.IsNullOrEmpty(args.SearchText))
                {
                    myResult = myResult.Where(f => f.Email != null && f.Email.ToLower().StartsWith(args.SearchText.ToLower())
                            || f.Name != null && f.Name.ToLower().StartsWith(args.SearchText.ToLower())
                            || f.Company != null && f.Company.ToLower().StartsWith(args.SearchText.ToLower())).ToList();
                }

                if (!queryOperations.GetAll)
                {
                    myResult = myResult.Skip(queryOperations.PageIndex).ToList();
                    myResult = myResult.Take(queryOperations.PageSize).ToList();
                }

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    response.Count = myResult.Count;
                }

                response.Result = myResult;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private OccasionContactArgs AnalyzeOccasionFilters(ApiQueryFilters filters)
        {
            OccasionContactArgs args = new OccasionContactArgs();

            List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
            for (int i = 1; i <= 10; i++)
            {
                object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);

                if (filterNameProp != null)
                {
                    string filterName = filterNameProp.ToString();
                    string filterValue = filterValue1 != null ? filterValue1.ToString() : null;

                    switch (filterName)
                    {
                        case "SearchText":
                            {
                                args.SearchText = filterValue;
                                break;
                            }

                        case "CustomerSizeId":
                            {
                                args.CustomerSizeId = filterValue;
                                break;
                            }

                        case "RegionId":
                            {
                                args.RegionId = filterValue;
                                break;
                            }

                        case "IndustryId":
                            {
                                args.IndustryId = filterValue;
                                break;
                            }

                        case "OccasionId":
                            {
                                args.OccasionId = filterValue;
                                break;
                            }

                        case "Products":
                            {
                                args.ProductTypes = filterValue;
                                break;
                            }

                        case "AdditionalServices":
                            {
                                args.AdditionalServices = filterValue;
                                break;
                            }
                    }
                }
            }

            return args;
        }
        private IQueryable<Customer> GetFilteredCustomers(OccasionContactArgs args, ICommonDataContext commonDataContext, int tenant)
        {
            CustomerRepository customerRepository = new CustomerRepository(commonDataContext);
            IQueryable<Customer> customers = customerRepository.GetCustomers(tenant);
            customers = customers.Where(d => d.IsCustomer);

            if (!string.IsNullOrEmpty(args.CustomerSizeId))
            {
                customers = customers.Where(d => d.CustomerSizeId == args.CustomerSizeId);
            }

            if (!string.IsNullOrEmpty(args.RegionId))
            {
                customers = customers.Where(d => d.RegionId == args.RegionId);
            }

            if (!string.IsNullOrEmpty(args.IndustryId))
            {
                customers = customers.Where(d => d.IndustryId == args.IndustryId);
            }

            return customers;
        }
        private IQueryable<CardContact> GetCustomerContacts(IQueryable<Customer> customers, ICommonDataContext commonDataContext, int tenant)
        {
            List<string> customersIds = customers.Select(s => s.Id).ToList();

            CardContactRepository cardContactRepository = new CardContactRepository(commonDataContext);
            IQueryable<CardContact> contacts = cardContactRepository.GetCardsContactsForCustomerIds(customersIds, tenant);

            return contacts;
        }
        private List<string> GetContactsIdsFromOccasion(string occasionId, int tenant)
        {
            ICRMContext cRMContext = CRMContext.GetContext(tenant);
            OccasionRepository occasionRepository = new OccasionRepository(cRMContext);
            OccasionInviteeRepository occasionInviteeRepository = new OccasionInviteeRepository(cRMContext);
            IQueryable<OccasionInvitee> occasionInvitees = occasionInviteeRepository.GetOccasionInviteesByOccasion(occasionId, tenant);
            List<string> contactsIds = occasionInvitees.Select(s => s.ContactId).ToList();
            return contactsIds;
        }
        private List<string> GetList(string myString, ICommonDataContext commonDataContext, int tenant)
        {
            List<string> myList = new List<string>();

            myString = myString.Replace(" ", "");

            if (myString.ToLower() == "all")
            {
            }

            else
            {
                myString = myString.Trim(',');
                string[] mySplitString = myString.Split(',');
                myList = mySplitString.ToList();
            }

            return myList;
        }
        private List<OccasionContactSearchresult> BuildFilteredContacts(IQueryable<CardContact> contacts, ICommonDataContext commonDataContext, int tenant)
        {
            List<OccasionContactSearchresult> myResult = new List<OccasionContactSearchresult>();

            foreach (CardContact cardContact in contacts)
            {
                string regionName = "";
                string industryName = "";
                string customerSizeName = "";
                if (cardContact.Card != null && cardContact.Card.Customer != null)
                {
                    if (!string.IsNullOrEmpty(cardContact.Card.Customer.RegionId))
                    {
                        Region region = commonDataContext.Regions.Where(d => d.Id == cardContact.Card.Customer.RegionId && d.Tenant == tenant).FirstOrDefault();
                        if (region != null)
                        {
                            regionName = region.Name;
                        }
                    }

                    if (!string.IsNullOrEmpty(cardContact.Card.Customer.IndustryId))
                    {
                        Industry industry = commonDataContext.Industries.Where(d => d.Id == cardContact.Card.Customer.IndustryId && d.Tenant == tenant).FirstOrDefault();
                        if (industry != null)
                        {
                            industryName = industry.Name;
                        }
                    }

                    if (!string.IsNullOrEmpty(cardContact.Card.Customer.CustomerSizeId))
                    {
                        CustomerSize customerSize = commonDataContext.CustomerSizes.Where(d => d.Id == cardContact.Card.Customer.CustomerSizeId && d.Tenant == tenant).FirstOrDefault();
                        if (customerSize != null)
                        {
                            customerSizeName = customerSize.Name;
                        }
                    }
                }

                string productsNames = "";
                CardContactProductRepository cardContactProductRepository = new CardContactProductRepository(commonDataContext);
                IQueryable<CardContactProduct> products = cardContactProductRepository.GetProductsByCardContactIdd(cardContact.Id, tenant);
                if (products != null && products.Count() > 0)
                {
                    foreach(CardContactProduct item in products)
                    {
                        if(item.ProductType != null)
                        {
                            if(string.IsNullOrEmpty(productsNames))
                            {
                                productsNames = item.ProductType.Name;
                            }

                            else
                            {
                                productsNames = productsNames + ", " + item.ProductType.Name;
                            }
                        }
                    }
                }

                myResult.Add(new OccasionContactSearchresult()
                {
                    ContactId = cardContact.ContactId,
                    Name = cardContact.Contact == null ? null : cardContact.Contact.EnglishName,
                    Email = cardContact.Contact == null ? null : cardContact.Contact.Email,
                    Company = cardContact.Card == null ? null : cardContact.Card.EnglishName,
                    Region = regionName,
                    Industry = industryName,
                    Product = productsNames,
                    CustomerSize = customerSizeName,

                    ContactMobile = cardContact.Contact == null ? null : cardContact.Contact.Mobile,
                    ContactPhone = cardContact.Contact == null ? null : cardContact.Contact.BusinessPhone,
                    ContactPosition = cardContact.Contact == null ? null : cardContact.Contact.Position,
                    ContactTel = cardContact.Contact == null ? null : cardContact.Contact.BusinessPhone,


                });
            }

            return myResult;
        }


        public HttpResponseMessage GetCountOfOccasionAllCustomers(String contactIds)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    var myResult = 0;
                    if (!string.IsNullOrEmpty(contactIds))
                    {
                        List<string> contactIds_Invited = contactIds.TrimEnd(',').Split(',').ToList();
                        CardContactRepository cardContactRepository = new CardContactRepository(tenant);
                        myResult = cardContactRepository.GetCardsContactsForContactIds_Count(contactIds_Invited, tenant);
                    }
 
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSupportMailboxsByTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                SupportMailboxQueryService queryService = new SupportMailboxQueryService(tenant);
                List<SupportMailboxPM> myList = queryService.GetSupportMailboxsByTenant(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeleteMailBox(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICRMContext objectContext = CRMContext.GetContext(tenant);
                SupportMailboxRepository repository = new SupportMailboxRepository(objectContext);
                SupportMailbox mailbox = repository.GetSingle(entityId, tenant);

                if(mailbox != null)
                {
                    repository.Remove(mailbox);
                    repository.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class MeetingSummary
    {
        public string ActivityId { get; set; }
        public string Summary { get; set; }
        public bool Post { get; set; }
    }

    public class OccasionSummary
    {
        public int Id { get; set; }
        public int AllOccasionsCount { get; set; }
    }

    public class OccasionContactArgs
    {
        public string CustomerSizeId { get; set; }
        public string RegionId { get; set; }
        public string IndustryId { get; set; }
        public string OccasionId { get; set; }
        public string ProductTypes { get; set; }
        public string AdditionalServices { get; set; }
        public string SearchText { get; set; }
    }

    public class OccasionContactSearchresult
    {
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Region { get; set; }
        public string Industry { get; set; }
        public string Product { get; set; }
        public string CustomerSize { get; set; }
        public string ContactPhone { get; set; }
        public string ContactPosition { get; set; }
        public string ContactMobile { get; set; }
        public string ContactTel { get; set; }
    }
}