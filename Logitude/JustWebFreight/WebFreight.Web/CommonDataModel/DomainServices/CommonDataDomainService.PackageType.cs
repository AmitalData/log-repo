using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdatePackageTypeList(PackageTypeList currentEntity)
        {
        }

        public IQueryable<PackageType> GetPackageTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeRepository = new PackageTypeRepository(tenant);
            return packageTypeRepository.GetPackageTypes(0);
        }

        public PackageTypePM GetSinglePackageType(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeQuery = new PackageTypeQuery(tenant);
            return packageTypeQuery.GetSinglePM(id, tenant);
        }

        public PackageTypeList GetSinglePackageTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeRepository = new PackageTypeRepository(tenant);
            PackageTypeList entityList = null;
            PackageType entity = packageTypeRepository.GetSinglePackageType(id, tenant);

            if (entity != null)
            {
                List<PackageType> singleEntityList = new List<PackageType>();
                singleEntityList.Add(entity);

                packageTypeQuery = new PackageTypeQuery(packageTypeRepository);
                IQueryable<PackageType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PackageTypeList> iQueryableEntityList = packageTypeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }
            return entityList;
        }

        public IQueryable<PackageTypeList> GetPackageTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeRepository = new PackageTypeRepository(tenant);
            packageTypeQuery = new PackageTypeQuery(packageTypeRepository);

            IQueryable<PackageType> iQueryable = packageTypeRepository.GetPackageTypes(tenant);
            IQueryable<PackageTypeList> query2 = packageTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PackageTypeList> GetPackageTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeRepository = new PackageTypeRepository(tenant);
            packageTypeQuery = new PackageTypeQuery(packageTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PackageType> iQueryable = packageTypeRepository.GetPackageTypes(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            PackageTypeCustomFilter customfilters = new PackageTypeCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);
            iQueryable = filter.GetFilteredQuery<PackageType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PackageTypeList> query2 = packageTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<PackageTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PackageTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PackageType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PackageTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PackageTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PackageTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PackageTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PackageTypeList, bool>(queryOperations, query2);
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

        public int GetPackageTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeRepository = new PackageTypeRepository(tenant);
            packageTypeQuery = new PackageTypeQuery(packageTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PackageType> iQueryable = packageTypeRepository.GetPackageTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PackageType>(nonListQueryOperation, iQueryable);

            IQueryable<PackageTypeList> query2 = packageTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PackageTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<PackageTypePM> GetPackageTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeQuery = new PackageTypeQuery(tenant);
            return packageTypeQuery.GetPackageTypePMsByTenant(tenant);
        }

        public IQueryable<PackageTypePM> GetPackageTypesByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeQuery = new PackageTypeQuery(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return packageTypeQuery.GetPackageTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return packageTypeQuery.GetPackageTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return packageTypeQuery.GetPackageTypePMsByTenant(tenant).Where(d => d.Tenant == tenant);
            }
        }

        public IQueryable<PackageTypePM> GetSinglePackageTypeByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PackageType", "READ", tenant);

            packageTypeQuery = new PackageTypeQuery(tenant);
            if (byCode)
            {
                return packageTypeQuery.GetPackageTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return packageTypeQuery.GetPackageTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertPackageType(PackageTypePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("PackageType", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            PackageTypeService service = new PackageTypeService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdatePackageType(PackageTypePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("PackageType", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            PackageTypeService service = new PackageTypeService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeletePackageType(PackageTypePM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            PackageType deletedEntity = PackageTypeRepository.GetSinglePackageType(entity.Id, entity.Tenant, false);
            if (deletedEntity != null)
            {
                packageTypeRepository.Remove(deletedEntity);
            }
        }
    }
}