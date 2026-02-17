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
    public partial class PartnersDomainService
    {
        public IQueryable<TarrifType> GetTarrifTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifTypeRepository = new TarrifTypeRepository(tenant);
            return tarrifTypeRepository.GetTarrifTypes();
        }

        public TarrifTypePM GetSingleTarrifTypePM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifTypeQuery = new TarrifTypeQuery(tenant);
            return tarrifTypeQuery.GetSingleTarrifTypePM(code);
        }

        public TarrifType GetSingleTarrifType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifTypeRepository = new TarrifTypeRepository(tenant);
            return tarrifTypeRepository.GetSingleTarrifType(code);
        }

        public TarrifTypeList GetSingleTarrifTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifTypeRepository = new TarrifTypeRepository(tenant);
            TarrifTypeList tarrifTypeList = null;
            TarrifType tarrifType = tarrifTypeRepository.GetSingleTarrifType(code);

            if (tarrifType != null)
            {
                List<TarrifType> singleEntityList = new List<TarrifType>();
                singleEntityList.Add(tarrifType);

                tarrifTypeQuery = new TarrifTypeQuery(tarrifTypeRepository);
                IQueryable<TarrifType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TarrifTypeList> iQueryableEntityList = tarrifTypeQuery.GetIQueryableEntityList(iQueryable);
                tarrifTypeList = iQueryableEntityList.FirstOrDefault();
            }

            return tarrifTypeList;
        }

        public IQueryable<TarrifTypeList> GetTarrifTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifTypeRepository = new TarrifTypeRepository(tenant);
            tarrifTypeQuery = new TarrifTypeQuery(tarrifTypeRepository);

            IQueryable<TarrifType> iQueryable = tarrifTypeRepository.GetTarrifTypes();
            IQueryable<TarrifTypeList> query2 = tarrifTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TarrifTypeList> GetTarrifTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifTypeRepository = new TarrifTypeRepository(tenant);
            tarrifTypeQuery = new TarrifTypeQuery(tarrifTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifType> iQueryable = tarrifTypeRepository.GetTarrifTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifType>(nonListQueryOperation, iQueryable);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TarrifTypeList> query2 = tarrifTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TarrifTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TarrifType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifTypeList, bool>(queryOperations, query2);
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

        public int GetTarrifTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifTypeRepository = new TarrifTypeRepository(tenant);
            tarrifTypeQuery = new TarrifTypeQuery(tarrifTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifType> iQueryable = tarrifTypeRepository.GetTarrifTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifType>(nonListQueryOperation, iQueryable);

            IQueryable<TarrifTypeList> query2 = tarrifTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertTarrifType(TarrifType entity)
        {
            tarrifTypeRepository.Add(entity);
        }

        public void UpdateTarrifType(TarrifType currentEntity)
        {
            tarrifTypeRepository.Update(currentEntity);
        }

        public void DeleteTarrifType(TarrifType entity)
        {
            tarrifTypeRepository.Remove(entity);
        }
    }
}