using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.BL.QuoteModel.CustomFilters;
using Logitude.BL.QuoteModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
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
using WebFreight.Web.Controllers.QuoteModel.ApiHelpers;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.QuoteModel
{
    public class QuoteFollowUpsViewsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Quote", "READ", authToken.Tenant);

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Shipment",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Shipments",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    QueryFilterItems = new List<QueryFilterItem>(),
                };

                List<ObjectField> ShipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant);
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

                        ObjectField field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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

                    foreach (QueryFilterItem myFilter in filters_list)
                    {
                        ObjectField field = ShipmentObjectFields.FirstOrDefault(f => f.FieldName == myFilter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = myFilter.FieldValue != null ? myFilter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = myFilter.FieldValue2 != null ? myFilter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(myFilter.FieldName, value1, field.IsCustomFilter, myFilter.Operator, value2, field.DisplayInList);
                        }

                        else
                        {
                            queryOperations.SetFilter(myFilter.FieldName, myFilter.FieldValue, myFilter.IsCustom, myFilter.Operator, myFilter.FieldValue2, myFilter.DisplayInList);
                        }
                    }
                }

                QuoteAPiHelper.AddFilters(queryOperations, tenant);

                QuoteRepository quoteRepository = new QuoteRepository(tenant);
                GenericFilter filter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                QuoteFollowUpsCustomFilter customfilters = new QuoteFollowUpsCustomFilter(tenant);

                IQueryable<QuoteFollowUpDataView> quoteFollowUps = quoteRepository.GetQuoteFollowUpDataViewByTenant(tenant);
                quoteFollowUps = customfilters.GetQuoteFollowUpFilteredQuery(queryOperations, quoteFollowUps);

                int skippedCount = queryOperations.PageIndex;

                IQueryable<QuoteList> entityLists = from f in quoteFollowUps
                                                    select new QuoteList()
                                                    {
                                                        IsClosed = f.IsClosed,
                                                        Id = f.Id,
                                                        Shipper = f.ShipperName,
                                                        Consignee = f.ConsigneeName,
                                                        QuoteViewId = f.Id + f.FollowUpId,
                                                        FromPort = f.FromPortCode,
                                                        ToPort = f.ToPortCode,
                                                        CustomerName = f.CustomerName,
                                                        Field1 = f.Field1,
                                                        Field2 = f.Field2,
                                                        Field3 = f.Field3,
                                                        Field4 = f.Field4,
                                                        Field5 = f.Field5,
                                                        Field6 = f.Field6,
                                                        Field7 = f.Field7,
                                                        Field9 = f.Field9,
                                                        Field8 = f.Field8,
                                                        Field10 = f.Field10,
                                                        ShipperReference1 = f.ShipperReference1,
                                                        LastModified = f.LastModified,
                                                        QuoteTypeCode = f.QuoteTypeCode,
                                                        ShipmentType = f.ShipmentTypeName,
                                                        DirectionId = f.DirectionId,
                                                        TransportModeId = f.TransportModeId,
                                                        ShipmentTypeId = f.ShipmentTypeId,
                                                        ShipperId = f.ShipperId,
                                                        ShipperName = f.ShipperName,
                                                        FromPortId = f.FromPortId,
                                                        ToPortId = f.ToPortId,
                                                        QuoteNumber = f.QuoteNumber,
                                                        OpenDate = f.OpenDate,
                                                        ExpirationDate = f.ExpirationDate,
                                                        MainCarriageCarrierName = f.MainCarriageCarrierName,
                                                        MainCarriageCarrierId = f.MainCarriageCarrierId,
                                                        QuoteTypeName = f.QuoteTypeName,
                                                        CarrierName = f.MainCarriageCarrierName,
                                                        ChargeableWeight = f.ChargeableWeight,
                                                        GrossWeight = f.GrossWeight,
                                                        IsCancelled = f.IsCancelled,
                                                        SearchFields = f.SearchFields,
                                                        Notes = f.Notes,
                                                        BranchId = f.BranchId,
                                                        DepartmentId = f.DepartmentId,
                                                        NumberOfContainers = f.NumberOfContainers,
                                                        FromPartnerId = f.FromPartnerId,
                                                        ToPartnerId = f.ToPartnerId,
                                                        FromPartnerAddressId = f.FromPartnerAddressId,
                                                        ToPartnerAddressId = f.ToPartnerAddressId,
                                                        FollowUpDate = f.FollowUpDate,
                                                        FollowUpType = f.FollowUpType,
                                                        FollowUpNotes = f.FollowUpNotes,
                                                        FollowUpOwner = f.FollowUpOwner,
                                                        Subject = f.Subject,
                                                        IsSubjectEdited = f.IsSubjectEdited,
                                                        StageId = f.StageId,
                                                        StageName = f.StageName,
                                                        StageDueDate = f.StageDueDate,
                                                        RatingCode = f.RatingCode,
                                                        LastActivityDate = f.LastActivityDate,
                                                        LastActivitySubject = f.LastActivitySubject,
                                                        LastActivityTypeCode = f.LastActivityTypeCode,
                                                        NextActivityDate = f.NextActivityDate,
                                                        NextActivitySubject = f.NextActivitySubject,
                                                        NextActivityTypeCode = f.NextActivityTypeCode,
                                                        RatingName = f.RatingCode == "C" ? "Cold" : (f.RatingCode == "H" ? "Hot" : f.RatingCode == "N" ? "Neutral" : "Warm"),
                                                        LastActivityTypeName = f.LastActivityTypeCode == "CL" ? "Call" : (f.LastActivityTypeCode == "TS" ? "Task" : (f.LastActivityTypeCode == "AP" ? "Appointment" : (f.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                                        NextActivityTypeName = f.NextActivityTypeCode == "CL" ? "Call" : (f.NextActivityTypeCode == "TS" ? "Task" : (f.NextActivityTypeCode == "AP" ? "Appointment" : (f.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                                        OpportunityId = f.OpportunityId,
                                                        IsAutomaticallyClosed = f.IsAutomaticallyClosed,
                                                        AutomaticallyCloseDays = f.AutomaticallyCloseDays,
                                                        AutomaticallyCloseDate = f.AutomaticallyCloseDate,
                                                        UpdateDate = f.UpdateDate,
                                                        BusinessUnitId = f.BusinessUnitId,
                                                        BusinessUnitName = f.BusinessUnitName,
                                                        QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                                                        QuoteClosingReasonName = f.QuoteClosingReasonName,
                                                        SalesmanUserId = f.SalesmanUserId,
                                                        IncotermCode = f.IncotermCode,
                                                        ValueOfGoods = f.ValueOfGoods,
                                                        CreatedByUserId = f.CreatedByUserId,
                                                        UpdatedByUserId = f.UpdatedByUserId,
                                                    };

                entityLists = filter.GetFilteredQuery<QuoteList>(queryOperations, entityLists);
                entityLists = QuoteAPiHelper.ApplyFilters(entityLists, tenant);

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(QuoteList).GetProperty(queryOperations.SortByColumnName);
                    List<ObjectField> quoteObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Quote", tenant).ToList();

                    ObjectField objectField = (from a in quoteObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "text":
                            case "ntext":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "double":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, double>(queryOperations, entityLists);
                                    break;
                                }
                            case "datetime":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, DateTime>(queryOperations, entityLists);
                                    break;
                                }
                            case "integer":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, int>(queryOperations, entityLists);
                                    break;
                                }
                            default:
                                {
                                    entityLists = entityLists.OrderByDescending(d => d.OpenDate);
                                    break;
                                }
                        }
                    }
                }

                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.OpenDate);
                }

                ServiceResponse response = new ServiceResponse();

                int count = 0;
                if (filters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                entityLists = entityLists.Skip(skippedCount);
                entityLists = entityLists.Take(queryOperations.PageSize);

                List<QuoteList> listQuery = entityLists.ToList();
                response.Result = listQuery;

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                if (filters.GetCount)
                {
                    reponseMessage.Headers.Add("TotalCount", count.ToString());

                }

                return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingle(string Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Quote", "READ", authToken.Tenant);

                IQuotesContext MyContext = QuotesContext.GetContext(authToken.Tenant);
                QuoteRepository quoteRepository = new QuoteRepository(MyContext);

                QuoteFollowUpDataView f = quoteRepository.GetSingleQuoteFollowUpDataView(Id, authToken.Tenant);

                QuoteList myResult = new QuoteList()
                {
                    IsClosed = f.IsClosed,
                    Id = f.Id,
                    Shipper = f.ShipperName,
                    Consignee = f.ConsigneeName,
                    QuoteViewId = f.Id + f.FollowUpId,
                    FromPort = f.FromPortCode,
                    ToPort = f.ToPortCode,
                    CustomerName = f.CustomerName,
                    Field1 = f.Field1,
                    Field2 = f.Field2,
                    Field3 = f.Field3,
                    Field4 = f.Field4,
                    Field5 = f.Field5,
                    Field6 = f.Field6,
                    Field7 = f.Field7,
                    Field9 = f.Field9,
                    Field8 = f.Field8,
                    Field10 = f.Field10,
                    ShipperReference1 = f.ShipperReference1,
                    LastModified = f.LastModified,
                    QuoteTypeCode = f.QuoteTypeCode,
                    ShipmentType = f.ShipmentTypeName,
                    DirectionId = f.DirectionId,
                    TransportModeId = f.TransportModeId,
                    ShipmentTypeId = f.ShipmentTypeId,
                    ShipperId = f.ShipperId,
                    ShipperName = f.ShipperName,
                    FromPortId = f.FromPortId,
                    ToPortId = f.ToPortId,
                    QuoteNumber = f.QuoteNumber,
                    OpenDate = f.OpenDate,
                    ExpirationDate = f.ExpirationDate,
                    MainCarriageCarrierName = f.MainCarriageCarrierName,
                    MainCarriageCarrierId = f.MainCarriageCarrierId,
                    QuoteTypeName = f.QuoteTypeName,
                    CarrierName = f.MainCarriageCarrierName,
                    ChargeableWeight = f.ChargeableWeight,
                    GrossWeight = f.GrossWeight,
                    IsCancelled = f.IsCancelled,
                    SearchFields = f.SearchFields,
                    Notes = f.Notes,
                    BranchId = f.BranchId,
                    DepartmentId = f.DepartmentId,
                    NumberOfContainers = f.NumberOfContainers,
                    FromPartnerId = f.FromPartnerId,
                    ToPartnerId = f.ToPartnerId,
                    FromPartnerAddressId = f.FromPartnerAddressId,
                    ToPartnerAddressId = f.ToPartnerAddressId,
                    FollowUpDate = f.FollowUpDate,
                    FollowUpType = f.FollowUpType,
                    FollowUpNotes = f.FollowUpNotes,
                    FollowUpOwner = f.FollowUpOwner,
                    Subject = f.Subject,
                    IsSubjectEdited = f.IsSubjectEdited,
                    StageId = f.StageId,
                    StageName = f.StageName,
                    StageDueDate = f.StageDueDate,
                    RatingCode = f.RatingCode,
                    LastActivityDate = f.LastActivityDate,
                    LastActivitySubject = f.LastActivitySubject,
                    LastActivityTypeCode = f.LastActivityTypeCode,
                    NextActivityDate = f.NextActivityDate,
                    NextActivitySubject = f.NextActivitySubject,
                    NextActivityTypeCode = f.NextActivityTypeCode,
                    RatingName = f.RatingCode == "C" ? "Cold" : (f.RatingCode == "H" ? "Hot" : f.RatingCode == "N" ? "Neutral" : "Warm"),
                    LastActivityTypeName = f.LastActivityTypeCode == "CL" ? "Call" : (f.LastActivityTypeCode == "TS" ? "Task" : (f.LastActivityTypeCode == "AP" ? "Appointment" : (f.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                    NextActivityTypeName = f.NextActivityTypeCode == "CL" ? "Call" : (f.NextActivityTypeCode == "TS" ? "Task" : (f.NextActivityTypeCode == "AP" ? "Appointment" : (f.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                    OpportunityId = f.OpportunityId,
                    IsAutomaticallyClosed = f.IsAutomaticallyClosed,
                    AutomaticallyCloseDays = f.AutomaticallyCloseDays,
                    AutomaticallyCloseDate = f.AutomaticallyCloseDate,
                    UpdateDate = f.UpdateDate,
                    BusinessUnitId = f.BusinessUnitId,
                    BusinessUnitName = f.BusinessUnitName,
                    QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                    QuoteClosingReasonName = f.QuoteClosingReasonName,
                    SalesmanUserId = f.SalesmanUserId,
                    IncotermCode = f.IncotermCode,
                    ValueOfGoods = f.ValueOfGoods,
                    CreatedByUserId = f.CreatedByUserId,
                    UpdatedByUserId = f.UpdatedByUserId,
                };

                ServiceResponse response = new ServiceResponse();
                response.Result = myResult;

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}