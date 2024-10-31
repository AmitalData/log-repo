using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace WebFreight.Web.Helpers
{
    public class HeaderHelper
    {
        public static bool IsGlobaUser()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                bool isGlobalUser = GlobalContext.GetContext().GlobalContacts.Any(d => d.GlobalTenantId == 0 && d.InActive == false && d.Email == authToken.Email);
                return isGlobalUser;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int? GetTenantFromToken()
        {
            try
            {
                AuthenticationToken authToken = GetTokenData();
                return authToken.Tenant;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static AuthenticationToken GetTokenData()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken;
        }

        public static AuthenticationToken Authenticate()
        {
            string token = HttpContext.Current.Request.Headers["Token"];

            if (string.IsNullOrEmpty(token))
                ThrowAutherizeException();

            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

            if (authToken == null)
                ThrowAutherizeException();

            return authToken;
        }

        private static void ThrowAutherizeException()
        {
            throw new HttpResponseException(HttpStatusCode.Forbidden);
        }
    }

    public class TokenAutherizeAttribute : Attribute
    {
        public TokenAutherizeAttribute()
        {
            HeaderHelper.Authenticate();
        }
    }
}