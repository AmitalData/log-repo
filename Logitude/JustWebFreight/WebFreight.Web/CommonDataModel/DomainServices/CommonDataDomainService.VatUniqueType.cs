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
using Logitude.BL.CommonDataModel.EntityLists;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {

        private VatUniqueTypeRepository vatUniqueTypeRepository;
        public void UpdateVatUniqueTypeTypeList(VatUniqueTypeList currentEntity)
        {

        }

        public VatUniqueTypeList GetSingleVatUniqueTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatUniqueTypeRepository = new VatUniqueTypeRepository(tenant);

            VatUniqueTypeList myResult = null;
            VatUniqueType entity = vatUniqueTypeRepository.GetSingleVatUniqueType(code);

            if (entity != null)
            {
                myResult = new VatUniqueTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<VatUniqueTypeList> GetVatUniqueTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatUniqueTypeRepository = new VatUniqueTypeRepository(tenant);

            IQueryable<VatUniqueType> iQueryable = vatUniqueTypeRepository.GetVatUniqueTypes();

            var query2 = from entity in iQueryable
                         select new VatUniqueTypeList()
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
        public IQueryable<VatUniqueTypeList> GetVatUniqueTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatUniqueTypeRepository = new VatUniqueTypeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VatUniqueType> iQueryable = vatUniqueTypeRepository.GetVatUniqueTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VatUniqueType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new VatUniqueTypeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             ViewOrder = entity.ViewOrder,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<VatUniqueTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VatUniqueTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("VatUniqueType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VatUniqueTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VatUniqueTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VatUniqueTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VatUniqueTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VatUniqueTypeList, bool>(queryOperations, query2);
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

        public int GetVatUniqueTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatUniqueTypeRepository = new VatUniqueTypeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VatUniqueType> iQueryable = vatUniqueTypeRepository.GetVatUniqueTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VatUniqueType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new VatUniqueTypeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             ViewOrder = entity.ViewOrder,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<VatUniqueTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}