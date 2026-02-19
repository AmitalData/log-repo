using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Infrastructure.Caching;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    /// <summary>
    /// Cached reference data endpoints for high-performance lookups
    /// </summary>
    public class CachedReferenceDataController : ApiController
    {
        private readonly ICacheService _cache;

        public CachedReferenceDataController()
        {
            _cache = CacheServiceFactory.Instance;
        }

        /// <summary>
        /// Gets all countries with 24-hour cache
        /// GET /api/v1/countries
        /// </summary>
        [HttpGet]
        [Route("api/v1/countries")]
        public HttpResponseMessage GetCountries()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                // Cache key with tenant isolation
                string cacheKey = $"Countries:{tenant}";

                // Get from cache or load from database
                var countries = _cache.GetOrAdd(cacheKey, () =>
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    CountryRepository repository = new CountryRepository(context);
                    CountryQuery query = new CountryQuery(repository);

                    return query.GetCountryPMs(tenant).ToList();
                }, TimeSpan.FromHours(24));

                return Request.CreateResponse(HttpStatusCode.OK, countries);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        /// <summary>
        /// Gets single country by ID with 24-hour cache
        /// GET /api/v1/countries/{id}
        /// </summary>
        [HttpGet]
        [Route("api/v1/countries/{id}")]
        public HttpResponseMessage GetCountryById(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                string cacheKey = $"Country:{id}:{tenant}";

                var country = _cache.GetOrAdd(cacheKey, () =>
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    CountryRepository repository = new CountryRepository(context);
                    CountryQuery query = new CountryQuery(repository);

                    return query.GetCountryPMById(id, tenant);
                }, TimeSpan.FromHours(24));

                if (country == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Country not found" });
                }

                return Request.CreateResponse(HttpStatusCode.OK, country);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        /// <summary>
        /// Gets all ports with 24-hour cache
        /// GET /api/v1/ports
        /// </summary>
        [HttpGet]
        [Route("api/v1/ports")]
        public HttpResponseMessage GetPorts(string type = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                // Include type in cache key if specified
                string cacheKey = string.IsNullOrEmpty(type)
                    ? $"Ports:All:{tenant}"
                    : $"Ports:{type}:{tenant}";

                var ports = _cache.GetOrAdd(cacheKey, () =>
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    PortRepository repository = new PortRepository(context);
                    PortQuery query = new PortQuery(repository);

                    var allPorts = query.GetPortPMs(tenant).ToList();

                    // Filter by type if specified
                    if (!string.IsNullOrEmpty(type))
                    {
                        allPorts = allPorts.Where(p => p.PortTypeCode == type).ToList();
                    }

                    return allPorts;
                }, TimeSpan.FromHours(24));

                return Request.CreateResponse(HttpStatusCode.OK, ports);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        /// <summary>
        /// Gets all status codes with 4-hour cache
        /// GET /api/v1/statuscodes/{category}
        /// </summary>
        [HttpGet]
        [Route("api/v1/statuscodes/{category}")]
        public HttpResponseMessage GetStatusCodes(string category)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                string cacheKey = $"StatusCodes:{category}:{tenant}";

                var statusCodes = _cache.GetOrAdd(cacheKey, () =>
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    EntityStatusRepository repository = new EntityStatusRepository(context);
                    EntityStatusQuery query = new EntityStatusQuery(repository);

                    return query.GetEntityStatusPMsByCategory(category, tenant).ToList();
                }, TimeSpan.FromHours(4));

                return Request.CreateResponse(HttpStatusCode.OK, statusCodes);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        /// <summary>
        /// Invalidates cache for a specific entity type
        /// POST /api/v1/cache/invalidate/{entityType}
        /// </summary>
        [HttpPost]
        [Route("api/v1/cache/invalidate/{entityType}")]
        public HttpResponseMessage InvalidateCache(string entityType)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                // Invalidate all cache entries for this entity type and tenant
                string pattern = $"{entityType}*:{tenant}";
                _cache.RemoveByPattern(pattern);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    message = $"Cache invalidated for {entityType}",
                    pattern = pattern
                });
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        /// <summary>
        /// Gets cache statistics (for monitoring)
        /// GET /api/v1/cache/stats
        /// </summary>
        [HttpGet]
        [Route("api/v1/cache/stats")]
        public HttpResponseMessage GetCacheStats()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                // Check which cache implementation is being used
                bool isRedis = _cache is RedisCacheService;

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    cacheType = isRedis ? "Redis" : "Memory",
                    message = "Cache is operational",
                    tenant = tenant,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}

