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
        private VatMandatoryTypeRepository vatMandatoryTypeRepository;
        public void UpdateVatMandatoryTypeTypeList(VatMandatoryTypeList currentEntity)
        {

        }

        public VatMandatoryTypeList GetSingleVatMandatoryTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatMandatoryTypeRepository = new VatMandatoryTypeRepository(tenant);

            VatMandatoryTypeList myResult = null;
            VatMandatoryType entity = vatMandatoryTypeRepository.GetSingleVatMandatoryType(code);

            if (entity != null)
            {
                myResult = new VatMandatoryTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    ViewOrder = entity.ViewOrder,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<VatMandatoryTypeList> GetVatMandatoryTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatMandatoryTypeRepository = new VatMandatoryTypeRepository(tenant);

            IQueryable<VatMandatoryType> iQueryable = vatMandatoryTypeRepository.GetVatMandatoryTypes();

            var query2 = from entity in iQueryable
                         select new VatMandatoryTypeList()
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
        public IQueryable<VatMandatoryTypeList> GetVatMandatoryTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatMandatoryTypeRepository = new VatMandatoryTypeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VatMandatoryType> iQueryable = vatMandatoryTypeRepository.GetVatMandatoryTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VatMandatoryType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new VatMandatoryTypeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             ViewOrder = entity.ViewOrder,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<VatMandatoryTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VatMandatoryTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("VatMandatoryType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VatMandatoryTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VatMandatoryTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VatMandatoryTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VatMandatoryTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VatMandatoryTypeList, bool>(queryOperations, query2);
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

        public int GetVatMandatoryTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            vatMandatoryTypeRepository = new VatMandatoryTypeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VatMandatoryType> iQueryable = vatMandatoryTypeRepository.GetVatMandatoryTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VatMandatoryType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new VatMandatoryTypeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             ViewOrder = entity.ViewOrder,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<VatMandatoryTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}