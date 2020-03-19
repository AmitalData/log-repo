using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
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
            BranchId = HybridData.BranchCodeHBRA,
            DepartmentId = HybridData.DepartmentCodeHDEP,
            CustomerId = HybridData.AgentCodeHAgent,
            FromPortId = HybridData.PortCodeMAN,
            ToPortId = HybridData.PortCodeLON,
            ExchangeRate = 1,
            CreatedByUserId = HybridData.UserCodeHU,
            UpdatedByUserId = HybridData.UserCodeHU,
            OpenDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            SaleCurrencyId = HybridData.CurrencyCodeHCR,
            Tenant = EnvironmentGlobalParams.MainTenant,
            BusinessUnitId = EnvironmentGlobalParams.MainTenant.ToString(),
            TransportModeId = "A", //A:Air, O:Occean, I:Inland
            DirectionId = "E", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
            QuoteTypeCode = "A", //A:Spot Rate, P:Routing Rate
            QuoteCustomerTypeCode = "CON", //CON:Consignee, AGT:Agent, SHI:Shipper, NOT:Notify, OTH:Other
        };
        public static QuotePM GetQuotePM()
        {
            return quotePM;
        }
        public static QuotePM GetQuotePMWithNewNumber()
        {
            quotePM.QuoteNumber = TableCounter.GetNumber(EnvironmentGlobalParams.MainTenant, "QUOT", quotePM.DirectionId, quotePM.TransportModeId);
            return quotePM;
        }
    }
}
