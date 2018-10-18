using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.IO;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Reflection;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        
        private FWBStatusRepository FWBStatusRepository;
        private FWBStatusQuery FWBStatusQuery;

        public IQueryable<FWBStatus> GetFWBStatus(int tenant)
        {
            FWBStatusRepository = new FWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return FWBStatusRepository.GetFWBStatus();
        }

        public IQueryable<FWBStatus> GetFWBStatusByTenant(int tenant)
        {
            FWBStatusRepository = new FWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return FWBStatusRepository.GetFWBStatus();
        }

        public IQueryable<FWBStatus> GetFirstFWBStatus(string input, int tenant)
        {
            FWBStatusRepository = new FWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            input = input.ToUpper();
            return FWBStatusRepository.GetFWBStatus().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public FWBStatusPM GetSingleFWBStatus(string code, int tenant)
        {
            FWBStatusQuery = new FWBStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return FWBStatusQuery.GetSingleFWBStatusPM(code);
        }

        public FWBStatusList GetSingleFWBStatusList(string code, int tenant)
        {
            FWBStatusQuery = new FWBStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            FWBStatusPM entityPM = FWBStatusQuery.GetSingleFWBStatusPM(code);
            FWBStatusList entityList = new FWBStatusList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
            };
            return entityList;
        }

        public IQueryable<FWBStatusList> GetFWBStatusLists(int tenant)
        {
            FWBStatusRepository = new FWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<FWBStatus> iQueryable = FWBStatusRepository.GetFWBStatus();
            var query2 = from entity in iQueryable
                         select new FWBStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<FWBStatusList> GetFWBStatusFilters(byte[] xmlFilters, int tenant)
        {
            FWBStatusRepository = new FWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<FWBStatus> iQueryable = FWBStatusRepository.GetFWBStatus();

            //PortCustomFilter customfilters = new PortCustomFilter();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<FWBStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new FWBStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<FWBStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(FWBStatusList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("FWBStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<FWBStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<FWBStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<FWBStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<FWBStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<FWBStatusList, bool>(queryOperations, query2);
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

        public int GetFWBStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            FWBStatusRepository = new FWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<FWBStatus> iQueryable = FWBStatusRepository.GetFWBStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<FWBStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new FWBStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<FWBStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        public void InsertFWBStatus(FWBStatus entity)
        {
            FWBStatusRepository.Add(entity);
        }

        public void UpdateFWBStatus(FWBStatus currentEntity)
        {
            FWBStatusRepository.Update(currentEntity);
        }

        public void DeleteFWBStatus(FWBStatus entity)
        {
            FWBStatusRepository.Remove(entity);
        }

    }
}