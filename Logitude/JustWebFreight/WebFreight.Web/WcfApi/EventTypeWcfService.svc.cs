using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.Security;
using Logitude.BL.Validators;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EventTypeWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EventTypeWcfService.svc or EventTypeWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EventTypeWcfService : IEventTypeWcfService
    { 
        public Response Upsert(EventTypePM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
          
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    
                    //ICommonDataContext commoncontext = CommonDataContext.GetContext(entityPM.Tenant);
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);

                    ObjectTableRepository objecttableRepository = new ObjectTableRepository(webFreightContext);
                    EntityStatusRepository entityStatusRepository = new EntityStatusRepository(webFreightContext);
                    EventTypeRepository eventTypeRepository = new EventTypeRepository(webFreightContext);

                    EventTypeService service = new EventTypeService(webFreightContext, entityPM.Tenant);
                    ObjectTable objectTable = null;
                    if (!string.IsNullOrEmpty(entityPM.ObjectTableName))
                    {
                        //entityPM.ObjectTableId = objecttableRepository.GetObjectTableByName(entityPM.ObjectTableName, entityPM.Tenant, false).Id;
                        objectTable = objecttableRepository.GetObjectTableByName(entityPM.ObjectTableName, 0, false);
                        if (objectTable != null)
                        {
                            entityPM.ObjectTableId = objectTable.Id;
                        }

                        if (String.IsNullOrWhiteSpace(entityPM.ObjectTableId))
                        {
                            throw new Exception("entityPM.ObjectTableName ( " + entityPM.ObjectTableName + " ) not exist in tenant 0 ");
                        }
                    }

                    if (entityPM.EntityStatusId != null)
                    {
                        //entityPM.ObjectTableId = objecttableRepository.GetObjectTableByName(entityPM.ObjectTableName, entityPM.Tenant, false).Id;
                        EntityStatus entitystatus = entityStatusRepository.GetSingleEntityStatusByCode(entityPM.Code, entityPM.Tenant);
                        if (entitystatus != null)
                        {
                            entityPM.EntityStatusId = entitystatus.Id;
                        }
                        if (String.IsNullOrWhiteSpace(entityPM.EntityStatusId))
                        {
                            throw new Exception("entityPM.EntityStatusId ( " + entityPM.EntityStatusId + " ) not exist");
                        }
                    }

                    ClassLevelValidator validationClass = new ClassLevelValidator("EventType", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }


                    entityPM.IsHybrid = true;
                    EventType entity = eventTypeRepository.GetSingleEventTypeByCodeAndObjectTableId(entityPM.Code, objectTable.Id, entityPM.Tenant);
                    if (entity == null)
                    {
                        service.Create(entityPM);
                    }
                    else
                    {
                        entityPM.Id = entity.Id;
                        service.Update(entityPM);
                    }

                    response.Result = entityPM.Id;
                    scope.Complete();
                    return response;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }
    }
}
