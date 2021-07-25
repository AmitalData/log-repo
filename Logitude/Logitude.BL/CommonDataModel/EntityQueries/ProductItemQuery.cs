using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ProductItemQuery
    {
        ProductItemRepository repository;

        public ProductItemQuery()
        {
            this.repository = new ProductItemRepository();
        }

        public ProductItemQuery(int tenant)
        {
            this.repository = new ProductItemRepository(tenant);
        }

        public ProductItemQuery(ProductItemRepository repository)
        {
            this.repository = repository;
        }

        public ProductItemPM GetSinglePM(string id, int tenant)
        {
            ProductItemPM result = null;
            ProductItem entityPoco = repository.GetSingleProductItem(id, tenant);

            if (entityPoco != null)
            {
                result = new ProductItemPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    CustomerId = entityPoco.CustomerId,
                    SKU = entityPoco.SKU,
                    InActive = entityPoco.InActive,
                    Description = entityPoco.Description,
                    Name = entityPoco.Name,
                    SearchFields = entityPoco.SearchFields,
                    Brand = entityPoco.Brand,              
                    ASIN = entityPoco.ASIN,
                    UPC = entityPoco.UPC,
                    OriginCountryId = entityPoco.OriginCountryId,
                    OriginCountryName = entityPoco.OriginCountry == null ? null : entityPoco.OriginCountry.EnglishName,
                };

                HTSCodeQuery hTSCodeQuery = new HTSCodeQuery(tenant);
                result.HTSCodes = hTSCodeQuery.GetHTSCodePMsByProductItemIds(entityPoco.Id, tenant);                
            }

            return result;
        }

        public IQueryable<ProductItemList> GetIQueryableEntityList(IQueryable<ProductItem> iQueryable)
        {
            IQueryable<ProductItemList> result = from entity in iQueryable
                                               select new ProductItemList()
                                               {
                                                   Id = entity.Id,
                                                   Tenant = entity.Tenant,
                                                   CustomerId = entity.CustomerId,
                                                   SKU = entity.SKU,
                                                   Brand = entity.Brand,
                                                   InActive = entity.InActive,
                                                   Description = entity.Description,
                                                   Name = entity.Name,
                                                   SearchFields = entity.SearchFields,
                                                   ASIN = entity.ASIN,
                                                   UPC = entity.UPC,
                                                   OriginCountryId = entity.OriginCountryId,
                                                   OriginCountryName = entity.OriginCountry == null ? null : entity.OriginCountry.EnglishName,
                                               };
            return result;
        }

        public List<ProductItemPM> GetProductItemPMsByCustomerId(string customerId,int tenant)
        {
            List<ProductItemPM> productItems = (from a in repository.context.ProductItems.Include("OriginCountry")
                                                where a.Tenant == tenant && a.CustomerId == customerId
                                                select new ProductItemPM()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    CustomerId = a.CustomerId,
                                                    SKU = a.SKU,
                                                    Brand = a.Brand,
                                                    InActive = a.InActive,
                                                    Description = a.Description,
                                                    Name = a.Name,
                                                    SearchFields = a.SearchFields,
                                                    ASIN = a.ASIN,
                                                    UPC = a.UPC,
                                                    OriginCountryId = a.OriginCountryId,
                                                    OriginCountryName = a.OriginCountry == null ? null : a.OriginCountry.EnglishName,
                                                }).ToList();
            if (productItems != null)
            { 
                foreach (ProductItemPM productItem in productItems)
                {
                  List<HTSCodePM> hTSCodes = (from a in repository.context.HTSCodes
                                              join country in repository.context.Countries on a.DestinationCountryId equals country.Id
                                              where a.Tenant == tenant && a.ItemId == productItem.Id
                                                select new HTSCodePM()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    ItemId = a.ItemId,
                                                    DestinationCountryId = a.DestinationCountryId,
                                                    Code = a.Code,
                                                    ApprovedByCustomer = a.ApprovedByCustomer,
                                                    CountryEnglishName = country!=null ? country.EnglishName :"",
                                                    InActive = a.InActive,
                                                    LineNumber = a.LineNumber,
                                                }).ToList();

            
                    if (productItem!= null)
                    {
                        productItem.HTSCodes = hTSCodes;
                    }
                }            
            }

            return productItems;
        }
    }
}
