using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebFreight.Web.Helpers
{
    public class QuoteTemplateChargeGroupService
    {
        private readonly int tenant;
        private readonly ChargesGroupQuery chargesGroupQuery;
        private readonly QuoteChargesGroupQuery quoteChargesGroupQuery;
        private readonly string splitBy;
        private readonly string quotePricingTableType;
        private readonly QuoteTemplateSettingPM quoteTemplateSetting;

        public QuoteTemplateChargeGroupService(string quotePricingTableType, QuoteTemplateSettingPM quoteTemplateSetting)
        {
            this.tenant = quoteTemplateSetting.Tenant;
            this.quotePricingTableType = quotePricingTableType;
            this.quoteTemplateSetting = quoteTemplateSetting;
            this.splitBy = GetSplitBy();

            chargesGroupQuery = new ChargesGroupQuery(tenant);
            quoteChargesGroupQuery = new QuoteChargesGroupQuery(tenant);

        }

        private string GetSplitBy()
        {
            if (quotePricingTableType == "PP") return string.IsNullOrEmpty(quoteTemplateSetting.QuoteTemplateSettingData.PackagesSplitBy) ? "Charge Group" : quoteTemplateSetting.QuoteTemplateSettingData.PackagesSplitBy;
            return string.IsNullOrEmpty(quoteTemplateSetting.QuoteTemplateSettingData.ContainersSplitBy) ? "Charge Group" : quoteTemplateSetting.QuoteTemplateSettingData.ContainersSplitBy;
        }

        public List<ChargesGroupList> GetChargesGroup()
        {
            if (splitBy == "Charge Group") return chargesGroupQuery.GetChargesGroupListsByTenant(tenant).ToList();
            return quoteChargesGroupQuery.GetQuoteChargesGroupListsByTenant(tenant).Select(MapChargeGroupToQoute()).ToList();
        }

        private static Expression<Func<QuoteChargesGroupList, ChargesGroupList>> MapChargeGroupToQoute()
        {
            return item => new ChargesGroupList
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                SearchFields = item.SearchFields,
                Tenant = item.Tenant,
                LocalName = item.LocalName,
                ViewOrder = item.ViewOrder
            };
        }

        public List<QuoteSaleChargePM> GetQuoteSaleCharges(List<QuoteSaleChargePM> quoteSaleCharges)
        {
            if (splitBy == "Charge Group") return quoteSaleCharges;
            if (quoteSaleCharges.Any(a => a.QuoteChargesGroupCode == null)) throw new ApplicationException("Please fill the Quote Charge Groups for all Charge types");

            return quoteSaleCharges.Select(quoteSaleCharge =>
                {
                    quoteSaleCharge.ChargesGroupCode = quoteSaleCharge.QuoteChargesGroupCode;
                    return quoteSaleCharge;
                }).ToList();
        }

    }
}