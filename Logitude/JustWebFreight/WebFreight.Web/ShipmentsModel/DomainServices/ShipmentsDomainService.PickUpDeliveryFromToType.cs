using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private PickUpDeliveryFromToTypeRepository pickUpDeliveryFromToTypeRepository;
        private PickUpDeliveryFromToTypeQuery pickUpDeliveryFromToTypeQuery;

        public IQueryable<PickUpDeliveryFromToType> GetPickUpDeliveryFromToTypesByTenant(int tenant)
        {
            pickUpDeliveryFromToTypeRepository = new PickUpDeliveryFromToTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return pickUpDeliveryFromToTypeRepository.GetPickUpDeliveryFromToTypes();
        }

        public PickUpDeliveryFromToTypePM GetSinglePickUpDeliveryFromToTypePM(string code, int tenant)
        {
            pickUpDeliveryFromToTypeQuery = new PickUpDeliveryFromToTypeQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return pickUpDeliveryFromToTypeQuery.GetSinglePickUpDeliveryFromToTypePM(code);
        }

        public PickUpDeliveryFromToType GetSinglePickUpDeliveryFromToType(string code, int tenant)
        {
            pickUpDeliveryFromToTypeRepository = new PickUpDeliveryFromToTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return pickUpDeliveryFromToTypeRepository.GetSinglePickUpDeliveryFromToType(code);
        }

        public PickUpDeliveryFromToTypeList GetSinglePickUpDeliveryFromToTypeList(string code, int tenant)
        {
            pickUpDeliveryFromToTypeRepository = new PickUpDeliveryFromToTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            PickUpDeliveryFromToType entityPM = pickUpDeliveryFromToTypeRepository.GetSinglePickUpDeliveryFromToType(code);
            PickUpDeliveryFromToTypeList entityList = new PickUpDeliveryFromToTypeList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
            };
            return entityList;
        }

        public IQueryable<PickUpDeliveryFromToTypeList> GetPickUpDeliveryFromToTypeLists(int tenant)
        {
            pickUpDeliveryFromToTypeRepository = new PickUpDeliveryFromToTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<PickUpDeliveryFromToType> iQueryable = pickUpDeliveryFromToTypeRepository.GetPickUpDeliveryFromToTypes();
            var query2 = from entity in iQueryable
                         select new PickUpDeliveryFromToTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PickUpDeliveryFromToTypeList> GetPickUpDeliveryFromToTypeFilters(byte[] xmlFilters, int tenant)
        {
            pickUpDeliveryFromToTypeRepository = new PickUpDeliveryFromToTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PickUpDeliveryFromToType> iQueryable = pickUpDeliveryFromToTypeRepository.GetPickUpDeliveryFromToTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PickUpDeliveryFromToType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new PickUpDeliveryFromToTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<PickUpDeliveryFromToTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PickUpDeliveryFromToTypeList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PartnerType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryFromToTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryFromToTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryFromToTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryFromToTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryFromToTypeList, bool>(queryOperations, query2);
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

        public int GetPickUpDeliveryFromToTypeCount(byte[] xmlFilters, int tenant)
        {
            pickUpDeliveryFromToTypeRepository = new PickUpDeliveryFromToTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PickUpDeliveryFromToType> iQueryable = pickUpDeliveryFromToTypeRepository.GetPickUpDeliveryFromToTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PickUpDeliveryFromToType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new PickUpDeliveryFromToTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<PickUpDeliveryFromToTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPickUpDeliveryFromToType(PickUpDeliveryFromToType entity)
        {
            pickUpDeliveryFromToTypeRepository.Add(entity);
        }

        public void UpdatePickUpDeliveryFromToType(PickUpDeliveryFromToType currentEntity)
        {
            pickUpDeliveryFromToTypeRepository.Update(currentEntity);
        }

        public void DeletePickUpDeliveryFromToType(PickUpDeliveryFromToType entity)
        {
            pickUpDeliveryFromToTypeRepository.Remove(entity);
        }

    }
}