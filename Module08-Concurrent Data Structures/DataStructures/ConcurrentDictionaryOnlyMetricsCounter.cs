using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DataStructures
{
    public class ConcurrentDictionaryOnlyMetricsCounter : IMetricsCounter
    {
        // Implement this class using only ConcurrentDictionary.
        // Use methods that change the state atomically to ensure that everything is counted properly.
        // This task does not require using any Interlocked, or Volatile methods. The only required API is provided by the ConcurrentDictionary
        private readonly ConcurrentDictionary<string, int> _dictionary = new ConcurrentDictionary<string, int>();
        
        public IEnumerator<KeyValuePair<string, int>> GetEnumerator() => _dictionary.GetEnumerator();

        public void Increment(string key) => _dictionary.AddOrUpdate(key, 1, (_, i) => i + 1);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}