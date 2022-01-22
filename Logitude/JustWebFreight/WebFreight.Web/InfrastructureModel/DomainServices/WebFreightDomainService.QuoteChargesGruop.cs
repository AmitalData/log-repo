using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
     public partial class WebFreightDomainService
    {
        [Query(HasSideEffects = true)]
        public IQueryable<ChargesGroupList> GetQuoteChargesGroupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ChargesGroup> chargreGroups = chargesGroupsRepository.GetChargesGroups(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            chargreGroups = filter.GetFilteredQuery<ChargesGroup>(nonListQueryOperation, chargreGroups);

            int skippedChargregroups = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
            IQueryable<ChargesGroupList> query2 = chargesGroupQuery.GetIQueryableEntityList(chargreGroups);
            query2 = filter.GetFilteredQuery<ChargesGroupList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ChargesGroupList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ChargesGroup", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedChargregroups);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetQuoteChargesGroupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ChargesGroup> chargreGroups = chargesGroupsRepository.GetChargesGroups(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            chargreGroups = filter.GetFilteredQuery<ChargesGroup>(nonListQueryOperation, chargreGroups);

            int skippedVatTypes = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
            IQueryable<ChargesGroupList> query2 = chargesGroupQuery.GetIQueryableEntityList(chargreGroups);
            query2 = filter.GetFilteredQuery<ChargesGroupList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}