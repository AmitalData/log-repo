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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateStateList(StateList currentEntity)
        {
        }

        public IQueryable<State> GetStates(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateRepository = new StateRepository(tenant);
            return stateRepository.GetStates(0);
        }

        public IQueryable<State> GetStatesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateRepository = new StateRepository(tenant);
            IQueryable<State> q = stateRepository.GetStates(tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public IQueryable<StatePM> GetStatePMsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateQuery = new StateQuery(tenant);
            IQueryable<StatePM> q = stateQuery.GetStatePMsByTenant(tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public IQueryable<StatePM> GetStatesByCountryTenant(string countryCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateQuery = new StateQuery(tenant);
            IQueryable<StatePM> q = stateQuery.GetStatePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.CountryCode == countryCode);
            return q;
        }

        public StatePM GetStateById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateQuery = new StateQuery(tenant);
            StatePM state = stateQuery.GetSinglePM(id, tenant);
            return state;
        }

        public StateList GetSingleStateList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateRepository = new StateRepository(tenant);
            StateList stateList = null;
            State state = stateRepository.GetSingleState(id, tenant);

            if (state != null)
            {
                List<State> singleEntityList = new List<State>();
                singleEntityList.Add(state);

                stateQuery = new StateQuery(stateRepository);
                IQueryable<State> iQueryable = singleEntityList.AsQueryable();
                IQueryable<StateList> iQueryableEntityList = stateQuery.GetIQueryableEntityList(iQueryable);
                stateList = iQueryableEntityList.FirstOrDefault();
            }
            return stateList;
        }

        public IQueryable<StateList> GetStateLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateRepository = new StateRepository(tenant);
            stateQuery = new StateQuery(stateRepository);

            IQueryable<State> states = stateRepository.GetStates(tenant);
            IQueryable<StateList> query2 = stateQuery.GetIQueryableEntityList(states);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<StateList> GetStateFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateRepository = new StateRepository(tenant);
            stateQuery = new StateQuery(stateRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<State> states = stateRepository.GetStates(tenant);
            
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            states = filter.GetFilteredQuery<State>(nonListQueryOperation, states);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<StateList> query2 = stateQuery.GetIQueryableEntityList(states);
            query2 = filter.GetFilteredQuery<StateList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(StateList).GetProperty(queryOperations.SortByColumnName);
                
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("State", tenant).ToList();
                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<StateList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<StateList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<StateList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<StateList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<StateList, bool>(queryOperations, query2);
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

        public int GetStateFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateRepository = new StateRepository(tenant);
            stateQuery = new StateQuery(stateRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<State> states = stateRepository.GetStates(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            states = filter.GetFilteredQuery<State>(nonListQueryOperation, states);

            IQueryable<StateList> query2 = stateQuery.GetIQueryableEntityList(states);

            query2 = filter.GetFilteredQuery<StateList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<StatePM> GetStatesSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateQuery = new StateQuery(tenant);
            IQueryable<StatePM> q = stateQuery.GetStatesByCodeOrName(code, name, tenant);
            IQueryable<StatePM> m = q.Where(s => s.Tenant == tenant);
            return m;
        }

        public IQueryable<StatePM> GetStatesByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateQuery = new StateQuery(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {
                if (byCode)
                {
                    return stateQuery.GetStatePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return stateQuery.GetStatePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return stateQuery.GetStatePMsByTenant(tenant).Where(d => d.Tenant == tenant);
            }
        }

        public IQueryable<StatePM> GetSingleStateByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("State", "READ", tenant);

            stateQuery = new StateQuery(tenant);
            if (byCode)
            {
                return stateQuery.GetStatePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return stateQuery.GetStatePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertState(StatePM entityPm)
        {
            SecurityUtility.CheckContactFeature("State", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            StateService service = new StateService(objectContext, entityPm.Tenant);
            service.Create(entityPm);

            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "State");
        }

        public void UpdateState(StatePM currententityPm)
        {
            SecurityUtility.CheckContactFeature("State", "UPDATE", currententityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currententityPm.Tenant);
            }

            StateService service = new StateService(objectContext, currententityPm.Tenant);
            service.Update(currententityPm);

            TableLastUpdateClass.UpdateTableHistory(currententityPm.Tenant, "State");
        }

        public void DeleteState(StatePM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }
            stateRepository = new StateRepository(objectContext);
            State removedEntity = stateRepository.GetSingleState(entityPm.Id, entityPm.Tenant);
            stateRepository.Remove(removedEntity);
        }
    }
}