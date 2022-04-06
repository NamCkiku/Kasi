using System.Security.Principal;

namespace Kasi_Server.Utils.Helpers
{
    public static class Thread
    {
        public static void WaitAll(params Action[] actions)
        {
            if (actions == null)
            {
                return;
            }
            List<Task> tasks = new List<Task>();
            foreach (var action in actions)
            {
                tasks.Add(Task.Factory.StartNew(action, TaskCreationOptions.None));
            }
            Task.WaitAll(tasks.ToArray());
        }

        public static void ParallelExecute(params Action[] actions)
        {
            Parallel.Invoke(actions);
        }

        public static void ParallelExecute(Action action, int count = 1, ParallelOptions options = null)
        {
            if (options == null)
            {
                Parallel.For(0, count, i => action());
                return;
            }

            Parallel.For(0, count, options, i => action());
        }

        public static string ThreadId => System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();

        public static IPrincipal CurrentPrincipal
        {
            get { return System.Threading.Thread.CurrentPrincipal; }
            set { System.Threading.Thread.CurrentPrincipal = value; }
        }

        public static int MaxThreadNumberInThreadPool
        {
            get
            {
                int maxNumber;
                int ioNumber;
                ThreadPool.GetMaxThreads(out maxNumber, out ioNumber);
                return maxNumber;
            }
        }

        public static void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        public static void StartTask(Action handler)
        {
            Task.Factory.StartNew(handler);
        }

        public static void StartTask(Action<object> handler, object state)
        {
            Task.Factory.StartNew(handler, state);
        }
    }
}