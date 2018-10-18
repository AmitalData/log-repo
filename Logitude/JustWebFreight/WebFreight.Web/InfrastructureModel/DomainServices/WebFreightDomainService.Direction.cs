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
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateDirectionList(DirectionList currentEntity)
        {
        }

        public IQueryable<Direction> GetDirections(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            return directionsRepository.GetDirections();
        }

        public IQueryable<Direction> GetDirectionsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            return directionsRepository.GetDirections();
        }

        public IQueryable<Direction> GetFirstDirectionsByTenant(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            input = input.ToUpper();
            return directionsRepository.GetDirections().Where(p => p.Id.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public DirectionPM GetSingleDirection(string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            directionQuery = new DirectionQuery(directionsRepository);
            return directionQuery.GetSingleDirectionPM(name);
        }

        public DirectionList GetSingleDirectionList(string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            DirectionList directionList = null;
            Direction direction = directionsRepository.GetSingleDirection(name);

            if (direction != null)
            {
                List<Direction> singleEntityList = new List<Direction>();
                singleEntityList.Add(direction);

                IQueryable<Direction> iQueryable = singleEntityList.AsQueryable();
                directionQuery = new DirectionQuery(directionsRepository);
                IQueryable<DirectionList> iQueryableEntityList = directionQuery.GetIQueryableEntityList(iQueryable);
                directionList = iQueryableEntityList.FirstOrDefault();
            }
            return directionList;
        }

        public IQueryable<DirectionList> GetDirectionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            IQueryable<Direction> iQueryable = directionsRepository.GetDirections();
            directionQuery = new DirectionQuery(directionsRepository);
            IQueryable<DirectionList> query2 = directionQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DirectionList> GetDirectionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Direction> iQueryable = directionsRepository.GetDirections();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Direction>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            directionQuery = new DirectionQuery(directionsRepository);
            IQueryable<DirectionList> query2 = directionQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DirectionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DirectionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Direction", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DirectionList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DirectionList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DirectionList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DirectionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DirectionList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetDirectionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            directionsRepository = new DirectionRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Direction> iQueryable = directionsRepository.GetDirections();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Direction>(nonListQueryOperation, iQueryable);

            directionQuery = new DirectionQuery(directionsRepository);
            IQueryable<DirectionList> query2 = directionQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DirectionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertDirection(Direction entity)
        {
            directionsRepository.Add(entity);
        }

        public void UpdateDirection(Direction currentEntity)
        {
            directionsRepository.Update(currentEntity);
        }

        public void DeleteDirection(Direction entity)
        {
            directionsRepository.Remove(entity);
        }
    }
}