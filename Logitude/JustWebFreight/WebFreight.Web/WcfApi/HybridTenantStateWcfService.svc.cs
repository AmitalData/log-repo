using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HybridTenantStateWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select HybridTenantStateWcfService.svc or HybridTenantStateWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HybridTenantStateWcfService : IHybridTenantStateWcfService
    {
        public Response Upsert(HybridTenantStatePM entitypm, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ICommonDataContext objectContext = CommonDataContext.GetContext(entitypm.Tenant);

                    HybridTenantStateRepository hybridTenantStateRepository = new HybridTenantStateRepository(objectContext);
                    HybridTenantState entity = hybridTenantStateRepository.GetSingleHybridTenantState(entitypm.Tenant);
                    if (entity != null)
                    {
                        entity.WaitingQueue = entitypm.WaitingQueue;
                        entity.FailedQueue = entitypm.FailedQueue;
                        entity.LastUpdateDateTime = DateTime.UtcNow;
                        entity.LastQueueDateTime = entitypm.LastQueueDateTime;
                        hybridTenantStateRepository.Update(entity);
                        hybridTenantStateRepository.SubmitChanges();
                    }
                    else
                    {
                        entity = new HybridTenantState() { Tenant = entitypm.Tenant, FailedQueue = entitypm.FailedQueue, WaitingQueue = entitypm.WaitingQueue, LastUpdateDateTime = DateTime.UtcNow };
                        hybridTenantStateRepository.Add(entity);
                        hybridTenantStateRepository.SubmitChanges();
                    }



                    response.Result = entitypm.Tenant.ToString();
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
