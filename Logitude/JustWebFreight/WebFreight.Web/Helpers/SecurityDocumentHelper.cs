using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel;

namespace WebFreight.Web.Helpers
{
    public static class SecurityDocumentHelper
    {
        public static SecurityDocumentResult ValidationDocumentToken(string token)
        {
            SecurityDocumentResult result = new SecurityDocumentResult();
   
            if (!string.IsNullOrEmpty(token))
            {
                AuthenticationTokenRepository tokenRep = new AuthenticationTokenRepository(GlobalContext.GetContext(0));
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken != null && authToken.ClientType == "DocumentDownload" && authToken.ExpirationDate != null)
                {
                    DateTime nowDate = DateTime.Now;
                    DateTime expirationDate = (DateTime)authToken.ExpirationDate;
                    if (expirationDate > nowDate)
                    {
                        result.Email = authToken.Email;
                        result.Tenant = authToken.Tenant;
                        result.IsValid = true;
                    }
                    else result.ExceptionResult = "Sorry, your download link has expired.";
                }
            }
          
            return result;
        }

    }

    public class SecurityDocumentResult
    {
        public bool IsValid { get; set; }
        public string ExceptionResult { get; set; }
        public string Email { get; set; }
        public int? Tenant { get; set; }

    }
}