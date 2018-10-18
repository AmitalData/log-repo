using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteTotalVATQuery
    {
        QuoteTotalVATRepository repository;

        public QuoteTotalVATQuery(int tenant)
        {
            repository = new QuoteTotalVATRepository(tenant);
        }

        public QuoteTotalVATQuery(QuoteTotalVATRepository repository)
        {
            this.repository = repository;
        }

        public List<QuoteTotalVATPM> GetTotalVATs(string quoteId, int tenant)
        {
            List<QuoteTotalVATPM> myResult
                = (from a in repository.context.QuoteTotalVATs.Include("VatType")
                   where a.QuoteId == quoteId && a.Tenant == tenant
                   select new QuoteTotalVATPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       QuoteId = a.QuoteId,
                       ExternalVATCard = a.ExternalVATCard,
                       ExternalTAXItemId = a.ExternalTAXItemId,
                       QuoteCurrencyVatableAmount = a.QuoteCurrencyVatableAmount,
                       QuoteCurrencyVATAmount = a.QuoteCurrencyVATAmount,
                       LocalCurrencyVatableAmount = a.LocalCurrencyVatableAmount,
                       LocalCurrencyVATAmount = a.LocalCurrencyVATAmount,
                       ProfitCurrencyVatableAmount = a.ProfitCurrencyVatableAmount,
                       ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                       VatTypeId = a.VatTypeId,
                       VatPercent = a.VatPercent,
                       VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                       VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),
                   }).ToList();

            return myResult;
        }
    }
}
