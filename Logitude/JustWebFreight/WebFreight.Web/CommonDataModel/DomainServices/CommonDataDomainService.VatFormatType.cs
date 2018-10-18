using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private VatFormatTypeRepository vatFormatTypeRepository;

        public void UpdateVatFormatTypeTypeList(VatFormatTypeList currentEntity)
        {

        }

        public VatFormatTypeList GetSingleVatFormatTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatFormatTypeRepository = new VatFormatTypeRepository(tenant);

            VatFormatTypeList myResult = null;
            VatFormatType entity = vatFormatTypeRepository.GetSingleVatFormatType(code);

            if (entity != null)
            {
                myResult = new VatFormatTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    ViewOrder = entity.ViewOrder,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<VatFormatTypeList> GetVatFormatTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatFormatTypeRepository = new VatFormatTypeRepository(tenant);

            IQueryable<VatFormatType> iQueryable = vatFormatTypeRepository.GetVatFormatTypes();

            var query2 = from entity in iQueryable
                         select new VatFormatTypeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             ViewOrder = entity.ViewOrder,
                             SearchFields = entity.SearchFields,
                         };

            if (query2 != null)
            {
                query2 = query2.OrderBy(d => d.ViewOrder);
            }

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<VatFormatTypeList> GetVatFormatTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatFormatTypeRepository = new VatFormatTypeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VatFormatType> iQueryable = vatFormatTypeRepository.GetVatFormatTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VatFormatType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new VatFormatTypeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             ViewOrder = entity.ViewOrder,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<VatFormatTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VatFormatTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("VatFormatType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VatFormatTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VatFormatTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VatFormatTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VatFormatTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VatFormatTypeList, bool>(queryOperations, query2);
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
                query2 = query2.OrderBy(d => d.ViewOrder);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetVatFormatTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatFormatTypeRepository = new VatFormatTypeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VatFormatType> iQueryable = vatFormatTypeRepository.GetVatFormatTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VatFormatType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new VatFormatTypeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             ViewOrder = entity.ViewOrder,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<VatFormatTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}