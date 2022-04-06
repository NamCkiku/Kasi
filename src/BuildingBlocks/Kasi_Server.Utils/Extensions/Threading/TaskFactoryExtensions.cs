namespace Kasi_Server.Utils.Extensions
{
    public static class TaskFactoryExtensions
    {
        public static Task StartDelayedTask(this TaskFactory factory, int millisecondsDelay, Action action)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (millisecondsDelay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (factory.CancellationToken.IsCancellationRequested)
            {
                return new Task(() => { }, factory.CancellationToken);
            }

            var tcs = new TaskCompletionSource<object>(factory.CreationOptions);
            var ctr = default(CancellationTokenRegistration);

            var ctr1 = ctr;
            var timer = new Timer(self =>
            {
                ctr1.Dispose();
                ((Timer)self).Dispose();
                tcs.TrySetResult(null);
            }, null, -1, -1);

            if (factory.CancellationToken.CanBeCanceled)
            {
                factory.CancellationToken.Register(() =>
                {
                    timer.Dispose();
                    tcs.TrySetCanceled();
                });
            }

            try
            {
                timer.Change(millisecondsDelay, Timeout.Infinite);
            }
            catch (ObjectDisposedException)
            {
            }

            return tcs.Task.ContinueWith(_ => action(), factory.CancellationToken,
                TaskContinuationOptions.OnlyOnRanToCompletion, factory.Scheduler ?? TaskScheduler.Current);
        }
    }
}