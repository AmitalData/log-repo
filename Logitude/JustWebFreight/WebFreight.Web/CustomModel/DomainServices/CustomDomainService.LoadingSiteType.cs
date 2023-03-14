using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public LoadingSiteTypePM GetSingleLoadingSiteTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            LoadingSiteTypeQueryService loadingSiteTypeQuery = new LoadingSiteTypeQueryService(customContext);
            LoadingSiteTypePM LoadingSiteType = loadingSiteTypeQuery.GetSingle(code, false, false);
            return LoadingSiteType;
        }

        public LoadingSiteTypeList GetSingleLoadingSiteTypeList(string code, int tenant)
        {
       
       
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            LoadingSiteTypeListQueryService listService = new LoadingSiteTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<LoadingSiteTypeList> GetLoadingSiteTypeLists(int tenant)
        {
       
            customContext = CustomContext.GetContext(tenant);
            LoadingSiteTypeListQueryService listService = new LoadingSiteTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<LoadingSiteTypeList> GetLoadingSiteTypeFilters(byte[] xmlFilters, int tenant)
        {
       
            customContext = CustomContext.GetContext(tenant);
            LoadingSiteTypeListQueryService listService = new LoadingSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }


        [Query(HasSideEffects = true)]
        public List<LoadingSiteTypeList> GetLoadingSiteTypeCompactFilters(byte[] xmlFilters, int tenant)
        {
             

            customContext = CustomContext.GetContext(tenant);
            LoadingSiteTypeListQueryService listService = new LoadingSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);


             
            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);
            object seachvalue = item != null ? item.FieldValue : null;
            List<LoadingSiteTypeList> resultList = null;

           
            if (seachvalue != null)
            {
                int count = listService.GetListCount(queryOperations);

                queryOperations.SetFilter("Code", seachvalue, false, "StartsWith", null, false);
                queryOperations.SortByColumnName = "Code";
                queryOperations.SortDirectin = "Ascending";



                List<LoadingSiteTypeList> codeQueryResult = listService.GetList(queryOperations, tenant);
                resultList = codeQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize && count > resultList.Count())
                {
                    queryOperations.SetFilter("Code", null, false, "StartsWith", null, false);
                    queryOperations.SetFilter("LocalName", seachvalue, false, "StartsWith", null, false);
                    queryOperations.SortByColumnName = "LocalName";

                    List<LoadingSiteTypeList> nameQueryResult = listService.GetList(queryOperations, tenant);



                    foreach (LoadingSiteTypeList site in nameQueryResult)
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

                        List<LoadingSiteTypeList> searchFieldQueryResult = listService.GetList(queryOperations, tenant);

                        foreach (LoadingSiteTypeList site in searchFieldQueryResult)
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

        public int GetLoadingSiteTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
          
            customContext = CustomContext.GetContext(tenant);
            LoadingSiteTypeListQueryService queryService = new LoadingSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}