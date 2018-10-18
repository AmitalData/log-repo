using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityLists;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.CustomFilters;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public IQueryable<RolePM> GetRolesByTenant(int tenant)
        {
           // SecurityUtility.AuthenticationOnTenant(tenant);
            

            roleRepository = new RoleRepository(tenant);
            RoleQuery roleQuery = new RoleQuery(roleRepository);
            return roleQuery.GetRolePMsByTenant(tenant);
        }

        public List<RolePM> GetRolesForUser(string userId, int tenant)
        {
          //  SecurityUtility.AuthenticationOnTenant(tenant);

            roleRepository = new RoleRepository(tenant);
            RoleQuery roleQuery = new RoleQuery(roleRepository);
            return roleQuery.GetRolesByUser(userId, tenant);
        }

        public RolePM GetSingleRolePM(string Id, int tenant)
        {
            //  SecurityUtility.AuthenticationOnTenant(tenant);

            roleRepository = new RoleRepository(tenant);
            RoleQuery roleQuery = new RoleQuery(roleRepository);
            return roleQuery.GetSinglePM(Id, tenant);
        }

        public void UpdateRoleList(RoleList entityList)
        {

        }

        public RoleList GetSingleRoleList(string id, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Role", "READ", tenant);

            roleRepository = new RoleRepository(tenant);
            Role entity = roleRepository.GetSingleRole(id, tenant);
            
            RoleList entityList = null;
            RoleQuery entityQuery = new RoleQuery(roleRepository);

            if (entity != null)
            {
                List<Role> singleEntityList = new List<Role>();
                singleEntityList.Add(entity);

                IQueryable<Role> iQueryable = singleEntityList.AsQueryable();
                IQueryable<RoleList> iQueryableEntityList = entityQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<RoleList> GetRoleLists(int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Role", "READ", tenant);

            roleRepository = new RoleRepository(tenant);
            IQueryable<Role> allEtnties = roleRepository.GetRoles(tenant);

            RoleQuery entityQuery = new RoleQuery(roleRepository);
            IQueryable<RoleList> query2 = entityQuery.GetIQueryableEntityList(allEtnties);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public List<RoleList> GetRoleFilters(byte[] xmlFilters, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Role", "READ", tenant);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            roleRepository = new RoleRepository(tenant);
            IQueryable<Role> allRoles = roleRepository.GetRoles(tenant);

            RoleCustomFilter myCustomfilters = new RoleCustomFilter(tenant);
            allRoles = myCustomfilters.GetFilteredQuery(queryOperations, allRoles);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            allRoles = filter.GetFilteredQuery<Role>(nonListQueryOperation, allRoles);

            int skippedPorts = queryOperations.PageIndex;

            RoleQuery roleQuery = new RoleQuery(roleRepository);
            IQueryable<RoleList> query2 = roleQuery.GetIQueryableEntityList(allRoles);
            query2 = filter.GetFilteredQuery<RoleList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RoleList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Role", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<RoleList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RoleList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RoleList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RoleList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<RoleList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RoleList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderBy(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            
            return query2.ToList();
        }

        public int GetRoleFiltersCount(byte[] xmlFilters, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Role", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            roleRepository = new RoleRepository(tenant);
            IQueryable<Role> allRoles = roleRepository.GetRoles(tenant);

            RoleCustomFilter myCustomfilters = new RoleCustomFilter(tenant);
            allRoles = myCustomfilters.GetFilteredQuery(queryOperations, allRoles);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            allRoles = filter.GetFilteredQuery<Role>(nonListQueryOperation, allRoles);

            RoleQuery roleQuery = new RoleQuery(roleRepository);
            IQueryable<RoleList> query2 = roleQuery.GetIQueryableEntityList(allRoles);
            query2 = filter.GetFilteredQuery<RoleList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertRole(RolePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            RoleService service = new RoleService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateRole(RolePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.CurrentTenant);
            }

            RoleService service = new RoleService(objectContext, entityPM.CurrentTenant);
            service.Update(entityPM);
        }

        public void DeleteRole(RolePM role)
        {
            //roleRepository = new RoleRepository(role.CurrentTenant);
            //Role entity = roleRepository.GetSingleRole(role.Id, role.Tenant);
            //roleRepository.Remove(entity);
        }
    }
}