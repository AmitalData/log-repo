using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class PortCompactFilter
    {

        public static IQueryable<EntityLists.PortList> GetFilteredQuery(object compactSeachvalue, Simplog.Server.Infrastructure.DataContracts.QueryOperations queryOperations, Simplog.Server.Infrastructure.Helpers.GenericFilter genericFilter, IQueryable<EntityLists.PortList> entityLists, int tenant)
        {
            if (compactSeachvalue != null)
            {
                List<PortList> resultList;
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                listQueryOperation.SetFilter("Code", compactSeachvalue, false, "StartsWith", null, false);
                queryOperations.SortByColumnName = "Code";
                queryOperations.SortDirectin = "Ascending";

                IQueryable<PortList> codeQueryResult = genericFilter.GetFilteredQuery<PortList>(listQueryOperation, entityLists).Take(queryOperations.PageSize);

                codeQueryResult = QuerySortClass.GetSortedQuery(queryOperations, codeQueryResult, "Port", tenant);
                resultList = codeQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("EnglishName", compactSeachvalue, false, "StartsWith", null, false);

                    IQueryable<PortList> nameQueryResult = genericFilter.GetFilteredQuery<PortList>(listQueryOperation, entityLists);

                    queryOperations.SortByColumnName = "EnglishName";
                    nameQueryResult = QuerySortClass.GetSortedQuery(queryOperations, nameQueryResult, "Port", tenant);

                    foreach (PortList port in nameQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == port.Code && p.CountryCode == port.CountryCode).Any())
                        {
                            resultList.Add(port);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("SearchFields", compactSeachvalue, false, "Contains", null, false);

                        IQueryable<PortList> searchFieldQueryResult = genericFilter.GetFilteredQuery<PortList>(listQueryOperation, entityLists);

                        foreach (PortList port in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.Code == port.Code && p.CountryCode == port.CountryCode).Any())
                            {
                                resultList.Add(port);
                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }
                entityLists = resultList.AsQueryable();
            }

            return entityLists;
        }
    }
}
