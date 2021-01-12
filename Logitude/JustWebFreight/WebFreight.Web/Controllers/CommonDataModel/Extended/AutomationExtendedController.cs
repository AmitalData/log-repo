using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class AutomationExtendedController : ApiController
    {
     

        public HttpResponseMessage GetAutomationesByObjectTableId(string objectTableId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Automation", "READ", authToken.Tenant);

                AutomationQuery automationQuery = new AutomationQuery(tenant);
                List<AutomationPM> myResult = automationQuery.GetAutomationPMsByObjectTableId(objectTableId, authToken.Tenant, true);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetAutomationBackupDataById(string automationId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Automation", "READ", authToken.Tenant);

                AutomationQuery automationQuery = new AutomationQuery(authToken.Tenant);
                string automationxmal = automationQuery.GetAutomationXmalById(automationId, authToken.Tenant);
                AutomatedBackup automatedDataBackup = new AutomatedBackup();
                if (!string.IsNullOrEmpty(automationxmal))
                {
                    automatedDataBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automationxmal);
                }

                return Request.CreateResponse(HttpStatusCode.OK, automatedDataBackup);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Post(AutomationPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("Automation", "NEW", authToken.Tenant);

                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        AutomationService service = new AutomationService(MyContext, entityPM.Tenant);

                        SaveAutomationXmal(entityPM);

                        service.Create(entityPM);
                        entityPM.AutomationXML = null;
                        entityPM.AutomationResultEmailRecipientLists = null;

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage Put(AutomationPM entityPM)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Automation", "UPDATE", authToken.Tenant);

                    string entityName = "Automation" + entityPM.Id + entityPM.Tenant;
                    string entityPmName = "AutomationPM" + entityPM.Id + entityPM.Tenant;
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    AutomationService service = new AutomationService(MyContext, entityPM.Tenant);

                    SaveAutomationXmal(entityPM);
              
                    service.Update(entityPM);

                    entityPM.AutomationXML = null;
                    entityPM.AutomationResultEmailRecipientLists = null;
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void SaveAutomationXmal(AutomationPM entityPM)
        {
            if (entityPM.AutomatedDataBackup != null)
            {
                System.Type type1 = typeof(AutomationCondition);
                System.Type type2 = typeof(AutomationSetValue);
                System.Type type3 = "string".GetType();
                System.Type type4 = typeof(AutomationFollowUp);
                System.Type type5 = typeof(FollowUpDocumentTypeList);
                System.Type type6 = typeof(AutomationSetSLAValue);
                System.Type type7 = typeof(AutomationQueuedTask);
                System.Type type8 = typeof(AutomationSendInterface);
                System.Type type9 = typeof(FTPAutomationDetails);
                System.Type type10 = typeof(AutomationSendDocument);





                System.Type[] types = new System.Type[10];
                types[0] = type1;
                types[1] = type2;
                types[2] = type3;
                types[3] = type4;
                types[4] = type5;
                types[5] = type6;
                types[6] = type7;
                types[7] = type8;

                types[8] = type9;
                types[9] = type10;
                entityPM.AutomationXML = LogitudeXmlSerializer.SerializeObjectToElementString(entityPM.AutomatedDataBackup, types);
            }
        }

       

        public HttpResponseMessage PutAuomationList(List<AutomationArgs> items)
        {

            try
            {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Automation", "UPDATE", authToken.Tenant);
                    AutomationRepository entityRepository = new AutomationRepository(authToken.Tenant);

                    foreach (AutomationArgs entityPM in items)
                    {
                        string entityName = "Automation" + entityPM.Id + entityPM.Tenant;
                        string entityPmName = "AutomationPM" + entityPM.Id + entityPM.Tenant;
                        if (CacheManager.CacheWrapper.Get(entityName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityName);
                        }
                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityPmName);
                        }


                        var Poco = entityRepository.GetSingleAutomation(entityPM.Id, authToken.Tenant);
                        if (Poco != null)
                        {
                            Poco.Order = entityPM.Order;
                            entityRepository.Update(Poco);
                        }

                    }

                    entityRepository.SubmitChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "");
               
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


    
        public HttpResponseMessage PutAutomationesTraceEvent(EventTypeArgs eventTypeArgs)
        {
            try
            {
                EventTypeRepository eventTypesRepository = new EventTypeRepository(eventTypeArgs.Tenant);
                EventTypeQuery eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                List<EventTypeList> eventList = eventTypeQuery.GetEventTypeIdsByListEventCode(eventTypeArgs.EventTypeCodeList, eventTypeArgs.Tenant, eventTypeArgs.ObjectTableId);

                TraceEventRepository traceEventRepository = new TraceEventRepository(eventTypeArgs.Tenant);

                if (eventList!=null &&  eventList.Count > 0)
                {
                    foreach (EventTypeList eventTypeList in eventList)
                    {
                        TraceEvent newEvent = new TraceEvent()
                        {
                            Id = IdCounter.GetNumber("EventType", eventTypeArgs.Tenant).ToString(),
                            LogDateTime = DateTime.Now,
                            EventDateTime = DateTime.Now,
                            Tenant = eventTypeArgs.Tenant,
                            UserId = eventTypeArgs.LoggedContactId,
                            ObjectTableId = eventTypeArgs.ObjectTableId,
                            EntityId = eventTypeArgs.EntityId,
                            IsAddedManually = false,
                            EventTypeId = eventTypeList.Id,
                        };

                        traceEventRepository.Add(newEvent);
                    }

                    traceEventRepository.SubmitChanges();

                }
             
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDoesAutomationCodeExist(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AutomationRepository automationRepository = new AutomationRepository(authToken.Tenant);
                bool a = (automationRepository.GetAutomations(authToken.Tenant).Where(d => d.Code == code && d.Tenant == authToken.Tenant)).Any();
                return Request.CreateResponse(HttpStatusCode.OK, a);
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
            SecurityUtility.CheckContactFeature("Automation", "READ", authToken.Tenant);
        }



    }
}