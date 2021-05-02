using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ProductItemQuery
    {
        ProductItemRepsitory repository;

        public ProductItemQuery()
        {
            this.repository = new ProductItemRepsitory();
        }

        public ProductItemQuery(int tenant)
        {
            this.repository = new ProductItemRepsitory(tenant);
        }

        public ProductItemQuery(ProductItemRepsitory repository)
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
                    Remarks = entityPoco.Remarks,
                    InActive = entityPoco.InActive,
                    Description = entityPoco.Description,
                };
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
                                                   Remarks = entity.Remarks,
                                                   InActive = entity.InActive,
                                                   Description = entity.Description,
                                               };
            return result;
        }

        public List<ProductItemPM> GetProductItemPMsByCustomerAndPortIds(string customerId, string dischargePortCountryId, int tenant)
        {
            List <ProductItemPM> productItems = (from a in repository.context.ProductItems
                                                where a.Tenant == tenant && a.CustomerId == customerId
                                                select new ProductItemPM()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    CustomerId = a.CustomerId,
                                                    SKU = a.SKU,
                                                    Remarks = a.Remarks,
                                                    InActive = a.InActive,
                                                    Description = a.Description,
                                                }).ToList();


            foreach (ProductItemPM productItem in productItems)
            {
                HTSCode hTSCode = (from a in repository.context.HTSCodes
                                          where a.Tenant == tenant && a.ItemId == productItem.Id && a.DestinationCountryId == dischargePortCountryId
                                          select a).FirstOrDefault();

                if(hTSCode != null)
                {
                    productItem.HTSCodeByCountry = hTSCode.Code;
                }
            }

            return productItems;
        }
    }
}
