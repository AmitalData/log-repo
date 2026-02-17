using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private NextLegRepository nextLegRepository;
        private NextLegQuery nextLegQuery;

        public IQueryable<NextLeg> GetNextLegsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            nextLegRepository = new NextLegRepository(tenant);
            return nextLegRepository.GetNextLegs();
        }

        public NextLegPM GetSingleNextLegPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            nextLegQuery = new NextLegQuery(tenant);
            return nextLegQuery.GetSingleNextLegPM(code);
        }

        public NextLeg GetSingleNextLeg(string code, int tenant)
        {
            nextLegRepository = new NextLegRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return nextLegRepository.GetSingleNextLeg(code);
        }

        public NextLegList GetSingleNextLegList(string code, int tenant)
        {
            nextLegRepository = new NextLegRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            NextLeg entityPM = nextLegRepository.GetSingleNextLeg(code);
            NextLegList entityList = new NextLegList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
            };
            return entityList;
        }

        public IQueryable<NextLegList> GetNextLegLists(int tenant)
        {
            nextLegRepository = new NextLegRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<NextLeg> iQueryable = nextLegRepository.GetNextLegs();
            var query2 = from entity in iQueryable
                         select new NextLegList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<NextLegList> GetNextLegFilters(byte[] xmlFilters, int tenant)
        {
            nextLegRepository = new NextLegRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<NextLeg> iQueryable = nextLegRepository.GetNextLegs();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<NextLeg>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new NextLegList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<NextLegList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(NextLegList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PartnerType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<NextLegList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<NextLegList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<NextLegList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<NextLegList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<NextLegList, bool>(queryOperations, query2);
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

        public int GetNextLegCount(byte[] xmlFilters, int tenant)
        {
            nextLegRepository = new NextLegRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<NextLeg> iQueryable = nextLegRepository.GetNextLegs();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<NextLeg>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new NextLegList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<NextLegList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertNextLeg(NextLeg entity)
        {
            nextLegRepository.Add(entity);
        }

        public void UpdateNextLeg(NextLeg currentEntity)
        {
            nextLegRepository.Update(currentEntity);
        }

        public void DeleteNextLeg(NextLeg entity)
        {
            nextLegRepository.Remove(entity);
        }

    }
}