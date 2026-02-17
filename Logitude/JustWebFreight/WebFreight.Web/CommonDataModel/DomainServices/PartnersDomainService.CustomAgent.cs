using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public bool DoesCustomAgentCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customAgentRepository = new CustomAgentRepository(tenant);
            return (customAgentRepository.GetCustomAgents(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<CustomAgent> GetCustomAgents(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentRepository = new CustomAgentRepository(tenant);
            return customAgentRepository.GetCustomAgents(0);
        }

        public IQueryable<CustomAgentPM> GetCustomAgentsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentQuery = new CustomAgentQuery(tenant);
            return customAgentQuery.GetCustomAgentPMsByTenant(tenant);
        }

        public IQueryable<CustomAgentPM> GetCustomAgentsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentQuery = new CustomAgentQuery(tenant);
            IQueryable<CustomAgentPM> q = customAgentQuery.GetCustomAgentsByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public CustomAgentPM GetCustomAgentById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentQuery = new CustomAgentQuery(tenant);
            return customAgentQuery.GetSinglePM(id, tenant);
        }

        public CustomAgentList GetSingleCustomAgentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentRepository = new CustomAgentRepository(tenant);
            CustomAgentList customAgentList = null;
            CustomAgent customAgent = customAgentRepository.GetSingleCustomAgent(tenant, id);

            if (customAgent != null)
            {
                List<CustomAgent> singleEntityList = new List<CustomAgent>();
                singleEntityList.Add(customAgent);

                customAgentQuery = new CustomAgentQuery(customAgentRepository);
                IQueryable<CustomAgent> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CustomAgentList> iQueryableEntityList = customAgentQuery.GetIQueryableEntityList(iQueryable);
                customAgentList = iQueryableEntityList.FirstOrDefault();
            }
            return customAgentList;
        }

        public IQueryable<CustomAgentList> GetCustomAgentLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentRepository = new CustomAgentRepository(tenant);
            customAgentQuery = new CustomAgentQuery(customAgentRepository);

            IQueryable<CustomAgent> customAgents = customAgentRepository.GetCustomAgents(tenant);
            IQueryable<CustomAgentList> query2 = customAgentQuery.GetIQueryableEntityList(customAgents);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomAgentList> GetCustomAgentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentRepository = new CustomAgentRepository(tenant);
            customAgentQuery = new CustomAgentQuery(customAgentRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomAgent> customAgents = customAgentRepository.GetCustomAgents(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomAgentCustomFilter customfilters = new CustomAgentCustomFilter(tenant);
            customAgents = customfilters.GetFilteredQuery(queryOperations, customAgents);

            customAgents = filter.GetFilteredQuery<CustomAgent>(nonListQueryOperation, customAgents);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomAgentList> query2 = customAgentQuery.GetIQueryableEntityList(customAgents);
            query2 = filter.GetFilteredQuery<CustomAgentList>(listQueryOperation, query2);
           
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomAgentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomAgent", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomAgentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomAgentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomAgentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomAgentList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<CustomAgentList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomAgentList, bool>(queryOperations, query2);
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

        public int GetCustomAgentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "READ", tenant);

            customAgentRepository = new CustomAgentRepository(tenant);
            customAgentQuery = new CustomAgentQuery(customAgentRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomAgent> customAgents = customAgentRepository.GetCustomAgents(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomAgentCustomFilter customfilters = new CustomAgentCustomFilter(tenant);
            customAgents = customfilters.GetFilteredQuery(queryOperations, customAgents);

            customAgents = filter.GetFilteredQuery<CustomAgent>(nonListQueryOperation, customAgents);

            IQueryable<CustomAgentList> query2 = customAgentQuery.GetIQueryableEntityList(customAgents);
            query2 = filter.GetFilteredQuery<CustomAgentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertCustomAgent(CustomAgentPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            CustomAgentService service = new CustomAgentService(objectContext , entityPm.Tenant);
            service.Create(entityPm);
        }

        public void UpdateCustomAgent(CustomAgentPM currentEntity)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntity.Tenant);
            SecurityUtility.CheckContactFeature("CustomAgent", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

            List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrenciesChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.CardExternalCodeByCurrencies).Cast<CardExternalCodeByCurrencyPM>().ToList();
            foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrenciesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            CustomAgentService service = new CustomAgentService(objectContext, currentEntity.Tenant);
            service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
            service.Update(currentEntity);
        }

        public void UpdateCustomAgentList(CustomAgentList currentEntity)
        {
        }

        public void DeleteCustomAgent(CustomAgentPM customAgent)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(customAgent.Tenant);
            }
            customAgentRepository = new CustomAgentRepository(objectContext);
            CustomAgent removedCustomAgent = customAgentRepository.GetSingleCustomAgent(customAgent.Tenant, customAgent.Id);
            customAgentRepository.Remove(removedCustomAgent);
        }
    }
}
