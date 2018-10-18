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
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private AWBChargesCodeRepository aWBChargeCodesRepository;
        private AWBChargesCodeQuery awbChargeCodesQuery;

        public AWBChargesCode GetaSingleAWBChargeCode(string code, int tenant)
        {
            aWBChargeCodesRepository = new AWBChargesCodeRepository(tenant);
            return aWBChargeCodesRepository.GetSingleAWBChargeCode(code);
        }
        public AWBChargesCodePM GetSingleAWBChargeCode(string id, int tenant)
        {
            
            SecurityUtility.AuthenticationOnTenant(tenant);
            awbChargeCodesQuery = new AWBChargesCodeQuery(tenant);
            return awbChargeCodesQuery.GetSingleAWBChargeCodePM(id);
        }

        public AWBChargesCodeList GetSingleAWBChargeCodeList(string id, int tenant)
        {
            
            SecurityUtility.AuthenticationOnTenant(tenant);
            awbChargeCodesQuery = new AWBChargesCodeQuery(tenant);
            AWBChargesCodePM entityPM = awbChargeCodesQuery.GetSingleAWBChargeCodePM(id);
            AWBChargesCodeList entityList = new AWBChargesCodeList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,

                SearchFields = entityPM.SearchFields,
            };
            return entityList;
        }

        public IQueryable<AWBChargesCodeList> GetAWBChargeCodeLists(int tenant)
        {
            aWBChargeCodesRepository = new AWBChargesCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<AWBChargesCode> iQueryable = aWBChargeCodesRepository.GetAWBChargeCodes();
            var query2 = from entity in iQueryable
                         select new AWBChargesCodeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,

                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBChargesCodeList> GetAWBChargesCodeFilters(byte[] xmlFilters, int tenant)
        {
            aWBChargeCodesRepository = new AWBChargesCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBChargesCode> iQueryable = aWBChargeCodesRepository.GetAWBChargeCodes();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBChargesCode>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new AWBChargesCodeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,

                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBChargesCodeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBChargesCodeList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBChargesCode", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AWBChargesCodeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBChargesCodeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBChargesCodeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBChargesCodeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBChargesCodeList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAWBChargesCodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            aWBChargeCodesRepository = new AWBChargesCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBChargesCode> iQueryable = aWBChargeCodesRepository.GetAWBChargeCodes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBChargesCode>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new AWBChargesCodeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,

                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBChargesCodeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAWBChargeCode(AWBChargesCode entity)
        {
            aWBChargeCodesRepository.Add(entity);
        }

        public void UpdateAWBChargeCode(AWBChargesCode currentEntity)
        {
            aWBChargeCodesRepository.Update(currentEntity);
        }

        public void DeleteAWBChargeCode(AWBChargesCode entity)
        {
            aWBChargeCodesRepository.Remove(entity);
        }

    }
}