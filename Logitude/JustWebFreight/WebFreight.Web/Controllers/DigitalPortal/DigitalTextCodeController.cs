using Logitude.BL.Helpers;
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
        public HttpResponseMessage GetDigitalProfileName(int tenant = 0)
        {
            string email = "";
            int defaultTenantNumber = 0;
            try
            {
                var digitalProfileQuery = new DigitalProfileQueryService(defaultTenantNumber);
                var tenantDigitalProfiles = digitalProfileQuery.GetDigitalProfileQuery(tenant);

                if (!tenantDigitalProfiles.Any())
                {
                    var digitalProfiles = digitalProfileQuery.GetDigitalProfileQuery(defaultTenantNumber);

                    foreach (var item in digitalProfiles)
                    {
                        digitalProfileQuery.UpdateDigitalProfile(new DigitalProfileList
                        {
                            Name = item.Name,
                            Tenant = tenant,
                            Code = item.Code,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        });
                    }

                    tenantDigitalProfiles = digitalProfileQuery.GetDigitalProfileQuery(tenant);
                    var textCodeQuery = new DigitalTextCodeQueryService(tenant);
                    var textCodes = textCodeQuery.GetDigitalTextCodesTenant0();
                    foreach (var item in textCodes)
                    {
                        if (item.ObjectTableName.Equals("general", StringComparison.InvariantCultureIgnoreCase))
                        {
                            textCodeQuery.UpdateDigitalTextCodes(new DigitalTextCodeList
                            {
                                Tenant = tenant,
                                Labels = item.Labels,
                                ObjectTableId = item.ObjectTableId,
                                ProfileId = tenantDigitalProfiles.Where(a => a.Code == item.ProfileCode).Select(a => a.Id).FirstOrDefault(),
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow
                            });
                        }
                        else
                        {
                            textCodeQuery.UpdateDigitalTextCodes(new DigitalTextCodeList
                            {
                                Tenant = tenant,
                                Labels = item.Labels,
                                ObjectTableId = item.ObjectTableId,
                                ProfileId = tenantDigitalProfiles.Where(a => a.Code == item.ProfileCode 
                                                                             && !a.Code.Equals("CM"))
                                                                 .Select(a => a.Id)
                                                                 .FirstOrDefault(),
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow
                            });
                        }
                    }

                    var filedsQuery = new DigitalFieldSecurityQueryService(tenant);
                    var fields = filedsQuery.GetDigitalFieldSecurityQueryTenant0();
                    foreach (var item in fields)
                    {
                        filedsQuery.UpdateDigitalFieldSecurity(new DigitalFieldSecurityList
                        {
                            Tenant = tenant,
                            DefaultSettings = item.DefaultSettings,
                            ObjectTableId = item.ObjectTableId,
                            ProfileId = tenantDigitalProfiles.Where(a => a.Code == item.ProfileCode
                                                                         && !a.Code.Equals("CM"))
                                                             .Select(a => a.Id)
                                                             .FirstOrDefault(),
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        });
                    }

                    var screenQuery = new DigitalPortalScreenQueryService(tenant);
                    var screens = screenQuery.GetDigitalPortalScreenNamesTenant0().ToList();

                    foreach (var item in screens)
                    {
                        screenQuery.UpdateDigitalPortalScreen(new DigitalPortalScreenList
                        {
                            Tenant = tenant,
                            Name = item.Name,
                            ScreenCode = item.ScreenCode,
                            Content = item.Content,
                            DraftContent = item.DraftContent,
                            ObjectTableId = item.ObjectTableId,
                            ProfileId = tenantDigitalProfiles.Where(a => a.Code == item.ProfileCode
                                                                         && !a.Code.Equals("CM"))
                                                             .Select(a => a.Id)
                                                             .FirstOrDefault(),
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, tenantDigitalProfiles);
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
        public HttpResponseMessage GetFeildPermissionByFilters(string cardId, string objectTableId, string profileCode)
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
                var defaultDigitalFieldSecurity = helper.GitDigitalSecuritesFeilds(objectTableId, profileCode, tenant);
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
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, digitalFeildSecurityObjectModel.CardId);

                var textCodeQuery = new DigitalTextCodeQueryService(0);
                var generalObjectTableId = textCodeQuery.GetDigitalTextCodesObjetTables(0)
                                                   .Where(a => a.ObjectTableName.Equals("General"))
                                                   .FirstOrDefault();

                if (generalObjectTableId.Equals(digitalFeildSecurityObjectModel.ObjectTableId))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, this operation isn't allowed for general fields");
                }

                digitalFeildSecurityObjectModel.Tenant = tenant;
                var digitalFieldSecurityQuery = new DigitalFieldSecurityQueryService(tenant);
                var customDigitalFieldSecurity = digitalFieldSecurityQuery.GetDigitalFieldSecurityQuery(digitalFeildSecurityObjectModel.Tenant, digitalFeildSecurityObjectModel.ObjectTableId, digitalFeildSecurityObjectModel.ProfileCode);

                if (customDigitalFieldSecurity != null)
                {
                    customDigitalFieldSecurity.DefaultSettings = JsonConvert.SerializeObject(digitalFeildSecurityObjectModel.DefaultSettings);
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
        public HttpResponseMessage GetTextCodesByFilters(string cardId, string objectTableId, string profileCode)
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

                if (profileCode == "null")
                {
                    var digitalProfileQueryService = new DigitalProfileQueryService(0);
                    profileCode = digitalProfileQueryService.GetDigitalProfileByName(0, "Customer").Code;
                }

                var defaultTextCode = textCodeQuery.GetDigitalTextCodesQuery(0, objectTableId, profileCode);

                if (defaultTextCode == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new List<DigitalTextCodeObject>());
                }

                var defaultCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(defaultTextCode.Labels);

                var customCodesObject = new List<DigitalTextCodeObject>();

                if (tenant != 0)
                {
                    var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId, profileCode);

                    if (customTextCodes != null)
                    {
                        customCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, defaultCodesObject);
                    }
                }

                foreach (var item in customCodesObject)
                {
                    var temp = defaultCodesObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));

                    if (temp != null)
                    {
                        temp.DisplayText = item.DisplayText;
                    }
                    else
                    {
                        defaultCodesObject.Add(item);
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
                var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(digitalTextCodeUpdateModel.Tenant, digitalTextCodeUpdateModel.ObjectTableId, digitalTextCodeUpdateModel.ProfileCode);

                if (customTextCodes != null)
                {
                    var customCodesMappedObject = JsonConvert.DeserializeObject<List<DigitalTextCodeUpdateObject>>(customTextCodes.Labels);

                    foreach (var item in digitalTextCodeUpdateModel.Lables)
                    {
                        var existingKey = customCodesMappedObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));

                        if (string.IsNullOrWhiteSpace(item.DisplayText))
                        {
                            customCodesMappedObject.RemoveAll(a => a.TextCode == item.TextCode);
                        }
                        else
                        {
                            if (existingKey != null)
                            {
                                existingKey.DisplayText = item.DisplayText;
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
        public HttpResponseMessage GetTranslationCodes(int tenant, string objectTableId, string profileCode)
        {
            try
            {
                var textCodeQuery = new DigitalTextCodeQueryService(tenant);
                var defaultTextCodes = textCodeQuery.GetDigitalTextCodesQuery(0, objectTableId, profileCode);

                if (defaultTextCodes == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new List<DigitalTextCodeObject>());
                }

                var defaultCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(defaultTextCodes.Labels);
                var customCodesObject = new List<DigitalTextCodeObject>();

                if (tenant != 0)
                {
                    var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId, profileCode);

                    if (customTextCodes != null)
                    {
                        customCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, defaultCodesObject);
                    }

                    foreach (var item in customCodesObject)
                    {
                        var data = defaultCodesObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));

                        if (data != null)
                        {
                            if (!string.IsNullOrWhiteSpace(item.DisplayText))
                            {
                                data.DefaultText = item.DisplayText;
                            }

                            continue;
                        }
                        else
                        {
                            defaultCodesObject.Add(item);
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