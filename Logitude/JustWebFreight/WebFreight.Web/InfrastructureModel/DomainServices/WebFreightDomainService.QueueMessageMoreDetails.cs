using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityLists;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateQueueMessageMoreDetailsList(QueueMessageMoreDetailsList currentEntity)
        {
        }

        public IQueryable<QueueMessageMoreDetails> GetQueueMessageMoreDetails(int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);
            return QueueMessageMoreDetailsRepository.GetQueueMessageMoreDetails();
        }

        //public IQueryable<QueueMessageMoreDetailsPM> GetQueueMessageMoreDetailsByTenant(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    eventTypesRepository = new EventTypeRepository(tenant);
        //    eventTypeQuery = new EventTypeQuery(eventTypesRepository);
        //    return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Tenant == tenant);
        //}

        public QueueMessageMoreDetailsPM GetQueueMessageMoreDetailsById(long Id, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);
            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            return QueueMessageMoreDetailsQuery.GetSingleQueueMessageMoreDetailsPM(Id);
        }

        public QueueMessageMoreDetailsList GetQueueMessageMoreDetailsListById(long Id, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);
            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            return QueueMessageMoreDetailsQuery.GetSingleQueueMessageMoreDetailsList(Id);
        }


        public IQueryable<QueueMessageMoreDetailsPM> GetQueueMessageMoreDetailsByField1(string Field1, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);

            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            return QueueMessageMoreDetailsQuery.GetIQueryableQueueMessageMoreDetailsPMByField1(Field1);
        }

        public IQueryable<QueueMessageMoreDetailsPM> GetQueueMessageMoreDetailsByField2(string Field2, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);

            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            return QueueMessageMoreDetailsQuery.GetIQueryableQueueMessageMoreDetailsPMByField1Field2(Field2,Field2);
        }

        public IQueryable<QueueMessageMoreDetailsList> GetQueueMessageMoreDetailsByField1List(string Field1, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);

            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            return QueueMessageMoreDetailsQuery.GetIQueryableQueueMessageMoreDetailsPMByField1List(Field1);
        }

        public IQueryable<QueueMessageMoreDetailsList> GetQueueMessageMoreDetailsByField2List(string Field2, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);

            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            return QueueMessageMoreDetailsQuery.GetIQueryableQueueMessageMoreDetailsPMByField2List(Field2);
        }

        

        [Query(HasSideEffects = true)]
        public IQueryable<QueueMessageMoreDetailsList> GetQueueMessageMoreDetailsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QueueMessageMoreDetails> QueueMessageMoreDetails = QueueMessageMoreDetailsRepository.GetQueueMessageMoreDetails();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            QueueMessageMoreDetails = filter.GetFilteredQuery<QueueMessageMoreDetails>(nonListQueryOperation, QueueMessageMoreDetails);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            IQueryable<QueueMessageMoreDetailsList> query2 = QueueMessageMoreDetailsQuery.GetIQueryableEntityList(QueueMessageMoreDetails);
            query2 = filter.GetFilteredQuery<QueueMessageMoreDetailsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QueueMessageMoreDetailsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QueueMessageMoreDetails", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QueueMessageMoreDetailsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QueueMessageMoreDetailsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QueueMessageMoreDetailsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QueueMessageMoreDetailsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QueueMessageMoreDetailsList, bool>(queryOperations, query2);
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

        public int GetQueueMessageMoreDetailsFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QueueMessageMoreDetailsRepository = new QueueMessageMoreDetailsRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QueueMessageMoreDetails> QueueMessageMoreDetails = QueueMessageMoreDetailsRepository.GetQueueMessageMoreDetails();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            QueueMessageMoreDetails = filter.GetFilteredQuery<QueueMessageMoreDetails>(nonListQueryOperation, QueueMessageMoreDetails);
            QueueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(QueueMessageMoreDetailsRepository);
            IQueryable<QueueMessageMoreDetailsList> query2 = QueueMessageMoreDetailsQuery.GetIQueryableEntityList(QueueMessageMoreDetails);
            query2 = filter.GetFilteredQuery<QueueMessageMoreDetailsList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQueueMessageMoreDetails(QueueMessageMoreDetailsPM entity)
        {
            
        }

        public void UpdateQueueMessageMoreDetails(QueueMessageMoreDetailsPM currentEntity)
        {
             
        }

        public void DeleteQueueMessageMoreDetails(QueueMessageMoreDetailsPM entity)
        {
            
        }

        
    }
}
