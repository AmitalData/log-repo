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
using Simplog.Server.Infrastructure;
using System.Transactions;
using WebFreight.Web.TopicQueues;
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
        public void UpdateBranchList(BranchList currentEntity)
        {
        }

        //public IQueryable<Branch> GetBranches(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

        //    branchRepository = new BranchRepository(tenant);
        //    return branchRepository.GetBranches(0);
        //}

        public IQueryable<BranchPM> GetBranchesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

            //branchRepository = new BranchRepository(tenant);
            BranchQuery branchQuery = new BranchQuery(tenant);
            return branchQuery.GetBranchPMsByTenant(tenant);
        }

        public BranchPM GetSingleBranch(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

            //branchRepository = new BranchRepository(tenant);
            BranchQuery branchQuery = new BranchQuery(tenant);
            return branchQuery.GetSinglePM(id, tenant,false);
        }

        public BranchPM GetBranchById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

            //branchRepository = new BranchRepository(tenant);

            //BranchPM branch = branchRepository.GetSinglePM(id, tenant);
            BranchQuery branchQuery = new BranchQuery(tenant);
            return branchQuery.GetSinglePM(id, tenant,false);
            //return branch;
        }

        public BranchList GetSingleBranchList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

            branchRepository = new BranchRepository(tenant);
            BranchQuery branchQuery = new BranchQuery(branchRepository);
            BranchList branchList = null;
            Branch branch = branchRepository.GetSingleBranch(id, tenant);

            if (branch != null)
            {
                List<Branch> singleEntityList = new List<Branch>();
                singleEntityList.Add(branch);

                IQueryable<Branch> iQueryable = singleEntityList.AsQueryable();
                IQueryable<BranchList> iQueryableEntityList = branchQuery.GetIQueryableEntityList(iQueryable);
                branchList = iQueryableEntityList.FirstOrDefault();
            }
            return branchList;
        }

        public IQueryable<BranchList> GetBranchLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

            branchRepository = new BranchRepository(tenant);
            BranchQuery branchQuery = new BranchQuery(branchRepository);
            IQueryable<Branch> branches = branchRepository.GetBranches(tenant);
            IQueryable<BranchList> query2 = branchQuery.GetIQueryableEntityList(branches);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<BranchList> GetBranchFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

            branchRepository = new BranchRepository(tenant);
            BranchQuery branchQuery = new BranchQuery(branchRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Branch> branches = branchRepository.GetBranches(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            branches = filter.GetFilteredQuery<Branch>(nonListQueryOperation, branches);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<BranchList> query2 = branchQuery.GetIQueryableEntityList(branches);

            query2 = filter.GetFilteredQuery<BranchList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BranchList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Branch", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BranchList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BranchList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BranchList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BranchList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BranchList, bool>(queryOperations, query2);
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
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetBranchFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", tenant);

            branchRepository = new BranchRepository(tenant);
            BranchQuery branchQuery = new BranchQuery(branchRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Branch> branches = branchRepository.GetBranches(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            branches = filter.GetFilteredQuery<Branch>(nonListQueryOperation, branches);

            IQueryable<BranchList> query2 = branchQuery.GetIQueryableEntityList(branches);

            query2 = filter.GetFilteredQuery<BranchList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapBranchBranchPM(BranchPM branchPM, Branch branch)
        //{
        //    branch.EnglishName = branchPM.EnglishName;
        //    branch.Id = branchPM.Id;
        //    branch.InActive = branchPM.InActive;
        //    branch.LocalName = branchPM.LocalName;
        //    branch.Notes = branchPM.Notes;
        //    branch.Tenant = branchPM.Tenant;
        //    branch.SearchFields = branchPM.EnglishName + "," + branchPM.LocalName;
        //}

        public void InsertBranch(BranchPM branch)
        {
            SecurityUtility.CheckContactFeature("Branch", "NEW", branch.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(branch.Tenant);
            }
            BranchService service = new BranchService(objectContext, branch.Tenant);
            service.Create(branch);
            
            //create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), branch.Tenant, true);

            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "CRBR", branch.Tenant, contact.Id, branch.Id, null, "Branch", null, null, false);
            
            TableLastUpdateClass.UpdateTableHistory(branch.Tenant, "Branch");
        }

        public void UpdateBranch(BranchPM currentbranch)
        {
            SecurityUtility.CheckContactFeature("Branch", "UPDATE", currentbranch.Tenant);
            
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentbranch.Tenant);
            }
           

            branchRepository = new BranchRepository(objectContext);

            string entityName = "Branch" + currentbranch.Id + currentbranch.Tenant;
            string entityPmName = "BranchPM" + currentbranch.Id + currentbranch.Tenant;
           
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            BranchService service = new BranchService(objectContext, currentbranch.Tenant);
            service.Update(currentbranch);
            TableLastUpdateClass.UpdateTableHistory(currentbranch.Tenant, "Branch");
            //Branch entity = branchRepository.GetSingleBranch(currentbranch.Id, currentbranch.Tenant, false);
            //MapBranchBranchPM(currentbranch, entity);
            //branchRepository.Update(entity);
           
            ////create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), currentbranch.Tenant, true);
            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "UPBR", currentbranch.Tenant, contact.Id, currentbranch.Id, null, "Branch", null, null, false);
         

        }

        public void DeleteBranch(BranchPM branch)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(branch.Tenant);
            }
            branchRepository = new BranchRepository(objectContext);
            Branch entity = branchRepository.GetSingleBranch(branch.Id, branch.Tenant);
            branchRepository.Remove(entity);
        }
    }
}