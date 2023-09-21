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

namespace WebFreight.Web.WcfApi
{
    public class StatusCodeWcfService : IStatusCodeWcfService
    {
        public Response Upsert(StatusCodePM entityPM, bool batch)
        {
            Response response = new Response();
            try
            {
                ClassLevelValidator validationClass = new ClassLevelValidator("StatusCode", entityPM.Tenant);
                if (!validationClass.IsValid(entityPM, entityPM, null))
                {
                    response.HasError = true;
                    response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                    return response;
                }
                StatusCodeRepository statusCodeRepository = new StatusCodeRepository(entityPM.Tenant);
                ICustomContext objectContext = CustomContext.GetContext(entityPM.Tenant);
                StatusCodeUpdateService statusCodeUpdateService = new StatusCodeUpdateService(objectContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                if (string.IsNullOrEmpty(entityPM.Status_Code))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Code field is required";
                    return response;
                }
                if (entityPM.Status_Code != null)
                {
                    StatusCode statusCode = statusCodeRepository.GetSingleByCode(entityPM.Status_Code, entityPM.Tenant);

                    if (statusCode == null)
                    {
                        entityPM.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                    else
                    {
                        entityPM.ChangeSetOp = ChangeSetOperation.Update;
                        entityPM.Id = statusCode.Id;
                    }

                    statusCodeUpdateService.Update(entityPM, true);
                    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                        StatusCode entity = statusCodeRepository.GetSingleByCode(entityPM.Status_Code, entityPM.Tenant);
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