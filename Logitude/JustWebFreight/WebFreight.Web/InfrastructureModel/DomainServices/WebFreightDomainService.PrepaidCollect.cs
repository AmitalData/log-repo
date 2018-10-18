using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdatePrepaidCollectList(PrepaidCollectList currentEntity)
        {
        }

        public IQueryable<PrepaidCollect> GetPrepaidCollects(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            prepaidCollectsRepository = new PrepaidCollectRepository(tenant);
            return prepaidCollectsRepository.GetPrepaidCollects();
        }

        public IQueryable<PrepaidCollect> PrepaidCollectsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            prepaidCollectsRepository = new PrepaidCollectRepository(tenant);
            return prepaidCollectsRepository.GetPrepaidCollects();
        }

        public PrepaidCollectPM GetSinglePrepaidCollect(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            prepaidCollectsRepository = new PrepaidCollectRepository(tenant);
            prepaidCollectQuery = new PrepaidCollectQuery(prepaidCollectsRepository);
            return prepaidCollectQuery.GetSinglePrepaidCollectPM(id);
        }

        public PrepaidCollectList GetSinglePrepaidCollectList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            prepaidCollectsRepository = new PrepaidCollectRepository(tenant);
            PrepaidCollectList prepaidCollectList = null;
            PrepaidCollect prepaidCollect = prepaidCollectsRepository.GetSinglePrepaidCollect(id);

            if (prepaidCollect != null)
            {
                List<PrepaidCollect> singleEntityList = new List<PrepaidCollect>();
                singleEntityList.Add(prepaidCollect);

                IQueryable<PrepaidCollect> iQueryable = singleEntityList.AsQueryable();
                prepaidCollectQuery = new PrepaidCollectQuery(prepaidCollectsRepository);
                IQueryable<PrepaidCollectList> iQueryableEntityList = prepaidCollectQuery.GetIQueryableEntityList(iQueryable);
                prepaidCollectList = iQueryableEntityList.FirstOrDefault();
            }
            return prepaidCollectList;
        }

        public IQueryable<PrepaidCollectList> GetPrepaidCollectLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            prepaidCollectsRepository = new PrepaidCollectRepository(tenant);
            IQueryable<PrepaidCollect> iQueryable = prepaidCollectsRepository.GetPrepaidCollects();
            prepaidCollectQuery = new PrepaidCollectQuery(prepaidCollectsRepository);
            IQueryable<PrepaidCollectList> query2 = prepaidCollectQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PrepaidCollectList> GetPrepaidCollectFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            prepaidCollectsRepository = new PrepaidCollectRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PrepaidCollect> iQueryable = prepaidCollectsRepository.GetPrepaidCollects();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PrepaidCollect>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            prepaidCollectQuery = new PrepaidCollectQuery(prepaidCollectsRepository);
            IQueryable<PrepaidCollectList> query2 = prepaidCollectQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PrepaidCollectList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PrepaidCollectList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PrepaidCollect", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PrepaidCollectList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PrepaidCollectList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PrepaidCollectList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PrepaidCollectList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PrepaidCollectList, bool>(queryOperations, query2);
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

        public int GetPrepaidCollectFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            prepaidCollectsRepository = new PrepaidCollectRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PrepaidCollect> iQueryable = prepaidCollectsRepository.GetPrepaidCollects();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PrepaidCollect>(nonListQueryOperation, iQueryable);
            prepaidCollectQuery = new PrepaidCollectQuery(prepaidCollectsRepository);
            IQueryable<PrepaidCollectList> query2 = prepaidCollectQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PrepaidCollectList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPrepaidCollect(PrepaidCollect entity)
        {
            prepaidCollectsRepository.Add(entity);
        }

        public void UpdatePrepaidCollect(PrepaidCollect currentEntity)
        {
            prepaidCollectsRepository.Update(currentEntity);
        }

        public void DeletePrepaidCollect(PrepaidCollect entity)
        {
            prepaidCollectsRepository.Remove(entity);
        }
    }
}