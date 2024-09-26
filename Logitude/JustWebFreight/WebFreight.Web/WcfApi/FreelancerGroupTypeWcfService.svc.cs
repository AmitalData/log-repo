using Logitude.BL.Validators;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Activation;
using System.Web.Http;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.WcfApi
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FreelancerGroupTypeWcfService : IFreelancerGroupTypeWcfService
    {
        public Response Upsert(FreelancerGroupTypePM entityPM, bool batch)
        {
            Response response = new Response();
            try
            {
                ClassLevelValidator validationClass = new ClassLevelValidator("FreelancerGroupType", entityPM.Tenant);
                if (!validationClass.IsValid(entityPM, entityPM, null))
                {
                    response.HasError = true;
                    response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                    return response;
                }
                var freelancerGroupTypeRepo = new FreelancerGroupTypeRepository(entityPM.Tenant);
                ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                if (string.IsNullOrEmpty(entityPM.Code))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Code field is required";
                    return response;
                }
                if (entityPM.Code != null)
                {
                    var freelancerGroupType = freelancerGroupTypeRepo.GetSingleFreelancerGroupType(entityPM.Code, entityPM.Tenant);
                    var service = new FreelancerGroupTypeService(objectContext, entityPM.Tenant);

                    if (freelancerGroupType == null)
                    {
                        service.Create(entityPM);
                        var entity = freelancerGroupTypeRepo.GetSingleFreelancerGroupType(entityPM.Code, entityPM.Tenant);
                        entityPM.Id = entity?.Id;
                    }
                    else
                    {
                        service.Update(entityPM);
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