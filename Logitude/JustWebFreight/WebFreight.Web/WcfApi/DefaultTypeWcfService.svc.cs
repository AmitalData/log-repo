using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Logitude.Customs.Data;

namespace WebFreight.Web.WcfApi
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DefaultTypeWcfService : IDefaultTypeWcfService
    {

        public Response Upsert(DefaultTypePM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("DefaultType", "UPDATE", entityPM.Tenant);//UPDATE//READ
                

                    ClassLevelValidator validationClass = new ClassLevelValidator("DefaultType", entityPM.Tenant);
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    DefaultTypeRepository defaultTypeRepository = new DefaultTypeRepository(entityPM.Tenant);

                    ICustomContext objectContext = CustomContext.GetContext(entityPM.Tenant);


                    DefaultTypeUpdateService defaultTypeUpdateService = new DefaultTypeUpdateService(objectContext, new Dictionary<string, IContext>(), entityPM.Tenant);


                    if (string.IsNullOrEmpty(entityPM.Code))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Code field is required";
                        return response;
                    }


                if (entityPM.Code != null)
                {
                    DefaultType defaultType = defaultTypeRepository.GetSingleByCode(entityPM.Code, entityPM.Tenant);

                    if (defaultType == null)
                    {
                        entityPM.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                    else
                    {
                        entityPM.ChangeSetOp = ChangeSetOperation.Update;
                        entityPM.Id = defaultType.Id;
                    }

                    defaultTypeUpdateService.Update(entityPM, true);
                    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                        DefaultType entity = defaultTypeRepository.GetSingleByCode(entityPM.Code, entityPM.Tenant);
                        entityPM.Id = entity?.Id;
                    }
                       
                    response.Result = entityPM.Id;

                   
                    }
                 return response;
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
