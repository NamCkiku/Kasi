using Kasi_Server.Utils.IdGenerators.Abstractions;
using Kasi_Server.Utils.IdGenerators.Ids;

namespace Kasi_Server.Utils.IdGenerators.Core
{
    public class TimestampIdGenerator : IStringGenerator
    {
        private readonly TimestampId _id = TimestampId.GetInstance();

        public static TimestampIdGenerator Current { get; } = new TimestampIdGenerator();

        public string Create()
        {
            return _id.GetId();
        }
    }
}