using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class QuoteWcfCaller
    {
        public static Response CallQuoteUpsert()
        {
            Response prepareResponse = InitialQuote();
            if (!prepareResponse.HasError)
            {
                QuotePM entityPM = new QuotePM()
                {
                    QuoteNumber = HybridData.QuoteCode,
                    TransportModeId = "A", //A:Air, O:Occean, I:Inland
                    DirectionId = "I", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
                    BranchId = HybridData.BranchCode,
                    DepartmentId = HybridData.DepartmentCode,
                    CustomerId = HybridData.AgentCode,
                    FromPortId = HybridData.FromPortCode,
                    ToPortId = HybridData.ToPortCode,
                    QuoteTypeCode = "A", //A:Spot Rate, P:Routing Rate
                    ExchangeRate = 1,
                    CreatedByUserId = HybridData.UserCode,
                    UpdatedByUserId = HybridData.UserCode,
                    OpenDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    QuoteCustomerTypeCode = "CON", //CON:Consignee, AGT:Agent, SHI:Shipper, NOT:Notify, OTH:Other
                    SaleCurrencyId = HybridData.CurrencyCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                    BusinessUnitId = TestEnvironmentGlobalParameters.Tenant.ToString(),
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Quote",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(QuotePM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.QuoteId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareQuote()
        {
            if (HybridData.QuoteId == null)
            {
                return CallQuoteUpsert();
            }
            return new Response();
        }

        private static Response InitialQuote()
        {
            Response prepareResponse =  UserWcfCaller.PrepareUser();
            if (!prepareResponse.HasError)
            {
                prepareResponse = PortWcfCaller.PrepareFromPort();
                if (!prepareResponse.HasError)
                {
                    prepareResponse = PortWcfCaller.PrepareToPort();
                    if (!prepareResponse.HasError)
                    {
                        prepareResponse = AgentWcfCaller.PrepareAgent();
                        if (!prepareResponse.HasError)
                        {
                            prepareResponse = CurrencyWcfCaller.PrepareCurrency();
                            if (!prepareResponse.HasError)
                            {
                                return prepareResponse;
                            }
                        }
                    }
                }
            }
            return prepareResponse;
        }
    }
}
