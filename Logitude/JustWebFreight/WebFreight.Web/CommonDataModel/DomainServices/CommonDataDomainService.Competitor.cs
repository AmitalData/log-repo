using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateCompetitorList(CompetitorList currentEntity)
        {
        }

        public IQueryable<Competitor> GetCompetitors(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Competitor", "READ", tenant);

            competitorRepository = new CompetitorRepository(tenant);
            return competitorRepository.GetCompetitors(0);
        }

        public CompetitorPM GetSingleCompetitorPM(string Id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Competitor", "READ", tenant);

            competitorQuery = new CompetitorQuery(tenant);
            return competitorQuery.GetSinglePM(Id, tenant);
        }

        public CompetitorList GetSingleCompetitorList(string Id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Competitor", "READ", tenant);

            competitorRepository = new CompetitorRepository(tenant);
            competitorQuery = new CompetitorQuery(competitorRepository);
            CompetitorList CompetitorList = null;
            Competitor Competitor = competitorRepository.GetSingleCompetitor(Id, tenant);

            if (Competitor != null)
            {
                List<Competitor> singleEntityList = new List<Competitor>();
                singleEntityList.Add(Competitor);

                IQueryable<Competitor> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CompetitorList> iQueryableEntityList = competitorQuery.GetIQueryableEntityList(iQueryable);
                CompetitorList = iQueryableEntityList.FirstOrDefault();
            }
            return CompetitorList;
        }

        public IQueryable<CompetitorList> GetCompetitorLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Competitor", "READ", tenant);

            competitorRepository = new CompetitorRepository(tenant);
            competitorQuery = new CompetitorQuery(competitorRepository);
            IQueryable<Competitor> Competitors = competitorRepository.GetCompetitors(tenant);
            IQueryable<CompetitorList> query2 = competitorQuery.GetIQueryableEntityList(Competitors);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CompetitorList> GetCompetitorFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Competitor", "READ", tenant);

            competitorRepository = new CompetitorRepository(tenant);
            competitorQuery = new CompetitorQuery(competitorRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Competitor> Competitors = competitorRepository.GetCompetitors(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            Competitors = filter.GetFilteredQuery<Competitor>(nonListQueryOperation, Competitors);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<CompetitorList> query2 = competitorQuery.GetIQueryableEntityList(Competitors);

            query2 = filter.GetFilteredQuery<CompetitorList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CompetitorList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Competitor", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CompetitorList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CompetitorList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CompetitorList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CompetitorList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CompetitorList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }
            
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCompetitorFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Competitor", "READ", tenant);

            competitorRepository = new CompetitorRepository(tenant);
            competitorQuery = new CompetitorQuery(competitorRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Competitor> Competitors = competitorRepository.GetCompetitors(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            Competitors = filter.GetFilteredQuery<Competitor>(nonListQueryOperation, Competitors);

            IQueryable<CompetitorList> query2 = competitorQuery.GetIQueryableEntityList(Competitors);

            query2 = filter.GetFilteredQuery<CompetitorList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertCompetitor(CompetitorPM entityPM)
        {
            SecurityUtility.CheckContactFeature("Competitor", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CompetitorService service = new CompetitorService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateCompetitor(CompetitorPM entityPM)
        {
            SecurityUtility.CheckContactFeature("Competitor", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            competitorRepository = new CompetitorRepository(objectContext);

            CompetitorService service = new CompetitorService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeleteCompetitor(CompetitorPM Competitor)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(Competitor.Tenant);
            }
            competitorRepository = new CompetitorRepository(objectContext);
            Competitor entity = competitorRepository.GetSingleCompetitor(Competitor.Id, Competitor.Tenant);
            competitorRepository.Remove(entity);
        }
    }
}