using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using System.Web;


namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class GTBFUSTATUListQueryService
    {
        private ICustomContext context;
        public GTBFUSTATUListQueryService(ICustomContext context)
        {
            this.context = context;
        }
        private IQueryable<GTBFUSTATUList> GetIqueryableList(IQueryable<EntityPOCOs.GTBFUSTATU> iQueryable)
        {

            IQueryable<GTBFUSTATUList> query = (from a in iQueryable
                                                select new GTBFUSTATUList()
                                                     {
                                                         Code = a.Code,
                                                         Name = a.Name,
                                                         SearchFields=a.Code,
                                                     });
            return query;
        }

        private IQueryable<EntityPOCOs.GTBFUSTATU> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<EntityPOCOs.GTBFUSTATU> iQueryable)
        {
            return iQueryable;
        }
        public List<GTBFUSTATUList> GetList(QueryOperations queryOperations, int tenant, IQueryable<GTBFUSTATU> iQueryable)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable);
            queryOperations.GetAll = true;
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<EntityPOCOs.GTBFUSTATU>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<GTBFUSTATUList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<GTBFUSTATUList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(GTBFUSTATUList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> GTBFUSTATUObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.GTBFUSTATU", tenant).ToList();

                ObjectField objectField = (from a in GTBFUSTATUObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<GTBFUSTATUList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<GTBFUSTATUList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<GTBFUSTATUList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<GTBFUSTATUList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<GTBFUSTATUList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<GTBFUSTATUList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<GTBFUSTATUList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.Code);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2.ToList();


        }

        public List<GTBFUSTATUList> GetList(int tenant, IQueryable<GTBFUSTATU> iQueryable)
        {
            return GetList(new QueryOperations() { QueryFilterItems = new List<QueryFilterItem>(), PageIndex = 0, GetAll = true }, tenant, iQueryable);
        }


        public int GetListCount(QueryOperations queryOperations, IQueryable<GTBFUSTATU> iQueryable)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<EntityPOCOs.GTBFUSTATU>(nonListQueryOperation, iQueryable);

            IQueryable<GTBFUSTATUList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<GTBFUSTATUList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


    }
}
