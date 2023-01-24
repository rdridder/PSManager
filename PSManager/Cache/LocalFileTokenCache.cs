using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

// Taken from https://sergeytihon.com/tag/authorization/
// Very nice solution to prevent errors after restarting the server
namespace PSManager.Cache
{
    public class LocalFileTokenCache : IDistributedCache
    {
        private class FileTransaction : IDisposable
        {
            public FileTransaction(string fileName = "cache.json")
            {
                var root = Path.GetDirectoryName(GetType().Assembly.Location);
                _fileName = Path.Combine(root, fileName);

                if (File.Exists(_fileName))
                {
                    var str = File.ReadAllText(_fileName);
                    Dict = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(str);
                }

                Dict ??= new Dictionary<string, byte[]>();
            }

            private readonly string _fileName;
            public Dictionary<string, byte[]> Dict { get; }

            public void Dispose()
            {
                var str = JsonSerializer.Serialize(Dict);
                File.WriteAllText(_fileName, str);
            }
        }

        public byte[] Get(string key)
        {
            using var cache = new FileTransaction();
            return cache.Dict.TryGetValue(key, out var value) ? cache.Dict[key] : null;
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            return Task.FromResult(Get(key));
        }

        public void Refresh(string key)
        {
            // Don't process anything
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            Refresh(key);
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            using var cache = new FileTransaction();
            cache.Dict.Remove(key, out _);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            using var cache = new FileTransaction();
            cache.Dict[key] = value;
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            Set(key, value, options);
            return Task.CompletedTask;
        }
    }
}
