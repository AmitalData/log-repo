# Caching Infrastructure

## Overview
This folder contains the caching infrastructure for the application, supporting both Redis (production) and in-memory caching (development).

## Components

### 1. **ICacheService** (Interface)
- Defines the contract for all cache implementations
- Methods: `Get`, `Set`, `GetOrAdd`, `Remove`, `RemoveByPattern`, `Exists`, `Clear`

### 2. **RedisCacheService** (Production)
- Redis-based distributed caching
- Includes Application Insights telemetry
- Automatic failover to memory cache if Redis unavailable
- Singleton ConnectionMultiplexer pattern (best practice)

### 3. **MemoryCacheService** (Development)
- In-memory caching using `MemoryCache.Default`
- Thread-safe with locking mechanisms
- Pattern-based key removal support

### 4. **CacheServiceFactory** (Factory)
- Singleton factory pattern
- Auto-detects cache type from configuration
- Falls back to memory cache if Redis fails

## Configuration

### Web.config

```xml
<!-- Option 1: Using CacheType appSetting -->
<appSettings>
  <add key="CacheType" value="Redis" /> <!-- or "Memory" -->
</appSettings>

<!-- Option 2: Using connection string -->
<connectionStrings>
  <add name="Redis" connectionString="your-redis-connection-string" />
</connectionStrings>

<!-- OR using appSetting -->
<appSettings>
  <add key="RedisConnection" value="your-redis-connection-string" />
</appSettings>
```

### Azure Redis Connection String Format:
```
yourappname.redis.cache.windows.net:6380,password=yourkey,ssl=True,abortConnect=False
```

## Usage Examples

### Basic Usage

```csharp
using WebFreight.Web.Infrastructure.Caching;

// Get cache service instance
var cache = CacheServiceFactory.Instance;

// Simple get/set
cache.Set("mykey", myObject, TimeSpan.FromMinutes(30));
var cached = cache.Get<MyType>("mykey");

// Get or add (lazy loading)
var data = cache.GetOrAdd("mykey", () => {
    // This function only runs on cache miss
    return LoadDataFromDatabase();
}, TimeSpan.FromHours(1));

// Remove specific key
cache.Remove("mykey");

// Remove by pattern (e.g., all country cache for tenant 123)
cache.RemoveByPattern("Country*:123");
```

### In Controllers

```csharp
public class MyController : ApiController
{
    private readonly ICacheService _cache;

    public MyController()
    {
        _cache = CacheServiceFactory.Instance;
    }

    public HttpResponseMessage GetData()
    {
        int tenant = GetTenant();
        string cacheKey = $"MyData:{tenant}";

        var data = _cache.GetOrAdd(cacheKey, () => {
            // Load from database
            return repository.GetData(tenant);
        }, TimeSpan.FromMinutes(30));

        return Request.CreateResponse(HttpStatusCode.OK, data);
    }
}
```

## Cache Key Naming Convention

**Pattern:** `{EntityType}:{EntityId}:{Tenant}:{AdditionalParams}`

**Examples:**
```
Country:US:123              // Single country for tenant 123
Countries:123               // All countries for tenant 123
Port:USNYC:123              // Single port for tenant 123
Ports:Sea:123               // All sea ports for tenant 123
StatusCodes:Shipment:123    // Shipment status codes for tenant 123
UserPermissions:user456:123 // User permissions
```

## Cache Durations (Guidelines)

| Data Type | Duration | Reason |
|-----------|----------|--------|
| **Reference Data** | 24 hours | Rarely changes (countries, ports, codes) |
| **Master Data** | 4-8 hours | Changes infrequently (carriers, airlines) |
| **Configuration** | 1-4 hours | May change during workday |
| **Aggregates** | 5-15 min | Needs to be reasonably fresh |
| **User Data** | 30 min | May change frequently |
| **Transactional** | DO NOT CACHE | Changes constantly |

## Cache Invalidation

### Time-Based (Automatic)
- Expires after configured duration
- No manual intervention needed

### Event-Based (Manual)
```csharp
// After updating a country
public void UpdateCountry(CountryPM country)
{
    repository.Update(country);
    
    // Invalidate specific country
    cache.Remove($"Country:{country.Id}:{country.Tenant}");
    
    // Invalidate country list
    cache.Remove($"Countries:{country.Tenant}");
}
```

