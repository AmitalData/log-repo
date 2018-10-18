using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        private MarkUpTypeRepository markUpTypeRepository;
        public IQueryable<MarkUpType> GetMarkUpTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            markUpTypeRepository = new MarkUpTypeRepository(tenant);
            return markUpTypeRepository.GetMarkUpTypes();
        }

        public MarkUpType GetSingleMarkUpType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            markUpTypeRepository = new MarkUpTypeRepository(tenant);
            return markUpTypeRepository.GetSingleMarkUpType(code);
        }

        public MarkUpTypeList GetSingleMarkUpTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            markUpTypeRepository = new MarkUpTypeRepository(tenant);
            markUpTypeQuery = new MarkUpTypeQuery(tenant);

            MarkUpTypeList markUpTypeList = null;
            MarkUpType markUpType = markUpTypeRepository.GetSingleMarkUpType(code);

            if (markUpType != null)
            {
                List<MarkUpType> singleEntityList = new List<MarkUpType>();
                singleEntityList.Add(markUpType);

                IQueryable<MarkUpType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<MarkUpTypeList> iQueryableEntityList = markUpTypeQuery.GetIQueryableEntityList(iQueryable);
                markUpTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return markUpTypeList;
        }

        public IQueryable<MarkUpTypeList> GetMarkUpTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            markUpTypeRepository = new MarkUpTypeRepository(tenant);
            markUpTypeQuery = new MarkUpTypeQuery(tenant);
            IQueryable<MarkUpType> iQueryable = markUpTypeRepository.GetMarkUpTypes();
            IQueryable<MarkUpTypeList> query2 = markUpTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<MarkUpTypeList> GetMarkUpTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            markUpTypeRepository = new MarkUpTypeRepository(tenant);
            markUpTypeQuery = new MarkUpTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MarkUpType> iQueryable = markUpTypeRepository.GetMarkUpTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MarkUpType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<MarkUpTypeList> query2 = markUpTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<MarkUpTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(MarkUpTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PartnerType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpTypeList, bool>(queryOperations, query2);
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

        public int GetMarkUpTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            markUpTypeRepository = new MarkUpTypeRepository(tenant);
            markUpTypeQuery = new MarkUpTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MarkUpType> iQueryable = markUpTypeRepository.GetMarkUpTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MarkUpType>(nonListQueryOperation, iQueryable);

            IQueryable<MarkUpTypeList> query2 = markUpTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<MarkUpTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertMarkUpType(MarkUpType entity)
        {
            markUpTypeRepository.Add(entity);
        }

        public void UpdateMarkUpType(MarkUpType currentEntity)
        {
            markUpTypeRepository.Update(currentEntity);
        }

        public void DeleteMarkUpType(MarkUpType entity)
        {
            markUpTypeRepository.Remove(entity);
        }
    }
}