using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using System.Data.Entity;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using System.Reflection;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts.Models;
using WebFreight.Web.Extensions;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPartnersController : ApiController
    {

        [HttpGet]
        [Route("DigitalPartners/NewGetPartnersByFilters")]
        public HttpResponseMessage NewGetPartnersByFilters(string cardId, string cardType, string searchText = "")
        {
            int tenant = 0;
            string email = "";

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 3)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new List<FilterSearchResponse>());
                }

                var shipmentRepository = new ShipmentRepository(authToken.Tenant);

                IQueryable<DigitalShipmentsDataView> shipments = shipmentRepository.GetDigitalShipmentViewsByTenant(authToken.Tenant);

                List<string> cards = cardId?.Split(',').ToList<string>();
                List<string> cardTypes = cardType?.Split(',').ToList<string>();
                var partners = shipments.Where(a => a.Tenant == authToken.Tenant
                                                    && (cardType == null
                                                        || cardType.Trim() == string.Empty
                                                        || (cardTypes.Contains("CS")
                                                            ? cards.Contains(a.CustomerId)
                                                            : cards.Contains(a.AgentId)
                                                            )
                                                            )
                                                    && (a.ConsigneeName.Trim().StartsWith(searchText)
                                                       || a.ShipperName.Trim().StartsWith(searchText)))
                                        .Take(100)
                                        .SelectMany(a => new List<FilterSearchResponse>
                                        {
                                        new FilterSearchResponse
                                        {
                                            Id = a.ConsigneeId,
                                            Name = a.ConsigneeName
                                        },
                                        new FilterSearchResponse
                                        {
                                            Id = a.ShipperId,
                                            Name = a.ShipperName
                                        }
                                        })
                                        .DistinctBy(a => a.Id)
                                        .Where(a => !string.IsNullOrWhiteSpace(a.Name)
                                                    && a.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                                        .Take(10)
                                        .OrderBy(x => x.Name)
                                        .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, partners);

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
        [Route("DigitalPartners/GetFromToDestinationFiltersFilters")]
        public HttpResponseMessage GetFromToDestinationFiltersFilters(string cardId, string cardType, string SearchType, string searchText = "")
        {
            int tenant = 0;
            string email = "";

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 3)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new List<FilterSearchResponse>());
                }

                var shipmentRepository = new ShipmentRepository(authToken.Tenant);

                IQueryable<DigitalShipmentsDataView> shipments = shipmentRepository.GetDigitalShipmentViewsByTenant(authToken.Tenant);

                bool isFrom = SearchType.Equals("From", StringComparison.InvariantCultureIgnoreCase);

                List<string> cards = cardId?.Split(',').ToList<string>();
                List<string> cardTypes = cardType?.Split(',').ToList() ?? new List<string>();


                var query = shipments.Where(a => a.Tenant == authToken.Tenant
                                                   && (isFrom
                                                       ? a.From.Trim().StartsWith(searchText)
                                                       : a.To.Trim().StartsWith(searchText)));

                if (cardTypes.Contains("CS")) query = query.Where(a => cards.Contains(a.CustomerId));
                else if (cardTypes.Contains("AG")) query = query.Where(a => cards.Contains(a.AgentId));

                var results = query.SelectMany(a => new List<FilterSearchResponse>
                                        {
                                        new FilterSearchResponse
                                        {
                                            Id = isFrom ? a.From : a.To,
                                            Name = isFrom ? a.From : a.To
                                        }
                                        })
                                        .DistinctBy(a => a.Name)
                                        .OrderBy(x => x.Name)
                                        .Take(10)
                                        .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, results);

            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalPartners/GetPartnersByFilters")]
        public HttpResponseMessage GetPartnersByFilters([FromUri] DigitalApiQueryFilters filters)
        {
            int tenant = 0;
            string email = "";

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, filters.CardId);

                var queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customer",
                    PageIndex = 0,
                    PageSize = 50,
                    QuerySection = "Customers",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                };

                List<ObjectField> CustomerObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customer", authToken.Tenant);

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = CustomerObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue?.ToString();
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2?.ToString();
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                var genericFilter = new GenericFilter();

                var item = queryOperations.QueryFilterItems
                                          .Where(f => f.FieldName == "CardSearchField")
                                          .FirstOrDefault();

                queryOperations.QueryFilterItems.Remove(item);

                string searchvalue = !string.IsNullOrEmpty(filters.SearchFields) ? !string.IsNullOrEmpty(filters.SearchFields.ToString()) ? filters.SearchFields.ToString() : null : null;
                ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                var customerRepository = new CustomerRepository(MyContext);
                IQueryable<CustomersDataView> entityPocos = customerRepository.GetCustomersDataViews(authToken.Tenant);
                var customerQuery = new CustomerQuery(customerRepository);

                var nonListQueryOperation = new QueryOperations
                {
                    QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
                };

                var listQueryOperation = new QueryOperations
                {
                    QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
                };

                entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);
                IQueryable<CustomerList> entityLists = customerQuery.GetDigitalIQueryableEntityList(entityPocos);
                entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

                if (!string.IsNullOrEmpty(searchvalue))
                {
                    CustomerDataSearchService customerDataSearchService = new CustomerDataSearchService();
                    entityLists = customerDataSearchService.Run(new CustomerSearchArgs() { SearchText = searchvalue, Tenant = authToken.Tenant, EntityLists = entityLists, SortByColumnName = queryOperations.SortByColumnName, SortDirectin = queryOperations.SortDirectin, PageSize = queryOperations.PageSize, FilterItems = queryOperations.QueryFilterItems }).AsQueryable();
                }

                else if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(CustomerList).GetProperty(queryOperations.SortByColumnName);

                    ObjectField objectField = CustomerObjectFields.FirstOrDefault(a => a.FieldName == queryOperations.SortByColumnName);

                    if (objectField != null)
                    {
                        var sortClass = new GenericSort();

                        if (objectField.IsCustom)
                        {
                            entityLists = sortClass.GetSorterQuery<CustomerList, string>(queryOperations, entityLists);
                        }
                        else
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "ntext":
                                case "text":
                                    {
                                        entityLists = sortClass.GetSorterQuery<CustomerList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "sigdouble":
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<CustomerList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "date":
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<CustomerList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsinteger":
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<CustomerList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<CustomerList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsdecimal":
                                case "decimal":
                                    {
                                        entityLists = sortClass.GetSorterQuery<CustomerList, decimal>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderByDescending(d => d.Code);
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.Code);
                }

                var response = new ServiceResponse();

                if (filters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                if (!queryOperations.GetAll)
                {
                    entityLists = entityLists.Skip(queryOperations.PageIndex);
                    entityLists = entityLists.Take(queryOperations.PageSize);
                }

                var listResult = entityLists.ToList();
                var customFieldResolver = new CustomFieldResolver(authToken.Tenant);
                customFieldResolver.SetCustomFieldsValues("Customer", authToken.Tenant, listResult.Cast<object>().ToList());
                response.Result = listResult;
                var reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
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
        [Route("DigitalPartners/GetDigitalShipmentPartners")]
        public HttpResponseMessage GetDigitalShipmentPartners(string shipmentId, string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId, true);
                shipmentId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;
                var shipmentQuery = new ShipmentQuery(tenant);
                var partners = shipmentQuery.GetDigitalShipmentPartners(shipmentId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, partners);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}