using System;

namespace WebFreight.Web.Infrastructure.Caching
{
    /// <summary>
    /// Interface for cache service to support Redis and in-memory caching
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets a cached value by key
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached value or default(T) if not found</returns>
        T Get<T>(string key);

        /// <summary>
        /// Sets a value in cache with expiration
        /// </summary>
        /// <typeparam name="T">Type of object to cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="expiration">Time until expiration</param>
        void Set<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Gets from cache or adds if not found (lazy loading pattern)
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Function to generate value if cache miss</param>
        /// <param name="expiration">Time until expiration</param>
        /// <returns>Cached or newly generated value</returns>
        T GetOrAdd<T>(string key, Func<T> factory, TimeSpan expiration);

        /// <summary>
        /// Removes a specific key from cache
        /// </summary>
        /// <param name="key">Cache key to remove</param>
        void Remove(string key);

        /// <summary>
        /// Removes all keys matching a pattern (e.g., "Country:*:123")
        /// </summary>
        /// <param name="pattern">Pattern to match keys</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Checks if a key exists in cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if key exists</returns>
        bool Exists(string key);

        /// <summary>
        /// Clears all cache (use with caution!)
        /// </summary>
        void Clear();
    }
}

