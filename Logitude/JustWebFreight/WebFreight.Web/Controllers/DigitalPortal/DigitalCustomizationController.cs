using Logitude.BL.InfrastructureModel.CustomFilters;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalCustomizationController : ApiController
    {
        [HttpGet]
        [Route("DigitalCustomization/GetObjectFieldsByFilters")]
        public HttpResponseMessage GetObjectFieldsByFilters([FromUri] ApiQueryFilters filters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "ObjectField",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "ObjectFields",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> ObjectFieldObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ObjectField", 0);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";                    
                        ObjectField field = ObjectFieldObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = ObjectFieldObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null 
                                                  ? filter.FieldValue.ToString() 
                                                  : null;

                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                            
                            string valuestring2 = filter.FieldValue2 != null 
                                                  ? filter.FieldValue2.ToString() 
                                                  : null;

                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                GenericFilter genericFilter = new GenericFilter();

                IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);
                ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository(MyContext);
                var IncludeMetaDataFields = queryOperations.QueryFilterItems.Where(a => a.FieldName == "IncludeMetaDataFields").FirstOrDefault() != null;
                IQueryable<ObjectField> entityPocos = objectFieldRepository.GetObjectFieldsFromTenanZeroAndMyTenant(tenant, IncludeMetaDataFields);

                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(objectFieldRepository);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                if (listQueryOperation.QueryFilterItems != null)
                {
                    var queryFilter = listQueryOperation.QueryFilterItems.Where(d => d.FieldName == "FullNameTextCodeDefaultText").FirstOrDefault();
                    if (queryFilter != null) queryFilter.Operator = "Contains";
                }

                entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);

                ObjectFieldCustomFilter customfilters = new ObjectFieldCustomFilter(tenant);
                entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);

                IQueryable<ObjectFieldList> entityLists = objectFieldQuery.GetIQueryableEntityList(entityPocos);

                entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

                ServiceResponse response = new ServiceResponse();

                if (filters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                if (!queryOperations.GetAll)
                {
                    entityLists = entityLists.Skip(queryOperations.PageIndex);
                    entityLists = entityLists.Take(queryOperations.PageSize);
                }

                List<ObjectFieldList> listResult = entityLists.OrderBy(d => d.FullNameTextCodeDefaultText).ToList();
                response.Result = listResult;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                return reponseMessage;
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
        [Route("DigitalCustomization/GetCustomFieldsByTableId")]
        public HttpResponseMessage GetCustomFieldsByTableId(string objectTableId)
        {
            int tenant = 0;
            string email = "";

            try
            {
                string securityKey = HttpContext.Current.Request.Headers["securitykey"];

                if (!string.IsNullOrWhiteSpace(securityKey))
                {
                    var authenticationHelper = new DigitalPortalAuthenticationHelper();
                    var shipmentIdAndTenant = authenticationHelper.GetShipmentBySecurityKey(securityKey);
                    if (shipmentIdAndTenant == null)
                    {
                        throw new AutenticationException("Sorry! this user is not authorized!");
                    }

                    tenant = shipmentIdAndTenant.Item2;
                }
                else
                {
                    var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                    email = authToken.Email;
                    tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                }

                var ObjectFieldsRepository = new ObjectFieldRepository(tenant);
                var objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
                List<ObjectFieldPM> result = objectFieldsQuery.GetDigitalCustomFieldsBytableID(objectTableId, tenant).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalCustomization/GetDigitalPortalScreenNames")]
        public HttpResponseMessage GetDigitalPortalScreenNames()
        {
            int tenant = 0;
            string email = "";
            try
            {
                var screenQueryService = new DigitalPortalScreenQueryService(tenant);
                var digitalPortalScreens = screenQueryService.GetDigitalPortalScreenNamesQuery(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, digitalPortalScreens);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalCustomization/GetDigitalPortalScreens")]
        public HttpResponseMessage GetDigitalPortalScreens(string objectTableId, string screenCode = "")
        {
            int tenant = 0;
            string email = "";
            try
            {
                string securityKey = HttpContext.Current.Request.Headers["securitykey"];

                if (!string.IsNullOrWhiteSpace(securityKey))
                {
                    var authenticationHelper = new DigitalPortalAuthenticationHelper();
                    var shipmentIdAndTenant = authenticationHelper.GetShipmentBySecurityKey(securityKey);
                    if (shipmentIdAndTenant == null)
                    {
                        throw new AutenticationException("Sorry! this user is not authorized!");
                    }

                    tenant = shipmentIdAndTenant.Item2;
                }
                else
                {
                    var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                    email = authToken.Email;
                    tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                }

                var screenQueryService = new DigitalPortalScreenQueryService(tenant);
                var digitalPortalScreens = screenQueryService.GetDigitalPortalScreensQuery(tenant, objectTableId, screenCode);
                var data = digitalPortalScreens.FirstOrDefault(a => a.Tenant == tenant);

                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }

                return Request.CreateResponse(HttpStatusCode.OK, digitalPortalScreens.FirstOrDefault());
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
        [Route("DigitalCustomization/GetDigitalPreDefinedComponents")]
        public HttpResponseMessage GetDigitalPreDefinedComponents(string objectTableId, string name = "")
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var preDefinedComponentQueryService = new DigitalPreDefinedComponentQueryService(tenant);
                var digitalPreDefinedComponents = preDefinedComponentQueryService.GetDigitalPreDefinedComponentQuery(tenant, objectTableId, name);
                return Request.CreateResponse(HttpStatusCode.OK, digitalPreDefinedComponents);
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
        [Route("DigitalCustomization/UpdateDigitalPortalScreen")]
        public HttpResponseMessage UpdateDigitalPortalScreen(DigitalPortalScreenUpdateModel digitalPortalScreenUpdateModel)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                digitalPortalScreenUpdateModel.Tenant = tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var digitalPreDefinedComponentQueryService = new DigitalPortalScreenQueryService(tenant);

                var tenantDigitalPortalScreen = digitalPreDefinedComponentQueryService.GetDigitalPortalScreensQuery(tenant, digitalPortalScreenUpdateModel.ObjectTableId, digitalPortalScreenUpdateModel.ScreenCode)
                                                                                      .FirstOrDefault(a => a.Tenant == tenant);

                DigitalPortalScreenList defaultTenantDigitalPortalScreen = null;

                if (digitalPortalScreenUpdateModel.IsDraft && string.IsNullOrEmpty(digitalPortalScreenUpdateModel.Content))
                {
                     defaultTenantDigitalPortalScreen = digitalPreDefinedComponentQueryService.GetDigitalPortalScreensQuery(tenant, digitalPortalScreenUpdateModel.ObjectTableId, digitalPortalScreenUpdateModel.ScreenCode)
                                                                      .FirstOrDefault(a => a.Tenant == 0);
                }


                if (tenantDigitalPortalScreen == null)
                {
                    tenantDigitalPortalScreen = new DigitalPortalScreenList
                    {
                        Name = digitalPortalScreenUpdateModel.Name,
                        Content = digitalPortalScreenUpdateModel.IsDraft  
                                    && string.IsNullOrEmpty(digitalPortalScreenUpdateModel.Content) 
                                 ? defaultTenantDigitalPortalScreen.Content 
                                 : digitalPortalScreenUpdateModel.Content,
                        DraftContent =  digitalPortalScreenUpdateModel.DraftContent,
                        Tenant = tenant,
                        ScreenCode = digitalPortalScreenUpdateModel.ScreenCode,
                        ObjectTableId = digitalPortalScreenUpdateModel.ObjectTableId,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };
                }
                else
                {
                    if (digitalPortalScreenUpdateModel.IsDraft)
                    {
                        tenantDigitalPortalScreen.DraftContent = digitalPortalScreenUpdateModel.DraftContent;
                        tenantDigitalPortalScreen.UpdateDate = DateTime.UtcNow;
                    }
                    else
                    {
                        tenantDigitalPortalScreen.Content = digitalPortalScreenUpdateModel.Content;
                        tenantDigitalPortalScreen.DraftContent = digitalPortalScreenUpdateModel.DraftContent;
                        tenantDigitalPortalScreen.UpdateDate = DateTime.UtcNow;
                    }
                }

                digitalPreDefinedComponentQueryService.UpdateDigitalPortalScreen(tenantDigitalPortalScreen);
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
        [Route("DigitalCustomization/GetDefaultScreenLayout")]
        public HttpResponseMessage GetDefaultScreenLayout(string objectTableId, string screenCode)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var digitalPreDefinedComponentQueryService = new DigitalPortalScreenQueryService(tenant);

                var allScreens = digitalPreDefinedComponentQueryService.GetDigitalPortalScreensQuery(tenant, objectTableId, screenCode);
                var defaultDisgitalPortalScreen = allScreens.FirstOrDefault(a => a.Tenant == 0);
                var tenantDigitalPortalScreen = allScreens.FirstOrDefault(a => a.Tenant == tenant);

                if (tenantDigitalPortalScreen != null)
                {
                    tenantDigitalPortalScreen.Content = tenantDigitalPortalScreen.Content;
                    tenantDigitalPortalScreen.DraftContent = defaultDisgitalPortalScreen.DraftContent;
                    tenantDigitalPortalScreen.UpdateDate = DateTime.UtcNow;
                    digitalPreDefinedComponentQueryService.UpdateDigitalPortalScreen(tenantDigitalPortalScreen);
                }
                else
                {
                    tenantDigitalPortalScreen = defaultDisgitalPortalScreen;
                }

                return Request.CreateResponse(HttpStatusCode.OK, tenantDigitalPortalScreen);
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