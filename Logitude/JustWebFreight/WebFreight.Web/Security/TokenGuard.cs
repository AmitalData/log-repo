using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Threading;
using Simplog.Server.Infrastructure;
using System.Web.Http;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Net;

namespace WebFreight.Web.Security
{
    public static class TokenGuard
    {
        public static AuthenticationToken Validate(int tenant)
        {
            var tokenValue = HttpContext.Current?.Request?.Headers["Token"];
            if (string.IsNullOrWhiteSpace(tokenValue))
                ThrowHttp(HttpStatusCode.Unauthorized, "Missing Token header.");

            var token = AuthenticationTokenRepository.GetSingleTokenFromCache(tokenValue);
            if (token == null || token.InActive ||
                (token.ExpirationDate.HasValue && token.ExpirationDate <= DateTime.UtcNow))
                ThrowHttp(HttpStatusCode.Unauthorized, "Invalid or expired token.");

            if (token.Tenant != tenant)
                ThrowHttp(HttpStatusCode.Forbidden, $"Token does not belong to tenant {tenant}.");

            return token;
        }
        private static void ThrowHttp(HttpStatusCode code, string message) =>
        throw new HttpException((int)code, message);
    }

}