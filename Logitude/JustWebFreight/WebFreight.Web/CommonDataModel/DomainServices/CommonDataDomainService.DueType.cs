using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateDueTypeList(DueTypeList currentEntity)
        {
        }

        public IQueryable<DueType> GetDueTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            return dueTypeRepository.GetDueTypes();
        }

        public IQueryable<DueType> GetDueTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            return dueTypeRepository.GetDueTypes();
        }

        public DueTypePM GetSingleDueType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeQuery = new DueTypeQuery(tenant);
            return dueTypeQuery.GetSingleDueTypePM(code);
        }

        public DueTypeList GetSingleDueTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            DueTypeList dueTypeList = null;
            DueType dueType = dueTypeRepository.GetSingleDueTypeUpdate(code, tenant);

            if (dueType != null)
            {
                List<DueType> singleEntityList = new List<DueType>();
                singleEntityList.Add(dueType);

                dueTypeQuery = new DueTypeQuery(dueTypeRepository);
                IQueryable<DueType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DueTypeList> iQueryableEntityList = dueTypeQuery.GetIQueryableEntityList(iQueryable);
                dueTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return dueTypeList;
        }

        public IQueryable<DueTypeList> GetDueTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            dueTypeQuery = new DueTypeQuery(dueTypeRepository);

            IQueryable<DueType> iQueryable = dueTypeRepository.GetDueTypes();
            IQueryable<DueTypeList> query2 = dueTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DueTypeList> GetDueTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            dueTypeQuery = new DueTypeQuery(dueTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DueType> iQueryable = dueTypeRepository.GetDueTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DueType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DueTypeList> query2 = dueTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DueTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DueTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DueType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DueTypeList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<DueTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DueTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DueTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DueTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DueTypeList, bool>(queryOperations, query2);
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

        public int GetDueTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            dueTypeQuery = new DueTypeQuery(dueTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DueType> iQueryable = dueTypeRepository.GetDueTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DueType>(nonListQueryOperation, iQueryable);

            IQueryable<DueTypeList> query2 = dueTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DueTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<DueType> GetFirstDueTypes(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            input = input.ToUpper();
            return dueTypeRepository.GetDueTypes().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public IQueryable<DueType> GetDueTypesByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return dueTypeRepository.GetDueTypes().Where(d => d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return dueTypeRepository.GetDueTypes().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return dueTypeRepository.GetDueTypes();
            }
        }

        public IQueryable<DueType> GetSingleDueTypeByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dueTypeRepository = new DueTypeRepository(tenant);
            if (byCode)
            {
                return dueTypeRepository.GetDueTypes().Where(d => d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return dueTypeRepository.GetDueTypes().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertDueType(DueType entity)
        {
            dueTypeRepository.Add(entity);
        }

        public void UpdateDueType(DueType currentEntity)
        {
            dueTypeRepository.Update(currentEntity);
        }

        public void DeleteDueType(DueType entity)
        {
            dueTypeRepository.Remove(entity);
        }
    }
}