### Pattern-Based (Bulk)
```csharp
// Invalidate all country-related cache for a tenant
cache.RemoveByPattern($"Country*:{tenant}");
```

## Monitoring

### Application Insights Metrics

The `RedisCacheService` automatically tracks:
- **CacheHit**: Successful cache retrieval
- **CacheMiss**: Cache miss (data loaded from source)
- **CacheSet**: Data written to cache
- **CacheRemove**: Data removed from cache
- **Exceptions**: Any cache operation failures

### Kusto Queries

```kusto
// Cache hit rate
customMetrics
| where name in ("CacheHit", "CacheMiss")
| where timestamp > ago(24h)
| summarize hits = countif(name == "CacheHit"), 
            misses = countif(name == "CacheMiss") 
| extend hitRate = hits * 100.0 / (hits + misses)
```

```kusto
// Cache operations per minute
customMetrics
| where name startswith "Cache"
| where timestamp > ago(1h)
| summarize count() by bin(timestamp, 1m), name
| render timechart
```

## Performance Impact

### Before Caching:
```
GET /api/countries     50-80ms  (database query)
GET /api/ports        40-60ms  (database query)
GET /api/statuscodes  30-50ms  (database query)
```

### After Caching:
```
GET /api/countries      2-5ms  (cache hit) - 90% faster!
GET /api/ports         2-5ms  (cache hit) - 90% faster!
GET /api/statuscodes   2-5ms  (cache hit) - 90% faster!
```

## Best Practices

### ✅ DO:
- ✅ Use tenant-specific cache keys
- ✅ Set appropriate expiration times
- ✅ Handle cache failures gracefully
- ✅ Monitor cache hit rates
- ✅ Invalidate cache after updates
- ✅ Use `GetOrAdd` for lazy loading

### ❌ DON'T:
- ❌ Cache user-specific transactional data
- ❌ Use excessively long cache durations
- ❌ Cache data without tenant isolation
- ❌ Ignore cache exceptions (log them!)
- ❌ Forget to invalidate after updates
- ❌ Cache null values unnecessarily

## Troubleshooting

### Cache Not Working
1. Check configuration (connection string, cache type)
2. Verify Redis connectivity
3. Check Application Insights for cache exceptions
4. Ensure cache keys follow naming convention

### High Cache Miss Rate
1. Check if data changes frequently
2. Verify expiration times are appropriate
3. Look for cache invalidation patterns
4. Monitor key naming consistency

### Redis Connection Issues
- Service automatically falls back to memory cache
- Check Application Insights for connection exceptions
- Verify Redis connection string
- Check Azure Redis firewall rules

## Testing

### Unit Tests
```csharp
[Test]
public void Cache_ShouldStore_AndRetrieve()
{
    var cache = new MemoryCacheService();
    var testData = new MyObject { Id = "123" };
    
    cache.Set("testkey", testData, TimeSpan.FromMinutes(5));
    var retrieved = cache.Get<MyObject>("testkey");
    
    Assert.IsNotNull(retrieved);
    Assert.AreEqual("123", retrieved.Id);
}
```

### Integration Tests
- Test with real Redis connection
- Verify cache invalidation
- Test concurrent access
- Measure performance improvement

## Deployment Checklist

- ☐ Configure Redis connection string in Web.config
- ☐ Set CacheType in appSettings (or auto-detect)
- ☐ Add StackExchange.Redis NuGet package
- ☐ Test cache operations in staging
- ☐ Set up Application Insights monitoring
- ☐ Create Azure Redis Cache instance
- ☐ Configure firewall rules for Redis
- ☐ Test fallback to memory cache
- ☐ Monitor cache hit rates post-deployment

## Related Files

- `ICacheService.cs` - Interface definition
- `RedisCacheService.cs` - Redis implementation
- `MemoryCacheService.cs` - Memory implementation
- `CacheServiceFactory.cs` - Factory pattern
- `CachedReferenceDataController.cs` - Example usage

## Support

For questions or issues:
1. Check Application Insights logs
2. Review this README
3. Contact DevOps team for Redis issues
4. See Phase 2B documentation in `/refactor` folder

