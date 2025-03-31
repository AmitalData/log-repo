using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using System.Linq;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class AuthenticationController : ApiController
    {

        public string GetSettingsLoginCode(int myDummyInteger, string myDummyString)
        => new SettingQueryService(0).GetMulti(a => true, a => a.LogoCode).FirstOrDefault();
    }
}