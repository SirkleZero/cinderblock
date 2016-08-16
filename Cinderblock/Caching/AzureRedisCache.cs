using System;
using StackExchange.Redis;

namespace Cinderblock.Caching
{
    /// <summary>
    /// Provides an interfacace to the Azure Redis Cache service.
    /// </summary>
    public sealed class AzureRedisCache : Cache
    {
        private readonly ConnectionMultiplexer connection;

        /// <summary>
        /// Creates an instance of the <see cref="AzureRedisCache"/> object.
        /// </summary>
        /// <param name="connection">The <see cref="ConnectionMultiplexer"/> instance that will be used to 
        /// connect to and manage the cache.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="connection"/> is <langword name="null"/>.</para>
        /// </exception>
        public AzureRedisCache(ConnectionMultiplexer connection)
        {
            if(connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            this.connection = connection;
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("contoso5.redis.cache.windows.net,abortConnect=false,ssl=true,password=...");
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        /// <summary>
        /// Adds an item to the cache using the default sliding cache duration for the application.
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
        public override void Add<T>(string key, T item)
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
        public override void Add<T>(string key, T item, TimeSpan cacheDuration)
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

            var cache = this.connection.GetDatabase();
            //HttpRuntime.Cache.Add(key, item, null, System.Web.Caching.Cache.NoAbsoluteExpiration, cacheDuration, CacheItemPriority.Normal, null);
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
        public override void Add<T>(string key, T item, DateTime absoluteExpiration)
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

            var cache = this.connection.GetDatabase();
            //HttpRuntime.Cache.Add(key, item, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Checks to see if an item exists in the cache based on its unique key.
        /// </summary>
        /// <param name="key">The unique key of the item to locate.</param>
        /// <returns>true if the item exists in the cache; otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public override bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            var cache = this.connection.GetDatabase();
            return false;
            //return HttpRuntime.Cache[key] != null;
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The unique key of the item to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="key"/> is out of range.</para>
        /// </exception>
        public override void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            var cache = this.connection.GetDatabase();
            //HttpRuntime.Cache.Remove(key);
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
        public override bool TryGetValue<T>(string key, out T value)
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

                var cache = this.connection.GetDatabase();
                //value = (T)HttpRuntime.Cache[key];
                value = default(T); // just for scaffolding!
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }
    }
}
