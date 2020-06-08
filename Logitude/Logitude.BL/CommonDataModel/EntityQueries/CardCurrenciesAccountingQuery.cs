using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CardCurrenciesAccountingQuery
    {
        CardCurrenciesAccountingRepository repository;

        public CardCurrenciesAccountingQuery()
        {
            repository = new CardCurrenciesAccountingRepository();
        }

        public CardCurrenciesAccountingQuery(int tenant)
        {
            repository = new CardCurrenciesAccountingRepository(tenant);
        }

        public CardCurrenciesAccountingQuery(CardCurrenciesAccountingRepository rep)
        {
            repository = rep;
        }

        public CardCurrenciesAccountingPM GetSinglePM(string id, int tenant)
        {
            CardCurrenciesAccountingPM cardCurrenciesAccounting = (from d in repository.context.CardCurrenciesAccountings.Include("Card").Include("Currency")
                                                  where d.Id == id && d.Tenant == tenant
                                                  select new CardCurrenciesAccountingPM()
                                                  {
                                                      Id = d.Id,
                                                      Tenant = d.Tenant,
                                                      CurrencyId = d.CurrencyId,
                                                      CardId = d.CardId,
                                                      ReceivableCreditAccount = d.ReceivableCreditAccount,
                                                      PayableDebitAccount = d.PayableDebitAccount,

                                                  }).FirstOrDefault();

            CardCurrenciesAccountingPM securedPm = new CardCurrenciesAccountingPM();
            SecuredMapping.GetMappedPM(cardCurrenciesAccounting, securedPm, "CardCurrenciesAccounting", tenant);
            return securedPm;
        }

        public IQueryable<CardCurrenciesAccountingPM> GetCardCurrenciesAccountingsForCard(string cardId, int tenant)
        {
            IQueryable<CardCurrenciesAccountingPM> result = (from d in repository.context.CardCurrenciesAccountings.Include("Card").Include("Currency")
                                                             where d.CardId == cardId && d.Tenant == tenant
                                                             select new CardCurrenciesAccountingPM()
                                                             {
                                                                 Id = d.Id,
                                                                 Tenant = d.Tenant,
                                                                 CurrencyId = d.CurrencyId,
                                                                 CardId = d.CardId,
                                                                 ReceivableCreditAccount = d.ReceivableCreditAccount,
                                                                 PayableDebitAccount = d.PayableDebitAccount,
                                                             });

            return result;
        }

        public IQueryable<CardCurrenciesAccountingList> GetIQueryableEntityList(IQueryable<CardCurrenciesAccounting> iQueryable)
        {
            IQueryable<CardCurrenciesAccountingList> result = from d in iQueryable
                                                              select new CardCurrenciesAccountingList()
                                                              {
                                                                  Id = d.Id,
                                                                  Tenant = d.Tenant,
                                                                  CurrencyId = d.CurrencyId,
                                                                  CardId = d.CardId,
                                                                  ReceivableCreditAccount = d.ReceivableCreditAccount,
                                                                  PayableDebitAccount = d.PayableDebitAccount,
                                                              };
            return result;
        }

        public CardCurrenciesAccountingList GetSingleCardCurrenciesAccountingList(string id, int tenant)
        {
            CardCurrenciesAccountingList entityList = (from d in repository.context.CardCurrenciesAccountings
                                                       where d.Tenant == tenant && d.Id == id
                                                       select new CardCurrenciesAccountingList()
                                                       {
                                                           Id = d.Id,
                                                           Tenant = d.Tenant,
                                                           CurrencyId = d.CurrencyId,
                                                           CardId = d.CardId,
                                                           ReceivableCreditAccount = d.ReceivableCreditAccount,
                                                           PayableDebitAccount = d.PayableDebitAccount,
                                                       }).FirstOrDefault();

            return entityList;
        }

        public CardCurrenciesAccountingList GetSingleCardCurrenciesAccountingListByCardAndCurrency(string cardId, string currencyId, int tenant)
        {
            return (from d in repository.context.CardCurrenciesAccountings
                    where d.CardId == cardId
                    && d.CurrencyId == currencyId
                    && d.Tenant == tenant
                    select new CardCurrenciesAccountingList()
                    {
                        Id = d.Id,
                        CurrencyId = d.CurrencyId,
                        CardId = d.CardId,
                        Tenant = d.Tenant,
                        PayableDebitAccount = d.PayableDebitAccount,
                        ReceivableCreditAccount = d.ReceivableCreditAccount,
                    }).FirstOrDefault();
        }

    }
}
