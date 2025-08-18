using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
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

        public ExternalLinkPM GetSinglePM(string id, int tenant, bool fromCache = false)
        {
            if (!fromCache)
            {
                ExternalLink externalLink = repository.GetSingleExternalLink(id, tenant);
                if (externalLink == null)
                    return tenant != 0 ? GetSinglePM(id, 0, false) : null;
                return ExternalLinkMapping.MapPM(externalLink);
            }

            string cacheKey = string.Format(CACHE_KEY_FORMAT, id);
            return GetOrSetCache(cacheKey, () => GetSinglePM(id, tenant, false));
        }

        public ExternalLinkPM GetSinglePMByRef(string Ref, int tenant, bool fromCache = true)
        {
            if (!fromCache)
            {
                ExternalLink externalLink = repository.GetSingleExternalLinkByRef(Ref, tenant);
                if (externalLink == null)
                    return null;

                return ExternalLinkMapping.MapPM(externalLink);
            }

            string cacheKey = string.Format(CACHE_KEY_FORMAT, Ref);
            return GetOrSetCache(cacheKey, () => GetSinglePMByRef(Ref, tenant, false));
        }

        public string AddExternalLink(string Ref, string param, int tenant)
        {
            if (string.IsNullOrEmpty(Ref))
                throw new ArgumentNullException(nameof(Ref), "Ref can not be null or empty");
            if (string.IsNullOrEmpty(param))
                throw new ArgumentNullException(nameof(param), "param can not be null or empty");

            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(tenant);

            ExternalLinkPM externalLinkPM = GetSinglePMByRef(Ref, tenant);
            if (externalLinkPM == null)
                throw new Exception($"not found settings of external link, ref: {Ref}, tenant: {tenant}");

            string _params = JsonConvert.SerializeObject(new AuthenticationTokenParams() { Link = param, Ref = Ref });
            AuthenticationToken authenticationToken = authenticationTokenRepository.GetSingleToken(tenant, externalLinkPM.Id, AuthenticationTokenRepository.ExternalLink, _params);

            if (authenticationToken == null)
            {
                authenticationToken = new AuthenticationToken()
                {
                    Token = Guid.NewGuid().ToString(),
                    Tenant = tenant,
                    Email = "",
                    Password = "",
                    CreateDate = DateTime.UtcNow,
                    APIToken = true,
                    InActive = false,
                    LinkId = externalLinkPM.Id,
                    Params = _params,
                    ExpirationDate = DateTime.UtcNow.AddDays(externalLinkPM.ExpirationDate),
                    ClientType = AuthenticationTokenRepository.ExternalLink
                };

                authenticationTokenRepository.Add(authenticationToken);
            }
            else
            {
                authenticationToken.ExpirationDate = DateTime.UtcNow.AddDays(externalLinkPM.ExpirationDate);
                authenticationTokenRepository.Update(authenticationToken);
            }

            authenticationTokenRepository.SubmitChanges();

            string link = CreateTokenLink(param, authenticationToken.Token);

            return link;
        }

        private static string CreateTokenLink(string param, string token)
        {
            string link = $"Angular/index.html?Menu=REDI&Token={token}";
            Uri uri = new Uri(param);
            string host = uri.Host;

            if (host == "localhost")
            {
                host = "localhost:4200";
                link = link.Replace("/Angular/index.html", "");
            }

            link = $"{uri.Scheme}://{uri.Host}/{link}";
            return link;
        }

        public IQueryable<ExternalLinkList> GetIQueryableEntityList(IQueryable<ExternalLink> iQueryable) =>
            iQueryable.Select(a => new ExternalLinkList()
            {
                Id = a.Id,
                Ref = a.Ref,
                ExpirationDate = a.ExpirationDate,
                ActivityLog = a.ActivityLog,
                Params = a.Params
            });

        public string GetFormToken(AuthenticationToken authenticationToken)
        {
            if (authenticationToken == null)
                throw new ArgumentNullException(nameof(authenticationToken), "authenticationToken can not be null");
            string token = authenticationToken.Token;
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token), "token can not be null or empty");
            if (string.IsNullOrEmpty(authenticationToken.Params))
                throw new ArgumentNullException(nameof(authenticationToken.Params), "Params can not be null or empty");

            AuthenticationTokenParams parmas = JsonConvert.DeserializeObject<AuthenticationTokenParams>(authenticationToken.Params);
            ExternalLinkPM externalLinkPM = GetSinglePMByRef(parmas.Ref, authenticationToken.Tenant);

            if (externalLinkPM == null)
                throw new ArgumentNullException(nameof(externalLinkPM), $"externalLinkPM not found for ref: {parmas.Ref}, tenant: {authenticationToken.Tenant},");
            if (string.IsNullOrEmpty(parmas.Link))
                throw new ArgumentNullException(nameof(parmas.Link), "link can not be null or empty");

            return parmas.Link;
        }

        private T GetOrSetCache<T>(string cacheKey, Func<T> factory, TimeSpan? duration = null) where T : class
        {
            if (HttpContext.Current != null && CacheManager.CacheWrapper.Get(cacheKey) is T cached)
                return cached;

            T result = factory();
            if (result != null && HttpContext.Current != null)
            {
                CacheManager.CacheWrapper.Insert(
                    cacheKey,
                    result,
                    null,
                    DateTime.UtcNow.Add(duration ?? TimeSpan.FromHours(8)),
                    TimeSpan.Zero
                );
            }

            return result;
        }

        private class AuthenticationTokenParams
        {
            public string Link { get; set; }
            public string Ref { get; set; }
        }
    }
}