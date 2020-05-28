using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class CustomsRequierdFieldsController : ApiController
    {
        //from general domain service:
        public HttpResponseMessage GetSomeObjectTables()
        {
            try
            { 
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ObjectTableRepository ObjectTableRepository = new ObjectTableRepository(0);
                ObjectTableQuery objectTableQuery = new ObjectTableQuery(ObjectTableRepository);
                List<ObjectTablePM> result = objectTableQuery.GetSomeObjectTables(0);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomsRequiredFieldListsByObjectTable(string objectTableId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsRequiredFieldQueryService customsRequiredFieldQuery = new CustomsRequiredFieldQueryService(customContext);
                List<CustomsRequiredFieldPM> result = customsRequiredFieldQuery.GetCustomRequiredFieldsByObjectTable(objectTableId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostRequiredFields([FromBody] List<RequierdFieldObject> fields)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            string loggedUserEmail = authToken.Email;

            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);

                foreach (RequierdFieldObject item in fields)
                {

                    CustomsRequiredFieldRepository rep = new CustomsRequiredFieldRepository(customContext);
                    CustomsRequiredField requiredField = rep.GetCustomRequiredFieldsByObjectFieldCode(item.ObjectfieldCode, tenant);

                    //CustomsRequiredFieldQueryService query = new CustomsRequiredFieldQueryService(customContext);
                    //CustomsRequiredFieldPM reqField = query.GetCustomRequiredFieldsByObjectFieldId(item.ObjectfieldId);
                    if (requiredField == null)
                    {
                        if (item.Active == true)
                        {
                            // create req field in DB
                            CustomsRequiredFieldUpdateService service = new CustomsRequiredFieldUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                            CustomsRequiredFieldPM field = new CustomsRequiredFieldPM()
                            {
                                ObjectfieldId = item.ObjectfieldId,
                                ObjectfieldCode = item.ObjectfieldCode,
                                ObjectTableId = item.ObjectTableId,
                                Tenant = tenant,
                                ObjectFieldName = item.ObjectFieldName,
                            };
                            field.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                            service.Update(field, true);
                        }
                    }
                    else
                    {
                        if (item.Active == false)
                        {
                            CustomsRequiredFieldQueryService query = new CustomsRequiredFieldQueryService(customContext);
                            CustomsRequiredFieldPM reqField = query.GetCustomRequiredFieldsByObjectFieldCode(item.ObjectfieldCode, tenant);
                            // delete requierd field from DB
                            CustomsRequiredFieldUpdateService service = new CustomsRequiredFieldUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                            reqField.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            service.Update(reqField, true);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        
    }

    public class RequierdFieldObject
    {
        public string ObjectfieldId { get; set; }
        public string ObjectfieldCode { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectFieldName { get; set; }
        public bool Active { get; set; }
    }
}