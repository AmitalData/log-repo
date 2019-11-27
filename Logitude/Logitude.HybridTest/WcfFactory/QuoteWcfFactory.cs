using Logitude.BL.QuoteModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfFactory
{
    class QuoteWcfFactory
    {
        readonly private static QuotePM quotePM = new QuotePM()
        {
            QuoteNumber = HybridData.QuoteCode,
            BranchId = HybridData.BranchCode,
            DepartmentId = HybridData.DepartmentCode,
            CustomerId = HybridData.AgentCode,
            FromPortId = HybridData.FromPortCode,
            ToPortId = HybridData.ToPortCode,
            ExchangeRate = 1,
            CreatedByUserId = HybridData.UserCode,
            UpdatedByUserId = HybridData.UserCode,
            OpenDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            SaleCurrencyId = HybridData.CurrencyCode,
            Tenant = TestEnvironmentGlobalParameters.Tenant,
            BusinessUnitId = TestEnvironmentGlobalParameters.Tenant.ToString(),
            TransportModeId = "A", //A:Air, O:Occean, I:Inland
            DirectionId = "I", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
            QuoteTypeCode = "A", //A:Spot Rate, P:Routing Rate
            QuoteCustomerTypeCode = "CON", //CON:Consignee, AGT:Agent, SHI:Shipper, NOT:Notify, OTH:Other
        };
        public static QuotePM GetquotePM() { return quotePM; }
    }
}
