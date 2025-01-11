using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{
    public interface ICacheWrapper
    {

        // Summary:
        //     Used in the absoluteExpiration parameter in an System.Web.Caching.Cache.Insert(System.String,System.Object)
        //     method call to indicate the item should never expire. This field is read-only.

        /*   static readonly DateTime NoAbsoluteExpiration;*/

        //
        // Summary:
        //     Used as the slidingExpiration parameter in an System.Web.Caching.Cache.Insert(System.String,System.Object)
        //     or System.Web.Caching.Cache.Add(System.String,System.Object,System.Web.Caching.CacheDependency,System.DateTime,System.TimeSpan,System.Web.Caching.CacheItemPriority,System.Web.Caching.CacheItemRemovedCallback)
        //     method call to disable sliding expirations. This field is read-only.


        /*    static readonly TimeSpan NoSlidingExpiration;*/

        // Summary:
        //     Initializes a new instance of the System.Web.Caching.Cache class.



        // Summary:
        //     Gets the number of items stored in the cache.
        //
        // Returns:
        //     The number of items stored in the cache.


        int Count { get; }


        //
        // Summary:
        //     Gets the percentage of physical memory that can be consumed by an application
        //     before ASP.NET starts removing items from the cache.
        //
        // Returns:
        //     The percentage of physical memory available to the application.


        long EffectivePercentagePhysicalMemoryLimit { get; }


        //
        // Summary:
        //     Gets the number of bytes available for the cache.
        //
        // Returns:
        //     The number of bytes available for the cache.


        long EffectivePrivateBytesLimit { get; }

        // Summary:
        //     Gets or sets the cache item at the specified key.
        //
        // Parameters:
        //   key:
        //     A System.String object that represents the key for the cache item.
        //
        // Returns:
        //     The specified cache item.


        // Summary:
        //     Adds the specified item to the System.Web.Caching.Cache object with dependencies,
        //     expiration and priority policies, and a delegate you can use to notify your
        //     application when the inserted item is removed from the Cache.
        //
        // Parameters:
        //   key:
        //     The cache key used to reference the item.
        //
        //   value:
        //     The item to be added to the cache.
        //
        //   dependencies:
        //     The file or cache key dependencies for the item. When any dependency changes,
        //     the object becomes invalid and is removed from the cache. If there are no
        //     dependencies, this parameter contains null.
        //
        //   absoluteExpiration:
        //     The time at which the added object expires and is removed from the cache.
        //     If you are using sliding expiration, the absoluteExpiration parameter must
        //     be System.Web.Caching.Cache.NoAbsoluteExpiration.
        //
        //   slidingExpiration:
        //     The interval between the time the added object was last accessed and the
        //     time at which that object expires. If this value is the equivalent of 20
        //     minutes, the object expires and is removed from the cache 20 minutes after
        //     it is last accessed. If you are using absolute expiration, the slidingExpiration
        //     parameter must be System.Web.Caching.Cache.NoSlidingExpiration.
        //
        //   priority:
        //     The relative cost of the object, as expressed by the System.Web.Caching.CacheItemPriority
        //     enumeration. The cache uses this value when it evicts objects; objects with
        //     a lower cost are removed from the cache before objects with a higher cost.
        //
        //   onRemoveCallback:
        //     A delegate that, if provided, is called when an object is removed from the
        //     cache. You can use this to notify applications when their objects are deleted
        //     from the cache.
        //
        // Returns:
        //     An object that represents the item that was added if the item was previously
        //     stored in the cache; otherwise, null.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The key or value parameter is set to null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The slidingExpiration parameter is set to less than TimeSpan.Zero or more
        //     than one year.
        //
        //   System.ArgumentException:
        //     The absoluteExpiration and slidingExpiration parameters are both set for
        //     the item you are trying to add to the Cache.
        // public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);


        //
        // Summary:
        //     Retrieves the specified item from the System.Web.Caching.Cache object.
        //
        // Parameters:
        //   key:
        //     The identifier for the cache item to retrieve.
        //
        // Returns:
        //     The retrieved cache item, or null if the key is not found.



        object Get(string key);



        //
        // Summary:
        //     Retrieves a dictionary enumerator used to iterate through the key settings
        //     and their values contained in the cache.
        //
        // Returns:
        //     An enumerator to iterate through the System.Web.Caching.Cache object.



        IDictionaryEnumerator GetEnumerator();



        //
        // Summary:
        //     Inserts an item into the System.Web.Caching.Cache object with a cache key
        //     to reference its location, using default values provided by the System.Web.Caching.CacheItemPriority
        //     enumeration.
        //
        // Parameters:
        //   key:
        //     The cache key used to reference the item.
        //
        //   value:
        //     The object to be inserted into the cache.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The key or value parameter is null.



        void Insert(string key, object value);



        //
        // Summary:
        //     Inserts an object into the System.Web.Caching.Cache that has file or key
        //     dependencies.
        //
        // Parameters:
        //   key:
        //     The cache key used to identify the item.
        //
        //   value:
        //     The object to be inserted in the cache.
        //
        //   dependencies:
        //     The file or cache key dependencies for the inserted object. When any dependency
        //     changes, the object becomes invalid and is removed from the cache. If there
        //     are no dependencies, this parameter contains null.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The key or value parameter is null.



        void Insert(string key, object value, CacheDependency dependencies);



        //
        // Summary:
        //     Inserts an object into the System.Web.Caching.Cache with dependencies and
        //     expiration policies.
        //
        // Parameters:
        //   key:
        //     The cache key used to reference the object.
        //
        //   value:
        //     The object to be inserted in the cache.
        //
        //   dependencies:
        //     The file or cache key dependencies for the inserted object. When any dependency
        //     changes, the object becomes invalid and is removed from the cache. If there
        //     are no dependencies, this parameter contains null.
        //
        //   absoluteExpiration:
        //     The time at which the inserted object expires and is removed from the cache.
        //     To avoid possible issues with local time such as changes from standard time
        //     to daylight saving time, use System.DateTime.UtcNow rather than System.DateTime.Now
        //     for this parameter value. If you are using absolute expiration, the slidingExpiration
        //     parameter must be System.Web.Caching.Cache.NoSlidingExpiration.
        //
        //   slidingExpiration:
        //     The interval between the time the inserted object is last accessed and the
        //     time at which that object expires. If this value is the equivalent of 20
        //     minutes, the object will expire and be removed from the cache 20 minutes
        //     after it was last accessed. If you are using sliding expiration, the absoluteExpiration
        //     parameter must be System.Web.Caching.Cache.NoAbsoluteExpiration.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The key or value parameter is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     You set the slidingExpiration parameter to less than TimeSpan.Zero or the
        //     equivalent of more than one year.
        //
        //   System.ArgumentException:
        //     The absoluteExpiration and slidingExpiration parameters are both set for
        //     the item you are trying to add to the Cache.



        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration);



        //
        // Summary:
        //     Inserts an object into the System.Web.Caching.Cache object together with
        //     dependencies, expiration policies, and a delegate that you can use to notify
        //     the application before the item is removed from the cache.
        //
        // Parameters:
        //   key:
        //     The cache key that is used to reference the object.
        //
        //   value:
        //     The object to insert into the cache.
        //
        //   dependencies:
        //     The file or cache key dependencies for the item. When any dependency changes,
        //     the object becomes invalid and is removed from the cache. If there are no
        //     dependencies, this parameter contains null.
        //
        //   absoluteExpiration:
        //     The time at which the inserted object expires and is removed from the cache.
        //     To avoid possible issues with local time such as changes from standard time
        //     to daylight saving time, use System.DateTime.UtcNow instead of System.DateTime.Now
        //     for this parameter value. If you are using absolute expiration, the slidingExpiration
        //     parameter must be set to System.Web.Caching.Cache.NoSlidingExpiration.
        //
        //   slidingExpiration:
        //     The interval between the time that the cached object was last accessed and
        //     the time at which that object expires. If this value is the equivalent of
        //     20 minutes, the object will expire and be removed from the cache 20 minutes
        //     after it was last accessed. If you are using sliding expiration, the absoluteExpiration
        //     parameter must be set to System.Web.Caching.Cache.NoAbsoluteExpiration.
        //
        //   onUpdateCallback:
        //     A delegate that will be called before the object is removed from the cache.
        //     You can use this to update the cached item and ensure that it is not removed
        //     from the cache.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The key, value, or onUpdateCallback parameter is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     You set the slidingExpiration parameter to less than TimeSpan.Zero or the
        //     equivalent of more than one year.
        //
        //   System.ArgumentException:
        //     The absoluteExpiration and slidingExpiration parameters are both set for
        //     the item you are trying to add to the Cache.-or-The dependencies parameter
        //     is null, and the absoluteExpiration parameter is set to System.Web.Caching.Cache.NoAbsoluteExpiration,
        //     and the slidingExpiration parameter is set to System.Web.Caching.Cache.NoSlidingExpiration.




        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback);




        //
        // Summary:
        //     Inserts an object into the System.Web.Caching.Cache object with dependencies,
        //     expiration and priority policies, and a delegate you can use to notify your
        //     application when the inserted item is removed from the Cache.
        //
        // Parameters:
        //   key:
        //     The cache key used to reference the object.
        //
        //   value:
        //     The object to be inserted in the cache.
        //
        //   dependencies:
        //     The file or cache key dependencies for the item. When any dependency changes,
        //     the object becomes invalid and is removed from the cache. If there are no
        //     dependencies, this parameter contains null.
        //
        //   absoluteExpiration:
        //     The time at which the inserted object expires and is removed from the cache.
        //     To avoid possible issues with local time such as changes from standard time
        //     to daylight saving time, use System.DateTime.UtcNow rather than System.DateTime.Now
        //     for this parameter value. If you are using absolute expiration, the slidingExpiration
        //     parameter must be System.Web.Caching.Cache.NoSlidingExpiration.
        //
        //   slidingExpiration:
        //     The interval between the time the inserted object was last accessed and the
        //     time at which that object expires. If this value is the equivalent of 20
        //     minutes, the object will expire and be removed from the cache 20 minutes
        //     after it was last accessed. If you are using sliding expiration, the absoluteExpiration
        //     parameter must be System.Web.Caching.Cache.NoAbsoluteExpiration.
        //
        //   priority:
        //     The cost of the object relative to other items stored in the cache, as expressed
        //     by the System.Web.Caching.CacheItemPriority enumeration. This value is used
        //     by the cache when it evicts objects; objects with a lower cost are removed
        //     from the cache before objects with a higher cost.
        //
        //   onRemoveCallback:
        //     A delegate that, if provided, will be called when an object is removed from
        //     the cache. You can use this to notify applications when their objects are
        //     deleted from the cache.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The key or value parameter is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     You set the slidingExpiration parameter to less than TimeSpan.Zero or the
        //     equivalent of more than one year.
        //
        //   System.ArgumentException:
        //     The absoluteExpiration and slidingExpiration parameters are both set for
        //     the item you are trying to add to the Cache.



        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);


        //
        // Summary:
        //     Removes the specified item from the application's System.Web.Caching.Cache
        //     object.
        //
        // Parameters:
        //   key:
        //     A System.String identifier for the cache item to remove.
        //
        // Returns:
        //     The item removed from the Cache. If the value in the key parameter is not
        //     found, returns null.

        void Insert<T>(int tenant, List<T> value);
        void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);
        void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback);
        void Insert<T>(int tenant, List<T> value, CacheDependency dependencies);
        void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        List<T> Get<T>(int tenant);
        object Invalidate<T>(int tenant);

        object Invalidate(string key);
        object Remove(string key);
    }

}
