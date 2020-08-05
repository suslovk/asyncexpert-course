using System;
using System.Threading;

namespace Synchronization.Core
{
    /*
     * Implement very simple wrapper around Mutex or Semaphore (remember both implement WaitHandle) to
     * provide a exclusive region created by `using` clause.
     *
     * Created region may be system-wide or not, depending on the constructor parameter.
     */
    public class NamedExclusiveScope : IDisposable
    {
        private readonly Mutex _mux;

        public NamedExclusiveScope(string name, bool isSystemWide)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var scopeName = isSystemWide ? @"Global\" + name : name;
            var mutex = new Mutex(true, scopeName, out var createdNew);
            if (!createdNew)
            {
                mutex.Close();
                throw new InvalidOperationException($"Unable to get a global lock {name}.");
            }

            _mux = mutex;
        }

        public void Dispose() => _mux.Close();
    }
}