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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class CustomsItemListQueryService
    {
        private IQueryable<CustomsItemList> GetIqueryableList(IQueryable<CustomsItem> iQueryable)
        {
            IQueryable<CustomsItemList> query = (from a in iQueryable
                                                 select new CustomsItemList()
                                                 {
                                                     ID = a.ID,
                                                     FullClassification = a.FullClassification,
                                                     ComputedCheckDigit = a.ComputedCheckDigit,
                                                     CustomsBookTypeID = a.CustomsBookTypeID,
                                                     CustomsItemCategoryID = a.CustomsItemCategoryID,
                                                     CustomsItemHierarchicLocationID = a.CustomsItemHierarchicLocationID,
                                                 });
            return query;
        }

        private IQueryable<CustomsItem> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsItem> iQueryable)
        {
            if (queryOperations.QueryFilterItems.Any(x => x.FieldName == "dateExpire"))
            {
                var join = iQueryable.Join(
                    context.PropertiesDetailsHistorys,
                    cusomsItem => cusomsItem.ID,
                    propertiesDetailsHistory => propertiesDetailsHistory.CustomsItemID,
                    (cusomsItem, propertiesDetailsHistory) => new { cusomsItem, propertiesDetailsHistory }
                    );

                join = join.Where(x=> x.propertiesDetailsHistory.StartDate <= DateTime.UtcNow);
                join = join.Where(x=> x.propertiesDetailsHistory.EndDate >= DateTime.UtcNow);

                iQueryable = join.Select(x => x.cusomsItem).Distinct().AsQueryable();
            }

            return iQueryable;
        }
        public List<JoinCustomsItemList> GetListJoinCustomsItem(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomsItem> iQueryable = (from a in context.CustomsItems
                                                  select a);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomsItem>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<JoinCustomsItemList> query2 = GetIqueryableListJoin(iQueryable);

            query2 = filter.GetFilteredQuery<JoinCustomsItemList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(JoinCustomsItemList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CustomsItemObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CustomsItem", tenant).ToList();

                ObjectField objectField = (from a in CustomsItemObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<JoinCustomsItemList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<JoinCustomsItemList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<JoinCustomsItemList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<JoinCustomsItemList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<JoinCustomsItemList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<JoinCustomsItemList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<JoinCustomsItemList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderBy(d => d.ID);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderBy(d => d.ID);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2.ToList();


        }
        private IQueryable<JoinCustomsItemList> GetIqueryableListJoin(IQueryable<CustomsItem> iQueryable)
        {
            var query1 = (from c in context.CustomsItemDetailsHistorys select c).Select(x => new { x.CustomsItemID, x.Title, x.EntityStatusID }).Distinct();
                       
            


            IQueryable<JoinCustomsItemList> query = (from a in iQueryable
                                                 join b in query1
                                                 on a.ID equals b.CustomsItemID into CustIt
                                                     from ci in CustIt.DefaultIfEmpty()
                                                 where ci.EntityStatusID!=4
                                                 select new JoinCustomsItemList()
                                                 {
                                                     ID = a.ID,
                                                     FullClassification = a.FullClassification,
                                                     ComputedCheckDigit = a.ComputedCheckDigit,
                                                     CustomsBookTypeID = a.CustomsBookTypeID,
                                                     CustomsItemCategoryID = a.CustomsItemCategoryID,
                                                     CustomsItemHierarchicLocationID = a.CustomsItemHierarchicLocationID,
                                                     Title = ci.Title,
                                                 });
          
            return query;
        }

     
    }


}
