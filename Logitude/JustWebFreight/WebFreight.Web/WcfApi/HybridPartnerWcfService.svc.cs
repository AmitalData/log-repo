using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HybridPartnerWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select HybridPartnerWcfService.svc or TestService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HybridPartnerWcfService : IHybridPartnerWcfService
    {


        public List<HybridPartnerList> GetMislakaPartners(int tenant, ref Logitude.Server.Tools.Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                List<HybridPartnerList> result = new List<HybridPartnerList>();
                ICommonDataContext commoncontext = CommonDataContext.GetContext(0);
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);

                result = (from b in hybridPartnerRepository.context.HybridPartners
                          where b.IsMislakaActivated == true
                          select new HybridPartnerList
                          {
                              Id = b.Id,
                              LocalName = b.LocalName,
                              LogoId = b.LogoId,
                              Name = b.Name,
                              PartnerTenant = b.PartnerTenant,
                              SearchFields = b.SearchFields,
                              SmallLogoId = b.SmallLogoId,
                          }).ToList();


                return result;

            }
            catch (Exception ex)
            {
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
