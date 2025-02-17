using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class AuthenticationController : ApiController
    {

        public string GetSettingsLoginCode(int myDummyInteger, string myDummyString)
        {
            string myResult = "";
            IGlobalContext globalContext = GlobalContext.GetContext();
            Setting mySettings = globalContext.Settings.FirstOrDefault();
            if (mySettings == null)
            {
                return myResult;
            }
            myResult = mySettings.LogoCode;
            return myResult;
        }


    }
}