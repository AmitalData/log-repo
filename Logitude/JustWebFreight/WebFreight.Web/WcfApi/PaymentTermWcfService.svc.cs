using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PaymentTermWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PaymentTermWcfService.svc or PaymentTermWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class PaymentTermWcfService : IPaymentTermWcfService
    { 
        public List<PaymentTermList> GetPaymentTerms(ref Response response, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                //SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);
                PaymentTermRepository paymentTermRepository = new PaymentTermRepository(tenant);
                PaymentTermQuery paymentTermQuery = new PaymentTermQuery(tenant);

                var paymentTerms = paymentTermRepository.GetPaymentTerms(tenant);
                List<PaymentTermList> result = paymentTermQuery.GetIQueryableEntityList(paymentTerms).ToList();

                return result;
            }

            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }
    }
}
