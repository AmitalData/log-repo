using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private ResponsibilityQuery responsibilityQuery;
        private ResponsibilityRepository responsibilityRepository;

        public IQueryable<Responsibility> GetResponsibilitys(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            responsibilityRepository = new ResponsibilityRepository(tenant);
            return responsibilityRepository.GetResponsibilities();
        }

        public IQueryable<Responsibility> GetResponsibilitysByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            responsibilityRepository = new ResponsibilityRepository(tenant);
            return responsibilityRepository.GetResponsibilities();
        }

        public void InsertResponsibility(Responsibility responsibility)
        {
            responsibilityRepository.Add(responsibility);
        }

        public void UpdateResponsibility(Responsibility currentresponsibility)
        {
            responsibilityRepository.Update(currentresponsibility);
        }

        public void DeleteResponsibility(Responsibility responsibility)
        {
            responsibilityRepository.Remove(responsibility);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ResponsibilityList> GetResponsibilityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            responsibilityRepository = new ResponsibilityRepository(tenant);
            responsibilityQuery = new ResponsibilityQuery(responsibilityRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Responsibility> Responsibilitys = responsibilityRepository.GetResponsibilities();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            Responsibilitys = filter.GetFilteredQuery<Responsibility>(nonListQueryOperation, Responsibilitys);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<ResponsibilityList> query2 = responsibilityQuery.GetIQueryableEntityList(Responsibilitys);
            query2 = filter.GetFilteredQuery<ResponsibilityList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ResponsibilityList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Responsibility", tenant).ToList();
                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ResponsibilityList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ResponsibilityList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ResponsibilityList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ResponsibilityList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ResponsibilityList, bool>(queryOperations, query2);
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

        public int GetResponsibilityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
 
            responsibilityRepository = new ResponsibilityRepository(tenant);
            responsibilityQuery = new ResponsibilityQuery(responsibilityRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Responsibility> Responsibilitys = responsibilityRepository.GetResponsibilities();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            Responsibilitys = filter.GetFilteredQuery<Responsibility>(nonListQueryOperation, Responsibilitys);

            IQueryable<ResponsibilityList> query2 = responsibilityQuery.GetIQueryableEntityList(Responsibilitys);

            query2 = filter.GetFilteredQuery<ResponsibilityList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}