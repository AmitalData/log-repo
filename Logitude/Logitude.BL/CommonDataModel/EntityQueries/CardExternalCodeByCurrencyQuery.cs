using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CardExternalCodeByCurrencyQuery
    {
        CardExternalCodeByCurrencyRepository repository;

        public CardExternalCodeByCurrencyQuery()
        {
            repository = new CardExternalCodeByCurrencyRepository();
        }

        public CardExternalCodeByCurrencyQuery(int tenant)
        {
            repository = new CardExternalCodeByCurrencyRepository(tenant);
        }

        public CardExternalCodeByCurrencyQuery(CardExternalCodeByCurrencyRepository repository)
        {
            this.repository = repository;
        }

        public CardExternalCodeByCurrencyPM GetSinglePM(string id, int tenant)
        {
            CardExternalCodeByCurrencyPM result = null;

            CardExternalCodeByCurrency entityPoco = repository.GetSingleCardExternalCodeByCurrency(id, tenant);

            if (entityPoco != null)
            {
                result = new CardExternalCodeByCurrencyPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    CurrencyId = entityPoco.CurrencyId,
                    CardId = entityPoco.CardId,
                    ExternalRecievableTableId = entityPoco.ExternalRecievableTableId,
                    ExternalPayableTableId = entityPoco.ExternalPayableTableId,
                    CurrencyCode = entityPoco.Currency != null ? entityPoco.Currency.Code : null,
                    CurrencyName = entityPoco.Currency != null ? entityPoco.Currency.EnglishName : null,
                    CardName = entityPoco.Card != null ? entityPoco.Card.EnglishName : null,

                    //  ExternalTableName = entityPoco.ExternalTable != null ? entityPoco.ExternalTable.Name : null,
                    //  ExternalTableCode = entityPoco.ExternalTable != null ? entityPoco.ExternalTable.Code : null,
                };
            }

            return result;
        }

        public List<CardExternalCodeByCurrencyPM> GetCardExternalCodeByCurrencyPMsForCustomer(string customerId, int tenant)
        {
            List<CardExternalCodeByCurrency> entityPoco = repository.GetCardExternalCodeByCurrencyforCustomer(tenant, customerId).ToList();

            List<CardExternalCodeByCurrencyPM> result = (from a in entityPoco

                                                         select new CardExternalCodeByCurrencyPM()
                                                         {
                                                             Id = a.Id,
                                                             CardId = a.CardId,
                                                             CurrencyId = a.CurrencyId,
                                                             ExternalRecievableTableId = a.ExternalRecievableTableId,
                                                             ExternalPayableTableId = a.ExternalPayableTableId,
                                                             Tenant = a.Tenant,
                                                             CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                             CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                             CardName = a.Card != null ? a.Card.EnglishName : null,

                                                             //  ExternalTableName = a.ExternalTable != null ? a.ExternalTable.Name : null,
                                                             //  ExternalTableCode = a.ExternalTable != null ? a.ExternalTable.Code : null,
                                                         }).ToList();

            return result;
        }

        public IQueryable<CardExternalCodeByCurrencyPM> GetCustomerMediatorByProductPMs(int tenant, string customerId)
        {
            IQueryable<CardExternalCodeByCurrency> entityPoco = repository.GetCardExternalCodeByCurrenciesByTenant(tenant);

            IQueryable<CardExternalCodeByCurrencyPM> result = (from a in entityPoco

                                                               select new CardExternalCodeByCurrencyPM()
                                                               {
                                                                   Id = a.Id,
                                                                   CardId = a.CardId,
                                                                   CurrencyId = a.CurrencyId,
                                                                   ExternalRecievableTableId = a.ExternalRecievableTableId,
                                                                   ExternalPayableTableId = a.ExternalPayableTableId,
                                                                   Tenant = a.Tenant,
                                                                   CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                                   CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                                   CardName = a.Card != null ? a.Card.EnglishName : null,

                                                                   // ExternalTableName = a.ExternalTable != null ? a.ExternalTable.Name : null,
                                                                   // ExternalTableCode = a.ExternalTable != null ? a.ExternalTable.Code : null,
                                                               });

            return result;
        }

        public IQueryable<CardExternalCodeByCurrencyList> GetIQueryableEntityList(IQueryable<CardExternalCodeByCurrency> iQueryable, int tenant)
        {
            IQueryable<CardExternalCodeByCurrencyList> result = from entity in iQueryable.Include("Currency").Include("ExternalTable")
                                                                select new CardExternalCodeByCurrencyList()
                                                                {
                                                                    Id = entity.Id,
                                                                    Tenant = entity.Tenant,
                                                                    CardId = entity.CardId,
                                                                    CurrencyId = entity.CurrencyId,
                                                                    ExternalRecievableTableId = entity.ExternalRecievableTableId,
                                                                    ExternalPayableTableId = entity.ExternalPayableTableId,
                                                                    CurrencyCode = entity.Currency != null ? entity.Currency.Code : null,
                                                                   // ExternalTableName = entity.ExternalTable != null ? entity.ExternalTable.Name : null,
                                                                   // ExternalTableCode = entity.ExternalTable != null ? entity.ExternalTable.Code : null,
                                                                };
            return result;
        }


    }
}
