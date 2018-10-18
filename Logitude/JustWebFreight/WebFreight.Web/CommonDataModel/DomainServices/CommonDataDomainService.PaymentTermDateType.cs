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
        private PaymentTermDateTypeQuery paymentTermDateTypeQuery;
        private PaymentTermDateTypeRepository paymentTermDateTypeRepository;
        public void UpdatePaymentTermDateTypeList(PaymentTermDateTypeList currentEntity)
        {

        }

        public PaymentTermDateTypeList GetSinglePaymentTermDateTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            PaymentTermDateTypeList entityList = null;
            paymentTermDateTypeRepository = new PaymentTermDateTypeRepository(tenant);
            PaymentTermDateType entityPOCO = paymentTermDateTypeRepository.GetSinglePaymentTermDateType(code);

            if (entityPOCO != null)
            {
                List<PaymentTermDateType> singleEntityList = new List<PaymentTermDateType>();
                singleEntityList.Add(entityPOCO);

                paymentTermDateTypeQuery = new PaymentTermDateTypeQuery(paymentTermDateTypeRepository);
                IQueryable<PaymentTermDateType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PaymentTermDateTypeList> iQueryableEntityList = paymentTermDateTypeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<PaymentTermDateTypeList> GetPaymentTermDateTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentTermDateTypeRepository = new PaymentTermDateTypeRepository(tenant);
            paymentTermDateTypeQuery = new PaymentTermDateTypeQuery(paymentTermDateTypeRepository);

            IQueryable<PaymentTermDateType> iQueryable = paymentTermDateTypeRepository.GetPaymentTermDateTypes();
            IQueryable<PaymentTermDateTypeList> query2 = paymentTermDateTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PaymentTermDateTypeList> GetPaymentTermDateTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            paymentTermDateTypeRepository = new PaymentTermDateTypeRepository(tenant);
            paymentTermDateTypeQuery = new PaymentTermDateTypeQuery(paymentTermDateTypeRepository);
            IQueryable<PaymentTermDateType> iQueryable = paymentTermDateTypeRepository.GetPaymentTermDateTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentTermDateType>(nonListQueryOperation, iQueryable);

            int skippedEntities = queryOperations.PageIndex;

            IQueryable<PaymentTermDateTypeList> query2 = paymentTermDateTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentTermDateTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PaymentTermDateTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PaymentTermDateType", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermDateTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermDateTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermDateTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermDateTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermDateTypeList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetPaymentTermDateTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            paymentTermDateTypeRepository = new PaymentTermDateTypeRepository(tenant);
            paymentTermDateTypeQuery = new PaymentTermDateTypeQuery(paymentTermDateTypeRepository);
            IQueryable<PaymentTermDateType> iQueryable = paymentTermDateTypeRepository.GetPaymentTermDateTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentTermDateType>(nonListQueryOperation, iQueryable);

            IQueryable<PaymentTermDateTypeList> query2 = paymentTermDateTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<PaymentTermDateTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

    }
}