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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateRateClassList(RateClassList currentEntity)
        {
        }

        public IQueryable<RateClass> GetRateClasses(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rateClassRepository = new RateClassRepository(tenant);
            return rateClassRepository.GetRateClasses();
        }

        public IQueryable<RateClass> RateClassesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rateClassRepository = new RateClassRepository(tenant);
            return rateClassRepository.GetRateClasses();
        }

        public RateClassPM GetSingleRateClass(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rateClassQuery = new RateClassQuery(tenant);
            return rateClassQuery.GetSinglePM(code);
        }

        public RateClassList GetSingleRateClassList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rateClassRepository = new RateClassRepository(tenant);
            RateClassList rateClassList = null;
            RateClass rateClass = rateClassRepository.GetSingleRateClass(code);

            if (rateClass != null)
            {
                List<RateClass> singleEntityList = new List<RateClass>();
                singleEntityList.Add(rateClass);

                rateClassQuery = new RateClassQuery(rateClassRepository);
                IQueryable<RateClass> iQueryable = singleEntityList.AsQueryable();
                IQueryable<RateClassList> iQueryableEntityList = rateClassQuery.GetIQueryableEntityList(iQueryable);
                rateClassList = iQueryableEntityList.FirstOrDefault();
            }
            return rateClassList;
        }

        public IQueryable<RateClassList> GetRateClassLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rateClassRepository = new RateClassRepository(tenant);
            rateClassQuery = new RateClassQuery(rateClassRepository);

            IQueryable<RateClass> iQueryable = rateClassRepository.GetRateClasses();
            IQueryable<RateClassList> query2 = rateClassQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<RateClassList> GetRateClassFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rateClassRepository = new RateClassRepository(tenant);
            rateClassQuery = new RateClassQuery(rateClassRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RateClass> iQueryable = rateClassRepository.GetRateClasses();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RateClass>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<RateClassList> query2 = rateClassQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<RateClassList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RateClassList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("RateClass", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RateClassList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RateClassList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RateClassList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RateClassList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RateClassList, bool>(queryOperations, query2);
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

        public int GetRateClassFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            rateClassRepository = new RateClassRepository(tenant);
            rateClassQuery = new RateClassQuery(rateClassRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RateClass> iQueryable = rateClassRepository.GetRateClasses();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RateClass>(nonListQueryOperation, iQueryable);

            IQueryable<RateClassList> query2 = rateClassQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<RateClassList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertRateClass(RateClass entity)
        {
            rateClassRepository.Add(entity);
        }

        public void UpdateRateClass(RateClass currentEntity)
        {
            rateClassRepository.Update(currentEntity);
        }

        public void DeleteRateClass(RateClass entity)
        {
            rateClassRepository.Remove(entity);
        }
    }
}