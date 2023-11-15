using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace LinkLair.Services.Tests;

public class MockMemoryCache : IMemoryCache
{
    private object _value;

    public MockMemoryCache()
    {
    }

    public void Dispose()
    {
        _value = null;
    }

    public bool TryGetValue(object key, out object value)
    {
        value = _value;
        return true;
    }

    public ICacheEntry CreateEntry(object key)
    {
        return new Mock<ICacheEntry>().Object;
    }

    public void Remove(object key)
    {
        _value = null;
    }
}
