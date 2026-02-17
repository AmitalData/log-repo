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
    public class CardExternalAccountsByProductQuery
    {
        CardExternalAccountsByProductRepository repository;
        public CardExternalAccountsByProductQuery(int tenant)
        {
            repository = new CardExternalAccountsByProductRepository(tenant);
        }
        public CardExternalAccountsByProductQuery(CardExternalAccountsByProductRepository myRepository)
        {
            repository = myRepository;
        }

        public CardExternalAccountsByProductPM GetSinglePM(string id, int tenant)
        {
            CardExternalAccountsByProductPM entityPM = (from a in repository.context.CardExternalAccountsByProducts.Include("ProductType").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                                                        where a.Tenant == tenant && a.Id == id
                                                        select new CardExternalAccountsByProductPM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            GLAccount = a.GLAccount,
                                                            CostCenter = a.CostCenter,
                                                            UpdateDate = a.UpdateDate,
                                                            CardId = a.CardId,
                                                            ProductTypeCode = a.ProductTypeCode,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            ProductTypeName = a.ProductType == null ? null : a.ProductType.Name,
                                                            UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                                                        }).FirstOrDefault();

            return entityPM;
        }
        public IQueryable<CardExternalAccountsByProductPM> GetCardExternalAccountsByProductsByCardId(string cardId, int tenant)
        {
            IQueryable<CardExternalAccountsByProductPM> myResult = from a in repository.context.CardExternalAccountsByProducts.Include("ProductType").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                                                                   where a.Tenant == tenant && a.CardId == cardId
                                                                   select new CardExternalAccountsByProductPM()
                                                                   {
                                                                       Id = a.Id,
                                                                       Tenant = a.Tenant,
                                                                       GLAccount = a.GLAccount,
                                                                       CostCenter = a.CostCenter,
                                                                       UpdateDate = a.UpdateDate,
                                                                       CardId = a.CardId,
                                                                       ProductTypeCode = a.ProductTypeCode,
                                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                                       ProductTypeName = a.ProductType == null ? null : a.ProductType.Name,
                                                                       UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                                                                   };
            return myResult;
        }
        public IQueryable<CardExternalAccountsByProductList> GetIQueryableEntityList(IQueryable<CardExternalAccountsByProduct> iQueryable)
        {
            IQueryable<CardExternalAccountsByProductList> myResult = from a in iQueryable
                                                                     select new CardExternalAccountsByProductList()
                                                                     {
                                                                         Id = a.Id,
                                                                         Tenant = a.Tenant,
                                                                         GLAccount = a.GLAccount,
                                                                         CostCenter = a.CostCenter,
                                                                         UpdateDate = a.UpdateDate,
                                                                         CardId = a.CardId,
                                                                         ProductTypeCode = a.ProductTypeCode,
                                                                         UpdatedByUserId = a.UpdatedByUserId,
                                                                     };
            return myResult;
        }
    }
}
