using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisQueue
{
    interface IQueue<T>
    {
        long Count { get; }
        Task<T> DequeueAsync();
        Task<T> PeekAsync();
        Task EnqueueAsync(T obj);
        Task ClearAsync();
    }
}
