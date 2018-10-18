using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private AWBStatusQuery AWBStatusQuery;
        private AWBStatusRepository AWBStatusRepository;

        public void UpdateAWBStatusList(AWBStatusList awbStatus)
        {

        }

        public IQueryable<AWBStatus> GetAWBStatus(int tenant)
        {
            AWBStatusRepository = new AWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return AWBStatusRepository.GetAWBStatus();
        }

        public IQueryable<AWBStatus> GetAWBStatusByTenant(int tenant)
        {
            AWBStatusRepository = new AWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return AWBStatusRepository.GetAWBStatus();
        }

        public IQueryable<AWBStatus> GetFirstAWBStatus(string input, int tenant)
        {
            AWBStatusRepository = new AWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            input = input.ToUpper();
            return AWBStatusRepository.GetAWBStatus().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public AWBStatusPM GetSingleAWBStatus(string code, int tenant)
        {
            AWBStatusQuery = new AWBStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return AWBStatusQuery.GetSingleAWBStatusPM(code);
        }

        public AWBStatusList GetSingleAWBStatusList(string code, int tenant)
        {
            AWBStatusQuery = new AWBStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            AWBStatusPM entityPM = AWBStatusQuery.GetSingleAWBStatusPM(code);
            AWBStatusList entityList = new AWBStatusList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
            };
            return entityList;
        }

        public IQueryable<AWBStatusList> GetAWBStatusLists(int tenant)
        {
            AWBStatusRepository = new AWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<AWBStatus> iQueryable = AWBStatusRepository.GetAWBStatus();
            var query2 = from entity in iQueryable
                         select new AWBStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBStatusList> GetAWBStatusFilters(byte[] xmlFilters, int tenant)
        {
            AWBStatusRepository = new AWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBStatus> iQueryable = AWBStatusRepository.GetAWBStatus();

            //PortCustomFilter customfilters = new PortCustomFilter();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new AWBStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBStatusList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ShipmentType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AWBStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBStatusList, bool>(queryOperations, query2);
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

        public int GetAWBStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            AWBStatusRepository = new AWBStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBStatus> iQueryable = AWBStatusRepository.GetAWBStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new AWBStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAWBStatus(AWBStatus entity)
        {
            AWBStatusRepository.Add(entity);
        }

        public void UpdateAWBStatus(AWBStatus currentEntity)
        {
            AWBStatusRepository.Update(currentEntity);
        }

        public void DeleteAWBStatus(AWBStatus entity)
        {
            AWBStatusRepository.Remove(entity);
        }

    }
}