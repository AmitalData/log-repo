using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
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
        private CheckDigitControlAlgorithmRepository checkDigitControlAlgorithmRepository;
        private CheckDigitControlAlgorithmQuery checkDigitControlAlgorithmQuery;

        public void UpdateCheckDigitControlAlgorithmList(CheckDigitControlAlgorithmList currentEntity)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CheckDigitControlAlgorithmList> GetCheckDigitControlAlgorithmFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            checkDigitControlAlgorithmRepository = new CheckDigitControlAlgorithmRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CheckDigitControlAlgorithm> iQueryable = checkDigitControlAlgorithmRepository.GetAll();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CheckDigitControlAlgorithm>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            checkDigitControlAlgorithmQuery = new CheckDigitControlAlgorithmQuery(checkDigitControlAlgorithmRepository);
            IQueryable<CheckDigitControlAlgorithmList> query2 = checkDigitControlAlgorithmQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CheckDigitControlAlgorithmList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CheckDigitControlAlgorithmList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CheckDigitControlAlgorithm", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CheckDigitControlAlgorithmList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CheckDigitControlAlgorithmList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CheckDigitControlAlgorithmList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CheckDigitControlAlgorithmList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CheckDigitControlAlgorithmList, bool>(queryOperations, query2);
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

        public int GetCheckDigitControlAlgorithmFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            checkDigitControlAlgorithmRepository = new CheckDigitControlAlgorithmRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CheckDigitControlAlgorithm> iQueryable = checkDigitControlAlgorithmRepository.GetAll();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CheckDigitControlAlgorithm>(nonListQueryOperation, iQueryable);
            checkDigitControlAlgorithmQuery = new CheckDigitControlAlgorithmQuery(checkDigitControlAlgorithmRepository);
            IQueryable<CheckDigitControlAlgorithmList> query2 = checkDigitControlAlgorithmQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CheckDigitControlAlgorithmList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}