using System;
using System.Web;
using System.Web.Caching;

namespace Cinderblock.Caching
{
    /// <summary>
    /// Represents a simplified interface to the intrinsic HttpRuntime.Cache system.
    /// </summary>
    public sealed class HttpCache : ICache
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="HttpCache"/> class.</para>
        /// </summary>
        public HttpCache() { }

        /// <summary>
        /// Adds an item to the cache using the default sliding cache duration of 30 minutes.
        /// </summary>
        /// <typeparam name="T">The generic <see cref="Type"/> of the object being added to the cache.</typeparam>
        /// <param name="key">The key that will be used to uniquely identify the cached item.</param>
        /// <param name="item">The item to add to the cache.</param>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="item"/> is <langword name="null"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public void Add<T>(string key, T item)
        {
            this.Add(key, item, TimeSpan.FromMinutes(30));
        }

        /// <summary>
        /// Adds an item to the cache using a sliding expiration.
        /// </summary>
        /// <typeparam name="T">The generic <see cref="Type"/> of the object being added to the cache.</typeparam>
        /// <param name="key">The key that will be used to uniquely identify the cached item.</param>
        /// <param name="item">The item to add to the cache.</param>
        /// <param name="cacheDuration">The <see cref="TimeSpan"/> that specifies how long an item will exist in the cache.</param>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="item"/> is <langword name="null"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// 	<para>-or-</para>
        /// 	<para>The argument <paramref name="cacheDuration"/> is out of range.</para>
        /// </exception>
        public void Add<T>(string key, T item, TimeSpan cacheDuration)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }
            if (cacheDuration.Equals(TimeSpan.Zero))
            {
                throw new ArgumentOutOfRangeException("cacheDuration");
            }

            HttpRuntime.Cache.Add(key, item, null, Cache.NoAbsoluteExpiration, cacheDuration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Adds an item to the cache using an absolute expiration.
        /// </summary>
        /// <typeparam name="T">The generic <see cref="Type"/> of the object being added to the cache.</typeparam>
        /// <param name="key">The key that will be used to uniquely identify the cached item.</param>
        /// <param name="item">The item to add to the cache.</param>
        /// <param name="absoluteExpiration">The <see cref="DateTime"/> that specifies the exact time that the item will be removed from the cache.</param>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="item"/> is <langword name="null"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// 	<para>-or-</para>
        /// 	<para>The argument <paramref name="absoluteExpiration"/> is out of range.</para>
        /// </exception>
        public void Add<T>(string key, T item, DateTime absoluteExpiration)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }
            if (absoluteExpiration < DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("absoluteExpiration");
            }

            HttpRuntime.Cache.Add(key, item, null, absoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The unique key of the item to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// Checks to see if an item exists in the cache based on its unique key.
        /// </summary>
        /// <param name="key">The unique key of the item to locate.</param>
        /// <returns>true if the item exists in the cache; otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            return HttpRuntime.Cache[key] != null;
        }

        /// <summary>
        /// Attempts to retrieve an item from the cache.  A return value indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="T">The generic <see cref="Type"/> of the object.</typeparam>
        /// <param name="key">The unique key that identifies the cached item.</param>
        /// <param name="value">When this method returns, contains the cached item if it is found, or the types default 
        /// value if it was not.  The retrieval will fail if the key does not exist in the cache, or the conversion 
        /// operation to the generic type fails.</param>
        /// <returns>true if the item exists in the cache and was located; otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public bool TryGetValue<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            try
            {
                if (!this.Exists(key))
                {
                    value = default(T);
                    return false;
                }
                value = (T)HttpRuntime.Cache[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a <see cref="Type"/> safe key that can be used to uniquely identify cached items.
        /// </summary>
        /// <typeparam name="T">The generic <see cref="Type"/> of the object.</typeparam>
        /// <param name="value">The object to generate a type safe key for.</param>
        /// <param name="key">The key that uniquely identifies the object.</param>
        /// <returns>A unique key based on the object type and object unique identifier.</returns>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="value"/> is <langword name="null"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public string CreateTypesafeKey<T>(T value, string key)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            return this.CreateTypesafeKey(value.GetType(), key);
        }

        /// <summary>
        /// Creates a <see cref="Type"/> safe key that can be used to uniquely identify cached items.
        /// </summary>
        /// <param name="t">The <see cref="Type"/> to generate a key for.</param>
        /// <param name="key">The key that uniquely identifies the object.</param>
        /// <returns>A unique key based on the object type and object unique identifier.</returns>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="t"/> is <langword name="null"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public string CreateTypesafeKey(Type t, string key)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            return string.Concat(t.ToString(), "_", key);
        }
    }
}
