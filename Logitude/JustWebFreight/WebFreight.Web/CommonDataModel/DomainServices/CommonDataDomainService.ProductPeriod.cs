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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateProductPeriodList(ProductPeriodList currentEntity)
        {
        }

        public IQueryable<ProductPeriod> GetProductPeriods(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            return productPeriodRepository.GetProductPeriods();
        }

        public IQueryable<ProductPeriod> GetProductPeriodsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            return productPeriodRepository.GetProductPeriods();
        }

        public ProductPeriodPM GetSingleProductPeriod(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodQuery = new ProductPeriodQuery(tenant);
            return productPeriodQuery.GetSingleProductPeriodPM(code);
        }

        public ProductPeriodList GetSingleProductPeriodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            ProductPeriodList ProductPeriodList = null;
            ProductPeriod ProductPeriod = productPeriodRepository.GetSingleProductPeriod(code);

            if (ProductPeriod != null)
            {
                List<ProductPeriod> singleEntityList = new List<ProductPeriod>();
                singleEntityList.Add(ProductPeriod);

                productPeriodQuery = new ProductPeriodQuery(productPeriodRepository);
                IQueryable<ProductPeriod> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ProductPeriodList> iQueryableEntityList = productPeriodQuery.GetIQueryableEntityList(iQueryable);
                ProductPeriodList = iQueryableEntityList.FirstOrDefault();
            }
            return ProductPeriodList;
        }

        public IQueryable<ProductPeriodList> GetProductPeriodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            productPeriodQuery = new ProductPeriodQuery(productPeriodRepository);

            IQueryable<ProductPeriod> iQueryable = productPeriodRepository.GetProductPeriods();
            IQueryable<ProductPeriodList> query2 = productPeriodQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ProductPeriodList> GetProductPeriodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            productPeriodQuery = new ProductPeriodQuery(productPeriodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ProductPeriod> iQueryable = productPeriodRepository.GetProductPeriods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ProductPeriod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ProductPeriodList> query2 = productPeriodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ProductPeriodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ProductPeriodList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ProductPeriod", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ProductPeriodList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<ProductPeriodList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ProductPeriodList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ProductPeriodList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ProductPeriodList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ProductPeriodList, bool>(queryOperations, query2);
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

        public int GetProductPeriodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            productPeriodQuery = new ProductPeriodQuery(productPeriodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ProductPeriod> iQueryable = productPeriodRepository.GetProductPeriods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ProductPeriod>(nonListQueryOperation, iQueryable);

            IQueryable<ProductPeriodList> query2 = productPeriodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ProductPeriodList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<ProductPeriod> GetProductPeriodsByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return productPeriodRepository.GetProductPeriods().Where(d => d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return productPeriodRepository.GetProductPeriods().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return productPeriodRepository.GetProductPeriods();
            }
        }

        public IQueryable<ProductPeriod> GetSingleProductPeriodByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            productPeriodRepository = new ProductPeriodRepository(tenant);
            if (byCode)
            {
                return productPeriodRepository.GetProductPeriods().Where(d => d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return productPeriodRepository.GetProductPeriods().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertProductPeriod(ProductPeriod entity)
        {
            productPeriodRepository.Add(entity);
        }

        public void UpdateProductPeriod(ProductPeriod currentEntity)
        {
            productPeriodRepository.Update(currentEntity);
        }

        public void DeleteProductPeriod(ProductPeriod entity)
        {
            productPeriodRepository.Remove(entity);
        }
    }
}