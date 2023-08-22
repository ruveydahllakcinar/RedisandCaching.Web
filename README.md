

# Notes on Redis and Caching

## What is Redis?
Redis is an open source and fast data store and a key-value based memory database. It is often used for caching purposes.

## What is Caching?
Caching is the process of storing data that we use very often in a temporary storage area. This allows quick access to this data when needed.

## What are the Advantages of Caching?
Caching makes websites run faster and improves the user experience. It also reduces unnecessary bandwidth and database load by storing frequently used data in memory instead of repeatedly pulling it from the database. This improves application performance and supports scalability.

### What are the Types of Caching?
#### In-Memory Cache:
It is a type of cache where data is stored in the RAM of the server where the application is running. This provides fast access to data.

#### Distributed Cache:
A type of cache that enables data sharing between different servers or processing units. It is used in large-scale applications and systems.

### Which Data Should We Cache?
We should pay attention to the following criteria to cache data:
- Data that does not change frequently should be preferred.
- Frequently accessed data should be suitable for caching.

### What is On-Demand and Prepopulation Caching?
### On-Demand:
Data is cached only when there is a demand for it.

#### Prepopulation:
Used to cache data when the application starts. This allows fast access to the data before the request arrives.

### What is Cache Lifetime (Absolute Time and Sliding Time)?
#### Absolute Time:
Keeps the cache active for a certain lifetime.

#### Sliding Time:
Keeps the cache active for a certain period of time since the last access. If the cache is accessed again during this period, the time starts again. When using this method, it is important to consider it together with Absolute Time.

## Using In-Memory Cache:
The `"AddMemoryCache()"` service is used to provide a fast memory cache in Asp.net applications. This service is added to the "ConfigureServices" section at application startup. We should use the `"IMemoryCache"` interface in the constructor (ctor) section of any class for data caching.

## Redis Data Types and Using Redis with ASP.NET Core

## Redis Data Types

### String
- It stores a data as a single key.
- To add a new record, we use the `SET` method.
- To read the record, we use the `GET` method.
- With `GETRANGE` we can read a specific index range.
- With `INCR` we can increment a numeric value one by one.
- With `INCRBY` we can increment a specific value.
- With `DECR` we can decrease one by one.
- With `DECRBY` we can decrease a specific value.
- With `APPEND` we can add to the value of a key.

### Redis List
- With `LPUSH` we add data at the beginning of the list.
- With `RPUSH` we add data to the end of the list.
- With `LRANGE` we sort the list elements.
- With `LPOP` we delete the element at the beginning of the list.
- With `RPOP` we delete the element at the end of the list.
- With `LINDEX` we can retrieve data at a specific index.

### Redis Set
* The set data type is a data type in which we can store strings like the list data type. However, it has two important differences from list. ,

1. The data you keep in the directory must be unic. However, you can keep as much of the same data as you want in lists.

2. When a new element was added in lists, we could make the decision to add it to the beginning or end of the list. However, there is no such decision mechanism in Redis sets.

* Since the data will be binary, you can keep data in any way you want.
- With `SADD` we add new data to a set.
- With `SMEMBERS` we list the added data.
- With `SREM` we delete data.

### Redis Sorted Set
* Redis keeps the queues random, but if we want it to be regular, we need to keep it as a score.
- With `ZADD` we create an ordered set and add it by giving it a score.
- With `ZRANGE` we sort the elements of the ordered set.
```
ZRANGE books 0 -1 WITHSCORES
```
- With `ZREM` we can delete elements.

### Redis Hash
- With HMSET we can store data in a hash.
- With HGET we can retrieve the data in the hash.
- With HDEL we can delete the data.

## Using Redis with ASP.NET Core

*By implementing the `IDistributedCache` interface in the ctor of any contrroller, that is, passing it as DI, you can very quickly cash your data to Redis.

In order to use this interface, some configurations are required.

First, we need to install `Microsoft.Extensions.Caching.StackExchaneRedis` plugin in the project.

Then I am adding the AddStackExchangeRedisCache service in Program.cs.

Program.cs (.NET7)
````csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";

}); 
````
After specifying my port in this service and which service it is running on, IDistributedCache is implemented in the ctor of the class you want to use in the application.
````csharp
  private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
````
This is the way to go if you don't want to do a lot of processing on the Redis side or if you only want to use the Get and Set methods, that is, if you don't want to work with Redis's data types.
