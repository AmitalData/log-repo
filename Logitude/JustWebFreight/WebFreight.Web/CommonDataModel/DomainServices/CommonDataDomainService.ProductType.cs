using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
	public partial class CommonDataDomainService
	{
        public void UpdateProductTypeList(ProductTypeList currentEntity)
        {
        }

        public IQueryable<ProductType> GetProductTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            return productTypeRepository.GetProductTypes();
        }

        public IQueryable<ProductType> GetProductTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            return productTypeRepository.GetProductTypes();
        }

        public ProductTypePM GetSingleProductType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeQuery = new ProductTypeQuery(tenant);
            return productTypeQuery.GetSinglePM(code,tenant);
        }

        public ProductTypeList GetSingleProductTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            ProductTypeList ProductTypeList = null;
            //ProductType ProductType = productTypeRepository.GetSingleProductType(code);

            //if (ProductType != null)
            //{
                //List<ProductType> singleEntityList = new List<ProductType>();
                //singleEntityList.Add(ProductType);

                productTypeQuery = new ProductTypeQuery(productTypeRepository);
                IQueryable<ProductType> iQueryable = productTypeRepository.GetProductTypes();
                
                IQueryable<ProductTypeList> iQueryableEntityList = productTypeQuery.GetIQueryableEntityList(iQueryable, tenant);
                ProductTypeList = iQueryableEntityList.Where(d=>d.Code==code).FirstOrDefault();

           // }
            return ProductTypeList;
        }

        public IQueryable<ProductTypeList> GetProductTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            productTypeQuery = new ProductTypeQuery(productTypeRepository);

            IQueryable<ProductType> iQueryable = productTypeRepository.GetActiveProductTypes(tenant);
            IQueryable<ProductTypeList> query2 = productTypeQuery.GetIQueryableEntityList(iQueryable, tenant);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ProductTypeList> GetProductTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            productTypeQuery = new ProductTypeQuery(productTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ProductType> iQueryable = productTypeRepository.GetProductTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ProductType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ProductTypeList> query2 = productTypeQuery.GetIQueryableEntityList(iQueryable, tenant);
            query2 = filter.GetFilteredQuery<ProductTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ProductTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ProductType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ProductTypeList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<ProductTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ProductTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ProductTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ProductTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ProductTypeList, bool>(queryOperations, query2);
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

        public int GetProductTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            productTypeQuery = new ProductTypeQuery(productTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ProductType> iQueryable = productTypeRepository.GetProductTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ProductType>(nonListQueryOperation, iQueryable);

            IQueryable<ProductTypeList> query2 = productTypeQuery.GetIQueryableEntityList(iQueryable, tenant);
            query2 = filter.GetFilteredQuery<ProductTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<ProductType> GetProductTypesByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return productTypeRepository.GetProductTypes().Where(d => d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return productTypeRepository.GetProductTypes().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return productTypeRepository.GetProductTypes();
            }
        }

        public IQueryable<ProductType> GetSingleProductTypeByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productTypeRepository = new ProductTypeRepository(tenant);
            if (byCode)
            {
                return productTypeRepository.GetProductTypes().Where(d => d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return productTypeRepository.GetProductTypes().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertProductType(ProductType entity)
        {
            productTypeRepository.Add(entity);
        }

        public void UpdateProductType(ProductTypePM currentEntity)
        {


            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }


            ProductTypeService service = new ProductTypeService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);
          
        }

        public void DeleteProductType(ProductType entity)
        {
            productTypeRepository.Remove(entity);
        }
	}
}