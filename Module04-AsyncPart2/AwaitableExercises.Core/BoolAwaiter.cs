using System;
using System.Runtime.CompilerServices;

namespace AwaitableExercises.Core
{
    public static class BoolExtensions
    {
        public static BoolAwaiter GetAwaiter(this bool @bool) => new BoolAwaiter(@bool);
    }

    public class BoolAwaiter : INotifyCompletion
    {
        private readonly bool _bool;

        public BoolAwaiter(bool @bool)
        {
            _bool = @bool;
        }

        public void OnCompleted(Action continuation)
        {
            continuation();
        }

        public bool GetResult() => _bool;
        public bool IsCompleted => true;
    }
}
