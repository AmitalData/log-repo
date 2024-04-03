using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
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
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class WarehouseExtendedController : ApiController
    {
        public HttpResponseMessage getTenantImportByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                int mytenant = 0;
                int tenant = 0;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                mytenant = authToken.Tenant;

                if (filters.Tenant != null)
                {
                    mytenant = filters.Tenant.Value;
                    SecurityUtility.AuthenticationOnTenant(filters.Tenant.Value);
                }
                

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Card",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Cards",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> CardObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Card", tenant);
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
                        ObjectField field = CardObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }
                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = CardObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);

                PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(MyContext);
                CardRepository cardRepository = new Simplog.Data.CommonDataModel.Repositories.CardRepository(MyContext);

                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                IQueryable<Card> cards = cardRepository.GetWarehouseCards(tenant);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
                CardCustomFilter customfilters = new CardCustomFilter(tenant);

                cards = customfilters.GetFilteredQuery(queryOperations, cards);
                cards = genericFilter.GetFilteredQuery<Card>(nonListQueryOperation, cards);
                int skippedEntities = queryOperations.PageIndex;
                
                IQueryable<CardList> myList = from card in cards                                              
                                              where card.Tenant == tenant
                                              select new CardList()
                                              {
                                                  Code = card.Code,
                                                  CreateDate = card.CreateDate,
                                                  EnglishName = card.EnglishName,
                                                  LocalName = card.LocalName,
                                                  ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                                  PayablesAccountingCard = card.PayablesAccountingCard,
                                                  InActive = card.InActive,
                                                  Notes = card.Notes,
                                                  SupportNotes = card.SupportNotes,
                                                  Id = card.Id,
                                                  Tenant = card.Tenant,
                                                  VatNumber = card.VatNumber,
                                                  PaymentTermId = card.PaymentTermId,
                                                  PartnerTypeId = card.PartnerTypeId,
                                                  PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
                                                  PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
                                                  SalesmanUserId = card.Customer != null ? card.Customer.SalesmanUserId : "",
                                                  WebSite = card.Website,
                                                  SearchFields = card.SearchFields,
                                                  InvitationDate = card.InvitationDate,
                                                  CargoTrackingInvitationDate = card.CargoTrackingInvitationDate,
                                                  SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                                  SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                                  CargoTrackingInvitationStatusCode = card.CargoTrackingInvitationStatusCode,
                                                  CargoTrackingInvitationStatusName = card.CargoTrackingInvitationStatus != null ? card.CargoTrackingInvitationStatus.Name : null,
                                                  LastLoginDate = card.LastLoginDate,
                                                  PrimaryContactId = card.PrimaryContactId,
                                                  CityName = card.CityName,
                                                  CountryId = card.CountryId,
                                                  CountryCode = card.CountryCode,
                                                  CountryName = card.CountryName,
                                              };

                var entityLists = myList;
                entityLists = genericFilter.GetFilteredQuery<CardList>(listQueryOperation, entityLists);
                
                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(CardList).GetProperty(queryOperations.SortByColumnName);
                    List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Card", tenant).ToList();

                    ObjectField objectField = (from a in shipmentObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "text":
                                {
                                    entityLists = sortClass.GetSorterQuery<CardList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "double":
                                {
                                    entityLists = sortClass.GetSorterQuery<CardList, double>(queryOperations, entityLists);
                                    break;
                                }
                            case "datetime":
                                {
                                    entityLists = sortClass.GetSorterQuery<CardList, DateTime>(queryOperations, entityLists);
                                    break;
                                }
                            case "integer":
                                {
                                    entityLists = sortClass.GetSorterQuery<CardList, int>(queryOperations, entityLists);
                                    break;
                                }
                            case "lookup":
                                {
                                    entityLists = sortClass.GetSorterQuery<CardList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "boolean":
                                {
                                    entityLists = sortClass.GetSorterQuery<CardList, bool>(queryOperations, entityLists);
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
                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.Code);
                }

                ServiceResponse response = new ServiceResponse();

                if (filters.GetCount)
                {
                    response.Count = entityLists.Count();
                }
                List<CardList> result = new List<CardList>();
                if (!queryOperations.GetAll)
                {
                    entityLists = entityLists.Skip(skippedEntities);
                    entityLists = entityLists.Take(queryOperations.PageSize);
                }

                List<Card> myCards = cardRepository.GetWarehouseCards(mytenant).ToList();

                foreach (CardList list in entityLists)
                {
                    PartnerType type = partnersTypeRepository.GetSinglePartnerType(list.PartnerTypeId);
                    ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
                    if (table != null)
                    {
                        if (myCards.Where(p => p.Code == list.Code).FirstOrDefault() != null)
                        {
                            list.InUse = true;
                        }
                        else
                        {
                            list.InUse = false;
                        }

                        if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                        {
                            result.Add(list);
                        }
                        else
                        { }
                    }
                }

                response.Result = result;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);


                return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetWarehouseTypeById(string warehouseId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                WarehouseQuery warehouseQuery = new WarehouseQuery(tenant);
                string warehouseType = warehouseQuery.GetWarehouseTypeById(warehouseId, tenant);

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, warehouseType);


                return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}