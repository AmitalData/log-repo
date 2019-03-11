using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
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
                Authentication();

              AutomationQuery automationQuery = new AutomationQuery(tenant);
              List<AutomationPM> myResult = automationQuery.GetAutomationPMsByObjectTableId(objectTableId,tenant, true);

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
                Authentication();

                AutomationQuery automationQuery = new AutomationQuery(tenant);
                string automationxmal = automationQuery.GetAutomationXmalById(automationId, tenant);
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

                        EntityChangeAutomationHelper entityChangeAutomationHelper = new EntityChangeAutomationHelper();
                        entityChangeAutomationHelper.AutomationLastUpdate(entityPM.ObjectTableId, entityPM.Tenant);


                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        AutomationService service = new AutomationService(MyContext, entityPM.Tenant);
                
                        service.Create(entityPM);
         
                     
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


                    EntityChangeAutomationHelper entityChangeAutomationHelper = new EntityChangeAutomationHelper();
                    entityChangeAutomationHelper.AutomationLastUpdate(entityPM.ObjectTableId, entityPM.Tenant);

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

                    if (entityPM.IsChangeAutomationXaml && entityPM.AutomatedDataBackup != null)
                    {
                        System.Type type1 = typeof(AutomationCondition);
                        System.Type type2 = typeof(AutomationSetValue);
                        System.Type type3 = "string".GetType();
                        System.Type type4 = typeof(AutomationFollowUp);
                        System.Type type5 = typeof(FollowUpDocumentTypeList);
                        System.Type type6 = typeof(AutomationSetSLAValue);
                        System.Type type7 = typeof(AutomationQueuedTask);

                        System.Type[] types = new System.Type[7];
                        types[0] = type1;
                        types[1] = type2;
                        types[2] = type3;
                        types[3] = type4;
                        types[4] = type5;
                        types[5] = type6;
                        types[6] = type7;

                        entityPM.AutomationXML = LogitudeXmlSerializer.SerializeObjectToElementString(entityPM.AutomatedDataBackup, types);

                        AutomationHistoryService automationHistoryService = new AutomationHistoryService(MyContext, entityPM.Tenant);

                        AutomationHistoryPM automationHistoryPM = new AutomationHistoryPM()
                        {
                            AutomationsId = entityPM.Id,
                            Version = entityPM.Version,
                            Tenant = entityPM.Tenant,
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                            AutomationXML = entityPM.AutomationXML,
                        };

                        automationHistoryService.Create(automationHistoryPM);
                    }

                    service.Update(entityPM);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
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


      
        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("Automation", "READ", authToken.Tenant);
        }



    }
}