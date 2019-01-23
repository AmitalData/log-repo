
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{


    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DepositionRequestWcfService : IDepositionRequestWcfService

    {

        public async Task<Response> SendDepositionRequestTaskToLogBox(DepositionRequestPM depositionRequestPM)
        {
            var response = new Response();
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if (depositionRequestPM != null)
                {
                    DepositionRequestHelper depositionRequestHelper = new DepositionRequestHelper();
                    response = await depositionRequestHelper.SendDepositionRequestTaskToLogBox(depositionRequestPM);
                }

                return response;
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

                return response;

            }

        }

       
    }

}
