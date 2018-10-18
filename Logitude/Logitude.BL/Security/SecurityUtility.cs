using System.Web;

namespace Logitude.BL.Security
{
    public class SecurityUtility
    {
        public static string GetAuthenticatedUser()
        {
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    throw new AutenticationException("Sorry! this user is not authorized!");
                }
            }
            throw new AutenticationException("Sorry! this user is not authorized!");
        }
    }
}
