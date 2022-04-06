namespace Kasi_Server.Utils.IdGenerators.Ids
{
    public class SnowflakeId
    {
        public const long TWEPOCH = 1288834974657L;

        private const int WORKER_ID_BITS = 5;

        private const int DATACENTER_ID_BITS = 5;

        private const int SEQUENCE_BITS = 12;

        private const long MAX_WORKER_ID = -1L ^ (-1L << WORKER_ID_BITS);

        private const long MAX_DATACENTER_ID = -1L ^ (-1L << DATACENTER_ID_BITS);

        private const long SEQUENCE_MASK = -1L ^ (-1L << SEQUENCE_BITS);

        private const int WORKER_ID_SHIFT = SEQUENCE_BITS;

        private const int DATACENTER_ID_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS;

        private const int TIMESTAMP_LEFT_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS + DATACENTER_ID_BITS;

        private long _sequence = 0L;

        private long _lastTimestamp = -1L;

        public long WorkerId { get; protected set; }

        public long DatacenterId { get; protected set; }

        public long Sequence
        {
            get => _sequence;
            internal set => _sequence = value;
        }

        public SnowflakeId(long workerId, long datacenterId, long sequence = 0L)
        {
            if (workerId > MAX_WORKER_ID || workerId < 0)
            {
                throw new ArgumentException($"worker Id 必须大于0，且不能大于 MaxWorkerId：{MAX_WORKER_ID}");
            }

            if (datacenterId > MAX_DATACENTER_ID || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id 必须大于0，且不能大于 MaxDatacenterId：{MAX_DATACENTER_ID}");
            }

            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;
        }

        private readonly object _lock = new object();

        public virtual long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception($"时间戳必须大于上一次生成ID的时间戳，拒绝为{_lastTimestamp - timestamp}毫秒生成id");
                }

                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & SEQUENCE_MASK;
                    if (_sequence == 0)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;
                return ((timestamp - TWEPOCH) << TIMESTAMP_LEFT_SHIFT) | (DatacenterId << DATACENTER_ID_SHIFT) | (WorkerId << WORKER_ID_SHIFT) | _sequence;
            }
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }

            return timestamp;
        }

        protected virtual long TimeGen()
        {
            return CurrentTimeMills();
        }

        public static Func<long> CurrentTimeFunc = InternalCurrentTimeMillis;

        public static long CurrentTimeMills()
        {
            return CurrentTimeFunc();
        }

        public static IDisposable StubCurrentTime(Func<long> func)
        {
            CurrentTimeFunc = func;
            return new DisposableAction(() => { CurrentTimeFunc = InternalCurrentTimeMillis; });
        }

        public static IDisposable StubCurrentTime(long millis)
        {
            CurrentTimeFunc = () => millis;
            return new DisposableAction(() => { CurrentTimeFunc = InternalCurrentTimeMillis; });
        }

        private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long InternalCurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
        }

        public class DisposableAction : IDisposable
        {
            private readonly Action _action;

            public DisposableAction(Action action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
            }

            public void Dispose()
            {
                _action();
            }
        }
    }
}