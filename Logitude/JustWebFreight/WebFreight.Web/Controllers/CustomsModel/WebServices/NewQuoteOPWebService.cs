using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;
using Unifreight.BL.BL;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class NewQuoteOPWebServiceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Carriers(string DIRECTIONID, string TRANSPORTMODEID, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = GetTenant();

                List<ObjectField> objectFields = new List<ObjectField>
                {
                    new ObjectField() { FieldName = "Name",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "AIRLINE_ID",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "VENDOR_ID",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "Prefix",DataTypeCode="Text" }
                };

                QueryOperations queryOperations = InitFilter(filters, objectFields);
                var quoteOpCarriers = new QuoteOpCarriers(tenant);
                var carriersList = quoteOpCarriers.GetCarriersItemsList(DIRECTIONID, TRANSPORTMODEID, queryOperations);

                return Request.CreateResponse(HttpStatusCode.OK, carriersList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage Ports(string DIRECTIONID, string TRANSPORTMODEID, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = GetTenant();
            
                List<ObjectField> objectFields = new List<ObjectField>
                {
                    new ObjectField() { FieldName = "Code",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "Name",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "CountryName",DataTypeCode="Text" }
                };

                QueryOperations queryOperations = InitFilter(filters, objectFields);
                var quoteOPService = new QuoteOPPorts(tenant);
                var carriersList = quoteOPService.GetItemsList(DIRECTIONID, TRANSPORTMODEID, queryOperations);

                return Request.CreateResponse(HttpStatusCode.OK, carriersList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage SpecialServices(string DIRECTIONID, string TRANSPORTMODEID, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = GetTenant();
            
                List<ObjectField> objectFields = new List<ObjectField>
                {
                    new ObjectField() { FieldName = "SERVLEVEL_ID",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "Name",DataTypeCode="Text" },
                };

                QueryOperations queryOperations = InitFilter(filters, objectFields);
                var quoteOPService = new QuoteOPSpecialServices(tenant);
                var carriersList = quoteOPService.GetItemsList(DIRECTIONID, TRANSPORTMODEID, queryOperations);

                return Request.CreateResponse(HttpStatusCode.OK, carriersList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage Incoterms([FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = GetTenant();
            
                List<ObjectField> objectFields = new List<ObjectField>
                {
                    new ObjectField() { FieldName = "PTERMID",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "Name",DataTypeCode="Text" },
                };

                QueryOperations queryOperations = InitFilter(filters, objectFields);
                var quoteOPService = new QuoteOPIncoterms(tenant);
                var carriersList = quoteOPService.GetItemsList(queryOperations);

                return Request.CreateResponse(HttpStatusCode.OK, carriersList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static int GetTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            return tenant;
        }

        private static QueryOperations InitFilter(ApiQueryFilters filters, List<ObjectField> objectFields)
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                PageIndex = filters.PageIndex,
                PageSize = filters.PageSize,
                SortDirectin = filters.SortDirection,
                SortByColumnName = filters.SortBy,
            };

            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                foreach (QueryFilterItem filter in filters_list)
                {
                    ObjectField field = objectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                    if (field != null)
                    {
                        string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                        //queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);

                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }

            return queryOperations;
        }
    }
}