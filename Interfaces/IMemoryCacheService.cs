using System;

namespace MovieBackend.Interaces
{
    public interface IMemoryCacheService
    {
        bool TryGetValue<T>(string key, out T value);
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        void Remove(string key);
    }
}
