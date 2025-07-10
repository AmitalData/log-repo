using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("{myDummyInteger?}/{myDummyString?}")]
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