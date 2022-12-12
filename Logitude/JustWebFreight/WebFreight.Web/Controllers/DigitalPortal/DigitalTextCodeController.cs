using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalTextCodeController : ApiController
    {
        [HttpGet]
        [Route("DigitalTextCode/GetTextCodesByFilters")]
        public HttpResponseMessage GetTextCodesByFilters(string cardId, string objectTableId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
                var textCodeQuery = new DigitalTextCodeQueryService(tenant);

                var defaultTextCode = textCodeQuery.GetDigitalTextCodesQuery(0, objectTableId);

                var defaultCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(defaultTextCode.Labels);

                var customCodesObject = new List<DigitalTextCodeObject>();

                if (tenant != 0)
                {
                    var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId);

                    if (customTextCodes != null)
                    {
                        customCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);
                    }
                }

                foreach (var item in customCodesObject)
                {
                    var temp = defaultCodesObject.FirstOrDefault(a => a.Code.Equals(item.Code));

                    if (temp != null)
                    {
                        temp.DisplayLable = item.DisplayText;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, defaultCodesObject);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalTextCode/UpdateTextCodes")]
        public HttpResponseMessage UpdateTextCodes(DigitalTextCodeUpdateModel digitalTextCodeUpdateModel)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                digitalTextCodeUpdateModel.Tenant = tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, digitalTextCodeUpdateModel.CardId);
                var textCodeQuery = new DigitalTextCodeQueryService(tenant);
                var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(digitalTextCodeUpdateModel.Tenant, digitalTextCodeUpdateModel.ObjectTableId);

                if (customTextCodes != null)
                {
                    var customCodesMappedObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);

                    foreach (var item in digitalTextCodeUpdateModel.Lables)
                    {
                        var existingKey = customCodesMappedObject.FirstOrDefault(a => a.Code.Equals(item.Code));

                        if (existingKey != null)
                        {
                            existingKey.DisplayText = item.DisplayText;
                        }
                        else
                        {
                            customCodesMappedObject.Add(item);
                        }
                    }

                    customTextCodes.Labels = JsonConvert.SerializeObject(customCodesMappedObject);
                    customTextCodes.UpdateDate = DateTime.UtcNow;
                }
                else
                {
                    customTextCodes = new DigitalTextCodeList
                    {
                        ObjectTableId = digitalTextCodeUpdateModel.ObjectTableId,
                        Tenant = digitalTextCodeUpdateModel.Tenant,
                        Labels = JsonConvert.SerializeObject(digitalTextCodeUpdateModel.Lables),
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };
                }

                textCodeQuery.UpdateDigitalTextCodes(customTextCodes);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        [HttpGet]
        [Route("DigitalTextCode/GetTranslationCodes")]
        public HttpResponseMessage GetTranslationCodes(int tenant, string objectTableId)
        {
            try
            {
                var textCodeQuery = new DigitalTextCodeQueryService(tenant);
                var defaultTextCodes = textCodeQuery.GetDigitalTextCodesQuery(0, objectTableId);
                var textCodes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId);
                var defaultCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(defaultTextCodes.Labels);

                var defaultCodesObjectDictionary = defaultCodesObject.ToDictionary(a => a.Code, x => x.DisplayText);

                var customCodesObject = new List<DigitalTextCodeObject>();

                if (tenant != 0)
                {
                    var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId);

                    if (customTextCodes != null)
                    {
                        customCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);
                    }

                    foreach (var item in customCodesObject)
                    {
                        if (defaultCodesObjectDictionary.ContainsKey(item.Code))
                        {
                            defaultCodesObjectDictionary[item.Code] = item.DisplayText;
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, defaultCodesObjectDictionary);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        [HttpGet]
        [Route("DigitalTextCode/GetDigitalTextCodesObjetTables")]
        public HttpResponseMessage GetDigitalTextCodesObjetTables()
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var textCodeQuery = new DigitalTextCodeQueryService(tenant);
                var objectTables = textCodeQuery.GetDigitalTextCodesObjetTables(0);
                return Request.CreateResponse(HttpStatusCode.OK, objectTables);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}