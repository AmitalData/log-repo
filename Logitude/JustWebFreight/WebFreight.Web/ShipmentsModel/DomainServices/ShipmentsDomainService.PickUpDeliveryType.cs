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
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private PickUpDeliveryTypeRepository pickUpDeliveryTypeRepository;
        private PickUpDeliveryTypeQuery pickUpDeliveryTypeQuery;

        public IQueryable<PickUpDeliveryType> GetPickUpDeliveryTypesByTenant(int tenant)
        {
            pickUpDeliveryTypeRepository = new PickUpDeliveryTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return pickUpDeliveryTypeRepository.GetPickUpDeliveryTypes();
        }

        public PickUpDeliveryTypePM GetSinglePickUpDeliveryTypePM(string code, int tenant)
        {
            pickUpDeliveryTypeQuery = new PickUpDeliveryTypeQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return pickUpDeliveryTypeQuery.GetSinglePickUpDeliveryTypePM(code);
        }

        public PickUpDeliveryType GetSinglePickUpDeliveryType(string code, int tenant)
        {
            pickUpDeliveryTypeRepository = new PickUpDeliveryTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return pickUpDeliveryTypeRepository.GetSinglePickUpDeliveryType(code);
        }

        public PickUpDeliveryTypeList GetSinglePickUpDeliveryTypeList(string code, int tenant)
        {
            pickUpDeliveryTypeRepository = new PickUpDeliveryTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            PickUpDeliveryType entityPM = pickUpDeliveryTypeRepository.GetSinglePickUpDeliveryType(code);
            PickUpDeliveryTypeList entityList = new PickUpDeliveryTypeList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
            };
            return entityList;
        }

        public IQueryable<PickUpDeliveryTypeList> GetPickUpDeliveryTypeLists(int tenant)
        {
            pickUpDeliveryTypeRepository = new PickUpDeliveryTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<PickUpDeliveryType> iQueryable = pickUpDeliveryTypeRepository.GetPickUpDeliveryTypes();
            var query2 = from entity in iQueryable
                         select new PickUpDeliveryTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PickUpDeliveryTypeList> GetPickUpDeliveryTypeFilters(byte[] xmlFilters, int tenant)
        {
            pickUpDeliveryTypeRepository = new PickUpDeliveryTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PickUpDeliveryType> iQueryable = pickUpDeliveryTypeRepository.GetPickUpDeliveryTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PickUpDeliveryType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new PickUpDeliveryTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<PickUpDeliveryTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PickUpDeliveryTypeList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PickUpDeliveryTypeList, bool>(queryOperations, query2);
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

        public int GetPickUpDeliveryTypeCount(byte[] xmlFilters, int tenant)
        {
            pickUpDeliveryTypeRepository = new PickUpDeliveryTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PickUpDeliveryType> iQueryable = pickUpDeliveryTypeRepository.GetPickUpDeliveryTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PickUpDeliveryType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new PickUpDeliveryTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<PickUpDeliveryTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPickUpDeliveryType(PickUpDeliveryType entity)
        {
            pickUpDeliveryTypeRepository.Add(entity);
        }

        public void UpdatePickUpDeliveryType(PickUpDeliveryType currentEntity)
        {
            pickUpDeliveryTypeRepository.Update(currentEntity);
        }

        public void DeletePickUpDeliveryType(PickUpDeliveryType entity)
        {
            pickUpDeliveryTypeRepository.Remove(entity);
        }

    }
}