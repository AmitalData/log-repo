using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityListQueryServices;
using Logitude.TariffModule.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.IO;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityQueryServices;
using System.ServiceModel.DomainServices.Hosting;

namespace WebFreight.Web.TariffModel.DomainServices
{
    [EnableClientAccess()]
    public partial class TariffDomainService : LogitudeDomainService
    {
        public List<TariffList> GetTariffFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

            ITariffModuleContext context = TariffModuleContext.GetContext(tenant);           

            TariffListQueryService listService = new TariffListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TariffList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Tariff", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetTariffFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

            ITariffModuleContext context = TariffModuleContext.GetContext(tenant);

            TariffListQueryService queryService = new TariffListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }



        [Query(HasSideEffects = true)]
        public IQueryable<TariffTypeList> GetTariffTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TariffTypeRepository tariffTypeRepository = new TariffTypeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TariffType> iQueryable = tariffTypeRepository.GetAll();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TariffType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            TariffTypeQueryService tariffTypeQueryService = new TariffTypeQueryService(tariffTypeRepository);
            IQueryable<TariffTypeList> query2 = tariffTypeQueryService.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TariffTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TariffTypeList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TariffType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TariffTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TariffTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TariffTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TariffTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TariffTypeList, bool>(queryOperations, query2);
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
        public int GetTariffTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TariffTypeRepository tariffTypeRepository = new TariffTypeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TariffType> iQueryable = tariffTypeRepository.GetAll();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TariffType>(nonListQueryOperation, iQueryable);
            TariffTypeQueryService tariffTypeQueryService = new TariffTypeQueryService(tariffTypeRepository);
            IQueryable<TariffTypeList> query2 = tariffTypeQueryService.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TariffTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}