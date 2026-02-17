using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateTransportModeList(TransportModeList currentEntity)
        {
        }

        public IQueryable<TransportMode> GetTransportModes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            return transportModeRepository.GetTransportModes();
        }

        public TransportMode GetSingleTransportMode(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            return transportModeRepository.GetSingleTransportMode(id);
        }

        public IQueryable<TransportMode> GetTransportModesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            return transportModeRepository.GetTransportModes();
        }

        public TransportModePM GetSingleTransportModePM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            transportModeQuery = new TransportModeQuery(transportModeRepository);
            return transportModeQuery.GetSinglePM(id, tenant);
        }

        public TransportModeList GetSingleTransportModeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            TransportModeList transportModeList = null;
            TransportMode transportMode = transportModeRepository.GetSingleTransportMode(id);

            if (transportMode != null)
            {
                List<TransportMode> singleEntityList = new List<TransportMode>();
                singleEntityList.Add(transportMode);

                IQueryable<TransportMode> iQueryable = singleEntityList.AsQueryable();
                transportModeQuery = new TransportModeQuery(transportModeRepository);
                IQueryable<TransportModeList> iQueryableEntityList = transportModeQuery.GetIQueryableEntityList(iQueryable);
                transportModeList = iQueryableEntityList.FirstOrDefault();
            }
            return transportModeList;
        }

        public IQueryable<TransportModeList> GetTransportModeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            IQueryable<TransportMode> iQueryable = transportModeRepository.GetTransportModes();
            transportModeQuery = new TransportModeQuery(transportModeRepository);
            IQueryable<TransportModeList> query2 = transportModeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TransportModeList> GetTransportModeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TransportMode> iQueryable = transportModeRepository.GetTransportModes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TransportMode>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            transportModeQuery = new TransportModeQuery(transportModeRepository);
            IQueryable<TransportModeList> query2 = transportModeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TransportModeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TransportModeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TransportMode", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TransportModeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TransportModeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TransportModeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TransportModeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TransportModeList, bool>(queryOperations, query2);
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

        public int GetTransportModeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TransportMode> iQueryable = transportModeRepository.GetTransportModes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TransportMode>(nonListQueryOperation, iQueryable);

            transportModeQuery = new TransportModeQuery(transportModeRepository);
            IQueryable<TransportModeList> query2 = transportModeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TransportModeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<TransportMode> GetFirstTransportModesByTenant(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            transportModeRepository = new TransportModeRepository(tenant);
            input = input.ToUpper();
            return transportModeRepository.GetTransportModes().Where(p => p.Id.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public void InsertTransportMode(TransportMode entity)
        {
            transportModeRepository.Add(entity);
        }

        public void UpdateTransportMode(TransportMode currentEntity)
        {
            transportModeRepository.Update(currentEntity);
        }

        public void DeleteTransportMode(TransportMode entity)
        {
            transportModeRepository.Remove(entity);
        }
    }
}