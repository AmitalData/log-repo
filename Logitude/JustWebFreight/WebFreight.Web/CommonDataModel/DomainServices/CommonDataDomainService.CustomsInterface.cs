using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        [Query(HasSideEffects = true)]
        public IQueryable<CustomsInterfaceList> GetCustomsInterfaceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("CustomsInterface", "READ", tenant);

            CustomsInterfaceRepository CustomsInterfaceRepository = new CustomsInterfaceRepository(tenant);
            CustomsInterfaceQuery customsInterfaceQuery = new CustomsInterfaceQuery(CustomsInterfaceRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomsInterface> iQueryable = CustomsInterfaceRepository.GetCustomsInterfaces();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomsInterface>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomsInterfaceList> query2 = customsInterfaceQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CustomsInterfaceList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomsInterfaceList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomsInterface", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsInterfaceList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsInterfaceList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsInterfaceList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsInterfaceList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsInterfaceList, bool>(queryOperations, query2);
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

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCustomsInterfaceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("CustomsInterface", "READ", tenant);

            CustomsInterfaceRepository CustomsInterfaceRepository = new CustomsInterfaceRepository(tenant);
            CustomsInterfaceQuery CustomsInterfaceQuery = new CustomsInterfaceQuery(CustomsInterfaceRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomsInterface> iQueryable = CustomsInterfaceRepository.GetCustomsInterfaces();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomsInterface>(nonListQueryOperation, iQueryable);

            IQueryable<CustomsInterfaceList> query2 = CustomsInterfaceQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CustomsInterfaceList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        //Setting
        public IQueryable<CustomsInterfaceSettingPM> GetCustomsInterfaceSettingPMs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomsInterfaceSettingQuery accountingSettingQuery = new CustomsInterfaceSettingQuery(tenant);
            return accountingSettingQuery.GetAccountSettingPMs();
        }

        public CustomsInterfaceSettingPM GetTenantCustomsInterfaceSetting(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomsInterfaceSettingQuery accountingSettingQuery = new CustomsInterfaceSettingQuery(tenant);
            return accountingSettingQuery.GetSinglePM(tenant, 0);
        }
    }
}