using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private AccountingInformationIdentifierQuery accountingInformationIdentifierQuery;
        private AccountingInformationIdentifierRepository accountingInformationIdentifierRepository;

        public AccountingInformationIdentifierList GetSingleAccountingInformationIdentifierList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingInformationIdentifierQuery = new AccountingInformationIdentifierQuery(tenant);
            AccountingInformationIdentifierList entityList = accountingInformationIdentifierQuery.GetSingleAccountingInformationIdentifierList(code);

            return entityList;
        }

        public IQueryable<AccountingInformationIdentifierList> GetAccountingInformationIdentifierLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingInformationIdentifierQuery = new AccountingInformationIdentifierQuery(tenant);
            accountingInformationIdentifierRepository = new AccountingInformationIdentifierRepository(tenant);

            IQueryable<AccountingInformationIdentifier> iQueryable = accountingInformationIdentifierRepository.GetAccountingInformationIdentifiers();
            var query2 = from entity in iQueryable
                         select new AccountingInformationIdentifierList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            return query2;
        }

        public void UpdateAccountingInformationIdentifierList(AccountingInformationIdentifierList entityList)
        {

        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountingInformationIdentifierList> GetAccountingInformationIdentifierFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            accountingInformationIdentifierRepository = new AccountingInformationIdentifierRepository(tenant);
            IQueryable<AccountingInformationIdentifier> iQueryable = accountingInformationIdentifierRepository.GetAccountingInformationIdentifiers();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountingInformationIdentifier>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new AccountingInformationIdentifierList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AccountingInformationIdentifierList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AccountingInformationIdentifierList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AccountingInformationIdentifier", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields where a.FieldName == queryOperations.SortByColumnName select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingInformationIdentifierList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingInformationIdentifierList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingInformationIdentifierList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingInformationIdentifierList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingInformationIdentifierList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAccountingInformationIdentifierFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            accountingInformationIdentifierRepository = new AccountingInformationIdentifierRepository(tenant);
            IQueryable<AccountingInformationIdentifier> iQueryable = accountingInformationIdentifierRepository.GetAccountingInformationIdentifiers();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountingInformationIdentifier>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new AccountingInformationIdentifierList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AccountingInformationIdentifierList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

    }
}