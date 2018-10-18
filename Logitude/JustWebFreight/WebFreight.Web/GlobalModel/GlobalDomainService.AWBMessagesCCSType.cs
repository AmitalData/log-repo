using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.GlobalModel
{
    public partial class GlobalDomainService
    {
        private AWBMessagesCCSTypeQuery aWBMessagesCCSTypeQuery;
        private AWBMessagesCCSTypeRepository aWBMessagesCCSTypeRepository;
        public void UpdateDueTypeList(AWBMessagesCCSTypeList currentEntity)
        {

        }

        public IQueryable<AWBMessagesCCSTypeList> GetAWBMessagesCCSTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aWBMessagesCCSTypeRepository = new AWBMessagesCCSTypeRepository(tenant);
            aWBMessagesCCSTypeQuery = new AWBMessagesCCSTypeQuery(aWBMessagesCCSTypeRepository);

            IQueryable<AWBMessagesCCSType> iQueryable = aWBMessagesCCSTypeRepository.GetAWBMessagesCCSTypes();
            IQueryable<AWBMessagesCCSTypeList> query2 = aWBMessagesCCSTypeQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }


        [Query(HasSideEffects = true)]
        public IQueryable<AWBMessagesCCSTypeList> GetAWBMessagesCCSTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aWBMessagesCCSTypeRepository = new AWBMessagesCCSTypeRepository(tenant);
            aWBMessagesCCSTypeQuery = new AWBMessagesCCSTypeQuery(aWBMessagesCCSTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBMessagesCCSType> iQueryable = aWBMessagesCCSTypeRepository.GetAWBMessagesCCSTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBMessagesCCSType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AWBMessagesCCSTypeList> query2 = aWBMessagesCCSTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<AWBMessagesCCSTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBMessagesCCSTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBMessagesCCSType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagesCCSTypeList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagesCCSTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagesCCSTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagesCCSTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagesCCSTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagesCCSTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAWBMessagesCCSTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aWBMessagesCCSTypeRepository = new AWBMessagesCCSTypeRepository(tenant);
            aWBMessagesCCSTypeQuery = new AWBMessagesCCSTypeQuery(aWBMessagesCCSTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBMessagesCCSType> iQueryable = aWBMessagesCCSTypeRepository.GetAWBMessagesCCSTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBMessagesCCSType>(nonListQueryOperation, iQueryable);

            IQueryable<AWBMessagesCCSTypeList> query2 = aWBMessagesCCSTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<AWBMessagesCCSTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }
    }
}