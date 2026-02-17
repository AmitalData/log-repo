using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.QuoteModel.Repositories;

using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuotePriceStepsQuery
    {
        QuotePriceStepsRepository repository;
        public QuotePriceStepsQuery()
        {
            repository = new QuotePriceStepsRepository(); 
        }

        public QuotePriceStepsQuery(int tenant)
        {
            repository = new QuotePriceStepsRepository(tenant);
        }

        public QuotePriceStepsQuery(QuotePriceStepsRepository quotePriceStepsRepository)
        {
            repository = quotePriceStepsRepository;
        }

        public IQueryable<QuotePriceStepsPM> GetQuotePriceStepPMsByTenant(int tenant)
        {
            return from a in repository.context.QuotePriceSteps
                   where a.Tenant == tenant
                   select new QuotePriceStepsPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       CostUnitPrice = a.CostUnitPrice,
                       SaleUnitPrice = a.SaleUnitPrice,
                       QuoteId = a.QuoteId,
                       QuoteChargeId = a.QuoteChargeId,
                       Step = a.Step,
                       MarkupValue = a.MarkupValue,
                   };
        }

        public IQueryable<QuotePriceStepsList> GetQuotePriceStepListsByTenant(int tenant)
        {
            return from a in repository.context.QuotePriceSteps
                   where a.Tenant == tenant
                   select new QuotePriceStepsList()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       CostUnitPrice = a.CostUnitPrice,
                       SaleUnitPrice = a.SaleUnitPrice,
                       QuoteId = a.QuoteId,
                       QuoteChargeId = a.QuoteChargeId,
                       Step = a.Step,
                       MarkupValue = a.MarkupValue,
                   };
        }

        public List<QuotePriceStepsPM> GetQuotePriceStepPMsByQuoteChargeId(string quoteId, string quoteChargeId, int tenant)
        {
            return (from a in repository.context.QuotePriceSteps
                    where a.QuoteId == quoteId && a.QuoteChargeId == quoteChargeId && a.Tenant == tenant
                    select new QuotePriceStepsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CostUnitPrice = a.CostUnitPrice,
                        SaleUnitPrice = a.SaleUnitPrice,
                        QuoteId = a.QuoteId,
                        Step = a.Step,
                        MarkupValue = a.MarkupValue,
                        QuoteChargeId = a.QuoteChargeId,
                    }).ToList();
        }

        public QuotePriceStepsPM GetSingleQuotePriceStepPM(string id)
        {
            return (from a in repository.context.QuotePriceSteps
                    where a.Id == id
                    select new QuotePriceStepsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CostUnitPrice = a.CostUnitPrice,
                        SaleUnitPrice = a.SaleUnitPrice,
                        QuoteId = a.QuoteId,
                        Step = a.Step,
                        MarkupValue = a.MarkupValue,
                    }).FirstOrDefault();
        }
    }
}
