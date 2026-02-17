using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateRankList(RankList currentEntity)
        {
        }

        public IQueryable<Rank> GetRanks(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rankRepository = new RankRepository(tenant);
            return rankRepository.GetRanks(0);
        }

        public IQueryable<RankPM> GetRanksByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rankQuery = new RankQuery(tenant);
            return rankQuery.GetRankPMsByTenant(tenant);
        }

        public RankList GetSingleRankList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rankRepository = new RankRepository(tenant);
            RankList rankList = null;
            Rank rank = rankRepository.GetSingleRank(id, tenant);

            if (rank != null)
            {
                List<Rank> singleEntityList = new List<Rank>();
                singleEntityList.Add(rank);

                rankQuery = new RankQuery(rankRepository);
                IQueryable<Rank> iQueryable = singleEntityList.AsQueryable();
                IQueryable<RankList> iQueryableEntityList = rankQuery.GetIQueryableEntityList(iQueryable);
                rankList = iQueryableEntityList.FirstOrDefault();
            }
            return rankList;
        }

        public IQueryable<RankList> GetRankLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rankRepository = new RankRepository(tenant);
            rankQuery = new RankQuery(rankRepository);

            IQueryable<Rank> ranks = rankRepository.GetRanks(tenant);
            IQueryable<RankList> query2 = rankQuery.GetIQueryableEntityList(ranks);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<RankList> GetRankFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rankRepository = new RankRepository(tenant);
            rankQuery = new RankQuery(rankRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Rank> ranks = rankRepository.GetRanks(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ranks = filter.GetFilteredQuery<Rank>(nonListQueryOperation, ranks);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<RankList> query2 = rankQuery.GetIQueryableEntityList(ranks);
            query2 = filter.GetFilteredQuery<RankList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RankList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Rank", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RankList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RankList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RankList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RankList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RankList, bool>(queryOperations, query2);
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

        public int GetRankFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rankRepository = new RankRepository(tenant);
            rankQuery = new RankQuery(rankRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Rank> ranks = rankRepository.GetRanks(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            ranks = filter.GetFilteredQuery<Rank>(nonListQueryOperation, ranks);

            IQueryable<RankList> query2 = rankQuery.GetIQueryableEntityList(ranks);

            query2 = filter.GetFilteredQuery<RankList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapRankRankPM(RankPM rankPM, Rank rank)
        //{
        //    rank.Name = rankPM.Name;
        //    rank.Tenant = rankPM.Tenant;
        //    rank.Code = rankPM.Code;
        //    rank.SearchFields = rankPM.Code + "," + rankPM.Name;
        //}

        public RankPM GetSingleRank(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Rank", "READ", tenant);

            rankQuery = new RankQuery(tenant);
            return rankQuery.GetSinglePM(id, tenant);
        }

        public void InsertRank(RankPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            RankService service = new RankService(objectContext , entity.Tenant);
            service.Create(entity);
            TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "Rank");
        }

        public void UpdateRank(RankPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }
            RankService service = new RankService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //rankRepository = new RankRepository(objectContext);
            //Rank entity = rankRepository.GetSingleRank(currentEntity.Id, currentEntity.Tenant);
            //MapRankRankPM(currentEntity, entity);
            //rankRepository.Update(entity);

            TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "Rank");
        }

        public void DeleteRank(Rank entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }
            rankRepository = new RankRepository(objectContext);
            Rank entity = rankRepository.GetSingleRank(entityPm.Id, entityPm.Tenant);
            rankRepository.Remove(entity);
        }
    }
}