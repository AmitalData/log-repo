using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.ServiceModel.DomainServices.Server;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public InternationalSitePM GetSingleInternationalSitePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            internationalSiteQuery = new InternationalSiteQueryService(customContext);
            InternationalSitePM InternationalSite = internationalSiteQuery.GetSingle(id, false, false);
            return InternationalSite;
        }

        public InternationalSiteList GetSingleInternationalSiteList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            InternationalSiteListQueryService listService = new InternationalSiteListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<InternationalSiteList> GetInternationalSiteLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            InternationalSiteListQueryService listService = new InternationalSiteListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<InternationalSiteList> GetInternationalSiteFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
 

            customContext = CustomContext.GetContext(tenant);
            InternationalSiteListQueryService listService = new InternationalSiteListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }


        [Query(HasSideEffects = true)]
        public List<InternationalSiteList> GetInternationalSiteCompactFilters(byte[] xmlFilters, int tenant)
        {


            customContext = CustomContext.GetContext(tenant);
            InternationalSiteListQueryService listService = new InternationalSiteListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);



            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);
            object seachvalue = item != null ? item.FieldValue : null;
            List<InternationalSiteList> resultList = null;


            if (seachvalue != null)
            {
                int count = listService.GetListCount(queryOperations);

                queryOperations.SetFilter("Code", seachvalue, false, "StartsWith", null, false);
                queryOperations.SortByColumnName = "Code";
                queryOperations.SortDirectin = "Ascending";



                List<InternationalSiteList> codeQueryResult = listService.GetList(queryOperations, tenant);
                resultList = codeQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize && count > resultList.Count())
                {
                    queryOperations.SetFilter("Code", null, false, "StartsWith", null, false);
                    queryOperations.SetFilter("LocalName", seachvalue, false, "StartsWith", null, false);
                    queryOperations.SortByColumnName = "LocalName";

                    List<InternationalSiteList> nameQueryResult = listService.GetList(queryOperations, tenant);



                    foreach (InternationalSiteList site in nameQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == site.Code).Any())
                        {
                            resultList.Add(site);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        queryOperations.SetFilter("Code", null, false, "StartsWith", null, false);
                        queryOperations.SetFilter("LocalName", null, false, "StartsWith", null, false);
                        queryOperations.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);
                        queryOperations.SortByColumnName = "Code";

                        List<InternationalSiteList> searchFieldQueryResult = listService.GetList(queryOperations, tenant);

                        foreach (InternationalSiteList site in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.Code == site.Code).Any())
                            {
                                resultList.Add(site);

                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }

            }
            else
            {
                resultList = listService.GetList(queryOperations, tenant);
            }

            return resultList;
        }

        public int GetInternationalSiteFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            InternationalSiteListQueryService queryService = new InternationalSiteListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}