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
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public bool DoesAgentCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            agentRepository = new AgentRepository(tenant);
            return (agentRepository.GetAgents(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        //public IQueryable<Agent> GetAgents(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Agent", "READ", tenant);
            
        //    agentRepository = new AgentRepository(tenant);
        //    return agentRepository.GetAgents(0);
        //}

        public IQueryable<AgentPM> GetAgentsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Agent", "READ", tenant);

 
            agentQuery = new AgentQuery(tenant);
            return agentQuery.GetAgentPMsByTenant(tenant);
        }

        public IQueryable<AgentPM> GetAgentsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Agent", "READ", tenant);

            agentQuery = new AgentQuery(tenant);
            IQueryable<AgentPM> q = agentQuery.GetAgentsByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public AgentPM GetAgentById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Agent", "READ", tenant);

            agentQuery = new AgentQuery(tenant);
            AgentPM agent = agentQuery.GetSinglePM(id,tenant);
            return agent;
        }

        public AgentList GetSingleAgentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Agent", "READ", tenant);

            agentRepository = new AgentRepository(tenant);
            AgentList agentList = null;
            Agent agent = agentRepository.GetSingleAgent(tenant, id);

            if (agent != null)
            {
                List<Agent> singleEntityList = new List<Agent>();
                singleEntityList.Add(agent);

                IQueryable<Agent> iQueryable = singleEntityList.AsQueryable();
                agentQuery = new AgentQuery(agentRepository);
                IQueryable<AgentList> iQueryableEntityList = agentQuery.GetIQueryableEntityList(iQueryable);
                agentList = iQueryableEntityList.FirstOrDefault();
            }
            return agentList;
        }

        public IQueryable<AgentList> GetAgentLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Agent", "READ", tenant);

            agentRepository = new AgentRepository(tenant);
            IQueryable<Agent> agents = agentRepository.GetAgents(tenant);
            agentQuery = new AgentQuery(agentRepository);
            IQueryable<AgentList> query2 = agentQuery.GetIQueryableEntityList(agents);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AgentList> GetAgentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Agent", "READ", tenant);

            agentRepository = new AgentRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Agent> agents = agentRepository.GetAgents(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AgentCustomFilter customfilters = new AgentCustomFilter(tenant);
            agents = customfilters.GetFilteredQuery(queryOperations, agents);
            agents = filter.GetFilteredQuery<Agent>(nonListQueryOperation, agents);
            int skippedPorts = queryOperations.PageIndex;//queryOperations.PageSize * (queryOperations.PageIndex - 1);
            agentQuery = new AgentQuery(agentRepository);
            IQueryable<AgentList> query2 = agentQuery.GetIQueryableEntityList(agents);
            query2 = filter.GetFilteredQuery<AgentList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AgentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Agent", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AgentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AgentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AgentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AgentList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<AgentList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AgentList, bool>(queryOperations, query2);
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

        public int GetAgentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Agent", "READ", tenant);

            agentRepository = new AgentRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Agent> agents = agentRepository.GetAgents(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AgentCustomFilter customfilters = new AgentCustomFilter(tenant);
            agents = customfilters.GetFilteredQuery(queryOperations, agents);
            agents = filter.GetFilteredQuery<Agent>(nonListQueryOperation, agents);
            agentQuery = new AgentQuery(agentRepository);
            IQueryable<AgentList> query2 = agentQuery.GetIQueryableEntityList(agents);
            query2 = filter.GetFilteredQuery<AgentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAgent(AgentPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            SecurityUtility.CheckContactFeature("Agent", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            AgentService service = new AgentService(objectContext, entityPm.Tenant);
            service.Create(entityPm);
        }

        public void UpdateAgent(AgentPM currentEntity)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntity.Tenant);
            SecurityUtility.CheckContactFeature("Agent", "UPDATE", currentEntity.Tenant);

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

            AgentService service = new AgentService(objectContext, currentEntity.Tenant);
            service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
            service.Update(currentEntity);
        }

        public void UpdateAgentList(AgentList currentEntity)
        {
        }

        public void DeleteAgent(AgentPM agent)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(agent.Tenant);
            }
            agentRepository = new AgentRepository(objectContext);
            Agent entity = agentRepository.GetSingleAgent(agent.Tenant, agent.Id);
            agentRepository.Remove(entity);
        }
    }
}