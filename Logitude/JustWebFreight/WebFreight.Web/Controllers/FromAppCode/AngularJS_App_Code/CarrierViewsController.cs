// this is added manually to get cards that are only carriers.

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class CarrierViewsController : ApiController
    {


        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                CardRepository cardRepository = new CardRepository(MyContext);
                CardList entityList = null;
                Card entityPoco = cardRepository.GetSingleCard(id, authToken.Tenant);

                if (entityPoco != null)
                {
                    CardQuery cardQuery = new CardQuery(cardRepository);
                    entityList = cardQuery.GetSingleCardList(entityPoco);

                }


                return Request.CreateResponse(HttpStatusCode.OK, entityList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }




     


        public HttpResponseMessage GetAll()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                CardRepository cardRepository = new CardRepository(MyContext);
                IQueryable<Card> entityPocos = cardRepository.GetCarrierCards(authToken.Tenant);

                CardQuery cardQuery = new CardQuery(cardRepository);
                IQueryable<CardList> entityLists = cardQuery.GetIQueryableEntityList(entityPocos);

                return Request.CreateResponse(HttpStatusCode.OK, entityLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = filters.Tenant.Value;

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
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
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
                IQueryable<Card> cards = cardRepository.GetCarrierCards(tenant);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
                CardCustomFilter customfilters = new CardCustomFilter(tenant);

                cards = customfilters.GetFilteredQuery(queryOperations, cards);
                cards = genericFilter.GetFilteredQuery<Card>(nonListQueryOperation, cards);
                int skippedEntities = queryOperations.PageIndex;

                AirlineRepository airlineRepository = new AirlineRepository(MyContext);

                IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
                IQueryable<CardList> myList = from card in cards
                                              join a in airlines on card.Id equals a.Id into airlineCardJoin
                                              from al in airlineCardJoin.DefaultIfEmpty()
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
                                                  Prefix = al.Prefix,
                                                  ICAO = al.ICAO,
                                                  InvitationDate = card.InvitationDate,
                                                  CargoTrackingInvitationDate = card.CargoTrackingInvitationDate,
                                                  SharedLogisticsInvitStatusCode = card.SharedLogisticsInvitStatusCode,
                                                  SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                                  CargoTrackingInvitatStatusCode = card.CargoTrackingInvitatStatusCode,
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
                    entityLists = entityLists.OrderBy(d => d.Code);
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

                foreach (CardList list in entityLists)
                {
                    PartnerType type = partnersTypeRepository.GetSinglePartnerType(list.PartnerTypeId);
                    ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
                    if (table != null)
                    {
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
                    mytenant = filters.Tenant.Value;

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
                IQueryable<Card> cards = cardRepository.GetCarrierCards(tenant);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
                CardCustomFilter customfilters = new CardCustomFilter(tenant);

                cards = customfilters.GetFilteredQuery(queryOperations, cards);
                cards = genericFilter.GetFilteredQuery<Card>(nonListQueryOperation, cards);
                int skippedEntities = queryOperations.PageIndex;

                AirlineRepository airlineRepository = new AirlineRepository(MyContext);

                IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
                IQueryable<CardList> myList = from card in cards
                                              join a in airlines on card.Id equals a.Id into airlineCardJoin
                                              from al in airlineCardJoin.DefaultIfEmpty()
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
                                                  Prefix = al.Prefix,
                                                  ICAO = al.ICAO,
                                                  InvitationDate = card.InvitationDate,
                                                  CargoTrackingInvitationDate = card.CargoTrackingInvitationDate,
                                                  SharedLogisticsInvitStatusCode = card.SharedLogisticsInvitStatusCode,
                                                  SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                                  CargoTrackingInvitatStatusCode = card.CargoTrackingInvitatStatusCode,
                                                  CargoTrackingInvitationStatusName = card.CargoTrackingInvitationStatus != null ? card.CargoTrackingInvitationStatus.Name : null,
                                                  LastLoginDate = card.LastLoginDate,
                                                  PrimaryContactId = card.PrimaryContactId,
                                                  CityName = card.CityName,
                                                  CountryId = card.CountryId,
                                                  CountryCode = card.CountryCode,
                                                  CountryName = card.CountryName,
                                                  UpdateDate=card.UpdateDate==null?card.CreateDate:card.UpdateDate,
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

                List<Card> myCards = cardRepository.GetCarrierCards(mytenant).ToList();

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

        public HttpResponseMessage GetCarrierCopyToCurrentTenant(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                CardQuery cardQuery = new CardQuery(authToken.Tenant);
                CardList myResult = cardQuery.GetCarrierCopyToCurrentTenant(id, authToken.Tenant, null, null, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetByCompactFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = filters.Tenant.Value;

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
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
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
                IQueryable<Card> cards = cardRepository.GetCarrierCards(tenant);

                QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
                queryOperations.QueryFilterItems.Remove(item);

                object seachvalue = item != null ? item.FieldValue : null;

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
                CardCustomFilter customfilters = new CardCustomFilter(tenant);

                cards = customfilters.GetFilteredQuery(queryOperations, cards);
                cards = genericFilter.GetFilteredQuery<Card>(nonListQueryOperation, cards);

                AirlineRepository airlineRepository = new AirlineRepository(MyContext);
                IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
                IQueryable<CardList> myList = from card in cards
                                              join a in airlines on card.Id equals a.Id into airlineCardJoin
                                              from al in airlineCardJoin.DefaultIfEmpty()
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
                                                  WebSite = card.Website,
                                                  SearchFields = card.SearchFields,
                                                  Prefix = al.Prefix,
                                                  ICAO = al.ICAO,
                                                  InvitationDate = card.InvitationDate,
                                                  CargoTrackingInvitationDate = card.CargoTrackingInvitationDate,
                                                  SharedLogisticsInvitStatusCode = card.SharedLogisticsInvitStatusCode,
                                                  SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                                  CargoTrackingInvitatStatusCode = card.CargoTrackingInvitatStatusCode,
                                                  CargoTrackingInvitationStatusName = card.CargoTrackingInvitationStatus != null ? card.CargoTrackingInvitationStatus.Name : null,
                                                  LastLoginDate = card.LastLoginDate,
                                                  PrimaryContactId = card.PrimaryContactId,
                                                  CityName = card.CityName,
                                                  CountryId = card.CountryId,
                                                  CountryCode = card.CountryCode,
                                                  CountryName = card.CountryName,
                                              };
                var query2 = myList;
                query2 = genericFilter.GetFilteredQuery<CardList>(listQueryOperation, query2);
                List<CardList> resultList;

                if (seachvalue != null)
                {
                    listQueryOperation.SetFilter("Code", seachvalue, false, "StartsWith", null, false);
                    IQueryable<CardList> codeQueryResult = genericFilter.GetFilteredQuery<CardList>(listQueryOperation, query2).Take(queryOperations.PageSize);

                    queryOperations.SortByColumnName = "Code";
                    queryOperations.SortDirectin = "Ascending";

                    codeQueryResult = QuerySortClass.GetSortedQuery(queryOperations, codeQueryResult, "Card", tenant);
                    resultList = codeQueryResult.ToList();

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        listQueryOperation.SetFilter("EnglishName", seachvalue, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);

                        IQueryable<CardList> nameQueryResult = genericFilter.GetFilteredQuery<CardList>(listQueryOperation, query2);

                        foreach (CardList card in nameQueryResult)
                        {
                            if (!resultList.Where(p => p.Code == card.Code).Any())
                            {
                                resultList.Add(card);
                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }

                        if (resultList.Count() < queryOperations.PageSize)
                        {
                            listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                            listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                            listQueryOperation.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);

                            IQueryable<CardList> searchFieldQueryResult = genericFilter.GetFilteredQuery<CardList>(listQueryOperation, query2);


                            foreach (CardList card in searchFieldQueryResult)
                            {
                                if (!resultList.Where(p => p.Code == card.Code).Any())
                                {
                                    resultList.Add(card);
                                }
                                if (resultList.Count == queryOperations.PageSize)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    query2 = resultList.AsQueryable();
                }
                else
                {
                    if (string.IsNullOrEmpty(queryOperations.SortByColumnName))
                    {
                        queryOperations.SortByColumnName = "EnglishName";
                    }
                    if (string.IsNullOrEmpty(queryOperations.SortDirectin))
                    {
                        queryOperations.SortDirectin = "Ascending";
                    }

                    query2 = QuerySortClass.GetSortedQuery(queryOperations, query2, "Card", tenant);

                }

                ServiceResponse response = new ServiceResponse();
                response.Count = query2.Count();

                query2 = query2.Take(queryOperations.PageSize);
                List<CardList> result = new List<CardList>();
                foreach (CardList list in query2)
                {
                    PartnerType type = partnersTypeRepository.GetSinglePartnerType(list.PartnerTypeId);
                    ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
                    if (table != null)
                    {
                        if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                        {
                            result.Add(list);
                        }
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
    }
}
