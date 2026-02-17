using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using System.ServiceModel.DomainServices.Server;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private FHLStatusRepository FHLStatusRepository;
        private FHLStatusQuery FHLStatusQuery;

        public IQueryable<FHLStatus> GetFHLStatus(int tenant)
        {
            FHLStatusRepository = new FHLStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return FHLStatusRepository.GetFHLStatus();
        }

        public IQueryable<FHLStatus> GetFHLStatusByTenant(int tenant)
        {
            FHLStatusRepository = new FHLStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return FHLStatusRepository.GetFHLStatus();
        }

        public IQueryable<FHLStatus> GetFirstFHLStatus(string input, int tenant)
        {
            FHLStatusRepository = new FHLStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            input = input.ToUpper();
            return FHLStatusRepository.GetFHLStatus().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public FHLStatusPM GetSingleFHLStatus(string code, int tenant)
        {
            FHLStatusQuery = new FHLStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return FHLStatusQuery.GetSingleFHLStatusPM(code);
        }

        public FHLStatusList GetSingleFHLStatusList(string code, int tenant)
        {
            FHLStatusQuery = new FHLStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            FHLStatusPM entityPM = FHLStatusQuery.GetSingleFHLStatusPM(code);
            FHLStatusList entityList = new FHLStatusList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
            };
            return entityList;
        }

        public IQueryable<FHLStatusList> GetFHLStatusLists(int tenant)
        {
            FHLStatusRepository = new FHLStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<FHLStatus> iQueryable = FHLStatusRepository.GetFHLStatus();
            var query2 = from entity in iQueryable
                         select new FHLStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<FHLStatusList> GetFHLStatusFilters(byte[] xmlFilters, int tenant)
        {
            FHLStatusRepository = new FHLStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<FHLStatus> iQueryable = FHLStatusRepository.GetFHLStatus();

            //PortCustomFilter customfilters = new PortCustomFilter();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<FHLStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new FHLStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<FHLStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(FHLStatusList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("FHLStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<FHLStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<FHLStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<FHLStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<FHLStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<FHLStatusList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetFHLStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            FHLStatusRepository = new FHLStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<FHLStatus> iQueryable = FHLStatusRepository.GetFHLStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<FHLStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new FHLStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<FHLStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        public void InsertFHLStatus(FHLStatus entity)
        {
            FHLStatusRepository.Add(entity);
        }

        public void UpdateFHLStatus(FHLStatus currentEntity)
        {
            FHLStatusRepository.Update(currentEntity);
        }

        public void DeleteFHLStatus(FHLStatus entity)
        {
            FHLStatusRepository.Remove(entity);
        }

    }
}