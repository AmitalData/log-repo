using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CardContactProductQuery
    {
        CardContactProductRepository repository;

        public CardContactProductQuery()
        {
            repository = new CardContactProductRepository();
        }

        public CardContactProductQuery(int tenant)
        {
            repository = new CardContactProductRepository(tenant);
        }

        public CardContactProductQuery(CardContactProductRepository CardContactProductRepository)
        {
            repository = CardContactProductRepository;
        }

        public CardContactProductPM GetSinglePM(string id, int tenant)
        {
            CardContactProductPM entity = (from a in repository.context.CardContactProducts.Include("ProductType")
                 where a.Tenant == tenant && a.Id == id
                 select new CardContactProductPM()
                 {
                     Id = a.Id,
                     CardContactId = a.CardContactId,
                     ProductTypeCode = a.ProductTypeCode,
                     Tenant = a.Tenant,                     
                     ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                 }).FirstOrDefault();

            return entity;
        }

        public List<CardContactProductPM> GetCardContactProductPMsByCardContactId(string CardContactId, int tenant)
        {
            List<CardContactProductPM> result =

                (from a in repository.context.CardContactProducts.Include("ProductType")
                 where a.Tenant == tenant && a.CardContactId == CardContactId
                 select new CardContactProductPM()
                 {
                     Id = a.Id,
                     CardContactId = a.CardContactId,
                     ProductTypeCode = a.ProductTypeCode,
                     Tenant = a.Tenant,                     
                     ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,                     
                 }).ToList();            

            return result;
        }        
    }
}
