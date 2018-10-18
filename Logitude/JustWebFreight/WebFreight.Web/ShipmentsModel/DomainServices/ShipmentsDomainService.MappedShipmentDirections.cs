using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.Tools.EntityService;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private MappedShipmentDirectionsRepository mappedShipmentDirectionsRepository;
        private MappedShipmentDirectionsQuery mappedShipmentDirectionsQuery;

        public MappedShipmentDirectionsPM GetSingleMappedShipmentDirectionPM(string DirectionId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MappedShipmentDirections", "READ", tenant);

            mappedShipmentDirectionsQuery = new MappedShipmentDirectionsQuery(tenant);
            return mappedShipmentDirectionsQuery.GetSinglePM(tenant, DirectionId);
        }

        public MappedShipmentDirectionsList GetSingleMappedShipmentDirectionList(string DirectionId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MappedShipmentDirections", "READ", tenant);

            mappedShipmentDirectionsRepository = new MappedShipmentDirectionsRepository(tenant);
            mappedShipmentDirectionsQuery = new MappedShipmentDirectionsQuery(mappedShipmentDirectionsRepository);
            MappedShipmentDirectionsList MappedShipmentDirectionList = null;
            MappedShipmentDirections MappedShipmentDirection = mappedShipmentDirectionsRepository.GetSingleMappedShipmentDirection(tenant, DirectionId);

            if (MappedShipmentDirection != null)
            {
                List<MappedShipmentDirections> singleEntityList = new List<MappedShipmentDirections>();
                singleEntityList.Add(MappedShipmentDirection);

                IQueryable<MappedShipmentDirections> iQueryable = singleEntityList.AsQueryable();
                IQueryable<MappedShipmentDirectionsList> iQueryableEntityList = mappedShipmentDirectionsQuery.GetIQueryableEntityList(iQueryable);
                MappedShipmentDirectionList = iQueryableEntityList.FirstOrDefault();
            }
            return MappedShipmentDirectionList;
        }

        public IQueryable<MappedShipmentDirectionsList> GetMappedShipmentDirectionsLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MappedShipmentDirections", "READ", tenant);


            mappedShipmentDirectionsRepository = new MappedShipmentDirectionsRepository(tenant);
            mappedShipmentDirectionsQuery = new MappedShipmentDirectionsQuery(mappedShipmentDirectionsRepository);
            IQueryable<MappedShipmentDirections> mappedShipmentDirections = mappedShipmentDirectionsRepository.GetMappedShipmentDirections(tenant);
            IQueryable<MappedShipmentDirectionsList> query2 = mappedShipmentDirectionsQuery.GetIQueryableEntityList(mappedShipmentDirections).AsQueryable();
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<MappedShipmentDirectionsList> GetMappedShipmentDirectionsFilters(byte[] xmlFilters, int tenant)
        {
            mappedShipmentDirectionsRepository = new MappedShipmentDirectionsRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MappedShipmentDirections> iQueryable = mappedShipmentDirectionsRepository.GetMappedShipmentDirections(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MappedShipmentDirections>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new MappedShipmentDirectionsList()
                         {
                             ShipmentDirectionId = entity.ShipmentDirectionId,
                             Tenant = entity.Tenant,
                             UpdateDateTime = entity.UpdateDateTime
                         };

            query2 = filter.GetFilteredQuery<MappedShipmentDirectionsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(MappedShipmentDirectionsList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> MappedShipmentDirectionsObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("MappedShipmentDirections", tenant).ToList();

                ObjectField objectField = (from a in MappedShipmentDirectionsObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<MappedShipmentDirectionsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<MappedShipmentDirectionsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<MappedShipmentDirectionsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<MappedShipmentDirectionsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<MappedShipmentDirectionsList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.UpdateDateTime);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.UpdateDateTime);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetMappedShipmentDirectionsCount(byte[] xmlFilters, int tenant)
        {
            mappedShipmentDirectionsRepository = new MappedShipmentDirectionsRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MappedShipmentDirections> iQueryable = mappedShipmentDirectionsRepository.GetMappedShipmentDirections(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MappedShipmentDirections>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new MappedShipmentDirectionsList()
                         {
                             ShipmentDirectionId = entity.ShipmentDirectionId,
                             Tenant = entity.Tenant,
                             UpdateDateTime = entity.UpdateDateTime
                         };

            query2 = filter.GetFilteredQuery<MappedShipmentDirectionsList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void Insert(MappedShipmentDirectionsPM entityPM)
        {

            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("MappedShipmentDirections", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
            }

            MappedShipmentDirectionsService service = new MappedShipmentDirectionsService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            

        }

        public void Update(MappedShipmentDirectionsPM currentEntityPM)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntityPM.Tenant);
            SecurityUtility.CheckContactFeature("MappedShipmentDirections", "UPDATE", currentEntityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(currentEntityPM.Tenant);
            }
            MappedShipmentDirectionsService service = new MappedShipmentDirectionsService(objectContext, currentEntityPM.Tenant);
            service.Update(currentEntityPM);

        }

       

    }
}