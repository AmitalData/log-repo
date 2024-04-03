using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.ExternalAPIs.V1
{
    public class JournalController : ApiController
    {
        public HttpResponseMessage GetSingleJournal(string id, string number, string externalNo, string externalSystem)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Journal", authToken.Tenant);

                JournalQueryService Service = new JournalQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                Journal Result = new Journal();

                if (id != null)
                {
                    Result = Service.GetJournalById(id, tenant);
                }
                if(number != null)
                {

                    Result = Service.GetJournalByNumber(number, tenant);
                }
               if((externalNo != null && externalSystem==null) || externalNo== null && externalSystem != null)
                {
                    throw new Exception("Both ExternalEntityCode And ExternalEnittyReference are required");
                }
                if(externalNo != null && externalSystem != null)
                {
                    Result = Service.GetSingleJournalByExternalNoAndExternalSystem(externalNo, externalSystem, tenant);
                }
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(Logitude.Accounting.BL.APIDataContract.ApiV1.Journal entity)
        {
            Logitude.Accounting.BL.APIDataContract.ApiV1.Journal oldEntity = entity;

            if (ModelState.IsValid)
            {
                string CommunicationId = "0";
                try
                {
                    CommunicationId = APIHelper.AddCommunicationLog(oldEntity, entity, "Journal", null, "Journal API", entity.Tenant);
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        entity.Tenant = authToken.Tenant;
						SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        SecurityUtility.AuthenticateAccessibleAPI("Journal", authToken.Tenant);

                        if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<Logitude.Accounting.BL.APIDataContract.ApiV1.Journal>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IAccountingContext MyContext = AccountingContext.GetContext(entity.Tenant);
                        JournalQueryService mappingService = new JournalQueryService(entity.Tenant);
                        entity = mappingService.SetJournalSystemUser(entity);
                        JournalPM entityPM = mappingService.JournalDataMappingAndValidatin(entity, entity.Tenant);

                       // entityPM.VoidedByJournalId = mappingService.SetVoidedByJournal(entity, entity.Tenant);
                        entityPM.OriginalJournalId = mappingService.SetOriginalJournal(entity, entity.Tenant);
                      
                        entityPM.Tenant = authToken.Tenant;


                        

                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        JournalUpdateService service = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        service.Update(entityPM, true);

                        entity = mappingService.JournalDataMappingAndValidatin(entityPM, entity.Tenant);
                        APIHelper.UpdateCommunicationLog(CommunicationId,"D", oldEntity, entity, "Journal", entityPM.Id, "Journal API", entity.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.UpdateCommunicationLog(CommunicationId,"F", oldEntity, apiExceptionResult.Exception, "Journal", null, "Journal API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "Journal", null, "Journal API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(Logitude.Accounting.BL.APIDataContract.ApiV1.Journal entity)
        {
            Logitude.Accounting.BL.APIDataContract.ApiV1.Journal oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {

                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;
						SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        SecurityUtility.AuthenticateAccessibleAPI("Journal", authToken.Tenant);

                        if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<Logitude.Accounting.BL.APIDataContract.ApiV1.Journal>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                        JournalQueryService mappingService = new JournalQueryService(tenant);
                        entity = mappingService.SetJournalSystemUser(entity);
                        JournalPM entityPM = mappingService.JournalDataMappingAndValidatin(entity, tenant);
                       // entityPM.VoidedByJournalId = mappingService.SetVoidedByJournal(entity, entity.Tenant);
                        entityPM.OriginalJournalId = mappingService.SetOriginalJournal(entity, entity.Tenant);


                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        JournalUpdateService service = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        service.Update(entityPM, true);

                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "Journal", entityPM.Id, "Journal API", authToken.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "Journal", null, "Journal API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "Journal", null, "Journal API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}