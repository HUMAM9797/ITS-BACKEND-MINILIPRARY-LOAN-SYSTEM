namespace LoanSystem.Application.Services
{
    public interface ISimpleCacheService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan duration);
        void Remove(string key);
    }

    public class SimpleCacheService : ISimpleCacheService
    {
        private readonly Dictionary<string, (object Value, DateTime Expiration)> _cache = new();
        private readonly object _lock = new();

        public T? Get<T>(string key)
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(key, out var item))
                {
                    if (item.Expiration > DateTime.Now)
                    {
                        return (T)item.Value;
                    }
                    else
                    {
                        _cache.Remove(key);
                    }
                }
                return default;
            }
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            lock (_lock)
            {
                _cache[key] = (value, DateTime.Now.Add(duration));
            }
        }

        public void Remove(string key)
        {
            lock (_lock)
            {
                _cache.Remove(key);
            }
        }
    }
}
