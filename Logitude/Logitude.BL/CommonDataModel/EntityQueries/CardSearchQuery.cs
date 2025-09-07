
using System;
using System.Linq;

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;


namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CardSearchQuery
    {
        CardSearchRepository repository;



        public CardSearchQuery(int tenant)
        {
            repository = new CardSearchRepository(tenant);
        }

        public CardSearchQuery(CardSearchRepository CardSearchRepositoryRepository)
        {
            repository = CardSearchRepositoryRepository;
        }

        public CardSearchPM GetSinglePM(string id, int tenant)
        {
            CardSearchPM myResult
                = (from a in repository.context.CardSearches
                   where a.Id == id && a.Tenant == tenant
                   select new CardSearchPM()
                   {
                      Id = a.Id,
                      Tenant = a.Tenant,
                      CardId = a.CardId,
                      Keyword = a.Keyword,
                      RecordDate = a.RecordDate,
                      Weight = a.Weight,
                      PartnerTypeId = a.PartnerTypeId,
                      InActive = a.InActive,
                      IsCustomer = a.IsCustomer,
                   }).FirstOrDefault();

            return myResult;
        }

     


        public IQueryable<CardSearchList> GetIQueryableEntityList(IQueryable<CardSearch> iQueryable)
        {
            IQueryable<CardSearchList> result = from a in iQueryable
                                                 select new CardSearchList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     CardId = a.CardId,
                                                     Keyword = a.Keyword,
                                                     RecordDate = a.RecordDate,
                                                     Weight = a.Weight,
                                                     PartnerTypeId = a.PartnerTypeId,
                                                     InActive = a.InActive,
                                                     IsCustomer = a.IsCustomer,

                                                 };
            return result;
        }
    }
}
