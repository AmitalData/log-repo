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
        public IQueryable<PasswordPolicy> GetPasswordPolicys(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            passwordPolicyRepository = new PasswordPolicyRepository(tenant);
            return passwordPolicyRepository.GetPasswordPolicies();
        }

        public IQueryable<PasswordPolicy> PasswordPolicysByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            passwordPolicyRepository = new PasswordPolicyRepository(tenant);
            return passwordPolicyRepository.GetPasswordPolicies();
        }

        public PasswordPolicyPM GetSinglePasswordPolicy(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            passwordPolicyQuery = new PasswordPolicyQuery(tenant);
            return passwordPolicyQuery.GetSinglePasswordPolicyPM(code);
        }

        public PasswordPolicyList GetSinglePasswordPolicyList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            passwordPolicyRepository = new PasswordPolicyRepository(tenant);
            PasswordPolicyList entityList = null;
            PasswordPolicy entity = passwordPolicyRepository.GetSinglePasswordPolicy(code);

            if (entity != null)
            {
                List<PasswordPolicy> singleEntityList = new List<PasswordPolicy>();
                singleEntityList.Add(entity);

                passwordPolicyQuery = new PasswordPolicyQuery(passwordPolicyRepository);
                IQueryable<PasswordPolicy> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PasswordPolicyList> iQueryableEntityList = passwordPolicyQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }
            return entityList;
        }

        public IQueryable<PasswordPolicyList> GetPasswordPoliciesLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            passwordPolicyRepository = new PasswordPolicyRepository(tenant);
            passwordPolicyQuery = new PasswordPolicyQuery(passwordPolicyRepository);

            IQueryable<PasswordPolicy> iQueryable = passwordPolicyRepository.GetPasswordPolicies();
            IQueryable<PasswordPolicyList> query2 = passwordPolicyQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PasswordPolicyList> GetPasswordPolicyFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            passwordPolicyRepository = new PasswordPolicyRepository(tenant);
            passwordPolicyQuery = new PasswordPolicyQuery(passwordPolicyRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PasswordPolicy> iQueryable = passwordPolicyRepository.GetPasswordPolicies();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PasswordPolicy>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PasswordPolicyList> query2 = passwordPolicyQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<PasswordPolicyList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PasswordPolicyList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PasswordPolicy", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PasswordPolicyList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PasswordPolicyList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PasswordPolicyList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PasswordPolicyList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PasswordPolicyList, bool>(queryOperations, query2);
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

        public int GetPasswordPolicyFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            passwordPolicyRepository = new PasswordPolicyRepository(tenant);
            passwordPolicyQuery = new PasswordPolicyQuery(passwordPolicyRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PasswordPolicy> iQueryable = passwordPolicyRepository.GetPasswordPolicies();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PasswordPolicy>(nonListQueryOperation, iQueryable);

            IQueryable<PasswordPolicyList> query2 = passwordPolicyQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<PasswordPolicyList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPasswordPolicy(PasswordPolicy entity)
        {
            passwordPolicyRepository.Add(entity);
        }

        public void UpdatePasswordPolicy(PasswordPolicy currentEntity)
        {
            passwordPolicyRepository.Update(currentEntity);
        }

        public void UpdatePasswordPolicyList(PasswordPolicyList currentEntity)
        {

        }

        public void DeletePasswordPolicy(PasswordPolicy entity)
        {
            passwordPolicyRepository.Remove(entity);
        }
    }
}