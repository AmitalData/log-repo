using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
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
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private AirlineMessagingRuleRepository AirlineMessagingRuleRepository;
        private AirlineMessagingRuleQuery AirlineMessagingRuleQuery;

        public void UpdateAirlineMessagingRuleList(AirlineMessagingRuleList currentEntity)
        {
        }

        public AirlineMessagingRulePM GetSingleAirlineMessagingRulePM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

            AirlineMessagingRuleQuery = new AirlineMessagingRuleQuery(tenant);
            return AirlineMessagingRuleQuery.GetSinglePM(id, tenant);
        }

        public AirlineMessagingRuleList GetSingleAirlineMessagingRuleList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

            AirlineMessagingRuleRepository = new AirlineMessagingRuleRepository(tenant);
            AirlineMessagingRuleList entityList = null;

            AirlineMessagingRule entityPoco = AirlineMessagingRuleRepository.GetSingleAirlineMessagingRule(id, tenant);

            if (entityPoco != null)
            {
                List<AirlineMessagingRule> singleEntityList = new List<AirlineMessagingRule>();
                singleEntityList.Add(entityPoco);

                AirlineMessagingRuleQuery = new AirlineMessagingRuleQuery(AirlineMessagingRuleRepository);
                IQueryable<AirlineMessagingRule> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AirlineMessagingRuleList> iQueryableEntityList = AirlineMessagingRuleQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<AirlineMessagingRuleList> GetAirlineMessagingRuleLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

            AirlineMessagingRuleRepository = new AirlineMessagingRuleRepository(tenant);
            AirlineMessagingRuleQuery = new AirlineMessagingRuleQuery(AirlineMessagingRuleRepository);

            IQueryable<AirlineMessagingRule> iQueryable = AirlineMessagingRuleRepository.GetAirlineMessagingRules(tenant);
            IQueryable<AirlineMessagingRuleList> query2 = AirlineMessagingRuleQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AirlineMessagingRuleList> GetAirlineMessagingRuleFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

            AirlineMessagingRuleRepository = new AirlineMessagingRuleRepository(tenant);
            AirlineMessagingRuleQuery = new AirlineMessagingRuleQuery(AirlineMessagingRuleRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AirlineMessagingRule> iQueryable = AirlineMessagingRuleRepository.GetAirlineMessagingRules(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AirlineMessagingRule>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AirlineMessagingRuleList> query2 = AirlineMessagingRuleQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AirlineMessagingRuleList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PackageList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AirlineMessagingRule", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineMessagingRuleList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineMessagingRuleList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineMessagingRuleList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineMessagingRuleList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineMessagingRuleList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.MessageTypeCode);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.MessageTypeCode);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAirlineMessagingRuleFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

            AirlineMessagingRuleRepository = new AirlineMessagingRuleRepository(tenant);
            AirlineMessagingRuleQuery = new AirlineMessagingRuleQuery(AirlineMessagingRuleRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AirlineMessagingRule> iQueryable = AirlineMessagingRuleRepository.GetAirlineMessagingRules(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AirlineMessagingRule>(nonListQueryOperation, iQueryable);

            IQueryable<AirlineMessagingRuleList> query2 = AirlineMessagingRuleQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AirlineMessagingRuleList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAirlineMessagingRule(AirlineMessagingRulePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            AirlineMessagingRuleService service = new AirlineMessagingRuleService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateAirlineMessagingRule(AirlineMessagingRulePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            AirlineMessagingRuleService service = new AirlineMessagingRuleService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeleteAirlineMessagingRule(AirlineMessagingRulePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            AirlineMessagingRuleRepository = new AirlineMessagingRuleRepository(objectContext);

            AirlineMessagingRule entity = AirlineMessagingRuleRepository.GetSingleAirlineMessagingRule(entityPM.Id, entityPM.Tenant);
            if (entity != null)
            {
                AirlineMessagingRuleRepository.Remove(entity);
            }
        }

        public IQueryable<AirlineMessagingRuleList> GetMessagingRulesForAirline(string airlineCode, string messageType, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

            AirlineMessagingRuleRepository = new AirlineMessagingRuleRepository(tenant);
            AirlineMessagingRuleQuery = new AirlineMessagingRuleQuery(AirlineMessagingRuleRepository);

            IQueryable<AirlineMessagingRule> iQueryable = AirlineMessagingRuleRepository.GetAirlineMessagingRulesForAirline(airlineCode, messageType, tenant);
            IQueryable<AirlineMessagingRuleList> query2 = AirlineMessagingRuleQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }
    }
}