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
using Logitude.BL.Validators;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityMapping;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace WebFreight.Web.WcfApi
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BusinessHourWcfService : IBusinessHourWcfService
    {

        public Response Upsert(BusinessHourPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("BusinessHour", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("BusinessHour", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);

                    BusinessHourRepository businessHourRepository = new BusinessHourRepository(objectContext);
                    BusinessHourService service = new BusinessHourService(objectContext, entityPM.Tenant);
                    BusinessHour businessHour = businessHourRepository.GetBusinessHourByCode(entityPM.Code, entityPM.Tenant);
                    if(businessHour==null)
                        service.Create(entityPM);
                    else
                    {
                        entityPM.Id = businessHour.Id;

                        if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
                        {
                            service.Delete(entityPM);
                        }
                        service.Update(entityPM);
                    }
                  
                     businessHour = businessHourRepository.GetBusinessHourByCode(entityPM.Code, entityPM.Tenant);

                    response.Result = businessHour?.Id;
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
