using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class AWBStackDomainController : ApiController
    {
        public HttpResponseMessage GetMAWBStackPMsByAirlineId(string myCardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                MAWBStackQuery entityQuery = new MAWBStackQuery(tenant);
                IQueryable<MAWBStackPM> myResult = entityQuery.GetMAWBStackPMsByAirlineId(myCardId, tenant).AsQueryable().OrderBy(a => a.InsertionDate);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMAWBStackPMByNumber(int number)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                MAWBStackQuery entityQuery = new MAWBStackQuery(tenant);
                MAWBStackPM myResult = entityQuery.GetSingleMAWBStackPMByNumber(number, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMAWBStackPMsCountByAirlineIdAndShipperId(string airlineId, string customerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                MAWBStackQuery mawbStackQuery = new MAWBStackQuery(tenant);
                List<MAWBStackPM> query = mawbStackQuery.GetMAWBStackPMsByAirlineIdAndCustomerId(airlineId, customerId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, query.Count());
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCardHasAssignedMawbStacks(string airlineId, string customerId)
        {
            try
            {
                if (customerId == "null")
                {
                    customerId = null;
                }

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                MAWBStackQuery mawbStackQuery = new MAWBStackQuery(tenant);
                bool myResult = mawbStackQuery.HasMAWBStackPMsForAirlineIdAndCustomerId(airlineId, customerId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMAWBStackPMsByAirlineIdAndShipperId(string airlineId, string customerId, int pageSize, int pageIndex)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                MAWBStackQuery mawbStackQuery = new MAWBStackQuery(tenant);
                int skippedStacks = pageSize * (pageIndex - 1);

                IQueryable<MAWBStackPM> myResult = mawbStackQuery.GetMAWBStackPMsByAirlineIdAndCustomerId(airlineId, customerId, tenant).AsQueryable().OrderBy(a => a.InsertionDate);
                myResult = myResult.Skip(skippedStacks);
                myResult = myResult.Take(pageSize);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomerStockSeries(string myCustomerId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    MawbStockService webService = new MawbStockService();

                    byte[] myResultBytes = webService.GetCustomerStockSeries(myCustomerId, tenant);
                    XmlSerializer serializer = new XmlSerializer(typeof(StockSeriesListClass));
                    MemoryStream memstream = new MemoryStream(myResultBytes);
                    StockSeriesListClass myResult = (StockSeriesListClass)serializer.Deserialize(memstream);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllAvailableStockSeries()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    MawbStockService webService = new MawbStockService();

                    byte[] myResultBytes = webService.GetAllAvailableStockSeries(tenant);
                    XmlSerializer serializer = new XmlSerializer(typeof(StockSeriesListClass));
                    MemoryStream memstream = new MemoryStream(myResultBytes);
                    StockSeriesListClass myResult = (StockSeriesListClass)serializer.Deserialize(memstream);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAirlineAvailableStockSeries(string airlineId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    MawbStockService webService = new MawbStockService();

                    byte[] myResultBytes = webService.GetAirlineAvailableStockSeries(airlineId, tenant);
                    XmlSerializer serializer = new XmlSerializer(typeof(StockSeriesListClass));
                    MemoryStream memstream = new MemoryStream(myResultBytes);
                    StockSeriesListClass myResult = (StockSeriesListClass)serializer.Deserialize(memstream);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAssignStockSeriesToCustomer(int start, int end, string airlineId, string customerId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    MawbStockService webService = new MawbStockService();

                    webService.AssignStockSeriesToCustomer(start, end, airlineId, customerId, tenant);

                    bool myResult = true;

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUnAssignStockSeriesToUser(int start, int end, string airlineId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    MawbStockService webService = new MawbStockService();

                    webService.UnAssignStockSeriesToUser(start, end, airlineId, tenant);

                    bool myResult = true;

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDeleteMAWBStacksOperation(string myStackId, string myAirlineId, bool isDeletingSeries)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                string loggedUserId = null;
                ContactRepository contactsRepository = new ContactRepository(objectContext);
                ContactQuery contactQuery = new ContactQuery(contactsRepository);
                ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                if (contact != null)
                {
                    loggedUserId = contact.Id;
                }

                MAWBStackService service = new MAWBStackService(objectContext, tenant);
                service.DeleteMAWBStacksOperation(myStackId, myAirlineId, isDeletingSeries, loggedUserId);

                bool myResult = true;

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCreateMAWBStacksOperation(string myAirlineId, int myStartNumber, int myEndNumber, string assignedToId)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "NEW", tenant);

                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                string loggedUserId = null;
                ContactRepository contactsRepository = new ContactRepository(objectContext);
                ContactQuery contactQuery = new ContactQuery(contactsRepository);
                ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                if (contact != null)
                {
                    loggedUserId = contact.Id;
                }

                if (assignedToId == "null")
                {
                    assignedToId = null;
                }

                MAWBStackService service = new MAWBStackService(objectContext, tenant);
                service.CreateMAWBStacks(myAirlineId, myStartNumber, myEndNumber, assignedToId, loggedUserId);

                bool myResult = true;

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}