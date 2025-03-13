using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
    public class DefaultValueWcfService : IDefaultValueWcfService
    {

        public Response Upsert(DefaultValuePM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("DefaultValue", "UPDATE", entityPM.Tenant);//UPDATE//READ


                ClassLevelValidator validationClass = new ClassLevelValidator("DefaultValue", entityPM.Tenant);
                if (!validationClass.IsValid(entityPM, entityPM, null))
                {
                    response.HasError = true;
                    response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                    return response;
                }



                if (string.IsNullOrEmpty(entityPM.DefaultTypeId))
                {
                    response.HasError = true;
                    response.ErrorMessage = "DefaultTypeId field is required";
                    return response;
                }
                ICustomContext customContext = CustomContext.GetContext(entityPM.Tenant);

                DefaultValueRepository defaultValueRepository = new DefaultValueRepository(entityPM.Tenant);

                DefaultValueUpdateService defaultValueUpdateService = new DefaultValueUpdateService(customContext, new Dictionary<string, IContext>(), entityPM.Tenant);



                if (entityPM.DefaultTypeId != null)
                {
                    var existingDefaultValue = defaultValueRepository.GetSingleByDefaultTypeIdAndCardId(entityPM.DefaultTypeId, entityPM.Tenant, entityPM.CardId);

                    if (existingDefaultValue == null)
                    {
                        entityPM.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                    else
                    {
                        entityPM.ChangeSetOp = ChangeSetOperation.Update;
                        entityPM.Id = existingDefaultValue.Id;
                    }
                    entityPM.BranchId = entityPM.BranchId == "NON" ? null : entityPM.BranchId;
                    entityPM.CardId = entityPM.CardId == "NON" ? null : entityPM.CardId;

                    defaultValueUpdateService.Update(entityPM, true);
                    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                        var insertedEntity = defaultValueRepository.GetSingleByDefaultTypeIdAndCardId(entityPM.DefaultTypeId, entityPM.Tenant, entityPM.CardId);
                        entityPM.Id = insertedEntity?.Id;
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
