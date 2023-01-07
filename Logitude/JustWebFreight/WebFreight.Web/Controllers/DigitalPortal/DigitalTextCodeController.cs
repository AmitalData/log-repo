using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
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
        [Route("DigitalTextCode/GetDigitalProfileName")]
        public HttpResponseMessage GetDigitalProfileName()
        {
            string email = "";
            int tenant = 0;
            try
            {
                var textCodeQuery = new DigitalProfileQueryService(tenant);
                var digitalProfiles = textCodeQuery.GetDigitalProfileQuery(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, digitalProfiles);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {0}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalTextCode/GetFeildPermissionByFilters")]
        public HttpResponseMessage GetFeildPermissionByFilters(string cardId, string objectTableId, string profileId)
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

                var helper = new DigitalFieldSecuritesHelper();
                var defaultDigitalFieldSecurity = helper.GitDigitalSecuritesFeilds(objectTableId, profileId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, defaultDigitalFieldSecurity);
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
        [Route("DigitalTextCode/UpdateFeildPermission")]
        public HttpResponseMessage UpdateFeildPermission(DigitalFeildSecurityObjectModel digitalFeildSecurityObjectModel)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                digitalFeildSecurityObjectModel.Tenant = tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, digitalFeildSecurityObjectModel.CardId);
                var digitalFieldSecurityQuery = new DigitalFieldSecurityQueryService(tenant);
                var customDigitalFieldSecurity = digitalFieldSecurityQuery.GetDigitalFieldSecurityQuery(digitalFeildSecurityObjectModel.Tenant, digitalFeildSecurityObjectModel.ObjectTableId, digitalFeildSecurityObjectModel.ProfileId);

                if (customDigitalFieldSecurity != null)
                {
                    var existingDigitalFieldSecurityMappedObject = JsonConvert.DeserializeObject<List<DigitalFeildSecurityUpdateModel>>(customDigitalFieldSecurity.DefaultSettings);

                    var diff = existingDigitalFieldSecurityMappedObject.Except(digitalFeildSecurityObjectModel.DefaultSettings).ToList();

                    foreach (var item in diff)
                    {
                        existingDigitalFieldSecurityMappedObject.Remove(item);
                    }

                    foreach (var item in digitalFeildSecurityObjectModel.DefaultSettings)
                    {
                        var existingKey = existingDigitalFieldSecurityMappedObject.FirstOrDefault(a => a.FieldCode.Equals(item.FieldCode));

                        if (existingKey != null)
                        {
                            continue;
                        }
                        else
                        {
                            existingDigitalFieldSecurityMappedObject.Add(item);
                        }
                    }

                    customDigitalFieldSecurity.DefaultSettings = JsonConvert.SerializeObject(existingDigitalFieldSecurityMappedObject);
                    customDigitalFieldSecurity.UpdateDate = DateTime.UtcNow;
                }
                else
                {
                    customDigitalFieldSecurity = new DigitalFieldSecurityList
                    {
                        ObjectTableId = digitalFeildSecurityObjectModel.ObjectTableId,
                        Tenant = digitalFeildSecurityObjectModel.Tenant,
                        DefaultSettings = JsonConvert.SerializeObject(digitalFeildSecurityObjectModel.DefaultSettings),
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        ProfileId = digitalFeildSecurityObjectModel.ProfileId
                    };
                }

                digitalFieldSecurityQuery.UpdateDigitalFieldSecurity(customDigitalFieldSecurity);

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
        [Route("DigitalTextCode/GetTextCodesByFilters")]
        public HttpResponseMessage GetTextCodesByFilters(string cardId, string objectTableId, string profileId)
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

                var defaultTextCode = textCodeQuery.GetDigitalTextCodesQuery(0, objectTableId, profileId);

                var defaultCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(defaultTextCode.Labels);

                var customCodesObject = new List<DigitalTextCodeObject>();

                if (tenant != 0)
                {
                    var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId, profileId);

                    if (customTextCodes != null)
                    {
                        customCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);
                    }
                }

                foreach (var item in customCodesObject)
                {
                    var temp = defaultCodesObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));

                    if (temp != null)
                    {
                        temp.DisplayText = item.DefaultText;
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
                var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(digitalTextCodeUpdateModel.Tenant, digitalTextCodeUpdateModel.ObjectTableId, digitalTextCodeUpdateModel.ProfileId);

                if (customTextCodes != null)
                {
                    var customCodesMappedObject = JsonConvert.DeserializeObject<List<DigitalTextCodeUpdateObject>>(customTextCodes.Labels);

                    foreach (var item in digitalTextCodeUpdateModel.Lables)
                    {
                        var existingKey = customCodesMappedObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));

                        if (string.IsNullOrWhiteSpace(item.DefaultText))
                        {
                            customCodesMappedObject.RemoveAll(a=>a.TextCode == item.TextCode);
                        }
                        else
                        {
                            if (existingKey != null)
                            {
                                existingKey.DefaultText = item.DefaultText;
                            }
                            else
                            {
                                customCodesMappedObject.Add(item);
                            }
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
                        ProfileId = digitalTextCodeUpdateModel.ProfileId,
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
        public HttpResponseMessage GetTranslationCodes(int tenant, string objectTableId, string profileId)
        {
            try
            {
                var textCodeQuery = new DigitalTextCodeQueryService(tenant);
                var defaultTextCodes = textCodeQuery.GetDigitalTextCodesQuery(0, objectTableId, profileId);
                var defaultCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(defaultTextCodes.Labels);
                var customCodesObject = new List<DigitalTextCodeObject>();

                if (tenant != 0)
                {
                    var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId, profileId);

                    if (customTextCodes != null)
                    {
                        customCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);
                    }

                    foreach (var item in customCodesObject)
                    {
                        var data = defaultCodesObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));

                        if (data != null)
                        {
                            data.DefaultText = item.DefaultText;
                        }
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalTextCode/GetDigitalTextCodesObjetTables")]
        public HttpResponseMessage GetDigitalTextCodesObjetTables()
        {
            string email = "";
            try
            {
                var textCodeQuery = new DigitalTextCodeQueryService(0);
                var objectTables = textCodeQuery.GetDigitalTextCodesObjetTables(0);
                return Request.CreateResponse(HttpStatusCode.OK, objectTables);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {0}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalTextCode/GetDigitalProfilesObjetTables")]
        public HttpResponseMessage GetDigitalProfilesObjetTables()
        {
            string email = "";
            try
            {
                var digitalFieldSecurityQuery = new DigitalFieldSecurityQueryService(0);
                var objectTables = digitalFieldSecurityQuery.GetDigitalProfilesObjetTables(0);
                return Request.CreateResponse(HttpStatusCode.OK, objectTables);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {0}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}