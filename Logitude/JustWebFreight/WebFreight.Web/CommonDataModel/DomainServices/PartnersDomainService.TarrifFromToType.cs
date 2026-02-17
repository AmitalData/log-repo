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
        public IQueryable<TarrifFromToType> GetTarrifFromToTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifFromToTypeRepository = new TarrifFromToTypeRepository(tenant);
            return tarrifFromToTypeRepository.GetTarrifFromToTypes();
        }

        public TarrifFromToTypePM GetSingleTarrifFromToTypePM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifFromToTypeQuery = new TarrifFromToTypeQuery(tenant);
            return tarrifFromToTypeQuery.GetSingleTarrifFromToTypePM(code);
        }

        public TarrifFromToType GetSingleTarrifFromToType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifFromToTypeRepository = new TarrifFromToTypeRepository(tenant);
            return tarrifFromToTypeRepository.GetSingleTarrifFromToType(code);
        }

        public TarrifFromToTypeList GetSingleTarrifFromToTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifFromToTypeRepository = new TarrifFromToTypeRepository(tenant);
            TarrifFromToTypeList tarrifFromToTypeList = null;
            TarrifFromToType tarrifFromToType = tarrifFromToTypeRepository.GetSingleTarrifFromToType(code);

            if (tarrifFromToType != null)
            {
                List<TarrifFromToType> singleEntityList = new List<TarrifFromToType>();
                singleEntityList.Add(tarrifFromToType);

                tarrifFromToTypeQuery = new TarrifFromToTypeQuery(tarrifFromToTypeRepository);
                IQueryable<TarrifFromToType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TarrifFromToTypeList> iQueryableEntityList = tarrifFromToTypeQuery.GetIQueryableEntityList(iQueryable);
                tarrifFromToTypeList = iQueryableEntityList.FirstOrDefault();
            }

            return tarrifFromToTypeList;
        }

        public IQueryable<TarrifFromToTypeList> GetTarrifFromToTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifFromToTypeRepository = new TarrifFromToTypeRepository(tenant);
            tarrifFromToTypeQuery = new TarrifFromToTypeQuery(tarrifFromToTypeRepository);

            IQueryable<TarrifFromToType> iQueryable = tarrifFromToTypeRepository.GetTarrifFromToTypes();
            IQueryable<TarrifFromToTypeList> query2 = tarrifFromToTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TarrifFromToTypeList> GetTarrifFromToTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifFromToTypeRepository = new TarrifFromToTypeRepository(tenant);
            tarrifFromToTypeQuery = new TarrifFromToTypeQuery(tarrifFromToTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifFromToType> iQueryable = tarrifFromToTypeRepository.GetTarrifFromToTypes();
            
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifFromToType>(nonListQueryOperation, iQueryable);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TarrifFromToTypeList> query2 = tarrifFromToTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifFromToTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TarrifFromToTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TarrifFromToType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifFromToTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifFromToTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifFromToTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifFromToTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifFromToTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifFromToTypeList, bool>(queryOperations, query2);
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

        public int GetTarrifFromToTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            tarrifFromToTypeRepository = new TarrifFromToTypeRepository(tenant);
            tarrifFromToTypeQuery = new TarrifFromToTypeQuery(tarrifFromToTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifFromToType> iQueryable = tarrifFromToTypeRepository.GetTarrifFromToTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifFromToType>(nonListQueryOperation, iQueryable);

            IQueryable<TarrifFromToTypeList> query2 = tarrifFromToTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifFromToTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertTarrifFromToType(TarrifFromToType entity)
        {
            tarrifFromToTypeRepository.Add(entity);
        }

        public void UpdateTarrifFromToType(TarrifFromToType currentEntity)
        {
            tarrifFromToTypeRepository.Update(currentEntity);
        }

        public void DeleteTarrifFromToType(TarrifFromToType entity)
        {
            tarrifFromToTypeRepository.Remove(entity);
        }
    }
}