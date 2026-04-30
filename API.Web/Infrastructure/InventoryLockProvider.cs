using System.Collections.Concurrent;

namespace API.Web.Infrastructure
{
    public static class InventoryLockProvider
    {
        // Dictionary where Key = ProductId, Value = a lock object (Semaphore)
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _locks = new();

        public static SemaphoreSlim GetLock(int productId)
        {
            // Get the existing lock for this product, or create a new one if it's the first time
            return _locks.GetOrAdd(productId, _ => new SemaphoreSlim(1, 1));
        }
    }
}
