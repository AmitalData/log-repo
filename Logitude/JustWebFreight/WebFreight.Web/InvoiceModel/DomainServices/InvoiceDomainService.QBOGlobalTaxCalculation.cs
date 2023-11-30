using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {

        public IQueryable<QBOGlobalTaxCalculationList> GetQBOGlobalTaxCalculationFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            QBOGlobalTaxCalculationRepository QBOGlobalTaxCalculationRepository = new QBOGlobalTaxCalculationRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QBOGlobalTaxCalculation> QBOGlobalTaxCalculations = QBOGlobalTaxCalculationRepository.GetQBOGlobalTaxCalculations();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            QBOGlobalTaxCalculations = filter.GetFilteredQuery<QBOGlobalTaxCalculation>(nonListQueryOperation, QBOGlobalTaxCalculations);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QBOGlobalTaxCalculationQuery QBOGlobalTaxCalculationQuery = new QBOGlobalTaxCalculationQuery(QBOGlobalTaxCalculationRepository);
            IQueryable<QBOGlobalTaxCalculationList> query2 = QBOGlobalTaxCalculationQuery.GetIQueryableEntityList(QBOGlobalTaxCalculations);

            query2 = filter.GetFilteredQuery<QBOGlobalTaxCalculationList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QBOGlobalTaxCalculationList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QBOGlobalTaxCalculationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QBOGlobalTaxCalculation", tenant).ToList();

                ObjectField objectField = (from a in QBOGlobalTaxCalculationObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QBOGlobalTaxCalculationList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QBOGlobalTaxCalculationList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QBOGlobalTaxCalculationList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QBOGlobalTaxCalculationList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QBOGlobalTaxCalculationList, bool>(queryOperations, query2);
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

        public int GetQBOGlobalTaxCalculationFiltersCount(byte[] xmlFilters, int tenant)
        {
            QBOGlobalTaxCalculationRepository QBOGlobalTaxCalculationRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);

            QBOGlobalTaxCalculationRepository = new QBOGlobalTaxCalculationRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QBOGlobalTaxCalculation> QBOGlobalTaxCalculations = QBOGlobalTaxCalculationRepository.GetQBOGlobalTaxCalculations();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            QBOGlobalTaxCalculations = filter.GetFilteredQuery<QBOGlobalTaxCalculation>(nonListQueryOperation, QBOGlobalTaxCalculations);
            QBOGlobalTaxCalculationQuery QBOGlobalTaxCalculationQuery = new QBOGlobalTaxCalculationQuery(QBOGlobalTaxCalculationRepository);
            IQueryable<QBOGlobalTaxCalculationList> query2 = QBOGlobalTaxCalculationQuery.GetIQueryableEntityList(QBOGlobalTaxCalculations);

            query2 = filter.GetFilteredQuery<QBOGlobalTaxCalculationList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}