using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdatePartnerTypeList(PartnerTypeList currentEntity)
        {
        }

        public IQueryable<PartnerType> GetPartnerTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeRepository = new PartnerTypeRepository(tenant);
            return partnerTypeRepository.GetPartnerTypes();
        }

        public IQueryable<PartnerTypePM> GetPartnerTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeQuery = new PartnerTypeQuery(tenant);
            return partnerTypeQuery.GetPartnerTypePMs();
        }

        public IQueryable<PartnerTypePM> GetFirstPartnerTypes(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeQuery = new PartnerTypeQuery(tenant);
            input = input.ToUpper();
            return partnerTypeQuery.GetPartnerTypePMs().Where(p => p.Id.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public IQueryable<PartnerTypePM> GetPartnerTypesByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeQuery = new PartnerTypeQuery(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return partnerTypeQuery.GetPartnerTypePMs().Where(d => d.Id.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return partnerTypeQuery.GetPartnerTypePMs().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return partnerTypeQuery.GetPartnerTypePMs();
            }
        }

        public IQueryable<PartnerTypePM> GetSinglePartnerTypeByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeQuery = new PartnerTypeQuery(tenant);
            if (byCode)
            {
                return partnerTypeQuery.GetPartnerTypePMs().Where(d => d.Id.ToUpper() == input.ToUpper());
            }
            else
            {
                return partnerTypeQuery.GetPartnerTypePMs().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public PartnerTypePM GetSinglePartnerType(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeQuery = new PartnerTypeQuery(tenant);
            return partnerTypeQuery.GetSinglePM(id);
        }

        public PartnerTypeList GetSinglePartnerTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeRepository = new PartnerTypeRepository(tenant);
            PartnerTypeList entityList = null;
            PartnerType entity = partnerTypeRepository.GetSinglePartnerType(id);
            
            if (entity != null)
            {
                List<PartnerType> singleEntityList = new List<PartnerType>();
                singleEntityList.Add(entity);

                partnerTypeQuery = new PartnerTypeQuery(partnerTypeRepository);
                IQueryable<PartnerType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PartnerTypeList> iQueryableEntityList = partnerTypeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }
            return entityList;
        }

        public IQueryable<PartnerTypeList> GetPartnerTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeRepository = new PartnerTypeRepository(tenant);
            partnerTypeQuery = new PartnerTypeQuery(partnerTypeRepository);

            IQueryable<PartnerType> iQueryable = partnerTypeRepository.GetPartnerTypes();
            IQueryable<PartnerTypeList> query2 = partnerTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PartnerTypeList> GetPartnerTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeRepository = new PartnerTypeRepository(tenant);
            partnerTypeQuery = new PartnerTypeQuery(partnerTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PartnerType> iQueryable = partnerTypeRepository.GetPartnerTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PartnerType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PartnerTypeList> query2 = partnerTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<PartnerTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PartnerTypeList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<PartnerTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PartnerTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PartnerTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PartnerTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PartnerTypeList, bool>(queryOperations, query2);
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

        public int GetPartnerTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            partnerTypeRepository = new PartnerTypeRepository(tenant);
            partnerTypeQuery = new PartnerTypeQuery(partnerTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PartnerType> iQueryable = partnerTypeRepository.GetPartnerTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PartnerType>(nonListQueryOperation, iQueryable);

            IQueryable<PartnerTypeList> query2 = partnerTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<PartnerTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void MapPartnerTypePMPartnerType(PartnerTypePM partnerTypePM, PartnerType partnerType)
        {
            partnerType.Name = partnerTypePM.Name;
            partnerType.SearchFields = partnerTypePM.Id + "," + partnerTypePM.Name;
        }

        public void InsertPartnerType(PartnerTypePM entity)
        {
            PartnerType newEntity = new PartnerType();
            newEntity.Id = entity.Id;
            MapPartnerTypePMPartnerType(entity, newEntity);
            partnerTypeRepository.Add(newEntity);
        }

        public void UpdatePartnerType(PartnerTypePM currentEntity)
        {
            PartnerType entity = partnerTypeRepository.GetSinglePartnerType(currentEntity.Id);
            MapPartnerTypePMPartnerType(currentEntity, entity);
            partnerTypeRepository.Update(entity);
        }

        public void DeletePartnerType(PartnerTypePM entityPm)
        {
            PartnerType entity = partnerTypeRepository.GetSinglePartnerType(entityPm.Id);
            partnerTypeRepository.Remove(entity);
        }
    }
}