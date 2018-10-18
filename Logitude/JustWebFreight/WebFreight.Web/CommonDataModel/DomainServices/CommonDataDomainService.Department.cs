using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
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
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateDepartmentList(DepartmentList currentEntity)
        {
        }

        public IQueryable<Department> GetDepartments(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            departmentRepository = new DepartmentRepository(tenant);
            return departmentRepository.GetDepartments(0);
        }

        public IQueryable<DepartmentPM> GetDepartmentsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            DepartmentQuery departmentQuery = new DepartmentQuery(tenant);
            return departmentQuery.GetDepartmentPMsByTenant(tenant);
        }

        public DepartmentPM GetSingleDepartment(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            DepartmentQuery departmentQuery = new DepartmentQuery(tenant);
            return departmentQuery.GetSinglePM(id, tenant);
        }

        public DepartmentPM GetDepartmentById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            DepartmentQuery departmentQuery = new DepartmentQuery(tenant);
            DepartmentPM department = departmentQuery.GetSinglePM(id, tenant);
            return department;
        }

        public DepartmentList GetSingleDepartmentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            departmentRepository = new DepartmentRepository(tenant);
            DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            DepartmentList departmentList = null;
            Department department = departmentRepository.GetSingleDepartment(id, tenant);

            if (department != null)
            {
                List<Department> singleEntityList = new List<Department>();
                singleEntityList.Add(department);

                IQueryable<Department> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DepartmentList> iQueryableEntityList = departmentQuery.GetIQueryableEntityList(iQueryable);
                departmentList = iQueryableEntityList.FirstOrDefault();
            }
            return departmentList;
        }

        public IQueryable<DepartmentList> GetDepartmentLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            departmentRepository = new DepartmentRepository(tenant);
            DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            IQueryable<Department> departments = departmentRepository.GetDepartments(tenant);
            IQueryable<DepartmentList> query2 = departmentQuery.GetIQueryableEntityList(departments);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DepartmentList> GetDepartmentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            departmentRepository = new DepartmentRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Department> departments = departmentRepository.GetDepartments(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            departments = filter.GetFilteredQuery<Department>(nonListQueryOperation, departments);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            IQueryable<DepartmentList> query2 = departmentQuery.GetIQueryableEntityList(departments);

            query2 = filter.GetFilteredQuery<DepartmentList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DepartmentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Department", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DepartmentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DepartmentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DepartmentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DepartmentList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DepartmentList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }
            
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetDepartmentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Department", "READ", tenant);

            departmentRepository = new DepartmentRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Department> departments = departmentRepository.GetDepartments(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            departments = filter.GetFilteredQuery<Department>(nonListQueryOperation, departments);
            DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            IQueryable<DepartmentList> query2 = departmentQuery.GetIQueryableEntityList(departments);

            query2 = filter.GetFilteredQuery<DepartmentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapDepartmentDepartmentPM(DepartmentPM departmentPM, Department department)
        //{
        //    department.EnglishName = departmentPM.EnglishName;
        //    department.InActive = departmentPM.InActive;
        //    department.LocalName = departmentPM.LocalName;
        //    department.Notes = departmentPM.Notes;
        //    department.Tenant = departmentPM.Tenant;
        //    department.SearchFields = departmentPM.EnglishName + "," + departmentPM.LocalName;
        //}

        public void InsertDepartment(DepartmentPM department)
        {
            SecurityUtility.CheckContactFeature("Department", "NEW", department.Tenant);
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(department.Tenant);
            }
            DepartmentService service = new DepartmentService(objectContext, department.Tenant);
            service.Create(department);

            TableLastUpdateClass.UpdateTableHistory(department.Tenant, "Department");
            //if (objectContext == null)
            //{
            //    objectContext = CommonDataContext.GetContext(department.Tenant);
            //}
            //departmentRepository = new DepartmentRepository(objectContext);

            //department.Id = IdCounter.GetNumber("Department", department.Tenant).ToString();
            //Department newDepartment = new Department();
            //newDepartment.Id = department.Id;
            //MapDepartmentDepartmentPM(department, newDepartment);
            //departmentRepository.Add(newDepartment);
            
            //create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), department.Tenant, true);
            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "CRDP", department.Tenant, contact.Id, department.Id, null, "Department", null, null, false);
            
            
        }

        public void UpdateDepartment(DepartmentPM currentdepartment)
        {
            SecurityUtility.CheckContactFeature("Department", "UPDATE", currentdepartment.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentdepartment.Tenant);
            }

            string entityName = "Department" + currentdepartment.Id + currentdepartment.Tenant;
            string entityPmName = "DepartmentPM" + currentdepartment.Id + currentdepartment.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            DepartmentService service = new DepartmentService(objectContext, currentdepartment.Tenant);
            service.Update(currentdepartment);
            TableLastUpdateClass.UpdateTableHistory(currentdepartment.Tenant, "Department");

            //departmentRepository = new DepartmentRepository(objectContext);
          
            //string entityName = "Department" + currentdepartment.Id + currentdepartment.Tenant;
            //string entityPmName = "DepartmentPM" + currentdepartment.Id + currentdepartment.Tenant;
            //if (CacheManager.CacheWrapper.Get(entityName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(entityName);
            //}
            //if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(entityPmName);
            //}
            ////DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            //Department entity = departmentRepository.GetSingleDepartment(currentdepartment.Id, currentdepartment.Tenant, false);
            //MapDepartmentDepartmentPM(currentdepartment, entity);
            //departmentRepository.Update(entity);
            
            ////create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), currentdepartment.Tenant, true);
            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "UPDP", currentdepartment.Tenant, contact.Id, currentdepartment.Id, null, "Department", null, null, false);
            
            //TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "Department");
        }

        public void DeleteDepartment(DepartmentPM department)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(department.Tenant);
            }

            departmentRepository = new DepartmentRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            Department entity = departmentRepository.GetSingleDepartment(department.Id, department.Tenant);
            departmentRepository.Remove(entity);
        }
    }
}