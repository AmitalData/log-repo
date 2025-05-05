using System.Security.Claims;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _contextAccessor;
        private static ClaimsPrincipal _overriddenUser;

        public static void Initialize(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public static HttpContext HttpContext => _contextAccessor.HttpContext;
        public static ClaimsPrincipal User
        {
            get
            {
                return _overriddenUser ?? _contextAccessor?.HttpContext?.User;
            }
            set
            {
                _overriddenUser = value;
            }
        }
        public static HttpRequest Request => _contextAccessor?.HttpContext?.Request;
        public static HttpResponse Response => _contextAccessor?.HttpContext?.Response;
        public static ISession Session => _contextAccessor?.HttpContext?.Session;
    }
}
