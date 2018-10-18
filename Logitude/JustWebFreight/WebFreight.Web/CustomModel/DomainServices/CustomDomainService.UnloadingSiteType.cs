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

        public UnloadingSiteTypePM GetSingleUnloadingSiteTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            unloadingSiteTypeQuery = new UnloadingSiteTypeQueryService(customContext);
            UnloadingSiteTypePM UnloadingSiteType = unloadingSiteTypeQuery.GetSingle(code, false, false);
            return UnloadingSiteType;
        }

        public UnloadingSiteTypeList GetSingleUnloadingSiteTypeList(string code, int tenant)
        {
       
       
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            UnloadingSiteTypeListQueryService listService = new UnloadingSiteTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<UnloadingSiteTypeList> GetUnloadingSiteTypeLists(int tenant)
        {
       
            customContext = CustomContext.GetContext(tenant);
            UnloadingSiteTypeListQueryService listService = new UnloadingSiteTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<UnloadingSiteTypeList> GetUnloadingSiteTypeFilters(byte[] xmlFilters, int tenant)
        {
       
            customContext = CustomContext.GetContext(tenant);
            UnloadingSiteTypeListQueryService listService = new UnloadingSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }


        [Query(HasSideEffects = true)]
        public List<UnloadingSiteTypeList> GetUnloadingSiteTypeCompactFilters(byte[] xmlFilters, int tenant)
        {
             

            customContext = CustomContext.GetContext(tenant);
            UnloadingSiteTypeListQueryService listService = new UnloadingSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);


             
            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);
            object seachvalue = item != null ? item.FieldValue : null;
            List<UnloadingSiteTypeList> resultList = null;

           
            if (seachvalue != null)
            {
                int count = listService.GetListCount(queryOperations);

                queryOperations.SetFilter("Code", seachvalue, false, "StartsWith", null, false);
                queryOperations.SortByColumnName = "Code";
                queryOperations.SortDirectin = "Ascending";



                List<UnloadingSiteTypeList> codeQueryResult = listService.GetList(queryOperations, tenant);
                resultList = codeQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize && count > resultList.Count())
                {
                    queryOperations.SetFilter("Code", null, false, "StartsWith", null, false);
                    queryOperations.SetFilter("LocalName", seachvalue, false, "StartsWith", null, false);
                    queryOperations.SortByColumnName = "LocalName";

                    List<UnloadingSiteTypeList> nameQueryResult = listService.GetList(queryOperations, tenant);



                    foreach (UnloadingSiteTypeList site in nameQueryResult)
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

                        List<UnloadingSiteTypeList> searchFieldQueryResult = listService.GetList(queryOperations, tenant);

                        foreach (UnloadingSiteTypeList site in searchFieldQueryResult)
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

        public int GetUnloadingSiteTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
          
            customContext = CustomContext.GetContext(tenant);
            UnloadingSiteTypeListQueryService queryService = new UnloadingSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}