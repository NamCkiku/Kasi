using Kasi_Server.Utils.Extensions;

namespace Kasi_Server.Utils.IdGenerators.Ids
{
    public class TimestampId
    {
        private long _lastTimestamp;
        private long _sequence;
        private readonly DateTime? _initialDateTime;
        private static TimestampId _timestampId;
        private const int MAX_END_NUMBER = 9999;

        private TimestampId(DateTime? initialDateTime)
        {
            _initialDateTime = initialDateTime;
        }

        public static TimestampId GetInstance(DateTime? initialDateTime = null)
        {
            if (initialDateTime.IsNull())
            {
                Interlocked.CompareExchange(ref _timestampId, new TimestampId(initialDateTime), null);
            }
            return _timestampId;
        }

        protected DateTime InitialDateTime
        {
            get
            {
                if (_initialDateTime == null || _initialDateTime.Value == DateTime.MinValue)
                {
                    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                }
                return _initialDateTime.Value;
            }
        }

        public string GetId()
        {
            long temp;
            var timestamp = GetUniqueTimeStamp(_lastTimestamp, out temp);
            return $"{timestamp}{Fill(temp)}";
        }

        private string Fill(long temp)
        {
            var num = temp.ToString();
            IList<char> chars = new List<char>();
            for (int i = 0; i < MAX_END_NUMBER.ToString().Length - num.Length; i++)
            {
                chars.Add('0');
            }
            return new string(chars.ToArray()) + num;
        }

        public long GetUniqueTimeStamp(long lastTimeStamp, out long temp)
        {
            lock (this)
            {
                temp = 1;
                var timeStamp = GetTimeStamp();
                if (timeStamp == _lastTimestamp)
                {
                    _sequence = _sequence + 1;
                    temp = _sequence;
                    if (temp >= MAX_END_NUMBER)
                    {
                        timeStamp = GetTimeStamp();
                        _lastTimestamp = timeStamp;
                        temp = _sequence = 1;
                    }
                }
                else
                {
                    _sequence = 1;
                    _lastTimestamp = timeStamp;
                }
                return timeStamp;
            }
        }

        private long GetTimeStamp()
        {
            if (InitialDateTime >= DateTime.Now)
            {
                throw new Exception("初始化时间比当前时间还大，不合理");
            }
            var ts = DateTime.UtcNow - InitialDateTime;
            return (long)ts.TotalMilliseconds;
        }
    }
}