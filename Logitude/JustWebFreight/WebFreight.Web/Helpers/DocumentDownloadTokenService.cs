using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DocumentDownloadTokenService
    {
        private AuthenticationToken authenticationToken = null;

        private AuthenticationTokenRepository authenticationTokenRepository = null;
        public DocumentDownloadTokenService(AuthenticationToken authenticationToken)
        {
            this.authenticationToken = authenticationToken;
            authenticationTokenRepository = new AuthenticationTokenRepository(authenticationToken.Tenant);
        }

        public string Get()
        {
            AuthenticationToken documentDownloadAuthenticationToken  = GetNewInStanceFromAuthenticationToken(authenticationToken);
            authenticationTokenRepository.Add(documentDownloadAuthenticationToken);
            authenticationTokenRepository.SubmitChanges();
            return documentDownloadAuthenticationToken.Token;
        }

        private AuthenticationToken GetNewInStanceFromAuthenticationToken(AuthenticationToken authenticationToken)
        {
            return new AuthenticationToken()
            {
                CreateDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(15),
                Email = authenticationToken.Email,
                Password = authenticationToken.Password,
                Token = AuthenticationUtil.GenerateToken(),
                Tenant = authenticationToken.Tenant,
                ClientType = "DocumentDownload"
            };
        }
    }
}