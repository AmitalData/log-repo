using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateDistributorList(DistributorList currentEntity, int tenant)
        {
        }

        public DistributorPM GetSingleDistributor(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            distributorQuery = new DistributorQuery(tenant);
            return distributorQuery.GetSingleDistributorPM(code);
        }

        public DistributorList GetSingleDistributorList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            distributorRepository = new DistributorRepository(tenant);
            DistributorList DistributorList = null;
            Distributor Distributor = distributorRepository.GetSingleDistributor(code, tenant);

            if (Distributor != null)
            {
                List<Distributor> singleEntityList = new List<Distributor>();
                singleEntityList.Add(Distributor);

                distributorQuery = new DistributorQuery(distributorRepository);
                IQueryable<Distributor> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DistributorList> iQueryableEntityList = distributorQuery.GetIQueryableEntityList(iQueryable);
                DistributorList = iQueryableEntityList.FirstOrDefault();
            }
            return DistributorList;
        }

        public IQueryable<DistributorList> GetDistributorLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            distributorRepository = new DistributorRepository(tenant);
            distributorQuery = new DistributorQuery(distributorRepository);

            IQueryable<Distributor> iQueryable = distributorRepository.GetDistributors();
            IQueryable<DistributorList> query2 = distributorQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DistributorList> GetDistributorFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            distributorRepository = new DistributorRepository(tenant);
            distributorQuery = new DistributorQuery(distributorRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Distributor> iQueryable = distributorRepository.GetDistributors();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Distributor>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DistributorList> query2 = distributorQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DistributorList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DistributorList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Distributor", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DistributorList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<DistributorList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DistributorList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DistributorList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DistributorList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DistributorList, bool>(queryOperations, query2);
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

        public int GetDistributorFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            distributorRepository = new DistributorRepository(tenant);
            distributorQuery = new DistributorQuery(distributorRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Distributor> iQueryable = distributorRepository.GetDistributors();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Distributor>(nonListQueryOperation, iQueryable);

            IQueryable<DistributorList> query2 = distributorQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DistributorList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }


        public void InsertDistributor(DistributorPM entity, int tenant)
        {
            if (this.objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            DistributorService service = new DistributorService(objectContext, tenant);
            service.Create(entity);
        }

        public void UpdateDistributor(DistributorPM currentEntity, int tenant)
        {
            if (this.objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            DistributorService service = new DistributorService(objectContext, tenant);
            service.Update(currentEntity);
        }

    }
}