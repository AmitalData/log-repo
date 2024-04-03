using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using WebFreight.Web.Security;


namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private CustomerTeamQuery customerTeamQuery;
        private CustomerTeamRepository customerTeamRepository;

        public IQueryable<CustomerTeam> GetCustomerTeams(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTeamRepository = new CustomerTeamRepository(tenant);
            return customerTeamRepository.GetCustomerTeams();
        }

        public IQueryable<CustomerTeam> GetCustomerTeamsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTeamRepository = new CustomerTeamRepository(tenant);
            return customerTeamRepository.GetCustomerTeams();
        }

        public void InsertCustomerTeam(CustomerTeam customerTeam)
        {
            customerTeamRepository.Add(customerTeam);
        }

        public void UpdateCustomerTeam(CustomerTeam currentcustomerTeam)
        {
            customerTeamRepository.Update(currentcustomerTeam);
        }

        public void DeleteCustomerTeam(CustomerTeam customerTeam)
        {
            customerTeamRepository.Remove(customerTeam);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerTeamList> GetCustomerTeamFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTeamRepository = new CustomerTeamRepository(tenant);
            customerTeamQuery = new CustomerTeamQuery(customerTeamRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTeam> CustomerTeams = customerTeamRepository.GetCustomerTeams();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerTeams = filter.GetFilteredQuery<CustomerTeam>(nonListQueryOperation, CustomerTeams);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<CustomerTeamList> query2 = customerTeamQuery.GetIQueryableEntityList(CustomerTeams);
            query2 = filter.GetFilteredQuery<CustomerTeamList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerTeamList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerTeam", tenant).ToList();
                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTeamList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTeamList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTeamList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTeamList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTeamList, bool>(queryOperations, query2);
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

        public int GetCustomerTeamFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTeamRepository = new CustomerTeamRepository(tenant);
            customerTeamQuery = new CustomerTeamQuery(customerTeamRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTeam> CustomerTeams = customerTeamRepository.GetCustomerTeams();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerTeams = filter.GetFilteredQuery<CustomerTeam>(nonListQueryOperation, CustomerTeams);

            IQueryable<CustomerTeamList> query2 = customerTeamQuery.GetIQueryableEntityList(CustomerTeams);

            query2 = filter.GetFilteredQuery<CustomerTeamList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}