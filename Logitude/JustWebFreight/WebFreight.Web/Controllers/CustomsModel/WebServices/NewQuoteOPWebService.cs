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
        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
        AmitalContext GetAmitalContext(int tenant)
        {
            var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
            if (tenantAmitalContext == null)
            {

                tenantAmitalContext = AmitalContext.GetContext(tenant);
                _AmitalContextList.Add(tenantAmitalContext);
            }
            return tenantAmitalContext;
        }
        public HttpResponseMessage GetETBPAYTRitemList(string PTERMID, string search, int top, bool searchNULLVendor = true)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (search != null && (search.ToLower() == "undefined" || search.ToLower() == "null"))
                {
                    search = null;
                }

                if (PTERMID != null && (PTERMID.ToLower() == "undefined" || PTERMID.ToLower() == "null"))
                {
                    PTERMID = null;
                }

                #region get data from Unifri

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {
                    var ETBPAYTR = new ETBPAYTRRepository(GetAmitalContext(tenant));
                    var q =
                     ETBPAYTR
                        .GetAll().Select(o => new
                        {
                            Name = o.NAMEENG,
                            PTERMID = o.PTERMID,
                        })
                    ;

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        search = search.ToUpper();
                        q = q.Where(rec => rec.PTERMID.Contains(search));
                    }
                    q = q.Distinct();
                    q = q.Take(top);

                    var ETBPAYTRList = q.ToList();
                    #endregion

                    return Request.CreateResponse(HttpStatusCode.OK, ETBPAYTRList);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCarriersItemsList(string DIRECTIONID, string TRANSPORTMODEID, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    SortDirectin = filters.SortDirection,
                    SortByColumnName = filters.SortBy,
                    
                };
                List<ObjectField> objectFields = new List<ObjectField>
                {
                    new ObjectField() { FieldName = "Name",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "AIRLINE_ID",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "VENDOR_ID",DataTypeCode="Text" },
                    new ObjectField() { FieldName = "Prefix",DataTypeCode="Text" }
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
                var quoteOpCarriers = new QuoteOpCarriers(tenant);
                var carriersList=quoteOpCarriers.GetCarriersItemsList(DIRECTIONID,TRANSPORTMODEID,queryOperations);
                return Request.CreateResponse(HttpStatusCode.OK, carriersList);

               
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSpecialServiceItemsList(string DIRECTIONID, string TRANSPORTMODEID, string search, int top, bool searchNULLVendor = true)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (search != null && (search.ToLower() == "undefined" || search.ToLower() == "null"))
                {
                    search = null;
                }

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {

                    if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
                    {
                        var ETBSERLVRepo = new ETBSERLVRepository(GetAmitalContext(tenant));
                        var ETBSERLVquery =
                         ETBSERLVRepo
                            .GetAll().Select(o => new
                            {
                                Name = o.NAMEENG,
                                SERVLEVEL_ID = o.SERVLEVELID,
                            })
                        ;
                        ETBSERLVquery = ETBSERLVquery.Distinct();
                        ETBSERLVquery = ETBSERLVquery.Take(top);

                        var ETBSERLVList = ETBSERLVquery.ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, ETBSERLVList);
                    }
                    else
                    {
                        string systemVariable = (DIRECTIONID == "E" && TRANSPORTMODEID == "O") ? "M" : ((DIRECTIONID == "I" && TRANSPORTMODEID == "A") ? "I" : ((DIRECTIONID == "I" && TRANSPORTMODEID == "O") ? "R" : ""));
                        var GTBSERLVRepo = new GTBSERLVRepository(GetAmitalContext(tenant));
                        var GTBSERLVquery =
                         GTBSERLVRepo
                            .GetAll().Where(a => a.SYSTEM == systemVariable).Select(o => new
                            {
                                Name = o.NAMEENG,
                                SERVLEVEL_ID = o.SERVLEVELID,
                            })
                        ;
                        GTBSERLVquery = GTBSERLVquery.Distinct();
                        GTBSERLVquery = GTBSERLVquery.Take(top);
                        var GTBSERLVList = GTBSERLVquery.ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, GTBSERLVList);
                    }


                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetPortsItemsList(string DIRECTIONID, string TRANSPORTMODEID, string search, int top, bool searchNULLVendor = true)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (search != null && (search.ToLower() == "undefined" || search.ToLower() == "null"))
                {
                    search = null;
                }

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                CountryRepository countryRepository = new CountryRepository(tenant);

                if (setting.IsConnectedToUniFreight)
                {
                    List<Port> Ports = new List<Port>();
                    if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
                    {
                        var ETBPORTRepo = new ETBPORTRepository(GetAmitalContext(tenant));
                        var ETBPORTquery =
                       ETBPORTRepo.GetAll().Select(o => new
                       {
                           Name = o.NAMEENG,
                           Code = o.PORTID,
                           COUNTRYID = o.COUNTRYID,
                       });
                        ETBPORTquery = ETBPORTquery.Distinct();
                        ETBPORTquery = ETBPORTquery.Take(top);

                        var ETBPORTList = ETBPORTquery.ToList();
                        foreach (var item in ETBPORTList)
                        {
                            var country = countryRepository.GetSingleCountryByCode(item.COUNTRYID, tenant, true);
                            var port = new Port();
                            if (country != null)
                            {
                                port.CountryName = country.EnglishName;
                            }
                            port.CountryId = item.COUNTRYID;
                            port.Code = item.Code;
                            port.Name = item.Name;
                            Ports.Add(port);
                        }
                    }
                    if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
                    {
                        var MTBPORTRepo = new MTBPORTRepository(GetAmitalContext(tenant));
                        var MTBPORTquery = MTBPORTRepo.GetAll().Select(o => new
                        {
                            Name = o.NAMEENG,
                            Code = o.PORTID,
                            COUNTRYID = o.COUNTRYID,
                        });

                        MTBPORTquery = MTBPORTquery.Distinct();
                        MTBPORTquery = MTBPORTquery.Take(top);

                        var MTBPORTList = MTBPORTquery.ToList();
                        foreach (var item in MTBPORTList)
                        {
                            var country= countryRepository.GetSingleCountryByCode(item.COUNTRYID, tenant,true);
                            var port = new Port();
                            if (country != null)
                            {
                                port.CountryName = country.EnglishName;
                            }
                            port.CountryId = item.COUNTRYID;
                            port.Code = item.Code;
                            port.Name = item.Name;
                            Ports.Add(port);
                        }
                    }
                    if (DIRECTIONID == "I" && TRANSPORTMODEID == "A")
                    {
                        var ITBPORTRepo = new ITBPORTRepository(GetAmitalContext(tenant));
                        var ITBPORTquery =
                         ITBPORTRepo
                            .GetAll().Select(o => new
                            {
                                Name = o.NAMEENG,
                                Code = o.PORTID,
                                COUNTRYID = o.COUNTRYID,
                            })
                        ;
                        ITBPORTquery = ITBPORTquery.Distinct();
                        ITBPORTquery = ITBPORTquery.Take(top);

                        var ITBPORTList = ITBPORTquery.ToList();
                        foreach (var item in ITBPORTList)
                        {
                            var country = countryRepository.GetSingleCountryByCode(item.COUNTRYID, tenant, true);
                            var port = new Port();
                            if (country != null)
                            {
                                port.CountryName = country.EnglishName;
                            }
                            port.CountryId = item.COUNTRYID;
                            port.Code = item.Code;
                            port.Name = item.Name;
                            Ports.Add(port);
                        }
                    }
                    if (DIRECTIONID == "I" && TRANSPORTMODEID == "O")
                    {
                        var RTBPORTRepo = new RTBPORTRepository(GetAmitalContext(tenant));
                        var RTBPORTquery =
                         RTBPORTRepo
                            .GetAll().Select(o => new
                            {
                                Name = o.NAMEENG,
                                Code = o.PORTID,
                                COUNTRYID = o.COUNTRYID,
                            })
                        ;
                        RTBPORTquery = RTBPORTquery.Distinct();
                        RTBPORTquery = RTBPORTquery.Take(top);

                        var RTBPORTList = RTBPORTquery.ToList();
                        foreach (var item in RTBPORTList)
                        {
                            var country = countryRepository.GetSingleCountryByCode(item.COUNTRYID, tenant, true);
                            var port = new Port();
                            if (country != null)
                            {
                                port.CountryName = country.EnglishName;
                            }
                            port.CountryId = item.COUNTRYID;
                            port.Code = item.Code;
                            port.Name = item.Name;
                            Ports.Add(port);
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, Ports);

                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    class Port
    {
        public string Name;
        public string Code;
        public string CountryId;
        public string CountryName;
    }
}