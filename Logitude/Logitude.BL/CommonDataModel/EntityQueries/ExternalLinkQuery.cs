using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;
using System;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ExternalLinkQuery
    {
        readonly ExternalLinkRepository repository;
        private static readonly string CACHE_KEY_FORMAT = "ExternalLink_{0}";

        public ExternalLinkQuery(int tenant)
        {
            repository = new ExternalLinkRepository(tenant);
        }

        public ExternalLinkQuery(ExternalLinkRepository repository)
        {
            this.repository = repository;
        }

        public ExternalLinkPM GetSinglePM(string id, int tenant) => GetSinglePM(id, true);
        public ExternalLinkPM GetSinglePM(string id, bool fromCache = true)
        {
            if (!fromCache)
            {
                ExternalLink externalLink = repository.GetSingle(id);
                if (externalLink == null)
                    return null;
                return ExternalLinkMapping.MapPM(externalLink);
            }

            string cacheKey = string.Format(CACHE_KEY_FORMAT, id);

            if (HttpContext.Current != null && CacheManager.CacheWrapper.Get(cacheKey) != null)
                return (ExternalLinkPM)CacheManager.CacheWrapper.Get(cacheKey);

            ExternalLinkPM externalLinkPM = GetSinglePM(id, fromCache: false);
            if (externalLinkPM == null)
                return null;
            CacheManager.CacheWrapper.Insert(cacheKey, externalLinkPM, null, System.DateTime.UtcNow.AddHours(8), TimeSpan.Zero);

            return externalLinkPM;
        }

        public ExternalLinkPM GetSinglePMByRef(string Ref, bool fromCache = true)
        {
            if (!fromCache)
            {
                ExternalLink externalLink = repository.GetSingleExternalLinkByRef(Ref);
                if (externalLink == null)
                    return null;

                return ExternalLinkMapping.MapPM(externalLink);
            }

            string cacheKey = string.Format(CACHE_KEY_FORMAT, Ref);

            if (HttpContext.Current != null && CacheManager.CacheWrapper.Get(cacheKey) != null)
                return (ExternalLinkPM)CacheManager.CacheWrapper.Get(cacheKey);

            ExternalLinkPM externalLinkPM = GetSinglePMByRef(Ref, false);
            if (externalLinkPM == null)
                return null;
            CacheManager.CacheWrapper.Insert(cacheKey, externalLinkPM, null, System.DateTime.UtcNow.AddHours(8), TimeSpan.Zero);

            return externalLinkPM;
        }

        public string GetExternalLink(string Ref, string param, int tenant)
        {
            if (string.IsNullOrEmpty(Ref) || string.IsNullOrEmpty(param) || tenant == null)
                throw new ArgumentException($"one of the next parameter is null, Ref: {Ref}, param: {param}, tenant: {tenant}");

            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(tenant);
            ExternalLinkPM externalLinkPM = GetSinglePMByRef(Ref);

            if (externalLinkPM is null)
                throw new InvalidOperationException($"PMByRef with Ref {Ref} not found.");

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

        public IQueryable<ExternalLinkList> GetIQueryableEntityList(IQueryable<ExternalLink> iQueryable) =>
            iQueryable.Select(a => new ExternalLinkList
            {
                Id = a.Id,
                Ref = a.Ref,
                Link = a.Link,
                ExpirationDate = a.ExpirationDate,
                ActivityLog = a.ActivityLog,
                Params = a.Params
            });       
    }
}