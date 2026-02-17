using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers
{
    public class APICredentialsHelper
    {
        public ApiCredential CheckUserState(APICredentialsParameters Key)
        {
            string token = AuthenticationUtil.GenerateToken();//Guid.NewGuid().ToString();
            ApiCredential userData = new ApiCredential();
            IGlobalContext globalContext = GlobalContext.GetContext();
            string PrimaryhashedKey = PasswordGenerator.GetOldHashedPassword(Key.PrimaryKey);
            bool customerCare = false;
            var Partner = globalContext.ApiCredintials.Where(c => c.HashedPrimaryAccessKey == PrimaryhashedKey).FirstOrDefault();
            if (!string.IsNullOrEmpty(Key.SecondaryKey) && Partner == null)
            {
                string SecondaryhashedKey = PasswordGenerator.GetOldHashedPassword(Key.SecondaryKey);
                Partner = globalContext.ApiCredintials.Where(c => c.HashedPrimaryAccessKey == SecondaryhashedKey).FirstOrDefault();

            }

            if (Partner != null)
            {
                userData.Tenant = Partner.Tenant;
                if (Partner.Tenant == 0)
                {
                    customerCare = true;
                }

                if (customerCare)
                {

                    string ipstring = Partner.AllowedIPs;
                    string[] authenticatedIPs = ipstring.Split(',');
                    if (!authenticatedIPs.Contains("*"))
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        
                        if (!authenticatedIPs.Contains(currentIP))
                        {
                            userData.IpRestricted = true;
                        }
                    }
                }
            }
            else
            {
                userData.InValidKey = true;
            }

            userData.HasError = (userData.InValidKey || userData.IpRestricted);
            userData.CustomerCare = customerCare;
            if (!userData.HasError)
            {
                userData.Token = token;
            }
            return userData;
        }
    }
}