using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Web;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ExternalLinkQuery
    {
        ExternalLinkRepository repository;

        public ExternalLinkQuery(int tenant)
        {
            repository = new ExternalLinkRepository(tenant);
        }

        public ExternalLinkQuery(ExternalLinkRepository repository)
        {
            this.repository = repository;
        }

        public ExternalLinkPM GetSinglePM(string Ref, bool fromCache = true)
        {
            if(!fromCache)
            {
                ExternalLink externalLink = repository.GetSingleExternalLink(Ref);
                return new ExternalLinkPM()
                {
                    Id = externalLink.Id,
                    Ref = externalLink.Ref,
                    Link = externalLink.Link,
                    ExpirationDate = externalLink.ExpirationDate,
                    ActivityLog = externalLink.ActivityLog,
                    Params = externalLink.Params
                };                
            }

            string cacheKey = "ExternalLink_" + Ref;

            if (HttpContext.Current != null && CacheManager.CacheWrapper.Get(cacheKey) != null)
                return (ExternalLinkPM)CacheManager.CacheWrapper.Get(cacheKey);

            ExternalLinkPM externalLinkPM = GetSinglePM(Ref, false);
            CacheManager.CacheWrapper.Insert(cacheKey, externalLinkPM, null, System.DateTime.UtcNow.AddHours(8), TimeSpan.Zero);

            return externalLinkPM;
        }

        public string GetExternalLink(string Ref, string param, int tenant)
        {
            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(tenant);
            ExternalLinkPM externalLinkPM = GetSinglePM(Ref);
            AuthenticationToken token = new AuthenticationToken()
            {
                Token = Guid.NewGuid().ToString(),
                Tenant = tenant,
                Email = "",
                Password = "",
                CreateDate = DateTime.UtcNow,
                APIToken = true,
                InActive = false,
                LinkId = externalLinkPM.Id,
                Params = param,
                ExpirationDate = DateTime.UtcNow.AddDays(externalLinkPM.ExpirationDate),
                ClientType = AuthenticationTokenRepository.ExternalLink
            };

            authenticationTokenRepository.Add(token);
            authenticationTokenRepository.SubmitChanges();

            return externalLinkPM.Link.Replace("{token}", token.Token);
        }
    }
}